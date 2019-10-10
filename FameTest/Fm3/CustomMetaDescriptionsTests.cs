using System;
using Fame;
using Fame.Fm3;
using Fame.Internal;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FameTest.Fm3 {
    [TestClass]
    public class CustomMetaDescriptionsTests {

        MetaDescription subject;


        [FameDescription("Dragon")]
        class Dragon {
            public int arbitraryProperty {
                set; get;
            } = 3;
            public int nonPropertyField = 5;
        }

        [TestInitialize]
        public void SetUp() {
            MetaDescriptionFactory factory;
            Tower tower = new Tower();
            factory = new MetaDescriptionFactory(typeof(Dragon), tower.metamodel);
            subject = factory.CreateInstance();
            factory.InitializeInstance();
        }

        [TestMethod]
        public void TestMetaDescriptionNameIsDragon() {
            Assert.AreEqual("Dragon", subject.Name);
        }
        [TestMethod]
        public void TestMetaDescriptionFullNameIsName() {
            Assert.AreEqual(subject.Fullname, "FameTest.Fm3."+subject.Name);
        }

        [TestMethod]
        public void TestMetaDescriptionIsNotRoot() {
            Assert.IsFalse(subject.IsRoot());
        }
        [TestMethod]
        public void TestMetaDescriptionSuperClassIsObject() {
            Assert.AreEqual(subject.SuperClass.Fullname, "System.Object");
        }
        [TestMethod]
        public void TestDragonHasTwoAttributes() {
            Assert.AreEqual(subject.Attributes.Count,  2);
        }
        [TestMethod]
        public void TestDragonHasFirstPropertyNamedArbitraryProperty () {
            Assert.AreEqual(subject.Attributes[0].Fullname, subject.Fullname+ ".arbitraryProperty");
        }
        [TestMethod]
        public void TestDragonHasSecondPropertyNamedNonPropertyField() {
            Assert.AreEqual(subject.Attributes[1].Fullname, subject.Fullname + ".nonPropertyField");
        }

    }
}
