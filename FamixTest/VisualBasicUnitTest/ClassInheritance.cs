using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;


namespace FamixTest.VisualBasicUnitTest {
    [TestClass]
    public class ClassInheritance : VisualBasicUnitTest {


        #region SettingUp
        public void ParseSimpleInheritance() {
            this.Import(@"
                    Class ExampleSuperclass 
                    End Class
                    Class ExampleSubclass
                        Inherits ExampleSuperclass
                    End Class
                    
            ");
        }
        public void ParseMustInheritNotInheritableSimpleInheritance() {
            this.Import(@"
                    NotInheritable Class ExampleSubclass
                        Inherits ExampleSuperclass
                    End Class  
                    MustInherit Class ExampleSuperclass 
                    End Class
                       
            ");
        }

        public void ParseNotInheritableMustInheritSimpleInheritance() {
            this.Import(@"
                    NotInheritable Class ExampleSuperclass 
                    End Class
                    MustInherit Class ExampleSubclass
                        Inherits ExampleSuperclass
                    End Class     
            ");
        }

        #endregion


        [TestMethod]
        public void ParseSimpleInheritance_HasThreeElements() {
            this.ParseSimpleInheritance();
            this.AssertAmountElements(3);
        }

        [TestMethod]
        public void ParseSimpleInheritance_HasOneInheritanceRelationShip() {
            this.ParseSimpleInheritance();
            Assert.AreEqual(importer.AllElementsOfType<FAMIX.Inheritance>().Count(), 1);
        }


        [TestMethod]
        public void ParseSimpleInheritance_HasAPropperInheritanceRelationShip() {
            this.ParseSimpleInheritance();
            FAMIX.Inheritance inh = importer.AllElementsOfType<FAMIX.Inheritance>().ToList()[0];

            Assert.AreEqual(inh.superclass.name, "ExampleSuperclass");
            Assert.AreEqual(inh.subclass.name, "ExampleSubclass");
        }

        [TestMethod]
        public void ParseMustInheritNotInheritableSimpleInheritance_HasOneInheritanceRelationShip() {
            this.ParseMustInheritNotInheritableSimpleInheritance();
            Assert.AreEqual(importer.AllElementsOfType<FAMIX.Inheritance>().Count(), 1);
        }

        [TestMethod]
        public void ParseMustInheritNotInheritableSimpleInheritance_HasAPropperInheritanceRelationShip() {
            this.ParseMustInheritNotInheritableSimpleInheritance();
            FAMIX.Inheritance inh = importer.AllElementsOfType<FAMIX.Inheritance>().ToList()[0];
            Assert.AreEqual(inh.superclass.name, "ExampleSuperclass");
            Assert.AreEqual(inh.subclass.name, "ExampleSubclass");
        }

        [TestMethod]
        public void ParseMustInheritNotInheritableSimpleInheritance_HasFourElements() {
            this.ParseMustInheritNotInheritableSimpleInheritance();
            this.AssertAmountElements(4);
        }

        [TestMethod]
        public void ParseNotInheritableMustInheritSimpleInheritance_HasFiveElements() {
            this.ParseNotInheritableMustInheritSimpleInheritance();
            this.AssertAmountElements(5);
        }

        [TestMethod]
        public void ParseSimpleInheritance_HasNoModifier() {
            this.ParseSimpleInheritance();
            var elements = importer.AllElementsOfType<FAMIX.Class>().ToList();
            Assert.AreEqual(elements[0].Modifiers.Count, 0);
            Assert.AreEqual(elements[1].Modifiers.Count, 0);
        }

        [TestMethod]
        public void ParseMustInheritNotInheritableSimpleInheritance_HasOneModifierEach() {
            this.ParseMustInheritNotInheritableSimpleInheritance();
            var elements = importer.AllElementsOfType<FAMIX.Class>().ToList();
            Assert.AreEqual(elements[0].Modifiers.Count, 1);
            Assert.AreEqual(elements[1].Modifiers.Count, 1);
        }

        [TestMethod]
        public void ParseNotInheritableMustInheritSimpleInheritance_HasOneModifierEach() {
            this.ParseNotInheritableMustInheritSimpleInheritance();
            var elements = importer.AllElementsOfType<FAMIX.Class>().ToList();
            Assert.AreEqual(elements[0].Modifiers.Count, 1);
            Assert.AreEqual(elements[1].Modifiers.Count, 1);
        }

        [TestMethod]
        public void ParseMustInheritNotInheritableSimpleInheritance_HaveTheExpectedModifiers() {
            this.ParseMustInheritNotInheritableSimpleInheritance();
            var elements = importer.AllElementsOfType<FAMIX.Class>().ToList();
            Assert.AreEqual(elements[1].Modifiers[0], "MustInherit");
            Assert.AreEqual(elements[0].Modifiers[0], "NotInheritable");
        }

        [TestMethod]
        public void ParseNotInheritableMustInheritSimpleInheritance_HaveTheExpectedModifiers() {
            this.ParseNotInheritableMustInheritSimpleInheritance();
            var elements = importer.AllElementsOfType<FAMIX.Class>().ToList();
            Assert.AreEqual(elements[0].Modifiers[0], "NotInheritable");
            Assert.AreEqual(elements[1].Modifiers[0], "MustInherit");
        }


        [TestMethod]
        public void ParseNotInheritableMustInheritSimpleInheritance_HasAPropperInheritanceRelationShip() {
            this.ParseNotInheritableMustInheritSimpleInheritance();
            FAMIX.Inheritance inh = importer.AllElementsOfType<FAMIX.Inheritance>().ToList()[0];

            Assert.AreEqual(inh.superclass.name, "Object");
            Assert.AreEqual(inh.subclass.name, "ExampleSubclass");
        }


    }
}
