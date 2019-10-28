using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;

namespace RoslynMonoFamix.InCSharp {

    public class ImportingHelper {
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
