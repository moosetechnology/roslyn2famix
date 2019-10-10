using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

using Fame;

namespace FameTest {
    [TestClass]
    public class FameRepositoryTest {
        Repository repository;
        object element;
        [FamePackage("RPG")]
        [FameDescription("Dragon")]
        class Dragon { }
        [TestInitialize]
        public void setUp() {
            repository = new Repository();
            repository.metamodel.RegisterType(typeof(Dragon));
            element = new Dragon();
        }

        [ExpectedException(typeof(ClassNotMetadescribedException))]
        [TestMethod]
        public void TestRespositoryDoesNotAcceptNonDescribedClasses() {

            Assert.IsTrue(repository.GetElements().Count == 0);
            repository.Add(repository);
            Assert.IsTrue(repository.GetElements().Count == 1);
            Assert.IsTrue(repository.GetElements()[0] == repository);
        }

        [TestMethod]
        public void TestAProperlyDescribedElementIsAcceptedInTheRepository() {
            Assert.IsTrue(repository.GetElements().Count == 0);
            repository.Add(element);
            Assert.IsTrue(repository.GetElements().Count == 1);
            Assert.IsTrue(repository.GetElements()[0] == element);
        }
    }
}
