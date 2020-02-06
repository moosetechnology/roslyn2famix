﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace FamixTest.VisualBasicUnitTest {
    [TestClass]
    public class ClassDefinedWithGenerics : VisualBasicUnitTest {


        #region SettingUp
       
        [TestInitialize]
        public void ParseGenericUsingClass() {
            this.Import(@"
                    Class Example(Of T)
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
            Assert.AreEqual(this.importer.AllElementsOfType<FAMIX.Class>().Count(), 1);
        }

        [TestMethod]
        public void ParseGenericUsingClass_FunctionReturnsTypedAsT() {
            Assert.AreEqual(this.Class().Methods[1].returnType, this.Class().TypeParameters.First());
        }

        [TestMethod]
        public void ParseGenericUsingClass_SubAcceptParameterTypedAsT() {
            Assert.AreEqual(this.Class().Methods[0].Parameters[0].declaredType, this.Class().TypeParameters.First());
        }



        [TestMethod]
        public void ParseGenericUsingClass_PropertyTypeIsTheInnerType() {
            Assert.AreEqual(this.Class().Attributes[1].declaredType, this.Class().TypeParameters.First());
        }


        [TestMethod]
        public void ParseGenericUsingClass_FieldTypeIsTheInnerType() {
            Assert.AreEqual(this.Class().Attributes[0].declaredType, this.Class().TypeParameters.First());
        }



        [TestMethod]
        public void ParseGenericUsingClass_DefinedClassHasOneInnerType() {
            Assert.AreEqual(this.Class().TypeParameters.Count , 1);
        }





        #endregion



    }
}
