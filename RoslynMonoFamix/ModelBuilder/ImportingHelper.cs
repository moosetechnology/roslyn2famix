using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;

namespace RoslynMonoFamix.ModelBuilder {


    public class ImportingHelper {
        protected Dictionary<string, System.Type> typeNameMap = new Dictionary<string, System.Type>()
            {
                { "Struct", typeof(CSharp.CSharpStruct) },
                { "Class", typeof(FAMIX.Class) },
                { "Interface", typeof(FAMIX.Class) },
                { "Delegate", typeof(CSharp.Delegate) },
                { "TypeParameter", typeof(FAMIX.ParameterType)},
                { "Enum", typeof(FAMIX.Enum) },
            };


        public System.Type ResolveFAMIXTypeName(ISymbol aType) {
            System.Type result = typeof(FAMIX.Class);
            if (aType is ITypeSymbol) typeNameMap.TryGetValue(((ITypeSymbol)aType).TypeKind.ToString(), out result);
            if (aType is INamedTypeSymbol) {
                var superType = (aType as INamedTypeSymbol).BaseType;
                while (superType != null) {
                    if (superType.Name.Equals("Attribute") && superType.ContainingNamespace.Name.Equals("System"))
                        return typeof(FAMIX.AnnotationType);
                    superType = superType.BaseType;
                }

                if ((aType as INamedTypeSymbol).IsGenericType) {
                    if ((aType as INamedTypeSymbol).IsDefinition)
                        result = typeof(FAMIX.ParameterizableClass);
                    else
                        result = typeof(FAMIX.ParameterizedType);
                }
            }
            if (aType is IArrayTypeSymbol)
                return ResolveFAMIXTypeName((aType as IArrayTypeSymbol).ElementType);
            if (aType is IPointerTypeSymbol)
                return ResolveFAMIXTypeName((aType as IPointerTypeSymbol).PointedAtType);
            if (result == null) {
                Console.WriteLine("Could not resolve type for  " + aType);
                if (aType.ContainingAssembly != null) Console.WriteLine("Containing Assembly " + aType.ContainingAssembly);
                result = typeof(FAMIX.Class);
            }
            return result;
        }
        public String FullTypeName(ISymbol aType) {
            var symbolDisplayFormat = new SymbolDisplayFormat(typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces, genericsOptions: SymbolDisplayGenericsOptions.IncludeTypeParameters);
            string fullyQualifiedName = aType.ToDisplayString(symbolDisplayFormat);
            if (aType is INamedTypeSymbol) {
                if ((aType as INamedTypeSymbol).IsGenericType && (aType as INamedTypeSymbol).IsDefinition)
                    fullyQualifiedName = "DefinitionOf" + fullyQualifiedName;
            }
            return fullyQualifiedName;
        }

        public String TypeName(ISymbol aType) {
            var symbolDisplayFormat = new SymbolDisplayFormat(typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameOnly, genericsOptions: SymbolDisplayGenericsOptions.IncludeTypeParameters);
            string fullyQualifiedName = aType.ToDisplayString(symbolDisplayFormat);
            return fullyQualifiedName;
        }
        public String FullMethodName(IMethodSymbol method) {
            return FullTypeName(method.ContainingType) + "." + MethodSignature(method);
        }

        public String MethodSignature(IMethodSymbol method) {
            var parameters = "";
            parameters += "(";
            foreach (var par in (method as IMethodSymbol).Parameters)
                parameters += par.Type.Name + ",";
            if (parameters.LastIndexOf(",") > 0)
                parameters = parameters.Substring(0, parameters.Length - 1);
            parameters += ")";
            return method.Name + parameters;
        }

        public String EventSignature (IEventSymbol Event) {
            return Event.Name;
        }
        public String FullEventName(IEventSymbol Event) {
            return FullTypeName(Event.ContainingType) + "." + EventSignature(Event);
        }
       
    }
}
