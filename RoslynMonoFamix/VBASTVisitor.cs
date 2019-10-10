using Microsoft.CodeAnalysis.VisualBasic;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using System;
using Fame;
using FAMIX;
using Microsoft.CodeAnalysis;
using RoslynMonoFamix.InCSharp;
using CSharp;
using System.Linq.Expressions;
using System.Linq;

public class VBASTVisitor : VisualBasicSyntaxWalker {
    private SemanticModel semanticModel;
    private InCSharpImporter importer;
    private Method currentMethod;
    private System.Collections.Stack currentTypeStack;
    private CSharp.CSharpProperty currentProperty;

    public VBASTVisitor(SemanticModel semanticModel, InCSharpImporter importer) {
        this.semanticModel = semanticModel;
        this.importer = importer;
        currentTypeStack = new System.Collections.Stack();
    }


    /************************************************************************************************
     *  Class Entities management   
     *    - In this following Visit methods we deal with the class and inside class elements    
     ************************************************************************************************/


    public override void VisitClassBlock(ClassBlockSyntax node) {
        var typeSymbol = semanticModel.GetDeclaredSymbol(node.ClassStatement);

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
        base.VisitClassBlock(node);
        ComputeFanout(currentTypeStack.Peek() as FAMIX.Type);
        currentTypeStack.Pop();
    }


    public override void VisitStructureBlock(StructureBlockSyntax node) {
        var typeSymbol = semanticModel.GetDeclaredSymbol(node);
        FAMIX.Type type = importer.EnsureType(typeSymbol);

        currentTypeStack.Push(type);
        importer.CreateSourceAnchor(type, node);
        type.isStub = false;
        base.VisitStructureBlock(node);
        currentTypeStack.Pop();
    }

    public override void VisitEnumBlock(EnumBlockSyntax node) {
        var typeSymbol = semanticModel.GetDeclaredSymbol(node);
        FAMIX.Type type = importer.EnsureType(typeSymbol);

        currentTypeStack.Push(type);
        importer.CreateSourceAnchor(type, node);
        type.isStub = false;
        base.VisitEnumBlock(node);
        currentTypeStack.Pop();
    }

    public override void VisitEnumMemberDeclaration(EnumMemberDeclarationSyntax node) {
        string attributeName = node.Identifier.ToString();
        var symbol = semanticModel.GetDeclaredSymbol(node);
        FAMIX.EnumValue anEnumValue = importer.EnsureAttribute(symbol) as FAMIX.EnumValue;
        importer.CreateSourceAnchor(anEnumValue, node);
        if (currentTypeStack.Peek() is FAMIX.Enum) {
            anEnumValue.parentEnum = currentTypeStack.Peek() as FAMIX.Enum;
            anEnumValue.parentEnum.AddValue(anEnumValue);
            anEnumValue.isStub = false;
        }
    }

    public override void VisitInterfaceBlock(InterfaceBlockSyntax node) {
        var typeSymbol = semanticModel.GetDeclaredSymbol(node);
        FAMIX.Class type = (FAMIX.Class)importer.EnsureType(typeSymbol);
        type.isInterface = true;
        //type.name = node.Identifier.ToString();
        AddSuperInterfaces(typeSymbol, type);

        currentTypeStack.Push(type);
        importer.CreateSourceAnchor(type, node);
        type.isStub = false;
        base.VisitInterfaceBlock(node);
        currentTypeStack.Pop();
    }

    public override void VisitConstructorBlock(ConstructorBlockSyntax node) {
        // Constructors in VB seem to not have Identifier. So far i am generating a name with the new keyword and the parameter list 
        string methodName = node.SubNewStatement.NewKeyword.ToString() + node.SubNewStatement.ParameterList.ToString();

        var methodSymbol = semanticModel.GetDeclaredSymbol(node);

        AddMethod(node.SubNewStatement, methodName);

        if (currentMethod != null) currentMethod.isConstructor = true;

        base.VisitConstructorBlock(node);
        currentMethod = null;
    }

    /* public override void VisitDestructorBlock(DestructorDeclarationSyntax node)
     {
         string methodName = node.Identifier.ToString();

         AddMethod(node, methodName);

         if (currentMethod != null) currentMethod.isConstructor = true;

         base.VisitDestructorDeclaration(node);
         currentMethod = null;
     }
     */
    public override void VisitAccessorBlock(AccessorBlockSyntax node) {
        if (currentProperty != null) {
            var methodSymbol = semanticModel.GetDeclaredSymbol(node);

            CSharp.CSharpPropertyAccessor aMethod = importer.EnsureMethod(methodSymbol) as CSharp.CSharpPropertyAccessor;
            if (methodSymbol.MethodKind == MethodKind.PropertyGet)
                currentProperty.getter = aMethod as CSharp.CSharpPropertyAccessor;
            else
                currentProperty.setter = aMethod as CSharp.CSharpPropertyAccessor;

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
        base.VisitAccessorBlock(node);
    }


    public override void VisitMethodBlock(MethodBlockSyntax node) {
        string methodName = node.SubOrFunctionStatement.Identifier.ToString();
        AddMethod(node.SubOrFunctionStatement, methodName);
        base.VisitMethodBlock(node);
        currentMethod = null;
    }









    public override void VisitPropertyBlock(PropertyBlockSyntax node) {

        string propertyName = node.PropertyStatement.Identifier.ToString();
        var prop = AddProperty(node, propertyName);
        if (prop is CSharp.CSharpProperty) currentProperty = prop as CSharp.CSharpProperty;
        base.VisitPropertyBlock(node);
        currentProperty = null;
    }

    private FAMIX.Attribute AddProperty(PropertyBlockSyntax node, String propertyName) {
        ISymbol symbol = semanticModel.GetDeclaredSymbol(node);
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

    public override void VisitEventBlock(EventBlockSyntax node) {

        string propertyName = node.EventStatement.Identifier.ToString();
        if (currentTypeStack.Count > 0) {
            var methodSymbol = semanticModel.GetDeclaredSymbol(node);
            var aMethod = importer.EnsureMethod(methodSymbol);
            aMethod.name = propertyName;
            aMethod.parentType = importer.EnsureType(methodSymbol.ContainingType);
            aMethod.parentType.AddMethod(aMethod);
            currentMethod = aMethod;
            importer.CreateSourceAnchor(aMethod, node);
            currentMethod.isStub = false;

        }
        base.VisitEventBlock(node);
    }



    public override void VisitFieldDeclaration(FieldDeclarationSyntax node) {
        AddField(node);
        base.VisitFieldDeclaration(node);
    }


    public override void VisitCatchBlock(CatchBlockSyntax node) {

        ISymbol typeSymbol = semanticModel.GetTypeInfo(node.CatchStatement.AsClause.Type).Type;
        var exceptionClass = (FAMIX.Class)importer.EnsureType(typeSymbol);
        FAMIX.CaughtException caughtException = importer.New<FAMIX.CaughtException>();
        caughtException.definingMethod = currentMethod;
        caughtException.exceptionClass = exceptionClass;
        base.VisitCatchBlock(node);
    }

    public override void VisitThrowStatement(ThrowStatementSyntax node) {
        if (node.Expression != null) {
            var symbolInfo = semanticModel.GetTypeInfo(node.Expression).Type;

            var exceptionClass = (FAMIX.Class)importer.EnsureType(symbolInfo);
            FAMIX.ThrownException thrownException = importer.New<FAMIX.ThrownException>();
            thrownException.definingMethod = currentMethod;
            thrownException.exceptionClass = exceptionClass;
        }
        base.VisitThrowStatement(node);
    }

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

    public override void VisitAssignmentStatement(AssignmentStatementSyntax node) {
        var isEvent = semanticModel.GetSymbolInfo(node.Left).Symbol;
        if (isEvent != null && isEvent.Kind == SymbolKind.Event) {
            var isMethod = semanticModel.GetSymbolInfo(node.Right).Symbol;
            if (isMethod != null && isMethod.Kind == SymbolKind.Method) {
                CSharpEvent cSharpEvent = importer.EnsureMethod(isEvent) as CSharpEvent;
                var handlerMethod = importer.EnsureMethod(isMethod as IMethodSymbol);
                AddMethodCall(node, cSharpEvent, handlerMethod);
            }
        }
        base.VisitAssignmentStatement(node);
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
    public override void VisitCaseBlock(CaseBlockSyntax node) {
        if (currentMethod != null) currentMethod.cyclomaticComplexity++;
        base.VisitCaseBlock(node);
    }
    public override void VisitSelectBlock(SelectBlockSyntax node) {
        if (currentMethod != null) currentMethod.cyclomaticComplexity++;
        base.VisitSelectBlock(node);
    }
    public override void VisitReturnStatement(ReturnStatementSyntax node) {
        if (currentMethod != null) currentMethod.cyclomaticComplexity++;
        base.VisitReturnStatement(node);
    }




    /*********************************************************************************************************
     * Auxiliary functions
     *********************************************************************************************************/




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
            (type as ParameterizableClass).Parameters.Add(importer.EnsureType(typeParameter) as FAMIX.ParameterType);
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
    /*
      This two methods AddMethod reveals the flaws of designing based on method overload instead of using polymorphism.  
    */


    private Method AddMethod(SubNewStatementSyntax node, string name) {
        return AddMethod(semanticModel.GetDeclaredSymbol(node), node, name);
    }
    private Method AddMethod(MethodStatementSyntax node, string name) {
        return AddMethod(semanticModel.GetDeclaredSymbol(node), node, name);
    }


    private Method AddMethod(IMethodSymbol methodSymbol, SyntaxNode node, string name) {
        if (currentTypeStack.Count > 0) {
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


    private void AddField(FieldDeclarationSyntax node) {
        foreach (var variableDeclarator in node.Declarators) {
            foreach (var attributeName in variableDeclarator.Names) {
                var returnTypeSymbol = semanticModel.GetDeclaredSymbol(variableDeclarator.AsClause.Type());
                var symbol = semanticModel.GetDeclaredSymbol(variableDeclarator);

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
    }



    private T GetSymbol<T>(SyntaxNode node) {
        var symbolInfo = semanticModel.GetSymbolInfo(node).Symbol;
        if (symbolInfo is T methodSymbol)
            return methodSymbol;
        return default(T);
    }

    /**
     * Visit an identifier, it can be anything, a method reference, a field, a local variable, etc.
     * We need to find out what it is and if it is located in a method and then make the appropriate connection.
     */


    private void AddAttributeAccess(SyntaxNode node, Method clientMethod, FAMIX.Attribute attribute) {
        Access access = importer.CreateNewAssociation<Access>("FAMIX.Access");
        access.accessor = currentMethod;
        access.variable = attribute;
        clientMethod.AddAccesse(access);
        attribute.AddIncomingAccesse(access);
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
        var symbol = semanticModel.GetSymbolInfo(node).Symbol;
        if (symbol is IMethodSymbol || symbol is IEventSymbol)
            return importer.EnsureMethod(symbol);
        if (symbol is IFieldSymbol)
            return importer.EnsureAttribute(symbol);
        if (symbol is IPropertySymbol)
            return importer.EnsureAttribute(symbol);
        return null;
    }

}