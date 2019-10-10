using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

using Fame;

namespace FameTest {
    [TestClass]
    public class DragonTest {

        [FamePackage("RPG")]
        [FameDescription("Dragon")]
        class Dragon {


            private List<Treasure> hoard = new List<Treasure>();

            [FameProperty(Name = "hoard")]
            public List<Treasure> Hoard {
                get { return hoard; }
                set { hoard = value; }
            }

            public void AddHoard(Treasure t) {
                hoard.Add(t);
            }
        }

        [FamePackage("RPG")]
        [FameDescription("Treasure")]
        class Treasure {
        }
        MetaRepository metaRepo;
        Tower t;
        [TestInitialize]
        public void setUp() {
            t = new Fame.Tower();
            metaRepo = t.metamodel;
            metaRepo.RegisterType(typeof(Dragon));
            metaRepo.RegisterType(typeof(Treasure));


        }

        [TestMethod]
        public void TestMetamodelCanCreateInstancesOfEachClass() {
            Dragon leDragon = t.model.New<Dragon>("RPG.Dragon");
            Treasure deltaHoard = t.model.New<Treasure>("RPG.Treasure");
            leDragon.AddHoard(deltaHoard);
            Assert.IsNotNull(leDragon);
            Assert.IsNotNull(deltaHoard);
            Assert.IsTrue(t.model.GetElements().Count == 2);
        }
        [TestMethod]
        public void TestMseFileCreatedWithTwoInstances() {
            Dragon leDragon = t.model.New<Dragon>("RPG.Dragon");
            Treasure deltaHoard = t.model.New<Treasure>("RPG.Treasure");
            t.model.ExportMSEFile("out.mse");
        }


        [TestMethod]
        public void TestMetaRepoContainsBothDefinedClasses() {
            Assert.IsNotNull(metaRepo.Get("RPG.Dragon"));
            Assert.IsNotNull(metaRepo.Get("RPG.Treasure"));
        }
    }
}
