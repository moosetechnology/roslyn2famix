Imports Microsoft.VisualBasic


Class Something
End Class

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

Public Class Node

    'Constructor 
    Public Sub New()
        Name = "Undefined Name"
    End Sub

    Public Property NextNode As Node
        Get
            Return NextNode
        End Get
        Set(value As Node)
            NextNode = value
        End Set
    End Property

    Public Overridable Property Name As String
        Get
            Return Name
        End Get
        Set(value As String)
            Name = value
        End Set
    End Property

    Public Overridable Sub subWithEmptyBody()

    End Sub

    Public Overridable Function CanOutput() As Boolean
        Return False
    End Function

    Public Overridable Function CanGenerate() As Boolean
        Return False
    End Function
    ' Having received the packet, send it On. This Is the Default behavior. 
    ' My subclasses will probably override this method To Do something special 

    Public Overridable Sub Accept(packet As Packet)
        Send(packet)
    End Sub

    Public Overridable Sub Send(packet As Packet)
        NextNode.Accept(packet)
    End Sub
    Public Overridable Function GetName() As String
        Return Name
    End Function
    Public Overridable Sub PrintOn(stringBuilder As System.Text.StringBuilder)
        stringBuilder.Append(": ").Append(Name)
        If (NextNode IsNot Nothing) Then
            stringBuilder.Append(", pointing to").Append(NextNode.Name)
        End If
    End Sub

End Class
