using System;
using System.Runtime.Serialization;
using Fame.Fm3;

namespace Fame {
    [Serializable]
    internal class ElementInPropertyNotMetadescribed : Exception {
        private PropertyDescription property;
        public PropertyDescription Property () {
            return property;
        }
        public ElementInPropertyNotMetadescribed(PropertyDescription property) {
            this.property = property;
        }
    }
}