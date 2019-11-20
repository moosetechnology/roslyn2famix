using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace FamixTest.VisualBasicUnitTest {
    [TestClass]
    public class ClassUsingGenericsInMethodsAndState : VisualBasicUnitTest {


        #region SettingUp

        [TestInitialize]
        public void ParseTypeParametrizedMethods () {
            this.Import(@"
                  Class Something
                  End Class
                  Class Example
                    Public Overridable Sub ExampleSub(Of T)(GenericVariable As T)
                    End Sub
                    Public Overridable Sub ExampleSubExtends(Of T As Something)(GenericVariable As T)
                    End Sub
                    Public Overridable Function ExampleFunctionExtends(Of T As Something)() As T
                    End Function
                    Public Overridable Function ExampleFunction(Of T)() As T
                    End Function
                End Class
            ");
        }

        #endregion

        protected FAMIX.ParameterizableMethod Method( String name ) {
            return this.importer.AllElementsOfType<FAMIX.ParameterizableMethod>().First( m => m.name == name);
        }

        #region ParseEmptySubManyParameters

        [TestMethod]
        public void ParseTypeParametrizedMethods_ExampleFunctionHasOneParameterNamed () {
            Assert.AreEqual(Method("ExampleFunction").TypeParameters.First().name, "T");
        }

        [TestMethod]
        public void ParseTypeParametrizedMethods_ExampleFunctionReturnsParametrizedType() {
            Assert.AreEqual(Method("ExampleFunction").TypeParameters.First(), Method("ExampleFunction").returnType);
        }

        [TestMethod]
        public void ParseTypeParametrizedMethods_ExampleFunctionExtendsHasOneParameterNamed() {
            Assert.AreEqual(Method("ExampleFunctionExtends").TypeParameters.First().name, "T");
        }

        [TestMethod]
        public void ParseTypeParametrizedMethods_ExampleFunctionExtendsReturnsParametrizedType() {
            Assert.AreEqual(Method("ExampleFunctionExtends").TypeParameters.First(), Method("ExampleFunctionExtends").returnType);
        }


        [TestMethod]
        public void ParseTypeParametrizedMethods_ExampleFunctionExtendsReturnsParametrizedExtendedType() {
            Assert.AreEqual(Method("ExampleFunctionExtends").TypeParameters.First(), Method("ExampleFunctionExtends").Parameters.First().declaredType);
        }




        [TestMethod]
        public void ParseTypeParametrizedMethods_ExampleSubHasOneParameter() {
            Assert.AreEqual(Method("ExampleSub").TypeParameters.Count(), 1);
        }
        [TestMethod]
        public void ParseTypeParametrizedMethods_ExampleSubHasOneParameterNamed() {
            Assert.AreEqual(Method("ExampleSub").Parameters.First().name, "GenericVariable");
        }
        [TestMethod]
        public void ParseTypeParametrizedMethods_ExampleSubHasOneParameterTyped() {
            Assert.AreEqual(Method("ExampleSub").TypeParameters.First().name, "T");
        }

        [TestMethod]
        public void ParseTypeParametrizedMethods_ExampleSubHasOneParameterTypedAsType() {
            Assert.AreEqual(Method("ExampleSub").TypeParameters.First(), Method("ExampleSub").Parameters.First().declaredType);
        }




        public void ParseTypeParametrizedMethods_ExampleSubExampleSubExtendsHasOneParameter() {
            Assert.AreEqual(Method("ExampleSubExtends").TypeParameters.Count(), 1);
        }

        [TestMethod]
        public void ParseTypeParametrizedMethods_ExampleSubExampleSubExtendsHasOneParameterNamed() {
            Assert.AreEqual(Method("ExampleSubExtends").TypeParameters.First().name, "GenericVariable");
        }
        [TestMethod]
        public void ParseTypeParametrizedMethods_ExampleSubExampleSubExtendsHasOneParameterTyped() {
            Assert.AreEqual(Method("ExampleSubExtends").TypeParameters.First().name, "T");
        }

        [TestMethod]
        public void ParseTypeParametrizedMethods_ExampleSubExtendsHasOneParameterTypedAsType() {
            Assert.AreEqual(Method("ExampleSubExtends").TypeParameters.First(), Method("ExampleSubExtends").Parameters.First().declaredType);
        }


        [TestMethod]
        public void ParseTypeParametrizedMethods_ExampleSubExampleSubExtendsHasOneParameterTypedWithSuperclass() {
            Assert.AreEqual(Method("ExampleSubExtends").TypeParameters.First().SuperInheritances.First().superclass, this.importer.AllElementsOfType<FAMIX.Class>().First());
        }


        #endregion





    }
}
