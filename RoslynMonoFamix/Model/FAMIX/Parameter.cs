using Fame;
using System;
using FILE;
using Dynamix;
using FAMIX;
using System.Collections.Generic;

namespace FAMIX {
    [FamePackage("FAMIX")]
    [FameDescription("Parameter")]
    public class Parameter : FAMIX.StructuralEntity {

        [FameProperty(Name = "defaultValue")]
        public string defaultValue { set; get; }

        [FameProperty(Name = "referenceType")]
        public string referenceType { set; get; }

        [FameProperty(Name = "parentBehaviouralEntity", Opposite = "parameters")]
        public FAMIX.BehaviouralEntity parentBehaviouralEntity { get; set; }

    }
}
