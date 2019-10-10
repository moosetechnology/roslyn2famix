using System;
using Fame.Fm3;

namespace Fame.Internal {
    internal class UnknownElementError : Exception {
        private Element description;
        private object element;
        public Element Description() {
            return description;
        }
        public object Element() {
            return description;
        }
        public UnknownElementError(Element description, object element) {
            this.description = description;
            this.element = element;
        }
    }
}