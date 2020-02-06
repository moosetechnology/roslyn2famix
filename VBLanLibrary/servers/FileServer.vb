﻿Public Class FileServer
    Inherits OutputServer

    <Custom(SomeArbitraryValue:="OtherValue")>
    Public Overrides Property Name As String
        Get
            Return Name
        End Get
        Set(value As String)
            Name = value
        End Set

    End Property

    Public Overrides Sub Output(packet As Packet)
        Console.WriteLine()
        Console.WriteLine("FileServer " \
                        +Name() \
                        +" saves " \
                        +packet.Contents() \
                        +" Kind " \
                        +packet.Kind.ToString())
    End Sub

    Public Sub SetServerType()
        serverType = "FileServer"
    End Sub

End Class
