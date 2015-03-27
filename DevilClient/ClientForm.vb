Imports System.Net.Sockets
Imports System.Net

Public Class ClientForm
    Inherits System.Windows.Forms.Form

    Dim Client As Socket
    Dim NbUser As Int32

    Dim SBuffer As String
    Dim RBuffer As String
    Dim BufferOn As Boolean
    Dim DebugOn As Boolean
    Private Declare Function InitCommonControls Lib "Comctl32.dll" () As Long


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
    Friend WithEvents UsersListBox As System.Windows.Forms.ListBox
    Friend WithEvents BtnEtat As System.Windows.Forms.Button
    Friend WithEvents TextBoxEtat As System.Windows.Forms.TextBox
    Friend WithEvents TextBoxMsg As System.Windows.Forms.TextBox
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents StatusBar1 As System.Windows.Forms.StatusBar
    Friend WithEvents TextBoxReceived As System.Windows.Forms.RichTextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(ClientForm))
        Me.ListBox1 = New System.Windows.Forms.ListBox
        Me.BtnEtat = New System.Windows.Forms.Button
        Me.TextBoxEtat = New System.Windows.Forms.TextBox
        Me.UsersListBox = New System.Windows.Forms.ListBox
        Me.TextBoxMsg = New System.Windows.Forms.TextBox
        Me.Button1 = New System.Windows.Forms.Button
        Me.StatusBar1 = New System.Windows.Forms.StatusBar
        Me.TextBoxReceived = New System.Windows.Forms.RichTextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'ListBox1
        '
        Me.ListBox1.Location = New System.Drawing.Point(112, 328)
        Me.ListBox1.Name = "ListBox1"
        Me.ListBox1.Size = New System.Drawing.Size(240, 186)
        Me.ListBox1.TabIndex = 3
        '
        'BtnEtat
        '
        Me.BtnEtat.Location = New System.Drawing.Point(40, 328)
        Me.BtnEtat.Name = "BtnEtat"
        Me.BtnEtat.Size = New System.Drawing.Size(64, 24)
        Me.BtnEtat.TabIndex = 4
        Me.BtnEtat.Text = "Etat"
        '
        'TextBoxEtat
        '
        Me.TextBoxEtat.Location = New System.Drawing.Point(360, 328)
        Me.TextBoxEtat.Multiline = True
        Me.TextBoxEtat.Name = "TextBoxEtat"
        Me.TextBoxEtat.Size = New System.Drawing.Size(312, 192)
        Me.TextBoxEtat.TabIndex = 5
        Me.TextBoxEtat.Text = ""
        '
        'UsersListBox
        '
        Me.UsersListBox.Location = New System.Drawing.Point(536, 88)
        Me.UsersListBox.Name = "UsersListBox"
        Me.UsersListBox.Size = New System.Drawing.Size(176, 199)
        Me.UsersListBox.TabIndex = 8
        '
        'TextBoxMsg
        '
        Me.TextBoxMsg.Location = New System.Drawing.Point(16, 8)
        Me.TextBoxMsg.Multiline = True
        Me.TextBoxMsg.Name = "TextBoxMsg"
        Me.TextBoxMsg.Size = New System.Drawing.Size(336, 56)
        Me.TextBoxMsg.TabIndex = 10
        Me.TextBoxMsg.Text = ""
        '
        'Button1
        '
        Me.Button1.Enabled = False
        Me.Button1.Location = New System.Drawing.Point(360, 8)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(144, 56)
        Me.Button1.TabIndex = 0
        Me.Button1.Text = "Envoyer"
        '
        'StatusBar1
        '
        Me.StatusBar1.Location = New System.Drawing.Point(0, 503)
        Me.StatusBar1.Name = "StatusBar1"
        Me.StatusBar1.Size = New System.Drawing.Size(720, 22)
        Me.StatusBar1.TabIndex = 13
        Me.StatusBar1.Text = "Déconnecté"
        '
        'TextBoxReceived
        '
        Me.TextBoxReceived.Location = New System.Drawing.Point(16, 88)
        Me.TextBoxReceived.Name = "TextBoxReceived"
        Me.TextBoxReceived.ReadOnly = True
        Me.TextBoxReceived.Size = New System.Drawing.Size(488, 200)
        Me.TextBoxReceived.TabIndex = 14
        Me.TextBoxReceived.Text = ""
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(536, 16)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(176, 48)
        Me.Label1.TabIndex = 15
        Me.Label1.Text = "Choisissez le pseudo de la personne avec laquelle vous voulez discuter ci-dessous" & _
        ""
        '
        'ClientForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(720, 525)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.TextBoxReceived)
        Me.Controls.Add(Me.StatusBar1)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.TextBoxMsg)
        Me.Controls.Add(Me.UsersListBox)
        Me.Controls.Add(Me.TextBoxEtat)
        Me.Controls.Add(Me.BtnEtat)
        Me.Controls.Add(Me.ListBox1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "ClientForm"
        Me.Text = "Devilim (Beta)"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Connect()
        Auth()
    End Sub

    Private Sub Connect()
        Dim AddressString
        Dim S As New SocketBytes
        Dim Rx As New System.Text.RegularExpressions.Regex("[12]?[0-9]?[0-9](\.[12]?[0-9]?[0-9]){3}")
        Dim IpEp As IPEndPoint

        If IO.File.Exists(AddressFileName) Then
            Dim FSR As New IO.StreamReader(AddressFileName)
            AddressString = FSR.ReadLine()
            FSR.Close()
        Else
            AddressString = InputBox("Adresse réseau (ip ou nom)")
            Dim FSW As New IO.StreamWriter(AddressFileName)
            FSW.WriteLine(AddressString)
            FSW.Close()
        End If

        If Rx.Match(AddressString).Length > 0 Then
            IpEp = New IPEndPoint(IPAddress.Parse(AddressString), PortNumber)
        Else
            IpEp = New IPEndPoint(Dns.GetHostByName(AddressString).AddressList(0), PortNumber)
        End If

        Client = New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)

        Try
            Client.Blocking = False
            S.Socket = Client
            Client.BeginConnect(IpEp, New AsyncCallback(AddressOf ConnectCallBack), S)
        Catch ex As SocketException
            MsgBox(ex.ToString)
        End Try
    End Sub
    
    
    Sub SendData(ByVal S As String)
        Dim So As New SocketBytes
        If Not S.EndsWith(Separator) Then
            S = S + Separator
        End If

        So.Bytes = System.Text.Encoding.ASCII.GetBytes(S)
        ReDim Preserve So.Bytes(S.Length)
        So.Bytes(S.Length) = 200

        So.Socket = Client

        If Not S.StartsWith("111") Then
            ClientState.SentPacket = ClientState.SentPacket + 1
        End If

        If BufferOn Then SBuffer = SBuffer + "(" + S.Trim(Chr(0)) + ")"
        Try
            So.Socket.BeginSend(So.Bytes, 0, So.Bytes.Length, SocketFlags.None, New AsyncCallback(AddressOf SendCallBack), So)
        Catch ex As SocketException
        End Try
    End Sub
    Private Sub ProcessMsg(ByVal B() As Byte)
        Dim MsgString As String
        Dim BLoop As Int32
        Dim R() As String
        For BLoop = 0 To B.Length - 1
            If B(BLoop) <> 200 Then
                MsgString = MsgString + Chr(B(BLoop))
            Else
                R = MsgString.Split(Separator)
                ProcessMsg(R)
                MsgString = ""
            End If
        Next
    End Sub
    Private Sub ProcessMsg(ByVal R() As String)
        Dim MsgType As Int32

        MsgType = Val(R(0))
        Select Case MsgType
            Case 101
                'Dim SB As New SocketBytes
                'RealClient = New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
                'SB.Socket = RealClient
                'SB.Socket.Blocking = False
                'SB.Socket.BeginConnect(New IPEndPoint(Dns.GetHostByAddress("127.0.0.1").AddressList(0), Val(R(1))), New AsyncCallback(AddressOf ConnectCallBack), SB)
                'Client.Close()
            Case 103
                ClientState.Authentified = True
                StatusBar1.Text = "Connecté"
                SendOK()
            Case 108
                NbUser = 0
                UsersListBox.Items.Clear()
                SendOK()
            Case 110
                Tex("Users number : " + NbUser.ToString)
                If ClientState.ChattingWith <> "" Then
                    If Not UsersListBox.Items.Contains(ClientState.ChattingWith) Then
                        ClientState.ChattingWith = ""
                        Button1.Enabled = False
                        StatusBar1.Text = "Votre correspondant s'est deconnecté"
                    End If
                End If
                SendOK()
            Case 109
                Tex("User " + R(1))
                NbUser = NbUser + 1
                UsersListBox.Items.Add(R(1))
                SendOK()
            Case 104
                ClientState.ConfirmedSentPacket = ClientState.ConfirmedSentPacket + 1
            Case 112
                MsgBox("Ce Username est déja connecté")
                SendData("199")
                ClientState.Connected = False
                SendOK()
            Case 113
                Dim TempString As String
                TextBoxReceived.SelectionFont = New Font(TextBoxReceived.SelectionFont, FontStyle.Bold)
                TextBoxReceived.SelectionColor = Color.Red
                TextBoxReceived.SelectedText = R(1)     'Username
                TextBoxReceived.SelectionFont = New Font(TextBoxReceived.SelectionFont, FontStyle.Bold)
                TextBoxReceived.SelectionColor = Color.Black
                TextBoxReceived.SelectedText = " dit " + R(2) + vbCrLf      'Message
                SendOK()
            Case 114
                ClientState.Connected = False
                SendData("199")
                SendOK()
        End Select
        Application.DoEvents()
    End Sub
    Private Sub Tex(ByVal S As String)
        ListBox1.Items.Add(S)
    End Sub
    Private Sub SendCallBack(ByVal ar As IAsyncResult)
        Dim SB As SocketBytes
        SB = CType(ar.AsyncState, SocketBytes)
        Try
            SB.Socket.EndSend(ar)
        Catch ex As SocketException
        End Try
    End Sub
    Private Sub SendOK()
        SendData("111")
    End Sub

    Private Sub Auth()
        Dim Username As String, Password As String
        Username = InputBox("Pseudo")
        If Username.Trim = "" Then
            Exit Sub
        End If
        Password = InputBox("Mot de passe")
        SendData("102" + Separator + Username + Separator + Password + Separator)
        ClientState.ChattingWith = ""
    End Sub
    Public Sub Listen(ByVal Socket As Socket)
        Dim S As New SocketBytes
        S.Socket = Socket
        Try
            S.Socket.BeginReceive(S.Bytes, 0, S.Bytes.Length, SocketFlags.None, New AsyncCallback(AddressOf ReceiveCallBack), S)
        Catch ex As SocketException

        End Try

    End Sub
    Public Sub ReceiveCallBack(ByVal ar As IAsyncResult)
        Dim SB As SocketBytes
        Dim I As Int32

        SB = CType(ar.AsyncState, SocketBytes)
        Try
            I = SB.Socket.EndReceive(ar)
            If I > 0 Then
                SB.Data = System.Text.Encoding.ASCII.GetString(SB.Bytes)
                If BufferOn Then RBuffer = RBuffer + "(" + SB.Data.Trim(Chr(0)) + ")"
                ProcessMsg(SB.Bytes)
            End If
        Catch ex As SocketException
            Select Case ex.ErrorCode
                Case 995
                Case 10054
                    ClientState.Connected = False
                    StatusBar1.Text = "Déconnecté"
                Case Else
                    MsgBox(ex.ToString, , ex.ErrorCode)
            End Select
        End Try
        Array.Clear(SB.Bytes, 0, SB.Bytes.Length)
        Try
            SB.Socket.BeginReceive(SB.Bytes, 0, SB.Bytes.Length, SocketFlags.None, New AsyncCallback(AddressOf ReceiveCallBack), SB)
        Catch ex As SocketException
        End Try
    End Sub
    Public Sub ConnectCallBack(ByVal ar As IAsyncResult)
        Dim SB As SocketBytes
        SB = CType(ar.AsyncState, SocketBytes)
        Try
            SB.Socket.EndConnect(ar)
            ClientState.Connected = True
        Catch ex As SocketException

        End Try

        Listen(SB.Socket)
    End Sub
    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnEtat.Click
        TextBoxEtat.Text = ""
        TextBoxEtat.Text = TextBoxEtat.Text + vbCrLf + "Sent " + SBuffer + vbCrLf + "Received " + RBuffer + vbCrLf
        TextBoxEtat.Text = TextBoxEtat.Text + vbCrLf + "SentPacket " + ClientState.SentPacket.ToString
        TextBoxEtat.Text = TextBoxEtat.Text + vbCrLf + "ConfirmedSentPacket " + ClientState.ConfirmedSentPacket.ToString
    End Sub
    Private Sub UsersListBox_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UsersListBox.SelectedIndexChanged
        ClientState.ChattingWith = UsersListBox.Items(UsersListBox.SelectedIndex)
        Button1.Enabled = True
    End Sub

    Private Sub ClientForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        If Not DebugOn Then
            TextBoxEtat.Visible = False
            BtnEtat.Visible = False
            ListBox1.Visible = False
            Me.Height = 340
        End If
        Connect()
        Auth()
    End Sub

    Private Sub Button1_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        SendMsg()
    End Sub
    Private Sub SendMsg()
        Dim SLoop As Int32
        For SLoop = 0 To TextBoxMsg.Text.Trim.Length - 1
            If TextBoxMsg.Text.Trim.Substring(SLoop, 1) = Separator Then
                MsgBox("N'utilisez pas le charactere '" + Separator + "' dans votre message !", MsgBoxStyle.Exclamation)
                Exit Sub
            End If
        Next
        If ClientState.ChattingWith.Trim <> "" And TextBoxMsg.Text.Trim <> "" Then
            If ClientState.Connected And ClientState.Authentified Then

                TextBoxReceived.SelectionFont = New Font(TextBoxReceived.SelectionFont, FontStyle.Regular)
                TextBoxReceived.SelectedText = TextBoxMsg.Text.Trim + vbCrLf
                SendData("107" + Separator + ClientState.ChattingWith + Separator + TextBoxMsg.Text)
            End If
        End If
    End Sub

    Private Sub ClientForm_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode.Enter Then
            SendMsg()
        End If
    End Sub
End Class
Public Class SocketBytes
    Public Socket As Socket
    Public Bytes(BytesSize) As Byte
    Public Data As String
End Class