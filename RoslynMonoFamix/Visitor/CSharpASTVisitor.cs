﻿using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using Fame;
using FAMIX;
using Microsoft.CodeAnalysis;
using RoslynMonoFamix.ModelBuilder;
using Net;
using System.Linq.Expressions;
using System.Linq;

public class CSharpASTVisitor : CSharpSyntaxWalker {

    private InCSharpImporter importer;
    private Method currentMethod;
    private System.Collections.Stack currentTypeStack;
    private Net.Property currentProperty;

    public CSharpASTVisitor(InCSharpImporter importer) {

        this.importer = importer;
        currentTypeStack = new System.Collections.Stack();
    }

    public override void VisitClassDeclaration(ClassDeclarationSyntax node) {
        var typeSymbol = importer.model.GetDeclaredSymbol(node);

        FAMIX.Type type = type = importer.EnsureType(typeSymbol);
        var superType = typeSymbol.BaseType;

        if (superType != null) {
            FAMIX.Type baseType = null;
            if (superType.DeclaringSyntaxReferences.Length == 0)
                baseType = importer.EnsureBinaryType(superType);
            else
                baseType = importer.EnsureType(typeSymbol.BaseType);
            Inheritance inheritance = importer.CreateNewAssociation<Inheritance>(typeof(FAMIX.Inheritance).FullName);
            inheritance.subclass = type;
            inheritance.superclass = baseType;
            baseType.AddSubInheritance(inheritance);
            type.AddSuperInheritance(inheritance);
        }

        //type.name = node.Identifier.ToString();
        AddSuperInterfaces(typeSymbol, type);
        AddAnnotations(typeSymbol, type);
        AddParameterTypes(typeSymbol, type);
        currentTypeStack.Push(type);
        importer.CreateSourceAnchor(type, node);
        type.isStub = false;
        if (type.container != null)
            type.container.isStub = false;
        base.VisitClassDeclaration(node);
        ComputeFanout(currentTypeStack.Peek() as FAMIX.Type);
        currentTypeStack.Pop();
    }


    private void ComputeFanout(FAMIX.Type currentType) {
        //  currentType.Methods.ForEach(method=>method.OutgoingInvocations.ForEach())
        var types = currentType.Methods.SelectMany(
            method => method.OutgoingInvocations.SelectMany(
                invocation => invocation.Candidates.SelectMany(called => new FAMIX.Type[] { (called as Method).parentType }))).Distinct<FAMIX.Type>();
        var fanout = types.Sum<FAMIX.Type>(type => type == currentType ? 0 : 1);
        currentType.fanOut = fanout;
    }

    private void AddParameterTypes(INamedTypeSymbol typeSymbol, FAMIX.Type type) {
        foreach (var typeParameter in typeSymbol.TypeParameters) {
            (type as ParameterizableClass).TypeParameters.Add(importer.EnsureType(typeParameter) as FAMIX.ParameterType);
        }
    }

    public override void VisitStructDeclaration(StructDeclarationSyntax node) {
        var typeSymbol = importer.model.GetDeclaredSymbol(node);
        FAMIX.Type type = importer.EnsureType(typeSymbol);

        currentTypeStack.Push(type);
        importer.CreateSourceAnchor(type, node);
        type.isStub = false;
        base.VisitStructDeclaration(node);
        currentTypeStack.Pop();
    }

    public override void VisitEnumDeclaration(EnumDeclarationSyntax node) {
        var typeSymbol = importer.model.GetDeclaredSymbol(node);
        FAMIX.Type type = importer.EnsureType(typeSymbol);

        currentTypeStack.Push(type);
        importer.CreateSourceAnchor(type, node);
        type.isStub = false;
        base.VisitEnumDeclaration(node);
        currentTypeStack.Pop();
    }

    public override void VisitEnumMemberDeclaration(EnumMemberDeclarationSyntax node) {
        string attributeName = node.Identifier.ToString();
        var symbol = importer.model.GetDeclaredSymbol(node);
        FAMIX.EnumValue anEnumValue = importer.EnsureAttribute(symbol) as FAMIX.EnumValue;
        importer.CreateSourceAnchor(anEnumValue, node);
        if (currentTypeStack.Peek() is FAMIX.Enum) {
            anEnumValue.parentEnum = currentTypeStack.Peek() as FAMIX.Enum;
            anEnumValue.parentEnum.AddValue(anEnumValue);
            anEnumValue.isStub = false;
        }

    }

    //TODO add AnnotationInstanceAttribute link to AnnotationAttribute is not implemented
    //because in C# there are AnnotationAttributes that are initilized via constructors
    private void AddAnnotations(INamedTypeSymbol typeSymbol, FAMIX.NamedEntity type) {
        var attributes = typeSymbol.GetAttributes();
        foreach (var attr in attributes) {
            try {
                FAMIX.AnnotationInstance annotationInstance = importer.New<FAMIX.AnnotationInstance>();

                FAMIX.AnnotationType annonType = (FAMIX.AnnotationType)importer.EnsureType(attr.AttributeClass);
                annotationInstance.annotatedEntity = type;
                annotationInstance.annotationType = annonType;

                foreach (var constrArgument in attr.ConstructorArguments) {
                    AnnotationInstanceAttribute annotationInstanceAttribute = importer.New<FAMIX.AnnotationInstanceAttribute>();
                    annotationInstanceAttribute.value = constrArgument.Value.ToString();
                    annotationInstance.AddAttribute(annotationInstanceAttribute);
                }
                foreach (var namedArgument in attr.NamedArguments) {
                    AnnotationInstanceAttribute annotationInstanceAttribute = importer.New<FAMIX.AnnotationInstanceAttribute>();
                    annotationInstanceAttribute.value = namedArgument.Value.ToString();
                    annotationInstance.AddAttribute(annotationInstanceAttribute);
                }
            } catch (InvalidCastException c) {
                Console.WriteLine(c.Message);
            }
        }
    }

    private void AddSuperInterfaces(INamedTypeSymbol typeSymbol, FAMIX.Type type) {
        foreach (var inter in typeSymbol.Interfaces) {
            FAMIX.Type fInterface = (FAMIX.Type)importer.EnsureType(inter);
            if (fInterface is FAMIX.Class) (fInterface as FAMIX.Class).isInterface = true;
            Inheritance inheritance = importer.CreateNewAssociation<Inheritance>("FAMIX.Inheritance");
            inheritance.subclass = type;
            inheritance.superclass = fInterface;
            fInterface.AddSubInheritance(inheritance);
            type.AddSuperInheritance(inheritance);
        }
    }

    public override void VisitInterfaceDeclaration(InterfaceDeclarationSyntax node) {
        var typeSymbol = importer.model.GetDeclaredSymbol(node);
        FAMIX.Class type = (FAMIX.Class)importer.EnsureType(typeSymbol);
        type.isInterface = true;
        //type.name = node.Identifier.ToString();
        AddSuperInterfaces(typeSymbol, type);

        currentTypeStack.Push(type);
        importer.CreateSourceAnchor(type, node);
        type.isStub = false;
        base.VisitInterfaceDeclaration(node);
        currentTypeStack.Pop();
    }

    public override void VisitConstructorDeclaration(ConstructorDeclarationSyntax node) {
        string methodName = node.Identifier.ToString();

        AddMethod(node, methodName);

        if (currentMethod != null) currentMethod.isConstructor = true;

        base.VisitConstructorDeclaration(node);
        currentMethod = null;
    }

    public override void VisitDestructorDeclaration(DestructorDeclarationSyntax node) {
        string methodName = node.Identifier.ToString();

        AddMethod(node, methodName);

        if (currentMethod != null) currentMethod.isConstructor = true;

        base.VisitDestructorDeclaration(node);
        currentMethod = null;
    }

    public override void VisitAccessorDeclaration(AccessorDeclarationSyntax node) {
        if (currentProperty != null) {
            var methodSymbol = importer.model.GetDeclaredSymbol(node);

            Net.PropertyAccessor aMethod = importer.EnsureMethod(methodSymbol) as Net.PropertyAccessor;
            if (methodSymbol.MethodKind == MethodKind.PropertyGet)
                currentProperty.getter = aMethod as Net.PropertyAccessor;
            else
                currentProperty.setter = aMethod as Net.PropertyAccessor;

            if (currentTypeStack.Count > 0) {
                aMethod.property = currentProperty;
                aMethod.parentType = currentTypeStack.Peek() as FAMIX.Type;
                aMethod.parentType.AddMethod(aMethod);
            }

            currentMethod = aMethod;

            var returnType = importer.EnsureType(methodSymbol.ReturnType);
            currentMethod.declaredType = returnType;
            importer.CreateSourceAnchor(aMethod, node);
            currentMethod.isStub = false;
        }
        base.VisitAccessorDeclaration(node);
    }


    public override void VisitMethodDeclaration(MethodDeclarationSyntax node) {
        string methodName = node.Identifier.ToString();
        AddMethod(node, methodName);
        base.VisitMethodDeclaration(node);
        currentMethod = null;
    }

    private Method AddMethod(BaseMethodDeclarationSyntax node, string name) {
        if (currentTypeStack.Count > 0) {
            var methodSymbol = importer.model.GetDeclaredSymbol(node);
            Method aMethod = importer.EnsureMethod(methodSymbol);
            aMethod.name = name;
            aMethod.parentType = importer.EnsureType(methodSymbol.ContainingType);
            aMethod.parentType.AddMethod(aMethod);
            currentMethod = aMethod;

            var returnType = importer.EnsureType(methodSymbol.ReturnType);
            currentMethod.declaredType = returnType;
            importer.CreateSourceAnchor(aMethod, node);
            currentMethod.isStub = false;
        }
        return currentMethod;
    }

    public override void VisitPropertyDeclaration(PropertyDeclarationSyntax node) {
        string propertyName = node.Identifier.ToString();
        var prop = AddProperty(node, propertyName);
        if (prop is Net.Property) currentProperty = prop as Net.Property;
        base.VisitPropertyDeclaration(node);
        currentProperty = null;
    }

    private FAMIX.Attribute AddProperty(BasePropertyDeclarationSyntax node, String propertyName) {
        ISymbol symbol = importer.model.GetDeclaredSymbol(node);
        FAMIX.Attribute propertyAttribute = null;

        if (currentTypeStack.Count > 0) {
            propertyAttribute = importer.EnsureAttribute(symbol) as FAMIX.Attribute;
            propertyAttribute.parentType = importer.EnsureType(symbol.ContainingType);
            propertyAttribute.parentType.AddAttribute(propertyAttribute);

            propertyAttribute.isStub = false;
            importer.CreateSourceAnchor(propertyAttribute, node);
        }
        return propertyAttribute;
    }

    public override void VisitEventDeclaration(EventDeclarationSyntax node) {
        string propertyName = node.Identifier.ToString();
        if (currentTypeStack.Count > 0) {
            var eventSymbol = importer.model.GetDeclaredSymbol(node) as IEventSymbol;
            var Event = importer.EnsureEvent(eventSymbol);
            Event.name = propertyName;
            Event.parentType = importer.EnsureType(eventSymbol.ContainingType);
            Event.parentType.AddMethod(Event);
            currentMethod = Event;
            importer.CreateSourceAnchor(Event, node);
            currentMethod.isStub = false;
        }
        base.VisitEventDeclaration(node);
    }

    public override void VisitEventFieldDeclaration(EventFieldDeclarationSyntax node) {
        foreach (var variable in node.Declaration.Variables) {
            string attributeName = variable.Identifier.ToString();

            var symbol = importer.model.GetDeclaredSymbol(variable) as IEventSymbol;

            if (currentTypeStack.Count > 0) {
                var anEvent = importer.EnsureEvent(symbol) as Net.Event;
                anEvent.parentType = importer.EnsureType(symbol.ContainingType);
                anEvent.parentType.AddMethod(anEvent);
                importer.CreateSourceAnchor(anEvent, node);
                anEvent.isStub = false;
            }

        }
        base.VisitEventFieldDeclaration(node);
    }

    public override void VisitDelegateDeclaration(DelegateDeclarationSyntax node) {
        var typeSymbol = importer.model.GetDeclaredSymbol(node);
        importer.EnsureType(typeSymbol);
        base.VisitDelegateDeclaration(node);
    }

    public override void VisitFieldDeclaration(FieldDeclarationSyntax node) {
        AddField(node);
        base.VisitFieldDeclaration(node);
    }

    private void AddField(BaseFieldDeclarationSyntax node) {
        foreach (var variable in node.Declaration.Variables) {
            string attributeName = variable.Identifier.ToString();
            var returnTypeSymbol = importer.model.GetDeclaredSymbol(node.Declaration.Type);
            var symbol = importer.model.GetDeclaredSymbol(variable);
            if (symbol is IFieldSymbol || symbol is IEventSymbol) {
                if (currentTypeStack.Count > 0) {
                    FAMIX.Attribute anAttribute = importer.EnsureAttribute(symbol) as FAMIX.Attribute;
                    anAttribute.parentType = importer.EnsureType(symbol.ContainingType);
                    anAttribute.parentType.AddAttribute(anAttribute);
                    importer.CreateSourceAnchor(anAttribute, node);
                    anAttribute.isStub = false;
                }
            }
        }
    }

    public override void VisitCatchDeclaration(CatchDeclarationSyntax node) {
        ISymbol typeSymbol = importer.model.GetTypeInfo(node.Type).Type;
        var exceptionClass = (FAMIX.Class)importer.EnsureType(typeSymbol);
        FAMIX.CaughtException caughtException = importer.New<FAMIX.CaughtException>();
        caughtException.definingMethod = currentMethod;
        caughtException.exceptionClass = exceptionClass;
        base.VisitCatchDeclaration(node);
    }

    public override void VisitThrowStatement(ThrowStatementSyntax node) {
        if (node.Expression != null) {
            var symbolInfo = importer.model.GetTypeInfo(node.Expression).Type;

            var exceptionClass = (FAMIX.Class)importer.EnsureType(symbolInfo);
            FAMIX.ThrownException thrownException = importer.New<FAMIX.ThrownException>();
            thrownException.definingMethod = currentMethod;
            thrownException.exceptionClass = exceptionClass;
        }
        base.VisitThrowStatement(node);
    }

    private T GetSymbol<T>(SyntaxNode node) {
        var symbolInfo = importer.model.GetSymbolInfo(node).Symbol;
        if (symbolInfo is T methodSymbol)
            return methodSymbol;
        return default(T);
    }

    /**
     * Visit an identifier, it can be anything, a method reference, a field, a local variable, etc.
     * We need to find out what it is and if it is located in a method and then make the appropriate connection.
     */
    public override void VisitIdentifierName(IdentifierNameSyntax node) {
        try {
            if (currentMethod != null) {
                FAMIX.NamedEntity referencedEntity = FindReferencedEntity(node);
                if (referencedEntity is FAMIX.Attribute) AddAttributeAccess(node, currentMethod, referencedEntity as FAMIX.Attribute);
            }
        } catch (InvalidCastException e) {
            Console.WriteLine(e.Message);
        }
        base.VisitIdentifierName(node);
    }

    public override void VisitInvocationExpression(InvocationExpressionSyntax node) {
        try {
            if (currentMethod != null) {
                FAMIX.NamedEntity referencedEntity = FindReferencedEntity(node.Expression);
                if (referencedEntity is FAMIX.Method)
                    AddMethodCall(node, currentMethod, referencedEntity as FAMIX.Method);
            }
        } catch (InvalidCastException e) {
            Console.WriteLine(e.Message);
        }
        base.VisitInvocationExpression(node);
    }


    public override void VisitObjectCreationExpression(ObjectCreationExpressionSyntax node) {
        try {
            if (currentMethod != null) {
                FAMIX.NamedEntity referencedEntity = FindReferencedEntity(node);
                if (referencedEntity is FAMIX.Method)
                    AddMethodCall(node, currentMethod, referencedEntity as FAMIX.Method);
            }
        } catch (InvalidCastException e) {
            Console.WriteLine(e.Message);
        }
        base.VisitObjectCreationExpression(node);
    }

    private void AddAttributeAccess(SyntaxNode node, Method clientMethod, FAMIX.Attribute attribute) {
        Access access = importer.CreateNewAssociation<Access>("FAMIX.Access");
        access.accessor = currentMethod;
        access.variable = attribute;
        clientMethod.AddAccesse(access);
        attribute.AddIncomingAccess(access);
        importer.CreateSourceAnchor(access, node);
    }

    private void AddMethodCall(SyntaxNode node, Method clientMethod, Method referencedEntity) {
        Invocation invocation = importer.CreateNewAssociation<Invocation>("FAMIX.Invocation");
        invocation.sender = clientMethod;
        invocation.AddCandidate(referencedEntity);
        invocation.signature = node.Span.ToString();
        //invocation.receiver = referencedEntity;
        clientMethod.AddOutgoingInvocation(invocation);
        referencedEntity.AddIncomingInvocation(invocation);
        importer.CreateSourceAnchor(invocation, node);
    }

    private NamedEntity FindReferencedEntity(ExpressionSyntax node) {
        var symbol = importer.model.GetSymbolInfo(node).Symbol;
        if (symbol is IMethodSymbol)
            return importer.EnsureMethod(symbol as IMethodSymbol);
        if (symbol is IEventSymbol)
            return importer.EnsureEvent(symbol as IEventSymbol);
        if (symbol is IFieldSymbol)
            return importer.EnsureAttribute(symbol);
        if (symbol is IPropertySymbol)
            return importer.EnsureAttribute(symbol);
        return null;
    }

    public override void VisitAssignmentExpression(AssignmentExpressionSyntax node) {
        var MaybeEventSymbol = importer.model.GetSymbolInfo(node.Left).Symbol;
        if (MaybeEventSymbol != null && MaybeEventSymbol.Kind == SymbolKind.Event) {
            var isMethod = importer.model.GetSymbolInfo(node.Right).Symbol;
            if (isMethod != null && isMethod.Kind == SymbolKind.Method) {
                Event cSharpEvent = importer.EnsureEvent(MaybeEventSymbol as IEventSymbol) as Event;
                var handlerMethod = importer.EnsureMethod(isMethod as IMethodSymbol);
                AddMethodCall(node, cSharpEvent, handlerMethod);
            }
        }
        base.VisitAssignmentExpression(node);
    }



    public override void VisitWhileStatement(WhileStatementSyntax node) {
        if (currentMethod != null) currentMethod.cyclomaticComplexity++;
        base.VisitWhileStatement(node);
    }

    public override void VisitIfStatement(IfStatementSyntax node) {
        if (currentMethod != null) currentMethod.cyclomaticComplexity++;
        base.VisitIfStatement(node);
    }
    public override void VisitDoStatement(DoStatementSyntax node) {
        if (currentMethod != null) currentMethod.cyclomaticComplexity++;
        base.VisitDoStatement(node);
    }
    public override void VisitCaseSwitchLabel(CaseSwitchLabelSyntax node) {
        if (currentMethod != null) currentMethod.cyclomaticComplexity++;
        base.VisitCaseSwitchLabel(node);
    }
    public override void VisitCasePatternSwitchLabel(CasePatternSwitchLabelSyntax node) {
        if (currentMethod != null) currentMethod.cyclomaticComplexity++;
        base.VisitCasePatternSwitchLabel(node);
    }
    public override void VisitReturnStatement(ReturnStatementSyntax node) {
        if (currentMethod != null) currentMethod.cyclomaticComplexity++;
        base.VisitReturnStatement(node);
    }
}