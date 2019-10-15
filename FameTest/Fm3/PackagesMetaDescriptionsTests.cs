using System;
using Fame;
using Fame.Fm3;
using Fame.Internal;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FameTest.Fm3 {
    [TestClass]
    public class PackagesMetaDescriptionsTests {
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
        public void TestPackageHasName() {
            Assert.AreEqual("FameTest.Fm3", subject.Package.Fullname);
        }
        [TestMethod]
        public void TestPackageContainsTwoElements() {
            Assert.AreEqual(2, subject.Package.Elements.Count);
        }
        [TestMethod]
        public void TestPackageContainsUnicorn() {
            Assert.IsTrue(subject.Package.Elements.Contains(subject));
        }
        [TestMethod]
        public void TestPackageContainsFantasticAnimal() {
            Assert.IsTrue(subject.Package.Elements.Contains(subject.SuperClass));
        }


    }
}
