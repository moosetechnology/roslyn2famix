Imports System.ComponentModel.DataAnnotations

Public Class SingleDestinationAddress
    Inherits AbstractDestinationAddress

    <Custom(SomeArbitraryValue:="ToastForIt")>
    <Required(ErrorMessage:="Invalid ID number")>
    Public Property Id As String

    Public Overrides Function IsDestinationFor(anId As String) As Boolean
        Return EqualsSingleId(anId)
    End Function
    Public Function EqualsSingleId(anId As String) As Boolean
        Return anId.Equals(Id)
    End Function

    Public Function EqualsMultiple(address As AbstractDestinationAddress) As Boolean
        Return False
    End Function
End Class
