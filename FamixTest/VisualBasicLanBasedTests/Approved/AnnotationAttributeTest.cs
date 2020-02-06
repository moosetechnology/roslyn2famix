﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace FamixTest.VisualBasic {
    [TestClass]
    public class AnnotationAttributeTest : LanProjectVisualBasicLoader {
        [TestMethod]
        public void IngestedAnnotationTypes() {
            var allAnnotationTypes = (importer.AllElementsOfType<FAMIX.AnnotationType>()).ToList();
            Assert.AreEqual(allAnnotationTypes.Count(), 9);
        }

        [TestMethod]
        public void BuiltInAnnotationIsDigested() {
            //TODO: System.ComponentModel.DataAnnotations.RequiredAttribute 

            var annotation = importer.Types.Named("RequiredAttribute");
            Assert.IsNotNull(annotation);
            Assert.IsTrue(annotation is FAMIX.AnnotationType);
            Assert.IsTrue(annotation.isStub);
        }
        [TestMethod]
        public void CustomAnnotationIsDigested() {
            var annotation = importer.Types.Named("VBLanLibrary.Custom");
            Assert.IsNotNull(annotation);
            Assert.IsTrue(annotation is FAMIX.AnnotationType);
            Assert.IsFalse(annotation.isStub);
        }
        [TestMethod]
        public void CustomAnnotationHasOneAttribute() {
            var annotation = importer.Types.Named("VBLanLibrary.Custom");
            Assert.AreEqual(1, annotation.Attributes.Count);
            Assert.AreEqual("SomeArbitraryValue", annotation.Attributes.First().name);
        }
    }
}
