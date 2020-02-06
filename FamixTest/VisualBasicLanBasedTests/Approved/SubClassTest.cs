using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FamixTest.VisualBasic {
    [TestClass]
    public class SubClassTest : LanProjectVisualBasicLoader {

        [TestMethod]
        public void AllTheClassesSubClassDirectlyOrNotFromObject() {
            List<FAMIX.Class> classes = importer.AllElementsOfType<FAMIX.Class>().ToList();
            foreach (FAMIX.Class instance in classes) {
                  Assert.IsTrue(this.SubClassDirectlyOrNotFromObject(instance));
            }
        }
        [TestMethod]
        public void OnlyObjectHasNullForSuperClass() {
            List<FAMIX.Class> classes = ( 
               from famixClass
               in importer.AllElementsOfType<FAMIX.Class>().ToList()
               where famixClass.SuperInheritances.Count == 0 && famixClass.name.Equals("Object")
               select famixClass).ToList();
            Assert.AreEqual(classes.Count, 1);
        }







        private Boolean SubClassDirectlyOrNotFromObject(FAMIX.Class instance) {
            if (instance.isInterface || instance.isStub) return true; 
            if (instance.SuperInheritances.Count == 0) {
                return instance.name.Equals("Object");
            }
            return this.SubClassDirectlyOrNotFromObject((FAMIX.Class)instance.SuperInheritances.First().superclass);
        }
       
    }   
}
