using Fame;
using System;
using FILE;
using Dynamix;
using FAMIX;
using System.Collections.Generic;


/*****************************************************************************
 * This class is mean to represent the argument-type given to bind a parameter-type. 
 * In famix it seems to be poorly named as 'ParameterizedType' what is confusing 
 * from the point of view of the terminology.
 * 
 *****************************************************************************/
namespace FAMIX {
    [FamePackage("FAMIX")]
    [FameDescription("ParameterizedType")]
    public class ArgumentType : FAMIX.Type {
        private List<FAMIX.Type> arguments = new List<FAMIX.Type>();
        [FameProperty(Name = "arguments", Opposite = "argumentsInParameterizedTypes")]
        public List<FAMIX.Type> Arguments {
            get { return arguments; }
            set { arguments = value; }
        }
        public void AddArgument(FAMIX.Type one) {
            arguments.Add(one);
        }
        [FameProperty(Name = "parameterizableClass", Opposite = "parameterizedTypes")]
        public FAMIX.ParameterizableClass parameterizableClass { get; set; }

    }
}
