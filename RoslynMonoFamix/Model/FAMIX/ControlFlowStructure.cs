using Fame;
using System;
using FILE;
using Dynamix;
using FAMIX;
using System.Collections.Generic;

namespace FAMIX {
    [FamePackage("FAMIX")]
    [FameDescription("ControlFlowStructure")]
    public class ControlFlowStructure : FAMIX.BehaviouralEntity {
        [FameProperty(Name = "kind")]
        public String kind;
        [FameProperty(Name = "condition")]
        public String Condition;
        [FameProperty(Name = "context", Opposite = "controlFlowStructures")]
        public BehaviouralEntity context;
    }
}
