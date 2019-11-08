using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace FamixTest.VisualBasicUnitTest {
    [TestClass]
    public class ClassUsingGenericsInMethodsAndState : VisualBasicUnitTest {


        #region SettingUp

        public void ParseEmptySubParameterLess() {
            this.Import(@"
                    Class Example 
                        Public Overridable Sub ExampleSub()
                        End Sub 
                    End Class
            ");
        }

        #endregion


        #region ParseEmptySubManyParameters
        [TestMethod]
        public void ParseEmptySubManyParameters_SevenEight() {
            Assert.Fail();
        }


        #endregion





    }
}
