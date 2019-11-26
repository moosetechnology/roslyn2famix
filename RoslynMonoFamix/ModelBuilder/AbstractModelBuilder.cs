using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynMonoFamix.ModelBuilder {

    public class AbstractModelBuilder {
        public NamedEntityAccumulator<FAMIX.Namespace> Namespaces { get; set; }
        public NamedEntityAccumulator<FAMIX.Type> Types { get; set; }
        public NamedEntityAccumulator<FAMIX.Method> Methods { get; set; }
        public NamedEntityAccumulator<FAMIX.StructuralEntity> Attributes { get; set; }

        public SemanticModel model { get; set; }
        protected Fame.Repository repository;
        protected ImportingHelper helper = new ImportingHelper();
        protected string projectBaseFolder;

        public AbstractModelBuilder(Fame.Repository repository, string projectBaseFolder) {

            this.repository = repository;
            this.Methods = new NamedEntityAccumulator<FAMIX.Method>();
            this.Types = new NamedEntityAccumulator<FAMIX.Type>();
            this.Namespaces = new NamedEntityAccumulator<FAMIX.Namespace>();
            this.Attributes = new NamedEntityAccumulator<FAMIX.StructuralEntity>();
            this.projectBaseFolder = projectBaseFolder;
        }
        public T CreateNewEntity<T>(String typeName) {
            return repository.New<T>(typeName);
        }
        private void LinkWithInheritance(FAMIX.Class subClass, FAMIX.Class superClass) {
            FAMIX.Inheritance inheritance = CreateNewEntity<FAMIX.Inheritance>(typeof(FAMIX.Inheritance).FullName);
            inheritance.subclass = subClass;
            inheritance.superclass = superClass;
            superClass.AddSubInheritance(inheritance);
            subClass.AddSuperInheritance(inheritance);
        }
        public IEnumerable<T> AllElementsOfType<T>() {
            return repository.GetElements().OfType<T>();
        }

        public void CreateSourceAnchor(FAMIX.SourcedEntity sourcedEntity, SyntaxNode node) {

            var lineSpan = node.SyntaxTree.GetLineSpan(node.Span);

            FAMIX.FileAnchor fileAnchor = CreateNewFileAnchor(node, ref lineSpan);
            
            var loc = lineSpan.EndLinePosition.Line - lineSpan.StartLinePosition.Line;
            if (sourcedEntity is FAMIX.BehaviouralEntity) (sourcedEntity as FAMIX.BehaviouralEntity).numberOfLinesOfCode = loc;


            sourcedEntity.sourceAnchor = fileAnchor;
            repository.Add(fileAnchor);
        }

        public void CreateSourceAnchor(FAMIX.Type sourcedEntity, ClassDeclarationSyntax node) {
            var lineSpan = node.SyntaxTree.GetLineSpan(node.Span);
            FAMIX.FileAnchor fileAnchor = CreateNewFileAnchor(node, ref lineSpan);
            var loc = lineSpan.EndLinePosition.Line - lineSpan.StartLinePosition.Line;

            if (node.Modifiers.ToFullString().Contains("partial")) {
                if (sourcedEntity.sourceAnchor == null) {
                    sourcedEntity.sourceAnchor = new FAMIX.MultipleFileAnchor();
                    repository.Add(sourcedEntity.sourceAnchor);
                }
                (sourcedEntity.sourceAnchor as FAMIX.MultipleFileAnchor).AddAllFile(fileAnchor);
            } else
                sourcedEntity.sourceAnchor = fileAnchor;
            (sourcedEntity as FAMIX.Type).numberOfLinesOfCode += loc;

            repository.Add(fileAnchor);
        }

        private FAMIX.FileAnchor CreateNewFileAnchor(SyntaxNode node, ref FileLinePositionSpan lineSpan) {
            String relativePath = "";

            if (node.SyntaxTree.FilePath.Length > 0) {
                relativePath = node.SyntaxTree.FilePath.Substring(projectBaseFolder.Length + 1);
            }


            FAMIX.FileAnchor fileAnchor = new FAMIX.FileAnchor
            {
                startLine = lineSpan.StartLinePosition.Line + 1,
                startColumn = lineSpan.StartLinePosition.Character,
                endLine = lineSpan.EndLinePosition.Line + 1,
                endColumn = lineSpan.EndLinePosition.Character + 1,
                fileName = relativePath
            };
            return fileAnchor;
        }

        public T CreateNewAssociation<T>(String typeName) => repository.New<T>(typeName);
        public void AddMethodCall(SyntaxNode node, FAMIX.BehaviouralEntity clientMethod, FAMIX.Method referencedEntity) {
            FAMIX.Invocation invocation = this.CreateNewAssociation<FAMIX.Invocation>("FAMIX.Invocation");
            invocation.sender = clientMethod;
            invocation.AddCandidate(referencedEntity);
            invocation.signature = node.Span.ToString();
            //invocation.receiver = referencedEntity;
            clientMethod.AddOutgoingInvocation(invocation);
            referencedEntity.AddIncomingInvocation(invocation);
            this.CreateSourceAnchor(invocation, node);
        }


    }
}
