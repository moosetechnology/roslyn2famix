﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FamixTest
{
    [TestClass]
    public class ImplementorTest : SampleSystemLoader
    {
        [TestMethod]
        public void ClassImplementsInterfaces()
        {
            Assert.AreEqual(2, importer.Types.Named(".SampleProject.Basic.Implementor").SuperInheritances.Count);
        }
    }
}
