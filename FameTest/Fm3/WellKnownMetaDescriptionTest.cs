using System;
using Fame;
using Fame.Fm3;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FameTest {
    [TestClass]
    public class WellKnownMetaDescriptionTest {

        [TestMethod]
        public void TestObjectIsRoot() {
            Assert.IsTrue(MetaDescription.OBJECT.IsRoot());
        }
        [TestMethod]
        public void TestObjectHasNoParent() {
            Assert.IsFalse(MetaDescription.OBJECT.HasSuperClass());
        }

        [TestMethod]
        public void TestPrimitiveObjectGetsObject() {
            Assert.AreEqual(MetaDescription.OBJECT, MetaDescription.PrimitiveNamed("Object"));
        }
        [TestMethod]
        public void TestPrimitiveStringGetsString() {
            Assert.AreEqual(MetaDescription.STRING, MetaDescription.PrimitiveNamed("String"));
        }
        [TestMethod]
        public void TestPrimitiveBooleanGetsBoolean() {
            Assert.AreEqual(MetaDescription.BOOLEAN, MetaDescription.PrimitiveNamed("Boolean"));
        }
        [TestMethod]
        public void TestPrimitiveNumberGetsNumber() {
            Assert.AreEqual(MetaDescription.NUMBER, MetaDescription.PrimitiveNamed("Number"));
        }
        [TestMethod]
        public void TestPrimitiveDateGetsDate() {
            Assert.AreEqual(MetaDescription.DATE, MetaDescription.PrimitiveNamed("Date"));
        }

        [TestMethod]
        [ExpectedException(typeof(ClassNotMetadescribedException))]
        public void TestPrimitiveUnexistantTypeGetsException() {
            MetaDescription.PrimitiveNamed("Unexistant");
        }

        [TestMethod]
        public void TestObjectIsNotPrimitive() {
            Assert.IsFalse(MetaDescription.OBJECT.IsPrimitive());
        }
        [TestMethod]
        public void TestStringIsPrimitive() {
            Assert.IsTrue(MetaDescription.STRING.IsPrimitive());
        }
        [TestMethod]
        public void TestBooleanIsPrimitive() {
            Assert.IsTrue(MetaDescription.BOOLEAN.IsPrimitive());
        }
        [TestMethod]
        public void TestNumberIsPrimitive() {
            Assert.IsTrue(MetaDescription.NUMBER.IsPrimitive());
        }
        [TestMethod]
        public void TestDateIsPrimitive() {
            Assert.IsTrue(MetaDescription.DATE.IsPrimitive());
        }

      
    }
}
