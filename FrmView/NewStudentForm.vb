Public Class NewStudentForm

    'Database Code Start
    Private access As New Database

    Private Sub NewStudentForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Text = "Student Register Form"
        txtID.Focus()

        cbbGroup.Items.Add("PL1A")
        cbbGroup.Items.Add("PL1B")
        cbbGroup.Items.Add("PL1C")

        'With cbbGroup
        '.Items.Add("PL1A")
        '.Items.Add("PL1B")
        '.Items.Add("PL1C")
        'End With

    End Sub

    Private Sub BtnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub


    Private Sub BtnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click

        'add parameters
        access.AddParam("@Student_ID", txtID.Text)
        access.AddParam("@Student_Name", txtName.Text)
        access.AddParam("@Student_Email", txtEmail.Text)
        access.AddParam("@Student_Group", cbbGroup.Text)


        'execute insert comment
        access.ExecQuery("INSERT INTO Student (StudID,StudName,StudEmail,StudGroup) " &
                             "VALUES (@Student_ID,@Student_Name,@Student_Email,@Student_Group); ")

        'succes added a record
        MessageBox.Show("Successful Registered.", "Congratulations")

        txtID.Clear()
        txtName.Clear()
        txtEmail.Clear()
        cbbGroup.SelectedIndex = -1
    End Sub

End Class