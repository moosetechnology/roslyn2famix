using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Net;
using FAMIX;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.VisualBasic;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;

namespace RoslynMonoFamix.ModelBuilder {
    public class VisualBasicModelBuilder : AbstractModelBuilder {
        FAMIX.ScopingEntity entity;
        public VisualBasicModelBuilder(Fame.Repository repository, string projectBaseFolder) : base(repository, projectBaseFolder) {

        }


        public FAMIX.Parameter EnsureParameterInMethod(IParameterSymbol parameterSymbol, Method CurrentMethod) {
            if (CurrentMethod.Parameters.Any(p => p.name == parameterSymbol.Name)) {
                return CurrentMethod.Parameters.Find(p => p.name == parameterSymbol.Name);
            }
            FAMIX.Parameter parameter = this.CreateParameter(parameterSymbol);
            CurrentMethod.AddParameter(parameter);
            return parameter;
        }

        public FAMIX.Class EnsureClass(INamedTypeSymbol type) {

            string classname = helper.FullTypeName(type);
            return Types.EntityNamedIfNone<FAMIX.Class>(classname,
                 () => { return this.CreateNewClass(type); });
        }

        public FAMIX.Class EnsureIterface(INamedTypeSymbol node) {
            string classname = helper.FullTypeName(node);
            return Types.EntityNamedIfNone<FAMIX.Class>(classname,
                 () => { return this.CreateNewInterface(node); });
        }
        public FAMIX.Namespace EnsureNamespace(INamespaceSymbol ns) {
            return Namespaces.EntityNamedIfNone<FAMIX.Namespace>(ns.Name,
                 () => { return this.CreateNamespace(ns); });
        }

        public Method EnsureMethod(IMethodSymbol method) {
            string name = helper.FullMethodName(method);
            return Methods.EntityNamedIfNone<FAMIX.Method>(name,
                 () => { return this.CreateNewMethod(method); });
        }

        public Net.PropertyAccessor EnsureAccessorInto(IMethodSymbol method, Net.Property Owner) {
            string name = helper.FullMethodName(method);
            return Methods.EntityNamedIfNone<Net.PropertyAccessor>(name,
                 () => { return this.CreateNewAccessor(method, Owner); });
        }


        private FAMIX.Parameter CreateParameter(IParameterSymbol parameterSymbol) {
            FAMIX.Parameter parameter = this.CreateNewEntity<FAMIX.Parameter>(typeof(FAMIX.Parameter).FullName);
            parameter.name = parameterSymbol.Name;
            parameter.Modifiers = (parameterSymbol.CustomModifiers.Select(p => p.Modifier.Name)
                                     .Concat(
                                            parameterSymbol.RefCustomModifiers.Select(p => p.Modifier.Name)
                                      )
                                   ).ToList();
            parameter.referenceType = helper.RefKindName(parameterSymbol.RefKind);
            if (parameterSymbol.IsParams) throw new System.Exception("Should cehck this");
            if (parameterSymbol.HasExplicitDefaultValue) {
                if (parameterSymbol.ExplicitDefaultValue == null) {
                    if (parameterSymbol.Language == LanguageNames.CSharp) {
                        parameter.defaultValue = "null";
                    } else {
                        parameter.defaultValue = "Nothing";
                    }
                } else {
                    parameter.defaultValue = parameterSymbol.ExplicitDefaultValue.ToString();
                }
            }
            return parameter;

        }



        private void ConfigureMethodWith(FAMIX.Method FamixMethod, IMethodSymbol method) {
            FamixMethod.isStub = true;
            FamixMethod.name = method.Name;
            FamixMethod.Modifiers = method.RefCustomModifiers.Select(p => p.Modifier.Name).ToList();
            FamixMethod.signature = helper.MethodSignature(method);
            FamixMethod.accessibility = helper.AccessibilityName(method.DeclaredAccessibility);
            FamixMethod.kind = helper.MethodKindName(method.MethodKind);
        }


        private Method CreateNewMethod(IMethodSymbol method) {
            Method FamixMethod;
            if (method.TypeParameters.Count() == 0) {
                FamixMethod = this.CreateNewEntity<FAMIX.Method>(typeof(FAMIX.Method).FullName);
            } else {
                FamixMethod = this.CreateNewEntity<FAMIX.ParameterizableMethod>(typeof(FAMIX.ParameterizableMethod).FullName);
            }
            ConfigureMethodWith(FamixMethod, method);
            return FamixMethod;
        }

        private Net.PropertyAccessor CreateNewAccessor(IMethodSymbol method, Net.Property Owner) {
            Net.PropertyAccessor NetAccessor = this.CreateNewEntity<Net.PropertyAccessor>(typeof(Net.PropertyAccessor).FullName);
            ConfigureMethodWith(NetAccessor, method);
            NetAccessor.property = Owner;
            if (method.MethodKind == MethodKind.PropertySet) {
                Owner.setter = NetAccessor;
            } else if (method.MethodKind == MethodKind.PropertyGet) {
                Owner.getter = NetAccessor;
            } else {
                throw new System.Exception("Unexpected non accessor method in accessor statement ");
            }
            return NetAccessor;
        }

        private Method CreateNewConstructor(IMethodSymbol method) {
            Method FamixMethod = this.CreateNewMethod(method);
            FamixMethod.isConstructor = true;
            return FamixMethod;
        }

        public Method EnsureConstructor(IMethodSymbol method) {
            string name = helper.FullTypeName(method);
            return Methods.EntityNamedIfNone<FAMIX.Method>(name,
                 () => { return this.CreateNewConstructor(method); });
        }

        private FAMIX.Class CreateNewClass(INamedTypeSymbol type) {
            FAMIX.Class entity;
            if (type.TypeParameters.Count() == 0) {
                entity = this.CreateNewEntity<FAMIX.Class>(typeof(FAMIX.Class).FullName);
            } else {
                entity = this.CreateNewEntity<FAMIX.ParameterizableClass>(typeof(FAMIX.ParameterizableClass).FullName);
            }
            entity.name = helper.FullTypeName(type);
            entity.isAbstract = type.IsAbstract;
            entity.isFinal = type.IsSealed;
            entity.accessibility = helper.AccessibilityName(type.DeclaredAccessibility);
            return entity;
        }

        public TypeBoundary CreateTypeBoundary(FAMIX.ParameterType type) {
            FAMIX.TypeBoundary boundary = this.CreateNewEntity<FAMIX.TypeBoundary>(typeof(FAMIX.TypeBoundary).FullName);
            boundary.ParameterType = type;
            type.AddBoundary(boundary);
            return boundary;
        }

        public FAMIX.Inheritance CreateInheritanceFor(FAMIX.Type inheritingClass) {
            FAMIX.Inheritance inheritance = this.CreateNewEntity<FAMIX.Inheritance>(typeof(FAMIX.Inheritance).FullName);
            inheritance.subclass = inheritingClass;
            return inheritance;

        }
        public FAMIX.Implements CreateImplementsFor(FAMIX.Type implementingClass) {
            FAMIX.Implements implements = this.CreateNewEntity<FAMIX.Implements>(typeof(FAMIX.Implements).FullName);
            implements.ImplementingClass = implementingClass;
            return implements;
        }
        public ScopingEntity CreateScopingEntity(CompilationUnitSyntax node) {
            if (entity != null) throw new System.Exception(" NOOOOOOO ");
            entity = new FAMIX.ScopingEntity();
            return entity;
        }

        private Namespace CreateNamespace(INamespaceSymbol ns) {
            FAMIX.Namespace entity = this.CreateNewEntity<FAMIX.Namespace>(typeof(FAMIX.Namespace).FullName);
            entity.name = ns.Name;
            return entity;
        }

        private FAMIX.Class CreateNewInterface(INamedTypeSymbol node) {
            FAMIX.Class entity = this.CreateNewEntity<FAMIX.Class>(typeof(FAMIX.Class).FullName);
            entity.isInterface = true;
            entity.isAbstract = true;
            entity.name = helper.FullTypeName(node);
            entity.isFinal = node.IsSealed;
            entity.accessibility = helper.AccessibilityName(node.DeclaredAccessibility);            
            return entity;
        }

        public FAMIX.Type EnsureType(ISymbol aType) {
            string fullName = helper.FullTypeName(aType);

            if (Types.has(fullName))
                return Types.Named(fullName);

            string typeKind = helper.ResolveFAMIXTypeName(aType).FullName;

            FAMIX.Type type = repository.New<FAMIX.Type>(typeKind);
            type.isStub = true;

            Types.Add(fullName, type);

            if (typeKind.Equals(typeof(FAMIX.ArgumentType).FullName)) {
                var parameterizedClass = EnsureType(aType.OriginalDefinition);
                (type as FAMIX.ArgumentType).parameterizableClass = parameterizedClass as FAMIX.ParameterizableClass;
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

        public FAMIX.Attribute EnsureField(IFieldSymbol symbol) {
            FAMIX.Attribute attribute = this.CreateNewEntity<FAMIX.Attribute>(typeof(FAMIX.Attribute).FullName);
            attribute.accessibility = helper.AccessibilityName(symbol.DeclaredAccessibility);
            attribute.declaredType = this.EnsureType(symbol.Type);
            attribute.name = symbol.Name;

            return attribute;
        }

        public AttributeGroup CreateStructuralEntityGroup() {
            return new AttributeGroup();
        }

        public Property EnsureProperty(IPropertySymbol symbol) {
            Net.Property property = this.CreateNewEntity<Net.Property>(typeof(Net.Property).FullName); ;
            property.accessibility = helper.AccessibilityName(symbol.DeclaredAccessibility);
            property.declaredType = this.EnsureType(symbol.Type);
            property.name = symbol.Name;
            return property;
        }

        public ParameterType EnsureParametrizedTypeInto(FAMIX.ParameterizableEntity parametrizableEntity, ITypeParameterSymbol typeParameterSymbol) {
            FAMIX.ParameterType parameter = (FAMIX.ParameterType)this.EnsureType(typeParameterSymbol);
            parameter.name = typeParameterSymbol.Name;
            if (typeParameterSymbol.TypeParameterKind != TypeParameterKind.Type && typeParameterSymbol.TypeParameterKind != TypeParameterKind.Method) {
                throw new System.Exception(" Unexpectd kind of type parameter! ");
            }

            parametrizableEntity.AddParameter(parameter);


            return parameter;
        }

    }
}
