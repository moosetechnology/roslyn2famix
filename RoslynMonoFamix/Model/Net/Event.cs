using Fame;
using System;
using FILE;
using Dynamix;
using FAMIX;
using System.Collections.Generic;

namespace Net
{
  [FamePackage("Net")]
  [FameDescription("Event")]
  public class Event : FAMIX.Method
  {
    [FameProperty(Name = "addMethod")]    
    public FAMIX.Method addMethod { get; set; }
    
    [FameProperty(Name = "removeMethod")]    
    public FAMIX.Method removeMethod { get; set; }
    
  }
}
