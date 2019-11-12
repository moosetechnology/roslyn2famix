using Fame;
using System;
using FILE;
using Dynamix;
using FAMIX;
using System.Collections.Generic;

namespace Net
{
  [FamePackage("Net")]
  [FameDescription("Property")]
  public class Property : FAMIX.Attribute
  {
    [FameProperty(Name = "getter")]    
    public Net.PropertyAccessor getter { get; set; }
    
    [FameProperty(Name = "setter")]    
    public Net.PropertyAccessor setter { get; set; }
    
  }
}
