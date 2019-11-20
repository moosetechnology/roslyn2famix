using Fame;
using System;
using FILE;
using Dynamix;
using FAMIX;
using System.Collections.Generic;

namespace FAMIX {

    public interface ParameterizableEntity {
        [FameProperty(Name = "parameters")]
        List<FAMIX.ParameterType> TypeParameters {
            get;
            set;
        }
        List<FAMIX.ArgumentType> ArgumentedTypes {
            get;
            set;
        }
        void AddArgumentType(FAMIX.ArgumentType one);        
        void AddParameter(FAMIX.ParameterType one);
    }
}
