﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FamixTest
{
    [TestClass]
    public class FanoutTest:SampleSystemCSharpLoader
    {
        [TestMethod]
        public void TestFanout()=> Assert.AreEqual(2, importer.Types.Named("SampleProject.Basic.Fanout").fanOut);
    }
}
