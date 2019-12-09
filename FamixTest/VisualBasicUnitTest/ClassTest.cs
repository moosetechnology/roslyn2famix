using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FamixTest.VisualBasicUnitTest {
    [TestClass]
    public class ClassTest : VisualBasicUnitTest {


        #region SettingUp
        public void ParseEmptyClass() {
            this.Import(@"
                    Class Example 
                    End Class
            ");
        }
        public void ParseClassWithEmptyClassInside() {
            this.Import(@"
               Class Example    
                    Class Inner 
                    End Class
               End Class
            ");
        }
        public void ParseEmptyMustInheritClass() {
            this.Import(@"
                    public MustInherit Class Example 
                    End Class
            ");
        }
        public void ParseEmptyNonInheritableClass() {
            this.Import(@"
                    protected NotInheritable Class Example 
                    End Class
            ");
        }
        public void ParseEmptyShadowsClass() {
            this.Import(@"
                    private Shadows Class Example 
                    End Class
            ");
        }
        #endregion

        #region Visibility
        [TestMethod]
        public void PrivateClassIsPrivate () {
            this.ParseEmptyShadowsClass();
            Assert.IsTrue(this.GetElement<FAMIX.Class>(1).isPrivate);
        }
        [TestMethod]
        public void PublicClassIsPublic() {
            this.ParseEmptyMustInheritClass();
            Assert.IsTrue(this.GetElement<FAMIX.Class>(1).isPublic);
        }
        [TestMethod]
        public void ProtectedClassIsProtected() {
            this.ParseEmptyNonInheritableClass();
            this.AssertAmountElements(2);
            Assert.IsTrue(this.GetElement<FAMIX.Class>(1).isProtected);
        }
        #endregion

        #region Shadows
        [TestMethod]
        public void EmptyNotShadowsClassModelContainsOneOnlyElement() {
            this.ParseEmptyShadowsClass();
            this.AssertAmountElements(2);
        }

        [TestMethod]
        public void EmptyShadowsClassIsClass() {
            this.ParseEmptyShadowsClass();
            Assert.IsTrue(this.GetElement<FAMIX.Entity>(1) is FAMIX.Class);
        }
        [TestMethod]
        public void EmptyShadowsClassIsEmpty() {
            this.ParseEmptyShadowsClass();
            FAMIX.Class element = this.GetElement<FAMIX.Class>(1);
            Assert.IsTrue(element.Types.Count == 0);
        }
        [TestMethod]
        public void EmptyShadowsClassIsNamedExample() {
            this.ParseEmptyShadowsClass();
            FAMIX.Class element = this.GetElement<FAMIX.Class>(1);
            Assert.AreEqual(element.name, "Example");
        }
        [TestMethod]
        public void EmptyShadowsClassIsNotAbstract() {
            this.ParseEmptyShadowsClass();
            FAMIX.Class element = this.GetElement<FAMIX.Class>(1);
            Assert.IsFalse(element.isAbstract);
        }

        [TestMethod]
        public void EmptyShadowsClassIsNotFinal() {
            this.ParseEmptyShadowsClass();
            FAMIX.Class element = this.GetElement<FAMIX.Class>(1);
            Assert.IsFalse(element.isFinal);
        }

        [TestMethod]
        public void EmptyShadowsClassIsShadowing () {
            this.ParseEmptyShadowsClass();
            FAMIX.Class element = this.GetElement<FAMIX.Class>(1);
            Assert.IsTrue(element.isShadow);
        }



        #endregion

        #region NotInheritable
        [TestMethod]
        public void EmptyNotInheritableClassModelContainsOneOnlyElement() {
            this.ParseEmptyNonInheritableClass();
            this.AssertAmountElements(2);
        }
        [TestMethod]
        public void EmptyNotInheritableClassIsClass() {
            this.ParseEmptyNonInheritableClass();
            Assert.IsTrue(this.GetElement<FAMIX.Entity>(1) is FAMIX.Class);
        }
        [TestMethod]
        public void EmptyNotInheritableClassIsEmpty() {
            this.ParseEmptyNonInheritableClass();
            FAMIX.Class element = this.GetElement<FAMIX.Class>(1);
            Assert.IsTrue(element.Types.Count == 0);
        }
        [TestMethod]
        public void EmptyNotInheritabletClassIsNamedExample() {
            this.ParseEmptyNonInheritableClass();
            FAMIX.Class element = this.GetElement<FAMIX.Class>(1);
            Assert.AreEqual(element.name, "Example");
        }
        [TestMethod]
        public void EmptyNotInheritableClassIsNotAbstract() {
            this.ParseEmptyNonInheritableClass();
            FAMIX.Class element = this.GetElement<FAMIX.Class>(1);
            Assert.IsFalse(element.isAbstract);
        }

        [TestMethod]
         public void EmptyNotInheritableIsFinal() {
            this.ParseEmptyNonInheritableClass();
            FAMIX.Class element = this.GetElement<FAMIX.Class>(1);
            Assert.IsTrue(element.isFinal);
        }

        [TestMethod]
        public void EmptyNotInheritableIsNotShadowing() {
            this.ParseEmptyNonInheritableClass();
            FAMIX.Class element = this.GetElement<FAMIX.Class>(1);
            Assert.IsFalse(element.isShadow);
        }
      



        #endregion

        #region MustInherit
        [TestMethod]
        public void EmptyMustInheritClassModelContainsOneOnlyElement() {
            this.ParseEmptyMustInheritClass();
            this.AssertAmountElements(2);
        }

        [TestMethod]
        public void EmptyMustInheritClassIsClass() {
            this.ParseEmptyMustInheritClass();
            Assert.IsTrue(this.GetElement<FAMIX.Entity>(1) is FAMIX.Class);
        }
        [TestMethod]
        public void EmptyMustInheritClassIsEmpty() {
            this.ParseEmptyMustInheritClass();
            FAMIX.Class element = this.GetElement<FAMIX.Class>(1);
            Assert.IsTrue(element.Types.Count == 0);
        }
        [TestMethod]
        public void EmptyMustInheritClassIsNamedExample() {
            this.ParseEmptyMustInheritClass();
            FAMIX.Class element = this.GetElement<FAMIX.Class>(1);
            Assert.AreEqual(element.name, "Example");
        }
        [TestMethod]
        public void EmptyMustInheritClassIsAbstract() {
            this.ParseEmptyMustInheritClass();
            FAMIX.Class element = this.GetElement<FAMIX.Class>(1);
            Assert.IsTrue(element.isAbstract);
        }

        [TestMethod]
        public void EmptyMustInheritIsNotFinal() {
            this.ParseEmptyMustInheritClass();
            FAMIX.Class element = this.GetElement<FAMIX.Class>(1);
            Assert.IsFalse(element.isFinal);
        }

        [TestMethod]
        
        public void EmptyMustInheritIsNotShadowing() {
            this.ParseEmptyMustInheritClass();
            FAMIX.Class element = this.GetElement<FAMIX.Class>(1);
            Assert.IsFalse(element.isShadow);
        }


        #endregion

        #region EmptyClass
        [TestMethod]
        public void EmptyClassModelContainsOneOnlyElement() {
            this.ParseEmptyClass();
            this.AssertAmountElements(2);
            
        }
        [TestMethod]
        public void EmptyClassIsClass() {
            this.ParseEmptyClass();
            Assert.IsTrue(this.GetElement<FAMIX.Entity>(1) is FAMIX.Class);
        }
        [TestMethod]
        public void EmptyClassIsEmpty() {
            this.ParseEmptyClass();
            FAMIX.Class element = this.GetElement<FAMIX.Class>(1);
            Assert.IsTrue(element.Types.Count == 0);
        }
        [TestMethod]
        public void EmptyClassIsNamedExample() {
            this.ParseEmptyClass();
            FAMIX.Class element = this.GetElement<FAMIX.Class>(1);
            Assert.AreEqual(element.name, "Example");
        }
        [TestMethod]
        public void EmptyClassIsNotAbstract() {
            this.ParseEmptyClass();
            FAMIX.Class element = this.GetElement<FAMIX.Class>(1);
            Assert.IsFalse(element.isAbstract);
            
        }
        [TestMethod]
        public void EmptyClassIsNotFinal() {
            this.ParseEmptyClass();
            FAMIX.Class element = this.GetElement<FAMIX.Class>(1);
            Assert.IsFalse(element.isFinal);
        }

        [TestMethod]
        public void EmptyClassIsNotShadowing() {
            this.ParseEmptyClass();
            FAMIX.Class element = this.GetElement<FAMIX.Class>(1);
            Assert.IsFalse(element.isShadow);
        }


        #endregion

        #region NotEmptyClass

        [TestMethod]
        public void NotEmptyClassIsClass() {
            this.ParseClassWithEmptyClassInside();
            FAMIX.Entity element = this.GetElement<FAMIX.Entity>(1);
            Assert.IsTrue(element is FAMIX.Class);
        }
        [TestMethod]
        public void NotEmptyClassIsNotEmpty() {
            this.ParseClassWithEmptyClassInside();
            FAMIX.Class element = this.GetElement<FAMIX.Class>(1);
            Assert.AreEqual(element.Types.Count, 1);
        }
        [TestMethod]
        public void NotEmptyClassContainsInnerClass() {
            this.ParseClassWithEmptyClassInside();
            FAMIX.Class element = this.GetElement<FAMIX.Class>(1);
            FAMIX.Class inner = this.GetElement<FAMIX.Class>(2);
            Assert.AreEqual(element.Types[0], inner);
        }


        [TestMethod]
        public void NotEmptyClassModelContainsThreeElement() {
            this.ParseClassWithEmptyClassInside();
            this.AssertAmountElements(3);
        }

        [TestMethod]
        public void InnerClassOfNotEmptyClassIsNamedInner() {
            this.ParseClassWithEmptyClassInside();
            FAMIX.Class element = this.GetElement<FAMIX.Class>(2);
            Assert.AreEqual(element.name, "Example.Inner");
        }

        [TestMethod]
        public void InnerClassOfNotEmptyClassIsContainedInExampleClass() {
            this.ParseClassWithEmptyClassInside();
            FAMIX.Class example = this.GetElement<FAMIX.Class>(1);
            FAMIX.Class inner = this.GetElement<FAMIX.Class>(2);
            
            Assert.AreEqual(inner.container, example);
        }

        [TestMethod]
        public void NotEmptyClassIsNamedExample() {
            this.ParseClassWithEmptyClassInside();
            FAMIX.Class element = this.GetElement<FAMIX.Class>(1);
            Assert.AreEqual(element.name, "Example");
        }

        #endregion

    }
}
