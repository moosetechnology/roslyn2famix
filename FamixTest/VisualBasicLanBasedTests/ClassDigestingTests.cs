using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;
using System.Collections.Generic;

using FAMIX;

namespace FamixTest.VisualBasic {
	[TestClass]
	public class ClassDigestingTests : LanProjectVisualBasicLoader {
        
        #region Assertions 
        public void AssertClassAggregationIsConsistent(string name) {
            FAMIX.Class value = (FAMIX.Class)importer.Types.Named(name);
            int constructors = (from m in value.Methods
                                where m.isConstructor
                                select m).ToList().Count();
            Assert.AreEqual(value.numberOfConstructorMethods, constructors);
            Assert.AreEqual(value.Methods.Count, value.numberOfMethods);
            Assert.AreEqual(value.Attributes.Count, value.numberOfAttributes);
        }
        public void AssertClassIsAccessibleByNameAndNamed(string fullName, string name) {
            FAMIX.Class value = (FAMIX.Class)importer.Types.Named(fullName);
            Assert.IsNotNull(value);
            Assert.AreEqual(value.name, name);
        }
        #endregion

        [TestMethod]
        public void TestAllClassesWhereIngested() {
            List<FAMIX.Class> classes = (
                from famixClass
                in importer.AllElementsOfType<FAMIX.Class>().ToList()
                where famixClass.container != null && famixClass.container.name.Equals("VBLanLibrary") && !famixClass.isInterface
                select famixClass).ToList();
            Assert.AreEqual(classes.Count, 8);
        }

        #region NodeClass
        [TestMethod]
        public void TestNodeClassCanBeAccessedByName () {
            this.AssertClassIsAccessibleByNameAndNamed("VBLanLibrary.Node", "Node");
        }
        [TestMethod]
        public void TestNodeClassIsComplete() {
            FAMIX.Class value = (FAMIX.Class)importer.Types.Named("VBLanLibrary.Node");
            Assert.AreEqual(value.SuperInheritances.Count, 1);
            Assert.AreEqual(value.SuperInheritances.First<FAMIX.Inheritance>().superclass.name, "Object");
            Assert.AreEqual(value.Attributes.Count, 2);
            Assert.AreEqual(value.Methods.Count, 6 + 1 + 4);

        }
        [TestMethod]
        public void TestNodeClassAggregationIsConsistent() {
            this.AssertClassAggregationIsConsistent("VBLanLibrary.Node");
        }
        #endregion

        #region PacketClass

        [TestMethod]
        public void TestPacketClassCanBeAccessedByName() {
            this.AssertClassIsAccessibleByNameAndNamed("VBLanLibrary.Packet", "Packet");
        }
        [TestMethod]
        public void TestPacketClassAggregationIsConsistent() {
            this.AssertClassAggregationIsConsistent("VBLanLibrary.Packet");
        }
        [TestMethod]
        public void TestPacketClassIsComplete() {
            FAMIX.Class value = (FAMIX.Class)importer.Types.Named("VBLanLibrary.Packet");
            Assert.AreEqual(value.SuperInheritances.Count, 1);
            Assert.AreEqual(value.SuperInheritances.First<FAMIX.Inheritance>().superclass.name, "Object");
            Assert.AreEqual(value.Attributes.Count, 3);
            Assert.AreEqual(value.Methods.Count, 1 + 6 );
        }

        #endregion PacketClass

        #region AbstractDestinationAddress

        [TestMethod]
        public void TestAbstractDestinationAddressClassCanBeAccessedByName() {
            this.AssertClassIsAccessibleByNameAndNamed("VBLanLibrary.AbstractDestinationAddress", "AbstractDestinationAddress");
        }
        [TestMethod]
        public void TestAbstractDestinationAddressClassAggregationIsConsistent() {
            this.AssertClassAggregationIsConsistent("VBLanLibrary.AbstractDestinationAddress");
        }
        [TestMethod]
        public void TestAbstractDestinationAddressClassIsComplete() {
            FAMIX.Class value = (FAMIX.Class)importer.Types.Named("VBLanLibrary.AbstractDestinationAddress");
            Assert.AreEqual(value.SuperInheritances.Count, 1);
            Assert.AreEqual(value.SuperInheritances.First<FAMIX.Inheritance>().superclass.name, "Object");
            Assert.AreEqual(value.Attributes.Count, 0);
            Assert.AreEqual(value.Methods.Count, 0);
        }
        #endregion

        #region SingleDestinationAddress
        [TestMethod]
        public void TestSingleDestinationAddressClassCanBeAccessedByName() {
            this.AssertClassIsAccessibleByNameAndNamed("VBLanLibrary.SingleDestinationAddress", "SingleDestinationAddress");
        }
        [TestMethod]
        public void TestSingleDestinationAddressClassIsConsistent() {
            this.AssertClassAggregationIsConsistent("VBLanLibrary.SingleDestinationAddress");
        }
        [TestMethod]
        public void TestSingleDestinationAddressClassIsComplete() { 
            FAMIX.Class value = (FAMIX.Class)importer.Types.Named("VBLanLibrary.SingleDestinationAddress");
            Assert.AreEqual(value.SuperInheritances.Count, 1);
            Assert.AreEqual(value.SuperInheritances.First<FAMIX.Inheritance>().superclass.name, "AbstractDestinationAddress");
            Assert.AreEqual(value.Attributes.Count, 1);
            Assert.AreEqual(value.Methods.Count, 2 + 3);
        }
        #endregion

        #region PrinterServer
        [TestMethod]
        public void TestPrinterServerClassCanBeAccessedByName() {
            this.AssertClassIsAccessibleByNameAndNamed("VBLanLibrary.PrinterServer", "PrinterServer");
        }
        [TestMethod]
        public void TestPrinterServerClassAggregationIsConsistent() {
            this.AssertClassAggregationIsConsistent("VBLanLibrary.PrinterServer");
        }
        [TestMethod]
        public void TestPrinterServerClassIsComplete() {
            FAMIX.Class value = (FAMIX.Class)importer.Types.Named("VBLanLibrary.PrinterServer");
            Assert.AreEqual(value.SuperInheritances.Count, 1);
            Assert.AreEqual(value.SuperInheritances.First<FAMIX.Inheritance>().superclass.name, "OutputServer");
            Assert.AreEqual(value.Attributes.Count, 1);
            Assert.AreEqual(value.Methods.Count, 2 + 1 + 1);
        }
        #endregion

        #region OutputServer
        [TestMethod]
        public void TestOutputServerClassCanBeAccessedByName() {
            this.AssertClassIsAccessibleByNameAndNamed("VBLanLibrary.OutputServer", "OutputServer");
        }
        [TestMethod]
        public void TestOutputServerClassAggregationIsConsistent() {
            this.AssertClassAggregationIsConsistent("VBLanLibrary.OutputServer");
        }
        [TestMethod]
        public void TestOutputServerClassIsComplete() {
            FAMIX.Class value = (FAMIX.Class) importer.Types.Named("VBLanLibrary.OutputServer") ;
            Assert.AreEqual(value.SuperInheritances.Count, 1);
            Assert.AreEqual(value.SuperInheritances.First<FAMIX.Inheritance>().superclass.name, "Node");
            // There is one Property defined. It should be detected regardless the abstract nature of the class 
            Assert.AreEqual(value.Attributes.Count, 1);
            // There is one abstract Method defined. It should be detected regardless the abstract nature of the class  and method. Plus the setters/ 
            Assert.AreEqual(value.Methods.Count, 2 + 3);
        }
        #endregion

        #region FileServer
        [TestMethod]
        public void TestFileServerClassCanBeAccessedByName() {
            this.AssertClassIsAccessibleByNameAndNamed("VBLanLibrary.FileServer", "FileServer");
        }
        [TestMethod]
        public void TestFileServerClassAggregationIsConsistent() {
            this.AssertClassAggregationIsConsistent("VBLanLibrary.FileServer");
        }
        [TestMethod]
        public void TestFileServerClassIsComplete() {
            FAMIX.Class value = (FAMIX.Class)importer.Types.Named("VBLanLibrary.FileServer");
            Assert.AreEqual(value.SuperInheritances.Count, 1);
            Assert.AreEqual(value.SuperInheritances.First<FAMIX.Inheritance>().superclass.name, "OutputServer");
            Assert.AreEqual(value.Attributes.Count, 1);
            Assert.AreEqual(value.Methods.Count, 2+2);
        }
        #endregion

        #region WorkStation
        [TestMethod]
        public void TestWorkStationClassCanBeAccessedByName() {
            this.AssertClassIsAccessibleByNameAndNamed("VBLanLibrary.WorkStation", "WorkStation");
        }
        [TestMethod]
        public void TestWorkStationClassAggregationIsConsistent() {
            this.AssertClassAggregationIsConsistent("VBLanLibrary.WorkStation");
        }
        [TestMethod]
        public void TestWorkStationClassIsComplete() {
            FAMIX.Class value = (FAMIX.Class)importer.Types.Named("VBLanLibrary.WorkStation");
            Assert.AreEqual(value.SuperInheritances.Count, 1);
            Assert.AreEqual(value.SuperInheritances.First<FAMIX.Inheritance>().superclass.name, "Object");
            Assert.AreEqual(value.Attributes.Count, 0);
            Assert.AreEqual(value.Methods.Count, 0);
        }
        #endregion


    }
}
