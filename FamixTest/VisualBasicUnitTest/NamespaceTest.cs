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
            Assert.IsTrue(this.GetElement<FAMIX.Entity>(1) is FAMIX.Namespace);
        }
        [TestMethod]
        public void TestEmptyNamespaceGeneratesOnlyTwoElements() {
            this.ParseEmptyNamespace();
            this.AssertAmountElements(2);
        }
        [TestMethod]
        public void TestEmptyNamespaceGeneratesAnEmptyNamespace() {
            this.ParseEmptyNamespace();
            FAMIX.Namespace nspace = this.GetElement<FAMIX.Namespace>(1);
            Assert.AreEqual(0, nspace.ChildScopes.Count);
        }
        #endregion


        #region SingleElement Namespace
        [TestMethod]
        public void TestSingleElementNamespaceIsRecogniced() {
            this.ParseNamespaceWithNamespaceInside();
            Assert.IsTrue(this.GetElement<FAMIX.Entity>(1) is FAMIX.Namespace);
        }
        [TestMethod]
        public void TestSingleElementNamespaceGeneratesOnlyTwoElements() {
            this.ParseNamespaceWithNamespaceInside();
            this.AssertAmountElements(3);
        }
        [TestMethod]
        public void TestSingleElementNamespaceGeneratesANamespaceWithOneChilds() {
            this.ParseNamespaceWithNamespaceInside();
            FAMIX.Namespace nspace = this.GetElement<FAMIX.Namespace>(1);
            Assert.AreEqual(1, nspace.ChildScopes.Count);
        }
        public void TestSingleElementNamespaceContainsTheSecondNamespace() {
            this.ParseNamespaceWithNamespaceInside();
            FAMIX.Namespace nspace = this.GetElement<FAMIX.Namespace>(1);
            this.AssertNamespaceContains(nspace, this.GetElement<FAMIX.Namespace>(1));
        }
        #endregion

         
        #region MultipleElement Namespace
        [TestMethod]
        public void TestMultipleElementNamespaceIsRecogniced() {
            this.ParseNamespaceWithManyNamespacesInside();
            Assert.IsTrue(MetaModel.GetElements()[1] is FAMIX.Namespace);
        }
        [TestMethod]
        public void TestMultipleElementNamespaceGeneratesOnlyMultipleElements() {
            this.ParseNamespaceWithManyNamespacesInside();
            this.AssertAmountElements(4);
        }
        [TestMethod]
        public void TestMultipleElementNamespaceGeneratesANamespaceWithManyChilds() {
            this.ParseNamespaceWithManyNamespacesInside();
            FAMIX.Namespace nspace = this.GetElement<FAMIX.Namespace>(1);
            Assert.AreEqual(2, nspace.ChildScopes.Count);
        }
        public void TestMultipleElementNamespaceContainsTheFurtherNamespaces() {
            this.ParseNamespaceWithManyNamespacesInside();
            FAMIX.Namespace nspace = this.GetElement<FAMIX.Namespace>(1);
            this.AssertNamespaceContains(nspace, this.GetElement<FAMIX.Namespace>(2));
            this.AssertNamespaceContains(nspace, this.GetElement<FAMIX.Namespace>(3));
        }
        #endregion
    }
}
