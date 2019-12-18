Public Class EditStudentForm

    'Database Code Start
    Private access As New Database
    Private CurrentRecord As Integer = 0

    Private Sub EditStudentForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Me.Text = "Student Edit Form"
        cbbGroup.Items.Add("PL1A")
        cbbGroup.Items.Add("PL1B")
        cbbGroup.Items.Add("PL1C")

        txtID.Enabled = False
        txtName.Enabled = False
        txtEmail.Enabled = False
        cbbGroup.Enabled = False
    End Sub

    Private Sub ShowData(sender As Object, e As EventArgs) Handles Me.Shown
        RefreshData()
        GetData()
    End Sub

    Public Sub RefreshData()
        access.ExecQuery("SELECT * FROM Student")
        dgvdata.DataSource = access.DBDT
    End Sub

    Private Sub GetData()
        'query water info to be filled
        access.ExecQuery("SELECT * FROM Student")
    End Sub

    Private Sub BtnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        btnDelete.Enabled = True
        btnFirst.Enabled = True
        btnPrevious.Enabled = True
        btnNext.Enabled = True
        btnLast.Enabled = True
        txtName.Enabled = False
        txtEmail.Enabled = False
        cbbGroup.Enabled = False
    End Sub

    Private Sub BtnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        DeleteUSer()
    End Sub

    Private Sub BtnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        btnDelete.Enabled = False
        btnFirst.Enabled = False
        btnPrevious.Enabled = False
        btnNext.Enabled = False
        btnLast.Enabled = False
        txtName.Enabled = True
        txtEmail.Enabled = True
        cbbGroup.Enabled = True
    End Sub

    Private Sub BtnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        UpdateRecord()
        GetRecord()
        RefreshData()

        btnDelete.Enabled = True
        btnFirst.Enabled = True
        btnPrevious.Enabled = True
        btnNext.Enabled = True
        btnLast.Enabled = True
        txtName.Enabled = False
        txtEmail.Enabled = False
        cbbGroup.Enabled = False
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

        'return first data row in table

        Dim r As DataRow = access.DBDT.Rows(CurrentRecord)

        'populate fields
        txtID.Text = r("StudID").ToString
        txtName.Text = r("StudName").ToString
        txtEmail.Text = r("StudEmail").ToString
        cbbGroup.Text = r("StudGroup").ToString
    End Sub

    Private Sub UpdateRecord()

        'add parameters order matters !!!
        access.AddParam("@Student_Name", txtName.Text)
        access.AddParam("@Student_Email", txtEmail.Text)
        access.AddParam("@Student_Group", cbbGroup.Text)

        Dim r As DataRow = access.DBDT.Rows(CurrentRecord)
        Dim a As String = r("StudID").ToString
        access.AddParam("@Student_ID", a)

        'run command
        access.ExecQuery("UPDATE Student " &
                         "SET StudName=@Student_Name,StudEmail=@Student_Email,StudGroup=@Student_Group " &
                         "WHERE StudID=@Student_ID")

        MessageBox.Show("Student successful updated.", "Congratulations")

        RefreshData()
        GetData()
    End Sub

    Private Sub DeleteUSer()

        'confirm deletion 
        If MsgBox("are you sure you want to delete the selected record?", MsgBoxStyle.YesNo, "Confirm Delete") = MsgBoxResult.No Then Exit Sub

        'delete selected user
        Dim r As DataRow = access.DBDT.Rows(CurrentRecord)
        Dim a As String = r("StudID").ToString
        access.AddParam("@Student_ID", a)

        'run Query
        access.ExecQuery("DELETE FROM Student WHERE StudID=@Student_ID")

        'profile successful deleted
        MessageBox.Show("Student successful deleted.", "Congratulations")

        'clean up and refresh
        txtID.Clear()
        txtName.Clear()
        txtEmail.Clear()
        cbbGroup.SelectedIndex = -1

        RefreshData()
    End Sub

End Class