Imports System.Data
Imports System.Data.SqlClient



Partial Class POSAddEmployee
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        If Not IsPostBack Then




        End If





    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSave.Click
        Dim objConnection As conClass = New conClass()
        objConnection.DBconnect()

        'create command for stored procedure
        Dim objCommand As SqlCommand = New SqlCommand()
        Dim objparameterreturn As SqlParameter = New SqlParameter("returnvalue", SqlDbType.Int)
        objparameterreturn.Direction = ParameterDirection.ReturnValue


        objCommand.Connection = objConnection.con1
        objCommand.CommandText = "proc_employee_addedit_details"
        objCommand.CommandType = CommandType.StoredProcedure
        objCommand.Parameters.AddWithValue("@employee_id", ViewState("storeno"))
        objCommand.Parameters.AddWithValue("@lname", txtlastname.Text.Trim())
        objCommand.Parameters.AddWithValue("@fname", txtFirstName.Text.Trim())
        objCommand.Parameters.AddWithValue("@mname", txtMiddleName.Text.Trim())
        objCommand.Parameters.AddWithValue("@empid", txtUsername.Text.Trim.Replace("'", "''"))
        objCommand.Parameters.AddWithValue("@emppassword", txtPassword.Text.Trim.Replace("'", "''"))
        objCommand.Parameters.AddWithValue("@address", txtAddress.Text.Trim())
        objCommand.Parameters.AddWithValue("@city", txtCity.Text.Trim())
        objCommand.Parameters.AddWithValue("@state", txtState.Text.Trim())
        objCommand.Parameters.AddWithValue("@zip", txtZip.Text.Trim())
        objCommand.Parameters.AddWithValue("@phone", txtPhone.Text.Trim.Trim())
        objCommand.Parameters.AddWithValue("@email", txtEmail.Text.Trim())
        objCommand.Parameters.AddWithValue("@ssn", txtssnumber.Text.Trim())
        objCommand.Parameters.AddWithValue("@basepay", txtHourlyrate.Text.Trim())
        objCommand.Parameters.AddWithValue("@us_citizen", ddlUscitizen.SelectedValue)
        objCommand.Parameters.AddWithValue("@birthday", txtBirthdate.Text.Trim())
        objCommand.Parameters.AddWithValue("@employed", txtemployed.TagName.Trim())
        objCommand.Parameters.AddWithValue("@terminated", txtTermineted.Value.Trim())
        objCommand.Parameters.AddWithValue("@note", txtNotes.Text.Trim())
        objCommand.Parameters.AddWithValue("@picture", "")
        objCommand.Parameters.AddWithValue("@lock", "")
        objCommand.Parameters.AddWithValue("@admin", "N")
        objCommand.Parameters.AddWithValue("@Fullscreen", "")
        objCommand.Parameters.AddWithValue("@storeno", ViewState("storeno"))

        objCommand.Parameters.Add(objparameterreturn)

        'Try
        objCommand.ExecuteNonQuery()
        Dim i As Integer = objCommand.Parameters("returnvalue").Value
        If i > 0 Then



        Else

        End If

        'Catch ex As Exception
        'Response.Write(ex.ToString())
        'Finally
        objConnection.DBDisconnect()
        ' End Try
    End Sub

    Protected Sub btnCancel1_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnCancel1.Click
        Response.Redirect("POSEmployee.aspx")
    End Sub


End Class
