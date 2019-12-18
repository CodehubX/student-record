Imports System.Data.OleDb
Public Class Database

    'create database connection
    Private dbcon As New OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;" &
                                          "Data Source =ProgramLang.accdb;")


    'prepare db command
    Private DBCmd As OleDbCommand


    'DB Data --> to store data into our database
    Public DBDA As OleDbDataAdapter
    Public DBDT As DataTable


    'Query parameters
    Public Params As New List(Of OleDbParameter)


    'Query statistics
    Public RecordCount As Integer
    Public Exception As String

    Public Sub ExecQuery(Query As String)
        'resete query stat

        RecordCount = 0
        Exception = ""

        Try

            'open connection 
            dbcon.Open()

            'create db command
            DBCmd = New OleDbCommand(Query, dbcon)

            'load and loop params into db command
            Params.ForEach(Sub(p) DBCmd.Parameters.Add(p))

            'clear params list
            Params.Clear()

            'execute command and fill our datatable
            DBDT = New DataTable
            DBDA = New OleDbDataAdapter(DBCmd)

            RecordCount = DBDA.Fill(DBDT)

        Catch ex As Exception
            Exception = ex.Message
        End Try

        'close your connection 
        If dbcon.State = ConnectionState.Open Then dbcon.Close()
    End Sub


    'include query and command parameters
    Public Sub AddParam(Name As String, Value As Object)
        Dim NewParam As New OleDbParameter(Name, Value)

        Params.Add(NewParam)
    End Sub


    Public Function GetTable() As List(Of String)

        Dim lstTable As New List(Of String)

        Dim restriction As String() = New String() {Nothing, Nothing, Nothing, "Table"}

        Try
            dbcon.Open()
            DBDT = dbcon.GetSchema("Tables", restriction)

            For Each r As DataRow In DBDT.Rows
                lstTable.Add(r("TABLE_NAME"))

            Next
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            If dbcon.State = ConnectionState.Open Then dbcon.Close()
        End Try
        Return lstTable

    End Function
End Class
