Imports VBLanLibrary

Public Class PrinterServer
    Inherits OutputServer
    Private printer As IPrinter

    Public Class XPrinter
        Implements IPrinter
        Private UselessNumber As Integer

        Public Sub Print(contents As String, rv As Boolean) Implements IPrinter.Print
        End Sub

        Public Function IdNumber() As Integer
            Return UselessNumber
        End Function

    End Class
    Public Sub PrintServer()
        Const forInner As Integer = 0
        Me.printer = New XPrinter()
    End Sub
    Public Overrides Sub Output(packet As Packet)
            Throw New NotImplementedException()
        End Sub
    End Class
