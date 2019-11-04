using System;
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
            this.PopContext();
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
            throw new Exception("MustReview");
        }
        public override void VisitImportsStatement(ImportsStatementSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitSimpleImportsClause(SimpleImportsClauseSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitImportAliasClause(ImportAliasClauseSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitXmlNamespaceImportsClause(XmlNamespaceImportsClauseSyntax node) {
            throw new Exception("MustReview");
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
            throw new Exception("MustReview");
        }
        public override void VisitStructureBlock(StructureBlockSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitInterfaceBlock(InterfaceBlockSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitClassBlock(ClassBlockSyntax node) {
            base.VisitClassBlock(node);
        }
        public override void VisitEnumBlock(EnumBlockSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitInheritsStatement(InheritsStatementSyntax node) {
            FAMIX.Inheritance inheritance = importer.CreateInheritanceFor(this.CurrentContext<FAMIX.Class>());
            this.PushContext(inheritance);
            base.VisitInheritsStatement(node);
            this.PopContext();
        }
        public override void VisitImplementsStatement(ImplementsStatementSyntax node) {
            FAMIX.Inheritance inheritance = importer.CreateInheritanceFor(this.CurrentContext<FAMIX.Class>());
            this.PushContext(inheritance);
            base.VisitImplementsStatement(node);
            this.PopContext();
        }
        public override void VisitModuleStatement(ModuleStatementSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitStructureStatement(StructureStatementSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitInterfaceStatement(InterfaceStatementSyntax node) {
            FAMIX.Class FamixClass = importer.EnsureIterface(node);
            this.PushContext(FamixClass);
            base.VisitInterfaceStatement(node);
        }
        public override void VisitClassStatement(ClassStatementSyntax node) {
            FAMIX.Class FamixClass = importer.EnsureClass(importer.model.GetDeclaredSymbol(node));

            FamixClass.isShadow = node.Modifiers.Any(SyntaxKind.ShadowsKeyword);
            FamixClass.isPrivate = node.Modifiers.Any(SyntaxKind.PrivateKeyword);
            FamixClass.isPublic = node.Modifiers.Any(SyntaxKind.PublicKeyword); ;
            FamixClass.isProtected = node.Modifiers.Any(SyntaxKind.ProtectedKeyword); ;

            FAMIX.IAddType AGoodSuperContext = (FAMIX.IAddType)this.CurrentContext<FAMIX.Entity>();
            AGoodSuperContext.AddType(FamixClass);
            this.PushContext(FamixClass);
            base.VisitClassStatement(node);
        }
        public override void VisitEnumStatement(EnumStatementSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitTypeParameterList(TypeParameterListSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitTypeParameter(TypeParameterSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitTypeParameterSingleConstraintClause(TypeParameterSingleConstraintClauseSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitTypeParameterMultipleConstraintClause(TypeParameterMultipleConstraintClauseSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitSpecialConstraint(SpecialConstraintSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitTypeConstraint(TypeConstraintSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitEnumMemberDeclaration(EnumMemberDeclarationSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitMethodBlock(MethodBlockSyntax node) {
            base.VisitMethodBlock(node);
        }
        public override void VisitConstructorBlock(ConstructorBlockSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitOperatorBlock(OperatorBlockSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitAccessorBlock(AccessorBlockSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitPropertyBlock(PropertyBlockSyntax node) {
            throw new Exception("MustReview");
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
            FamixMethod.isPublic = node.Modifiers.Any(SyntaxKind.PublicKeyword); ;
            FamixMethod.isProtected = node.Modifiers.Any(SyntaxKind.ProtectedKeyword); ;

            FAMIX.Type AGoodSuperContext = this.CurrentContext<FAMIX.Type>();
            AGoodSuperContext.AddMethod(FamixMethod);
            this.PushContext(FamixMethod);
            base.VisitMethodStatement(node);
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
            throw new Exception("MustReview");
        }
        public override void VisitAccessorStatement(AccessorStatementSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitImplementsClause(ImplementsClauseSyntax node) {
            throw new Exception("MustReview");
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
            throw new Exception("MustReview");
        }
        public override void VisitVariableDeclarator(VariableDeclaratorSyntax node) {
            throw new Exception("MustReview");
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
            throw new Exception("MustReview");
        }
        public override void VisitParameter(ParameterSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitModifiedIdentifier(ModifiedIdentifierSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitArrayRankSpecifier(ArrayRankSpecifierSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitAttributeList(AttributeListSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitAttribute(AttributeSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitAttributeTarget(AttributeTargetSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitAttributesStatement(AttributesStatementSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitExpressionStatement(ExpressionStatementSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitPrintStatement(PrintStatementSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitWhileBlock(WhileBlockSyntax node) {
            throw new Exception("MustReview");
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
            throw new Exception("MustReview");
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
            throw new Exception("MustReview");
        }
        public override void VisitSingleLineIfStatement(SingleLineIfStatementSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitSingleLineElseClause(SingleLineElseClauseSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitMultiLineIfBlock(MultiLineIfBlockSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitIfStatement(IfStatementSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitElseIfBlock(ElseIfBlockSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitElseIfStatement(ElseIfStatementSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitElseBlock(ElseBlockSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitElseStatement(ElseStatementSyntax node) {
            throw new Exception("MustReview");
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
            throw new Exception("MustReview");
        }
        public override void VisitDoStatement(DoStatementSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitLoopStatement(LoopStatementSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitWhileOrUntilClause(WhileOrUntilClauseSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitWhileStatement(WhileStatementSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitForBlock(ForBlockSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitForEachBlock(ForEachBlockSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitForStatement(ForStatementSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitForStepClause(ForStepClauseSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitForEachStatement(ForEachStatementSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitNextStatement(NextStatementSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitUsingStatement(UsingStatementSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitThrowStatement(ThrowStatementSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitAssignmentStatement(AssignmentStatementSyntax node) {
            throw new Exception("MustReview");
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
            throw new Exception("MustReview");
        }
        public override void VisitParenthesizedExpression(ParenthesizedExpressionSyntax node) {
            throw new Exception("MustReview");
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
            throw new Exception("MustReview");
        }
        public override void VisitMyBaseExpression(MyBaseExpressionSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitMyClassExpression(MyClassExpressionSyntax node) {
            throw new Exception("MustReview");
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
            throw new Exception("MustReview");
        }
        public override void VisitXmlMemberAccessExpression(XmlMemberAccessExpressionSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitInvocationExpression(InvocationExpressionSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitObjectCreationExpression(ObjectCreationExpressionSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitAnonymousObjectCreationExpression(AnonymousObjectCreationExpressionSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitArrayCreationExpression(ArrayCreationExpressionSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitCollectionInitializer(CollectionInitializerSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitCTypeExpression(CTypeExpressionSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitDirectCastExpression(DirectCastExpressionSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitTryCastExpression(TryCastExpressionSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitPredefinedCastExpression(PredefinedCastExpressionSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitBinaryExpression(BinaryExpressionSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitUnaryExpression(UnaryExpressionSyntax node) {
            throw new Exception("MustReview");
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
            throw new Exception("MustReview");
        }
        public override void VisitOmittedArgument(OmittedArgumentSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitSimpleArgument(SimpleArgumentSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitNameColonEquals(NameColonEqualsSyntax node) {
            throw new Exception("MustReview");
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
            FAMIX.Type type = importer.EnsureType(typing.RelatedSymbol);
            typing.SetType(type);
            base.VisitPredefinedType(node);
        }
        public override void VisitIdentifierName(IdentifierNameSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitGenericName(GenericNameSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitQualifiedName(QualifiedNameSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitGlobalName(GlobalNameSyntax node) {
            throw new Exception("MustReview");
        }
        public override void VisitTypeArgumentList(TypeArgumentListSyntax node) {
            throw new Exception("MustReview");
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
