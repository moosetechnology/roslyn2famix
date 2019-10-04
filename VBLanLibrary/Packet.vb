Imports Microsoft.VisualBasic

Public Class Packet
    Public Property Contents As String

    Public Property Addressee As SingleDestinationAddress

    Public Property Originator As Node

    Public Sub PrintOn(stringBuilder As System.Text.StringBuilder)
        If (Originator IsNot Nothing) Then
            stringBuilder.Append(" Coming From ").Append(Originator.Name)
        End If
        stringBuilder.Append(" Addressed to ").Append(Addressee.Id).Append("With Contents: ").Append(Contents)
    End Sub

End Class
