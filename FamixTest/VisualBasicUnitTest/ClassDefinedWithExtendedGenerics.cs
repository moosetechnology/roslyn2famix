using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace FamixTest.VisualBasicUnitTest {
    [TestClass]
    public class ClassDefinedWithExtendedGenerics : VisualBasicUnitTest {


        #region SettingUp
       
        [TestInitialize]
        public void ParseGenericConfiguredUsingClass() {
            this.Import(@"
                    Class Something
                    End Class
                    Class Example(Of Out T As Something)
                        Public AGenericField As T
                        Public Property AGenericProperty As T
                        Public Overridable Sub SubAcceptsT(Parameter As T)
                        End Sub 
                        Public Overridable Function FunctionReturnsT() As T
                        End Function 
                    End Class
            ");
        }

        protected FAMIX.ParameterizableClass Class() {
            return this.importer.AllElementsOfType<FAMIX.ParameterizableClass>().First();
        }

        #endregion


        #region ParseEmptySubManyParameters

        [TestMethod]
        public void ParseGenericUsingClass_ClassIsParametrizableClass() {
            Assert.AreEqual(this.importer.AllElementsOfType<FAMIX.ParameterizableClass>().Count(), 1);
            Assert.AreEqual(this.importer.AllElementsOfType<FAMIX.Class>().Count(), 2);
        }

        [TestMethod]
        public void ParseGenericUsingClass_FunctionReturnsTypedAsT() {
            Assert.AreEqual(this.Class().Methods[1].returnType, this.Class().Parameters.First());
        }

        [TestMethod]
        public void ParseGenericUsingClass_SubAcceptParameterTypedAsT() {
            Assert.AreEqual(this.Class().Methods[0].Parameters[0].declaredType, this.Class().Parameters.First());
        }



        [TestMethod]
        public void ParseGenericUsingClass_PropertyTypeIsTheInnerType() {
            Assert.AreEqual(this.Class().Attributes[1].declaredType, this.Class().Parameters.First());
        }

        [TestMethod]
        public void ParseGenericUsingClass_InnerTypeSuperclass() {
            Assert.AreEqual(this.Class().SuperInheritances.First().superclass, this.importer.AllElementsOfType<FAMIX.Class>().First());
        }


        [TestMethod]
        public void ParseGenericUsingClass_FieldTypeIsTheInnerType() {
            Assert.AreEqual(this.Class().Attributes[0].declaredType, this.Class().Parameters.First());
        }



        [TestMethod]
        public void ParseGenericUsingClass_DefinedClassHasOneInnerType() {
            Assert.AreEqual(this.Class().Parameters.Count , 1);
        }





        #endregion



    }
}
