using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FamixTest.VisualBasic {
    [TestClass]
    public class SimpleStructTest : LanProjectVisualBasicLoader {
        [TestMethod]
        public void StructIngested() => Assert.IsNotNull(importer.Types.Named("SampleProject.Basic.SimpleStruct"));

        [TestMethod]
        public void StructAttributeIngested() => Assert.IsNotNull(importer.Attributes.Named("SampleProject.Basic.SimpleStruct.i"));
    }
}
