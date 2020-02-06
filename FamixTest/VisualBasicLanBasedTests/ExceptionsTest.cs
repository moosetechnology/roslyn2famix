using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace FamixTest.VisualBasic {
    [TestClass]
    public class ExceptionsTest : LanProjectVisualBasicLoader {
        [TestMethod]
        public void CaughtExceptions()
        {
            var allCaughtExceptions = importer.AllElementsOfType<FAMIX.CaughtException>();
            Assert.AreEqual("Method", allCaughtExceptions.First<FAMIX.CaughtException>().definingMethod.name);
            Assert.AreEqual("Exceptions", allCaughtExceptions.First<FAMIX.CaughtException>().exceptionClass.name);
        }
    }
  
}
