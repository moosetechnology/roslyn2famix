using System;
using Fame;
using Fame.Fm3;
using Fame.Internal;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FameTest.Fm3 {
    [TestClass]
    public class CustomClassHierarchClassMetaDescriptionsTests {
        MetaDescription subject;


        [FameDescription("FantasticAnimal")]
        abstract class FantasticAnimal {
            public int arbitraryProperty {
                set; get;
            } = 3;
            public int nonPropertyField = 5;
            public abstract void doSomethingFantastic();
        }
        [FameDescription("Unicorn")]
        class Unicorn : FantasticAnimal {
            public new float arbitraryProperty {
                set; get;
            } = 3;
            public string nonPropertyField2 = "5";
            public override void doSomethingFantastic() {
                this.doSomethingMonoHornid();
            }
            private void doSomethingMonoHornid() { }
        }

        [TestInitialize]
        public void SetUp() {
            MetaDescriptionFactory factory;
            Tower tower = new Tower();
            factory = new MetaDescriptionFactory(typeof(Unicorn), tower.metamodel);
            subject = factory.CreateInstance();
            factory.InitializeInstance();
        }

        [TestMethod]
        public void TestMetaDescriptionNameIsUnicorn() {
            Assert.AreEqual("Unicorn", subject.Name);
        }
        [TestMethod]
        public void TestMetaDescriptionFullNameIsName() {
            Assert.AreEqual(subject.Fullname, "FameTest.Fm3." + subject.Name);
        }

        [TestMethod]
        public void TestMetaDescriptionIsNotRoot() {
            Assert.IsFalse(subject.IsRoot());
        }
        [TestMethod]
        public void TestMetaDescriptionSuperClassIsFantasticAnimal() {
            Assert.AreEqual(subject.SuperClass.Fullname, "FameTest.Fm3.FantasticAnimal");
        }
        [TestMethod]
        public void TestUnicornHasThreeAttributes() {
            Assert.AreEqual(subject.Attributes.Count, 3);
        }
        [TestMethod]
        public void TestFantasticAnimalHasTwoAttributes() {
            Assert.AreEqual(subject.SuperClass.Attributes.Count, 2);
        }
        [TestMethod]
        public void TestFantasticAnimalHasFirstPropertyNamedArbitraryProperty() {
            Assert.AreEqual(subject.SuperClass.Attributes[0].Fullname, subject.SuperClass.Fullname + ".arbitraryProperty");
        }
        [TestMethod]
        public void TestFantasticAnimalHasSecondPropertyNamedNonPropertyField() {
            Assert.AreEqual(subject.SuperClass.Attributes[1].Fullname, subject.SuperClass.Fullname + ".nonPropertyField");
        }

        [TestMethod]
        public void TestUnicornHasFirstPropertyNamedArbitraryProperty() {
            Assert.AreEqual(subject.Attributes[0].Fullname, subject.Fullname + ".arbitraryProperty");
        }
        [TestMethod]
        public void TestUnicornHasSecondPropertyNamedNonPropertyField() {
            Assert.AreEqual(subject.Attributes[1].Fullname, subject.Fullname + ".nonPropertyField");
        }
        [TestMethod]
        public void TestUnicornHasThirdPropertyNamedNonPropertyField() {
            Assert.AreEqual(subject.Attributes[2].Fullname, subject.Fullname + ".nonPropertyField2");
        }
        
    }
}
