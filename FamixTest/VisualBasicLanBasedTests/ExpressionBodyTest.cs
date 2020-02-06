using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FamixTest.VisualBasic { 
    [TestClass]
    public class ExpressionBodyTest : LanProjectVisualBasicLoader {
        [TestMethod]
        public void ConstructorCallsAndAccesses() {
            Assert.AreEqual(2, importer.Methods.Named("SampleProject.Basic.ExpressionBody..ctor(String)").Accesses.Count);
            Assert.AreEqual(1, importer.Methods.Named("SampleProject.Basic.ExpressionBody..ctor(String)").OutgoingInvocations.Count);
        }

        [TestMethod]
        public void PropertySetterAccesses() {
            Assert.AreEqual(2, (importer.Attributes.Named("SampleProject.Basic.ExpressionBody.Name") as Net.Property).setter.Accesses.Count);
        }
    }
}
