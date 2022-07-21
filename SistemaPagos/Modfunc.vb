Imports ADODB.CommandTypeEnum
Imports ADODB.DataTypeEnum
Imports ADODB.ParameterDirectionEnum
Module Modfunc
    '------------------------------------------------------------------------------------------------------------------------
    '** fecha : 20/03/03
    '** este modulo tiene como finalidad ejecutar, consultas de los
    '** diversos procesos mediante llamadas a procedimientos almacenados
    '** o funciones. Cada funcion recibe los parametros respectivos
    '------------------------------------------------------------------------------------------------------------------------

    '** esta funcion ejecuta diversas consultas a la base de datos
    '** en el parametro ssql, y devuelve los resultados en el recordset rssql
    '** en el procedimiento que lo llamo
    Public Sub Ejecuta_consultas(strSQL As String, _
    rs As ADODB.Recordset)

        rs = New ADODB.Recordset
        rs.Open(strSQL, cnStr, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic)
        'MsgBox err.Description
    End Sub

    '************************************************************************************************
    '** esta funcion ejecuta diversas comando sql a la base de datos
    '** en el parametro ssql, tales como Insert, Delete, y devuelve los resultados
    '** -1 como no realizado, 1 en caso contrario
    Public Function Ejecuta_comandos(Ssql As String) As Long
        Ejecuta_comandos = -1

        cmd = New ADODB.Command
        cmd.CommandType = CommandType.Text
        cmd.CommandText = Ssql
        cmd.ActiveConnection = cn
        cmd.Execute()
        Ejecuta_comandos = 1 'Okk
        'MsgBox err.Description
    End Function

    '************************************************************************************************
    '** Muestra las sedes que existen en la BD, los resultados son mostrados en un combobox
    Public Sub MuestraSedes_cbo(cboSede As ComboBox)
        On Error Resume Next
        Ssql = "Select * From SEDE Order By 1"
        Ejecuta_consultas(Ssql, rssql)
        With rssql
            If .EOF Or .BOF Then Exit Sub
            While Not .EOF
                'cboSede.AddItem Trim(.Fields(0))
                cboSede.Items.Add(Trim(CStr(.Fields(0))))
                .MoveNext()
            End While
        End With
    End Sub

    '************************************************************************************************
    '** esta funcion ejecuta procedimientos almacenados que tengan un solo parametro
    '** en el parametro NombreProcAlm es el nombre del proced. almadenado a ejecutar
    '** , tales como Insert, Delete, y devuelve los resultados
    '** tamano es el tamaño del parametro, VALOR es el valor enviado
    Public Function Busca(NombreProcAlm As String, tamano As Long, Parametro As String, VALOR As String) As ADODB.Recordset
        I = -1
        On Error GoTo PROC_ERR
        cmd = New ADODB.Command
        With cmd
            .ActiveConnection = cn
            .CommandType = adCmdStoredProc  'adCmdStoredProc
            .CommandText = NombreProcAlm
            .CreateParameter("RETURN_VALUE", adInteger, adParamReturnValue, 0)
            .CreateParameter(Parametro, adVarChar, adParamInput, tamano, Left(Trim(VALOR), tamano))
            Busca = .Execute
            I = .Parameters("RETURN_VALUE").Value

        End With
        cmd = Nothing
PROC_ERR:  'MsgBox err.Description
    End Function

    '************************************************************************************************
    '** Muestra las menciones que tiene el programa CODPROGRAMA, el resultado
    '** se enciuentra en rssql
    Public Sub Carga_MencionesProg(CODPROGRAMA As String)
        Ssql = "Select * From vista_Mencion Where CODPROGRAMA='" & CODPROGRAMA & "' Order By 2"
        Ejecuta_consultas(Ssql, rssql)
    End Sub

    '************************************************************************************************
    '** Muestra los programas que existen en la bd, el resultado es mostrado en
    '** un datacombo, el ultimo parametro intOpcion permite mostrar :
    '** (1)solo los programas(por defecto)/ (2) doctorados / (3)programas y doctorados
    Public Sub Muestra_Menciones(DatCProgramas As ComboBox, Optional intOpcion As Integer = 1)
        On Error Resume Next
        Select Case intOpcion
            Case 1 'solo los programas
                Ssql = "Select * From PROGRAMAS WHERE LEFt(CODPROGRAMA,2) ='PG' Order By 2"

            Case 2 'doctorados
                Ssql = "Select * From PROGRAMAS WHERE LEFt(CODPROGRAMA,2) ='DC' Order By 2"

            Case 3 'programas y doctorados
                Ssql = "Select * From PROGRAMAS Order By 2"
        End Select

        Ejecuta_consultas(Ssql, rssql)

        If rssql.EOF Or rssql.BOF Then Exit Sub 'MsgBox "NO EXISTEN PROGRAMAS REGISTRADOS ¡CONSULTE!", vbInformation, "Menciones"
        'DatCProgramas.RowSource = rssql
        DatCProgramas.DataSource = rssql
    End Sub


    '************************************************************************************************
    '** Segun el parametro Tipo:
    '(0)Devuelve el nombre de la Mencion del Doctorado de codigo: CODPROGRAMA
    '(1)Devuelve el Codigo de la Mencion del Doctorado de codigo: CODPROGRAMA
    Public Function CodMencion_Doctorado(TIPO As Integer, CODPROGRAMA As String) As String
        CodMencion_Doctorado = ""
        On Error Resume Next
        Ssql = "Select CODMENCION, NOMBRE From Mencion Where CODPROGRAMA='" & CODPROGRAMA & "'"
        Ejecuta_consultas(Ssql, rssql)
        If rssql.EOF Or rssql.BOF Then Exit Function
        CodMencion_Doctorado = IIf(TIPO = 0, rssql.Fields(0), rssql.Fields(1))
    End Function

    '************************************************************************************************
    '** consulta el valor del contador en la tabla parametro, para conocer
    '** el siguiente correlativo y generar un codigo
    Public Function Consulta_TablaParametro(NOMBRETABLA As String, CAMPO As String) As Integer
        Consulta_TablaParametro = -1
        On Error GoTo errores_
        Ssql = "SELECT MAX(" & CAMPO & ") FROM " & NOMBRETABLA
        Ejecuta_consultas(Ssql, rssql)
        Consulta_TablaParametro = 1
errores_:  'MsgBox err.Description
    End Function

    '************************************************************************************************
    '** consulta el valor del contador en la tabla parametro, para conocer
    '** el siguiente correlativo y generar un codigo
    Public Sub Asigna_msh(CodMencion As String, NUMPLAN As String, mshPlan As DataGridView)
        Ssql = "Select * From vista_CursosPlan Where CODMENCION='" & CodMencion & "' And NUMPLAN='" & NUMPLAN & "' ORDER BY 1"
        Ejecuta_consultas(Ssql, rssql)
        If rssql.EOF Or rssql.BOF Then Exit Sub
        With mshPlan
            .DataSource = rssql
            .Columns(0).Width = 350

            .Columns(1).Width = 600 'CICLO
            .Columns(2).Width = 850 'COD
            .Columns(3).Width = 5500 'NOMBRE
            .Columns(.Columns.Count - 1).Width = 0 '
            .Columns(.Columns.Count - 2).Width = 0 '
            .Columns(.Columns.Count - 3).Width = 0 '
            .MergeCells = flexMergeFree
            .MergeCol(1) = True

            '.ColWidth(0) = 350
            '.ColWidth(1) = 600 'CICLO
            '.ColWidth(2) = 850 'COD
            '.ColWidth(3) = 5500 'NOMBRE
            '.ColWidth(.Cols - 1) = 0 '
            '.ColWidth(.Cols - 2) = 0 '
            '.ColWidth(.Cols - 3) = 0 '
            '.MergeCells = flexMergeFree
            '.MergeCol(1) = True
        End With
    End Sub

    '************************************************************************************************
    '** elimina un curso del plan de estudios, llama a la funcion ejecuta_comandos
    Public Sub EliminaCursoPlan(NUMPLAN As String, CICLO As String, CODCURSO As String, _
    CodMencion As String, mshPlan As MSHFlexGrid)
        On Error GoTo errores
        Ssql = "Delete From PLANESTUDIOS Where NUMPLAN='" & _
                NUMPLAN & "' AND CICLO='" & CICLO & "' AND CODCURSO='" & CODCURSO & "'"
        I = Ejecuta_comandos(Ssql)
        If I = 1 Then
            MsgBox("Curso ha sido Eliminado...", vbInformation, "**  Plan de Estudios")
            'MOSTRAR 'Asigna_msh CodMencion, NUMPLAN, mshPlan
errores:
        Else : MsgBox("Curso no Fue Eliminado !!", vbInformation, "**  Plan de Estudios")
        End If
    End Sub

    '************************************************************************************************
    '** Muestra los planes de estudio que tiene una mencion, los resultados son mostrados
    '** en un datacombo
    Public Sub MuestraNumPlanes(CodMencion As String, DatCNumPlan As DataCombo)
        On Error GoTo errores_
        Ssql = "Select * From vista_NumPlanMencion Where CODMENCION='" & CodMencion & "'"
        Ejecuta_consultas(Ssql, rssql)
        'If rssql.EOF Or rssql.BOF Then Exit Sub
        DatCNumPlan.ListField = "NUMPLAN"
        DatCNumPlan.RowSource = rssql
        DatCNumPlan.SetFocus()
errores_:
    End Sub

    '************************************************************************************************
    '** Muestra el reporte del palan de estudio, como rseultado de la llamada al procedimiento
    '** almacenado que esta enlazado con el comando DatEPostgrado.CmdPlanesEst_Grouping
    Public Sub ReportePlanEstudios(NUMPLAM As String, _
    Programas As String, MENCION As String, Optional sem As String = "2001-1")
        On Error GoTo errores_
        On Error Resume Next
        With DatRPlanEstudios
            .Sections("ReportHeader").Controls("lblProg").Caption = "PROGRAMA DE " & Programas
            If Trim(MENCION) <> "" Then
                .Sections("ReportHeader").Controls("lblMencion").Caption = ("MENCION: " + MENCION)
            End If
            .Sections("ReportHeader").Controls("lblNumPlan").Caption = "PLAN DE ESTUDIOS: " & sem
            .Sections("ReportHeader").Controls("lblCodPlan").Caption = "NRO PLAN: " & NUMPLAM
            DatEPostgrado.CmdPlanesEst_Grouping(Trim(NUMPLAM))
            .Show()
        End With
errores_:  'MsgBox Err.Description
    End Sub

    '************************************************************************************************
    '** Muestra las sedes que existen en la BD, los resultados son mostrados en un datacombo
    Public Sub MuestraSedes(DatcSede As DataCombo)
        Ssql = "Select * From SEDE Order By 1"
        Ejecuta_consultas(Ssql, rssql)
        If rssql.EOF Or rssql.BOF Then Exit Sub
        DatcSede.RowSource = rssql
    End Sub

    '************************************************************************************************
    'Devuelve a los alumnos por mencion - semestre - sede
    Public Sub Mostrar_AlumnosMencion(PROMOCION As String, _
    CodMencion As String, SEDE As String)

        On Error GoTo PROC_ERR
        cmd = New ADODB.Command
        With cmd
            .CommandText = "sp_MuestraAlumnoMencion"
            .CommandType = adCmdStoredProc
            .ActiveConnection = DatEPostgrado.CnnPostgrado
            .Parameters.Append.CreateParameter("RETURN_VALUE", adInteger, adParamReturnValue, 0)
            .Parameters.Append.CreateParameter("CODMENCION", adChar, adParamInput, 4, CodMencion)
            .Parameters.Append.CreateParameter("SEDE", adVarChar, adParamInput, 30, SEDE)
            .Parameters.Append.CreateParameter("PROMOCION", adChar, adParamInput, 6, PROMOCION)
            rssql = .Execute
            I = .Parameters("RETURN_VALUE") '
        End With
        cmd = Nothing
PROC_ERR:  'MsgBox Err.Description
    End Sub

    '************************************************************************************************
    'Devuelve a los alumnos invictos de una determinada promocion y mencion
    ' que hayan cumplido los creditos de su plan de estudios, con nota aprobatoria
    Public Function Mostrar_AlumnosInvictos(PROMOCION As String, _
    CodMencion As String, SEMESTRE As String, TCRED As Integer, _
    Optional NOTA As Double = 12) As ADODB.Recordset

        On Error GoTo PROC_ERR
        cmd = New ADODB.Command
        With cmd
            .CommandText = "sp_AlumnosInvictos"
            .CommandType = adCmdStoredProc
            .ActiveConnection = DatEPostgrado.CnnPostgrado
            .Parameters.Append.CreateParameter("RETURN_VALUE", adInteger, adParamReturnValue, 0)
            .Parameters.Append.CreateParameter("PROMOCION", adChar, adParamInput, 6, PROMOCION)
            .Parameters.Append.CreateParameter("CODMENCION", adChar, adParamInput, 4, CodMencion)
            .Parameters.Append.CreateParameter("SEMESTRE", adChar, adParamInput, 6, SEMESTRE)
            .Parameters.Append.CreateParameter("TCRED", adTinyInt, adParamInput, , TCRED)
            .Parameters.Append.CreateParameter("NOTA", adDouble, adParamInput, , NOTA)
            Mostrar_AlumnosInvictos = .Execute
            I = .Parameters("RETURN_VALUE") 'filas afectadas
        End With
        cmd = Nothing
PROC_ERR:  'MsgBox Err.Description
    End Function

    '************************************************************************************************
    'Devuelve a los alumnos desaprobados de una determinada promocion y mencion
    ' que hayan realizado mas de cuatro ciclos
    Public Function Mostrar_AlumnosDesap(PROMOCION As String, _
    CodMencion As String, SEMESTRE As String, Optional NOTA As Double = 12) As ADODB.Recordset

        On Error GoTo PROC_ERR
        cmd = New ADODB.Command
        With cmd
            .CommandText = "sp_AlumnosDesaprob"
            .CommandType = adCmdStoredProc
            .ActiveConnection = DatEPostgrado.CnnPostgrado
            .Parameters.Append.CreateParameter("RETURN_VALUE", adInteger, adParamReturnValue, 0)
            .Parameters.Append.CreateParameter("PROMOCION", adChar, adParamInput, 6, PROMOCION)
            .Parameters.Append.CreateParameter("CODMENCION", adChar, adParamInput, 4, CodMencion)
            .Parameters.Append.CreateParameter("SEMESTRE", adChar, adParamInput, 6, SEMESTRE)
            .Parameters.Append.CreateParameter("NOTA", adDouble, adParamInput, , NOTA)
            Mostrar_AlumnosDesap = .Execute
            I = .Parameters("RETURN_VALUE") 'filas afectadas
        End With
        cmd = Nothing
PROC_ERR:  'MsgBox Err.Description
    End Function

    'devuelve el ciclo del alumno, semestres de evaluado
    Public Function TraeCicloAlumno(CODALUMNO As String) As String
        TraeCicloAlumno = ""
        On Error GoTo PROC_ERR
        cmd = New ADODB.Command
        With cmd
            .CommandText = "sp_CicloAlumno"
            .CommandType = adCmdStoredProc
            .ActiveConnection = DatEPostgrado.CnnPostgrado
            .Parameters.Append.CreateParameter("RETURN_VALUE", adInteger, adParamReturnValue, 0)
            .Parameters.Append.CreateParameter("CODALUMNO", adVarChar, adParamInput, 10, CODALUMNO)
            rssql = .Execute
            I = .Parameters("RETURN_VALUE") '
            TraeCicloAlumno = IIf(Trim(rssql.Fields(1)) = 0, 1, IIf(rssql.Fields(1) > 4, 4, Trim(rssql.Fields(1))))
        End With
        cmd = Nothing
PROC_ERR:  'MsgBox Err.Description
    End Function

    '************************************************************************************************
    'Devuelve el valor de verdad o falso, segun si un determinado alumno se encuentra o no matriculado
    ' en un semestre determinado
    Public Function EstaMatriculadoAlumno(CODALUMNO As String, SEMESTRE As String) As Boolean
        EstaMatriculadoAlumno = False
        Ssql = "SELECT CODALUMNO FROM INSCRIPCIONALUMNOS Where " & _
                "CODALUMNO='" & CODALUMNO & "' And SEMESTRE='" & SEMESTRE & "'"
        Ejecuta_consultas(Ssql, rssql)
        If rssql.EOF Or rssql.BOF Then Exit Function
        EstaMatriculadoAlumno = True
    End Function

    '************************************************************************************************
    'Devuelve el nombre de los alumnos ingresantes (solo se han registrado)segun coincidencias del parametro letra
    'que perternecen a una mencion determinada el numero de alumnos se almacena en el parametro I
    Public Function AlumnosIngPorLetra(Letra As String, CodMencion As String) As ADODB.Recordset
        I = -1
        On Error GoTo PROC_ERR
        cmd = New ADODB.Command
        With cmd
            .CommandText = "sp_AlumnosIngresantesLetra"
            .CommandType = adCmdStoredProc
            .ActiveConnection = DatEPostgrado.CnnPostgrado
            .Parameters.Append.CreateParameter("RETURN_VALUE", adInteger, adParamReturnValue, 0)
            .Parameters.Append.CreateParameter("Letra", adVarChar, adParamInput, 20, Letra)
            .Parameters.Append.CreateParameter("CODMENCION", adChar, adParamInput, 4, CodMencion)
            AlumnosIngPorLetra = .Execute
            I = .Parameters("RETURN_VALUE") '
        End With
        cmd = Nothing
PROC_ERR:  'MsgBox Err.Description
    End Function

    '************************************************************************************************
    'Devuelve el nombre de los alumnos que son regulares segun coincidencias del parametro letra
    'que perternecen a una mencion determinada, el numero de alumnos se almacena en el parametro I
    Public Function AlumnosRegulares(Letra As String, CodMencion As String) As ADODB.Recordset
        I = -1
        On Error GoTo PROC_ERR
        cmd = New ADODB.Command
        With cmd
            .CommandText = "sp_AlumnosRegularesLetra"
            .CommandType = adCmdStoredProc
            .ActiveConnection = DatEPostgrado.CnnPostgrado
            .Parameters.Append.CreateParameter("RETURN_VALUE", adInteger, adParamReturnValue, 0)
            .Parameters.Append.CreateParameter("Letra", adVarChar, adParamInput, 20, Letra)
            .Parameters.Append.CreateParameter("CODMENCION", adChar, adParamInput, 4, CodMencion)
            AlumnosRegulares = .Execute
            I = .Parameters("RETURN_VALUE") '
        End With
        cmd = Nothing
PROC_ERR:  'MsgBox Err.Description
    End Function

    '************************************************************************************************
    'Devuelve el nombre de los alumnos que estan matriculados en un semestre, segun coincidencias
    ' del parametro letra, que perternecen a una mencion determinada, el numero de alumnos se almacena en el parametro I
    Public Function AlumnosMatricPorLetra(Letra As String, CodMencion As String, _
    SEMESTRE As String, Optional ind As Integer = 1) As ADODB.Recordset
        I = -1
        On Error GoTo PROC_ERR
        cmd = New ADODB.Command
        With cmd
            .CommandText = "sp_AlumnosMatricLetra"
            .CommandType = adCmdStoredProc
            .ActiveConnection = DatEPostgrado.CnnPostgrado
            .Parameters.Append.CreateParameter("RETURN_VALUE", adInteger, adParamReturnValue, 0)
            .Parameters.Append.CreateParameter("Letra", adVarChar, adParamInput, 20, Letra)
            .Parameters.Append.CreateParameter("CODMENCION", adChar, adParamInput, 4, CodMencion)
            .Parameters.Append.CreateParameter("SEMESTRE", adChar, adParamInput, 6, SEMESTRE)
            .Parameters.Append.CreateParameter("Ind", adTinyInt, adParamInput, , ind)
            AlumnosMatricPorLetra = .Execute
            I = .Parameters("RETURN_VALUE") '
        End With
        cmd = Nothing
PROC_ERR:  'MsgBox err.Description
    End Function

    '************************************************************************************************
    'funcion que devuelve la relacion de alumnos de una determinada mencion, que han sido evaluados en un semestre determinado,
    'con la finalidad de obtener sus notas semestrales
    Public Function AlumnosBoletaNotas(Letra As String, CodMencion As String, _
    SEMESTRE As String) As ADODB.Recordset
        I = -1
        On Error GoTo PROC_ERR
        cmd = New ADODB.Command
        With cmd
            .CommandText = "sp_AlumnosBoletaNotaLetra"
            .CommandType = adCmdStoredProc
            .ActiveConnection = DatEPostgrado.CnnPostgrado
            .Parameters.Append.CreateParameter("RETURN_VALUE", adInteger, adParamReturnValue, 0)
            .Parameters.Append.CreateParameter("Letra", adVarChar, adParamInput, 20, Letra)
            .Parameters.Append.CreateParameter("CODMENCION", adChar, adParamInput, 4, CodMencion)
            .Parameters.Append.CreateParameter("SEMESTRE", adChar, adParamInput, 6, SEMESTRE)
            AlumnosBoletaNotas = .Execute
            I = .Parameters("RETURN_VALUE") '
        End With
        cmd = Nothing
PROC_ERR:  'MsgBox Err.Description
    End Function

    '************************************************************************************************
    'funcion que devuelve las coincidencias de alumnos de una determinada mencion, a quienes se les ha registrado
    'inscripcion de cursos, con la finalidad de consultar su historial academico
    Public Function AlumnosHistPorLetra(Letra As String, _
    CodMencion As String) As ADODB.Recordset ', SEDE As String
        I = -1
        On Error GoTo PROC_ERR
        cmd = New ADODB.Command
        With cmd
            .CommandText = "sp_AlumnosHistorialesLetra"
            .CommandType = adCmdStoredProc
            .ActiveConnection = DatEPostgrado.CnnPostgrado
            .Parameters.Append.CreateParameter("RETURN_VALUE", adInteger, adParamReturnValue, 0)
            .Parameters.Append.CreateParameter("Letra", adVarChar, adParamInput, 20, Letra)
            .Parameters.Append.CreateParameter("CODMENCION", adChar, adParamInput, 4, CodMencion)
            '.Parameters.Append .CreateParameter("SEDE", adChar, adParamInput, 30, SEDE)
            AlumnosHistPorLetra = .Execute
            I = .Parameters("RETURN_VALUE") '
        End With
        cmd = Nothing
PROC_ERR:  'MsgBox Err.Description
    End Function


    '************************************************************************************************
    'busca a los docentes que estan en un determinado semestre en programacion academica
    'busca las coincidencias segun el parmetro Letra
    Public Function ProfesorEnProgLetra(Letra As String, SEMESTRE As String) As ADODB.Recordset
        I = -1
        On Error GoTo PROC_ERR
        cmd = New ADODB.Command
        With cmd
            .CommandText = "sp_ProfesorEnProgLetra"
            .CommandType = adCmdStoredProc
            .ActiveConnection = DatEPostgrado.CnnPostgrado
            .Parameters.Append.CreateParameter("RETURN_VALUE", adInteger, adParamReturnValue, 0)
            .Parameters.Append.CreateParameter("Letra", adVarChar, adParamInput, 20, Letra)
            .Parameters.Append.CreateParameter("Semestre", adChar, adParamInput, 6, SEMESTRE)
            ProfesorEnProgLetra = .Execute
            I = .Parameters("RETURN_VALUE") '
        End With
        cmd = Nothing
PROC_ERR:  'MsgBox Err.Description
    End Function

    '***************************************************************************************************************
    '**************************  FUNCIONES DE Tesis ************************************************************
    '** devuelve la relacion de alumnos q tienen por coincidencia una frase
    '** del tema de su tesis
    Public Function TemasTesisPorLetra(Letra As String) As ADODB.Recordset
        I = 0
        On Error GoTo PROC_ERR
        cmd = New ADODB.Command
        With cmd
            .CommandText = "sp_BuscaTitulosTesis"
            .CommandType = adCmdStoredProc
            .ActiveConnection = DatEPostgrado.CnnPostgrado
            .Parameters.Append.CreateParameter("RETURN_VALUE", adInteger, adParamReturnValue, 0)
            .Parameters.Append.CreateParameter("Letra", adVarChar, adParamInput, 20, Letra)
            TemasTesisPorLetra = .Execute
            I = .Parameters("RETURN_VALUE") '
        End With
        cmd = Nothing
PROC_ERR:  'MsgBox Err.Description
    End Function

    '***************************************************************************************************************
    '** devuelve la relacion de alumnos q han sustentado, ordendos por programa y por fecha
    '** el parametro Tipo = 0 devuelve la relacion de todos los programas, en caso contrario
    '** devuelve la relacion de un programa especifico.
    Public Function Alu_Sustentantes(TIPO As Integer, CODPROGRAMA As String) As ADODB.Recordset
        I = 0
        On Error GoTo PROC_ERR
        cmd = New ADODB.Command
        With cmd
            .CommandText = "sp_Alu_Sustentantes"
            .CommandType = adCmdStoredProc
            .ActiveConnection = DatEPostgrado.CnnPostgrado
            .Parameters.Append.CreateParameter("RETURN_VALUE", adInteger, adParamReturnValue, 0)
            .Parameters.Append.CreateParameter("Tipo", adTinyInt, adParamInput, , TIPO)
            .Parameters.Append.CreateParameter("CODPROGRAMA", adVarChar, adParamInput, 5, CODPROGRAMA)
            Alu_Sustentantes = .Execute
            I = .Parameters("RETURN_VALUE") '
        End With
        cmd = Nothing
PROC_ERR:  'MsgBox Err.Description
    End Function

    '************************************************************************************************
    'busca a los alumnos que se estan en el superior al tercer ciclo para
    'tramitar su tesis, devuelce a los alumnos que tengan coincidencias, con la letra
    'especificada, dependiendo si se restringe la busqueda por mencion si CodMencion ="0000"
    Public Function AlumnosTesisPorLetra(Letra As String, _
    CodMencion As String) As ADODB.Recordset
        I = -1
        On Error GoTo PROC_ERR
        cmd = New ADODB.Command
        With cmd
            .CommandText = "sp_AlumnosTesisLetra"
            .CommandType = adCmdStoredProc
            .ActiveConnection = DatEPostgrado.CnnPostgrado
            .Parameters.Append.CreateParameter("RETURN_VALUE", adInteger, adParamReturnValue, 0)
            .Parameters.Append.CreateParameter("Letra", adVarChar, adParamInput, 20, Letra)
            .Parameters.Append.CreateParameter("CODMENCION", adChar, adParamInput, 4, CodMencion)
            AlumnosTesisPorLetra = .Execute
            I = .Parameters("RETURN_VALUE") '
        End With
        cmd = Nothing
PROC_ERR:  'MsgBox Err.Description
    End Function

    '************************************************************************************************
    'busca a los alumnos que inician el tramite de abstrac, por nombre con la
    'condicion  que existan en la tabla tesis
    Public Function AlumnosAbstracPorLetra(Letra As String) As ADODB.Recordset
        I = -1
        On Error GoTo PROC_ERR
        cmd = New ADODB.Command
        With cmd
            .CommandText = "sp_AlumnosAbstracLetra"
            .CommandType = adCmdStoredProc
            .ActiveConnection = DatEPostgrado.CnnPostgrado
            .Parameters.Append.CreateParameter("RETURN_VALUE", adInteger, adParamReturnValue, 0)
            .Parameters.Append.CreateParameter("Letra", adVarChar, adParamInput, 20, Letra)
            AlumnosAbstracPorLetra = .Execute
            I = .Parameters("RETURN_VALUE") '
        End With
        cmd = Nothing
PROC_ERR:  'MsgBox Err.Description
    End Function

    '*********************************************************************************************************
    'funcion que devuelve si el codigo del alumno esta en el ciclo superior al tercero,
    'para , condicion para tramitar su tesis
    Public Function AlumnosTesisPorCodigo(CODALUMNO As String) As ADODB.Recordset
        I = -1
        On Error GoTo PROC_ERR
        cmd = New ADODB.Command
        With cmd
            .CommandText = "sp_AlumnosTesisCodigo"
            .CommandType = adCmdStoredProc
            .ActiveConnection = DatEPostgrado.CnnPostgrado
            .Parameters.Append.CreateParameter("RETURN_VALUE", adInteger, adParamReturnValue, 0)
            .Parameters.Append.CreateParameter("CODALUMNO", adVarChar, adParamInput, 10, CODALUMNO)
            AlumnosTesisPorCodigo = .Execute
            I = .Parameters("RETURN_VALUE") '
        End With
        cmd = Nothing
PROC_ERR:  'MsgBox Err.Description
    End Function

    '*********************************************************************************************************
    'funcion que devuelve si el codigo del alumno esta en el ciclo superior al tercero,
    'para , condicion para tramitar su tesis
    Public Function AlumnosAbstracPorCodigo(CODALUMNO As String) As ADODB.Recordset
        I = -1
        On Error GoTo PROC_ERR
        cmd = New ADODB.Command
        With cmd
            .CommandText = "sp_AlumnosAbstracCodigo"
            .CommandType = adCmdStoredProc
            .ActiveConnection = DatEPostgrado.CnnPostgrado
            .Parameters.Append.CreateParameter("RETURN_VALUE", adInteger, adParamReturnValue, 0)
            .Parameters.Append.CreateParameter("CODALUMNO", adVarChar, adParamInput, 10, CODALUMNO)
            AlumnosAbstracPorCodigo = .Execute
            I = .Parameters("RETURN_VALUE") '
        End With
        cmd = Nothing
PROC_ERR:  'MsgBox Err.Description
    End Function

    '*********************************************************************************************************
    'segun el parametro ind, devuelve el codigo(ind = 1) o nombres (ind <> 1)de alumnos de coincidencias Letra
    'de una mencion
    Public Function AlumnoEnTesisPorCodLetra(Letra As String, CODALUMNO As String, _
    CodMencion As String, ind As Integer) As ADODB.Recordset
        I = -1
        On Error GoTo PROC_ERR
        cmd = New ADODB.Command
        With cmd
            .CommandText = "sp_AlumnosEnTesisCodLetra"
            .CommandType = adCmdStoredProc
            .ActiveConnection = DatEPostgrado.CnnPostgrado
            .Parameters.Append.CreateParameter("RETURN_VALUE", adInteger, adParamReturnValue, 0)
            .Parameters.Append.CreateParameter("Letra", adVarChar, adParamInput, 20, Letra)
            .Parameters.Append.CreateParameter("CODALUMNO", adVarChar, adParamInput, 10, CODALUMNO)
            .Parameters.Append.CreateParameter("CODMENCION", adVarChar, adParamInput, 40, CodMencion)
            .Parameters.Append.CreateParameter("Ind", adTinyInt, adParamInput, , ind)
            AlumnoEnTesisPorCodLetra = .Execute
            I = .Parameters("RETURN_VALUE") '
        End With
        cmd = Nothing
PROC_ERR:  'MsgBox Err.Description
    End Function

    '*********************************************************************************************************
    '*********************************************************************************************************

    '************************************************************************************************
    '** esta funcion tiene como finalidad, traer todos los grupos de un curso
    '** que han sido programados
    Public Function TraeGruposEnProg(CodMencion As String, SEMESTRE As String, _
    SEDE As String, CODCURSO As String) As ADODB.Recordset
        On Error GoTo errores_
        I = -1
        cmd = New ADODB.Command
        With cmd
            .CommandText = "sp_TraeGrupoProgAcademica"
            .CommandType = adCmdStoredProc
            .ActiveConnection = DatEPostgrado.CnnPostgrado
            .Parameters.Append.CreateParameter("RETURN_VALUE", adInteger, adParamReturnValue, 0)
            .Parameters.Append.CreateParameter("CODMENCION", adChar, adParamInput, 4, CodMencion)
            .Parameters.Append.CreateParameter("SEMESTRE", adChar, adParamInput, 6, SEMESTRE)
            .Parameters.Append.CreateParameter("SEDE", adVarChar, adParamInput, 30, SEDE)
            .Parameters.Append.CreateParameter("CODCURSO", adChar, adParamInput, 6, CODCURSO)
            TraeGruposEnProg = .Execute
            I = .Parameters("RETURN_VALUE") '
        End With
        cmd = Nothing
errores_:
    End Function

    '** consulta la programacion y verifica cual es la sgte seccion a crear
    '** , para ello se ejecuta el procedimiento almacenado sp_TraeSeccion.
    '** la seccion es mostrada en SECCION
    Public Sub TraerSeccion(CodMencion As String, SEMESTRE As String, _
    CODCURSO As String, SEDE As String, GRUPO As Integer, SECCION As TextBox)

        On Error GoTo errores_
        cmd = New ADODB.Command
        With cmd
            .CommandText = "sp_TraeSeccion"
            .CommandType = adCmdStoredProc
            .ActiveConnection = DatEPostgrado.CnnPostgrado
            .Parameters.Append.CreateParameter("RETURN_VALUE", adInteger, adParamReturnValue, 0)
            .Parameters.Append.CreateParameter("CODMENCION", adChar, adParamInput, 4, CodMencion)
            .Parameters.Append.CreateParameter("SEMESTRE", adChar, adParamInput, 6, SEMESTRE)
            .Parameters.Append.CreateParameter("CODCURSO", adChar, adParamInput, 6, CODCURSO)
            .Parameters.Append.CreateParameter("SEDE", adVarChar, adParamInput, 30, SEDE)
            .Parameters.Append.CreateParameter("GRUPO", adVarChar, adParamInput, 2, GRUPO)
            .Execute(Options:=adExecuteNoRecords)
            I = .Parameters("RETURN_VALUE") 'la seccion
        End With
        cmd = Nothing
        SECCION = Format(I, "00")
errores_:
    End Sub

    '** esta funcion tiene como finalidad, traer todas las secciones programadas en el
    '** curso respectivo
    Public Function TraeSeccionesEnProg(CodMencion As String, SEMESTRE As String, _
    SEDE As String, CODCURSO As String) As ADODB.Recordset
        On Error GoTo errores_
        I = -1
        cmd = New ADODB.Command
        With cmd
            .CommandText = "sp_TraeSeccProgAcademica"
            .CommandType = adCmdStoredProc
            .ActiveConnection = DatEPostgrado.CnnPostgrado
            .Parameters.Append.CreateParameter("RETURN_VALUE", adInteger, adParamReturnValue, 0)
            .Parameters.Append.CreateParameter("CODMENCION", adChar, adParamInput, 4, CodMencion)
            .Parameters.Append.CreateParameter("SEMESTRE", adChar, adParamInput, 6, SEMESTRE)
            .Parameters.Append.CreateParameter("SEDE", adVarChar, adParamInput, 30, SEDE)
            .Parameters.Append.CreateParameter("CODCURSO", adChar, adParamInput, 6, CODCURSO)
            TraeSeccionesEnProg = .Execute
            I = .Parameters("RETURN_VALUE") '
        End With
        cmd = Nothing
errores_:
    End Function

    '** funcion que devuelve los datos de un alumno matriculado en un semestre determinado
    '** tales como codigo , nombre y su numplan
    Public Function TraeAlumnoMatricSem(CODALUMNO As String, _
    SEMESTRE As String) As ADODB.Recordset
        On Error GoTo errores_
        I = -1
        cmd = New ADODB.Command
        With cmd
            .CommandText = "sp_TraeAlumnoMatriculadoSem"
            .CommandType = adCmdStoredProc
            .ActiveConnection = DatEPostgrado.CnnPostgrado
            '.Parameters.Append .CreateParameter("RETURN_VALUE", adInteger, adParamReturnValue, 0)
            .Parameters.Append.CreateParameter("CODALUMNO", adVarChar, adParamInput, 10, CODALUMNO)
            .Parameters.Append.CreateParameter("SEMESTRE", adChar, adParamInput, 6, SEMESTRE)
            TraeAlumnoMatricSem = .Execute
            'I = .Parameters("RETURN_VALUE") '
            I = 1
        End With
        cmd = Nothing
errores_:
    End Function

    '** funcion que devuelve la relacion de alumno POR MENCION que se encuentran registrados en un
    'acta, buscada por grupo: SEMESTRE -GRUPO-CODCURSO
    Public Function TraerActaPromocional(SEDE As String, _
    SEMESTRE As String, CodMencion As String, GRUPO As String, _
    CODCURSO As String) As ADODB.Recordset
        'Trae el Acta Promocional por Grupo
        On Error GoTo PROC_ERR
        cmd = New ADODB.Command
        With cmd
            .CommandText = "sp_Notas"
            .CommandType = adCmdStoredProc
            .ActiveConnection = DatEPostgrado.CnnPostgrado
            .Parameters.Append.CreateParameter("RETURN_VALUE", adInteger, adParamReturnValue, 0)
            .Parameters.Append.CreateParameter("SEDE", adVarChar, adParamInput, 30, SEDE)
            .Parameters.Append.CreateParameter("SEMESTRE", adVarChar, adParamInput, 6, SEMESTRE)
            .Parameters.Append.CreateParameter("CODMENCION", adChar, adParamInput, 4, CodMencion)
            .Parameters.Append.CreateParameter("GRUPO", adChar, adParamInput, 2, GRUPO)
            .Parameters.Append.CreateParameter("CODCURSO", adChar, adParamInput, 6, CODCURSO)
            TraerActaPromocional = .Execute
            I = .Parameters("RETURN_VALUE") '
        End With
        cmd = Nothing
PROC_ERR:  'MsgBox Err.Description
    End Function

    '** funcion que devuelve la relacion de alumno POR MENCION que se encuentran registrados en un
    '** acta, buscada por seccion: SEMESTRE -SECCION-CODCURSO
    Public Function TraerActaPromocionalSeccion(SEDE As String, _
    SEMESTRE As String, CodMencion As String, SECCION As Integer, _
    CODCURSO As String) As ADODB.Recordset
        'Trae el Acta Promocional por seccion
        On Error GoTo PROC_ERR
        cmd = New ADODB.Command
        With cmd
            .CommandText = "sp_NotasPorSeccion"
            .CommandType = adCmdStoredProc
            .ActiveConnection = DatEPostgrado.CnnPostgrado
            .Parameters.Append.CreateParameter("RETURN_VALUE", adInteger, adParamReturnValue, 0)
            .Parameters.Append.CreateParameter("SEDE", adVarChar, adParamInput, 30, SEDE)
            .Parameters.Append.CreateParameter("SEMESTRE", adVarChar, adParamInput, 6, SEMESTRE)
            .Parameters.Append.CreateParameter("CODMENCION", adChar, adParamInput, 4, CodMencion)
            .Parameters.Append.CreateParameter("SECCION", adTinyInt, adParamInput, , SECCION)
            .Parameters.Append.CreateParameter("CODCURSO", adChar, adParamInput, 6, CODCURSO)
            TraerActaPromocionalSeccion = .Execute
            I = .Parameters("RETURN_VALUE") '
        End With
        cmd = Nothing
PROC_ERR:  'MsgBox Err.Description
    End Function

    '** funcion que devuelve las secciones que tiene asigando un curso en programacion academica, dando como parametros de busqueda
    'el @CODMENCION , el semestre: @SEMESTRE, la @SEDE, el @CODCURSO , y el @GRUPO
    Public Function TraeSeccSegunGrupo(CodMencion As String, SEMESTRE As String, _
    SEDE As String, CODCURSO As String, GRUPO As String) As ADODB.Recordset
        On Error GoTo PROC_ERR
        cmd = New ADODB.Command
        With cmd
            .CommandText = "sp_TraeSeccSegunGrupo"
            .CommandType = adCmdStoredProc
            .ActiveConnection = DatEPostgrado.CnnPostgrado
            .Parameters.Append.CreateParameter("RETURN_VALUE", adInteger, adParamReturnValue, 0)
            .Parameters.Append.CreateParameter("CODMENCION", adChar, adParamInput, 4, CodMencion)
            .Parameters.Append.CreateParameter("SEMESTRE", adVarChar, adParamInput, 6, SEMESTRE)
            .Parameters.Append.CreateParameter("SEDE", adVarChar, adParamInput, 30, SEDE)
            .Parameters.Append.CreateParameter("CODCURSO", adChar, adParamInput, 6, CODCURSO)
            .Parameters.Append.CreateParameter("GRUPO", adChar, adParamInput, 2, GRUPO)
            TraeSeccSegunGrupo = .Execute
            I = .Parameters("RETURN_VALUE") '
        End With
        cmd = Nothing
PROC_ERR:  'MsgBox Err.Description
    End Function

    '** funcion que devuelve las secciones que tiene asigando un curso en programacion academica, dando como parametros de busqueda
    '** el @CODMENCION , el semestre: @SEMESTRE, la @SEDE, el @CODCURSO , y la @SECCION
    Public Function TraeGrupoSegunSecc(CodMencion As String, SEMESTRE As String, _
    SEDE As String, CODCURSO As String, SECCION As Integer) As ADODB.Recordset
        On Error GoTo PROC_ERR
        cmd = New ADODB.Command
        With cmd
            .CommandText = "sp_TraeGrupoSegunSecc"
            .CommandType = adCmdStoredProc
            .ActiveConnection = DatEPostgrado.CnnPostgrado
            .Parameters.Append.CreateParameter("RETURN_VALUE", adInteger, adParamReturnValue, 0)
            .Parameters.Append.CreateParameter("CODMENCION", adChar, adParamInput, 4, CodMencion)
            .Parameters.Append.CreateParameter("SEMESTRE", adVarChar, adParamInput, 6, SEMESTRE)
            .Parameters.Append.CreateParameter("SEDE", adVarChar, adParamInput, 30, SEDE)
            .Parameters.Append.CreateParameter("CODCURSO", adChar, adParamInput, 6, CODCURSO)
            .Parameters.Append.CreateParameter("SECCION", adTinyInt, adParamInput, , SECCION)
            TraeGrupoSegunSecc = .Execute
            I = .Parameters("RETURN_VALUE") '
        End With
        cmd = Nothing
PROC_ERR:  'MsgBox Err.Description
    End Function

    ' pa que devuelve la relacion de cursos de un alumno con sus respectivas evaluaciones en un semestre
    '( su etiqueta )
    Public Function TraerBoletaNotas(CODALUMNO As String, _
    SEMESTRE As String) As ADODB.Recordset

        On Error GoTo PROC_ERR
        cmd = New ADODB.Command
        With cmd
            .CommandText = "sp_BoletaNotas"
            .CommandType = adCmdStoredProc
            .ActiveConnection = DatEPostgrado.CnnPostgrado
            .Parameters.Append.CreateParameter("RETURN_VALUE", adInteger, adParamReturnValue, 0)
            .Parameters.Append.CreateParameter("CODALUMNO", adVarChar, adParamInput, 10, CODALUMNO)
            .Parameters.Append.CreateParameter("SEMESTRE", adVarChar, adParamInput, 6, SEMESTRE)
            TraerBoletaNotas = .Execute
            I = .Parameters("RETURN_VALUE") '
        End With
        cmd = Nothing
PROC_ERR:  'MsgBox Err.Description
    End Function

    Public Function TraerPromNumCredSemestral(CODALUMNO As String, _
    SEMESTRE As String, NTCRED As Integer) As ADODB.Recordset

        On Error GoTo PROC_ERR
        cmd = New ADODB.Command
        With cmd
            .CommandText = "sp_PromNumCreditosSemestral"
            .CommandType = adCmdStoredProc
            .ActiveConnection = DatEPostgrado.CnnPostgrado
            .Parameters.Append.CreateParameter("CODALUMNO", adVarChar, adParamInput, 10, CODALUMNO)
            .Parameters.Append.CreateParameter("SEMESTRE", adVarChar, adParamInput, 6, SEMESTRE)
            .Parameters.Append.CreateParameter("NTCRED", adTinyInt, adParamOutput, , Null)
            TraerPromNumCredSemestral = .Execute
            NTCRED = .Parameters("NTCRED")
        End With
        cmd = Nothing
PROC_ERR:  'MsgBox Err.Description
    End Function

    Public Sub MuestraSemestresNotas(DatCSemestre As DataCombo)
        Ssql = "Select Distinct SEMESTRE From NOTAS Order By 1"
        Ejecuta_consultas(Ssql, rssql)
        DatCSemestre.ListField = "SEMESTRE"
        DatCSemestre.RowSource = rssql
    End Sub

    Public Sub Trae_EspecProfesion(DatCEsp As DataCombo, DatCProf As DataCombo)
        Ssql = "Select * From PROFESIONES Order By 1"
        Ejecuta_consultas(Ssql, rssql)
        If rssql.EOF Or rssql.BOF Then Exit Sub
        DatCEsp.RowSource = rssql
        DatCProf.RowSource = rssql
    End Sub

    '** actualiza el directorio de un alumno o un profesor segun lo indicque la variable ind
    ' si ind=1 se actualiZan los datos del alumno
    Public Function ActualizaAlumnoProf(CODIGO As String, NOMBRE As String, _
    DIRECCION As String, TELEFONO As String, EMAIL As String, CENTROTRABAJO As String, _
    PROFESION As String, ESPECIALIDAD As String, NUMPROMO As Integer, _
    Sexo As String, ind As Integer, NUMPLAN As String) As Integer
        On Error GoTo PROC_ERR
        ActualizaAlumnoProf = -1
        cmd = New ADODB.Command
        With cmd
            .CommandText = "sp_ActualizaDatosAlumnoProfesor"
            .CommandType = adCmdStoredProc
            .ActiveConnection = DatEPostgrado.CnnPostgrado
            .Parameters.Append.CreateParameter("RETURN_VALUE", adInteger, adParamReturnValue, 0)
            .Parameters.Append.CreateParameter("CODIGO", adVarChar, adParamInput, 10, CODIGO)
            .Parameters.Append.CreateParameter("NOMBRE", adVarChar, adParamInput, 80, NOMBRE)
            .Parameters.Append.CreateParameter("DIRECCION", adVarChar, adParamInput, 80, DIRECCION)
            .Parameters.Append.CreateParameter("TELEFONO", adVarChar, adParamInput, 14, TELEFONO)
            .Parameters.Append.CreateParameter("EMAIL", adVarChar, adParamInput, 80, EMAIL)
            .Parameters.Append.CreateParameter("CENTROTRABAJO", adVarChar, adParamInput, 80, CENTROTRABAJO)
            .Parameters.Append.CreateParameter("PROFESION", adVarChar, adParamInput, 80, PROFESION)
            .Parameters.Append.CreateParameter("ESPECIALIDAD", adVarChar, adParamInput, 80, ESPECIALIDAD)
            .Parameters.Append.CreateParameter("SEXO", adVarChar, adParamInput, 1, Sexo)
            .Parameters.Append.CreateParameter("NUMPROMO", adTinyInt, adParamInput, , NUMPROMO)
            .Parameters.Append.CreateParameter("IND", adTinyInt, adParamInput, , ind)
            .Parameters.Append.CreateParameter("NUMPLAN", adVarChar, adParamInput, 5, NUMPLAN)
            .Execute(Options:=adExecuteNoRecords)
            ActualizaAlumnoProf = .Parameters("RETURN_VALUE") '
        End With
        cmd = Nothing
PROC_ERR:  'MsgBox err.Description
    End Function

    '** devuelve el registro con los nombres de las menciones que tienen por lo menos
    '** un alumno matriculado en un semestre determinado
    Public Function TraeMencionesSem(SEMESTRE As String, SEDE As String) As ADODB.Recordset
        On Error GoTo errores_
        I = -1
        cmd = New ADODB.Command
        With cmd
            .CommandText = "sp_TraeMencionesSemestre"
            .CommandType = adCmdStoredProc
            .ActiveConnection = DatEPostgrado.CnnPostgrado
            .Parameters.Append.CreateParameter("RETURN_VALUE", adInteger, adParamReturnValue, 0)
            .Parameters.Append.CreateParameter("SEMESTRE", adChar, adParamInput, 6, SEMESTRE)
            .Parameters.Append.CreateParameter("SEDE", adVarChar, adParamInput, 30, SEDE)
            TraeMencionesSem = .Execute
            I = .Parameters("RETURN_VALUE") '
        End With
        cmd = Nothing
errores_:
    End Function

    '** esta funcion verifica si un alumno esta inscrito en el curso, en el semestre determinado en la BD
    '** ello se realiza mediante la llamada al procedimiento, retorna 0 si no lo esta,
    '** 1 en caso contrario
    Public Function YaEstaInscritoEnCurso(CODALUMNO As String, _
    SEMESTRE As String, CODCURSO As String) As Integer
        On Error GoTo errores_
        YaEstaInscritoEnCurso = 0 'no esta inscrito
        cmd = New ADODB.Command
        With cmd
            .CommandText = "sp_EstaInscritoEnCurso"
            .CommandType = adCmdStoredProc
            .ActiveConnection = DatEPostgrado.CnnPostgrado
            .Parameters.Append.CreateParameter("RETURN_VALUE", adInteger, adParamReturnValue, 0)
            .Parameters.Append.CreateParameter("CODCURSO", adChar, adParamInput, 6, CODCURSO)
            .Parameters.Append.CreateParameter("CODALUMNO", adVarChar, adParamInput, 10, CODALUMNO)
            .Parameters.Append.CreateParameter("SEMESTRE", adChar, adParamInput, 6, SEMESTRE)
            .Execute(Options:=adExecuteNoRecords)
            YaEstaInscritoEnCurso = .Parameters("RETURN_VALUE") '1 o 0
        End With
        cmd = Nothing
errores_:
    End Function

    '***************************************************************************************************
    '** Trae el Nro de creditos del curso segun el plan de estudios del alumno
    Public Function Trae_NroCredCurso(CODALUMNO As String, CODCURSO As String) As Integer
        Trae_NroCredCurso = 3 ' valor por defecto
        Dim NUMPLAN As String ' el plan de estudios del alumno

        Ssql = "Select NUMPLAN From ALUMNO Where CODALUMNO='" & CODALUMNO & "'"
        Ejecuta_consultas(Ssql, rssql)
        If rssql.EOF Or rssql.BOF Then Exit Function
        NUMPLAN = Trim(rssql.Fields(0).Value)

        On Error Resume Next
        Trae_NroCredCurso = NCred_Ciclo_CursoPlan(NUMPLAN, CODCURSO)
    End Function

    '** esta funcion consulta que requisitos tiene un curso del plan de estudios(NUMPLAN)
    '** recoge los requisitos en el arreglo Requisitos
    Public Function TieneRequisitosCurso(CODCURSO As String, _
    NUMPLAN As String) As Integer
        intNReq = 0 ' se asume 0 requisitos
        TieneRequisitosCurso = 0 'se asume q no tiene
        ReDim Requisitos(0 To 1, 0 To 1)
        Ssql = "SELECT CODREQUISITO1, CODREQUISITO2 from PLANESTUDIOS Where CODCURSO='" & _
                CODCURSO & "' And NUMPLAN='" & NUMPLAN & "'"
        Ejecuta_consultas(Ssql, rssql)
        If rssql.EOF Or rssql.BOF Then Exit Function
        If Trim(rssql.Fields(0)) = "" Then Exit Function

        Requisitos(0, 0) = Trim(rssql.Fields(0))
        intNReq = intNReq + 1 ' un requisito

        If Trim(rssql.Fields(1)) <> "" Then Exit Function
        Requisitos(0, 1) = Trim(rssql.Fields(1)) 'dos requisitos
        intNReq = intNReq + 1
    End Function

    '** esta funcion devuelve cero si el alumno no cumple con el requisito del
    '** curso, devuelve 1 en caso contrario
    Public Function CumpleRequisitoCurso(CODCURSO As String, NUMPLAN As String, _
    CODALUMNO As String, Optional NumReqCurso As Integer = 1) As Integer
        On Error GoTo errores_
        CumpleRequisitoCurso = 0 'no cumple req
        cmd = New ADODB.Command
        With cmd
            .CommandText = "sp_CumpleRequisitoCurso"
            .CommandType = adCmdStoredProc
            .ActiveConnection = DatEPostgrado.CnnPostgrado
            .Parameters.Append.CreateParameter("RETURN_VALUE", adInteger, adParamReturnValue, 0)
            .Parameters.Append.CreateParameter("CODCURSO", adChar, adParamInput, 6, CODCURSO)
            .Parameters.Append.CreateParameter("NUMPLAN", adChar, adParamInput, 5, NUMPLAN)
            .Parameters.Append.CreateParameter("CODALUMNO", adVarChar, adParamInput, 10, CODALUMNO)
            .Parameters.Append.CreateParameter("NumReqCurso", adTinyInt, adParamInput, , NumReqCurso)
            .Execute(Options:=adExecuteNoRecords)
            CumpleRequisitoCurso = .Parameters("RETURN_VALUE") '> 0 si cumple Requisitos
        End With
        cmd = Nothing
errores_:
    End Function

    'devuelve los alumnos matriculados en un semestre
    Public Function AlumnosMatric(MENCION As String, _
    SEMESTRE As String) As ADODB.Recordset
        On Error GoTo errores_
        I = -1
        cmd = New ADODB.Command
        With cmd
            .CommandText = "sp_AlumnosMatriculados"
            .CommandType = adCmdStoredProc
            .ActiveConnection = DatEPostgrado.CnnPostgrado
            .Parameters.Append.CreateParameter("RETURN_VALUE", adInteger, adParamReturnValue, 0)
            .Parameters.Append.CreateParameter("MENCION", adChar, adParamInput, 4, MENCION)
            .Parameters.Append.CreateParameter("SEMESTRE", adChar, adParamInput, 6, SEMESTRE)
            AlumnosMatric = .Execute
            I = .Parameters("RETURN_VALUE") '
        End With
        cmd = Nothing
errores_:
    End Function

    'devuelve los alumnos matriculados en un semestre (solo su codigo y su nombre)
    Public Function AlumnosMatricEsp(MENCION As String, _
    SEMESTRE As String) As ADODB.Recordset
        On Error GoTo errores_
        I = -1
        cmd = New ADODB.Command
        With cmd
            .CommandText = "sp_AlumnosMatriculadosEsp"
            .CommandType = adCmdStoredProc
            .ActiveConnection = DatEPostgrado.CnnPostgrado
            .Parameters.Append.CreateParameter("RETURN_VALUE", adInteger, adParamReturnValue, 0)
            .Parameters.Append.CreateParameter("MENCION", adChar, adParamInput, 4, MENCION)
            .Parameters.Append.CreateParameter("SEMESTRE", adChar, adParamInput, 6, SEMESTRE)
            AlumnosMatricEsp = .Execute
            I = .Parameters("RETURN_VALUE") '
        End With
        cmd = Nothing
errores_:
    End Function

    'devuelve los alumnos postulantes en un semestre (con sus datos generales)
    Public Function AlumnosPostulantes(CodMencion As String, _
    PROMOCION As String) As ADODB.Recordset
        On Error GoTo errores_
        I = -1
        cmd = New ADODB.Command
        With cmd
            .CommandText = "sp_AlumnosPostulantesPorProm"
            .CommandType = adCmdStoredProc
            .ActiveConnection = DatEPostgrado.CnnPostgrado
            .Parameters.Append.CreateParameter("RETURN_VALUE", adInteger, adParamReturnValue, 0)
            .Parameters.Append.CreateParameter("PROMOCION", adChar, adParamInput, 6, PROMOCION)
            .Parameters.Append.CreateParameter("CODMENCION", adChar, adParamInput, 4, CodMencion)
            AlumnosPostulantes = .Execute
            I = .Parameters("RETURN_VALUE") '
        End With
        cmd = Nothing
errores_:
    End Function

    Public Function AlumnosIng(MENCION As String, _
    SEMESTRE As String) As ADODB.Recordset
        On Error GoTo errores_
        I = -1
        cmd = New ADODB.Command
        With cmd
            .CommandText = "sp_AlumnosIngresantes"
            .CommandType = adCmdStoredProc
            .ActiveConnection = DatEPostgrado.CnnPostgrado
            .Parameters.Append.CreateParameter("RETURN_VALUE", adInteger, adParamReturnValue, 0)
            .Parameters.Append.CreateParameter("MENCION", adChar, adParamInput, 4, MENCION)
            .Parameters.Append.CreateParameter("SEMESTRE", adChar, adParamInput, 6, SEMESTRE)
            AlumnosIng = .Execute
            I = .Parameters("RETURN_VALUE") '
        End With
        cmd = Nothing
errores_:
    End Function


    Public Function ExisteProgramacion(SEMESTRE As String, _
    CodMencion As String, SEDE As String, _
    Optional Ret_Cons As Integer = 0) As Integer
        'Ret_Cons =0 no retornar la consulta

        On Error GoTo PROC_ERR
        ExisteProgramacion = 0 'no existe
        cmd = New ADODB.Command
        With cmd
            .CommandText = "sp_ProgramacionMencion"
            .CommandType = adCmdStoredProc
            .ActiveConnection = DatEPostgrado.CnnPostgrado
            .Parameters.Append.CreateParameter("RETURN_VALUE", adInteger, adParamReturnValue, 0)
            .Parameters.Append.CreateParameter("CODMENCION", adChar, adParamInput, 4, CodMencion)
            .Parameters.Append.CreateParameter("SEMESTRE", adChar, adParamInput, 6, SEMESTRE)
            .Parameters.Append.CreateParameter("SEDE", adVarChar, adParamInput, 30, SEDE)
            If Ret_Cons = 0 Then
                .Execute(Options:=adExecuteNoRecords)
            Else : rssql = .Execute
            End If
            ExisteProgramacion = .Parameters("RETURN_VALUE")
        End With
        cmd = Nothing
PROC_ERR:  'MsgBox Err.Description
    End Function

    Public Function SemestresAcad(ESTADOACTUAL As String) As ADODB.Recordset
        I = -1
        On Error GoTo PROC_ERR
        cmd = New ADODB.Command
        With cmd
            .CommandText = "sp_SemestresAcad"
            .CommandType = adCmdStoredProc
            .ActiveConnection = DatEPostgrado.CnnPostgrado
            .Parameters.Append.CreateParameter("RETURN_VALUE", adInteger, adParamReturnValue, 0)
            .Parameters.Append.CreateParameter("ESTADOACTUAL", adChar, adParamInput, 2, ESTADOACTUAL)
            SemestresAcad = .Execute
            I = .Parameters("RETURN_VALUE") '
        End With
        cmd = Nothing
PROC_ERR:  'MsgBox Err.Description
    End Function

    Public Function HistorialAcad(CODALUMNO As String) As ADODB.Recordset
        I = -1
        On Error GoTo PROC_ERR
        cmd = New ADODB.Command
        With cmd
            .CommandText = "sp_HistorialAcademico"
            .CommandType = adCmdStoredProc
            .ActiveConnection = DatEPostgrado.CnnPostgrado
            .Parameters.Append.CreateParameter("RETURN_VALUE", adInteger, adParamReturnValue, 0)
            .Parameters.Append.CreateParameter("CODALUMNO", adVarChar, adParamInput, 10, CODALUMNO)
            HistorialAcad = .Execute
            I = .Parameters("RETURN_VALUE") '
        End With
        cmd = Nothing
PROC_ERR:  'MsgBox Err.Description
    End Function

    '** Trae el Historial academico del alumno ordenados por ciclo
    '** del plan de estudios, atraveees de la llamada al procedimiento almacenado
    '** la funcion devuelve un recordset
    Public Function HistorialAcadPorCicloPlan(CODALUMNO As String, NUMPLAN As String) As ADODB.Recordset
        I = -1
        On Error GoTo PROC_ERR
        cmd = New ADODB.Command
        With cmd
            .CommandText = "sp_HistorialAcademicoPorCicloPlan"
            .CommandType = adCmdStoredProc
            .ActiveConnection = DatEPostgrado.CnnPostgrado
            .Parameters.Append.CreateParameter("RETURN_VALUE", adInteger, adParamReturnValue, 0)
            .Parameters.Append.CreateParameter("CODALUMNO", adVarChar, adParamInput, 10, CODALUMNO)
            .Parameters.Append.CreateParameter("NUMPLAN", adChar, adParamInput, 5, NUMPLAN)
            HistorialAcadPorCicloPlan = .Execute
            I = .Parameters("RETURN_VALUE") '
        End With
        cmd = Nothing
PROC_ERR:  'MsgBox err.Description
    End Function

    '***************************************************************************************************
    '** pa que devuelve el promedio ponderado acumulado de un alumno hasta el semestre especificado
    Public Function PromedioSemestralAcu(CODALUMNO As String, SEMESTRE As String) As Double
        PromedioSemestralAcu = 0
        On Error GoTo PROC_ERR
        cmd = New ADODB.Command
        With cmd
            .CommandText = "sp_PromedioSemestralAcum"
            .CommandType = adCmdStoredProc
            .ActiveConnection = DatEPostgrado.CnnPostgrado
            .Parameters.Append.CreateParameter("RETURN_VALUE", adInteger, adParamReturnValue, 0)
            .Parameters.Append.CreateParameter("CODALUMNO", adVarChar, adParamInput, 10, CODALUMNO)
            .Parameters.Append.CreateParameter("SEMESTRE", adChar, adParamInput, 6, SEMESTRE)
            rssql = .Execute
            PromedioSemestralAcu = rssql.Fields(1)
        End With
        cmd = Nothing
PROC_ERR:  ' MsgBox err.Description
    End Function

    '***************************************************************************************************
    '** pa que devuelve el promedio ponderado acumulado de un alumno hasta el semestre registrado en la BD
    '** si el curso ha sido aprobado con dos notas aprobatorias se toma la de mayor nota
    Public Function PromedioSemAcuCursoMay(CODALUMNO As String) As Double
        PromedioSemAcuCursoMay = 0
        On Error GoTo PROC_ERR
        cmd = New ADODB.Command
        With cmd
            .CommandText = "sp_PromedioSemAcumCursosMay"
            .CommandType = adCmdStoredProc
            .ActiveConnection = DatEPostgrado.CnnPostgrado
            .Parameters.Append.CreateParameter("RETURN_VALUE", adInteger, adParamReturnValue, 0)
            .Parameters.Append.CreateParameter("CODALUMNO", adVarChar, adParamInput, 10, CODALUMNO)
            rssql = .Execute
            PromedioSemAcuCursoMay = rssql.Fields(1)
        End With
        cmd = Nothing
PROC_ERR:  ' MsgBox err.Description
    End Function

    '** Devuelve los cursos de un alumno que han llevado de otros planes de estudio
    Public Function Cursos_OtrosPlanes(NUMPLAN As String, _
    CODALUMNO As String) As ADODB.Recordset

        On Error GoTo PROC_ERR
        cmd = New ADODB.Command
        With cmd
            .CommandText = "sp_CursosOtroPlanEstudios"
            .CommandType = adCmdStoredProc
            .ActiveConnection = DatEPostgrado.CnnPostgrado
            .Parameters.Append.CreateParameter("RETURN_VALUE", adInteger, adParamReturnValue, 0)
            .Parameters.Append.CreateParameter("NUMPLAN", adChar, adParamInput, 5, NUMPLAN)
            .Parameters.Append.CreateParameter("CODALUMNO", adVarChar, adParamInput, 10, CODALUMNO)
            Cursos_OtrosPlanes = .Execute
            I = .Parameters("RETURN_VALUE") '
        End With
        cmd = Nothing
PROC_ERR:  'MsgBox Err.Description
    End Function

    '** devuelve los semestres validos en la cual ha existido alumnos matriculados ingresantes
    Public Sub Traersemestres(CodMencion As String, DatCSemestre As DataCombo)
        On Error GoTo errores_
        Ssql = "Select Distinct PROMOCION From ALUMNO WHERE CODMENCION='" & CodMencion & "' AND ESTADOACTUAL<>'01' ORDER BY 1"
        Ejecuta_consultas(Ssql, rssql)
        DatCSemestre.ListField = "PROMOCION"
        DatCSemestre.RowSource = rssql
errores_:  'MsgBox err.Description

    End Sub

    '** devuelve la denomincacion de un programa pe PROMAINFO segun el codigo del programa enviado
    Public Function TraeAbrevPrograma(CODPROGRAMA As String) As String
        TraeAbrevPrograma = ""
        Ssql = "Select DENOMINACION From PROGRAMAS Where CODPROGRAMA='" & CODPROGRAMA & "'"
        Ejecuta_consultas(Ssql, rssql)
        If rssql.EOF Or rssql.BOF Then Exit Function
        TraeAbrevPrograma = Trim(rssql.Fields(0))
    End Function

    '** devuelve la seccion de un programa pe PROMAINFO pertenece a la seccion CIENCIAS segun el codigo del programa enviado
    Public Function TraeSeccPrograma(CODPROGRAMA As String) As String
        TraeSeccPrograma = ""
        Ssql = "Select NOMBRESECCION From vista_ProgramaSeccion Where CODPROGRAMA='" & CODPROGRAMA & "'"
        Ejecuta_consultas(Ssql, rssql)
        If rssql.EOF Or rssql.BOF Then Exit Function
        TraeSeccPrograma = Trim(rssql.Fields(0))
    End Function

    '** devuelve el grado academico de magister de la mencion
    Public Function TraeGradoAcadMag(CodMencion As String) As String
        TraeGradoAcadMag = ""
        Ssql = "Select GRADO From MENCION Where CODMENCION='" & CodMencion & "'"
        Ejecuta_consultas(Ssql, rssql)
        If rssql.EOF Or rssql.BOF Then Exit Function
        TraeGradoAcadMag = Trim(rssql.Fields(0))
    End Function

    Public Function TraeProfAlumno(CODALUMNO As String) As String
        TraeProfAlumno = ""
        Ssql = "Select PROFESION From ALUMNO Where CODALUMNO='" & CODALUMNO & "'"
        Ejecuta_consultas(Ssql, rssql)
        If rssql.EOF Or rssql.BOF Then Exit Function
        TraeProfAlumno = Trim(rssql.Fields(0))
    End Function

    Public Function TraeTemaCalificaTesis(CODALUMNO As String) As ADODB.Recordset
        I = -1
        Ssql = "Select TITULOANTEP, CALIFICATIVO, PRESIDENTE, SECRETARIO, VOCAL " & _
        "From TESIS Where CODALUMNO='" & CODALUMNO & "'"
        Ejecuta_consultas(Ssql, rssql)
        If rssql.EOF Or rssql.BOF Then Exit Function
        I = 1
    End Function

    '** esta funcion tiene por finalidad traer una lista de cursos del programa
    '** seleccionado segun la letra escrita en el cuadro de texto codcurso
    '** o trae como segunda opcion todos los cursos del programa
    Public Function Trae_CursosPrograma(CODPROGRAMA As String, _
    LetraCodigo As String, Optional ind As Integer = 0) As ADODB.Recordset

        On Error GoTo errores_
        cmd = New ADODB.Command
        With cmd
            .CommandText = "sp_CursosPorPrograma"
            .CommandType = adCmdStoredProc
            .ActiveConnection = DatEPostgrado.CnnPostgrado
            .Parameters.Append.CreateParameter("RETURN_VALUE", adInteger, adParamReturnValue, 0)
            .Parameters.Append.CreateParameter("CODPROGRAMA ", adChar, adParamInput, 5, CODPROGRAMA)
            .Parameters.Append.CreateParameter("LetraCodigo", adVarChar, adParamInput, 10, LetraCodigo)
            .Parameters.Append.CreateParameter("Ind", adTinyInt, adParamInput, , ind)
            Trae_CursosPrograma = .Execute
            I = .Parameters("RETURN_VALUE")
        End With
        cmd = Nothing
errores_:  'MsgBox err.Description
    End Function

    '** esta funcion tiene por finalidad traer la informacion de un curso determinado
    '** por programa y curso (ind =1 )/ por codigo (3)
    Public Function BUSCACURSO(cod As String, CODPROG As String, SEMES As String, SEDE As String, ind As Integer) As ADODB.Recordset
        'DatEPostgrado.CnnPostgrado.BeginTrans
        BUSCACURSO = DatEPostgrado.CnnPostgrado.Execute("exec SPBUSCACURSO '" & cod & "','" & CODPROG & "','" & SEMES & "','" & SEDE & "'," & ind & "")
        'MsgBox err.Description
    End Function

    '** esta funcion tiene por finalidad traer el numero de creditos de un determinado plan de
    '** estudios (NUMPLAN)
    Public Function TotalCreditosPlan(NUMPLAN As String) As Integer
        TotalCreditosPlan = 0
        On Error GoTo errores_
        Ssql = "SELECT SUM(numcreditos) From vista_CursoPlan WHERE numplan ='" & NUMPLAN & "'"
        Ejecuta_consultas(Ssql, rssql)
        TotalCreditosPlan = rssql.Fields(0).Value
errores_:
    End Function

    '** esta funcion tiene por finalidad traer el número de creditos obligatorios o electivos
    '** de un determinado plan de estudios (NUMPLAN).
    Public Function NCreditosPlanSegunOE(NUMPLAN As String, CONDCURSO As String) As Integer
        NCreditosPlanSegunOE = 0
        On Error GoTo errores_
        cmd = New ADODB.Command
        With cmd
            .CommandText = "sp_NCreditosPlanSegunOE"
            .CommandType = adCmdStoredProc
            .ActiveConnection = DatEPostgrado.CnnPostgrado
            .Parameters.Append.CreateParameter("RETURN_VALUE", adInteger, adParamReturnValue, 0)
            .Parameters.Append.CreateParameter("NUMPLAN ", adChar, adParamInput, 5, NUMPLAN)
            .Parameters.Append.CreateParameter("CONDCURSO", adVarChar, adParamInput, 2, CONDCURSO)
            .Execute(Options:=adExecuteNoRecords)
            NCreditosPlanSegunOE = .Parameters("RETURN_VALUE")
        End With
        cmd = Nothing
errores_:  'MsgBox Err.Description
    End Function

    '** esta funcion tiene por finalidad verificar si un curso esta en la tabla inscripcioncursos
    '** antes de proceder a eliminarlo
    Public Function ExisteCursoInsCursos(CODCURSO As String) As Integer
        ExisteCursoInsCursos = 0 'se asume que no existe
        Ssql = "SELECT CODCURSO From INSCRIPCIONCURSOS WHERE CODCURSO ='" & CODCURSO & "'"
        Ejecuta_consultas(Ssql, rssql)
        If rssql.EOF Or rssql.BOF Then Exit Function
        ExisteCursoInsCursos = 1 'existe
    End Function

    '** esta funcion devuelve un conjunto de registros que representan a los alumnos
    '** de una determinada promocion, codmencion y sede.
    '** los alumnos so aquellos que no tienen registrados matricula- y aquellos que tienen
    '** por los menos una matricula.
    Public Function TraerAlumnosPorProm(CodMencion As String, SEDE As String, _
    PROMOCION As String, ind As Integer) As ADODB.Recordset
        On Error GoTo errores_
        cmd = New ADODB.Command
        With cmd
            .CommandText = "sp_AlumnosPorPromocion"
            .CommandType = adCmdStoredProc
            .ActiveConnection = DatEPostgrado.CnnPostgrado
            .Parameters.Append.CreateParameter("RETURN_VALUE", adInteger, adParamReturnValue, 0)
            .Parameters.Append.CreateParameter("CODMENCION ", adChar, adParamInput, 4, CodMencion)
            .Parameters.Append.CreateParameter("SEDE", adVarChar, adParamInput, 10, SEDE)
            .Parameters.Append.CreateParameter("PROMOCION", adChar, adParamInput, 6, PROMOCION)
            .Parameters.Append.CreateParameter("Ind ", adTinyInt, adParamInput, , ind)
            TraerAlumnosPorProm = .Execute
            I = .Parameters("RETURN_VALUE")
        End With
        cmd = Nothing
errores_:  'MsgBox Err.Description
    End Function

    '** VERIFICA SI EL ALUMNO ESTA inscrito en algun curso
    Public Function ExisteAlumnoInsCursos(CODALU As String) As Integer
        ExisteAlumnoInsCursos = 0 ' no Existe
        Ssql = "Select CODALUMNO From INSCRIPCIONCURSOS Where CODALUMNO='" & CODALU & "'"
        Ejecuta_consultas(Ssql, rssql)
        If rssql.EOF Or rssql.BOF Then Exit Function
        ExisteAlumnoInsCursos = 1 '  Existe
    End Function

    '** VERIFICA SI EL ALUMNO ESTA inscrito en algun curso, en un semestre determinado
    Public Function EstaAlumnoInsCursoSem(CODALU As String, SEMESTRE As String) As Integer
        EstaAlumnoInsCursoSem = 0 ' no esta inscrito
        Ssql = "Select CODALUMNO From INSCRIPCIONCURSOS Where CODALUMNO='" & CODALU & "'" & _
                " AND SEMESTRE='" & SEMESTRE & "'"
        Ejecuta_consultas(Ssql, rssql)
        If rssql.EOF Or rssql.BOF Then Exit Function
        EstaAlumnoInsCursoSem = 1 '  esta inscrito
    End Function

    '** elimina la matricula normal de un alumno en un año determinado
    Public Function EliminaMatricula(CODALUMNO As String, SEMESTRE As String, _
    ESTADO As String) As Integer
        On Error GoTo PROC_ERR
        EliminaMatricula = -1
        cmd = New ADODB.Command
        With cmd
            .CommandText = "sp_EliminaMatricula"
            .CommandType = adCmdStoredProc
            .ActiveConnection = DatEPostgrado.CnnPostgrado
            .Parameters.Append.CreateParameter("RETURN_VALUE", adInteger, adParamReturnValue, 0)
            .Parameters.Append.CreateParameter("CODALUMNO", adVarChar, adParamInput, 10, CODALUMNO)
            .Parameters.Append.CreateParameter("SEMES", adChar, adParamInput, 6, SEMESTRE)
            .Parameters.Append.CreateParameter("ESTADO", adChar, adParamInput, 2, ESTADO)
            .Execute(Options:=adExecuteNoRecords)
            EliminaMatricula = .Parameters("RETURN_VALUE") '0 ok
        End With
        cmd = Nothing
PROC_ERR:  'MsgBox Err.Description
    End Function

    '** elimina la matricula normal de un alumno en un año determinado
    Public Function EliminaMatriculaVerano(CODALUMNO As String, ESTADO As String, SEMESTRE As String) As Integer
        On Error GoTo PROC_ERR
        EliminaMatriculaVerano = -1
        cmd = New ADODB.Command
        With cmd
            .CommandText = "sp_EliminaMatriculaVerano"
            .CommandType = adCmdStoredProc
            .ActiveConnection = DatEPostgrado.CnnPostgrado
            .Parameters.Append.CreateParameter("RETURN_VALUE", adInteger, adParamReturnValue, 0)
            .Parameters.Append.CreateParameter("CODALUMNO", adVarChar, adParamInput, 10, CODALUMNO)
            .Parameters.Append.CreateParameter("ESTADO", adChar, adParamInput, 2, ESTADO)
            .Parameters.Append.CreateParameter("SEMES", adChar, adParamInput, 6, SEMESTRE)
            .Execute(Options:=adExecuteNoRecords)
            EliminaMatriculaVerano = .Parameters("RETURN_VALUE") '0 ok
        End With
        cmd = Nothing
PROC_ERR:  'MsgBox Err.Description
    End Function

    '** verifica la existencia de un codigo del alumno (evitar repetir codigo)
    Public Function ExisteCodigo(CODALU As String) As Boolean
        ExisteCodigo = False ' no Existe
        Ssql = "Select CODALUMNO From INSCRIPCIONALUMNOS Where CODALUMNO='" & CODALU & "'"
        Ejecuta_consultas(Ssql, rssql)
        If rssql.EOF Or rssql.BOF Then Exit Function
        ExisteCodigo = True '  Existe
    End Function

    '** Trae los datos del Maximo plan de una mencion, devuelve un registro
    Public Function TraerMaxPlanMencion(CodMencion As String) As Recordset

        On Error GoTo errores_
        cmd = New ADODB.Command
        With cmd
            .CommandText = "sp_MaxPlanEstudiosMencion"
            .CommandType = adCmdStoredProc
            .ActiveConnection = DatEPostgrado.CnnPostgrado
            .Parameters.Append.CreateParameter("CODMENCION", adChar, adParamInput, 4, CodMencion)
            TraerMaxPlanMencion = .Execute
        End With
        cmd = Nothing
errores_:
    End Function

    '** Trae el estado de un alumno, "01", "02" ,"03" , ..
    Public Function TraeEstadoAlumno(CODALUMNO As String) As String
        TraeEstadoAlumno = ""
        Ssql = "Select ESTADOACTUAL from ALUMNO Where CODALUMNO ='" & CODALUMNO & "'"
        Ejecuta_consultas(Ssql, rssql)
        If rssql.EOF Or rssql.BOF Then Exit Function
        TraeEstadoAlumno = Trim(rssql.Fields(0))
    End Function

    '** Actualiza el campo CICLO de la tabla inscripcionalumnos, despues de q el alumno
    '** ha realizado su matricula, el ciclo verano es como un ciclo separado
    Public Sub ActualizarCiclo(CODALUMNO As String)
        On Error GoTo errores_
        Dim rs As ADODB.Recordset
        Dim sem As String, CIC As Integer, Ret As Integer
        CIC = 1
        rs = New ADODB.Recordset

        On Error Resume Next

        Ssql = "SELECT CODALUMNO, SEMESTRE From INSCRIPCIONALUMNOS " & _
                "WHERE CODALUMNO ='" & CODALUMNO & "' Order By 2"
        Ejecuta_consultas(Ssql, rs)
        While Not rs.EOF
            sem = Trim(rs.Fields(1)) 'semestre
            cmd = New ADODB.Command
            With cmd
                .CommandText = "sp_ActualizarCiclo"
                .CommandType = adCmdStoredProc
                .ActiveConnection = DatEPostgrado.CnnPostgrado
                .Parameters.Append.CreateParameter("RETURN_VALUE", adInteger, adParamReturnValue, 0)
                .Parameters.Append.CreateParameter("CODALUMNO", adVarChar, adParamInput, 10, CODALUMNO)
                .Parameters.Append.CreateParameter("SEMESTRE", adChar, adParamInput, 6, sem)
                .Parameters.Append.CreateParameter("CICLO", adChar, adParamInput, 1, CIC)
                .Execute(Options:=adExecuteNoRecords)
                Ret = .Parameters("RETURN_VALUE") '> 0 OOk
            End With
            cmd = Nothing
            CIC = CIC + 1
            rs.MoveNext()
        End While
        rs.Close()
errores_:
        rs = Nothing
    End Sub

    '** Devuelve a los alumnos que han sido evaluados en un semestre determinado, y q
    '** pertenecen a una determinada mencion, los alumnos estan ordenados por su puntaje
    '** smestral  en forma descendente, en i se devuelve el numero de alumnos
    Public Function OrdenMeritoSem(SEMESTRE As String, _
    CodMencion As String, PROMOCION As String) As ADODB.Recordset
        I = -1
        On Error GoTo PROC_ERR
        cmd = New ADODB.Command
        With cmd
            .CommandText = "sp_OrdenMeritoSemestral"
            .CommandType = adCmdStoredProc
            .ActiveConnection = DatEPostgrado.CnnPostgrado
            .Parameters.Append.CreateParameter("RETURN_VALUE", adInteger, adParamReturnValue, 0)
            .Parameters.Append.CreateParameter("SEMESTRE", adChar, adParamInput, 6, SEMESTRE)
            .Parameters.Append.CreateParameter("CODMENCION", adChar, adParamInput, 4, CodMencion)
            .Parameters.Append.CreateParameter("PROMOCION", adChar, adParamInput, 6, PROMOCION)
            OrdenMeritoSem = .Execute
            I = .Parameters("RETURN_VALUE") '
        End With
        cmd = Nothing
PROC_ERR:  'MsgBox Err.Description
    End Function

    '** devuelve el Nro de cursos de un plan de estudios (NUMPLAN) de una mencion,
    '** de un ciclo determinado
    Public Function NumCursosLlevados(CODALUMNO As String, NUMPLAN As String) As Integer
        NumCursosCiclo = 0
        On Error GoTo PROC_ERR
        cmd = New ADODB.Command
        With cmd
            .CommandText = "sp_NroCursosLlevados"
            .CommandType = adCmdStoredProc
            .ActiveConnection = DatEPostgrado.CnnPostgrado
            .Parameters.Append.CreateParameter("RETURN_VALUE", adInteger, adParamReturnValue, 0)
            .Parameters.Append.CreateParameter("CODALUMNO", adVarChar, adParamInput, 10, CODALUMNO)
            .Parameters.Append.CreateParameter("NUMPLAN", adChar, adParamInput, 5, NUMPLAN)
            .Execute() ' los resultados no se asignan a ningun recordset
            NumCursosLlevados = .Parameters("RETURN_VALUE") '
        End With
        cmd = Nothing
PROC_ERR:  'MsgBox Err.Description
    End Function


    Public Function NumCursosPlanEstudios(NUMPLAN As String) As Integer
        On Error GoTo errores_
        Dim rs As ADODB.Recordset
        rs = New ADODB.Recordset
        NumCursosPlanEstudios = 0
        Ssql = "SELECT CODCURSO FROM PLANESTUDIOS WHERE NUMPLAN ='" & NUMPLAN & "'"
        Ejecuta_consultas(Ssql, rssql)
        NumCursosPlanEstudios = rssql.RecordCount
        rs.Close()
errores_:
        rs = Nothing
    End Function

    '** esta funcion devuelve el puntaje acumulado por el alumno, hasta un semestre SEMESTRE incluso
    '** ello se realiza mediante la llamada al procedimiento
    Public Function PuntajeAcumuladoSem(CODALUMNO As String, _
    SEMESTRE As String) As Integer
        Dim rs As ADODB.Recordset
        rs = New ADODB.Recordset

        PuntajeAcumuladoSem = 0 'se asume q no tiene puntaje
        On Error GoTo errores_

        cmd = New ADODB.Command
        With cmd
            .CommandText = "sp_PuntajeAcumuladoSem"
            .CommandType = adCmdStoredProc
            .ActiveConnection = DatEPostgrado.CnnPostgrado
            .Parameters.Append.CreateParameter("CODALUMNO", adVarChar, adParamInput, 10, CODALUMNO)
            .Parameters.Append.CreateParameter("SEMESTRE", adChar, adParamInput, 6, SEMESTRE)
            rs = .Execute
            PuntajeAcumuladoSem = rs.Fields(1).Value
        End With
        cmd = Nothing
        rs.Close()
errores_:  ' MsgBox err.Description
        rs = Nothing
    End Function


    '** esta funcion trae el orden de merito de un alumno, en un semestre determinado
    '** de evaluacion, con respecto a su promocion -mencion
    Public Function Coloca_OrdenMerito(CODALUMNO As String, SEMESTRE As String, _
    CodMencion As String, AnyoINGRESO As String) As String
        Dim MERITO As Integer
        MERITO = 1
        Coloca_OrdenMerito = ""
        On Error Resume Next
        rssql = OrdenMeritoSem(SEMESTRE, CodMencion, AnyoINGRESO)
        While Not rssql.EOF
            If rssql.Fields("CODALUMNO") = CODALUMNO Then
                Coloca_OrdenMerito = Format(CStr(MERITO), "00")
                Exit Function
            End If
            MERITO = MERITO + 1
            rssql.MoveNext()
        End While

    End Function


    Public Sub Mostrar_SemPlan(Txt As Object, NUMPLAN As String)
        On Error GoTo errores_
        Dim rs As ADODB.Recordset
        Ssql = "Select SEMESTREPLAN From PLANDE where NUMPLAN='" & NUMPLAN & "'"
        Ejecuta_consultas(Ssql, rs)
        If rs.EOF Or rs.BOF Then Exit Sub
        Txt = rs.Fields(0).Value
        rs.Close()
errores_:
        rs = Nothing
    End Sub

    '** esta funcion devuelve un conjunto de registros que representan a los notas del
    '** alumno, para un certificado de estudios, solo aprobados, devueve un recordset
    Public Function CursosCertificado(CODALUMNO As String, NUMPLAN As String, NOTA As Integer) As ADODB.Recordset
        On Error GoTo errores_
        cmd = New ADODB.Command
        With cmd
            .CommandText = "sp_HistorialAcadPorCicloPlanAprob"
            .CommandType = adCmdStoredProc
            .ActiveConnection = DatEPostgrado.CnnPostgrado
            .Parameters.Append.CreateParameter("CODALUMNO", adVarChar, adParamInput, 10, CODALUMNO)
            .Parameters.Append.CreateParameter("NUMPLAN", adChar, adParamInput, 5, NUMPLAN)
            .Parameters.Append.CreateParameter("NOTA", adTinyInt, adParamInput, , NOTA)
            CursosCertificado = .Execute
        End With
        cmd = Nothing
errores_:  'MsgBox Err.Description
    End Function

    '** esta funcion devuelve los cursos llevados por un alumno en la EPG y los cursos
    '** convalidados de un alumnos, para ello se llama al proc. almac. sp_CursosConvalidadosH
    Public Function CursosLlev_Conv(CODALUMNO As String) As ADODB.Recordset
        On Error GoTo errores_
        cmd = New ADODB.Command
        With cmd
            .CommandText = "sp_CursosConvalidadosH"
            .CommandType = adCmdStoredProc
            .ActiveConnection = DatEPostgrado.CnnPostgrado
            .Parameters.Append.CreateParameter("CODALUMNO", adVarChar, adParamInput, 10, CODALUMNO)
            CursosLlev_Conv = .Execute
        End With
        cmd = Nothing
errores_:  'MsgBox Err.Description
    End Function

    '**************************************************************************
    '** trae el maximo semestre q ha sido evaluado de un alumno
    Public Function UltimoSemestreEvaluado(CODALUMNO As String) As String
        UltimoSemestreEvaluado = ""
        Ssql = "SELECT MAX(semestre) From NOTAS WHERE CODALUMNO ='" & CODALUMNO & "'"
        Ejecuta_consultas(Ssql, rssql)
        If rssql.EOF Or rssql.BOF Then Exit Function
        UltimoSemestreEvaluado = rssql.Fields(0).Value

    End Function

    '** funcion qu tiene por fin devolver a los alumnos de una determinada mencion y
    '** promocion que han estudiado un semestre de_
    '** terminado, junto con su codigo, el Nro de creditos, el Nro de cursos y su PPs.
    '** devuelve un recordset
    Public Function AlumnosParaMeritoSem(SEMESTRE As String, CodMencion As String, _
    PROMOCION As String, Optional SEDE As String = "PIURA") As ADODB.Recordset
        On Error GoTo errores_
        cmd = New ADODB.Command
        With cmd
            .CommandText = "sp_AlumnosParaMeritoSem"
            .CommandType = adCmdStoredProc
            .ActiveConnection = DatEPostgrado.CnnPostgrado
            .Parameters.Append.CreateParameter("SEMESTRE", adChar, adParamInput, 6, SEMESTRE)
            .Parameters.Append.CreateParameter("CODMENCION", adChar, adParamInput, 4, CodMencion)
            .Parameters.Append.CreateParameter("PROMOCION", adChar, adParamInput, 6, PROMOCION)
            .Parameters.Append.CreateParameter("SEDE", adVarChar, adParamInput, 20, SEDE)
            AlumnosParaMeritoSem = .Execute
        End With
        cmd = Nothing
errores_:  'MsgBox Err.Description
    End Function

    '** funcion qu tiene por fin devolver los codigos de los alumnos de una determinada mencion y
    '** promocion que han terminado SU plan de estudios, egresados insertados anteriomente en la tabla merito
    '** el indicador flag =1 indica que solo se devuelve el codigo de los alumnos y sus meritos
    '** flag =2 indica que se devuelve el codigo y nombre de los alumnos y sus meritos
    Public Function AlumnosParaMeritoAcu(PROMOCION As String, CodMencion As String, _
    Optional flag As Integer = 1) As ADODB.Recordset
        On Error GoTo errores_
        cmd = New ADODB.Command
        Select Case flag
            Case 1
                With cmd
                    .CommandText = "sp_AlumnosParaMeritoAcum"
                    .CommandType = adCmdStoredProc
                    .ActiveConnection = DatEPostgrado.CnnPostgrado
                    .Parameters.Append.CreateParameter("PROMOCION", adChar, adParamInput, 6, PROMOCION)
                    .Parameters.Append.CreateParameter("CODMENCION", adChar, adParamInput, 4, CodMencion)
                    AlumnosParaMeritoAcu = .Execute
                End With
            Case 2
                With cmd
                    .CommandText = "sp_Alumnos_MeritoAcum"
                    .CommandType = adCmdStoredProc
                    .ActiveConnection = DatEPostgrado.CnnPostgrado
                    .Parameters.Append.CreateParameter("PROMOCION", adChar, adParamInput, 6, PROMOCION)
                    .Parameters.Append.CreateParameter("CODMENCION", adChar, adParamInput, 4, CodMencion)
                    AlumnosParaMeritoAcu = .Execute
                End With
        End Select

        cmd = Nothing
errores_:  'MsgBox err.Description
    End Function

    '** funcion qu tiene por fin elimianr de la tabla merito los alumnos, para crear una nueva consulta de
    '** ordenes de merito
    Public Function EliminaMeritoAcu(PROMOCION As String, CodMencion As String) As Integer
        On Error GoTo errores_
        EliminaMeritoAcu = -1
        cmd = New ADODB.Command
        With cmd
            .CommandText = "sp_ElimnaMeritoParaMeritoAcum"
            .CommandType = adCmdStoredProc
            .ActiveConnection = DatEPostgrado.CnnPostgrado
            .Parameters.Append.CreateParameter("PROMOCION", adChar, adParamInput, 6, PROMOCION)
            .Parameters.Append.CreateParameter("CODMENCION", adChar, adParamInput, 4, CodMencion)
            .Execute(Options:=adExecuteNoRecords)
        End With
        cmd = Nothing
        EliminaMeritoAcu = 1
errores_:  'MsgBox Err.Description
    End Function

    '** funcion qu tiene por fin devolver a los alumnos Q pertenecen a la misma promocion
    '** junto con su codigo, ppa,CON la finalidad de obtener el Orden de Merito
    '** flag es una variable que indica el tipo de consulta, si es =1 se devuelve a los
    '** alumnos que cumplen con el nro de creditos de su plan, =0 aquellos alumnos que deben creditos
    Public Function AlumnosMeritoAcumulado(CodMencion As String, _
    PROMOCION As String, CREDPLAN As Integer, NOTA As Double, Optional SEDE As String = "PIURA") As ADODB.Recordset
        On Error GoTo errores_
        cmd = New ADODB.Command
        With cmd
            .CommandText = "sp_AlumnosMeritoAcumul"
            .CommandType = adCmdStoredProc
            .ActiveConnection = DatEPostgrado.CnnPostgrado
            .Parameters.Append.CreateParameter("CODMENCION", adChar, adParamInput, 4, CodMencion)
            .Parameters.Append.CreateParameter("PROMOCION", adChar, adParamInput, 6, PROMOCION)
            .Parameters.Append.CreateParameter("SEDE", adChar, adParamInput, 15, SEDE)
            .Parameters.Append.CreateParameter("CREDPLAN", adInteger, adParamInput, , CREDPLAN)
            .Parameters.Append.CreateParameter("NOTA", adDouble, adParamInput, , NOTA)
            AlumnosMeritoAcumulado = .Execute
        End With
        cmd = Nothing
errores_:  'MsgBox err.Description
    End Function

    '** devuelve el orden de merito de un alumno en un semestre determinado
    '** en la tabla temporal MERITO. con los alumnos que el usuario  selecciona
    Public Function OrdenMerito(SEMESTRE As String, CODALUMNO As String) As String
        OrdenMerito = ""
        On Error Resume Next
        Ssql = "Select MERITO from MERITO WHERE CODALUMNO ='" & CODALUMNO & _
                "' AND SEMESTRE ='" & SEMESTRE & "'"
        Ejecuta_consultas(Ssql, rssql)
        OrdenMerito = Format(rssql.Fields(0).Value, "00")

    End Function


    '** muestra los semestres validos para elegir el Orden de merito semestral/Acumulado
    '** los semestres validos se considera dos años pe promocion:1997-1 ->1998-2
    '** si los alumnos no aparecen en la lista han perdido su orden de merito, han
    '** han estudiados mas semestres del normal, no se considera semestre verano.
    Public Function Mostrar_SemValidosOrdenMerito(CodMencion As String, PROMOCION As String, _
    SEMVALIDO As String) As ADODB.Recordset
        On Error GoTo PROC_ERR
        cmd = New ADODB.Command
        With cmd
            .CommandText = "sp_SemestresOrdenMeritoSem"
            .CommandType = adCmdStoredProc
            .ActiveConnection = DatEPostgrado.CnnPostgrado
            .Parameters.Append.CreateParameter("CODMENCION", adChar, adParamInput, 4, CodMencion)
            .Parameters.Append.CreateParameter("PROMOCION", adChar, adParamInput, 6, PROMOCION)
            .Parameters.Append.CreateParameter("SEMVALIDO", adChar, adParamInput, 6, SEMVALIDO)     '
            Mostrar_SemValidosOrdenMerito = .Execute
        End With
        cmd = Nothing
PROC_ERR:  'MsgBox Err.Description
    End Function

    '** devuelve el semestre maximo y minimo en la cual han sido evaluados los alumno de una determinada
    '** promocion y  mencion
    Public Function SemestreMaximoMinEvalProm(CodMencion As String, PROMOCION As String) As ADODB.Recordset
        On Error GoTo PROC_ERR
        cmd = New ADODB.Command
        With cmd
            .CommandText = "sp_SemestreMaximoMinEvalProm"
            .CommandType = adCmdStoredProc
            .ActiveConnection = DatEPostgrado.CnnPostgrado
            .Parameters.Append.CreateParameter("CODMENCION", adChar, adParamInput, 4, CodMencion)
            .Parameters.Append.CreateParameter("PROMOCION", adChar, adParamInput, 6, PROMOCION)     '
            SemestreMaximoMinEvalProm = .Execute
        End With
        cmd = Nothing
PROC_ERR:  'MsgBox err.Description
    End Function

    '** despues de grabar a los alumnos del semestre, se procede a actualizar su
    '** orden de merito
    Public Sub ActualizarOrdenMerito(SEMESTRE As String)
        Dim MERITO As Integer, NOTA As Double   'Nota es la nota del alumno anterior

        MERITO = 1
        On Error GoTo errores_

        cmd = New ADODB.Command
        With cmd
            .CommandText = "sp_OrdenMeritoSem"
            .CommandType = adCmdStoredProc
            .ActiveConnection = DatEPostgrado.CnnPostgrado
            .Parameters.Append.CreateParameter("RETURN_VALUE", adInteger, adParamReturnValue, 0)
            .Parameters.Append.CreateParameter("SEMESTRE", adChar, adParamInput, 6, SEMESTRE)
            rssql = .Execute
        End With
        cmd = Nothing
        On Error Resume Next
        While Not rssql.EOF
            NOTA = rssql.Fields("PPS")
            While (NOTA = rssql.Fields("PPS")) And (Not rssql.EOF)
                Ssql = "UPDATE MERITO SET MERITO=" & MERITO & " WHERE CODALUMNO='" & _
                        Trim(rssql.Fields(0).Value) & "' and SEMESTRE='" & SEMESTRE & "'"
                Ejecuta_comandos(Ssql)
                rssql.MoveNext()
                If rssql.EOF Then Exit Sub
            End While
            MERITO = MERITO + 1
        End While

errores_:
    End Sub

    '************************************************************************************************
    '************************** Estadisticas *************************************************
    '** funcion qu tiene por fin devolver las estadisticas de los alumnos Ingresantes por
    '** programa
    Public Function EstadisticasPorProg(CODPROGRAMA As String) As ADODB.Recordset
        On Error GoTo errores_
        cmd = New ADODB.Command
        With cmd
            .CommandText = "sp_EstadistPorProg"
            .CommandType = adCmdStoredProc
            .ActiveConnection = DatEPostgrado.CnnPostgrado
            .Parameters.Append.CreateParameter("CODPROGRAMA", adChar, adParamInput, 5, CODPROGRAMA)
            EstadisticasPorProg = .Execute
        End With
        cmd = Nothing
errores_:  'MsgBox err.Description
    End Function

    '** funcion qu tiene por fin devolver las estadisticas de los alumnos Matriculados por
    '** programa
    Public Function EstadisticasPorProgMat(CODPROGRAMA As String) As ADODB.Recordset
        On Error GoTo errores_
        cmd = New ADODB.Command
        With cmd
            .CommandText = "sp_EstPorProgMatriculados"
            .CommandType = adCmdStoredProc
            .ActiveConnection = DatEPostgrado.CnnPostgrado
            .Parameters.Append.CreateParameter("CODPROGRAMA", adChar, adParamInput, 5, CODPROGRAMA)
            EstadisticasPorProgMat = .Execute
        End With
        cmd = Nothing
errores_:  'MsgBox err.Description
    End Function

    '** funcion qu tiene por fin devolver las estadisticas de los alumnos postulantes por
    '** programa
    Public Function EstadisticasPorProgPost(CODPROGRAMA As String) As ADODB.Recordset
        On Error GoTo errores_
        cmd = New ADODB.Command
        With cmd
            .CommandText = "sp_EstadistPorProgPost"
            .CommandType = adCmdStoredProc
            .ActiveConnection = DatEPostgrado.CnnPostgrado
            .Parameters.Append.CreateParameter("CODPROGRAMA", adChar, adParamInput, 5, CODPROGRAMA)
            EstadisticasPorProgPost = .Execute
        End With
        cmd = Nothing
errores_:  'MsgBox err.Description
    End Function

    '** funcion qu tiene por fin devolver las estadisticas acerca del nro de trabajos de tesis
    '** registrados . el parametro Tipo indica:=1 la consulta se realiza de un año determinado
    '** por programa, =0 la consulta se realiza de todos los años registrados por año y por programa.
    Public Function EstadisticasTrabTesis(TIPO As Integer, _
    ANYO As Integer) As ADODB.Recordset
        On Error GoTo errores_
        cmd = New ADODB.Command
        With cmd
            .CommandText = "sp_NroTrabajosTesis"
            .CommandType = adCmdStoredProc
            .ActiveConnection = DatEPostgrado.CnnPostgrado
            .Parameters.Append.CreateParameter("Tipo", adTinyInt, adParamInput, , TIPO)
            .Parameters.Append.CreateParameter("Anyo", adInteger, adParamInput, , ANYO)
            EstadisticasTrabTesis = .Execute
        End With
        cmd = Nothing
errores_:  'MsgBox err.Description
    End Function


    '************************************************************************************************
    'tiene por finalidad devolver la especialidades -profesiones - centros de trabajo
    ' de los alumno de la base de datos q coinciden con una letra
    Public Function RubroDatosAlu(Letra As String, _
    ind As Integer) As ADODB.Recordset
        I = -1
        On Error GoTo PROC_ERR
        cmd = New ADODB.Command
        With cmd
            .CommandText = "sp_RubroDatos"
            .CommandType = adCmdStoredProc
            .ActiveConnection = DatEPostgrado.CnnPostgrado
            .Parameters.Append.CreateParameter("Letra", adVarChar, adParamInput, 15, Letra)
            .Parameters.Append.CreateParameter("Ind", adTinyInt, adParamInput, , ind)
            RubroDatosAlu = .Execute
        End With
        cmd = Nothing
PROC_ERR:  'MsgBox Err.Description
    End Function

    'tiene por finalidad validar a los usuario del sistema en la base de datos
    Public Function LeeUser(PASSWORD As String, USUARIO As String, _
    nivel As String) As Integer
        LeeUser = -1
        On Error Resume Next

        On Error GoTo PROC_ERR
        cmd = New ADODB.Command
        With cmd
            .CommandText = "sp_LeeUser"
            .CommandType = adCmdStoredProc
            .ActiveConnection = DatEPostgrado.CnnPostgrado
            .Parameters.Append.CreateParameter("RETURN_VALUE", adInteger, adParamReturnValue, 0)
            .Parameters.Append.CreateParameter("PASSWORD", adVarChar, adParamInput, 60, PASSWORD)
            .Parameters.Append.CreateParameter("USUARIO", adVarChar, adParamInput, 20, USUARIO)
            .Parameters.Append.CreateParameter("NIVEL", adVarChar, adParamInput, 15, nivel)
            LeeUser = .Parameters("RETURN_VALUE") '
            rssql = .Execute
            If rssql.EOF Or rssql.BOF Then Exit Function
            LeeUser = 1 'usuario validado
        End With
        cmd = Nothing
        rs.Close()
        rs = Nothing
PROC_ERR:  'MsgBox err.Description
    End Function

    '** tiene por finalidad actualizar solo la clave de un usuario del sistema en
    '** la base de datos
    Public Function ActualizaAnyo(DenAnyo As String) As Integer
        ActualizaAnyo = -1 ' no existen user
        On Error Resume Next
        cmd = New ADODB.Command
        With cmd
            .CommandText = "sp_ActualizaParametroAnyo"
            .CommandType = adCmdStoredProc
            .ActiveConnection = DatEPostgrado.CnnPostgrado
            .Parameters.Append.CreateParameter("RETURN_VALUE", adInteger, adParamReturnValue, 0)
            .Parameters.Append.CreateParameter("DenAnyo", adVarChar, adParamInput, 250, DenAnyo)
            .Execute(Options:=adExecuteNoRecords)
            ActualizaAnyo = .Parameters("RETURN_VALUE") '
        End With
        cmd = Nothing
PROC_ERR:  'MsgBox err.Description
    End Function

    '** tiene por finalidad actualizar solo la clave de un usuario del sistema en
    '** la base de datos
    Public Function Update_User(PASSWORD As String, USUARIO As String) As Integer
        Update_User = -1
        On Error Resume Next

        On Error GoTo PROC_ERR
        cmd = New ADODB.Command
        With cmd
            .CommandText = "sp_UpdUser"
            .CommandType = adCmdStoredProc
            .ActiveConnection = DatEPostgrado.CnnPostgrado
            .Parameters.Append.CreateParameter("RETURN_VALUE", adInteger, adParamReturnValue, 0)
            .Parameters.Append.CreateParameter("PASSWORD", adVarChar, adParamInput, 60, PASSWORD)
            .Parameters.Append.CreateParameter("USUARIO", adVarChar, adParamInput, 20, USUARIO)
            Update_User = .Parameters("RETURN_VALUE") '
            .Execute(Options:=adExecuteNoRecords)
        End With
        cmd = Nothing
PROC_ERR:  'MsgBox err.Description
    End Function

    '
    'tiene por finalidad verificar si Existen Usuarios en la BD
    Public Function Existen_UsuariosBD() As Integer
        Existen_UsuariosBD = -1 ' no existen user
        On Error Resume Next
        On Error GoTo PROC_ERR
        cmd = New ADODB.Command
        With cmd
            .CommandText = "sp_ExistenUserBD"
            .CommandType = adCmdStoredProc
            .ActiveConnection = DatEPostgrado.CnnPostgrado
            .Parameters.Append.CreateParameter("RETURN_VALUE", adInteger, adParamReturnValue, 0)
            .Execute(Options:=adExecuteNoRecords)
            Existen_UsuariosBD = .Parameters("RETURN_VALUE") '
        End With
        cmd = Nothing
PROC_ERR:  'MsgBox err.Description
    End Function

    '** funcion que devuelve el listado de Alumnos Ingresantes Por
    '** Promocion y Sede , con su directorio
    Public Function AlumnosingPorPromSede(PROMOCION As String, _
    CodMencion As String, SEDE As String) As ADODB.Recordset
        J = 0 'nro de alumnos
        On Error GoTo PROC_ERR
        cmd = New ADODB.Command
        With cmd
            .CommandText = "sp_AlumnosIngrPorPromSede"
            .CommandType = adCmdStoredProc
            .ActiveConnection = DatEPostgrado.CnnPostgrado
            .Parameters.Append.CreateParameter("RETURN_VALUE", adInteger, adParamReturnValue, 0)
            .Parameters.Append.CreateParameter("PROMOCION", adChar, adParamInput, 6, PROMOCION)
            .Parameters.Append.CreateParameter("CODMENCION", adChar, adParamInput, 4, CodMencion)
            .Parameters.Append.CreateParameter("SEDE", adVarChar, adParamInput, 30, SEDE)
            AlumnosingPorPromSede = .Execute
            I = .Parameters("RETURN_VALUE") '
        End With
        cmd = Nothing
PROC_ERR:  'MsgBox Err.Description
    End Function

    'Trae el Nombre de un Alumno a partir de su codigo
    Public Function TraeNombreAlumno(CODALUMNO As String)
        On Error GoTo errores_
        TraeNombreAlumno = ""
        Ssql = "Select NOMALUMNO From ALUMNO Where CODALUMNO='" & CODALUMNO & "'"
        Ejecuta_consultas(Ssql, rssql)
        If rssql.EOF Or rssql.BOF Then Exit Function
        TraeNombreAlumno = Trim(rssql.Fields(0))
errores_:
    End Function

    '** funcion que devuelve la lista de de Alumnos egresados de una mencion
    '** Promocion , segun el criterio : n de cred aprobados =n cred del plan de es
    '** estudios de su promocion
    '** la variable Tipo indica si los alumnos q han convalida cursos forman parte de la
    '** consulta, egresados(Tipo =1); caso contrario no son considerados (Tipo =0)
    Public Function AlumnosEgresados(PROMOCION As String, _
    CodMencion As String, NOTA As Double, TotalcredPlan As Integer, Optional TIPO As Integer = 1, Optional SEDE As String = "PIURA") As ADODB.Recordset
        k = 0 'nro de alumnos
        On Error GoTo PROC_ERR
        cmd = New ADODB.Command
        With cmd
            .CommandText = "sp_AlumnosEgresados"
            .CommandType = adCmdStoredProc
            .ActiveConnection = DatEPostgrado.CnnPostgrado
            .Parameters.Append.CreateParameter("RETURN_VALUE", adInteger, adParamReturnValue, 0)
            .Parameters.Append.CreateParameter("CODMENCION", adChar, adParamInput, 4, CodMencion)
            .Parameters.Append.CreateParameter("PROMOCION", adChar, adParamInput, 6, PROMOCION)
            .Parameters.Append.CreateParameter("SEDE", adVarChar, adParamInput, 20, SEDE)
            .Parameters.Append.CreateParameter("NOTA", adDouble, adParamInput, , NOTA)
            .Parameters.Append.CreateParameter("TotalCredPlan", adTinyInt, adParamInput, , TotalcredPlan)
            .Parameters.Append.CreateParameter("Tipo", adTinyInt, adParamInput, , TIPO)
            AlumnosEgresados = .Execute
            k = .Parameters("RETURN_VALUE") 'n de alumnos de la consulta
        End With
        cmd = Nothing
PROC_ERR:  'MsgBox Err.Description
    End Function

    '** funcion que devuelve la lista de de Alumnos egresados segun el semestre de egreso segun
    '** el criterio : n de cred aprobados =n cred del plan de es estudios de su promocion
    '** la variable Tipo indica si los alumnos q han convalida cursos forman parte de la
    '** consulta, egresados(Tipo =1); caso contrario no son considerados (Tipo =0)
    Public Function AlumnosEgresadosSemestre(SEMESTRE As String, _
    CodMencion As String, NOTA As Double, TotalcredPlan As Integer, Optional TIPO As Integer = 1, Optional SEDE As String = "PIURA") As ADODB.Recordset
        k = 0 'nro de alumnos
        On Error GoTo PROC_ERR
        cmd = New ADODB.Command
        With cmd
            .CommandText = "sp_AlumnosEgresadosSemestre"
            .CommandType = adCmdStoredProc
            .ActiveConnection = DatEPostgrado.CnnPostgrado
            .Parameters.Append.CreateParameter("RETURN_VALUE", adInteger, adParamReturnValue, 0)
            .Parameters.Append.CreateParameter("CODMENCION", adChar, adParamInput, 4, CodMencion)
            .Parameters.Append.CreateParameter("SEMESTRE", adChar, adParamInput, 6, SEMESTRE)
            .Parameters.Append.CreateParameter("SEDE", adVarChar, adParamInput, 20, SEDE)
            .Parameters.Append.CreateParameter("NOTA", adDouble, adParamInput, , NOTA)
            .Parameters.Append.CreateParameter("TotalCredPlan", adTinyInt, adParamInput, , TotalcredPlan)
            .Parameters.Append.CreateParameter("Tipo", adTinyInt, adParamInput, , TIPO)
            AlumnosEgresadosSemestre = .Execute
            k = .Parameters("RETURN_VALUE") 'n de alumnos de la consulta
        End With
        cmd = Nothing
PROC_ERR:  'MsgBox Err.Description
    End Function

    '** funcion que devuelve la lista de de Alumnos graduandos de una programa
    '** segun el criterio : n de cred aprobados =n cred del plan de estudios
    '** estudios de su promocion, aprobar la nota de ingles, y tener calificativo de tesis
    Public Function AlumnosGraduandos(CODPROGRAMA As String, NOTA As Double) As ADODB.Recordset
        k = 0 'nro de alumnos
        On Error GoTo PROC_ERR
        cmd = New ADODB.Command
        With cmd
            .CommandText = "sp_AlumnosGraduandos"
            .CommandType = adCmdStoredProc
            .ActiveConnection = DatEPostgrado.CnnPostgrado
            .Parameters.Append.CreateParameter("RETURN_VALUE", adInteger, adParamReturnValue, 0)
            .Parameters.Append.CreateParameter("CODPROGRAMA", adChar, adParamInput, 5, CODPROGRAMA)
            .Parameters.Append.CreateParameter("NOTA", adDouble, adParamInput, , NOTA)
            AlumnosGraduandos = .Execute
            k = .Parameters("RETURN_VALUE") 'n de alumnos de la consulta
        End With
        cmd = Nothing
PROC_ERR:  'MsgBox Err.Description
    End Function

    '** funcion que devuelve la lista de de Alumnos graduandos(y su tema de tesis) agrupados por programa
    '** segun el criterio : n de cred aprobados =n cred del plan de estudios
    '** estudios, aprobar la nota de ingles, y tener calificativo de tesis
    Public Function AlumnosGraduandos_TemaTes(NOTA As Double) As ADODB.Recordset
        k = 0 'nro de alumnos
        On Error GoTo PROC_ERR
        cmd = New ADODB.Command
        With cmd
            .CommandText = "sp_AlumnosGraduandos_TemaTes"
            .CommandType = adCmdStoredProc
            .ActiveConnection = DatEPostgrado.CnnPostgrado
            .Parameters.Append.CreateParameter("RETURN_VALUE", adInteger, adParamReturnValue, 0)
            .Parameters.Append.CreateParameter("NOTA", adDouble, adParamInput, , NOTA)
            AlumnosGraduandos_TemaTes = .Execute
            k = .Parameters("RETURN_VALUE") 'n de alumnos de la consulta
        End With
        cmd = Nothing
PROC_ERR:  'MsgBox Err.Description
    End Function

    '** trae de la tabla parametro la nota q es aprobatoria segun el reglamento
    '** se asume q es doce
    Public Function TraeNotaAprobRegl() As Double
        On Error GoTo errores_
        TraeNotaAprobRegl = 12
        Ssql = "Select CONTADOR From PARAMETRO WHERE NOMBRETABLA ='NOTA'"
        Ejecuta_consultas(Ssql, rssql)
        If rssql.EOF Or rssql.BOF Then Exit Function
        TraeNotaAprobRegl = IIf(Trim(rssql.Fields(0)) = 0, 12, Trim(rssql.Fields(0)))
errores_:  'MsgBox err.Description
    End Function

    '** CONSULta el plan de estudios q le corresponde a una determinada promocion
    Public Function PlanEstudioPromocion(CodMencion As String, _
    PROMOCION As String) As String
        On Error GoTo errores_
        PlanEstudioPromocion = "" 'VALOR POR DEFECTO
        Ssql = "SELECT TOP 1 * From PLANDE WHERE CODMENCION = '" & CodMencion & "' AND" & _
                " SEMESTREPLAN <= '" & PROMOCION & "'" & " ORDER BY SEMESTREPLAN DESC"
        Ejecuta_consultas(Ssql, rssql)
        If rssql.EOF Or rssql.BOF Then Exit Function
        PlanEstudioPromocion = IIf(Trim(rssql.Fields(0)) = "", "2001-1", Trim(rssql.Fields(0)))
errores_:
    End Function

    '** CONSULta el Maximo semestre que ha sido evaluado un alumno (tabla notas)
    Public Function MaxSemEvaluadoAlu(CODALUMNO As String) As String
        On Error GoTo errores_
        MaxSemEvaluadoAlu = "" 'VALOR POR DEFECTO
        Ssql = "SELECT MAX (SEMESTRE) FROM NOTAS WHERE CODALUMNO ='" & CODALUMNO & "'"
        Ejecuta_consultas(Ssql, rssql)
        If rssql.EOF Or rssql.BOF Then Exit Function
        MaxSemEvaluadoAlu = IIf(Trim(rssql.Fields(0)) = "", "", Trim(rssql.Fields(0)))
errores_:
    End Function

    '** esta funcion tiene como finalidad, traer todos la descripcion de un aula
    Public Function Trae_DescripcionAula(AULA As String) As String
        Trae_DescripcionAula = ""
        On Error GoTo errores_
        Ssql = "SELECT DESCRIPCION FROM AULAS WHERE AULA ='" & AULA & "'"
        Ejecuta_consultas(Ssql, rssql)
        If rssql.EOF Or rssql.BOF Then Exit Function
        Trae_DescripcionAula = Trim(rssql.Fields(0))
errores_:
    End Function

    '** esta funcion tiene como finalidad traer los datos del directorio del alumno:
    '** PROMOCION , NUMPLAN
    Public Sub ObtieneDatosAlumno(PROMOCION As String, NUMPLAN As String, CODALUMNO As String)
        On Error GoTo errores_
        Ssql = "SELECT PROMOCION, NUMPLAN FROM ALUMNO WHERE CODALUMNO='" & CODALUMNO & "'"
        Ejecuta_consultas(Ssql, rssql)
        If rssql.EOF Or rssql.BOF Then Exit Sub
        PROMOCION = Trim(rssql.Fields(0))
        NUMPLAN = Trim(rssql.Fields(1))
errores_:
    End Sub

    '** esta funcion tiene como finalidad, traer todos los semestres en las cuales ha
    '** existido matriculas de una determinada mencion
    Public Function TraeSemEnMatricula(CodMencion As String) As ADODB.Recordset
        On Error GoTo errores_
        cmd = New ADODB.Command
        With cmd
            .CommandText = "sp_SemestresEnMatricula"
            .CommandType = adCmdStoredProc
            .ActiveConnection = DatEPostgrado.CnnPostgrado
            .Parameters.Append.CreateParameter("CODMENCION", adChar, adParamInput, 4, CodMencion)
            TraeSemEnMatricula = .Execute
        End With
        cmd = Nothing
errores_:
    End Function


    '** FUNCIONES PARA OBTENER EL ORDEN DE MERITO EN LOS DOCUMENTOS ACADEMICOS

    '** funcion que tiene por finalidad devolver el numero de creditos de un plan de estudios
    '** hasta el ciclo enviado de un plan de estudios especifico
    Public Function TraeNumeroCredHastaCiclo(CICLO As Integer, NUMPLAN As String) As Integer
        Dim rs As ADODB.Recordset

        TraeNumeroCredHastaCiclo = 0
        On Error GoTo PROC_ERR
        cmd = New ADODB.Command
        With cmd
            .CommandText = "sp_TraeNumeroCredHastaCiclo"
            .CommandType = adCmdStoredProc
            .ActiveConnection = DatEPostgrado.CnnPostgrado
            .Parameters.Append.CreateParameter("CICLO", adTinyInt, adParamInput, , CICLO)
            .Parameters.Append.CreateParameter("NUMPLAN", adChar, adParamInput, 5, NUMPLAN)
            rssql = .Execute
            TraeNumeroCredHastaCiclo = rssql.Fields(0)
        End With
        cmd = Nothing
PROC_ERR:  ' MsgBox err.Description
    End Function

    '** funcion que tiene por finalidad devolver los semestres (registros) en la cual una determinada promocion
    '** de una mencion ha sido evaluada, se puede obtener mas de 4 semestres
    Public Function TraeSemestresEvaluadosProm(PROMOCION As String, CodMencion As String) As ADODB.Recordset
        On Error GoTo PROC_ERR
        cmd = New ADODB.Command
        With cmd
            .CommandText = "sp_TraeSemestresEvaluadosProm"
            .CommandType = adCmdStoredProc
            .ActiveConnection = DatEPostgrado.CnnPostgrado
            .Parameters.Append.CreateParameter("RETURN_VALUE", adInteger, adParamReturnValue, 0)
            .Parameters.Append.CreateParameter("PROMOCION", adTinyInt, adParamInput, 6, PROMOCION)
            .Parameters.Append.CreateParameter("CODMENCION", adChar, adParamInput, 4, CodMencion)
            TraeSemestresEvaluadosProm = .Execute
        End With
        cmd = Nothing
PROC_ERR:  ' MsgBox err.Description
    End Function

    '** devuelve el actas de ingles, en un recordset
    Public Function TraeNotasCursoExtrangero(CodMencion As String, PROMOCION As String) As ADODB.Recordset
        On Error GoTo PROC_ERR
        cmd = New ADODB.Command
        With cmd
            .CommandText = "sp_TraeNotasCursoExtrangero"
            .CommandType = adCmdStoredProc
            .ActiveConnection = DatEPostgrado.CnnPostgrado
            .Parameters.Append.CreateParameter("RETURN_VALUE", adInteger, adParamReturnValue, 0)
            .Parameters.Append.CreateParameter("CODMENCION", adChar, adParamInput, 4, CodMencion)
            .Parameters.Append.CreateParameter("PROMOCION", adChar, adParamInput, 6, PROMOCION)
            TraeNotasCursoExtrangero = .Execute
        End With
        cmd = Nothing
PROC_ERR:  ' MsgBox err.Description
    End Function


    '** esta funcion devuelve el Nro de creditos aprobados por el alumno
    '** cuya nota hayan sido aprobadas.
    Public Function NroCredAprobAlumno(CODALUMNO As String, NOTA As Double) As Integer
        On Error GoTo errores_
        Dim rs As ADODB.Recordset
        NroCredAprobAlumno = 0 'cero cred aprob.
        cmd = New ADODB.Command
        With cmd
            .CommandText = "sp_NroCredAprobAlumno"
            .CommandType = adCmdStoredProc
            .ActiveConnection = DatEPostgrado.CnnPostgrado
            .Parameters.Append.CreateParameter("CODALUMNO", adVarChar, adParamInput, 10, CODALUMNO)
            .Parameters.Append.CreateParameter("NOTA", adDouble, adParamInput, , NOTA)
            rs = .Execute
            NroCredAprobAlumno = rs.Fields(0).Value
        End With
        cmd = Nothing
        rs.Close()
errores_:
        rs = Nothing
    End Function



    '*********************************************************************************************************
    '*************************Para los Abstrac de la tesis ***************************************************

    'Devuelve a los abstrac registrados de los alumnos q pertenecen al programa enviado
    Public Function Mostrar_AbstracPorProg(ind As Integer, CODPROGRAMA As String) As ADODB.Recordset
        On Error GoTo PROC_ERR
        cmd = New ADODB.Command
        With cmd
            .CommandText = "sp_AbstracPorPrograma"
            .CommandType = adCmdStoredProc
            .ActiveConnection = DatEPostgrado.CnnPostgrado
            .Parameters.Append.CreateParameter("RETURN_VALUE", adInteger, adParamReturnValue, 0)
            .Parameters.Append.CreateParameter("Ind", adTinyInt, adParamInput, , ind)
            .Parameters.Append.CreateParameter("CODPROGRAMA", adChar, adParamInput, 5, CODPROGRAMA)
            Mostrar_AbstracPorProg = .Execute
            I = .Parameters("RETURN_VALUE") '
        End With
        cmd = Nothing
PROC_ERR:  'MsgBox Err.Description
    End Function

    '*********************************************************************************************************
    '************************* Para las estadisticas ***************************************************

    'funcion q Devuelve el nro de abstrac registrados por programa
    'agrupados por mencion
    Public Function Estadis_AbstracPorProg(CODPROGRAMA As String, ind As Integer) As ADODB.Recordset
        On Error GoTo PROC_ERR
        cmd = New ADODB.Command
        With cmd
            .CommandText = "sp_Estad_AbstracPorProg"
            .CommandType = adCmdStoredProc
            .ActiveConnection = DatEPostgrado.CnnPostgrado
            .Parameters.Append.CreateParameter("RETURN_VALUE", adInteger, adParamReturnValue, 0)
            .Parameters.Append.CreateParameter("CODPROGRAMA", adChar, adParamInput, 5, CODPROGRAMA)
            .Parameters.Append.CreateParameter("Ind", adTinyInt, adParamInput, , ind)
            Estadis_AbstracPorProg = .Execute
            I = .Parameters("RETURN_VALUE") '
        End With
        cmd = Nothing
PROC_ERR:  'MsgBox Err.Description
    End Function

    '** Funcion q devuelve la relacion del programa, mencion y sus respectivas promociones con su Nro de egresantes
    Public Function Estadis_Egresantes(PROMOCION As String, NOTA As Double, _
    TipoCons As Integer) As ADODB.Recordset
        On Error GoTo PROC_ERR
        cmd = New ADODB.Command
        With cmd
            .CommandText = "sp_EstadisticasEgresantes"
            .CommandType = adCmdStoredProc
            .ActiveConnection = DatEPostgrado.CnnPostgrado
            .Parameters.Append.CreateParameter("RETURN_VALUE", adInteger, adParamReturnValue, 0)
            .Parameters.Append.CreateParameter("PROMOCION", adVarChar, adParamInput, 6, PROMOCION)
            .Parameters.Append.CreateParameter("NOTA", adDouble, adParamInput, , NOTA)
            .Parameters.Append.CreateParameter("TipoCons", adTinyInt, adParamInput, , TipoCons)
            Estadis_Egresantes = .Execute
            I = .Parameters("RETURN_VALUE") '
        End With
        cmd = Nothing
PROC_ERR:  'MsgBox Err.Description
    End Function

    '*********************************************************************************************************
    '** Funcion q devuelve la relacion del programa, mencion y sus respectivas promociones con su Nro de graduados
    'se considera Graduados a los alumnos que hayan cumplido las exigencias de su plan de estudios, mas Tesis y han rendido examen de ingles.
    'segun el valor de la variable TipoCons se devuelven los resultados , TipoCons =1 se devuelven todas las promociones
    'TipoCons =2 de devuelve a partir de la promocion PROMOCION
    'Se considera la nota aprobatoria de Ingles superior a 11 (valor fijo en el procedimmiento)
    Public Function Estadis_Graduandos(PROMOCION As String, NOTA As Double, _
    TipoCons As Integer) As ADODB.Recordset
        On Error GoTo PROC_ERR
        cmd = New ADODB.Command
        With cmd
            .CommandText = "sp_EstadisticasGraduandos"
            .CommandType = adCmdStoredProc
            .ActiveConnection = DatEPostgrado.CnnPostgrado
            .Parameters.Append.CreateParameter("RETURN_VALUE", adInteger, adParamReturnValue, 0)
            .Parameters.Append.CreateParameter("PROMOCION", adVarChar, adParamInput, 6, PROMOCION)
            .Parameters.Append.CreateParameter("NOTA", adDouble, adParamInput, , NOTA)
            .Parameters.Append.CreateParameter("TipoCons", adTinyInt, adParamInput, , TipoCons)
            Estadis_Graduandos = .Execute
            I = .Parameters("RETURN_VALUE") '
        End With
        cmd = Nothing
PROC_ERR:  'MsgBox err.Description
    End Function

    '*********************************************************************************************************

    'devuelve el nombre de todos los asesores - presidentes y secretarios que se tiene registrados
    'que tengan coincidencias con la variable letra -para mostrarlos en una lista - el N° se almacena
    'en la variable I - Esto acelera las busquedas
    Public Function Docentes_Tesis(Letra As String) As ADODB.Recordset
        I = -1
        On Error GoTo PROC_ERR
        cmd = New ADODB.Command
        With cmd
            .CommandText = "sp_Devuelve_Ase_Pres_Secre"
            .CommandType = adCmdStoredProc
            .ActiveConnection = DatEPostgrado.CnnPostgrado
            .Parameters.Append.CreateParameter("RETURN_VALUE", adInteger, adParamReturnValue, 0)
            .Parameters.Append.CreateParameter("Letra", adVarChar, adParamInput, 10, Letra)
            Docentes_Tesis = .Execute
            I = .Parameters("RETURN_VALUE") '
        End With
        cmd = Nothing
PROC_ERR:  'MsgBox Err.Description
    End Function

    'funcion que Elimina el codigo del alumno: CODALUMNO del curso inscrito CODCURSO
    'llevado en el semestre: SEMESTRE
    Public Function EliminaInsCurso(CODALUMNO As String, CODCURSO As String, _
    SEMESTRE As String) As Integer
        EliminaInsCurso = -1 'no elimino
        On Error GoTo errores_
        cmd = New ADODB.Command
        With cmd
            .CommandText = "sp_EliminaInscripcionCursos"
            .CommandType = adCmdStoredProc
            .ActiveConnection = DatEPostgrado.CnnPostgrado
            .Parameters.Append.CreateParameter("RETURN_VALUE", adInteger, adParamReturnValue, 0)
            .Parameters.Append.CreateParameter("CODALUMNO", adVarChar, adParamInput, 10, CODALUMNO)
            .Parameters.Append.CreateParameter("CODCURSO", adChar, adParamInput, 6, CODCURSO)
            .Parameters.Append.CreateParameter("SEMESTRE", adChar, adParamInput, 6, SEMESTRE)
            .Execute(Options:=adExecuteNoRecords)
            EliminaInsCurso = .Parameters("RETURN_VALUE") '0 es Ok
        End With
        cmd = Nothing
errores_:  'MsgBox err.Description
    End Function


    '************************************************************************************************
    'funcion que devuelve las coincidencias de alumnos de una determinada mencion, quienes deben creditos
    'segun su plan de estudios
    Public Function Alumnos_DebeCreditos(Letra As String, _
    CodMencion As String) As ADODB.Recordset ', SEDE As String
        I = -1
        On Error GoTo PROC_ERR
        cmd = New ADODB.Command
        With cmd
            .CommandText = "sp_Alumnos_DebeCreditos"
            .CommandType = adCmdStoredProc
            .ActiveConnection = DatEPostgrado.CnnPostgrado
            .Parameters.Append.CreateParameter("RETURN_VALUE", adInteger, adParamReturnValue, 0)
            .Parameters.Append.CreateParameter("CODMENCION", adChar, adParamInput, 4, CodMencion)
            .Parameters.Append.CreateParameter("Letra", adVarChar, adParamInput, 20, Letra)
            .Parameters.Append.CreateParameter("NOTA", adDouble, adParamInput, , NOTA) 'NOTA as variable local
            Alumnos_DebeCreditos = .Execute
            I = .Parameters("RETURN_VALUE") '
        End With
        cmd = Nothing
PROC_ERR:  'MsgBox Err.Description
    End Function


    '** Funcion que devuelve el Nro de creditos o el ciclo de un curso, segun el parametro Ind:
    '** si Ind =0 entonces la funcion devuelve el NCRED(por defecto), caso contrario CICLO, del plan de estudios: NUMPLAN
    Public Function NCred_Ciclo_CursoPlan(NUMPLAN As String, CODCURSO As String, _
    Optional ind As Integer = 0) As Integer

        NCred_Ciclo_CursoPlan = 3 ' valor por defecto
        On Error Resume Next
        Ssql = "Select NCRED, CICLO From PLANESTUDIOS Where NUMPLAN='" & NUMPLAN & _
                "' And CODCURSO='" & CODCURSO & "'"
        Ejecuta_consultas(Ssql, rssql)

        If Not (rssql.EOF Or rssql.BOF) Then 'el curso esta en el plan de estudios del alumno
            Select Case ind
                Case 0 ' devuelve el nro de creditos
                    NCred_Ciclo_CursoPlan = CInt(rssql.Fields(0).Value)
                Case Else ' devuelve el ciclo del curso
                    NCred_Ciclo_CursoPlan = CInt(rssql.Fields(1).Value)
            End Select
        Else
            Select Case ind
                Case 0 ' devuelve el nro de creditos, el nro de creditos del curso
                    NCred_Ciclo_CursoPlan = Mid(CODCURSO, 4, 1)
                Case Else ' devuelve el ciclo del curso, se asume el ultimo ciclo
                    NCred_Ciclo_CursoPlan = 4
            End Select
        End If
    End Function

    '** funcion que devuelve la lista de de Alumnos q han convalidado algun curso
    Public Function Alumnos_EnConvalidacion(PROMOCION As String, _
    CodMencion As String, NOTA As Double, TotalcredPlan As Integer) As ADODB.Recordset
        On Error GoTo PROC_ERR
        cmd = New ADODB.Command
        With cmd
            .CommandText = "sp_Alumnos_EnConvalidacion"
            .CommandType = adCmdStoredProc
            .ActiveConnection = DatEPostgrado.CnnPostgrado
            .Parameters.Append.CreateParameter("RETURN_VALUE", adInteger, adParamReturnValue, 0)
            .Parameters.Append.CreateParameter("CODMENCION", adChar, adParamInput, 4, CodMencion)
            .Parameters.Append.CreateParameter("PROMOCION", adChar, adParamInput, 6, PROMOCION)
            .Parameters.Append.CreateParameter("NOTA", adDouble, adParamInput, , NOTA)
            .Parameters.Append.CreateParameter("TotalCredPlan", adTinyInt, adParamInput, , TotalcredPlan)
            '.Parameters.Append .CreateParameter("Tipo", adTinyInt, adParamInput, , TIPO)
            Alumnos_EnConvalidacion = .Execute
            I = .Parameters("RETURN_VALUE") 'n de alumnos de la consulta
        End With
        cmd = Nothing
PROC_ERR:  'MsgBox Err.Description
    End Function

    'funcion que devuelve el nro de creditos llevados en la epg, mas el nro de creditos
    'convalidados, total tiene los creditos llevados, rs contiene la los creditos convalidados, y se almacena en total
    Public Sub Total_Creditos_Conv(TOTAL As Integer, CODALUMNO As String)
        On Error Resume Next
        Dim rsa As ADODB.Recordset
        rsa = Busca("sp_NroCredConvalidados", 10, "CODALUMNO ", CODALUMNO)
        If rsa.EOF Or rsa.BOF Then Exit Sub
        TOTAL = TOTAL + rsa.Fields(0).Value  'creditos
    End Sub


    '************************************************************************************************
    'Devuelve a los alumnos por mencion - pormocion que han egresado teniendo convalidadación de cursos
    'jj es el numero de alumnos resultado de la consulta

    Public Function AlumnosEgres_Convalidado(PROMOCION As String, _
    CodMencion As String, NOTA As Double, TotalcredPlan As Integer, jj As Integer, Optional SEDE As String = "PIURA") As ADODB.Recordset

        On Error GoTo PROC_ERR
        cmd = New ADODB.Command
        With cmd
            .CommandText = "sp_AlumnosEgresadosConv"
            .CommandType = adCmdStoredProc
            .ActiveConnection = DatEPostgrado.CnnPostgrado
            .Parameters.Append.CreateParameter("RETURN_VALUE", adInteger, adParamReturnValue, 0)
            .Parameters.Append.CreateParameter("CODMENCION", adChar, adParamInput, 4, CodMencion)
            .Parameters.Append.CreateParameter("PROMOCION", adChar, adParamInput, 6, PROMOCION)
            .Parameters.Append.CreateParameter("SEDE", adVarChar, adParamInput, 20, SEDE)
            .Parameters.Append.CreateParameter("NOTA", adDouble, adParamInput, , NOTA)
            .Parameters.Append.CreateParameter("TotalCredPlan", adTinyInt, adParamInput, , TotalcredPlan)
            AlumnosEgres_Convalidado = .Execute
            jj = .Parameters("RETURN_VALUE") 'Nro de alumnos egresantes q han realizado convalidacion
        End With
        cmd = Nothing
PROC_ERR:  ' MsgBox err.Description
    End Function


    '************************************************************************************************
    ' pa que devuelve el listado de los coordinadores registrados, si la opcion intOpcion =1 se muestran los datos de un programa  especifica,
    ' caso contrario se muestran de todos los progrmas
    Public Function Resol_Coordinadores(intOpcion As Integer, CODPROGRAMA As String) As ADODB.Recordset

        On Error GoTo PROC_ERR
        cmd = New ADODB.Command
        With cmd
            .CommandText = "sp_Resol_Coordinadores"
            .CommandType = adCmdStoredProc
            .ActiveConnection = DatEPostgrado.CnnPostgrado
            .Parameters.Append.CreateParameter("RETURN_VALUE", adInteger, adParamReturnValue, 0)
            .Parameters.Append.CreateParameter("intOpcion", adTinyInt, adParamInput, , intOpcion)
            .Parameters.Append.CreateParameter("CODPROGRAMA", adVarChar, adParamInput, 5, CODPROGRAMA)
            Resol_Coordinadores = .Execute
            I = .Parameters("RETURN_VALUE") 'Nro de coord
        End With
        cmd = Nothing
PROC_ERR:  'MsgBox err.Description
    End Function

    'muestra los semestres en los cuales hubieron postulantes
    Public Sub Muestra_Semestres(COD_MENCION As String, DatCSemestre As DataCombo)
        On Error Resume Next
        Ssql = "Select Distinct PROMOCION From ALUMNO WHERE CODMENCION='" & COD_MENCION & "' ORDER BY 1"
        Ejecuta_consultas(Ssql, rssql)
        DatCSemestre.ListField = "PROMOCION"
        DatCSemestre.RowSource = rssql
    End Sub

    'muestra los semestres en los cuales han existido notas, de una mencion
    Public Sub MuestraSemestres_Notas(DatCSemestre As DataCombo, CodMencion As String)
        rssql = Busca("sp_SemestresEnInscripCursos", 4, "CODMENCION", CodMencion)
        DatCSemestre.ListField = "SEMESTRE"
        DatCSemestre.RowSource = rssql
    End Sub




End Module
