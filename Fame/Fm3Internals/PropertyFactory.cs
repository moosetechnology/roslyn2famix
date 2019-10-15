using Fame.Fm3;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fame.Internal {
   public class PropertyFactory {
        private Access access;
        private PropertyDescription instance;
        private MetaRepository repository;

        public PropertyFactory(Access access, MetaRepository metaRepository) {
            this.access = access;
            this.repository = metaRepository;
        }
        public PropertyDescription CreateInstance() {
            instance = new PropertyDescription(access.GetName());
            return instance;
        }
        public void InitializeInstance() {
            this.InitializeType();
            this.InitializeMultivalued();
            this.InitializeAccessors();
        }
        private void InitializeMultivalued() {
            instance.IsMultivalued = access.IsMultivalued();
        }
        private void InitializeAccessors() {
            instance.Access = access;
        }
        private void InitializeType() {
            repository.RegisterType(access.GetElementType());
            MetaDescription type = repository.GetDescription(access.GetElementType());
            instance.Type = type;
        }
    }
}
