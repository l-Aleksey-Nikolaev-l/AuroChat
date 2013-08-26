Module For_Notify

    Public Enum RichSnd
        Asterisk
        Beep
        Exclamation
        Hand
        Alarm
        Bfire
        Bipbip
        Blink
        Ding
        Laser
        Msg
        Tyty
        Win
        NoSound
    End Enum

    Public Enum RichPos
        BR_U
        BR_L
        BL_U
        BL_R
        TR_D
        TR_L
        TL_D
        TL_R
    End Enum

    Public Sub NotifyBox(ByVal Richicon As Drawing.Icon, ByVal RichSound As RichSnd, Optional ByVal RichName As String = "Сообщение", Optional ByVal RichText As String = "", Optional ByVal RichZoom As Single = 1, Optional ByVal RichPause As Integer = 10, Optional ByVal RichPosition As RichPos = RichPos.BR_U, Optional ByVal WinStep As Integer = 5)
        'RichIcon - значок в заголовке формы
        'RichSound - звук, издаваемый при окончании выезда формы
        'RichText - текст на форме в верхней секции
        'RichZoom - масштабный коэффициент для отображения текста
        'RichName - текст в шапке формы
        'RichPause - время задержки фыдвинутой формы на экране (если 0, то форма не исчезает автоматически)
        'RichPosition - начальная позиция и направление выезда: 
        'BR_U - нижний правый угол и выезд вверх
        'BR_L - нижний правый угол и выезд влево
        'BL_U - нижний левый угол и выезд вверх
        'BL_R - нижний левый угол и выезд вправо
        'TR_D - верхний правый угол и выезд вниз
        'TR_L - верхний правый угол и выезд влево
        'TL_D - верхний левый угол и выезд вниз
        'TL_R - верхний правый угол и выезд вправо
        'WinStep - шаг выезда формы (если 0 - появляется вся сразу в заданном углу)






        'Dim newNB As New frmNotif
        'newNB.WinSound = RichSound
        'newNB.Icon = Richicon
        'newNB.Text = RichName
        'With newNB.rtbMSG
        '    .ZoomFactor = RichZoom
        '    .ScrollBars = RichTextBoxScrollBars.None
        '    newNB.rtbMSG.SelectionStart = 0
        '    newNB.rtbMSG.SelectionLength = 0
        '    newNB.rtbMSG.SelectedRtf = RichText
        '    .SelectionStart = 0
        '    .SelectionLength = .TextLength
        '    .SelectionAlignment = HorizontalAlignment.Center
        '    .SelectionLength = 0
        '    .SelectionStart = .TextLength

        'End With
        'newNB.closeint = RichPause
        'newNB.startPos = RichPosition
        'newNB.WinStep = WinStep
        'newNB.Show()
    End Sub

End Module
