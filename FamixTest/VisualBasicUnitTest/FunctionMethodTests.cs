using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace FamixTest.VisualBasicUnitTest {
    [TestClass]
    public class FunctionMethodTests : VisualBasicUnitTest {

        #region SettingUp
        public void ParseEmptyFunctionParameterLess() {
            this.Import(@"
                    Class Example 
                            Public Overridable Function ExampleFunction() As Boolean
                            End Function 
                    End Class
            ");
        }
        public void ParseEmptyFunctionOneParameter() {
            this.Import(@"
                    Class Example 
                            Protected Overridable Function ExampleFunction(Parameter As Integer) As Boolean
                            End Function 
                    End Class
            ");
        }
        public void ParseEmptyFunctionManyParameters() {
            this.Import(@"
                    Class Example 
                        Internal Overridable Function ExampleFunction(Parameter1 As Integer, Parameter2 as String) As Boolean
                        End Function 
                    End Class
            ");
        }
   

        #endregion

        #region ParseEmptyFunctionManyParameters
        [TestMethod]
        public void ParseEmptyFunctionManyParameters_SevenTen() {
            this.ParseEmptyFunctionManyParameters();
            this.AssertAmountElements(10);
        }

        [TestMethod]
        public void ParseEmptyFunctionManyParameters_RecogniceParameterClass() {
            this.ParseEmptyFunctionManyParameters();
            Assert.IsTrue(this.importer.AllElementsOfType<FAMIX.Class>().Any(p => p.name == "Int32"));
            Assert.IsTrue(this.importer.AllElementsOfType<FAMIX.Class>().Any(p => p.name == "String"));
        }

        [TestMethod]
        public void ParseEmptyFunctionManyParameters_AllClassesAreNamed() {
            this.ParseEmptyFunctionManyParameters();
            Assert.IsFalse(this.importer.AllElementsOfType<FAMIX.Class>().Any(p => p.name == "" || p.name == null));
        }


        [TestMethod]
        public void ParseEmptyFunctionManyParameters_IsInternal() {
            this.ParseEmptyFunctionManyParameters();
            var MethodElement = this.GetElement<FAMIX.Method>(2);
            Assert.IsFalse(MethodElement.isPublic);
            Assert.IsFalse(MethodElement.isProtected);
            Assert.IsFalse(MethodElement.isPrivate);
            
            Assert.AreEqual(MethodElement.Modifiers.Count, 1);

            Assert.AreEqual(MethodElement.Modifiers.First(), "Overridable");

            Assert.AreEqual(MethodElement.accessibility, "Public");
        }
        [TestMethod]
        public void ParseEmptyFunctionManyParameters_HasTwoParameters() {
            this.ParseEmptyFunctionManyParameters();
            var MethodElement = this.GetElement<FAMIX.Method>(2);
            Assert.AreEqual(MethodElement.Parameters.Count, 2);
        }

        #endregion

        #region ParameterLessTest


        [TestMethod]
        public void ParseEmptyFunctionParameterLess_IsNamedExample() {
            this.ParseEmptyFunctionParameterLess();
            var MethodElement = this.GetElement<FAMIX.Method>(2);
            Assert.AreEqual(MethodElement.name, "ExampleFunction");
        }
        [TestMethod]
        public void ParseEmptyFunctionParameterLess_ReturnsBoolean() {
            this.ParseEmptyFunctionParameterLess();
            var MethodElement = this.GetElement<FAMIX.Method>(2);
            Assert.AreEqual(MethodElement.returnType.name, "Boolean");
        }

       
        [TestMethod]
        public void ParseEmptyFunctionParameterLess_IsContainedInExampleClass() {
            this.ParseEmptyFunctionParameterLess();
            var MethodElement = this.GetElement<FAMIX.Method>(2);
            Assert.AreEqual(MethodElement.parentType, this.GetElement<FAMIX.Class>(1));
        }
        [TestMethod]
        public void ParseEmptyFunctionParameterLess_SixElements() {
            this.ParseEmptyFunctionParameterLess();
            this.AssertAmountElements(6);
        }
        [TestMethod]
        public void ParseEmptyFunctionParameterLess_Public() {
            this.ParseEmptyFunctionParameterLess();
            var MethodElement = this.GetElement<FAMIX.Method>(2);
            Assert.IsTrue(MethodElement.isPublic);
        }
        [TestMethod]
        public void ParseEmptyFunctionParameterLess_HasNoParameter() {
            this.ParseEmptyFunctionParameterLess();
            var MethodElement = this.GetElement<FAMIX.Method>(2);
            Assert.AreEqual(MethodElement.Parameters.Count, 0);
        }

        #endregion

        #region ParseEmptyFunctionOneParameter
        [TestMethod]
        public void ParseEmptyFunctionOneParameter_EightElements() {
            this.ParseEmptyFunctionOneParameter();
            this.AssertAmountElements(8);
        }

        [TestMethod]
        public void ParseEmptyFunctionOneParameter_RecogniceParameterClass() {
            this.ParseEmptyFunctionOneParameter();
            Assert.IsTrue(this.importer.AllElementsOfType<FAMIX.Class>().Any(p => p.name == "Int32"));

        }

        [TestMethod]
        public void ParseEmptyFunctionOneParameter_AllClassesAreNamed() {
            this.ParseEmptyFunctionOneParameter();
            Assert.IsFalse(this.importer.AllElementsOfType<FAMIX.Class>().Any(p => p.name == "" || p.name ==  null));
        }

        [TestMethod]
        public void ParseEmptyFunctionOneParameters_Protected() {
            this.ParseEmptyFunctionOneParameter();
            var MethodElement = this.GetElement<FAMIX.Method>(2);
            Assert.IsTrue(MethodElement.isProtected);
        }
        [TestMethod]
        public void ParseEmptyFunctionOneParameter_HasOneParameter() {
            this.ParseEmptyFunctionOneParameter();
            var MethodElement = this.GetElement<FAMIX.Method>(2);
            Assert.AreEqual(MethodElement.Parameters.Count, 1);
        }

        #endregion

    }
}
