Imports System.Data.OleDb

Public Class FormIngCursos


    Private Sub FormIngCursos_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Top = 0
        Me.Left = 0
        Muestra_Menciones(DatCProgramas, 3)
    End Sub
    '** al seleccionar un programa de la lista se muestran los nombres de los cursos
    '** que existen en dicho programa
    Private Sub DatCProgramas_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DatCProgramas.SelectedIndexChanged
        mshCursos.Clear()
        mshCursos.Rows = 3
        If Len(Trim(DatCProgramas.SelectedValue)) = 5 Then
            CODPROGRAMA = Strings.Left(Trim(DatCProgramas.SelectedValue), 5)
            Mostrar_CursosProg(CODPROGRAMA)
            'NUEVO_Click()
            'NUEVO.SetFocus
        End If
    End Sub

    Public Sub Mostrar_CursosProg(CODPROGRAMA As String)
        'On Error Resume Next


        Try
            Ssql = "Select CODCURSO,NOMCURSO, NUMCREDITOS From CURSO Where CODPROGRAMA='" & CODPROGRAMA & "'"

            'Ejecuta_consultas(Ssql, rssql)
            rssql = New ADODB.Recordset
            rssql.Open(Ssql, cn, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic)
            Dim objDA As New OleDbDataAdapter()
            Dim objDT As New DataTable()
            objDA.Fill(objDT, rssql)
            MsgBox(objDT.Rows.Count)
            MsgBox(rssql.Fields.Count)
            'If rssql.EOF Or rssql.BOF Then Exit Sub
            'With mshCursos
            '    '.dat
            '    '.DataBindings
            '    .set_ColWidth(0, 350)
            '    .set_ColWidth(1, 800) 'cod
            '    .set_ColWidth(2, 5000) 'nom

            '    .set_TextMatrix(0, 0, "abc")

            '    .set_TextMatrix(0, 1, "Codigo")
            '    .set_TextMatrix(0, 2, "Nombre del Curso")
            '    .set_TextMatrix(0, 3, "N° Cred.")
            '    Coloca_Numeracion(mshCursos)
            '    ColocaNegritaCab(mshCursos)
            'End With
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try


       

    End Sub



End Class