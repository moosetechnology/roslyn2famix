using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FamixTest.VisualBasic {
    [TestClass]
    public class EnumTest : LanProjectVisualBasicLoader {
        [TestMethod]
        public void EnumIsIngested() => Assert.IsNotNull(importer.Types.Named("SampleProject.Basic.Enum.MyFavoriteEnum"));

        [TestMethod]
        public void EnumValueIsIngested() => Assert.IsNotNull(importer.Attributes.Named("SampleProject.Basic.Enum.MyFavoriteEnum.ONE"));
    }
}
