Public Class User
    Public UserName As String
    Public Password As String
    Public RealName As String
    Public Sub New(ByVal UN As String, ByVal PWD As String, ByVal RN As String)
        UserName = UN
        Password = PWD
        RealName = RN
    End Sub
    Public Sub New()

    End Sub
End Class