Public Class FormAcceso


    Private Sub FormAcceso_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Text = " Inicio de Sesión [ PRODODECIP ] "

        'Conectar()


        Try
            
            cn.ConnectionString = My.Settings.ConnectionString
            cn.Open()

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try


        'If cn.State = 0 Then
        '    btnIngresar.Enabled = False
        'End If
        cboNivel.SelectedIndex = 0
    End Sub
    Function VerificarClave(ByVal xx)
        Dim I As Integer
        Dim Lon As Integer
        Dim ClaveEncriptando As Byte
        Dim ClaveEncriptada As String = ""
        Lon = Len(xx)
        For I = 1 To Lon
            ClaveEncriptando = Asc(Mid(xx, I, 1))
            ClaveEncriptada += Str(ClaveEncriptando)
        Next I
        Return Mid(ClaveEncriptada, 2)

    End Function

    Private Sub txtUsuario_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtUserName.KeyPress
        If e.KeyChar = Microsoft.VisualBasic.ChrW(Keys.Return) Then
            SendKeys.Send("{TAB}")
            e.Handled = True
        End If
    End Sub

    Private Sub txtClave_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtPassword.KeyPress
        If e.KeyChar = Microsoft.VisualBasic.ChrW(Keys.Return) Then
            SendKeys.Send("{TAB}")
            e.Handled = True
        End If
    End Sub

    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Me.Close()
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Static sw As Integer
        If sw = 0 Then

            Imgllave.Top = 70 '600
            Imgllave.Left = 30 '600
        ElseIf sw = 1 Then
            Imgllave.Top = 70 '600
            Imgllave.Left = 110

        ElseIf sw = 2 Then
            Imgllave.Top = 110 '800
            Imgllave.Left = 110 '960
        Else

            Imgllave.Top = 110 '800
            Imgllave.Left = 30 ' 600

        End If
        sw = sw + 1
        If sw = 4 Then
            sw = 0
        End If
    End Sub

    Private Sub TimerTitulo_Tick(sender As Object, e As EventArgs) Handles TimerTitulo.Tick
        Static sf As Integer
        If sf = 0 Then
            lblNE.Left = 161
            lblNE.Text = "SIGAA"
        Else
            lblNE.Left = 122
            lblNE.Text = "PRODODECIP"
        End If
        sf = sf + 1
        If sf = 2 Then
            sf = 0
        End If
    End Sub

    '1.	SISTEMA INTEGRADO DE GESTION ACADEMICA Y ADMINISTRATIVA
    'SISTEMA INTEGRADO DE GESTION ACADEMICA Y ADMINISTRATIVA PARA EL DOCTORADO Y LA MAESTRÍA EN DERECHO Y CIENCIAS POLITICAS (SIGAA)

    Private Sub btnIngresar_Click(sender As Object, e As EventArgs) Handles btnIngresar.Click
        Static I As Integer
        'Dim bool As Boolean
        Dim unloadd = 0

        If Trim(txtUserName.Text) = "" Then
            MsgBox("Ingrese un nombre de usuario...", vbQuestion, "Inicio de sesión - Clave de Seguridad")
            txtUserName.Focus()
            Exit Sub
        End If

        If Trim(txtPassword.Text) = "" Then
            MsgBox("Ingrese su Contraseña...", vbQuestion, "Inicio de sesión - Clave de Seguridad")
            txtPassword.Focus()
            Exit Sub
        End If

        Dim res As Integer
        Dim clave As String


        'res = Existen_UsuariosBD()
        'If res < 1 Then 'no existen usuarios
        '    If vbYes = MsgBox("No existen Usuarios, en el sistema..." & vbCrLf & "Desea Grabar los datos del Usuario?", vbInformation + vbYesNo, Me.Text) Then
        '        On Error Resume Next
        '        clave = GeneraClaveEncriptada(Trim(txtPassword.Text))
        '        'res = GrabaUsuarios(Trim(clave), Trim(txtUserName.Text), Trim(cboNivel.Text)) + 1
        '        GoTo entrar_
        '        'Else : cmdCancel_Click()
        '        '  Exit Sub
        '    End If

        'End If
        'MsgBox(DesencriptarClave("101 112 103 97 100 109 105 110 49 53"))
        'MsgBox(DesencriptarClave("87 66 85 83 86 95 91 92 3 7"))
        'Exit Sub

        clave = GeneraClaveEncriptada(Trim(txtPassword.Text))
        res = LeeUser((clave), Trim(txtUserName.Text), Trim(cboNivel.Text))
entrar_:
        If I < 3 And (res = 1) Then
            MsgBox("SISTEMA INTEGRADO DE  GESTION ACADEMICA..." & vbCrLf & vbCrLf & _
            " Usuario: " & Trim(txtUserName.Text), vbInformation, "** Sistema de Seguridad")
            USER = Trim(txtUserName.Text)
            NIVEL_USER = cboNivel.SelectedIndex
            If intfrmLogin = 0 Then
                'MsgBox("OK")
                FormMDI.Show() 'MDIPRINCIPAL.Show()) 'se ingresa al sistema
            Else
                'MDIPRINCIPAL.Configura_Menu NIVEL_USER ' se ha cambiado de usuario
            End If

            unloadd = 1 'salir
            Me.Close()
        ElseIf I < 3 And (res <> 1) Then
            MsgBox("Usuario no Valido..", vbInformation, Me.Text)
            Beep()
            txtUserName.Focus()
        End If

        If unloadd = 0 Then 'salir
            If I > 1 Then
                MsgBox("Ha realizado Tres intentos...", vbExclamation, "** Sistema de Seguridad")
                Me.Close()
            Else : I = I + 1
                Beep()
                txtUserName.Focus()
            End If
        End If
    End Sub
End Class
