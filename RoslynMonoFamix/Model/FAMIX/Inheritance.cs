using Fame;
using System;
using FILE;
using Dynamix;
using FAMIX;
using System.Collections.Generic;

namespace FAMIX {
    [FamePackage("FAMIX")]
    [FameDescription("Inheritance")]
    public class Inheritance : FAMIX.Association, FAMIX.IAddType {
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

        public void AddType (FAMIX.Type superType) {
            FAMIX.Class superclass = (FAMIX.Class) superType;
            Inheritance copy = new Inheritance();
            copy.subclass = this.subclass;
            copy.superclass = superclass;
            copy.subclass.AddSuperInheritance(copy);
            copy.superclass.AddSubInheritance(copy);
        }
    }
}
