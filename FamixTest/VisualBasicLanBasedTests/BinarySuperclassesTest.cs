using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FamixTest.VisualBasic {
    [TestClass]
    public class BinarySuperclassesTest : LanProjectVisualBasicLoader {
        [TestMethod]
        public void LinkedListIsIngested() => Assert.IsNotNull(importer.Types.Named("DefinitionOfSystem.Collections.Generic.LinkedList<T>"));

        [TestMethod]
        public void BinaryClassHasMethods() =>
          Assert.AreEqual(33, importer.Types.Named("DefinitionOfSystem.Collections.Generic.LinkedList<T>").Methods.Count);

    }
}
