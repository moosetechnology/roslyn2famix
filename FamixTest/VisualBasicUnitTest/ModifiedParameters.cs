using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Collections.Generic;

namespace FamixTest.VisualBasicUnitTest {
    [TestClass]
    public class ModifiedParameters : VisualBasicUnitTest {


        #region SettingUp
        public void ParsingExample () {
            this.Import(@"
                    Class Example 
                        Internal Overridable Function ExampleFunction (ByRef values As String, ByVal searchString As String, Optional ByVal matchCase As Boolean = False, Optional ByRef optionalRef As Object = Nothing) As String
                        End Function 
                    End Class
            ");
        }

        #endregion


        #region ParsingExample testing 
        [TestMethod]
        public void ParsingExample_HasFourParameters () {
            this.ParsingExample();
            Assert.AreEqual(this.importer.AllElementsOfType<FAMIX.Parameter>().Count(), 4);
        }

        [TestMethod]
        public void ParsingExample_HasElevenElements() {
            this.ParsingExample();
            this.AssertAmountElements(11);
        }

        private FAMIX.Parameter GetParameter(int i) {
            return this.importer.AllElementsOfType<FAMIX.Method>().ToList()[0].Parameters[i];
        }
        private List<String> GetParameterModifiers(int i) {
            return this.importer.AllElementsOfType<FAMIX.Method>().ToList()[0].Parameters[i].Modifiers;
        }

        [TestMethod]
        public void ParsingExample_OnlyOneMethod() {
            this.ParsingExample();
            Assert.AreEqual(this.importer.AllElementsOfType<FAMIX.Method>().Count(), 1);
        }

        [TestMethod]
        public void ParsingExample_FourthParameterTwoModifiers() {
            this.ParsingExample();
            Assert.AreEqual(2, this.GetParameterModifiers(3).Count());
        }
        [TestMethod]
        public void ParsingExample_FourthFirstParameterIsOptional() {
            this.ParsingExample();
            Assert.AreEqual("Optional", this.GetParameterModifiers(3)[0]);
        }

        [TestMethod]
        public void ParsingExample_FourthSecondParameterIsByRef() {
            this.ParsingExample();
            Assert.AreEqual("ByRef", this.GetParameterModifiers(3)[1]);
        }


        [TestMethod]
        public void ParsingExample_ThirdParameterHasTwoModifier() {
            this.ParsingExample();
            Assert.AreEqual(2, this.GetParameterModifiers(2).Count());
        }
        [TestMethod]
        public void ParsingExample_ThirdParameterIsOptional() {
            this.ParsingExample();
            Assert.AreEqual("Optional", this.GetParameterModifiers(2)[0]);
        }
        [TestMethod]
        public void ParsingExample_ThirdParameterIsByVal() {
            this.ParsingExample();
            Assert.AreEqual("ByVal", this.GetParameterModifiers(2)[1]);
        }


        [TestMethod]
        public void ParsingExample_FourthParameterHasDefaultValue() {
            this.ParsingExample();
            Assert.AreEqual("Nothing", this.GetParameter(3).defaultValue);
        }


        [TestMethod]
        public void ParsingExample_ThirdParameterHasDefaultValue() {
            this.ParsingExample();
            Assert.AreEqual("False", this.GetParameter(2).defaultValue);
        }

        [TestMethod]
        public void ParsingExample_FirstParameterHasOneModifier() {
            this.ParsingExample();
            Assert.AreEqual(1, this.GetParameterModifiers(0).Count());
        }
        [TestMethod]
        public void ParsingExample_FirstParameterIsByRef() {
            this.ParsingExample();
            Assert.AreEqual("ByRef", this.GetParameterModifiers(0)[0]);
        }

        [TestMethod]
        public void ParsingExample_SecondParameterHasNoModifier() { 
            // ByVal is the default behaviour 
            this.ParsingExample();
            Assert.AreEqual(1, this.GetParameterModifiers(1).Count());
        }


        #endregion


    }
}
