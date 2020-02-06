using System;
using System.Runtime.Serialization;

namespace Fame {
    [Serializable]
    public class ClassNotMetadescribedException : Exception {
        public ClassNotMetadescribedException(string message) : base(message) {
        }
    }
}