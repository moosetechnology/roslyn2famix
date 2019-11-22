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
            Public Sub Dummy()
            End Sub
            Public Sub Dummy(index As Integer)
            End Sub
            Public Sub ExampleIf()
                If (True) Then
                    Me.Dummy()
                End If
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
                For index As Integer = 1 To 5
                    Me.Dummy()
                Next
            End Sub
            Public Sub ExampleForSecond()
                For index As Integer = 1 To Me.ExampleReturn()
                    Me.Dummy(index)
                Next
            End Sub
            Public Function ExampleReturn() As Integer
                Return 1
            End Function

        End Class
            ");
        }

        #endregion


        public void TestExampleReturn() {
            Assert.AreEqual(MethodNamed("ExampleReturn").numberOfConditionals, 0);
            Assert.AreEqual(MethodNamed("ExampleReturn").numberOfLoops, 0);
            Assert.AreEqual(MethodNamed("ExampleReturn").numberOfLinesOfCode, 1);
            Assert.AreEqual(MethodNamed("ExampleReturn").OutgoingInvocations.Count(), 0);
            Assert.AreEqual(MethodNamed("ExampleReturn").IncomingInvocations.Count(), 2);
        }
        public void TestExampleForSecond() {
            Assert.AreEqual(MethodNamed("ExampleForSecond").numberOfConditionals, 1);
            Assert.AreEqual(MethodNamed("ExampleForSecond").numberOfLoops, 1);
            Assert.AreEqual(MethodNamed("ExampleForSecond").numberOfLinesOfCode, 3);
            Assert.AreEqual(MethodNamed("ExampleForSecond").OutgoingInvocations.Count(), 1);
            Assert.AreEqual(MethodNamed("ExampleForSecond").IncomingInvocations.Count(), 0);
        }
        public void TestExampleForSecondInvocations() {
            Assert.AreEqual(MethodNamed("ExampleForSecond").OutgoingInvocations[0].receiver, MethodNamed("ExampleForSecond").OutgoingInvocations[0].sender);
            Assert.AreEqual(MethodNamed("ExampleForSecond").OutgoingInvocations[0].signature, "Dummy(Integer)");
        }
        public void TestExampleFor() {
            Assert.AreEqual(MethodNamed("ExampleFor").numberOfConditionals, 1);
            Assert.AreEqual(MethodNamed("ExampleFor").numberOfLoops, 1);
            Assert.AreEqual(MethodNamed("ExampleFor").numberOfLinesOfCode, 3);
            Assert.AreEqual(MethodNamed("ExampleFor").OutgoingInvocations.Count(), 1);
            Assert.AreEqual(MethodNamed("ExampleFor").IncomingInvocations.Count(), 0);
        }
        public void TestExampleForInvocations() {
            Assert.AreEqual(MethodNamed("ExampleFor").OutgoingInvocations[0].receiver, MethodNamed("ExampleFor").OutgoingInvocations[0].sender);
            Assert.AreEqual(MethodNamed("ExampleFor").OutgoingInvocations[0].signature, "Dummy");
        }
        public void TestExampleWhile() {
            Assert.AreEqual(MethodNamed("ExampleWhile").numberOfConditionals, 1);
            Assert.AreEqual(MethodNamed("ExampleWhile").numberOfLoops, 1);
            Assert.AreEqual(MethodNamed("ExampleWhile").numberOfLinesOfCode, 3);
            Assert.AreEqual(MethodNamed("ExampleWhile").OutgoingInvocations.Count(), 1);
            Assert.AreEqual(MethodNamed("ExampleWhile").IncomingInvocations.Count(), 0);
        }
        public void TestExampleWhileInvocations() {
            Assert.AreEqual(MethodNamed("ExampleWhile").OutgoingInvocations[0].receiver, MethodNamed("ExampleWhile").OutgoingInvocations[0].sender);
            Assert.AreEqual(MethodNamed("ExampleWhile").OutgoingInvocations[0].signature, "Dummy");
        }
        public void TestDummy() {
            Assert.AreEqual(MethodNamed("Dummy").ReceivingInvocations.Count(), 5);
            foreach (FAMIX.Invocation invocation in MethodNamed("Dummy").ReceivingInvocations) {
                Assert.AreEqual(invocation.sender, invocation.receiver);
            }
        }
        public void TestExampleIf () {
            Assert.AreEqual(MethodNamed("ExampleIf").numberOfConditionals, 1);
            Assert.AreEqual(MethodNamed("ExampleIf").numberOfLinesOfCode, 3);
            Assert.AreEqual(MethodNamed("ExampleIf").OutgoingInvocations.Count(), 1);
            Assert.AreEqual(MethodNamed("ExampleIf").IncomingInvocations.Count(), 0);
        }
        public void TestExampleIfInvocations() {
            Assert.AreEqual(MethodNamed("ExampleIf").OutgoingInvocations.First().receiver, MethodNamed("ExampleIf").OutgoingInvocations.First().sender);
            Assert.AreEqual(MethodNamed("ExampleIf").OutgoingInvocations.First().signature, "Dummy");
        }

        public void TestExampleIfElse() {
            Assert.AreEqual(MethodNamed("ExampleIfElse").numberOfConditionals, 1);
            Assert.AreEqual(MethodNamed("ExampleIfElse").numberOfLinesOfCode, 5);
            Assert.AreEqual(MethodNamed("ExampleIfElse").OutgoingInvocations.Count(), 2);
            Assert.AreEqual(MethodNamed("ExampleIfElse").IncomingInvocations.Count(), 0);
        }
        public void TestExampleIfElseInvocations() {
            Assert.AreEqual(MethodNamed("ExampleIfElse").OutgoingInvocations[0].receiver, MethodNamed("ExampleIfElse").OutgoingInvocations[0].sender);
            Assert.AreEqual(MethodNamed("ExampleIfElse").OutgoingInvocations[0].signature, "Dummy");
            Assert.AreEqual(MethodNamed("ExampleIfElse").OutgoingInvocations[1].receiver, MethodNamed("ExampleIfElse").OutgoingInvocations[1].sender);
            Assert.AreEqual(MethodNamed("ExampleIfElse").OutgoingInvocations[1].signature, "ExampleReturn");
        }
        

    }
}
