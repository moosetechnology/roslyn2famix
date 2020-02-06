using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FamixTest.VisualBasic {
    [TestClass]
    public class InterfaceDigestingTests : LanProjectVisualBasicLoader {
        [TestMethod]
        public void VBLanInterfacesHaveBeingDigested () {
            List<FAMIX.Class> classes = (
               from famixClass
               in importer.AllElementsOfType<FAMIX.Class>().ToList()
               where famixClass.container != null && famixClass.container.name.Equals("VBLanLibrary") && famixClass.isInterface
               select famixClass).ToList();
            Assert.AreEqual(classes.Count, 1);
            Assert.AreEqual(classes.First().name, "IPrinter" );
        }

        [TestMethod]
        public void VBLanInterfacesHaveImplementors() {
            List<FAMIX.Class> classes = (
               from famixClass
               in importer.AllElementsOfType<FAMIX.Class>().ToList()
               where famixClass.container != null && famixClass.container.name.Equals("VBLanLibrary") && famixClass.isInterface
               select famixClass).ToList();
            FAMIX.Class Interface = classes.First();

            Assert.AreEqual(Interface.SubInheritances.Count, 1);
            Assert.AreEqual(Interface.SubInheritances.First().subclass.name, "XPrinter");
        }


        [TestMethod]
        public void ThirdPartiesInterfacesHaveBeingDigested() {
            List<FAMIX.Class> classes = (
               from famixClass
               in importer.AllElementsOfType<FAMIX.Class>().ToList()
               where (famixClass.container == null || (famixClass.container != null && !famixClass.container.name.Equals("VBLanLibrary"))) && famixClass.isInterface
               select famixClass).ToList();
            Assert.AreEqual(classes.Count, 1); // IList at least should be accessible 
        }
    }
}
