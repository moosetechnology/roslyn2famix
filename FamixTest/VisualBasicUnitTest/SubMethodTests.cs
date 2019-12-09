using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace FamixTest.VisualBasicUnitTest {
    [TestClass]
    public class SubMethodTests : VisualBasicUnitTest {


        #region SettingUp
       
        public void ParseEmptySubParameterLess() {
            this.Import(@"
                    Class Example 
                        Public Overridable Sub ExampleSub()
                        End Sub 
                    End Class
            ");
        }
        public void ParseEmptySubOneParameter() {
            this.Import(@"
                    Class Example 
                        Public Overridable Sub ExampleSub(Parameter As Integer)
                        End Sub 
                    End Class
            ");
        }
        public void ParseEmptySubManyParameters() {
            this.Import(@"
                     Class Example 
                        Internal Overrides Sub ExampleSub(Parameter1 As Integer, Parameter2 as String)
                        End Sub 
                    End Class
            ");
        }

        #endregion


        #region ParseEmptySubManyParameters
        [TestMethod]
        public void ParseEmptySubManyParameters_Nine() {
            this.ParseEmptySubManyParameters();
            this.AssertAmountElements(9);
        }

        [TestMethod]
        public void ParseEmptySubManyParameters_RecogniceParameterClass() {
            this.ParseEmptySubManyParameters();
            Assert.IsTrue(this.importer.AllElementsOfType<FAMIX.Class>().Any(p => p.name == "Int32"));
            Assert.IsTrue(this.importer.AllElementsOfType<FAMIX.Class>().Any(p => p.name == "String"));
        }

        [TestMethod]
        public void ParseEmptySubManyParameters_AllClassesAreNamed() {
            this.ParseEmptySubManyParameters();
            Assert.IsFalse(this.importer.AllElementsOfType<FAMIX.Class>().Any(p => p.name == "" || p.name == null));
        }


        [TestMethod]
        public void ParseEmptySubManyParameters_IsInternal() {
            this.ParseEmptySubManyParameters();
            var MethodElement = this.GetElement<FAMIX.Method>(2);
            Assert.IsFalse(MethodElement.isPublic);
            Assert.IsFalse(MethodElement.isProtected);
            Assert.IsFalse(MethodElement.isPrivate);
            
            Assert.AreEqual(MethodElement.Modifiers.Count, 1);

            Assert.AreEqual(MethodElement.Modifiers.First(), "Overrides");

            Assert.AreEqual(MethodElement.accessibility, "Public");
        }
        [TestMethod]
        public void ParseEmptySubManyParameters_HasTwoParameters() {
            this.ParseEmptySubManyParameters();
            var MethodElement = this.GetElement<FAMIX.Method>(2);
            Assert.AreEqual(MethodElement.Parameters.Count, 2);
        }

        #endregion


        #region ParameterLessTest


        [TestMethod]
        public void ParseEmptySubParameterLess_IsNamedExample() {
            this.ParseEmptySubParameterLess();
            var MethodElement = this.GetElement<FAMIX.Method>(2);
            Assert.AreEqual(MethodElement.name, "ExampleSub");
        }
        [TestMethod]
        public void ParseEmptySubParameterLess_ReturnsNothing() {
            this.ParseEmptySubParameterLess();
            var MethodElement = this.GetElement<FAMIX.Method>(2);
            Assert.AreEqual(MethodElement.returnType, null);
        }

       
        [TestMethod]
        public void ParseEmptySubParameterLess_IsContainedInExampleClass() {
            this.ParseEmptySubParameterLess();
            var MethodElement = this.GetElement<FAMIX.Method>(2);
            Assert.AreEqual(MethodElement.parentType, this.GetElement<FAMIX.Class>(1));
        }
        [TestMethod]
        public void ParseEmptySubParameterLess_FourElements() {
            this.ParseEmptySubParameterLess();
            this.AssertAmountElements(4);
        }
        [TestMethod]
        public void ParseEmptySubParameterLess_Public() {
            this.ParseEmptySubParameterLess();
            var MethodElement = this.GetElement<FAMIX.Method>(2);
            Assert.IsTrue(MethodElement.isPublic);
        }
        [TestMethod]
        public void ParseEmptySubParameterLess_HasNoParameter() {
            this.ParseEmptySubParameterLess();
            var MethodElement = this.GetElement<FAMIX.Method>(2);
            Assert.AreEqual(MethodElement.Parameters.Count, 0);
        }

        #endregion

        #region ParseEmptySubOneParameter
        [TestMethod]
        public void ParseEmptySubOneParameter_SevenElements() {
            this.ParseEmptySubOneParameter();
            this.AssertAmountElements(7);
        }

        [TestMethod]
        public void ParseEmptySubOneParameter_RecogniceParameterClass() {
            this.ParseEmptySubOneParameter();
            Assert.IsTrue(this.importer.AllElementsOfType<FAMIX.Class>().Any(p => p.name == "Int32"));

        }

        [TestMethod]
        public void ParseEmptySubOneParameter_AllClassesAreNamed() {
            this.ParseEmptySubOneParameter();
            Assert.IsFalse(this.importer.AllElementsOfType<FAMIX.Class>().Any(p => p.name == "" || p.name ==  null));
        }

        [TestMethod]
        public void ParseEmptySubOneParameters_Protected() {
            this.ParseEmptySubOneParameter();
            var MethodElement = this.GetElement<FAMIX.Method>(2);
            Assert.IsTrue(MethodElement.isPublic);
        }
        [TestMethod]
        public void ParseEmptySubOneParameter_HasOneParameter() {
            this.ParseEmptySubOneParameter();
            var MethodElement = this.GetElement<FAMIX.Method>(2);
            Assert.AreEqual(MethodElement.Parameters.Count, 1);
        }

        #endregion



    }
}
