using Fame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMIX {
    [FamePackage("FAMIX")]
    [FameDescription("ParameterizableMethod")]
    public class ParameterizableMethod : Method, FAMIX.ParameterizableEntity {
        private List<FAMIX.ArgumentType> argumentedTypes = new List<FAMIX.ArgumentType>();

        [FameProperty(Name = "parameterizedTypes", Opposite = "parameterizableClass")]
        public List<FAMIX.ArgumentType> ArgumentedTypes {
            get { return argumentedTypes; }
            set { argumentedTypes = value; }
        }
        public void AddArgumentType(FAMIX.ArgumentType one) {
            argumentedTypes.Add(one);
        }

        private List<FAMIX.ParameterType> typeParameters = new List<FAMIX.ParameterType>();

        [FameProperty(Name = "parameters")]
        public List<FAMIX.ParameterType> TypeParameters {
            get { return typeParameters; }
            set { typeParameters = value; }
        }
        public void AddParameter(FAMIX.ParameterType one) {
            typeParameters.Add(one);
        }
    }
}
