using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FAMIX;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.VisualBasic;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;

namespace RoslynMonoFamix.ModelBuilder {
    class VisualBasicModelBuilder : AbstractModelBuilder {
        FAMIX.ScopingEntity entity;
        public VisualBasicModelBuilder(Fame.Repository repository, string projectBaseFolder) : base(repository, projectBaseFolder) {

        }
        public FAMIX.Inheritance CreateInheritanceFor(FAMIX.Class inheritingClass) {
            FAMIX.Inheritance inheritance = this.CreateNewEntity<FAMIX.Inheritance>(typeof(FAMIX.Inheritance).FullName);
            inheritance.subclass = inheritingClass;
            return inheritance;
        }


        protected T EnsureNamespaceNamed<T>(string name, Func<T> func) where T : FAMIX.Namespace {
            T type;
            if (Types.has(name)) {
                return (T)Namespaces.Named(name);
            }
            type = func();
            Namespaces.Add(name, type);
            return type;
        }
        internal ScopingEntity CreateScopingEntity(CompilationUnitSyntax node) {
            if (entity != null) throw new System.Exception (" NOOOOOOO ");
             entity = new FAMIX.ScopingEntity();
            return entity;
        }

        public FAMIX.Class EnsureClass(ClassStatementSyntax node) {
            ClassBlockSyntax block = (ClassBlockSyntax) node.Parent;
            string classname = helper.FullTypeName(model.GetDeclaredSymbol(block));
           return Types.EnsureEntityNamed<FAMIX.Class>(classname,
                () => { return this.CreateNewClass(node); } );
        }

        public FAMIX.Class EnsureIterface(InterfaceStatementSyntax node) {
            string classname = helper.FullTypeName(model.GetDeclaredSymbol(node));
            return Types.EnsureEntityNamed<FAMIX.Class>(classname,
                 () => { return this.CreateNewInterface(node); });
        }
        public FAMIX.Namespace EnsureNamespace(NamespaceStatementSyntax node) {
            SyntaxNode Current = node.Parent;
            string NamespaceName = node.Name.ToString(); 
            while (Current != null) {
                if (Current is NamespaceStatementSyntax) {
                    NamespaceName = ((NamespaceStatementSyntax)Current).Name.ToFullString() + "." + NamespaceName;
                 }
                Current = Current.Parent; 
            }
            return Namespaces.EnsureEntityNamed<FAMIX.Namespace>(NamespaceName,
                 () => { return this.CreateNamespace(NamespaceName); });
        }

        public Method EnsureMethod(MethodStatementSyntax node) {
            string classname = helper.FullTypeName(model.GetDeclaredSymbol(node));
            return Methods.EnsureEntityNamed<FAMIX.Method>(classname,
                 () => { return this.CreateNewMethod(node); });
        }

        private Method CreateNewMethod(MethodStatementSyntax node) {
            var methodSymbol = model.GetDeclaredSymbol(node);
                Method method = this.CreateNewEntity<FAMIX.Method>(typeof(FAMIX.Method).FullName); 
                method.isStub = true;
                method.name = methodSymbol.Name;
                method.signature = helper.MethodSignature(methodSymbol);
            return method;
        }
        private Method CreateNewConstructor(SubNewStatementSyntax node) {
            var methodSymbol = model.GetDeclaredSymbol(node);
            Method method = this.CreateNewEntity<FAMIX.Method>(typeof(FAMIX.Method).FullName);
            method.isStub = true;
            method.name = methodSymbol.Name;
            method.signature = helper.MethodSignature(methodSymbol);
            method.isConstructor = true;
            return method;
        }

        public Method EnsureConstructor(SubNewStatementSyntax node) {
            string classname = helper.FullTypeName(model.GetDeclaredSymbol(node));
            return Methods.EnsureEntityNamed<FAMIX.Method>(classname,
                 () => { return this.CreateNewConstructor(node); });
        }

        private FAMIX.Class CreateNewClass(ClassStatementSyntax node) {
            ClassBlockSyntax block = (ClassBlockSyntax) node.Parent;
            FAMIX.Class entity = this.CreateNewEntity<FAMIX.Class>(typeof(FAMIX.Class).FullName);
            var symbol = model.GetDeclaredSymbol(node);
            entity.name = helper.FullTypeName(symbol);
            entity.isAbstract = node.Modifiers.Any(SyntaxKind.MustInheritKeyword);
            entity.isShadow = node.Modifiers.Any(SyntaxKind.ShadowsKeyword);
            entity.isFinal = node.Modifiers.Any(SyntaxKind.NotInheritableKeyword);
            entity.isProtected = node.Modifiers.Any(SyntaxKind.ProtectedKeyword);
            entity.isPublic = node.Modifiers.Any(SyntaxKind.PublicKeyword);
            entity.isPrivate = node.Modifiers.Any(SyntaxKind.PrivateKeyword);
            return entity;
        }


        private Namespace CreateNamespace(string NamespaceName) {
            FAMIX.Namespace entity = this.CreateNewEntity<FAMIX.Namespace>(typeof(FAMIX.Namespace).FullName);
            entity.name = NamespaceName;
            return entity;
        }

        private FAMIX.Class CreateNewInterface(InterfaceStatementSyntax node) {
            FAMIX.Class entity = this.CreateNewEntity<FAMIX.Class>(helper.FullTypeName(model.GetDeclaredSymbol(node)));
            entity.isInterface = true;
            entity.isAbstract = true;
            return entity;
        }

    }
}
