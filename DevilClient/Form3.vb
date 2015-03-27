Imports System.Net.Sockets
Imports System.Net

Public Class DevilForm
    Inherits System.Windows.Forms.Form
    Dim Client As TcpClient
    Dim RealClient As TcpClient
    Dim T As System.Threading.Thread
    Dim Authentified As Boolean

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
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents ListBox1 As System.Windows.Forms.ListBox
    Friend WithEvents Button4 As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.Button1 = New System.Windows.Forms.Button
        Me.Button2 = New System.Windows.Forms.Button
        Me.Button3 = New System.Windows.Forms.Button
        Me.ListBox1 = New System.Windows.Forms.ListBox
        Me.Button4 = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(40, 32)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(192, 96)
        Me.Button1.TabIndex = 0
        Me.Button1.Text = "Connect"
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(32, 152)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(208, 72)
        Me.Button2.TabIndex = 1
        Me.Button2.Text = "Send"
        '
        'Button3
        '
        Me.Button3.Location = New System.Drawing.Point(32, 240)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(200, 56)
        Me.Button3.TabIndex = 2
        Me.Button3.Text = "Close"
        '
        'ListBox1
        '
        Me.ListBox1.Location = New System.Drawing.Point(312, 40)
        Me.ListBox1.Name = "ListBox1"
        Me.ListBox1.Size = New System.Drawing.Size(240, 251)
        Me.ListBox1.TabIndex = 3
        '
        'Button4
        '
        Me.Button4.Location = New System.Drawing.Point(32, 328)
        Me.Button4.Name = "Button4"
        Me.Button4.Size = New System.Drawing.Size(216, 56)
        Me.Button4.TabIndex = 4
        Me.Button4.Text = "Authentifier"
        '
        'DevilForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(584, 405)
        Me.Controls.Add(Me.Button4)
        Me.Controls.Add(Me.ListBox1)
        Me.Controls.Add(Me.Button3)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Button1)
        Me.Name = "DevilForm"
        Me.Text = "Client"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim MT As New MT
        Client = New TcpClient
        RealClient = New TcpClient
        Try
            Client.Connect("127.0.0.1", 13000)
            MT.Client = Client
            MT.RealClient = RealClient
            MT.L = ListBox1
            T = New Threading.Thread(AddressOf MT.Listen)
            T.Start()
            ListBox1.Items.Add("Connected")
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        SendData("199")

    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        SendData("Petit message")
    End Sub
    Sub SendData(ByVal S As String)
        Dim Bytes(1024) As Byte
        Dim Stream As NetworkStream

        Stream = RealClient.GetStream()
        Bytes = System.Text.Encoding.ASCII.GetBytes(S)

        Stream.Write(Bytes, 0, Bytes.Length)
    End Sub

    Private Sub Form3_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Form3_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        If T.ThreadState = Threading.ThreadState.Running Then
            T.Abort()
        End If
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        Dim Username As String, Password As String
        Username = InputBox("Username")
        Password = InputBox("Password")
        SendData("102|" + Username + "|" + Password + "|")
    End Sub
End Class
Public Class MT
    Public L As ListBox
    Public Client As TcpClient
    Public RealClient As TcpClient
    Public Sub Listen()
        Dim RealConnection As Boolean
        Dim Stream As NetworkStream
        Dim RealStream As NetworkStream
        Dim Bytes(1024) As Byte
        Dim I As Int32
        Dim Data As String
        Dim R() As String
        Tex("Thread started")
        Stream = Client.GetStream
        While True
            While Stream.DataAvailable
                Bytes.Clear(Bytes, 0, Bytes.Length)
                Stream.Read(Bytes, 0, Bytes.Length)
                Data = System.Text.Encoding.ASCII.GetString(Bytes)
                R = Data.Split("|")
                Tex(Data)
                Select Case Val(R(0))
                    Case 101
                        RealClient.Connect("127.0.0.1", Val(R(1)))
                        Tex("Port changed")
                        RealStream = RealClient.GetStream
                        RealConnection = True
                    Case 103
                        Tex("Authentifié avec success")
                End Select

            End While
            While RealConnection
                While RealStream.DataAvailable


                    Bytes.Clear(Bytes, 0, Bytes.Length)
                    RealStream.Read(Bytes, 0, Bytes.Length)
                    Data = System.Text.Encoding.ASCII.GetString(Bytes)
                    Tex(Data)
                End While
            End While
        End While

    End Sub
    Private Sub Tex(ByVal S As String)
        L.Items.Add(S)
    End Sub
End Class
