using Fame;
using System;
using FILE;
using Dynamix;
using FAMIX;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace FAMIX {
    [FamePackage("FAMIX")]
    [FameDescription("Implements")]
    public class Implements : FAMIX.Entity, FAMIX.ITyped {
        [FameProperty(Name = "implementingClass", Opposite = "interfaces")]
        protected FAMIX.Type _implementingClass;
        public FAMIX.Type ImplementingClass {
            get {
                return this._implementingClass;
            }
            set {
                if (this._implementingClass != null) {
                    throw new System.Exception(" parameterType is already setted ");
                }
                this._implementingClass = value;
            }
        }
        [FameProperty(Name = "implementedInterfaces", Opposite = "implementors")]
        protected List<FAMIX.Type> _implementedInterfaces;
        public List<FAMIX.Type> ImplementedInterfaces {
            get {
                return this._implementedInterfaces;
            }
            set {
                if (this._implementedInterfaces != null) {
                    throw new System.Exception(" boundaryType is already setted ");
                }
                this._implementedInterfaces = value;
            }
        }

        public void SetImplementedInterface(List<FAMIX.Type> givenInterface) {
            this.ImplementedInterfaces = givenInterface;
            this.ImplementingClass.AddImplements(this);
            foreach (FAMIX.Type type in this.ImplementedInterfaces) {
                type.AddImplementor(this);
            }
        }

        public TypingContext TypingContext(ISymbol symbol) {
   
            throw new System.Exception("Error. Implements cannot cope with this polymorphism ");
        }

        public ImplementsTypingContext ImplementsTypingContext() {
            return (ImplementsTypingContext)FAMIX.TypingContext.Implements(this);
        }
    }
}
