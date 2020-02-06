using Fame.Fm3;
using Fame.Internal;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fame {
    public class MetaRepository : Repository {
        private Dictionary<String, Element> bindings = new Dictionary<string, Element>();
        private Dictionary<Type, MetaDescription> classes = new Dictionary<Type, MetaDescription>();
        public MetaRepository(MetaRepository metaRepository) : base(metaRepository) { }
        internal static MetaRepository CreateFM3() {
            MetaRepository mse = new MetaRepository(null);
            mse.RegisterType(typeof(MetaDescription));
            mse.RegisterType(typeof(Element));
            mse.RegisterType(typeof(PackageDescription));
            mse.RegisterType(typeof(PropertyDescription));
            mse.AddAll(mse.bindings.Values.ToList<object>());
            return mse;
        }
        public static bool IsValidName(string name) {
            throw new System.NotImplementedException();
        }
        public void RegisterType(Type type) {
            if (!classes.ContainsKey(type)) {
                MetaDescriptionFactory factory = new MetaDescriptionFactory(type, this);
                MetaDescription instance = factory.CreateInstance();
                if (instance != null) {
                    classes.Add(type, instance);
                    factory.InitializeInstance();
                    this.Add(instance);
                }
            }
        }
        public override void Add(object element) {
            if (element is Element) {
                if (!bindings.ContainsKey(((Element)element).Fullname)) {
                    this.bindings.Add(((Element)element).Fullname, (Element)element);
                }
            } else
                throw new Exception("tried to add a non element");
        }
        internal MetaDescription GetDescription(Type type) {
            classes.TryGetValue(type, out MetaDescription value);
            if (value == null)
                throw new ClassNotMetadescribedException("The type " + type.ToString() + " is not a valid type in this MetaRepository");
            return value;
        }
        public Element Get(String fullName) {
            bindings.TryGetValue(fullName, out Element e);
            return e;
        }
        public PackageDescription InitializePackageNamed(String name) {
            if (!bindings.TryGetValue(name, out Element description)) {
                description = new PackageDescription(name);
                bindings.Add(name, description);
            }
            return (PackageDescription)description;
        }
    }
}