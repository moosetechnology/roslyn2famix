using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
namespace FamixTest.VisualBasicUnitTest {
    [TestClass]
    public class InterfaceTest : VisualBasicUnitTest {


        #region SettingUp
        [TestInitialize]
        public void ParseEmptyClass() {
            this.Import(@"
                 Public Interface InterfaceExample
                    Sub InterfaceExampleSub(parameter As InterfaceExample)
                    Function InterfaceExampleFunction() As InterfaceExample
                End Interface
                Public Class ImplementingClassExample
                    Inherits ClassExampleSuperclass
                    Implements InterfaceExample
                End Class
                Public Class ClassExampleSuperclass
                End Class
            ");
        }
        #endregion
        public FAMIX.Class InterfaceExample() {
            return this.importer.AllElementsOfType<FAMIX.Class>().First(c => c.name == "InterfaceExample");
        }
        public FAMIX.Class ImplementingClassExample() {
            return this.importer.AllElementsOfType<FAMIX.Class>().First(c => c.name == "ImplementingClassExample");
        }
        public FAMIX.Class ClassExampleSuperclass() {
            return this.importer.AllElementsOfType<FAMIX.Class>().First(c => c.name == "ClassExampleSuperclass");
        }


        #region Visibility
        [TestMethod]
        public void InterfaceIsInterface() {
            Assert.IsTrue(InterfaceExample().isInterface);
        }
        [TestMethod]
        public void InterfaceIsPublic() {
            Assert.IsTrue(InterfaceExample().isPublic);
        }
        [TestMethod]
        public void HasTwoMethods() {
            Assert.AreEqual(InterfaceExample().Methods.Count(), 2);
        }
        [TestMethod]
        public void ImplementingClassExampleHasOneInheritances() {
            Assert.AreEqual(ImplementingClassExample().SuperInheritances.Count(), 1);
        }

        [TestMethod]
        public void ImplementingClassExampleHasOneImplementation() {
            Assert.AreEqual(ImplementingClassExample().Implements.Count(), 1);
        }

        [TestMethod]
        public void ImplementingClassExampleInheritsInterface() {
            Assert.AreEqual(ImplementingClassExample().Implements.First().ImplementedInterfaces.First(), InterfaceExample());
        }

        [TestMethod]
        public void ImplementingClassExampleInheritsSuperclass() {
            Assert.AreEqual(ImplementingClassExample().SuperInheritances.First().superclass, ClassExampleSuperclass());
        }

        #endregion


    }
}
