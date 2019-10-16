using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FamixTest.VisualBasic {
    [TestClass]
    public class ISimpleInterfaceTest : LanProjectVisualBasicLoader {
        [TestMethod]
        public void InterfaceWasIngested() => Assert.IsNotNull(importer.Types.Named("SampleProject.Basic.ISimpleInterface"));
    }
}
