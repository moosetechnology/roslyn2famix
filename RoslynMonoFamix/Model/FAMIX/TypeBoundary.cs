using Fame;
using System;
using FILE;
using Dynamix;
using FAMIX;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace FAMIX {
    [FamePackage("FAMIX")]
    [FameDescription("TypeBoundary")]
    public class TypeBoundary : FAMIX.Entity, FAMIX.ITyped {
        [FameProperty(Name = "parameterType", Opposite = "boundaries")]
        protected FAMIX.ParameterType _parameterType;
        public FAMIX.ParameterType ParameterType { 
            get {
                return this._parameterType;
            }
            set {
                if (this._parameterType != null ) { 
                    throw new System.Exception(" parameterType is already setted "); 
                }
                this._parameterType = value;
            }
        }
        [FameProperty(Name = "boundaryType")]
        protected FAMIX.Type _boundaryType;
        public FAMIX.Type BoundaryType {
            get {
                return this._boundaryType;
            }
            set {
                if (this._boundaryType != null) {
                    throw new System.Exception(" boundaryType is already setted ");
                }
                this._boundaryType = value;
            }
        }




        public TypingContext TypingContext(ISymbol symbol) {
            if (symbol is INamedTypeSymbol) return this.TypingContext((INamedTypeSymbol)symbol);
            throw new System.Exception("Error");
        }

        public TypingContext TypingContext(INamedTypeSymbol symbol) {
            return FAMIX.TypingContext.TypeBoundary(this,symbol);
        }
    }
}
