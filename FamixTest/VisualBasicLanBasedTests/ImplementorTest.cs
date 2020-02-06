using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FamixTest.VisualBasic {
    [TestClass]
    public class ImplementorTest : LanProjectVisualBasicLoader {
        [TestMethod]
        public void ClassImplementsInterfaces()
        {
            Assert.AreEqual(3, importer.Types.Named("SampleProject.Basic.Implementor").SuperInheritances.Count);
        }
    }
}
