Public Class Custom
    Inherits Attribute
    Property SomeArbitraryValue As String

    Public Sub New(s As String)
        SomeArbitraryValue = s
    End Sub

    Public Sub New()
    End Sub

End Class
