namespace Fame.Fm3 {
    using System;
    using System.Collections.Generic;
    using Common;

    /// <summary>
    /// Holds meta-information about packaging.
    /// <p> 
    /// subclasses NamedElement with attributes 
    /// </p> 
    /// <ul> 
    /// <li>Class<code> classes</code>(multivalued, opposite Class.package)</li> 
    /// <li>Property<code> extensions</code>(multivalued, opposite Property.package)
    /// </li> 
    /// </ul> 
    /// <p> 
    /// with these constraints
    /// </p> 
    /// <ul> 
    /// <li> <code>owner</code> is nil</li> 
    /// <li> <code>classes</code> must have unique names</li> 
    /// </ul> 
    ///  
    /// @author Adrian Kuhn
    /// </summary>
    [FamePackage("FM3")]
    [FameDescription("Package")]
    public class PackageDescription : Element {
        public static readonly string NAME = "FM3.Package";
        [FameProperty(Opposite = "package")]
        public ISet<MetaDescription> Elements { get; }

        [FameProperty(Opposite = "package")]
        public ISet<PropertyDescription> Extensions { get; set; }

        public PackageDescription() : this(string.Empty) {
        }

        public PackageDescription(string name) : base(name) {
            Elements = new HashSet<MetaDescription>();
            Extensions = new HashSet<PropertyDescription>();
        }

        public void AddElement(MetaDescription instance) {
            if (Elements.Add(instance)) {
                instance.Package = this;
            }
        }

        public override void CheckContraints(Warnings warnings) {
            if (!MetaRepository.IsValidName(Name))
                warnings.Add("Name must be alphanumeric", this);
        }

        public override Element GetOwner() {
            return null;
        }
    }
}
