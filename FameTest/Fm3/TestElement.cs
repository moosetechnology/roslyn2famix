using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Fame;
using Fame.Common;
using Fame.Fm3;
namespace FameTest {
    [TestClass]
    public class TestElement {


        public class TestableElement : Element {
            public Element owner;
            public TestableElement(String str, Element owner) : base(str) {
                this.owner = owner;
            }
            public override void CheckContraints(Warnings warnings) {
                // Nothing to do
            }
            [FamePropertyWithDerived]
            public override Element GetOwner() {
                return owner;
            }
        }

        public Element unOwnedElement;
        public Element ownedElement;

        [TestInitialize]
        public void SetUp() {
            unOwnedElement = new TestableElement("Parent-Element", null);
            ownedElement = new TestableElement("SubElement", unOwnedElement);
        }

        [TestMethod]
        public void TestUnownedElementNameIsTheConstructedName() {
            Assert.AreEqual("Parent-Element", unOwnedElement.Name);
        }
        [TestMethod]
        public void TestOwnedElementNameIsTheConstructedName() {
            Assert.AreEqual("SubElement", ownedElement.Name);
        }
        [TestMethod]
        public void TestUnownedElementFullNameIsTheName() {
            Assert.AreEqual(unOwnedElement.Name, unOwnedElement.Fullname);
        }
        [TestMethod]
        public void TestOwnedElementFullNameIsTheParentNamePlusName() {
            Assert.AreEqual(unOwnedElement.Name + '.' + ownedElement.Name, ownedElement.Fullname);
        }
    }
}
