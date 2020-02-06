using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FamixTest.VisualBasic {
    [TestClass]
    public class EventDelegateClassTest : LanProjectVisualBasicLoader {
        [TestMethod]
        public void EventDelegateClassPlainMethodCallsEvent()
        {
            Assert.AreEqual("MyEvent", 
                importer.Methods.Named("SampleProject.Basic.EventDelegateClass.Trigger()").
                OutgoingInvocations[0].Candidates[0].name);
        }
    }
}
