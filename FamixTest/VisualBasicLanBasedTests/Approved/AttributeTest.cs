using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
namespace FamixTest.VisualBasic {
    [TestClass]
    public class AttributeTest : LanProjectVisualBasicLoader {
        [TestMethod]
        public void ThereIsATotalOf22AttributesInTheWholeProject() {
            Assert.AreEqual(importer.AllElementsOfType<FAMIX.Attribute>().Count(), 735);
        }
        
        [TestMethod]
        public void TheAttributeGotFromAttributesAreTheSameAsThoseGotFromTheClasses() {
            List<FAMIX.Class> classes = (
               from famixClass
               in importer.AllElementsOfType<FAMIX.Class>().ToList()
               where famixClass.container is FAMIX.ScopingEntity && famixClass.container.name == null && !famixClass.isInterface
               select famixClass).ToList();
            foreach(FAMIX.Class c in classes) {
                foreach(FAMIX.Attribute attribute in c.Attributes) {
                    Assert.AreEqual(attribute, importer.AllElementsOfType<FAMIX.Attribute>().First(n => n.name == (attribute.name)));
                }
            }
        }
        [TestMethod]
        public void AttributeAccessorsAreDetected() {
            
            var attribute = importer.AllElementsOfType<Net.Property>().First(n => n.name == "VBLanLibrary.Node.Name");
            Assert.IsTrue (attribute.setter != null);
            Assert.IsTrue(attribute.getter != null);
        }
        [TestMethod]
        public void AttributeReadOnlyAttributesHaveOnlyGetter() {
            var attribute = importer.AllElementsOfType<Net.Property>().First(n => n.name == "VBLanLibrary.WorkStation.Type");
            Assert.IsTrue(attribute.getter != null);
            Assert.IsTrue(attribute.setter == null);

            
        }
        [TestMethod]
        public void AttributeWriteOnlyAttributesHaveOnlySetter() {
            var attribute = importer.AllElementsOfType<Net.Property>().First(n => n.name == "VBLanLibrary.WorkStation.Conf");
            Assert.IsTrue(attribute.getter == null);
            Assert.IsTrue(attribute.setter != null);
        }
    }
}
