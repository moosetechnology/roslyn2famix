using Fame;
using System;
using FILE;
using Dynamix;
using FAMIX;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace FAMIX {
    [FamePackage("FAMIX")]
    [FameDescription("Inheritance")]
    public class Inheritance : FAMIX.Association, FAMIX.ITyped {
        [FameProperty(Name = "subclass", Opposite = "superInheritances")]
        protected FAMIX.Type __subclass;
        public FAMIX.Type subclass {
            get {
                return this.__subclass;
            }
            set {
                if (this.__subclass != null ) { 
                    throw new System.Exception(" Subclass is already setted "); 
                }
                this.__subclass = value;
            }
        }

        [FameProperty(Name = "superclass", Opposite = "subInheritances")]
        protected FAMIX.Type __superclass;

        public FAMIX.Type superclass {
            get {
                return this.__superclass;
            }
            set {
                if (this.__superclass != null) {
                    throw new System.Exception(" superclass is already setted ");
                }
                this.__superclass = value;
            }
        }

        public void SetSuperType (FAMIX.Type superType) {
            this.superclass = (FAMIX.Class) superType;
            this.subclass.AddSuperInheritance(this);
            this.superclass.AddSubInheritance(this);

            
        }

        public TypingContext TypingContext(ISymbol symbol) {
            if (symbol is INamedTypeSymbol) return this.TypingContext((INamedTypeSymbol)symbol);
            throw new System.Exception("Error");
        }

        public TypingContext TypingContext(INamedTypeSymbol symbol) {
            return FAMIX.TypingContext.Inheritance(this,symbol);
        }
    }
}
