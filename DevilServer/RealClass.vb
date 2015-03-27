Imports System.Net
Imports System.Net.Sockets
Public Class RealServer
    Public Tx As Tex
    Public UserList As ArrayList
    Public ConnectedUserList As ArrayList
    Public Client As Socket
    Public RBuffer As String
    Public ID As Int32
    Public SBuffer As String
    Public WhenAClientConnect As ServerForm.WhenAClientConnectDelegate
    Public WhenAClientDisconnect As ServerForm.WhenAClientDisconnectDelegate
    Public TransferMsg As ServerForm.TransferMsgDelegate
    Public RSUserName As String
    Public Delegate Sub Tex(ByVal S As String)
    Dim ClientState As New ClientState
    Dim BufferOn As Boolean
    Dim DebugOn As Boolean
    Public Sub Listen()
        Dim State As New StateSocket
        State.Socket = Client
        State.Socket.Blocking = False
        Try
            State.Socket.BeginReceive(State.Bytes, 0, State.Bytes.Length, SocketFlags.None, New AsyncCallback(AddressOf ListenCallBack), State)
        Catch ex As SocketException

        End Try

    End Sub
    Public Sub SendData(ByVal S As String)
        If DebugOn Then Tx(Me.ID + " " + Me.RSUserName + " " + S)

        Dim Ss As New StateSocket
        Ss.Socket = Client
        If Not S.EndsWith(Separator) Then
            S = S + Separator
        End If
        Ss.Bytes = System.Text.Encoding.ASCII.GetBytes(S)
        ReDim Preserve Ss.Bytes(S.Length)
        Ss.Bytes(S.Length) = 200
        If Not S.StartsWith("104") Then
            ClientState.SentPacket = ClientState.SentPacket + 1
        End If
        If BufferOn Then SBuffer = SBuffer + "(" + S.Trim(Chr(0)) + ")"
        Try
            Ss.Socket.BeginSend(Ss.Bytes, 0, Ss.Bytes.Length, SocketFlags.None, New AsyncCallback(AddressOf SendCallBack), Ss)
        Catch ex As SocketException

        End Try

    End Sub
    Public Sub AcceptCallBack(ByVal ar As IAsyncResult)
        Dim State As New StateSocket
        Dim Socket As Socket
        Socket = CType(ar.AsyncState, Socket)
        Client = Socket.EndAccept(ar)
        Client.Blocking = False
        State.Socket = Client
        State.Socket.BeginReceive(State.Bytes, 0, State.Bytes.Length, SocketFlags.None, New AsyncCallback(AddressOf ListenCallBack), State)
    End Sub
    Public Sub ListenCallBack(ByVal ar As IAsyncResult)
        Dim rState As StateSocket
        Dim nB As Int32


        rState = CType(ar.AsyncState, StateSocket)
        Try
            nB = rState.Socket.EndReceive(ar)
            If nB > 0 Then
                rState.Data = System.Text.Encoding.ASCII.GetString(rState.Bytes)
                If BufferOn Then RBuffer = RBuffer + "(" + rState.Data.Trim(Chr(0)) + ")"
                ProcessMsg(rState.Bytes, rState)
            End If
        Catch ex As SocketException
            Select Case ex.ErrorCode
                Case 995
                Case 10054
                    WhenAClientDisconnect(ClientState, Me)
                Case Else
                    MsgBox(ex.ToString, MsgBoxStyle.Information, ex.ErrorCode)
            End Select
        End Try

        rState.Bytes.Clear(rState.Bytes, 0, rState.Bytes.Length)
        Try
            rState.Socket.BeginReceive(rState.Bytes, 0, rState.Bytes.Length, SocketFlags.None, New AsyncCallback(AddressOf ListenCallBack), rState)
        Catch ex As SocketException

        End Try

    End Sub
    Private Sub ProcessMsg(ByVal R() As String, ByVal rState As StateSocket)

        Dim U As User
        Dim AlreadyConnected As Boolean
        Select Case Val(R(0))
            Case 199
                If ClientState.Authentified = True Then
                    ClientState.Authentified = False
                End If
                WhenAClientDisconnect(ClientState, Me)
                SendOk()
            Case 102
                SendOk()
                ClientState.UserName = R(1)
                ClientState.Password = R(2)
                For Each U In ConnectedUserList
                    If U.UserName = ClientState.UserName Then
                        SendData("112")
                        AlreadyConnected = True
                    End If
                Next
                If Not AlreadyConnected Then
                    For Each U In UserList
                        If U.UserName.ToUpper = ClientState.UserName.ToUpper And U.Password = ClientState.Password Then
                            ClientState.RealName = U.RealName
                            ClientState.Authentified = True
                            RSUserName = ClientState.UserName
                            SendData("103")
                            WhenAClientConnect(ClientState)
                        End If
                    Next
                End If

            Case 107
                TransferMsg(R(2), R(1), Me)
                SendOk()
            Case 111
                ClientState.ConfirmedSentPacket = ClientState.ConfirmedSentPacket + 1
        End Select
    End Sub
    Private Sub ProcessMsg(ByVal B() As Byte, ByVal rState As StateSocket)
        Dim MsgString As String
        Dim BLoop As Int32
        Dim R() As String
        For BLoop = 0 To B.Length - 1
            If B(BLoop) <> 200 Then
                MsgString = MsgString + Chr(B(BLoop))
            Else
                R = MsgString.Split(Separator)
                ProcessMsg(R, rState)
                MsgString = ""
            End If
        Next

        'Select Case Val(R(0))
        '    Case 199
        'SendOk()
        'ClientState.Authentified = False
        'rState.Socket.Close()
        '    Case 102
        'SendOk()
        'Dim U As User
        'ClientState.UserName = R(1)
        'ClientState.Password = R(2)
        'For Each U In UserList
        'If U.UserName = ClientState.UserName And U.Password = ClientState.Password Then
        'ClientState.Authentified = True
        'ConnectedUserList.Add(U)
        'SendData("103|")
        'SendUserList()
        'End If
        'Next
        '    Case 107
        'Tx("De " + ClientState.UserName + " " + R(1))
        'SendOk()
        '    Case 111
        'ClientState.ConfirmedSentPacket = ClientState.ConfirmedSentPacket + 1
        'End Select
    End Sub
    Public Sub SendUserList()
        Dim U As User
        SendData("108")
        For Each U In ConnectedUserList
            If U.UserName <> ClientState.UserName Then
                SendData("109" + Separator + U.UserName)
            End If
        Next
        SendData("110")
    End Sub
    Private Sub SendOk()
        SendData("104")
    End Sub
    Private Sub SendCallBack(ByVal ar As IAsyncResult)
        Dim SB As StateSocket
        SB = CType(ar.AsyncState, StateSocket)
        Try
            SB.Socket.EndSend(ar)
        Catch ex As SocketException

        End Try

    End Sub
    Public Class StateSocket
        Public Socket As Socket
        Public Bytes(BytesSize) As Byte
        Public Data As String
    End Class
End Class