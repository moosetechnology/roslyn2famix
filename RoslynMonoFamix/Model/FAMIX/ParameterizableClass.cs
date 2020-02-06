using Fame;
using System;
using FILE;
using Dynamix;
using FAMIX;
using System.Collections.Generic;

namespace FAMIX
{
  [FamePackage("FAMIX")]
  [FameDescription("ParameterizableClass")]
  public class ParameterizableClass : FAMIX.Class, FAMIX.ParameterizableEntity {
    private List<FAMIX.ArgumentType> argumentedTypes = new List<FAMIX.ArgumentType>();
    
    [FameProperty(Name = "parameterizedTypes",  Opposite = "parameterizableClass")]    
    public List <FAMIX.ArgumentType> ArgumentedTypes
    {
      get { return argumentedTypes; }
      set { argumentedTypes = value; }
    }
    public void AddArgumentType(FAMIX.ArgumentType one)
    {
      argumentedTypes.Add(one);
    }
    
    private List<FAMIX.ParameterType> parameters = new List<FAMIX.ParameterType>();
    
    [FameProperty(Name = "parameters")]    
    public List <FAMIX.ParameterType> TypeParameters
    {
      get { return parameters; }
      set { parameters = value; }
    }
    public void AddParameter(FAMIX.ParameterType one)
    {
      parameters.Add(one);
    }
    
  }
}
