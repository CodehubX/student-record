Public Class SearchStudentForm

    'Database Code Start
    Private access As New Database
    Private CurrentRecord As Integer = 0

    Private Sub SearchStudentForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Text = "Student Search Form"
    End Sub

    Private Sub BtnFirst_Click(sender As Object, e As EventArgs) Handles btnFirst.Click
        CurrentRecord = 0
        GetRecord()

        btnLast.Enabled = True
        btnNext.Enabled = True
        btnPrevious.Enabled = True
    End Sub

    Private Sub BtnPrevious_Click(sender As Object, e As EventArgs) Handles btnPrevious.Click
        NextRecord(-1)
    End Sub

    Private Sub BtnNext_Click(sender As Object, e As EventArgs) Handles btnNext.Click
        NextRecord(1)
    End Sub

    Private Sub BtnLast_Click(sender As Object, e As EventArgs) Handles btnLast.Click
        CurrentRecord = access.DBDT.Rows.Count - 1
        GetRecord()
    End Sub

    Private Sub BtnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub BtnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        Dim studentName As String

        studentName = InputBox("Key in Name of Student.")

        SearchAdmin(studentName)
    End Sub

    Private Sub SearchAdmin(Name As String)

        access.AddParam("@name", "%" & Name & "%")

        access.ExecQuery("SELECT * " &
                         "FROM Student " &
                         "WHERE studName LIKE @name")

        'fill datagrid from the combobox
        dgvdata.DataSource = access.DBDT

    End Sub

    Private Sub NextRecord(Addval As Integer)

        CurrentRecord += Addval ' advance position by adval

        If CurrentRecord > access.DBDT.Rows.Count - 1 Then CurrentRecord = 0 'loop to first record

        If CurrentRecord < 0 Then CurrentRecord = access.DBDT.Rows.Count - 1 'loop to last record

        'update form 
        GetRecord()
    End Sub

    Private Sub GetRecord()

        'fail if no record found or position is out of range
        If access.DBDT.Rows.Count < 1 OrElse CurrentRecord > access.DBDT.Rows.Count - 1 Then Exit Sub
    End Sub

End Class