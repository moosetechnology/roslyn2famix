using Fame;
using System;
using FILE;
using Dynamix;
using FAMIX;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace FAMIX {
    [FamePackage("FAMIX")]
    [FameDescription("StructuralEntity")]
    public class StructuralEntity : FAMIX.LeafEntity, FAMIX.ITyped {

        [FameProperty(Name = "declaredType", Opposite = "structuresWithDeclaredType")]
        public FAMIX.Type declaredType { get; set; }

        private List<FAMIX.Access> incomingAccesses = new List<FAMIX.Access>();

        [FameProperty(Name = "incomingAccesses", Opposite = "variable")]
        public List<FAMIX.Access> IncomingAccesses {
            get { return incomingAccesses; }
            set { incomingAccesses = value; }
        }
        public void AddIncomingAccess(FAMIX.Access one) {
            incomingAccesses.Add(one);
        }

        public FAMIX.TypingContext TypingContext(ISymbol symbol) {
            return FAMIX.TypingContext.StructuralEntity(this, symbol);
        }
    }
}
