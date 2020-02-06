namespace Fame.Internal {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;

    public class Access {

        private Type elementType;
        private PropertyInfo propertyInfo;

        public static  Access CreateAccess(PropertyInfo propertyInfo) {
            Boolean IsAssignable = typeof(IEnumerable).GetTypeInfo().IsAssignableFrom(propertyInfo.PropertyType.GetTypeInfo());
            Boolean IsParametrizedType = propertyInfo.PropertyType.GenericTypeArguments.Length > 0; 
            if(IsAssignable && IsParametrizedType) {
                Type elementType = propertyInfo.PropertyType.GenericTypeArguments[0];
                 Type containerType = propertyInfo.PropertyType ;
                return new MultivaluedAccess(propertyInfo, elementType, containerType);
            }
            return new Access(propertyInfo, propertyInfo.PropertyType);
        }
        public Access(PropertyInfo propertyInfo, Type elementType) {
            this.elementType = elementType;
            this.propertyInfo = propertyInfo;
        }
        internal string GetName() {
            var name = propertyInfo.GetCustomAttribute<FamePropertyAttribute>();
            if (name != null && name.Name != null) return name.Name;
            return propertyInfo.Name;
        }
        internal Type GetElementType() {
            return elementType;
        }
        public object Read(object element) {
            return propertyInfo.GetValue(element, null);
        }
        public void Write<T>(object element, ICollection<T> values) {
            throw new System.NotImplementedException();
        }
        public void Write<T>(object element, T value) {
            throw new System.NotImplementedException();
        }
        public virtual bool IsMultivalued() {
            return false ;
        }
    }


    public class MultivaluedAccess : Access {
        private Type containerType;
        public MultivaluedAccess(PropertyInfo propertyInfo, Type elementType, Type containerType) : base(propertyInfo, elementType) {
            this.containerType = containerType;
        }
        public override bool IsMultivalued() {
            return true;
        }

    }
}