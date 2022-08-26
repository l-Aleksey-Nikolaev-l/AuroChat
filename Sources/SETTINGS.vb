Public Class SETTINGS

    Private Sub SETTINGS_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Form1.Show()
        Me.Hide()
    End Sub

#Region "== Основные =="

#Region "Сворачивать программу"

    Private Sub CheckBox3_Click(sender As Object, e As EventArgs) Handles CheckBox3.Click
        CheckBox4.CheckState = CheckState.Unchecked
        Form1.ShowInTaskbar = False
        Form1.NotifyIcon1.Visible = True
    End Sub

    Private Sub CheckBox4_Click(sender As Object, e As EventArgs) Handles CheckBox4.Click
        CheckBox3.CheckState = CheckState.Unchecked
        Form1.ShowInTaskbar = True
        Form1.NotifyIcon1.Visible = False
    End Sub

    '#######################################################################################################

    Private Sub CheckBox3_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox3.CheckedChanged
        Form1.ShowInTaskbar = False
        Form1.NotifyIcon1.Visible = True
    End Sub

    Private Sub CheckBox4_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox4.CheckedChanged
        Form1.ShowInTaskbar = True
        Form1.NotifyIcon1.Visible = False
    End Sub

#End Region

#Region "Сворачивать окно"

    Private Sub CheckBox1_Click(sender As Object, e As EventArgs) Handles CheckBox1.Click
        CheckBox2.CheckState = CheckState.Unchecked
    End Sub

    Private Sub CheckBox2_Click(sender As Object, e As EventArgs) Handles CheckBox2.Click
        CheckBox1.CheckState = CheckState.Unchecked
    End Sub

#End Region

#End Region

#Region "== Соединение =="

#End Region

#Region "== Темы =="

    Private Sub Button3_Click(sender As System.Object, e As System.EventArgs) Handles Button3.Click
        If ComboBox1.Text = "Стандартная" Then
            Thems.Standart()
        End If

        If ComboBox1.Text = "Голубая" Then
            Thems.Blue()
        End If

        If ComboBox1.Text = "Красная" Then
            Thems.Red()
        End If
    End Sub

#End Region

#Region "== Дополнительно =="

#End Region

#Region "== Об... =="

#End Region

#Region "== Обновлениея =="

#End Region

End Class