using Fame;
using Microsoft.CodeAnalysis;
using FAMIX;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using CSharp;

namespace RoslynMonoFamix.ModelBuilder {
    public class InCSharpImporter : AbstractModelBuilder  {

        public InCSharpImporter(Fame.Repository repository, string projectBaseFolder) : base(repository, projectBaseFolder) {

        }


        internal FAMIX.Type EnsureBinaryType(INamedTypeSymbol superType) {

            string fullName = helper.FullTypeName(superType.OriginalDefinition);

            if (Types.has(fullName))
                return Types.Named(fullName);

            FAMIX.Type binaryType = EnsureType(superType.OriginalDefinition);
            var members = superType.GetMembers();
            foreach (var member in members) {
                if (member is IFieldSymbol || member is IPropertySymbol) {
                    var attr = EnsureAttribute(member) as FAMIX.Attribute;
                    binaryType.AddAttribute(attr);
                    attr.parentType = binaryType;
                }
                if (member is IMethodSymbol) {
                    var methd = EnsureMethod(member as IMethodSymbol) as FAMIX.Method;
                    binaryType.AddMethod(methd);
                    methd.parentType = binaryType;
                }
            }

            if (superType.BaseType != null) {
                var superDuper = EnsureBinaryType(superType.BaseType.OriginalDefinition);
                LinkWithInheritance(binaryType, superDuper);
            }

            foreach (var inter in superType.AllInterfaces) {
                var superDuper = EnsureBinaryType(inter.OriginalDefinition);
                LinkWithInheritance(binaryType, superDuper);
            }
            return binaryType;
        }

        private void LinkWithInheritance(FAMIX.Type subClass, FAMIX.Type superClass) {
            Inheritance inheritance = CreateNewAssociation<Inheritance>(typeof(FAMIX.Inheritance).FullName);
            inheritance.subclass = subClass;
            inheritance.superclass = superClass;
            superClass.AddSubInheritance(inheritance);
            subClass.AddSuperInheritance(inheritance);
        }

        public CSharp.CSharpPropertyAccessor EnsureAccessor (IMethodSymbol MethodSelector) {
            return this.MethodNamedIfNone<CSharp.CSharpPropertyAccessor>(helper.FullMethodName(MethodSelector),
              () => this.CreateAndRegisterAccessor(MethodSelector)
          );

        }
        public CSharp.CSharpEvent EnsureEvent(IEventSymbol EventSelector) {
            return this.MethodNamedIfNone<CSharp.CSharpEvent>(helper.FullEventName(EventSelector),
               () => this.CreateAndRegisterEvent(EventSelector)
           );
        }

        private T MethodNamedIfNone<T> (String methodFullName, Func<T> IfNone ) where T : Method {
            if (Methods.has(methodFullName)) return Methods.Named(methodFullName) as T;
            return IfNone(); 
        }
        public Method EnsureMethod(IMethodSymbol aMethod) {
            return this.MethodNamedIfNone<Method>(helper.FullMethodName(aMethod),
                () => this.CreateAndRegisterMethod(aMethod)
            );
        }

        private void RegisterMethod(Method method, String methodFullName) {
            Methods.Add(methodFullName, method);
        }


        private Method CreateAndRegisterMethod(IMethodSymbol methodSymbol) {
            if (methodSymbol.MethodKind == MethodKind.PropertyGet || methodSymbol.MethodKind == MethodKind.PropertySet) {
                throw new System.Exception("The given Method Symbol belongs to a property Accessor! ");
            }
            Method method = repository.New<Method>(typeof(FAMIX.Method).FullName);
            method.isStub = true;
            method.name = methodSymbol.Name;
            method.signature = helper.MethodSignature(methodSymbol);
            this.RegisterMethod(method, helper.FullMethodName(methodSymbol));
            return method;
        }

        private CSharp.CSharpPropertyAccessor CreateAndRegisterAccessor(IMethodSymbol accessorSymbol) {
            if (accessorSymbol.MethodKind != MethodKind.PropertyGet && accessorSymbol.MethodKind != MethodKind.PropertySet) {
                throw new System.Exception("The given method symbol do not belongs to a property accessor!");
            }
            CSharp.CSharpPropertyAccessor Accessor = repository.New<CSharp.CSharpPropertyAccessor>(typeof(CSharp.CSharpPropertyAccessor).FullName) ;
            Accessor.isStub = true;
            Accessor.name = accessorSymbol.Name;
            Accessor.signature = helper.MethodSignature(accessorSymbol);
            this.RegisterMethod(Accessor, helper.FullMethodName(accessorSymbol));
            return Accessor;
        }

        private CSharp.CSharpEvent CreateAndRegisterEvent(IEventSymbol eventSymbol) {
            CSharp.CSharpEvent Event = repository.New<CSharp.CSharpEvent>(typeof(CSharp.CSharpEvent).FullName);
            Event.isStub = true;
            Event.name = eventSymbol.Name;
            Event.signature = helper.EventSignature(eventSymbol);
            this.RegisterMethod(Event, helper.FullEventName(eventSymbol));
            return Event;
        }


        private FAMIX.Namespace EnsureNamespace(INamespaceSymbol ns) {
            if (Namespaces.has(ns.Name))
                return Namespaces.Named(ns.Name);
            FAMIX.Namespace newNs = repository.New<FAMIX.Namespace>(typeof(FAMIX.Namespace).FullName);
            newNs.name = ns.Name;
            newNs.isStub = true;
            Namespaces.Add(ns.Name, newNs);
            return newNs;
        }
       
        public FAMIX.Type EnsureType(ISymbol aType) {

            string fullName = helper.FullTypeName(aType);

            if (Types.has(fullName))
                return Types.Named(fullName);

            string typeKind = helper.ResolveFAMIXTypeName(aType).FullName;

            FAMIX.Type type = repository.New<FAMIX.Type>(typeKind);
            type.isStub = true;

            Types.Add(fullName, type);

            if (typeKind.Equals(typeof(FAMIX.ParameterizedType).FullName)) {
                var parameterizedClass = EnsureType(aType.OriginalDefinition);
                (type as FAMIX.ParameterizedType).parameterizableClass = parameterizedClass as FAMIX.ParameterizableClass;
            }

            type.name = helper.TypeName(aType);
            if (aType.ContainingType != null) {
                var containingType = EnsureType(aType.ContainingType);
                type.container = containingType;
            } else
            if (aType.ContainingNamespace != null) {
                var ns = EnsureNamespace(aType.ContainingNamespace);
                type.container = ns;
            }

            return type;
        }

       

        public FAMIX.NamedEntity EnsureAttribute(ISymbol field) {
            String attributeFullName = FullFieldName(field);
            if (Attributes.has(attributeFullName))
                return Attributes.Named(attributeFullName);

            string attributeKind = ResolveAttritbuteTypeName(field);

            var attribute = repository.New<FAMIX.NamedEntity>(attributeKind);
            attribute.isStub = true;
            attribute.name = field.Name;
            if (attribute is StructuralEntity)
                Attributes.Add(attributeFullName, attribute as StructuralEntity);
            else
                Methods.Add(attributeFullName, attribute as CSharpEvent);
            return attribute;
        }

        private string ResolveAttritbuteTypeName(ISymbol field) {
            if (field.ContainingType != null) {
                if (helper.ResolveFAMIXTypeName(field.ContainingType).Equals(typeof(FAMIX.Enum)))
                    return typeof(FAMIX.EnumValue).FullName;
                if (helper.ResolveFAMIXTypeName(field.ContainingType).Equals(typeof(FAMIX.AnnotationType)))
                    return typeof(FAMIX.AnnotationTypeAttribute).FullName;
            }
            if (field is IEventSymbol)
                return typeof(CSharp.CSharpEvent).FullName;
            if (field is IPropertySymbol)
                return typeof(CSharp.CSharpProperty).FullName;
            return typeof(FAMIX.Attribute).FullName;
        }

        private String FullFieldName(ISymbol field) {
            var fullClassName = "";
            if (field.ContainingType != null) {
                fullClassName = helper.FullTypeName(field.ContainingType);
            }
            return fullClassName + "." + field.Name;
        }

        public T CreateNewAssociation<T>(String typeName) => repository.New<T>(typeName);

        internal T New<T>() {
            return repository.New<T>(typeof(T).FullName);
        }


        public void CreateSourceAnchor(SourcedEntity sourcedEntity, SyntaxNode node) {

            var lineSpan = node.SyntaxTree.GetLineSpan(node.Span);
            var relativePath = node.SyntaxTree.FilePath.Substring(projectBaseFolder.Length + 1);
            FileAnchor fileAnchor = CreateNewFileAnchor(node, ref lineSpan);
            var loc = lineSpan.EndLinePosition.Line - lineSpan.StartLinePosition.Line;
            if (sourcedEntity is BehaviouralEntity) (sourcedEntity as BehaviouralEntity).numberOfLinesOfCode = loc;


            sourcedEntity.sourceAnchor = fileAnchor;
            repository.Add(fileAnchor);
        }
        public void CreateSourceAnchor(FAMIX.Type sourcedEntity, ClassDeclarationSyntax node) {
            var lineSpan = node.SyntaxTree.GetLineSpan(node.Span);
            FileAnchor fileAnchor = CreateNewFileAnchor(node, ref lineSpan);
            var loc = lineSpan.EndLinePosition.Line - lineSpan.StartLinePosition.Line;

            if (node.Modifiers.ToFullString().Contains("partial")) {
                if (sourcedEntity.sourceAnchor == null) {
                    sourcedEntity.sourceAnchor = new MultipleFileAnchor();
                    repository.Add(sourcedEntity.sourceAnchor);
                }
                (sourcedEntity.sourceAnchor as MultipleFileAnchor).AddAllFile(fileAnchor);
            } else
                sourcedEntity.sourceAnchor = fileAnchor;
            (sourcedEntity as FAMIX.Type).numberOfLinesOfCode += loc;

            repository.Add(fileAnchor);
        }

        private FileAnchor CreateNewFileAnchor(SyntaxNode node, ref FileLinePositionSpan lineSpan) {
            var relativePath = node.SyntaxTree.FilePath.Substring(projectBaseFolder.Length + 1);
            FileAnchor fileAnchor = new FileAnchor
            {
                startLine = lineSpan.StartLinePosition.Line + 1,
                startColumn = lineSpan.StartLinePosition.Character,
                endLine = lineSpan.EndLinePosition.Line + 1,
                endColumn = lineSpan.EndLinePosition.Character + 1,
                fileName = relativePath
            };
            return fileAnchor;
        }
    }
}
