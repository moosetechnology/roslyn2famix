using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace FamixTest.VisualBasicUnitTest {
    [TestClass]
    public class ClassWithFieldsAndProperties : VisualBasicUnitTest {
        FAMIX.Class ClassElement;

        #region SettingUp

        [TestInitialize]
        public void ParseStudyCase() {
            this.Import(@"
                    Class Example 
                        Public AStringValue As string
                        Private ReadOnly AnIntValue As integer
                        Protected ADateValue  As Date
                        Property AnEmptySetGetProperty As String
                        Public Property AnNonEmptySetGetProperty As String
                        Get
                            Return AStringValue
                        End Get
                        Set(value As String)
                            AStringValue = value
                        End Set
                        End Property
                    End Class
            ");
            ClassElement = this.GetElement<FAMIX.Class>(0);
        }

        #endregion

        #region Assertions


        public FAMIX.Attribute Attribute(int AttributeNumber) {
            return ClassElement.Attributes[AttributeNumber];
        }

        public void AssertAttributeHasModifier(int AttributeNumber, string modifier) {
            Assert.IsTrue(Attribute(AttributeNumber).Modifiers.Any(a => a == modifier));
        }
        public void AssertAttributeHasModifiers(int AttributeNumber, int Amount) {
            Assert.AreEqual(Attribute(AttributeNumber).Modifiers.Count(), Amount);
        }

        public void AssertAttributeHasTypeNamed(int AttributeNumber, string Name) {
            Assert.AreEqual(Attribute(AttributeNumber).declaredType.name, Name);
        }

        public void AssertAttributeIsNamed(int AttributeNumber, string Name) {
            Assert.AreEqual(Attribute(AttributeNumber).name, Name);
        }
        

        #endregion

        #region Attribute 5

        [TestMethod]
        public void ParseStudyCase_Attribute5HasOneModifier() {
            AssertAttributeHasModifiers(4, 1);
        }
        [TestMethod]
        public void ParseStudyCase_Attribute5HasModifierPublic() {
            AssertAttributeHasModifier(4, "Public");
        }
        [TestMethod]
        public void ParseStudyCase_Attribute5HasSetterAndGetter() {
            Assert.AreEqual(((Net.Property)Attribute(4)).getter.kind, "PropertyGet");
            Assert.AreEqual(((Net.Property)Attribute(4)).setter.kind, "PropertySet");
        }
        [TestMethod]
        public void ParseStudyCase_Attribute5SetterAndGetterReliesOnAttribute1() {
            Assert.Fail();
        }

        [TestMethod]
        public void ParseStudyCase_Attribute5IsTypedString() {
            AssertAttributeHasTypeNamed(4, "String");
        }
        [TestMethod]
        public void ParseStudyCase_Attribute5IsNamed() {
            AssertAttributeIsNamed(4, "AnNonEmptySetGetProperty");
        }
        #endregion

        #region Attribute 4

        [TestMethod]
        public void ParseStudyCase_Attribute4HasNoneModifiers() {
            AssertAttributeHasModifiers(3, 0);
        }
        
        [TestMethod]
        public void ParseStudyCase_Attribute4IsTypedString() {
            AssertAttributeHasTypeNamed(3, "String");
        }
        [TestMethod]
        public void ParseStudyCase_Attribute4IsNamed() {
            AssertAttributeIsNamed(3, "AnEmptySetGetProperty");
        }
        #endregion

        #region Attribute 3

        [TestMethod]
        public void ParseStudyCase_Attribute3HasOneModifiers() {
            AssertAttributeHasModifiers(2, 1);
        }
        [TestMethod]
        public void ParseStudyCase_Attribute3HasModifierProtected() {
            AssertAttributeHasModifier(2, "Protected");
        }
        [TestMethod]
        public void ParseStudyCase_Attribute3IsTypedDate() {
            AssertAttributeHasTypeNamed(2, "DateTime");
        }
        [TestMethod]
        public void ParseStudyCase_Attribute3IsNamed() {
            AssertAttributeIsNamed(2, "ADateValue");
        }
        #endregion

        #region Attribute 2

        [TestMethod]
        public void ParseStudyCase_Attribute2HasTwoModifiers() {
            AssertAttributeHasModifiers(1, 2);
        }
        [TestMethod]
        public void ParseStudyCase_Attribute2HasModifiersPrivateAndReadOnly() {
            AssertAttributeHasModifier(1, "Private");
            AssertAttributeHasModifier(1, "ReadOnly");
        }
        [TestMethod]
        public void ParseStudyCase_Attribute2IsTypedInt32() {
            AssertAttributeHasTypeNamed(1, "Int32");
        }
        [TestMethod]
        public void ParseStudyCase_Attribute2IsNamed() {
            AssertAttributeIsNamed(1, "AnIntValue");
        }
        #endregion

        #region Attribute 1
       
        [TestMethod]
        public void ParseStudyCase_Attribute1HasOneModifiers() {
            AssertAttributeHasModifiers(0, 1);
        }
        [TestMethod]
        public void ParseStudyCase_Attribute1HasModifiersPublicAndWriteOnly() {
            AssertAttributeHasModifier(0, "Public");
        }
        [TestMethod]
        public void ParseStudyCase_Attribute1IsTypedString() {
            AssertAttributeHasTypeNamed(0, "String");
        }
        [TestMethod]
        public void ParseStudyCase_Attribute1IsNamed() {
            AssertAttributeIsNamed(0, "AStringValue");
        }
        #endregion

        [TestMethod]
        public void ParseStudyCase_Contains5Attributes() {
            Assert.AreEqual(ClassElement.Attributes.Count(), 5);
        }



    }
}
