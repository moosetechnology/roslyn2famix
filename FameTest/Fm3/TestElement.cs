using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fame;
using Fame.Common;
using Fame.Fm3;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace FameTest.Fm3 {
    internal class TestableElement : Element {
        public Element owner;
        public TestableElement (String str, Element owner) : base(str) {
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

    [TestClass]
    class TestElement {
        Element unownedElement;
        Element ownedElement;
        [TestInitialize]
        public void SetUp () {
            unownedElement = new TestableElement("Parent-Element", null);
            ownedElement = new TestableElement("SubElement", unownedElement);
        }
        [TestMethod]
        public void TestUnownedElementNameIsTheConstructedName () {
            Assert.AreEqual("Parent-Element", unownedElement.Name);
        }
        [TestMethod]
        public void TestOwnedElementNameIsTheConstructedName() {
            Assert.AreEqual("SubElement", ownedElement.Name);
        }
        [TestMethod]
        public void TestUnownedElementFullNameIsTheName() {
            Assert.AreEqual(unownedElement.Name, unownedElement.Fullname);
        }
        [TestMethod]
        public void TestOwnedElementFullNameIsTheParentNamePlusName() {
            Assert.AreEqual(unownedElement.Name + '.' + ownedElement.Name, ownedElement.Fullname);
        }
   
    }
}
