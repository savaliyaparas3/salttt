Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Globalization
Partial Class POSAddEditDonation
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Request.QueryString("Mode").ToString.ToLower.Trim = "add" Then
                trstatus.Visible = False
                lblHeader.Text = "Add Donation"
                txtorgcode.Text = ""
                txtorgname.Text = ""
            Else
                trstatus.Visible = True
                lblHeader.Text = "Edit Donation"
                txtorgcode.Text = Request.QueryString("OrgCode").ToString.Trim
                txtorgname.Text = Request.QueryString("OrgName").ToString.Trim
                If Request.QueryString("OrgStatus").ToString.Trim = "Archived" Then
                    ddlstatus.SelectedValue = 0
                Else
                    ddlstatus.SelectedValue = 1
                End If

            End If
        End If
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim objStore As New vbstoresetup
        Dim strReturn As String
        objStore.OrgCode = CultureInfo.CurrentCulture.TextInfo.ToUpper(txtorgcode.Text.ToString.Trim)
        objStore.OrgName = txtorgname.Text.ToString.Trim
        If Request.QueryString("Mode").ToString.ToLower.Trim = "edit" Then
            objStore.Status = ddlstatus.SelectedValue
            objStore.OrgId = Request.QueryString("OrgId").ToString.Trim
        End If

        strReturn = objStore.fnAddEditDonation()
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "Get", "CloseWindowWithRetValue();", True)
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "Get", "CloseWindow();", True)
    End Sub
End Class
