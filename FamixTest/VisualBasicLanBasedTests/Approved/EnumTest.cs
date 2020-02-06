using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FamixTest.VisualBasic {
    [TestClass]
    public class EnumTest : LanProjectVisualBasicLoader {

        [TestMethod]
        public void EnumIsIngested() {
            FAMIX.Enum Enumeration = (FAMIX.Enum)importer.Types.Named("VBLanLibrary.Packet.Protocol");
            Assert.IsNotNull(Enumeration); 
        }
        [TestMethod]
        public void EnumIsIngestedWithItsValues() {
            FAMIX.Enum Enumeration = (FAMIX.Enum)importer.Types.Named("VBLanLibrary.Packet.Protocol"); 
            Assert.AreEqual(Enumeration.Values.Count, 2);
        }

        [TestMethod]
        public void EnumValuesAreIngested() {
            FAMIX.Enum Enumeration = (FAMIX.Enum)importer.Types.Named("VBLanLibrary.Packet.Protocol");

            Assert.IsNotNull(importer.Attributes.Named("VBLanLibrary.Packet.Protocol.TCP"));
            Assert.IsNotNull(importer.Attributes.Named("VBLanLibrary.Packet.Protocol.UDP"));
        }


        [TestMethod]
        public void EnumValuesPickedByTheImporterAreTheSameAsPickedFromEnum() {
            FAMIX.Enum Enumeration = (FAMIX.Enum)importer.Types.Named("VBLanLibrary.Packet.Protocol");
            var Tcp = (FAMIX.EnumValue)importer.Attributes.Named("VBLanLibrary.Packet.Protocol.TCP");
            var Udp = (FAMIX.EnumValue)importer.Attributes.Named("VBLanLibrary.Packet.Protocol.UDP");
            Assert.AreEqual(Tcp, importer.Attributes.Named("VBLanLibrary.Packet.Protocol.TCP"));
            Assert.AreEqual(Udp, importer.Attributes.Named("VBLanLibrary.Packet.Protocol.UDP"));
        }

        [TestMethod]
        public void EnumValueAreUsed() {
            FAMIX.Enum Enumeration = (FAMIX.Enum)importer.Types.Named("VBLanLibrary.Packet.Protocol");
            Assert.AreEqual(Enumeration.IncomingReferences.Count, 2);
            /* 
             * Should test that the usage of the enums is recogniced non? 
             * So far i am expecting to references, one from the definition 
             * in the packet and other from the usage in fileserver.
             */
            Assert.IsTrue(false);
        }
    }
}
