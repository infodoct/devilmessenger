Imports System.Net
Imports System.Net.Sockets

Public Class ServerForm
    Inherits System.Windows.Forms.Form
    Dim Socket As Socket
    Dim IpAd As IPAddress
    Dim GoodPort As Int32
    Dim ConnectedUserList As New ArrayList
    Dim RSList As New ArrayList
    Dim T As System.Threading.Thread
    Dim RsID As Int32
    Dim DebugOn As Boolean


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
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Button3 As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.ListBox1 = New System.Windows.Forms.ListBox
        Me.TextBox1 = New System.Windows.Forms.TextBox
        Me.Button1 = New System.Windows.Forms.Button
        Me.Button3 = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'ListBox1
        '
        Me.ListBox1.Location = New System.Drawing.Point(8, 16)
        Me.ListBox1.Name = "ListBox1"
        Me.ListBox1.Size = New System.Drawing.Size(696, 238)
        Me.ListBox1.TabIndex = 0
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(8, 256)
        Me.TextBox1.Multiline = True
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(696, 232)
        Me.TextBox1.TabIndex = 1
        Me.TextBox1.Text = ""
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(8, 504)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(200, 24)
        Me.Button1.TabIndex = 2
        Me.Button1.Text = "Buffers"
        '
        'Button3
        '
        Me.Button3.Location = New System.Drawing.Point(472, 504)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(192, 24)
        Me.Button3.TabIndex = 4
        Me.Button3.Text = "Gestion des utilisateurs"
        '
        'ServerForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(720, 541)
        Me.Controls.Add(Me.Button3)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.ListBox1)
        Me.Name = "ServerForm"
        Me.Text = "The Devil Server"
        Me.ResumeLayout(False)

    End Sub

#End Region
    Private Sub Tex(ByVal s As String)
        ListBox1.Items.Add(s)
    End Sub
    Private Sub ListenThread()

        Dim Client As Socket
        Dim Bytes(BytesSize) As Byte
        Dim Data As String
        Dim RS As RealServer
        Dim RST As System.Threading.Thread

        Dim IpEp As IPEndPoint


        IpAd = IPAddress.Any

        IpEp = New IPEndPoint(IpAd, PortNumber)

        Socket = New Socket(IpAd.AddressFamily, SocketType.Stream, ProtocolType.Tcp)

        Socket.Bind(IpEp)

        Socket.Listen(5)

        While True

            Client = Socket.Accept()

            'GiveMeAPort()

            'Client.Send(System.Text.Encoding.ASCII.GetBytes("101|" + GoodPort.ToString))

            'Client.Close()

            RS = New RealServer
            RsID = RsID + 1

            RS.WhenAClientConnect = New WhenAClientConnectDelegate(AddressOf WhenAClientConnect)
            RS.WhenAClientDisconnect = New WhenAClientDisconnectDelegate(AddressOf WhenAClientDisconnect)
            RS.TransferMsg = New TransferMsgDelegate(AddressOf TransferMsg)
            RS.UserList = UserList
            RS.ConnectedUserList = ConnectedUserList
            RS.Client = Client
            RS.ID = RsID
            RS.Tx = AddressOf Tex

            RST = New System.Threading.Thread(AddressOf RS.Listen)

            RSList.Add(RS)

            RST.Start()
        End While
    End Sub

    Private Sub Form4_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim FSR As New System.IO.StreamReader(PasswordFileName)
        Dim FData As String
        Dim FR() As String
        Dim FI As Int32

        FData = FSR.ReadLine()

        While Not FData Is Nothing
            FR = FData.Split(Separator)
            UserList.Add(New User(FR(0), FR(1), FR(2)))
            FData = FSR.ReadLine()
        End While

        FSR.Close()
        
        T = New System.Threading.Thread(AddressOf ListenThread)
        T.Start()

        RsID = 0
    End Sub
    Private Sub ServerForm_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        T.Abort()
        Socket.Close()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        TextBox1.Text = ""
        Dim R As RealServer
        For Each R In RSList
            TextBox1.Text = TextBox1.Text + ControlChars.CrLf + "Sent " + R.SBuffer + ControlChars.CrLf + "Received " + R.RBuffer + ControlChars.CrLf
        Next
    End Sub
    Public Delegate Sub WhenAClientConnectDelegate(ByVal CS As ClientState)
    Public Delegate Sub WhenAClientDisconnectDelegate(ByRef CS As ClientState, ByRef RS As RealServer)
    Public Delegate Sub TransferMsgDelegate(ByVal Msg As String, ByVal RSUserName As String, ByRef RS As RealServer)
    Public Sub WhenAClientConnect(ByVal Cs As ClientState)
        Dim Rs As RealServer
        Dim U As New User
        U.UserName = Cs.UserName
        U.Password = Cs.Password
        U.RealName = Cs.RealName

        ConnectedUserList.Add(U)

        If DebugOn Then Tex("A client has just connected " + Cs.UserName)
        For Each Rs In RSList
            Rs.ConnectedUserList = ConnectedUserList
            Rs.SendUserList()
        Next

        DisplayConnectedUserList()
    End Sub
    Public Sub WhenAClientDisconnect(ByRef CS As ClientState, ByRef RS As RealServer)
        Dim U As User, R As RealServer
        If DebugOn Then Tex("A client has just disconnected " + CS.UserName)


        For Each U In ConnectedUserList
            If CS.UserName = U.UserName Then
                ConnectedUserList.Remove(U)
                Exit For
            End If
        Next

        For Each R In RSList
            If RS.ID <> R.ID Then
                R.ConnectedUserList = ConnectedUserList
                R.SendUserList()
            End If
        Next

        For Each R In RSList
            If R.ID = RS.ID Then
                R.Client.Close()
                RSList.Remove(R)
                Exit For
            End If
        Next

        DisplayConnectedUserList()
    End Sub
    Public Sub TransferMsg(ByVal Msg As String, ByVal DestUserName As String, ByRef RS As RealServer)
        Dim RSDest As RealServer
        For Each RSDest In RSList
            If RSDest.RSUserName.ToUpper = DestUserName.ToUpper Then
                RSDest.SendData("113" + Separator + RS.RSUserName + Separator + Msg)
                Exit For
            End If
        Next
    End Sub
    Private Sub DisplayConnectedUserList()
        Dim U As User
        ListBox1.Items.Clear()

        ListBox1.Items.Add("Pseudo" + vbTab + "Nom")
        For Each U In ConnectedUserList
            ListBox1.Items.Add(U.UserName + vbTab + U.RealName)
        Next
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim U As New User, RS As RealServer
        U.UserName = InputBox("UN")
        U.Password = InputBox("PW")

        UserList.Add(U)

        For Each RS In RSList
            RS.SendUserList()
        Next
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Dim F As New UserListForm
        F.Show()
    End Sub
End Class