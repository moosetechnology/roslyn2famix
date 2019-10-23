using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
namespace FamixTest.VisualBasic {
    [TestClass]
    public class AttributeTest : LanProjectVisualBasicLoader {
        [TestMethod]
        public void ThereIsATotalOf13AttributesInTheWholeProject() {
            Assert.AreEqual(importer.Attributes.size(), 21);
        }
        
        [TestMethod]
        public void TheAttributeGotFromAttributesAreTheSameAsThoseGotFromTheClasses() {
            List<FAMIX.Class> classes = (
               from famixClass
               in importer.AllElementsOfType<FAMIX.Class>().ToList()
               where famixClass.container != null && famixClass.container.name.Equals("VBLanLibrary") && !famixClass.isInterface
               select famixClass).ToList();
            foreach(FAMIX.Class c in classes) {
                foreach(FAMIX.Attribute attribute in c.Attributes) {
                    Assert.AreEqual(attribute, importer.Attributes.Named("VBLanLibrary."+ attribute.parentType.name+"."+ attribute.name));
                }
            }
        }
        [TestMethod]
        public void AttributeAccessorsAreDetected() {
            var attribute = importer.Methods.Named("VBLanLibrary.Node.Name");
            Assert.AreEqual(2, attribute.Accesses.Count);
        }
        [TestMethod]
        public void AttributeReadOnlyAttributesHaveOnlyGetter() {
            var attribute = importer.Methods.Named("VBLanLibrary.WorkStation.Type");
            Assert.AreEqual(1, attribute.Accesses.Count);
            Assert.IsFalse(importer.Methods.Named("VBLanLibrary.WorkStation.Type").Accesses.First().isWrite);
        }
        [TestMethod]
        public void AttributeWriteOnlyAttributesHaveOnlySetter() {
            var attribute = importer.Methods.Named("VBLanLibrary.WorkStation.Conf");
            Assert.AreEqual(1, attribute.Accesses.Count);
            Assert.IsTrue(importer.Methods.Named("VBLanLibrary.WorkStation.Type").Accesses.First().isWrite);
        }
    }
}
