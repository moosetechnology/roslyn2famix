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

        internal FAMIX.Type EnsureBinaryType(INamedTypeSymbol superType) {

            string fullName = helper.FullTypeName(superType.OriginalDefinition);

            if (Types.has(fullName))
                return Types.Named(fullName);

            FAMIX.Type binaryType = RealEnsureType(superType.OriginalDefinition);
            var members = superType.GetMembers();
            foreach (var member in members) {

                if ( member is IPropertySymbol) {
                    var attr = EnsureProperty((IPropertySymbol)member) as FAMIX.Attribute;
                    binaryType.AddAttribute(attr);
                    attr.parentType = binaryType;
                }

                if (member is IFieldSymbol) {
                    var attr = EnsureField((IFieldSymbol)member) as FAMIX.Attribute;
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
                var superDuper = EnsureType(superType.BaseType.OriginalDefinition);
                LinkWithInheritance(binaryType, superDuper);
            }

            foreach (var inter in superType.AllInterfaces) {
                var superDuper = EnsureType(inter.OriginalDefinition);
                LinkWithInheritance(binaryType, superDuper);
            }
            return binaryType;
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
            FamixMethod.name = helper.FullMethodName(method);
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

            if (type.DeclaringSyntaxReferences.Length == 0) {
                return (FAMIX.Class)EnsureBinaryType(type);
            }

            string typeKind = helper.ResolveFAMIXTypeName(type).FullName;

            entity = this.CreateNewEntity<FAMIX.Class>(typeKind);
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
            entity = this.CreateNewEntity<FAMIX.ScopingEntity>(typeof(FAMIX.ScopingEntity).FullName);
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
            if (aType.DeclaringSyntaxReferences.Length == 0 && aType is INamedTypeSymbol)
                return EnsureBinaryType((INamedTypeSymbol)aType);
            return RealEnsureType(aType);
        }

        public FAMIX.Type RealEnsureType(ISymbol aType) {
            string typeKind = helper.ResolveFAMIXTypeName(aType).FullName;
            return this.EnsureType(aType, typeKind);
        }

            public FAMIX.Type EnsureType(ISymbol aType, String typeKind) {
            string fullName = helper.FullTypeName(aType);

            if (Types.has(fullName))
                return Types.Named(fullName);

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

        public FAMIX.EnumValue EnsureEnumField(IFieldSymbol symbol) {
            FAMIX.EnumValue value = this.CreateNewEntity<FAMIX.EnumValue>(typeof(FAMIX.EnumValue).FullName);
            value.accessibility = helper.AccessibilityName(symbol.DeclaredAccessibility);
            value.declaredType = this.EnsureType(symbol.Type);
            value.name = symbol.Name;

            return value;
        }

        public LocalVariable EnsureLocalVariable(ILocalSymbol symbol) {
            FAMIX.LocalVariable localvariable = this.CreateNewEntity<FAMIX.LocalVariable>(typeof(FAMIX.LocalVariable).FullName);
            localvariable.accessibility = helper.AccessibilityName(symbol.DeclaredAccessibility);
            localvariable.declaredType = this.EnsureType(symbol.Type);
            localvariable.name = symbol.Name;
            return localvariable;
        }
        public StructuralEntityGroup CreateStructuralEntityGroup() {
            return new StructuralEntityGroup();
        }

        public Property EnsureProperty(IPropertySymbol symbol) {
            Net.Property property = this.CreateNewEntity<Net.Property>(typeof(Net.Property).FullName); ;
            property.accessibility = helper.AccessibilityName(symbol.DeclaredAccessibility);
            property.declaredType = this.EnsureType(symbol.Type);
            property.name = helper.FullPropertyName(symbol);
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

        public ControlFlowStructure CreateControlStructure(String kind, BehaviouralEntity context) {
            FAMIX.ControlFlowStructure FamixEntity = this.CreateNewEntity<FAMIX.ControlFlowStructure>(typeof(FAMIX.ControlFlowStructure).FullName);
            FamixEntity.kind = kind;
            context.AddControlFlow(FamixEntity);
            FamixEntity.context = context;
            return FamixEntity;
        }

        public ThrownException CreateExceptionFor(ITypeSymbol symbolInfo, FAMIX.Method method) {
            FAMIX.ThrownException thrownException = this.CreateNewEntity<FAMIX.ThrownException>(typeof(FAMIX.ThrownException).FullName);
            thrownException.definingMethod = method;
            thrownException.exceptionClass = (FAMIX.Class)this.EnsureType(symbolInfo);
            return thrownException;
        }

        public AnnotationInstance EnsureAnnotationInstance(AttributeData symbol, FAMIX.NamedEntity currentContext) {
            FAMIX.AnnotationInstance instance;
            FAMIX.AnnotationType type;
            
            type = (FAMIX.AnnotationType)this.EnsureType(symbol.AttributeClass, typeof(FAMIX.AnnotationType).FullName);

            instance = this.CreateNewEntity<FAMIX.AnnotationInstance>(typeof(FAMIX.AnnotationInstance).FullName);

            type.AddInstance(instance);

            instance.annotationType = type;

            instance.annotatedEntity = currentContext;

            return instance;
        }


       public FAMIX.Attribute EnsureAnnotationTypeAttribute(FAMIX.AnnotationType annotationType, String attributeName) {
            var attributes = annotationType.Attributes.FindAll(a => a.name == attributeName);

            if (attributes.Count() > 0) return attributes[0];

            FAMIX.Attribute attr = this.CreateNewEntity<FAMIX.Attribute>(typeof(FAMIX.Attribute).FullName);
            attr.name = attributeName;
            annotationType.AddAttribute(attr);
            return attr;

       }
        public AnnotationInstanceAttribute EnsureAnnotationAttributeInto(AnnotationInstance annotationInstance, string attributeName, string attributeValue) {
            AnnotationInstanceAttribute attr;
            attr = this.CreateNewEntity<FAMIX.AnnotationInstanceAttribute>(typeof(FAMIX.AnnotationInstanceAttribute).FullName);
            
            attr.annotationTypeAttribute = this.EnsureAnnotationTypeAttribute(annotationInstance.annotationType, attributeName);
            attr.value = attributeValue;
            return attr;
        }
    }
}
