Imports System.Data.OleDb

Module ModVariable
    '------------------------------------------------------------------------------------------------------------------------
    '** en este modulo se declaran las variables de ambiente utilizadas por el sistema, tales como:
    '** variables para bucles , contenedoras de valores temporales, variables utilizadas mientras el sistema es
    '** utilizado, arreglos , objetos  que rescatan datos de una consulta
    '------------------------------------------------------------------------------------------------------------------------

    Public INICIO As Integer
    Public VALORPROGRAMA As String
    Public PROGRA As String
    Public NPLAN As String '
    Public SEMEST As String
    Public CODIGOCURSO As String '
    Public CODIGOALUMNO As String
    Public CODIGOPROGRAMA As String '
    Public CODIGOMENCION As String '
    Public CODCURSO As String '
    Public CODALUMNO As String
    Public CODPROGRAMA As String '
    Public CodMencion As String '
    Public CODREQUISITOS As String '
    Public straux As String '
    Public REPORTCODPROG As String
    Public REPORTSEMES As String
    Public SEMESTREACAD As String
    Public Ssql As String
    ''''

    Public SECCION As String, COD_SECCION As String

    Public USER As String, NIVEL_USER As Integer   'Es el usuario que ha ingresado al sistema
    Public ANYO As String 'Es la denominacion del año actual
    Public SECRE_ACAD As String 'Es el actual secretario academico
    Public DIR_POSGRADO As String 'Es el actual director de posgrado


    Public intfrmEst As Integer  ' variable para las estadisticsd de los formulario
    Public intfrmRes As Integer  ' variable para las resoluciones de los formulario
    Public I As Integer, J As Integer, k As Integer, res As Integer
    Public intFrm As Integer
    Public intCICLO As Integer
    Public intfrmLogin As Integer 'para presentar la primera vez el formulario de login
    Public intNArreglo As Integer 'Arreglo
    Public intNReq As Integer 'Arreglo
    Public CodigoEnc As Integer
    Public SIST As Integer  'es para la ventana de inicio de sesion

    Public dblFlot As Double
    Public NOTA As Double ' es la nota aprobatoria reglamentaria

    Public rssql As ADODB.Recordset
    Public cmd As ADODB.Command

    Public DataTable As New DataTable()
    Public DataAdapter As New OleDbDataAdapter()
    Public DataSet As DataSet()


    Public arreglo() As Object
    Public Requisitos(0, 0) As Object
    Public ArraySem() As Object 'arreglo de dos dimensiones, en la primera dimension
    '** se guarda los semestres de evaluacion del alumno, y en segundo si ese semestre
    '** ha sido guardado por el usuario

    Public cn As New ADODB.Connection

    'Public cnStr = "Provider=SQLOLEDB.1;Data Source=DESKTOP-JPQOK8S;Persist Security Info=False;User ID=sa;Initial Catalog=bd_postgrado"
    'Provider=SQLOLEDB.1;Data Source=DESKTOP-JPQOK8S;Persist Security Info=False;User ID=sa;Initial Catalog=bd_postgrado




End Module
