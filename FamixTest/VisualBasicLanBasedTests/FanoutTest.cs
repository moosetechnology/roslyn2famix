using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FamixTest.VisualBasic {
    [TestClass]
    public class FanoutTest: LanProjectVisualBasicLoader {
        [TestMethod]
        public void TestFanout()=> Assert.AreEqual(2, importer.Types.Named("SampleProject.Basic.Fanout").fanOut);
    }
}
