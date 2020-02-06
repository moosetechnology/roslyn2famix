using Fame;
using System;
using FILE;
using Dynamix;
using FAMIX;
using System.Collections.Generic;

namespace FAMIX
{
    [FamePackage("FAMIX")]
    [FameDescription("ParameterType")]
    public class ParameterType : FAMIX.Type {
        [FameProperty(Name = "boundaries", Opposite = "parameterType")]
        public List<TypeBoundary> boundaries = new List<TypeBoundary>();
        public void AddBoundary(TypeBoundary boundary) {
            this.boundaries.Add(boundary);
        }
    }
}
