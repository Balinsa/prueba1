Imports MSFlexGridLib
Imports System.Math

Module ModAplica
    '------------------------------------------------------------------------------------------------------------------------
    '** fecha : 25/04/03
    '** este modulo tiene contiene las declaraciones de fucniones que son utilizadas
    '** por la aplicacion referentes al diseño, validacion y formatos
    '** Cada funcion recibe los parametros respectivos
    '------------------------------------------------------------------------------------------------------------------------

    '** esta funcion valida que las entradas del usuario sean solo letras de la A-Z
    '** en el parametro tecla se recibe la tecla presionada, devuelve la tecla valida en mayusculas
    Function SoloLetrasAZ(tecla As Integer) As Integer
        Select Case tecla
            Case 65 To 90, 32, 8, 209, 40, 41, 180, 43 'AZ - ' ' - <- Ñ(209) (-)-shift
                SoloLetrasAZ = tecla
            Case 97 To 122, 241 'az ñ 241
                SoloLetrasAZ = (tecla - 32)
        End Select
    End Function

    '** esta funcion valida que las entradas del usuario sean solo numeros
    '** en el parametro tecla se recibe la tecla presionada, devuelve la tecla valida, caso contrario
    '** devuleve cero
    Function SoloNumeros(tecla As Integer) As Integer
        Select Case tecla
            Case 48 To 57, 8, 13
                SoloNumeros = tecla
            Case Else
                SoloNumeros = 0
        End Select
    End Function

    '** esta funcion valida que las entradas del usuario sean numeros y letras A-Z
    '** en el parametro tecla se recibe la tecla presionada, si la tecla valida es caracter
    '** devuleve la tecla en mayuscula, caso contrario devuelve el numero
    Public Function NumerosLetras(tecla As Integer) As Integer
        Select Case tecla
            Case 65 To 90, 32, 8, 209, 40, 41, 180 'AZ - ' ' - <- Ñ(209) (-)-shift
                NumerosLetras = tecla
            Case 97 To 122, 241 'az ñ 241
                NumerosLetras = (tecla - 32)
            Case 48 To 57, 8, 13
                NumerosLetras = tecla
        End Select
    End Function

    '** esta funcion valida que las entradas del usuario sean numeros y letras A-Z
    '** y el caracter -, en el parametro tecla se recibe la tecla presionada, si la tecla valida es caracter
    '** devuleve la tecla en mayuscula
    Public Function NumerosLetrasRaya(tecla As Integer) As Integer
        Select Case tecla
            Case 65 To 90, 32, 8, 209, 40, 41, 180 'AZ - ' ' - <- Ñ(209) (-)-shift
                NumerosLetrasRaya = tecla
            Case 97 To 122, 241 'az ñ 241
                NumerosLetrasRaya = (tecla - 32)
            Case 48 To 57, 8, 13, 45 '45: -
                NumerosLetrasRaya = tecla
        End Select
    End Function

    '** esta funcion emite el mensaje DESEA SALIR DE LA APLICACION, devuelve el valor del usuario
    '** en un valor booleano
    Public Sub Cerrar(Cancel As Boolean, MENSAJE As String, Tit As String, frm As Form)
        Dim respuesta, Titulo
        Titulo = vbYesNo + vbDefaultButton2 + vbInformation
        respuesta = MsgBox(MENSAJE, Titulo, Tit)
        If respuesta = vbYes Then
            'Unload frm 'End
            frm.Close()
        Else : Cancel = True
        End If
    End Sub

    '** esta funcion da formato a las cuadricula, recibe el MSHFlexGrid, y formatea las cabeceras
    '** en negrita y en letra arial
    Sub ColocaNegritaCab(msh As MSFlexGrid)
        With msh
            .Row = 0
            For I = 0 To .Cols - 1
                .col = I
                .CellAlignment = 4
                .CellFontBold = True
                .CellFontName = "Arial"
            Next
        End With
    End Sub

    '** esta funcion da formato a las cuadricula, recibe el MSHFlexGrid, y formatea una fila especifica
    '** que es recibida como parametro, en negrita y en letra arial
    'Sub ColocaNegritaCabFila(msh As MSHFlexGrid, fil As Integer)
    '    With msh
    '        .Row = fil
    '        For I = 0 To .Cols - 1
    '            .col = I
    '            .CellAlignment = 4
    '            .CellFontBold = True
    '            .CellFontName = "Arial"
    '        Next
    '    End With
    'End Sub

    '** esta funcion da formato a las cuadricula, recibe el MSHFlexGrid, y enumera la primera columna
    '** de la cuadricula
    Public Sub Coloca_Numeracion(msh As MSFlexGrid)
        Dim intfil As Integer
        intfil = 1
        With msh
            .TextMatrix(0, 0) = "N°"
            For I = 1 To .Rows - 1
                If .RowHeight(I) <> 0 Then
                    .TextMatrix(I, 0) = intfil
                    intfil = intfil + 1
                End If
            Next
        End With
    End Sub

    '** esta funcion separa y devuelve los requisitos en el arreglo (arreglo), proveniente de la
    '** cadena Requisitos.
    '** la funcion devuelve
    Public Function Traer_Requisitos(Requisitos As String) As Integer
        On Error GoTo errores_
        Traer_Requisitos = 0 'Num Req
        intNArreglo = 0
        straux = Requisitos
        Dim cadena As String
        ReDim arreglo(0 To 4) 'se asume 4 req
        If Len(straux) = 6 Then
            arreglo(intNArreglo) = straux
            Traer_Requisitos = 1
            intNArreglo = 1
            Exit Function
        End If

        While Len(straux)
            cadena = Mid(straux, 1, InStr(1, straux, "-") - 1)
            straux = Mid(straux, Len(cadena), Len(straux) - Len(cadena))
            arreglo(intNArreglo) = cadena
            intNArreglo = intNArreglo + 1
        End While
        Traer_Requisitos = intNArreglo
errores_:
    End Function

    '** esta funcion valida las fechas ingresadas en la aplicacion que las llama
    Public Function ValidaFechas(FECHA As String) As Boolean
        On Error GoTo errores_
        ValidaFechas = False 'no valida
        If Not IsDate(FECHA) Then Exit Function
        ValidaFechas = True
errores_:
    End Function

    '** esta funcion valida las notas ingresadas en una cuadricula mshflexgrid
    '** en la columna col contiene la columna donde se encuentra la nota
    '    Public Function Valida_Notas_msh(msh As MSHFlexGrid, col As Integer) As Boolean
    '        Dim cdblNota As Double
    '        Valida_Notas_msh = True
    '        On Error GoTo sale_

    '        With msh
    '            For I = 1 To .Rows - 1
    '                If Trim(.TextMatrix(I, col)) = "" Or Not IsNumeric(Trim(.TextMatrix(I, col))) Then
    '                    .TextMatrix(I, col) = "0.00"
    '                End If
    '                straux = Trim(.TextMatrix(I, col))
    '                cdblNota = CDbl(straux)
    '                If cdblNota < 0 Or cdblNota > 20 Then GoTo sale_
    '            Next
    '        End With
    '        Exit Function

    'sale_:
    '        Valida_Notas_msh = False
    '    End Function

    '** Carga la figura de desactivado. en la columna correspondiente
    '    Public Sub MostrarDesactivo(mshAlumnos As MSHFlexGrid, col As Integer)
    '        On Error GoTo noExisteFig
    '        With mshAlumnos
    '            .col = col
    '            For I = 1 To .Rows - 1
    '                .Row = I
    '                .CellPicture = LoadPicture("C:\SistAcad_EscPost\Iconos\chkDesabilitado.bmp")
    '                .TextMatrix(I, .Cols - 1) = 0
    '                .CellAlignment = 4 'flexAlignCenterCenter
    '                .CellPictureAlignment = 4
    '            Next
    '            '   Set MSHFlexGrid1.CellPicture = _
    '            LoadPicture("Icons\Computer\Trash02b.ico")
    '        End With
    'noExisteFig:  'MsgBox err.Description
    '    End Sub

    '** esta funcion valida si el semestre ingresdo en la aplicacion que las llama es valido
    '** es decir q siga el formato '2003-1'
    Public Function Valida_Semestre(SEMESTRE As Object) As Boolean
        Valida_Semestre = False
        If InStr(1, Trim(SEMESTRE), "_") <> 0 Then
            MsgBox("Ingrese un Semestre...", vbInformation, "** Validacion de Semestre")
            SEMESTRE.SetFocus()
            Exit Function
        End If
        If CInt(Right(Trim(SEMESTRE), 1)) > 2 Then
            MsgBox("Ingrese un Semestre Valido ...", vbInformation, "** Validacion de Semestre")
            SEMESTRE.SetFocus()
            Exit Function
        End If
        Valida_Semestre = True
    End Function

    '** Esta funcion genera claves Encriptadas, para los usuarios
    Public Function GeneraClaveEncriptada(clave As String) As String
        'Return Mid(ClaveEncriptada, 2)
        Dim I As Integer
        Dim Lon As Integer
        Dim ClaveEncriptando As Byte
        Dim ClaveEncriptada As String = ""
        Lon = Len(clave)
        For I = 1 To Lon
            ClaveEncriptando = Asc(Mid(clave, I, 1))
            ClaveEncriptada += Str(ClaveEncriptando)
        Next I
        'Return
        GeneraClaveEncriptada = Mid(ClaveEncriptada, 2) '"97 119 113 96 119" '
    End Function

    '** Esta funcion devuelve el valor de las claves Encriptadas, para los usuarios
    Public Function DesencriptarClave(clave As String) As String
        Dim CONT As Integer
        Dim LongClave As Integer ', CodigoEnc As Integer
        Dim ContAux As Integer
        Dim cadAux As String
        Dim carInt As String
        Dim enc As Integer

        'CodigoEnc = 50
        Const Longitud = 8
        Dim ClaveDesenc = ""
        cadAux = ""
        CONT = 0
        LongClave = Len(clave)

        Do While CONT < Longitud
            ContAux = InStr(clave, " ")
            If ContAux <> 0 Then
                cadAux = Left(clave, ContAux - 1)
            Else
                cadAux = Trim(clave)
            End If
            enc = CInt(cadAux) Xor CodigoEnc
            carInt = Chr(enc)
            ClaveDesenc = ClaveDesenc & CStr(carInt)
            clave = Right(clave, LongClave - ContAux)
            CONT = CONT + 1
            LongClave = LongClave - ContAux
        Loop
        DesencriptarClave = ClaveDesenc
    End Function


    '** Devuelve un numero ordinal a su cardinal correspondiente
    Public Function NumOrd_Cardinal(Numero As Integer) As String
        NumOrd_Cardinal = ""
        Select Case Numero
            Case 1
                NumOrd_Cardinal = "Primera"
            Case 2
                NumOrd_Cardinal = "Segunda"
            Case 3
                NumOrd_Cardinal = "Tercera"
            Case 4
                NumOrd_Cardinal = "Cuarta"
            Case 5
                NumOrd_Cardinal = "Quinta"
            Case 6
                NumOrd_Cardinal = "Sexta"
            Case 7
                NumOrd_Cardinal = "Septima"
            Case 8
                NumOrd_Cardinal = "Octava"
            Case 9
                NumOrd_Cardinal = "Novena"
            Case 10
                NumOrd_Cardinal = "Decima"
            Case 11
                NumOrd_Cardinal = "Onceava"
            Case 12
                NumOrd_Cardinal = "Doceava"
            Case 13
                NumOrd_Cardinal = "Treceava"
            Case 14
                NumOrd_Cardinal = "Catorceava"
            Case 15
                NumOrd_Cardinal = "Quinceava"
            Case 16
                NumOrd_Cardinal = "Diesiseisava"
            Case 17
                NumOrd_Cardinal = "Diesisieteava"
            Case 18
                NumOrd_Cardinal = "Diesiochova"
        End Select
    End Function

    '** funcion q Imprime el logo de la unp en el reporte
    Public Sub Imprime_logoUnp()
        'On Error Resume Next
        'Dim ImageLeft As Single, ImageTop As Single
        'ImageLeft = 1 + HorizontalMargin
        'ImageTop = VerticalMargin
        'Printer.ScaleMode = vbCentimeters
        'Printer.PaintPicture(imglogo.Picture, ImageLeft, ImageTop)

    End Sub

    '** funcion q valida la nota de un mshflexgrid de la aplicacion
    '** la nota se considera entre cero y 20
    Public Function Valida_Nota(TxtEdit As Control) As Boolean
        On Error GoTo sale
        If TxtEdit.Text = "" Then GoTo okk
        If CDbl(Trim(TxtEdit.Text)) < 0 Or CDbl(Trim(TxtEdit.Text)) > 20 Then
            GoTo sale
        End If
okk:
        Valida_Nota = True : Exit Function
sale:
        TxtEdit.Visible = True
        TxtEdit.Text = ""
        On Error Resume Next
        MsgBox("Nota debe ser entre 0 y 20/Nota Incorrecta...", vbInformation + vbOKOnly, "** Ingreso de notas  **")
        TxtEdit.Focus()
        Valida_Nota = False
    End Function

    '** Esta funcion devuelve un numero en letras por ej. 10 ->> Diez
    Public Function NumeroLetras(Numero As Integer) As String
        On Error Resume Next
        NumeroLetras = ""
        Dim Numeros As New clsNumeros

        NumeroLetras = Numeros.NroEnLetras(Numero)
    End Function

    '** Esta funcion devuelve el numero de dias en letras por ej. 10 ->> Diez
    Public Function NumerodiaLetras(Numero As Integer) As String
        NumerodiaLetras = ""
        On Error Resume Next
        Dim Numeros As New clsNumeros

        Select Case Numero
            Case 1
                NumerodiaLetras = "Primer"
            Case 2 To 20, 22 To 50
                NumerodiaLetras = Numeros.NroEnLetras(Numero)
            Case 21
                NumerodiaLetras = "Veinte y un"
        End Select

    End Function


    '** Esta funcion devuelve un numero(orden de merito) en valor literal letras
    '** por ej. 1 ->> Diez
    Public Function NumeroOrdenMerito(MERITO As Integer) As String
        NumeroOrdenMerito = ""
        On Error Resume Next
        Dim Numeros As New clsNumeros

        Select Case MERITO
            Case 1
                NumeroOrdenMerito = "PRIMER"
            Case 2
                NumeroOrdenMerito = "SEGUNDO"
            Case 3
                NumeroOrdenMerito = "TERCERO"
            Case 4
                NumeroOrdenMerito = "CUARTO"
            Case 5
                NumeroOrdenMerito = "QUINTO"
            Case 6
                NumeroOrdenMerito = "SEXTO"
            Case 7
                NumeroOrdenMerito = "SEPTIMO"
            Case 8
                NumeroOrdenMerito = "OCTAVO"
            Case 9
                NumeroOrdenMerito = "NOVENO"
            Case 10
                NumeroOrdenMerito = "DECIMO"
            Case 11 To 30, 40, 50, 60
                NumeroOrdenMerito = Numeros.NroEnLetras(MERITO, False) & "AVO" '"ONCEAVO"
            Case 31
                NumeroOrdenMerito = "TREINTAIUNAVO"
            Case 32
                NumeroOrdenMerito = "TREINTAIDOSAVO"
            Case 33
                NumeroOrdenMerito = "TREINTAITRESAVO"
            Case 34
                NumeroOrdenMerito = "TREINTAICUATROAVO"
            Case 35
                NumeroOrdenMerito = "TREINTAICINCOAVO"
            Case 36
                NumeroOrdenMerito = "TREINTAISEISAVO"
            Case 37
                NumeroOrdenMerito = "TREINTAISIETEAVO"
            Case 38
                NumeroOrdenMerito = "TREINTAIOCHOAVO"
            Case 39
                NumeroOrdenMerito = "TREINTAINUEVEAVO"
            Case 41
                NumeroOrdenMerito = "CUARENTAIUNAVO"
            Case 42
                NumeroOrdenMerito = "CUARENTAIDOSAVO"
            Case 43
                NumeroOrdenMerito = "CUARENTAITRESAVO"
            Case 44
                NumeroOrdenMerito = "CUARENTAICUATROAVO"
            Case 45
                NumeroOrdenMerito = "CUARENTAICINCOAVO"
            Case 46
                NumeroOrdenMerito = "CUARENTAISEISAVO"
            Case 47
                NumeroOrdenMerito = "CUARENTAISIETEAVO"
            Case 48
                NumeroOrdenMerito = "CUARENTAIOCHOAVO"
            Case 49
                NumeroOrdenMerito = "CUARENTAINUEVEAVO"
            Case 51
                NumeroOrdenMerito = "CINCUENTAIUNAVO"
            Case 52
                NumeroOrdenMerito = "CINCUENTAIDOSAVO"
            Case 53
                NumeroOrdenMerito = "CINCUENTAITRESAVO"
            Case 54
                NumeroOrdenMerito = "CINCUENTAICUATROAVO"
            Case 55
                NumeroOrdenMerito = "CINCUENTAICINCOAVO"
            Case 56
                NumeroOrdenMerito = "CINCUENTAISEISAVO"
            Case 57
                NumeroOrdenMerito = "CINCUENTAISIETEAVO"
            Case 58
                NumeroOrdenMerito = "CINCUENTAIOCHOAVO"
            Case 59
                NumeroOrdenMerito = "CINCUENTAINUEVEAVO"
            Case 61
                NumeroOrdenMerito = "SESENTAIUNOAVO"
            Case 62
                NumeroOrdenMerito = "SESENTAIDOSAVO"
            Case 63
                NumeroOrdenMerito = "SESENTAITRESAVO"
        End Select
    End Function

    '** funcion q permite imprimir un texto justificado en la impresora, utilidad no
    '** encontrada en el datareport
    Sub justifica_printerdAN(x0, xf, y0, Txt)
        '    Dim x, Y, k, ancho
        '    Dim s As String, ss As String
        '    Dim x_spc

        '    s = Txt
        '    x = x0
        '    Y = y0
        '    ancho = (xf - x0)
        '    'X_printer = x '''posicion de la x, donde se quedo
        '    'Y_printer = Y '''
        '    On Error Resume Next

        '    While s <> ""

        '        ss = ""
        '        While (s <> "") And (Printer.TextWidth(ss) <= ancho)
        '            ss = ss & Left$(s, 1)
        '            s = Right$(s, Len(s) - 1)
        '        End While
        '        If (Printer.TextWidth(ss) > ancho) Then
        '            s = Right$(ss, 1) & s
        '            ss = Left$(ss, Len(ss) - 1)
        '        End If
        '        ' aqui tenemos en ss lo maximo que cabe en una linea
        '        If Right$(ss, 1) = " " Then
        '            ss = Left$(ss, Len(ss) - 1)
        '        Else
        '            If (InStr(ss, " ") > 0) And (Left$(s & " ", 1) <> " ") Then
        '                While Right$(ss, 1) <> " "
        '                    s = Right$(ss, 1) & s
        '                    ss = Left$(ss, Len(ss) - 1)
        '                End While
        '                ss = Left$(ss, Len(ss) - 1)
        '            End If
        '        End If
        '        x_spc = 0
        '        x = x0
        '        'X_printer = x '''
        '        If (Len(ss) > 1) And (s & "" <> "") Then
        '            x_spc = (ancho - Printer.TextWidth(ss)) / (Len(ss) - 1)
        '        End If
        '        Printer.CurrentX = x
        '        Printer.CurrentY = Y

        '        If x_spc = 0 Then
        'Printer.Print ss;
        '        Else
        '            For k = 1 To Len(ss)
        '                Printer.CurrentX = x
        ' Printer.Print Mid$(ss, k, 1);
        '                x = x + Printer.TextWidth("*" & Mid$(ss, k, 1) & "*") - Printer.TextWidth("**")
        '                x = x + x_spc
        '                'X_printer = x '''
        '            Next
        '        End If

        '        Y = Y + Printer.TextHeight(ss)
        '        'Y_printer = Y '''
        '        While Left$(s, 1) = " "
        '            s = Right$(s, Len(s) - 1)
        '        End While
        '    End While
        '    'MsgBox err.Description
    End Sub

    '** funcion q permite imprimir un texto justificado en la impresora, utilidad no
    '** encontrada en el datareport
    Sub justifica_printer__(xo As Integer, xf As Integer, yo As Single, Txt As String)
        'Dim x, y1, k, ancho
        'Dim s As String, ss As String
        'Dim x_spc
        's = Txt
        'x = xo
        'y1 = yo
        'ancho = (xf - xo)

        'While s <> ""
        '    ss = ""
        '    While (s <> "") And (Printer.TextWidth(ss) <= ancho)
        '        ss = ss & Left$(s, 1)
        '        s = Right$(s, Len(s) - 1)
        '    End While
        '    If (Printer.TextWidth(ss) > ancho) Then
        '        s = Right$(ss, 1) & s
        '        ss = Left$(ss, Len(ss) - 1)
        '    End If
        '    If Right$(ss, 1) = " " Then
        '        ss = Left$(ss, Len(ss) - 1)
        '    Else
        '        If (InStr(ss, " ") > 0) And (Left$(s & " ", 1) <> " ") Then
        '            While Right$(ss, 1) <> " "
        '                s = Right$(ss, 1) & s
        '                ss = Left$(ss, Len(ss) - 1)
        '            End While
        '            ss = Left$(ss, Len(ss) - 1)
        '        End If
        '    End If
        '    x_spc = 0
        '    x = xo
        '    If (Len(ss) > 1) And (s & "" <> "") Then
        '        x_spc = (ancho - Printer.TextWidth(ss)) / (Len(ss) - 1)
        '    End If
        '    Printer.CurrentX = x
        '    Printer.CurrentY = y1

        '    If x_spc = 0 Then
        'Printer.Print ss;
        '    Else
        '        For k = 1 To Len(ss)
        '            Printer.CurrentX = x
        '    Printer.Print Mid$(ss, k, 1);
        '            x = x + Printer.TextWidth("*" & Mid$(ss, k, 1) & "*") - Printer.TextWidth("*")
        '            x = x + x_spc
        '        Next
        '    End If
        '    y1 = y1 + Printer.TextHeight(ss)
        '    While Left$(s, 1) = " "
        '        s = Right$(s, Len(s) - 1)
        '    End While
        'End While
    End Sub

    '** funcion q permite imprimir un texto justificado en la impresora, utilidad no
    '** encontrada en el datareport, en el punto de inicio x0, y margen derecho xf, desde la
    '** coordenada y0, txt es el texto a imprimir. en la variable global J se almacena el
    '** valor de la posicion Y
    Sub justifica_printer(x0, xf, y0, Txt)
        '    Dim x, Y, k, ancho
        '    Dim s As String, ss As String
        '    Dim x_spc

        '    s = Txt
        '    x = x0
        '    Y = y0
        '    ancho = (xf - x0)

        '    While s <> ""

        '        ss = ""
        '        While (s <> "") And (Printer.TextWidth(ss) <= ancho)
        '            ss = ss & Left$(s, 1)
        '            s = Right$(s, Len(s) - 1)
        '        End While
        '        If (Printer.TextWidth(ss) > ancho) Then
        '            s = Right$(ss, 1) & s
        '            ss = Left$(ss, Len(ss) - 1)
        '        End If
        '        ' aqui tenemos en ss lo maximo que cabe en una linea
        '        If Right$(ss, 1) = " " Then
        '            ss = Left$(ss, Len(ss) - 1)
        '        Else
        '            If (InStr(ss, " ") > 0) And (Left$(s & " ", 1) <> " ") Then
        '                While Right$(ss, 1) <> " "
        '                    s = Right$(ss, 1) & s
        '                    ss = Left$(ss, Len(ss) - 1)
        '                End While
        '                ss = Left$(ss, Len(ss) - 1)
        '            End If
        '        End If
        '        x_spc = 0
        '        x = x0
        '        If (Len(ss) > 1) And (s & "" <> "") Then
        '            x_spc = (ancho - Printer.TextWidth(ss)) / (Len(ss) - 1)
        '        End If
        '        Printer.CurrentX = x
        '        Printer.CurrentY = Y

        '        If x_spc = 0 Then
        'Printer.Print ss;
        '        Else
        '            For k = 1 To Len(ss)
        '                Printer.CurrentX = x
        ' Printer.Print Mid$(ss, k, 1);
        '                x = x + Printer.TextWidth("*" & Mid$(ss, k, 1) & "*") - Printer.TextWidth("**")
        '                x = x + x_spc
        '            Next
        '        End If

        '        Y = Y + Printer.TextHeight(ss) + 0.0001
        '        J = Y
        '        While Left$(s, 1) = " "
        '            s = Right$(s, Len(s) - 1)
        '        End While
        '    End While

    End Sub

    '** Funcion que convierte un nro Romano a Numero Cardinal; NumRom ="I", "II"...
    Public Function Convertir_Romanos_Num(NumRom As String) As Integer
        Dim Convertir_Romanos = 4
        Select Case NumRom
            Case "I"
                Convertir_Romanos_Num = 1
            Case "II"
                Convertir_Romanos_Num = 2
            Case "III"
                Convertir_Romanos_Num = 3
            Case "IV"
                Convertir_Romanos_Num = 4
            Case "V"
                Convertir_Romanos_Num = 5
            Case "VI"
                Convertir_Romanos_Num = 6
        End Select
    End Function

    '** Funcion que redondea los numeros flotantes a 3 digitos de un msflexgrid,
    'en la columna a redondear
    Public Sub Redondear_msh(msh As MSFlexGrid, col As Integer)
        With msh
            For I = 1 To .Rows - 1
                .TextMatrix(I, col) = Round(CDec(msh.TextMatrix(I, col)), 3)
            Next
        End With
    End Sub




End Module
