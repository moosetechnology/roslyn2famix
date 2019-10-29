using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FamixTest.VisualBasicUnitTest {
    [TestClass]
    public class NamespaceTest : VisualBasicUnitTest {


        #region Assertions
        public void AssertNamespaceContains(FAMIX.Namespace nspace, FAMIX.Namespace nspaceInside) {
            Assert.IsTrue(nspace.ChildScopes.Contains(nspaceInside));
        }
        #endregion
        #region ParsingMethods
        public void ParseEmptyNamespace() {
            this.Import(@"
                Namespace TopLevel 
                End Namespace  
            ");
        }

        public void ParseNamespaceWithNamespaceInside() {
            this.Import(@"
                 Namespace TopLevel 
                        Namespace OtherNamespace
                        End Namespace  
                 End Namespace  
            ");
        }

        public void ParseNamespaceWithManyNamespacesInside() {
            this.Import(@"
                Namespace TopLevel 
                        Namespace OtherNamespace
                        End Namespace 
                        Namespace AnOtherNamespace
                        End Namespace  
                 End Namespace 
            ");
        }
        #endregion

        #region Empty Namespace
        [TestMethod]
        public void TestEmptyNamespaceIsRecogniced () {
            this.ParseEmptyNamespace();
            Assert.IsTrue(MetaModel.GetElements()[0] is FAMIX.Namespace);
        }
        [TestMethod]
        public void TestEmptyNamespaceGeneratesOnlyOneElement() {
            this.ParseEmptyNamespace();
            Assert.AreEqual(1, MetaModel.GetElements().Count);
        }
        [TestMethod]
        public void TestEmptyNamespaceGeneratesAnEmptyNamespace() {
            this.ParseEmptyNamespace();
            FAMIX.Namespace nspace = (FAMIX.Namespace)MetaModel.GetElements()[0];
            Assert.AreEqual(0, nspace.ChildScopes.Count);
        }
        #endregion


        #region SingleElement Namespace
        [TestMethod]
        public void TestSingleElementNamespaceIsRecogniced() {
            this.ParseNamespaceWithNamespaceInside();
            Assert.IsTrue(MetaModel.GetElements()[0] is FAMIX.Namespace);
        }
        [TestMethod]
        public void TestSingleElementNamespaceGeneratesOnlyTwoElements() {
            this.ParseNamespaceWithNamespaceInside();
            Assert.AreEqual(2, MetaModel.GetElements().Count);
        }
        [TestMethod]
        public void TestSingleElementNamespaceGeneratesANamespaceWithOneChilds() {
            this.ParseNamespaceWithNamespaceInside();
            FAMIX.Namespace nspace = (FAMIX.Namespace)MetaModel.GetElements()[0];
            Assert.AreEqual(1, nspace.ChildScopes.Count);
        }
        public void TestSingleElementNamespaceContainsTheSecondNamespace() {
            this.ParseNamespaceWithNamespaceInside();
            FAMIX.Namespace nspace = (FAMIX.Namespace)MetaModel.GetElements()[0];
            this.AssertNamespaceContains(nspace, (FAMIX.Namespace)MetaModel.GetElements()[1]);
        }
        #endregion

         
        #region MultipleElement Namespace
        [TestMethod]
        public void TestMultipleElementNamespaceIsRecogniced() {
            this.ParseNamespaceWithManyNamespacesInside();
            Assert.IsTrue(MetaModel.GetElements()[0] is FAMIX.Namespace);
        }
        [TestMethod]
        public void TestMultipleElementNamespaceGeneratesOnlyMultipleElements() {
            this.ParseNamespaceWithManyNamespacesInside();
            Assert.AreEqual(3, MetaModel.GetElements().Count);
        }
        [TestMethod]
        public void TestMultipleElementNamespaceGeneratesANamespaceWithManyChilds() {
            this.ParseNamespaceWithManyNamespacesInside();
            FAMIX.Namespace nspace = (FAMIX.Namespace)MetaModel.GetElements()[0];
            Assert.AreEqual(2, nspace.ChildScopes.Count);
        }
        public void TestMultipleElementNamespaceContainsTheFurtherNamespaces() {
            this.ParseNamespaceWithManyNamespacesInside();
            FAMIX.Namespace nspace = (FAMIX.Namespace)MetaModel.GetElements()[0];
            this.AssertNamespaceContains(nspace, (FAMIX.Namespace)MetaModel.GetElements()[1]);
            this.AssertNamespaceContains(nspace, (FAMIX.Namespace)MetaModel.GetElements()[2]);
        }
        #endregion
    }
}
