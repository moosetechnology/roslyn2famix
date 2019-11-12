﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FamixTest
{
    [TestClass]
    public class PropertiesTest : SampleSystemCSharpLoader
    {
        [TestMethod]
        public void TestPropertyHasSetter()
        {
            Net.Property pro = importer.Attributes.Named("SampleProject.Basic.Properties.Value") as Net.Property;
            Assert.IsNotNull(pro.setter);
            Assert.AreSame(pro.setter.OutgoingInvocations[0].Candidates[0],(importer.Methods.Named("SampleProject.Basic.Properties.Method()")));
        }
    }
}
