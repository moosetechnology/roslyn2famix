using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FamixTest.VisualBasicUnitTest {
    [TestClass]
    public class InMethodDescriptionTests : VisualBasicUnitTest {

        #region SettingUp
        [TestInitialize]
        public void ParseMultipleMethods() {
            this.Import(@"
        Public Class Example
            Public Sub ExampleIf()
                If (True) Then
                    Dim a, b, c As Single, x, y As Double, i As Integer
                    Me.Dummy()
                End If
            End Sub
            Public Function ExampleReturn() As Integer
                Return 1
            End Function
            Public Sub Dummy(index As Integer)
                Dim a, b, c As Single, x, y As Double, i As Integer
            End Sub
            Public Sub Dummy()
                Dim variable As Example
                Dim collection(20) As Example
            End Sub
            Public Sub ExampleIfElse()
                If (False) Then
                    Me.Dummy()
                Else
                    Me.ExampleReturn()
                End If
            End Sub
            Public Sub ExampleWhile()
                While (True)
                    Me.Dummy()
                End While
            End Sub
            Public Sub ExampleFor()
                Dim index As Integer
                For index = 1 To 5
                    Me.Dummy()
                Next
            End Sub
            Public Sub ExampleForSecond()
                Dim index As Integer
                For index = 1 To 20
                    Me.Dummy(index)
                Next
            End Sub
        End Class
            ");
        }

        #endregion

        [TestMethod]
        public void TestExampleReturn() {
            Assert.AreEqual(MethodOfSignature("ExampleReturn()").numberOfConditionals, 0);
            Assert.AreEqual(MethodOfSignature("ExampleReturn()").numberOfLoops, 0);
            Assert.AreEqual(MethodOfSignature("ExampleReturn()").OutgoingInvocations.Count(), 0);
            Assert.AreEqual(MethodOfSignature("ExampleReturn()").IncomingInvocations.Count(), 1);
        }
        [TestMethod]
        public void TestExampleForSecond() {
            Assert.AreEqual(MethodOfSignature("ExampleForSecond()").numberOfConditionals, 1);
            Assert.AreEqual(MethodOfSignature("ExampleForSecond()").numberOfLoops, 1);
            Assert.AreEqual(MethodOfSignature("ExampleForSecond()").OutgoingInvocations.Count(), 1);
            Assert.AreEqual(MethodOfSignature("ExampleForSecond()").IncomingInvocations.Count(), 0);
        }
        [TestMethod]
        public void TestExampleForSecondInvocations() {
            Assert.AreEqual(MethodOfSignature("ExampleForSecond()").OutgoingInvocations[0].Candidates[0], MethodOfSignature("Dummy(Int32)"));
            Assert.AreEqual(MethodOfSignature("ExampleForSecond()").OutgoingInvocations[0].Candidates[0].signature, "Dummy(Int32)");
        }
        [TestMethod]
        public void TestExampleFor() {
            Assert.AreEqual(MethodOfSignature("ExampleFor()").numberOfConditionals, 1);
            Assert.AreEqual(MethodOfSignature("ExampleFor()").numberOfLoops, 1);
            Assert.AreEqual(MethodOfSignature("ExampleFor()").OutgoingInvocations.Count(), 1);
            Assert.AreEqual(MethodOfSignature("ExampleFor()").IncomingInvocations.Count(), 0);
        }
        [TestMethod]
        public void TestExampleForInvocations() {
            Assert.AreEqual(MethodOfSignature("ExampleFor()").OutgoingInvocations[0].Candidates[0], MethodOfSignature("Dummy()"));
            Assert.AreEqual(MethodOfSignature("ExampleFor()").OutgoingInvocations[0].Candidates[0].name, "Dummy");
        }
        public void TestExampleWhile() {
            Assert.AreEqual(MethodOfSignature("ExampleWhile()").numberOfConditionals, 1);
            Assert.AreEqual(MethodOfSignature("ExampleWhile()").numberOfLoops, 1);
            Assert.AreEqual(MethodOfSignature("ExampleWhile()").OutgoingInvocations.Count(), 1);
            Assert.AreEqual(MethodOfSignature("ExampleWhile()").IncomingInvocations.Count(), 0);
        }
        [TestMethod]
        public void TestExampleWhileInvocations() {
            Assert.AreEqual(MethodOfSignature("ExampleWhile()").OutgoingInvocations[0].Candidates[0], MethodOfSignature("Dummy()"));
            Assert.AreEqual(MethodOfSignature("ExampleWhile()").OutgoingInvocations[0].Candidates[0].name, "Dummy");
        }
        [TestMethod]
        public void TestDummy() {
            Assert.AreEqual(MethodOfSignature("Dummy()").IncomingInvocations.Count(), 4);
        }
        [TestMethod]
        public void TestExampleIf () {
            Assert.AreEqual(MethodOfSignature("ExampleIf()").numberOfConditionals, 1);
            Assert.AreEqual(MethodOfSignature("ExampleIf()").OutgoingInvocations.Count(), 1);
            Assert.AreEqual(MethodOfSignature("ExampleIf()").IncomingInvocations.Count(), 0);
        }
        [TestMethod]
        public void TestExampleIfInvocations() {
            Assert.AreEqual(MethodOfSignature("ExampleIf()").OutgoingInvocations.First().Candidates[0].name, "Dummy");
            Assert.AreEqual(MethodOfSignature("ExampleIf()").OutgoingInvocations.First().Candidates[0], MethodOfSignature("Dummy()"));
        }
        [TestMethod]
        public void TestExampleIfElse() {
            Assert.AreEqual(MethodOfSignature("ExampleIfElse()").numberOfConditionals, 1);
            Assert.AreEqual(MethodOfSignature("ExampleIfElse()").OutgoingInvocations.Count(), 2);
            Assert.AreEqual(MethodOfSignature("ExampleIfElse()").IncomingInvocations.Count(), 0);
        }
        [TestMethod]
        public void TestExampleIfElseInvocations() {
            
            Assert.AreEqual(MethodOfSignature("ExampleIfElse()").OutgoingInvocations[1].Candidates[0].name, "ExampleReturn");
            Assert.AreEqual(MethodOfSignature("ExampleIfElse()").OutgoingInvocations[0].Candidates[0].name, "Dummy");
            Assert.AreEqual(MethodOfSignature("ExampleIfElse()").OutgoingInvocations[0].Candidates[0], MethodOfSignature("Dummy()"));
            Assert.AreEqual(MethodOfSignature("ExampleIfElse()").OutgoingInvocations[1].Candidates[0], MethodOfSignature("ExampleReturn()"));
            
        }
        

    }
}
