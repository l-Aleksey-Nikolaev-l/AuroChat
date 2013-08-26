Imports System.Net

Public Class Form1

    Friend Const LocalPort As Integer = 22259 'через этот порт происходит обмен сообщениями между членами чата
    Friend FWS As FormWindowState
    Dim Client_UDP_output As New System.Net.Sockets.UdpClient
    Dim Client_UDP_input As New System.Net.Sockets.UdpClient(LocalPort)
    Dim RemoteIpEndPoint As New IPEndPoint(IPAddress.Any, 0)
    Dim ListenerThread As Threading.Thread
    'Dim notif As New clsNotify
    Dim CurrentHistoryFileName As String = Date.Today.ToShortDateString & "_history.rtf"

    Private Sub ОнлайнToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ОнлайнToolStripMenuItem.Click
        ToolStripStatusLabel1.ForeColor = Color.LimeGreen
    End Sub

    Private Sub НетНаМестеToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles НетНаМестеToolStripMenuItem.Click
        ToolStripStatusLabel1.ForeColor = Color.Goldenrod
    End Sub

    Private Sub ОффлайнToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ОффлайнToolStripMenuItem.Click
        ToolStripStatusLabel1.ForeColor = Color.Red
    End Sub

    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        'Me.SaveCurrentHistory()
        NotifyIcon1.Dispose()
    End Sub

    Private Sub NotifyIcon1_DoubleClick(sender As Object, e As EventArgs) Handles NotifyIcon1.DoubleClick
        Me.WindowState = FormWindowState.Normal
        Me.Show()
    End Sub

    Private Sub НастройкиToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles НастройкиToolStripMenuItem.Click

        Dim abfrm As New SETTINGS
        abfrm.ShowDialog(Me)

    End Sub

    Private Sub ОткрытьToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ОткрытьToolStripMenuItem.Click
        Me.WindowState = FormWindowState.Normal
        Me.ShowInTaskbar = True
    End Sub

    Private Sub ВыходToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ВыходToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub Form1_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load

        Dim i As Integer
        For i = 1 To 200
            Me.CheckedListBox1.Items.Add(i)
        Next
        Me.CheckedListBox1.SelectedIndex = CInt(Me.RichTextBox2.SelectionFont.Size)

        Dim Families As FontFamily() = FontFamily.Families
        Dim Family As Drawing.FontFamily
        For Each Family In Families
            Me.ToolStripComboBox1.Items.Add(Family.Name)
        Next
        Me.ToolStripComboBox1.SelectedItem = Me.RichTextBox2.SelectionFont.Name

        Me.ToolStripLabel1.Image = GetFontImg(Me.RichTextBox2.SelectionColor)

        Dim nbmp As New Bitmap(24, 24, Imaging.PixelFormat.Format32bppArgb)
        Dim g As Graphics = Graphics.FromImage(nbmp)
        g.DrawImage(SystemIcons.Information.ToBitmap, 2, 2, 20, 20)
        g.Dispose()
        'Me.ToolTip1.ToolTipTitle = My.Application.Info.Title
        Me.Text = My.Application.Info.Title & " [" & String.Format("v. {0}", My.Application.Info.Version.ToString) & "]"
        Me.start_thread()
        Me.Me_inc_out(True)



        If SETTINGS.ComboBox1.Text = "Стандартная" Then
            Thems.Standart()
        End If

        If SETTINGS.ComboBox1.Text = "Голубая" Then
            Thems.Blue()
        End If

        If SETTINGS.ComboBox1.Text = "Красная" Then
            Thems.Red()
        End If

    End Sub


    Private Sub ToolStripLabel1_Click(sender As Object, e As EventArgs) Handles ToolStripLabel1.Click
        Dim clrdialog As New ColorDialog
        With clrdialog
            Try
                .Color = Me.RichTextBox2.SelectionColor
            Catch ex As Exception
            End Try
            .SolidColorOnly = False
        End With
        If clrdialog.ShowDialog = Windows.Forms.DialogResult.OK Then
            Me.RichTextBox2.SelectionColor = clrdialog.Color
        End If
    End Sub

    Private Sub setfont(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton1.Click, ToolStripButton2.Click, ToolStripButton3.Click, ToolStripButton4.Click, ToolStripComboBox1.SelectedIndexChanged, ToolStripComboBox2.SelectedIndexChanged
        Me.Set_Current_Font()
    End Sub

    Private Sub Form1_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        Me.Me_inc_out(False)
        Me.Client_UDP_input.Close()
    End Sub

    Private Sub start_thread()
        Try
            ListenerThread = New Threading.Thread(AddressOf DoListen)
            ListenerThread.Start()
        Catch ex As Exception
        End Try
    End Sub

    Private Sub DoListen()
        Do While Not ListenerThread Is Nothing
            Try
                Dim receiveBytes As Byte() = Client_UDP_input.Receive(RemoteIpEndPoint)
                Dim returnData As String = System.Text.Encoding.Default.GetString(receiveBytes)
                Me.ReturnData_toMainThread(returnData)
            Catch ex As Exception
                ListenerThread.Abort()
            End Try
        Loop
    End Sub

    Private Delegate Sub MSG_Delegate(ByVal Text As String)

    Private Sub ReturnData_toMainThread(ByVal Text As String)
        If Not Me.InvokeRequired Then
            Me.UDP_DataArrival(Text)
        Else
            Dim d As System.Delegate = New MSG_Delegate(AddressOf ReturnData_toMainThread)
            Me.BeginInvoke(d, New String() {Text})
        End If
    End Sub

    Friend Sub SendData_to_LocalNet(ByVal strData As String, ByVal RemoteAddress As IPAddress)
        Try
            Dim sendBytes As [Byte]() = System.Text.Encoding.Default.GetBytes(strData)
            Client_UDP_output.Send(sendBytes, sendBytes.Length, New IPEndPoint(RemoteAddress, LocalPort))
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub UDP_DataArrival(ByVal DataSTR As String)
        If DataSTR.StartsWith("INC_CMP") = True Then 'извещение о подключении члена чата
            Me.Inc_out_cmp(DataSTR, True)
            Exit Sub
        End If
        If DataSTR.StartsWith("OUT_CMP") = True Then 'извещение об отключении члена чата
            Me.Inc_out_cmp(DataSTR, False)
            Exit Sub
        End If
        'If DataSTR.Contains("DSTR_ME") = True Then 'закрываем приложение
        '    Me.Close()
        '    Exit Sub
        'End If
        Me.TextBox2.SelectionLength = 0
        Me.TextBox2.SelectionStart = Me.TextBox2.Text.Length
        Me.TextBox2.ScrollToCaret()
        ' если свернуто основное окно

    End Sub


    Private Sub Inc_out_cmp(ByVal data As String, ByVal add As Boolean)
        Dim sep = ":"
        Dim hname As String = data.Split(sep).GetValue(1)
        Dim hip As String = data.Split(sep).GetValue(2)
        If hname.ToUpper.Equals(My.Computer.Name.ToUpper) Then Exit Sub
        Select Case add
            Case Is = True
                Me.CheckedListBox1.Items.Add(hname & ":" & hip, False)
                Me.CheckedListBox1.Refresh()
            Case Is = False
                Me.CheckedListBox1.Items.Remove(hname & ":" & hip)
                Me.CheckedListBox1.Refresh()
        End Select
    End Sub

    Private Sub Me_inc_out(ByVal inc As Boolean)
        Dim cmd As String = ""
        Dim endip = IPAddress.Broadcast
        Dim MyIP As IPAddress = System.Net.Dns.GetHostAddresses(My.Computer.Name.ToString).GetValue(1)
        Select Case inc
            Case Is = True
                cmd = "INC_CMP"
            Case Is = False
                cmd = "OUT_CMP"
        End Select
        Me.SendData_to_LocalNet(cmd & ":" & My.Computer.Name & ":" & MyIP.ToString, endip)
    End Sub

    Private Function RTF_to_send(ByVal infostr As String, ByVal Data_rtf As String) As String
        Dim rtb As New RichTextBox
        rtb.Text = infostr
        rtb.SelectionLength = 0
        rtb.SelectionStart = rtb.TextLength
        rtb.SelectedRtf = Data_rtf
        Return rtb.Rtf
    End Function


    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim EndIP As IPAddress
        Dim EndHostName As String
        Dim MyIP As IPAddress = System.Net.Dns.GetHostAddresses(My.Computer.Name.ToString).GetValue(1)
        Dim selitm As String
        If Me.RichTextBox2.TextLength = 0 Then Exit Sub
        For Each selitm In Me.CheckedListBox1.CheckedItems
            Dim sep = ":"
            EndIP = IPAddress.Parse(selitm.Split(sep).GetValue(1))
            EndHostName = selitm.Split(sep).GetValue(0)
            Dim MTS As String = RTF_to_send("[" & Date.Now.ToShortTimeString & "] " & My.Computer.Name.ToLower & ">>" & EndHostName.ToLower & " : ", Me.RichTextBox2.Rtf)
            Me.SendData_to_LocalNet(MTS, EndIP)
            If (MyIP.ToString.ToUpper <> EndIP.ToString.ToUpper) Then
                Me.SendData_to_LocalNet(MTS, MyIP)
            End If
        Next
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        'Me.ChlbComps.Items.Clear()
        Me.Me_inc_out(True)
    End Sub


    Private Sub RichTextBox2_SelectionChanged(sender As Object, e As EventArgs) Handles RichTextBox2.SelectionChanged
        Try
            If Me.RichTextBox2.SelectionFont.Bold = True Then
                Me.ToolStripButton1.Checked = True
            Else
                Me.ToolStripButton1.Checked = False
            End If
            If Me.RichTextBox2.SelectionFont.Italic = True Then
                Me.ToolStripButton2.Checked = True
            Else
                Me.ToolStripButton2.Checked = False
            End If
            If Me.RichTextBox2.SelectionFont.Underline = True Then
                Me.ToolStripButton3.Checked = True
            Else
                Me.ToolStripButton3.Checked = False
            End If
            If Me.RichTextBox2.SelectionFont.Strikeout = True Then
                Me.ToolStripButton4.Checked = True
            Else
                Me.ToolStripButton4.Checked = False
            End If

            Me.ToolStripComboBox1.SelectedItem = Me.RichTextBox2.SelectionFont.Name
            Me.ToolStripComboBox2.SelectedItem = CInt(Me.RichTextBox2.SelectionFont.Size)
        Catch ex As Exception
        End Try
    End Sub

    Private Sub Set_Current_Font()
        On Error Resume Next
        Dim fs As FontStyle = FontStyle.Regular
        If Me.ToolStripButton1.Checked Then
            fs = fs Or FontStyle.Bold
        End If
        If Me.ToolStripButton2.Checked Then
            fs = fs Or FontStyle.Italic
        End If
        If Me.ToolStripButton3.Checked Then
            fs = fs Or FontStyle.Underline
        End If
        If Me.ToolStripButton4.Checked Then
            fs = fs Or FontStyle.Strikeout
        End If
        Dim font As New Font(Me.ToolStripComboBox1.SelectedItem.ToString, CInt(Me.ToolStripComboBox2.SelectedItem), fs)
        Me.RichTextBox2.SelectionFont = font
    End Sub


    Private Function GetFontImg(ByVal Fcolor As Color) As Bitmap
        Dim img As New Bitmap(16, 16, Imaging.PixelFormat.Format32bppArgb)
        Dim g As Graphics = Graphics.FromImage(img)
        Dim brsh As Brush = New SolidBrush(Fcolor)
        Dim w, h As Integer
        Dim fnt As New Font("Microsoft Sans Serif", 16, FontStyle.Regular Or FontStyle.Bold, GraphicsUnit.Pixel)
        g.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias
        w = g.MeasureString("T", fnt).Width
        h = g.MeasureString("T", fnt).Height
        g.DrawString("T", fnt, brsh, (16 - w) / 2, (16 - h) / 2)
        If Fcolor = Nothing Then
            g.Clear(Color.White)
        End If
        g.Dispose()
        Return img
    End Function


End Class
