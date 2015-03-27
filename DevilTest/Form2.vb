Imports System.Net
Imports System.Net.Sockets
Public Class Form2
    Inherits System.Windows.Forms.Form
    Dim Port As Int32 = 13000
    Dim LocalAddr As IPAddress = IPAddress.Parse("127.0.0.1")
    Dim Server As New TcpListener(LocalAddr, Port)

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
    Friend WithEvents Button1 As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.ListBox1 = New System.Windows.Forms.ListBox
        Me.Button1 = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'ListBox1
        '
        Me.ListBox1.Location = New System.Drawing.Point(72, 88)
        Me.ListBox1.Name = "ListBox1"
        Me.ListBox1.Size = New System.Drawing.Size(384, 173)
        Me.ListBox1.TabIndex = 0
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(80, 24)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(232, 48)
        Me.Button1.TabIndex = 1
        Me.Button1.Text = "Button1"
        '
        'Form2
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(520, 485)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.ListBox1)
        Me.Name = "Form2"
        Me.Text = "Server"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub Form2_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
       
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim ListenThread1 As System.Threading.Thread
        Dim ListenThread2 As System.Threading.Thread
        ListenThread1 = New Threading.Thread(AddressOf Listen)
        ListenThread2 = New Threading.Thread(AddressOf Listen)
        ListenThread1.Start()
        ListenThread2.Start()
    End Sub
    Public Sub Tex(ByVal S As String)
        ListBox1.Items.Add(S)
    End Sub
    Public Sub Listen()
        Dim Client As TcpClient
        Dim Stream As NetworkStream
        Dim I, L As Int32
        Dim ConnectionEstablished As Boolean

        Dim Data As String
        Dim Bytes(1024) As Byte
        Try
            Server.Start()
            Tex("Server started")
            While Not IsClosed

                If Server.Pending Then
                    Tex("Connection pending")
                    Client = Server.AcceptTcpClient()
                    Tex("Connection accepted")
                    ConnectionEstablished = True
                    Stream = Client.GetStream()
                End If

                While ConnectionEstablished

                    If Stream.DataAvailable Then
                        I = Stream.Read(Bytes, 0, Bytes.Length)
                        Data = System.Text.Encoding.ASCII.GetString(Bytes, 0, I)
                        Tex(Data)
                        Bytes = System.Text.Encoding.ASCII.GetBytes(Data.ToUpper)
                        Stream.Write(Bytes, 0, Bytes.Length)

                        If Data = "fin" Then
                            Client.Close()
                            Tex("Connection closed")
                            ConnectionEstablished = False
                        End If
                    End If
                End While


            End While

        Catch ex As SocketException
            MsgBox("Exception : " + ex.ToString)
        End Try
    End Sub
End Class
