﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FamixTest
{
    [TestClass]
    public class ISimpleInterfaceTest : SampleSystemCSharpLoader
    {
        [TestMethod]
        public void InterfaceWasIngested() => Assert.IsNotNull(importer.Types.Named("SampleProject.Basic.ISimpleInterface"));
    }
}
