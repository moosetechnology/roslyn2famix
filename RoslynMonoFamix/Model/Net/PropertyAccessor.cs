using Fame;
using System;
using FILE;
using Dynamix;
using FAMIX;
using System.Collections.Generic;

namespace Net {
    [FamePackage("Net")]
    [FameDescription("PropertyAccessor")]
    public class PropertyAccessor : FAMIX.Method {
        [FameProperty(Name = "property")]
        public Net.Property property { get; set; }

    }
}
