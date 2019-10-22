Public MustInherit Class OutputServer
    Inherits Node
    Public Property ServerType As String = Nothing

    Public Overrides Sub Accept(packet As Packet)

        If (packet.Addressee.IsDestinationFor(Me.Name)) Then
            Me.Output(packet)
        Else
            Send(packet)
        End If
    End Sub

    Public Overrides Function CanOutput() As Boolean
        Return True
    End Function
    Public MustOverride Sub Output(packet As Packet)
End Class
