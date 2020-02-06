﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.VisualBasic;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using RoslynMonoFamix.ModelBuilder;

namespace RoslynMonoFamix.Visitor {
    class VBImportingVisitor : VisualBasicStackedVisitor {

        protected VisualBasicModelBuilder importer;


        public VBImportingVisitor(VisualBasicModelBuilder importer) : base() {
            this.importer = importer;
        }
        public override void VisitEmptyStatement(EmptyStatementSyntax node) {
            base.VisitEmptyStatement(node);
            // TODO: We should mark the current context as empty? 
            // this.CurrentContext<FAMIX.Entity>()
        }
        public override void VisitEndBlockStatement(EndBlockStatementSyntax node) {

            if (node.Parent is MethodBlockSyntax && stack.Count != 3) {
                var a = " ";

            }


            if (node.Kind() == SyntaxKind.EndSubStatement
                || node.Kind() == SyntaxKind.EndFunctionStatement
                || node.Kind() == SyntaxKind.EndClassStatement
                || node.Kind() == SyntaxKind.EndModuleStatement
                || node.Kind() == SyntaxKind.EndInterfaceStatement
                || node.Kind() == SyntaxKind.EndNamespaceStatement
                || node.Kind() == SyntaxKind.EndPropertyStatement
                || node.Kind() == SyntaxKind.EndSetStatement
                || node.Kind() == SyntaxKind.EndGetStatement
                || node.Kind() == SyntaxKind.EndWhileStatement
                || node.Kind() == SyntaxKind.EndIfStatement
                || node.Kind() == SyntaxKind.EndEnumStatement
                ) {
                this.PopContext();
            } else {
                var a = " ";
            }
        }
        protected void Assert(Boolean val) {
            if (!val) throw new Exception("Assert failure");
        }
        public override void VisitCompilationUnit(CompilationUnitSyntax node) {
            this.Assert(stack.Count == 0);
            FAMIX.ScopingEntity unit = importer.CreateScopingEntity(node);
            this.PushContext(unit);
            base.VisitCompilationUnit(node);
            this.PopContext();
            this.Assert(stack.Count == 0);
        }
        public override void VisitOptionStatement(OptionStatementSyntax node) {
            base.VisitOptionStatement(node);
        }
        public override void VisitImportsStatement(ImportsStatementSyntax node) {
            //Nothing to do;
        }
        public override void VisitSimpleImportsClause(SimpleImportsClauseSyntax node) {
            //Nothing to do;
        }
        public override void VisitImportAliasClause(ImportAliasClauseSyntax node) {
            //Nothing to do;
        }
        public override void VisitXmlNamespaceImportsClause(XmlNamespaceImportsClauseSyntax node) {
            //Nothing to do;
        }
        public override void VisitNamespaceBlock(NamespaceBlockSyntax node) {
            base.VisitNamespaceBlock(node);
        }
        public override void VisitNamespaceStatement(NamespaceStatementSyntax node) {
            FAMIX.Namespace nspace = importer.EnsureNamespace(importer.model.GetDeclaredSymbol(node));
            this.CurrentContext<FAMIX.ScopingEntity>().AddChildScope(nspace);
            this.PushContext(nspace);
        }
        public override void VisitModuleBlock(ModuleBlockSyntax node) {
            base.VisitModuleBlock(node);
        }
        public override void VisitStructureBlock(StructureBlockSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitInterfaceBlock(InterfaceBlockSyntax node) {
            base.VisitInterfaceBlock(node);
        }
        public override void VisitClassBlock(ClassBlockSyntax node) {
            base.VisitClassBlock(node);
        }
        public override void VisitEnumBlock(EnumBlockSyntax node) {
            base.VisitEnumBlock(node);
        }
        public override void VisitInheritsStatement(InheritsStatementSyntax node) {
            FAMIX.Inheritance inheritance = importer.CreateInheritanceFor(this.CurrentContext<FAMIX.Class>());

            this.PushContext(inheritance.TypingContext(importer.model.GetDeclaredSymbol(node.Parent)));
            base.VisitInheritsStatement(node);
            this.PopContext();
        }
        public override void VisitImplementsStatement(ImplementsStatementSyntax node) {
            // we should check how to get the type of the interface. 

            FAMIX.Implements implementation = importer.CreateImplementsFor(this.CurrentContext<FAMIX.Class>());
            FAMIX.ImplementsTypingContext context = implementation.ImplementsTypingContext();
            context.AddSymbols(node.Types.Select(t => (INamedTypeSymbol)importer.model.GetSymbolInfo(t).Symbol).ToList());

            this.PushContext(context);
            base.VisitImplementsStatement(node);
            this.PopContext();
        }
        public override void VisitModuleStatement(ModuleStatementSyntax node) {
            FAMIX.Class FamixClass = importer.EnsureClass(importer.model.GetDeclaredSymbol(node));
            FamixClass.isModule = true;
            FamixClass.isShadow = node.Modifiers.Any(SyntaxKind.ShadowsKeyword);
            FamixClass.isPrivate = node.Modifiers.Any(SyntaxKind.PrivateKeyword);
            FamixClass.isPublic = node.Modifiers.Any(SyntaxKind.PublicKeyword); ;
            FamixClass.isProtected = node.Modifiers.Any(SyntaxKind.ProtectedKeyword); ;
            FamixClass.Modifiers.AddRange(node.Modifiers.Select(p => p.Text).ToList());
            FAMIX.IAddType AGoodSuperContext = (FAMIX.IAddType)this.CurrentContext<FAMIX.Entity>();
            AGoodSuperContext.AddType(FamixClass);
            this.PushContext(FamixClass);
            base.VisitModuleStatement(node);
        }
        public override void VisitStructureStatement(StructureStatementSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitInterfaceStatement(InterfaceStatementSyntax node) {
            FAMIX.Class FamixClass = importer.EnsureIterface(importer.model.GetDeclaredSymbol(node));
            FamixClass.isShadow = node.Modifiers.Any(SyntaxKind.ShadowsKeyword);
            FamixClass.isPrivate = node.Modifiers.Any(SyntaxKind.PrivateKeyword);
            FamixClass.isPublic = node.Modifiers.Any(SyntaxKind.PublicKeyword); ;
            FamixClass.isProtected = node.Modifiers.Any(SyntaxKind.ProtectedKeyword); ;
            FamixClass.Modifiers.AddRange(node.Modifiers.Select(p => p.Text).ToList());
            FAMIX.IAddType AGoodSuperContext = (FAMIX.IAddType)this.CurrentContext<FAMIX.Entity>();
            AGoodSuperContext.AddType(FamixClass);

            this.PushContext(FamixClass);
            base.VisitInterfaceStatement(node);
        }
        public override void VisitClassStatement(ClassStatementSyntax node) {
            FAMIX.Class FamixClass = importer.EnsureClass(importer.model.GetDeclaredSymbol(node));

            FamixClass.isShadow = node.Modifiers.Any(SyntaxKind.ShadowsKeyword);
            FamixClass.isPrivate = node.Modifiers.Any(SyntaxKind.PrivateKeyword);
            FamixClass.isPublic = node.Modifiers.Any(SyntaxKind.PublicKeyword); ;
            FamixClass.isProtected = node.Modifiers.Any(SyntaxKind.ProtectedKeyword); ;
            FamixClass.Modifiers.AddRange(node.Modifiers.Select(p => p.Text).ToList());
            FAMIX.IAddType AGoodSuperContext = (FAMIX.IAddType)this.CurrentContext<FAMIX.Entity>();
            AGoodSuperContext.AddType(FamixClass);
            this.PushContext(FamixClass);
            base.VisitClassStatement(node);
        }
        public override void VisitEnumStatement(EnumStatementSyntax node) {
            FAMIX.Type FamixType = importer.EnsureType(importer.model.GetDeclaredSymbol(node));

            FamixType.isShadow = node.Modifiers.Any(SyntaxKind.ShadowsKeyword);
            FamixType.isPrivate = node.Modifiers.Any(SyntaxKind.PrivateKeyword);
            FamixType.isPublic = node.Modifiers.Any(SyntaxKind.PublicKeyword); ;
            FamixType.isProtected = node.Modifiers.Any(SyntaxKind.ProtectedKeyword); ;
            FamixType.Modifiers.AddRange(node.Modifiers.Select(p => p.Text).ToList());
            FAMIX.IAddType AGoodSuperContext = (FAMIX.IAddType)this.CurrentContext<FAMIX.Entity>();
            AGoodSuperContext.AddType(FamixType);
            this.PushContext(FamixType);
            base.VisitEnumStatement(node);
        }
        public override void VisitTypeParameterList(TypeParameterListSyntax node) {
            base.VisitTypeParameterList(node);
        }
        public override void VisitTypeParameter(TypeParameterSyntax node) {
            FAMIX.ParameterizableEntity FamixClass = (FAMIX.ParameterizableEntity)this.CurrentContext<FAMIX.Entity>();
            FAMIX.ParameterType Type = importer.EnsureParametrizedTypeInto(FamixClass, importer.model.GetDeclaredSymbol(node));
            this.PushContext(Type);
            base.VisitTypeParameter(node);

            node.TypeParameterConstraintClause?.Accept(this);
            this.PopContext();
        }
        public override void VisitTypeParameterSingleConstraintClause(TypeParameterSingleConstraintClauseSyntax node) {

            FAMIX.TypeBoundary boundary = importer.CreateTypeBoundary(this.CurrentContext<FAMIX.ParameterType>());
            ITypeParameterSymbol symbol = (ITypeParameterSymbol)importer.model.GetDeclaredSymbol(node.Parent);
            this.PushContext(boundary.TypingContext(symbol.ConstraintTypes.Single()));
            base.VisitTypeParameterSingleConstraintClause(node);
            this.PopContext();
        }

        public override void VisitTypeParameterMultipleConstraintClause(TypeParameterMultipleConstraintClauseSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitSpecialConstraint(SpecialConstraintSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitTypeConstraint(TypeConstraintSyntax node) {
            base.VisitTypeConstraint(node);
        }
        public override void VisitEnumMemberDeclaration(EnumMemberDeclarationSyntax node) {

            var Enumeration = this.CurrentContext<FAMIX.Enum>();
            var EnumVal = importer.EnsureEnumField(importer.model.GetDeclaredSymbol(node));
            Enumeration.AddValue(EnumVal);
            EnumVal.parentEnum = Enumeration;
            this.PushContext(EnumVal);
            base.VisitEnumMemberDeclaration(node);
            this.PopContext();
        }
        public override void VisitMethodBlock(MethodBlockSyntax node) {
            base.VisitMethodBlock(node);
        }
        public override void VisitConstructorBlock(ConstructorBlockSyntax node) {
            base.VisitConstructorBlock(node);
        }
        public override void VisitOperatorBlock(OperatorBlockSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitAccessorBlock(AccessorBlockSyntax node) {

            base.VisitAccessorBlock(node);
        }
        public override void VisitPropertyBlock(PropertyBlockSyntax node) {
            base.VisitPropertyBlock(node);
        }
        public override void VisitEventBlock(EventBlockSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitParameterList(ParameterListSyntax node) {
            base.VisitParameterList(node);
        }


        public override void VisitMethodStatement(MethodStatementSyntax node) {
            FAMIX.Method FamixMethod = importer.EnsureMethod(importer.model.GetDeclaredSymbol(node));

            FamixMethod.isShadow = node.Modifiers.Any(SyntaxKind.ShadowsKeyword);
            FamixMethod.isPrivate = node.Modifiers.Any(SyntaxKind.PrivateKeyword);
            FamixMethod.isPublic = node.Modifiers.Any(SyntaxKind.PublicKeyword);
            FamixMethod.isProtected = node.Modifiers.Any(SyntaxKind.ProtectedKeyword);
            FamixMethod.Modifiers.AddRange(node.Modifiers.Select(p => p.Text).ToList());

            FAMIX.Type AGoodSuperContext = this.CurrentContext<FAMIX.Type>();
            AGoodSuperContext.AddMethod(FamixMethod);
            this.PushContext(FamixMethod);

            importer.CreateSourceAnchor(FamixMethod, node);


            base.VisitMethodStatement(node);

            if (node.Parent is InterfaceBlockSyntax || node.Parent is ClassBlockSyntax) {
                this.PopContext();
            } else {
                if (!(node.Parent is MethodBlockSyntax)) {
                    /*
                     It seems that property statement can be included directly into the class body (for the fully automatic properties)
                     And it can be included also by a property block, when we define a body for the property.
                     If we have a body, means we have an end property statement, where we should be doing the pop of the statemet.
                     Still, none of this affirmations are completely 
                     */

                    throw new Exception("Unexpected parent. ");
                }
            }
        }
        public override void VisitSubNewStatement(SubNewStatementSyntax node) {
            FAMIX.Method FamixClass = importer.EnsureConstructor(importer.model.GetDeclaredSymbol(node));
            FAMIX.Type AGoodSuperContext = this.CurrentContext<FAMIX.Type>();
            AGoodSuperContext.AddMethod(FamixClass);
            this.PushContext(FamixClass);
            base.VisitSubNewStatement(node);
        }
        public override void VisitDeclareStatement(DeclareStatementSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitDelegateStatement(DelegateStatementSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitEventStatement(EventStatementSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitOperatorStatement(OperatorStatementSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitPropertyStatement(PropertyStatementSyntax node) {
            var symbol = this.importer.model.GetDeclaredSymbol(node);
            FAMIX.Class FamixClass = this.CurrentContext<FAMIX.Class>();
            Net.Property property = this.importer.EnsureProperty(symbol);
            property.Modifiers.AddRange(node.Modifiers.Select(p => p.Text).ToList());
            FamixClass.AddAttribute(property);
            property.parentType = FamixClass;

            this.PushContext(property);
            base.VisitPropertyStatement(node);

            if (node.Parent is ClassBlockSyntax) {
                this.PopContext();
            } else {
                if (!(node.Parent is PropertyBlockSyntax)) {
                    /*
                     It seems that property statement can be included directly into the class body (for the fully automatic properties)
                     And it can be included also by a property block, when we define a body for the property.
                     If we have a body, means we have an end property statement, where we should be doing the pop of the statemet.
                     Still, none of this affirmations are completely 
                     */

                    throw new Exception("Unexpected parent. ");
                }

            }



            // The poping of this content happens during the end statement . 

        }
        public override void VisitAccessorStatement(AccessorStatementSyntax node) {

            Net.Property Owner = this.CurrentContext<Net.Property>();
            Net.PropertyAccessor NetPropertyAccessor = importer.EnsureAccessorInto(importer.model.GetDeclaredSymbol(node), Owner);

            NetPropertyAccessor.isShadow = node.Modifiers.Any(SyntaxKind.ShadowsKeyword);
            NetPropertyAccessor.isPrivate = node.Modifiers.Any(SyntaxKind.PrivateKeyword);
            NetPropertyAccessor.isPublic = node.Modifiers.Any(SyntaxKind.PublicKeyword);
            NetPropertyAccessor.isProtected = node.Modifiers.Any(SyntaxKind.ProtectedKeyword);
            NetPropertyAccessor.Modifiers.AddRange(node.Modifiers.Select(p => p.Text).ToList());

            this.PushContext(NetPropertyAccessor);

            base.VisitAccessorStatement(node);

        }
        public override void VisitImplementsClause(ImplementsClauseSyntax node) {
            if (node.Parent is MethodStatementSyntax) return;
            throw new Exception("Should we care about this? ");
        }
        public override void VisitHandlesClause(HandlesClauseSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitKeywordEventContainer(KeywordEventContainerSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitWithEventsEventContainer(WithEventsEventContainerSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitWithEventsPropertyEventContainer(WithEventsPropertyEventContainerSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitHandlesClauseItem(HandlesClauseItemSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitIncompleteMember(IncompleteMemberSyntax node) {
            throw new Exception("MustReview");
        }


        public override void VisitFieldDeclaration(FieldDeclarationSyntax node) {
            // this define that whaever-is declared inside is mean to be a field (meaning that it belongs to a class or type)  
            FAMIX.StructuralEntityGroup Group = this.importer.CreateStructuralEntityGroup();
            FAMIX.Class FamixClass = this.CurrentContext<FAMIX.Class>();
            this.PushContext(Group);
            base.VisitFieldDeclaration(node);
            this.PopContext();
            Group.AddModifiers(node.Modifiers.Select(p => p.Text).ToList());
            Group.AddAllAttributesInto(FamixClass);
        }
        public override void VisitModifiedIdentifier(ModifiedIdentifierSyntax node) {
            var symbol = this.importer.model.GetDeclaredSymbol(node);
            if (symbol != null && symbol is IFieldSymbol) {
                FAMIX.StructuralEntityGroup Group = this.CurrentContext<FAMIX.StructuralEntityGroup>();
                FAMIX.Attribute Attribute = this.importer.EnsureField((IFieldSymbol)symbol);
                Group.AddAttribute(Attribute);
            }
            if (symbol != null && symbol is ILocalSymbol) {
                FAMIX.StructuralEntityGroup Group = this.CurrentContext<FAMIX.StructuralEntityGroup>();
                FAMIX.LocalVariable Variable = this.importer.EnsureLocalVariable((ILocalSymbol)symbol);

                Group.AddLocalVariable(Variable);
            }
            base.VisitModifiedIdentifier(node);
        }
        public override void VisitVariableDeclarator(VariableDeclaratorSyntax node) {
            // A variable declarator is mean to put together a a set names along side a type. Int i, j, k; 
            base.VisitVariableDeclarator(node);
        }
        public override void VisitSimpleAsClause(SimpleAsClauseSyntax node) {
            FAMIX.ITyped Typing = (this.CurrentContext<FAMIX.Entity>() as FAMIX.ITyped);
            this.PushContext(Typing.TypingContext(importer.model.GetDeclaredSymbol(node.Parent)));
            base.VisitSimpleAsClause(node);
            this.PopContext();
        }
        public override void VisitAsNewClause(AsNewClauseSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitObjectMemberInitializer(ObjectMemberInitializerSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitObjectCollectionInitializer(ObjectCollectionInitializerSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitInferredFieldInitializer(InferredFieldInitializerSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitNamedFieldInitializer(NamedFieldInitializerSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitEqualsValue(EqualsValueSyntax node) {
            FAMIX.Entity context = this.CurrentContext<FAMIX.Entity>();

            if ((context is FAMIX.StructuralEntity) || context is FAMIX.StructuralEntityGroup) {
                base.VisitEqualsValue(node);
                return;
            }

            if (!(context is FAMIX.Parameter)) throw new Exception("Should check what's happening here. The parameters are managing the default value. What about this thing in the stack? ");
            base.VisitEqualsValue(node);
        }
        public override void VisitParameter(ParameterSyntax node) {
            FAMIX.Method CurrentMethod = this.CurrentContext<FAMIX.Method>();
            FAMIX.Parameter parameter = importer.EnsureParameterInMethod(importer.model.GetDeclaredSymbol(node), CurrentMethod);
            parameter.Modifiers.AddRange(node.Modifiers.Select(p => p.Text).ToList());
            this.PushContext(parameter);
            base.VisitParameter(node);
            this.PopContext();
        }

        public override void VisitArrayRankSpecifier(ArrayRankSpecifierSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitAttributeList(AttributeListSyntax node) {
            base.VisitAttributeList(node);
        }
        public override void VisitAttribute(AttributeSyntax node) {
            FAMIX.AnnotationInstance instance;
            ISymbol symbolicRef = this.importer.model.GetDeclaredSymbol(node.Parent.Parent);


            if (symbolicRef != null) {
                String name = node.Name.ToString();
                List<AttributeData> symbols;
                symbols = symbolicRef.GetAttributes().ToList().FindAll(f => f.AttributeClass.Name == name);
                if (symbols.Count() == 0) {
                    if (node.Name is QualifiedNameSyntax) {
                        name = ((QualifiedNameSyntax)node.Name).Right.ToString();
                    }
                    symbols = symbolicRef.GetAttributes().ToList().FindAll(f => f.AttributeClass.Name == name);
                }
                if (symbols.Count() > 0) {
                    instance = importer.EnsureAnnotationInstance(symbols.First(), this.CurrentContext<FAMIX.NamedEntity>());
                    this.PushContext(instance);
                    base.VisitAttribute(node);
                    this.PopContext();
                }
            }
        }
        public override void VisitAttributeTarget(AttributeTargetSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitAttributesStatement(AttributesStatementSyntax node) {
            base.VisitAttributesStatement(node);
        }
        public override void VisitExpressionStatement(ExpressionStatementSyntax node) {
            base.VisitExpressionStatement(node);
        }
        public override void VisitPrintStatement(PrintStatementSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitWhileBlock(WhileBlockSyntax node) {
            base.VisitWhileBlock(node);
        }
        public override void VisitUsingBlock(UsingBlockSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitSyncLockBlock(SyncLockBlockSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitWithBlock(WithBlockSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitLocalDeclarationStatement(LocalDeclarationStatementSyntax node) {


            FAMIX.StructuralEntityGroup Group = this.importer.CreateStructuralEntityGroup();
            FAMIX.BehaviouralEntity FamixEntity = this.CurrentContext<FAMIX.BehaviouralEntity>();

            this.PushContext(Group);
            base.VisitLocalDeclarationStatement(node);
            this.PopContext();

            Group.AddModifiers(node.Modifiers.Select(p => p.Text).ToList());
            Group.AddAllLocalVariablesInto(FamixEntity);

        }
        public override void VisitLabelStatement(LabelStatementSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitGoToStatement(GoToStatementSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitLabel(LabelSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitStopOrEndStatement(StopOrEndStatementSyntax node) {
            this.PopContext();
        }
        public override void VisitExitStatement(ExitStatementSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitContinueStatement(ContinueStatementSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitReturnStatement(ReturnStatementSyntax node) {
            this.PushContext(FAMIX.TypingContext.NullContext());
            base.VisitReturnStatement(node);
            this.PopContext();
        }
        public override void VisitSingleLineIfStatement(SingleLineIfStatementSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitSingleLineElseClause(SingleLineElseClauseSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitMultiLineIfBlock(MultiLineIfBlockSyntax node) {
            base.VisitMultiLineIfBlock(node);
        }
        public override void VisitIfStatement(IfStatementSyntax node) {
            FAMIX.BehaviouralEntity method = this.CurrentContext<FAMIX.BehaviouralEntity>();
            method.numberOfConditionals++;

            FAMIX.ControlFlowStructure FamixEntity = this.importer.CreateControlStructure("IF", method);
            FAMIX.StructuralEntityGroup Group = this.importer.CreateStructuralEntityGroup();
            FamixEntity.Condition = node.Condition.ToString();


            this.PushContext(FamixEntity);
            this.PushContext(Group);

            base.VisitIfStatement(node);


            this.PopContext();
            Group.AddAllLocalVariablesInto(FamixEntity);

        }
        public override void VisitElseIfBlock(ElseIfBlockSyntax node) {
            base.VisitElseIfBlock(node);
        }
        public override void VisitElseIfStatement(ElseIfStatementSyntax node) {
            this.PopContext();

            FAMIX.BehaviouralEntity method = this.CurrentContext<FAMIX.BehaviouralEntity>();
            method.numberOfConditionals++;

            FAMIX.ControlFlowStructure FamixEntity = this.importer.CreateControlStructure("ELSE-IF", method);
            FamixEntity.Condition = node.Condition.ToString();

            FAMIX.StructuralEntityGroup Group = this.importer.CreateStructuralEntityGroup();

            this.PushContext(FamixEntity);
            this.PushContext(Group);

            base.VisitElseIfStatement(node);


            this.PopContext();
            Group.AddAllLocalVariablesInto(FamixEntity);


        }
        public override void VisitElseBlock(ElseBlockSyntax node) {
            base.VisitElseBlock(node);
        }
        public override void VisitElseStatement(ElseStatementSyntax node) {
            this.PopContext();

            FAMIX.BehaviouralEntity method = this.CurrentContext<FAMIX.BehaviouralEntity>();
            method.numberOfConditionals++;

            FAMIX.ControlFlowStructure FamixEntity = this.importer.CreateControlStructure("ELSE", method);
            FAMIX.StructuralEntityGroup Group = this.importer.CreateStructuralEntityGroup();

            this.PushContext(FamixEntity);
            this.PushContext(Group);

            base.VisitElseStatement(node);


            this.PopContext();
            Group.AddAllLocalVariablesInto(FamixEntity);

        }
        public override void VisitTryBlock(TryBlockSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitTryStatement(TryStatementSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitCatchBlock(CatchBlockSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitCatchStatement(CatchStatementSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitCatchFilterClause(CatchFilterClauseSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitFinallyBlock(FinallyBlockSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitFinallyStatement(FinallyStatementSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitErrorStatement(ErrorStatementSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitOnErrorGoToStatement(OnErrorGoToStatementSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitOnErrorResumeNextStatement(OnErrorResumeNextStatementSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitResumeStatement(ResumeStatementSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitSelectBlock(SelectBlockSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitSelectStatement(SelectStatementSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitCaseBlock(CaseBlockSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitCaseStatement(CaseStatementSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitElseCaseClause(ElseCaseClauseSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitSimpleCaseClause(SimpleCaseClauseSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitRangeCaseClause(RangeCaseClauseSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitRelationalCaseClause(RelationalCaseClauseSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitSyncLockStatement(SyncLockStatementSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitDoLoopBlock(DoLoopBlockSyntax node) {
            base.VisitDoLoopBlock(node);
        }
        public override void VisitDoStatement(DoStatementSyntax node) {
            this.CurrentContext<FAMIX.Method>().numberOfConditionals++;
            this.CurrentContext<FAMIX.Method>().numberOfLoops++;
            base.VisitDoStatement(node);
        }
        public override void VisitLoopStatement(LoopStatementSyntax node) {
            this.CurrentContext<FAMIX.Method>().numberOfConditionals++;
            this.CurrentContext<FAMIX.Method>().numberOfLoops++;
            base.VisitLoopStatement(node);
        }
        public override void VisitWhileOrUntilClause(WhileOrUntilClauseSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitWhileStatement(WhileStatementSyntax node) {
            FAMIX.BehaviouralEntity method = this.CurrentContext<FAMIX.BehaviouralEntity>();
            method.numberOfConditionals++;
            method.numberOfLoops++;

            FAMIX.ControlFlowStructure FamixEntity = this.importer.CreateControlStructure("WHILE", method);
            FAMIX.StructuralEntityGroup Group = this.importer.CreateStructuralEntityGroup();
            FamixEntity.Condition = node.Condition.ToString();

            this.PushContext(FamixEntity);
            this.PushContext(Group);

            base.VisitWhileStatement(node);

            this.PopContext();
            Group.AddAllLocalVariablesInto(FamixEntity);
        }
        public override void VisitForBlock(ForBlockSyntax node) {
            base.VisitForBlock(node);
        }
        public override void VisitForEachBlock(ForEachBlockSyntax node) {
            base.VisitForEachBlock(node);
        }
        public override void VisitForStatement(ForStatementSyntax node) {
            FAMIX.BehaviouralEntity method = this.CurrentContext<FAMIX.BehaviouralEntity>();
            method.numberOfConditionals++;
            method.numberOfLoops++;

            FAMIX.ControlFlowStructure FamixEntity = this.importer.CreateControlStructure("FOR", method);
            FamixEntity.Condition = node.FromValue.ToString() + " To " + node.ToValue.ToString() + " Step ";
            if (node.StepClause == null || node.StepClause.StepValue == null) {
                FamixEntity.Condition += "1";
            } else {
                FamixEntity.Condition += node.StepClause.StepValue.ToString();
            }
            FAMIX.StructuralEntityGroup Group = this.importer.CreateStructuralEntityGroup();

            this.PushContext(FamixEntity);
            this.PushContext(Group);

            base.VisitForStatement(node);

            this.PopContext();
            Group.AddAllLocalVariablesInto(FamixEntity);
        }
        public override void VisitForStepClause(ForStepClauseSyntax node) {

            base.VisitForStepClause(node);
        }
        public override void VisitForEachStatement(ForEachStatementSyntax node) {
            FAMIX.BehaviouralEntity method = this.CurrentContext<FAMIX.BehaviouralEntity>();
            method.numberOfConditionals++;
            method.numberOfLoops++;

            FAMIX.ControlFlowStructure FamixEntity = this.importer.CreateControlStructure("FOREACH", method);
            FamixEntity.Condition = node.Expression.ToString();
            FAMIX.StructuralEntityGroup Group = this.importer.CreateStructuralEntityGroup();

            this.PushContext(FamixEntity);
            this.PushContext(Group);

            base.VisitForEachStatement(node);

            this.PopContext();
            Group.AddAllLocalVariablesInto(FamixEntity);


        }
        public override void VisitNextStatement(NextStatementSyntax node) {

            base.VisitNextStatement(node);
            this.PopContext();
        }
        public override void VisitUsingStatement(UsingStatementSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitThrowStatement(ThrowStatementSyntax node) {
            if (node.Expression != null) {
                var symbolInfo = importer.model.GetTypeInfo(node.Expression).Type;
                FAMIX.ThrownException thrownException = importer.CreateExceptionFor(symbolInfo, (FAMIX.Method)this.CurrentMethod());
            }
            base.VisitThrowStatement(node);
        }
        public override void VisitAssignmentStatement(AssignmentStatementSyntax node) {
            this.PushContext(FAMIX.TypingContext.NullContext());
            base.VisitAssignmentStatement(node);
            this.PopContext();
        }
        public override void VisitMidExpression(MidExpressionSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitCallStatement(CallStatementSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitAddRemoveHandlerStatement(AddRemoveHandlerStatementSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitRaiseEventStatement(RaiseEventStatementSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitWithStatement(WithStatementSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitReDimStatement(ReDimStatementSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitRedimClause(RedimClauseSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitEraseStatement(EraseStatementSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitLiteralExpression(LiteralExpressionSyntax node) {
            base.VisitLiteralExpression(node);
        }
        public override void VisitParenthesizedExpression(ParenthesizedExpressionSyntax node) {
            base.VisitParenthesizedExpression(node);
        }
        public override void VisitTupleExpression(TupleExpressionSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitTupleType(TupleTypeSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitTypedTupleElement(TypedTupleElementSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitNamedTupleElement(NamedTupleElementSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitMeExpression(MeExpressionSyntax node) {
            base.VisitMeExpression(node);
        }
        public override void VisitMyBaseExpression(MyBaseExpressionSyntax node) {
            base.VisitMyBaseExpression(node);
        }
        public override void VisitMyClassExpression(MyClassExpressionSyntax node) {
            base.VisitMyClassExpression(node);
        }
        public override void VisitGetTypeExpression(GetTypeExpressionSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitTypeOfExpression(TypeOfExpressionSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitGetXmlNamespaceExpression(GetXmlNamespaceExpressionSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitMemberAccessExpression(MemberAccessExpressionSyntax node) {

        }
        public override void VisitXmlMemberAccessExpression(XmlMemberAccessExpressionSyntax node) {
            throw new Exception("MustReview");
        }

        public override void VisitInvocationExpression(InvocationExpressionSyntax node) {
            FAMIX.BehaviouralEntity current = this.CurrentMethod();
            SymbolInfo info = importer.model.GetSymbolInfo(node);

            if (info.Symbol != null) {
                FAMIX.Method invoked = this.importer.EnsureMethod((IMethodSymbol)info.Symbol);
                importer.AddMethodCall(node, current, invoked);
            } else if (info.CandidateSymbols.Count() > 0) {
                foreach (ISymbol s in info.CandidateSymbols) {
                    FAMIX.Method invoked = this.importer.EnsureMethod((IMethodSymbol)s);
                    importer.AddMethodCall(node, current, invoked);
                }
            } else {
                /* In this invocation expression area, we have to link the outgoing calls. If we cannot get a direct call nor some candidates, it may mean that we are missing something */
                throw new Exception("missing something maybe? ");
            }

        }
        public override void VisitObjectCreationExpression(ObjectCreationExpressionSyntax node) {
            // Nothing to do here! 
        }
        public override void VisitAnonymousObjectCreationExpression(AnonymousObjectCreationExpressionSyntax node) {
            // Nothing to do here! 
        }
        public override void VisitArrayCreationExpression(ArrayCreationExpressionSyntax node) {
            // Nothing to do here! 
        }
        public override void VisitCollectionInitializer(CollectionInitializerSyntax node) {
            // Nothing to do here! 
        }
        public override void VisitCTypeExpression(CTypeExpressionSyntax node) {
            // Nothing to do here.. by the moment base.VisitCTypeExpression(node);
        }
        public override void VisitDirectCastExpression(DirectCastExpressionSyntax node) {
            base.VisitDirectCastExpression(node);
        }
        public override void VisitTryCastExpression(TryCastExpressionSyntax node) {
            base.VisitTryCastExpression(node);
        }
        public override void VisitPredefinedCastExpression(PredefinedCastExpressionSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitBinaryExpression(BinaryExpressionSyntax node) {
            base.VisitBinaryExpression(node);
        }
        public override void VisitUnaryExpression(UnaryExpressionSyntax node) {
            base.VisitUnaryExpression(node);
        }
        public override void VisitBinaryConditionalExpression(BinaryConditionalExpressionSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitTernaryConditionalExpression(TernaryConditionalExpressionSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitSingleLineLambdaExpression(SingleLineLambdaExpressionSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitMultiLineLambdaExpression(MultiLineLambdaExpressionSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitLambdaHeader(LambdaHeaderSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitArgumentList(ArgumentListSyntax node) {
            base.VisitArgumentList(node);
        }
        public override void VisitOmittedArgument(OmittedArgumentSyntax node) {
            base.VisitOmittedArgument(node);
        }
        public override void VisitSimpleArgument(SimpleArgumentSyntax node) {
            if (node.NameColonEquals != null) {
                this.importer.EnsureAnnotationAttributeInto(this.CurrentContext<FAMIX.AnnotationInstance>(), node.NameColonEquals.Name.ToString(), node.Expression.ToString());
            }
            base.VisitSimpleArgument(node);
        }
        public override void VisitNameColonEquals(NameColonEqualsSyntax node) {
            base.VisitNameColonEquals(node);
        }
        public override void VisitRangeArgument(RangeArgumentSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitQueryExpression(QueryExpressionSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitCollectionRangeVariable(CollectionRangeVariableSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitExpressionRangeVariable(ExpressionRangeVariableSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitAggregationRangeVariable(AggregationRangeVariableSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitVariableNameEquals(VariableNameEqualsSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitFunctionAggregation(FunctionAggregationSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitGroupAggregation(GroupAggregationSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitFromClause(FromClauseSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitLetClause(LetClauseSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitAggregateClause(AggregateClauseSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitDistinctClause(DistinctClauseSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitWhereClause(WhereClauseSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitPartitionWhileClause(PartitionWhileClauseSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitPartitionClause(PartitionClauseSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitGroupByClause(GroupByClauseSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitJoinCondition(JoinConditionSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitSimpleJoinClause(SimpleJoinClauseSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitGroupJoinClause(GroupJoinClauseSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitOrderByClause(OrderByClauseSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitOrdering(OrderingSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitSelectClause(SelectClauseSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitXmlDocument(XmlDocumentSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitXmlDeclaration(XmlDeclarationSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitXmlDeclarationOption(XmlDeclarationOptionSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitXmlElement(XmlElementSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitXmlText(XmlTextSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitXmlElementStartTag(XmlElementStartTagSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitXmlElementEndTag(XmlElementEndTagSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitXmlEmptyElement(XmlEmptyElementSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitXmlAttribute(XmlAttributeSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitXmlString(XmlStringSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitXmlPrefixName(XmlPrefixNameSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitXmlName(XmlNameSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitXmlBracketedName(XmlBracketedNameSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitXmlPrefix(XmlPrefixSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitXmlComment(XmlCommentSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitXmlProcessingInstruction(XmlProcessingInstructionSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitXmlCDataSection(XmlCDataSectionSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitXmlEmbeddedExpression(XmlEmbeddedExpressionSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitArrayType(ArrayTypeSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitNullableType(NullableTypeSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitPredefinedType(PredefinedTypeSyntax node) {
            FAMIX.TypingContext typing = this.CurrentContext<FAMIX.TypingContext>();
            typing.TypeUsing(importer);
            base.VisitPredefinedType(node);
        }
        public override void VisitIdentifierName(IdentifierNameSyntax node) {
            if (!(node.Parent is ForStatementSyntax || node.Parent is AttributeSyntax || node.Parent is NameColonEqualsSyntax || node.Parent is BinaryExpressionSyntax || node.Parent is QualifiedNameSyntax)) {
                FAMIX.TypingContext typing = this.CurrentContext<FAMIX.TypingContext>();
                typing.TypeUsing(importer);
            }
            base.VisitIdentifierName(node);
        }
        public override void VisitGenericName(GenericNameSyntax node) {
            if (!(node.Parent is ForStatementSyntax || node.Parent is AttributeSyntax || node.Parent is NameColonEqualsSyntax || node.Parent is BinaryExpressionSyntax)) {
                FAMIX.TypingContext typing = this.CurrentContext<FAMIX.TypingContext>();
                typing.TypeUsing(importer);
            }
            base.VisitGenericName(node);

        }
        public override void VisitQualifiedName(QualifiedNameSyntax node) {
            if (!(node.Parent is ForStatementSyntax || node.Parent is AttributeSyntax || node.Parent is NameColonEqualsSyntax || node is QualifiedNameSyntax)) {
                FAMIX.TypingContext typing = this.CurrentContext<FAMIX.TypingContext>();
                typing.TypeUsing(importer);
            }
            base.VisitQualifiedName(node);
        }
        public override void VisitGlobalName(GlobalNameSyntax node) {
            base.VisitGlobalName(node);
        }
        public override void VisitTypeArgumentList(TypeArgumentListSyntax node) {
            base.VisitTypeArgumentList(node);
        }
        public override void VisitCrefReference(CrefReferenceSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitCrefSignature(CrefSignatureSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitCrefSignaturePart(CrefSignaturePartSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitCrefOperatorReference(CrefOperatorReferenceSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitQualifiedCrefOperatorReference(QualifiedCrefOperatorReferenceSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitYieldStatement(YieldStatementSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitAwaitExpression(AwaitExpressionSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitSkippedTokensTrivia(SkippedTokensTriviaSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitDocumentationCommentTrivia(DocumentationCommentTriviaSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitXmlCrefAttribute(XmlCrefAttributeSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitXmlNameAttribute(XmlNameAttributeSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitConditionalAccessExpression(ConditionalAccessExpressionSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitNameOfExpression(NameOfExpressionSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitInterpolatedStringExpression(InterpolatedStringExpressionSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitInterpolatedStringText(InterpolatedStringTextSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitInterpolation(InterpolationSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitInterpolationAlignmentClause(InterpolationAlignmentClauseSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitInterpolationFormatClause(InterpolationFormatClauseSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitConstDirectiveTrivia(ConstDirectiveTriviaSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitIfDirectiveTrivia(IfDirectiveTriviaSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitElseDirectiveTrivia(ElseDirectiveTriviaSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitEndIfDirectiveTrivia(EndIfDirectiveTriviaSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitRegionDirectiveTrivia(RegionDirectiveTriviaSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitEndRegionDirectiveTrivia(EndRegionDirectiveTriviaSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitExternalSourceDirectiveTrivia(ExternalSourceDirectiveTriviaSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitEndExternalSourceDirectiveTrivia(EndExternalSourceDirectiveTriviaSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitExternalChecksumDirectiveTrivia(ExternalChecksumDirectiveTriviaSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitEnableWarningDirectiveTrivia(EnableWarningDirectiveTriviaSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitDisableWarningDirectiveTrivia(DisableWarningDirectiveTriviaSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitReferenceDirectiveTrivia(ReferenceDirectiveTriviaSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitBadDirectiveTrivia(BadDirectiveTriviaSyntax node) {
            throw new Exception("MustReview");
        }


    }
}
