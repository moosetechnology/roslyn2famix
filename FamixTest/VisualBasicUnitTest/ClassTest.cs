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

        public void ParseEmptyClassInsideNamespace() {
            this.Import(@"
                Namespace TopLevel 
                    Class Example 
                    End Class
                End Namespace  
            ");
        }

        public void ParseClassWithEmptyClassInsideInsideNamespace() {
            this.Import(@"
                Namespace TopLevel 
                    Class Example    
                        Class Inner 
                        End Class
                    End Class
                End Namespace  
            ");
        }

        #endregion


        [TestMethod]
        public void EmptyClassModelContainsOneOnlyElement() {
            this.ParseEmptyClass();
            this.AssertAmountElements(1);
            
        }

        [TestMethod]
        public void EmptyClassIsClass() {
            this.ParseEmptyClass();
            Assert.IsTrue(this.GetElement<FAMIX.Entity>(0) is FAMIX.Class);
        }
        [TestMethod]
        public void EmptyClassIsEmpty() {
            this.ParseEmptyClass();
            FAMIX.Class element = this.GetElement<FAMIX.Class>(0);
            Assert.IsTrue(element.Types.Count == 0);
        }
        [TestMethod]
        public void EmptyClassIsNamedExample() {
            this.ParseEmptyClass();
            FAMIX.Class element = this.GetElement<FAMIX.Class>(0);
            Assert.AreEqual(element.name, "Example");
        }


        [TestMethod]
        public void NotEmptyClassIsClass() {
            this.ParseClassWithEmptyClassInside();
            FAMIX.Entity element = this.GetElement<FAMIX.Entity>(0);
            Assert.IsTrue(element is FAMIX.Class);
        }
        [TestMethod]
        public void NotEmptyClassIsNotEmpty() {
            this.ParseClassWithEmptyClassInside();
            FAMIX.Class element = this.GetElement<FAMIX.Class>(0);
            Assert.AreEqual(element.Types.Count, 1);
        }
        [TestMethod]
        public void NotEmptyClassContainsInnerClass() {
            this.ParseClassWithEmptyClassInside();
            FAMIX.Class element = this.GetElement<FAMIX.Class>(0);
            FAMIX.Class inner = this.GetElement<FAMIX.Class>(1);
            Assert.AreEqual(element.Types[0], inner);
        }


        [TestMethod]
        public void NotEmptyClassModelContainsTwoElement() {
            this.ParseClassWithEmptyClassInside();
            this.AssertAmountElements(2);
        }

        [TestMethod]
        public void InnerClassOfNotEmptyClassIsNamedInner() {
            this.ParseClassWithEmptyClassInside();
            FAMIX.Class element = this.GetElement<FAMIX.Class>(1);
            Assert.AreEqual(element.name, "Example.Inner");
        }

        [TestMethod]
        public void InnerClassOfNotEmptyClassIsContainedInExampleClass() {
            this.ParseClassWithEmptyClassInside();
            FAMIX.Class example = this.GetElement<FAMIX.Class>(0);
            FAMIX.Class inner = this.GetElement<FAMIX.Class>(1);
            
            Assert.AreEqual(inner.container, example);
        }

        [TestMethod]
        public void NotEmptyClassIsNamedExample() {
            this.ParseClassWithEmptyClassInside();
            FAMIX.Class element = this.GetElement<FAMIX.Class>(0);
            Assert.AreEqual(element.name, "Example");
        }


    }
}
