using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;



/*
 * This test case is tendencious, hard to mantain and not really proving much. 
 * We should think about taking it out and unit testing the mechanism behind
 */
namespace FamixTest.VisualBasic {
    [TestClass]
    public class CallsTest : LanProjectVisualBasicLoader {
        [TestMethod]
        public void TestTheAmountOfMethodsWithOutgoingInvocationsIs () {
            var methodsWithOutgoingInvocations = 
            (from entity in importer.Methods.RegisteredEntities()
            where entity.OutgoingInvocations.Count > 0
            select entity).ToList(); 

            Assert.AreEqual(15, methodsWithOutgoingInvocations.Count);
        }

        [TestMethod]
        public void TestTheAmountOfMethodsWithWithoutOutgoingInvocationsIs() {
            var methodsWithoutOutGoingInvocations =
            (from entity in importer.Methods.RegisteredEntities()
             where entity.OutgoingInvocations.Count == 0
             select entity.name).ToList();

            Assert.AreEqual(90, methodsWithoutOutGoingInvocations.Count);
        }

        [TestMethod]
        public void TestTheAmountOfMethodsWithIncomingInvocationsIs() {
            var methodsWithIncomingInvocations =
            (from entity in importer.Methods.RegisteredEntities()
             where entity.IncomingInvocations.Count > 0
             select entity.name).ToList();

            Assert.AreEqual(17, methodsWithIncomingInvocations.Count);
        }
        [TestMethod]
        public void TestTheAmountOfMethodsWithoutIncomingInvocationsIs() {
            var methodsWithoutIncomingInvocations =
            (from entity in importer.Methods.RegisteredEntities()
             where entity.IncomingInvocations.Count == 0
             select entity.name).ToList();

            Assert.AreEqual(88, methodsWithoutIncomingInvocations.Count);
        }


    }
}
