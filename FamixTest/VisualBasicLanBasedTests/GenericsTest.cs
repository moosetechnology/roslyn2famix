using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FamixTest.VisualBasic {
    [TestClass]
    public class GenericsTest : LanProjectVisualBasicLoader {
        [TestMethod]
        public void GenericsTypeIsIngested() { 
            Assert.IsNotNull(importer.Types.Named("DefinitionOfSampleProject.Basic.Generics<T>"));
        }
        [TestMethod]
        public void GenericsTypeIsCorrectType() { 
            Assert.IsInstanceOfType(importer.Types.Named("DefinitionOfSampleProject.Basic.Generics<T>"), typeof(FAMIX.ParameterizableClass)); 
        }


    }
}
