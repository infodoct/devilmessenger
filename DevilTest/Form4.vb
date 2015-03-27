Imports System.Net
Imports System.Net.Sockets

Public Class Form4
    Inherits System.Windows.Forms.Form
    Dim IpAd As IPAddress
    Dim GoodPort As Int32
    Dim UserList As New ArrayList
    Dim ConnectedUsers As New ArrayList


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
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.ListBox1 = New System.Windows.Forms.ListBox
        Me.SuspendLayout()
        '
        'ListBox1
        '
        Me.ListBox1.Location = New System.Drawing.Point(8, 16)
        Me.ListBox1.Name = "ListBox1"
        Me.ListBox1.Size = New System.Drawing.Size(696, 238)
        Me.ListBox1.TabIndex = 0
        '
        'Form4
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(720, 273)
        Me.Controls.Add(Me.ListBox1)
        Me.Name = "Form4"
        Me.Text = "Server"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub
    Private Sub Tex(ByVal s As String)
        ListBox1.Items.Add(s)
    End Sub
    Private Sub ListenThread()
        Dim Socket As Socket
        Dim Client As Socket
        Dim Bytes(1024) As Byte
        Dim Data As String
        Dim RS As RealServer
        Dim RST As System.Threading.Thread

        

        Dim IpEp As IPEndPoint
        Dim IpHe As IPHostEntry
        Tex("LT Started")


        IpHe = Dns.GetHostByAddress("127.0.0.1")

        IpAd = IpHe.AddressList(0)

        IpEp = New IPEndPoint(IpAd, 13000)

        Socket = New Socket(IpAd.AddressFamily, SocketType.Stream, ProtocolType.Tcp)

        Socket.Bind(IpEp)

        Socket.Listen(5)

        Tex("Server On")
        While True

            Client = Socket.Accept()

            GiveMeAPort()

            Client.Send(System.Text.Encoding.ASCII.GetBytes("101|" + GoodPort.ToString))

            Client.Close()

            RS = New RealServer

            RS.IpAd = IpAd
            RS.Port = GoodPort
            RS.UserList = UserList
            RS.Tx = AddressOf Tex

            RST = New System.Threading.Thread(AddressOf RS.Listen)

            RST.Start()
        End While


    End Sub

    Private Sub Form4_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        UserList.Add(New User("Zino", "zinozino", "Mohamed"))
        UserList.Add(New User("Reda", "redareda", "Soufi Reda"))
        Dim T As New System.Threading.Thread(AddressOf ListenThread)
        GoodPort = 13000
        T.Start()
    End Sub
    Private Function GiveMeAPort() As Int32
        Dim TestSocket As Socket

        Try
            TestSocket = New Socket(IpAd.AddressFamily, SocketType.Stream, ProtocolType.Tcp)
            TestSocket.Bind(New IPEndPoint(IpAd, GoodPort))
            TestSocket.Close()
        Catch ex As Exception
            GoodPort = GoodPort + 1
            GiveMeAPort()
        End Try


    End Function


End Class
Public Class RealServer
    Public Port As Int32
    Public IpAd As IPAddress
    Public Tx As Tex
    Public UserList As ArrayList
    Dim Client As Socket

    Public Delegate Sub Tex(ByVal S As String)
    Public Sub Listen()
        Dim RealSocket As New Socket(IpAd.AddressFamily, SocketType.Stream, ProtocolType.Tcp)

        Dim Bytes(1024) As Byte
        Dim Data As String
        Dim R() As String
        Dim U As User
        Dim UserName, Password, RealName As String
        Dim AuthentifiedUser As Boolean

        Tx("RST started")

        Tx("Client on port " + Port.ToString)


        RealSocket.Bind(New IPEndPoint(IpAd, Port))
        RealSocket.Listen(1)
        Client = RealSocket.Accept()
        Tx("Connection accepted")
        While Client.Connected
            If Client.Available > 0 Then
                Bytes.Clear(Bytes, 0, Bytes.Length)
                Client.Receive(Bytes)
                Data = System.Text.Encoding.ASCII.GetString(Bytes)
                Tx("de " + Port.ToString + " " + Data)
                R = Data.Split("|")
                Select Case Val(R(0))
                    Case 199
                        Tx("Connection closed")
                        AuthentifiedUser = False
                        Client.Close()
                        RealSocket.Close()
                    Case 102
                        UserName = R(1)
                        Password = R(2)
                        For Each U In UserList
                            If U.UserName = UserName And U.Password = Password Then
                                AuthentifiedUser = True
                                Tx("Authentifié " + UserName + " " + Password)
                                SendData("103")
                            End If
                        Next
                End Select

            End If
        End While
    End Sub
    Public Sub SendData(ByVal S As String)
        Client.Send(System.Text.Encoding.ASCII.GetBytes(S))
    End Sub
End Class
Public Class User
    Public UserName As String
    Public Password As String
    Public RealName As String
    Public Sub New(ByVal UN As String, ByVal PWD As String, ByVal RN As String)
        UserName = UN
        Password = PWD
        RealName = RN
    End Sub
End Class

