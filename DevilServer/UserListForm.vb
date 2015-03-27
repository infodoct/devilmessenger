Public Class UserListForm
    Inherits System.Windows.Forms.Form

#Region " Code généré par le Concepteur Windows Form "

    Public Sub New()
        MyBase.New()

        'Cet appel est requis par le Concepteur Windows Form.
        InitializeComponent()

        'Ajoutez une initialisation quelconque après l'appel InitializeComponent()

    End Sub

    'La méthode substituée Dispose du formulaire pour nettoyer la liste des composants.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Requis par le Concepteur Windows Form
    Private components As System.ComponentModel.IContainer

    'REMARQUE : la procédure suivante est requise par le Concepteur Windows Form
    'Elle peut être modifiée en utilisant le Concepteur Windows Form.  
    'Ne la modifiez pas en utilisant l'éditeur de code.
    Friend WithEvents ListBox1 As System.Windows.Forms.ListBox
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Button3 As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.ListBox1 = New System.Windows.Forms.ListBox
        Me.Button2 = New System.Windows.Forms.Button
        Me.Button1 = New System.Windows.Forms.Button
        Me.Button3 = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'ListBox1
        '
        Me.ListBox1.Location = New System.Drawing.Point(16, 16)
        Me.ListBox1.MultiColumn = True
        Me.ListBox1.Name = "ListBox1"
        Me.ListBox1.Size = New System.Drawing.Size(424, 173)
        Me.ListBox1.TabIndex = 0
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(16, 200)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(424, 24)
        Me.Button2.TabIndex = 4
        Me.Button2.Text = "Ajouter utilisateur"
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(16, 232)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(424, 24)
        Me.Button1.TabIndex = 5
        Me.Button1.Text = "Supprimer utilisateur"
        '
        'Button3
        '
        Me.Button3.Location = New System.Drawing.Point(16, 264)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(424, 24)
        Me.Button3.TabIndex = 6
        Me.Button3.Text = "Sauvegarder les changement"
        '
        'UserListForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(456, 293)
        Me.Controls.Add(Me.Button3)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.ListBox1)
        Me.Name = "UserListForm"
        Me.Text = "Liste des utilisateurs"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub UserListForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        DisplayUserList()
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        AddUser()
        DisplayUserList()
    End Sub
    Private Sub DisplayUserList()
        Dim U As User
        ListBox1.Items.Clear()
        ListBox1.Items.Add("Username" + vbTab + "mot de passe")
        For Each U In UserList
            ListBox1.Items.Add(U.UserName + vbTab + vbTab + U.Password)
        Next
    End Sub
    Private Sub AddUser()
        Dim U As New User
        Dim eU As User

        U.UserName = InputBox("Pseudo")

        If U.UserName.IndexOf(Separator) >= 0 Then
            MsgBox("N'utilisez pas '" + Separator + "' dans le pseudo", MsgBoxStyle.Critical)
            Exit Sub
        End If

        If U.UserName.Trim = "" Or U.UserName.Length > 15 Then
            Exit Sub
        End If

        For Each eU In UserList
            If eU.UserName.ToUpper = U.UserName.ToUpper Then
                MsgBox("Un autre utilisateur existe avec ce pseudo !", MsgBoxStyle.Critical)
                Exit Sub
            End If
        Next
        U.Password = InputBox("Mot de passe")
        If U.Password.IndexOf(Separator) >= 0 Then
            MsgBox("N'utilisez pas '" + Separator + "' dans le mot de passe", MsgBoxStyle.Critical)
            Exit Sub
        End If
        U.RealName = InputBox("Nom")
        If U.RealName.IndexOf(Separator) >= 0 Then
            MsgBox("N'utilisez pas '" + Separator + "' dans le nom", MsgBoxStyle.Critical)
            Exit Sub
        End If
        UserList.Add(U)
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        RemoveUser()
        DisplayUserList()
    End Sub
    Private Sub RemoveUser()
        Dim R() As String, U As User
        If ListBox1.SelectedIndex >= 0 Then
            R = ListBox1.Items(ListBox1.SelectedIndex).split(vbTab)
            For Each U In UserList
                If U.UserName = R(0) Then
                    UserList.Remove(U)
                    Exit For
                End If
            Next
        End If
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Dim FSW As New IO.StreamWriter("password.txt", False)
        Dim Line As String
        Dim U As User
        For Each U In UserList
            Line = U.UserName + "|" + U.Password + "|" + U.RealName
            FSW.WriteLine(Line)
        Next
        FSW.Close()
    End Sub
End Class
