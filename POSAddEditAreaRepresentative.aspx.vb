Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Partial Class POSAddEditAreaRepresentative
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
       
        If Not IsPostBack Then
            If Not Request.QueryString("storeno") Is Nothing Then
                ViewState("storeno") = Request.QueryString("storeno").Trim()
            End If
            If Not Request.QueryString("storename") Is Nothing Then
                ViewState("storename") = Request.QueryString("storename").Trim()
                'lblStoreName.Text = ViewState("storename").ToString()
            End If
            ViewState("CurrentPage") = 1
            ViewState("DisplayNoofRow") = 15
            ViewState("sortcolumn") = "arealocation"
            ViewState("sortstatus") = "ASC"
            Call getAreaRepresentative()
        End If
        
    End Sub

    Protected Sub btnAddAreaPresentative_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddAreaPresentative.Click
        ViewState("Mode") = "Add"
        imgAr1.Visible = False
        'imgAr2.Visible = False
        Call getEmployeesList()
        txtAreaLocation.Text = ""
        'txtCommission.Text = "40.00"
        'txtARRoyaltyCommission.Text = "5.00"
        txtCommission.Text = "5.00"
        txtARRoyaltyCommission.Text = "40.00"
        mpeaddAreaRepresentative.Show()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "FocusScript", "setTimeout(function(){$get('" + txtAreaLocation.ClientID + "').focus();}, 100);", True)
        updtaddAreaRepresentative.Update()

    End Sub

    Protected Sub btnEditAreaRepresentative_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEditAreaRepresentative.Click
        ViewState("Mode") = "Edit"
        imgAr1.Visible = False
        'imgAr2.Visible = False
        Call getAreaRepresenativeid()
        Call getEmployeesList()
        Call EditAreaRepresenative()
        updtaddAreaRepresentative.Update()
    End Sub

    Protected Sub btnDeleteAreaRepresentative_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDeleteAreaRepresentative.Click
        Dim result As Integer = 0
        ViewState("Mode") = "Delete"
        Call getAreaRepresenativeid()
        Dim objSetup As New vbStationSetup
        objSetup.StoreNo = Session("storeno").ToString()
        objSetup.AreaRepID = ViewState("AreaRepID")
        result = objSetup.fnDeleteAreaRepresentative()
        If result > 0 Then
            ViewState("AreaRepID") = Nothing
            Call getAreaRepresentative()
        Else
            Call SetAlertMessage("We are unable to delete this Area Location!")
        End If

    End Sub

    Public Sub getAreaRepresentative()

        Dim ds As New DataSet
        Dim objSetup As New vbStationSetup

        objSetup.StoreNo = Session("storeno").ToString()
        objSetup.CurrentPage = ViewState("CurrentPage")
        objSetup.NoofDisplayRow = ViewState("DisplayNoofRow")
        objSetup.SortColumn = ViewState("sortcolumn").ToString()
        objSetup.SortStatus = ViewState("sortstatus").ToString()
        objSetup.Mode = 0
        ds = objSetup.fnGetAreaRepresenative()
        Dim count As Integer = ds.Tables(1).Rows(0)("cnt")
        If ds.Tables(0).Rows.Count > 0 Then
            gvAreaRepresentative.DataSource = ds.Tables(0)
            gvAreaRepresentative.DataBind()
            btnDeleteAreaRepresentative.Visible = True
            btnEditAreaRepresentative.Visible = True

            hdnNoofPage.Value = CInt(Math.Ceiling(count / CInt(ViewState("DisplayNoofRow"))))

            If ViewState("CurrentPage") = 1 AndAlso hdnNoofPage.Value > 1 Then
                lnkPrevCust.ForeColor = Drawing.Color.Gray
                lnkNextCust.ForeColor = Drawing.Color.Blue
                lnkPrevCust.Enabled = False
                lnkNextCust.Enabled = True

            ElseIf ViewState("CurrentPage") = 1 AndAlso hdnNoofPage.Value = 1 Then
                lnkPrevCust.Enabled = False
                lnkNextCust.Enabled = False
                lnkPrevCust.ForeColor = Drawing.Color.Gray
                lnkNextCust.ForeColor = Drawing.Color.Gray
            ElseIf ViewState("CurrentPage") = hdnNoofPage.Value AndAlso hdnNoofPage.Value > 1 Then
                lnkPrevCust.Enabled = True
                lnkNextCust.Enabled = False
                lnkPrevCust.ForeColor = Drawing.Color.Blue
                lnkNextCust.ForeColor = Drawing.Color.Gray
            ElseIf hdnNoofPage.Value = 0 Then
                lnkPrevCust.Enabled = False
                lnkNextCust.Enabled = False
                lnkPrevCust.ForeColor = Drawing.Color.Gray
                lnkNextCust.ForeColor = Drawing.Color.Gray
            End If
            Call getAreaRepresenativeid()
        Else

            gvAreaRepresentative.EmptyDataText = "No record found"
            gvAreaRepresentative.DataSource = Nothing
            gvAreaRepresentative.DataBind()
            btnDeleteAreaRepresentative.Visible = False
            btnEditAreaRepresentative.Visible = False
            lnkPrevCust.Enabled = False
            lnkNextCust.Enabled = False
            lnkPrevCust.ForeColor = Drawing.Color.Gray
            lnkNextCust.ForeColor = Drawing.Color.Gray
        End If
        pnlgrid.Update()

    End Sub

    Protected Sub gvAreaRepresentative_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvAreaRepresentative.RowDataBound
        
        If e.Row.RowType = DataControlRowType.DataRow Then
            If ViewState("AreaRepID") <> Nothing Then
                If (ViewState("AreaRepID").ToString.Trim() = CType(e.Row.Cells(0).FindControl("chkAreaRepresentative"), HiddenField).Value.Trim()) Then
                    CType(e.Row.Cells(0).FindControl("chkboxrecords"), CheckBox).Checked = True
                Else
                    CType(e.Row.Cells(0).FindControl("chkboxrecords"), CheckBox).Checked = False
                End If
            Else
                If e.Row.RowIndex = 0 Then
                    CType(e.Row.Cells(1).FindControl("chkboxrecords"), CheckBox).Checked = True
                    ViewState("AreaRepID") = CType(e.Row.Cells(0).FindControl("chkAreaRepresentative"), HiddenField).Value.Trim()
                    updtaddAreaRepresentative.Update()
                End If
            End If
        End If
        If e.Row.RowType = DataControlRowType.Header Then
            For Each cell As TableCell In e.Row.Cells
                If cell.HasControls Then
                    Dim imgAreaLocation As Image
                    Dim imgAreaRepresentative1 As Image
                    Dim imgAreaRepresentative2 As Image

                    imgAreaLocation = CType(e.Row.FindControl("imgAreaLocation"), Image)
                    imgAreaRepresentative1 = CType(e.Row.FindControl("imgAreaRepresentative1"), Image)
                    imgAreaRepresentative2 = CType(e.Row.FindControl("imgAreaRepresentative2"), Image)

                    If ViewState("sortcolumn").ToString().Trim.ToLower() = "arealocation" Then
                        imgAreaLocation.Visible = True
                        imgAreaRepresentative1.Visible = False
                        imgAreaRepresentative2.Visible = False
                        If ViewState("sortstatus") = "ASC" Then
                            imgAreaLocation.ImageUrl = "~/Images/arrowup.gif"
                        Else
                            imgAreaLocation.ImageUrl = "~/Images/arrowdown.gif"
                        End If
                    End If
                    If ViewState("sortcolumn") = "e1.lname" Then
                        imgAreaLocation.Visible = False
                        imgAreaRepresentative1.Visible = True
                        imgAreaRepresentative2.Visible = False
                        If ViewState("sortstatus") = "ASC" Then
                            imgAreaRepresentative1.ImageUrl = "~/Images/arrowup.gif"
                        Else
                            imgAreaRepresentative1.ImageUrl = "~/Images/arrowdown.gif"
                        End If
                    End If

                    If ViewState("sortcolumn") = "e2.lname" Then
                        imgAreaLocation.Visible = False
                        imgAreaRepresentative1.Visible = False
                        imgAreaRepresentative2.Visible = True
                        If ViewState("sortstatus") = "ASC" Then
                            imgAreaRepresentative2.ImageUrl = "~/Images/arrowup.gif"
                        Else
                            imgAreaRepresentative2.ImageUrl = "~/Images/arrowdown.gif"
                        End If
                    End If
                   
                End If
            Next

        End If
    End Sub
   
    Protected Sub imgsaveAreaRepresentative_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles imgsaveAreaRepresentative.Click
        Dim objSetup As New vbStationSetup
        imgAr1.Visible = False
        'imgAr2.Visible = False
        If drpFirstAreaRepresentative.SelectedIndex = 0 Then
            imgAr1.Visible = True
            Exit Sub
            'ElseIf drpSecondAreaRepresentative.SelectedIndex = 0 Then
            'imgAr2.Visible = True
            'Exit Sub
        End If

        objSetup.StoreNo = Session("storeno").ToString()
        objSetup.AreaLocation = txtAreaLocation.Text
        objSetup.AreaRepresentativeID1 = CType(drpFirstAreaRepresentative.SelectedValue, Integer)
        objSetup.AreaRepresentativeID2 = CType(drpSecondAreaRepresentative.SelectedValue, Integer)
        objSetup.Commission = txtCommission.Text
        objSetup.ARRoyaltyCommission = txtARRoyaltyCommission.Text

        If ViewState("Mode") = "Edit" Then
            objSetup.AreaRepID = ViewState("AreaRepID")
        Else
            objSetup.AreaRepID = 0
        End If
        Dim result As Integer = ViewState("AreaRepID")
        result = objSetup.fnAddEditAreaRepresentative()
        ViewState("AreaRepID") = result
        mpeaddAreaRepresentative.Hide()
        updtaddAreaRepresentative.Update()

        Call getAreaRepresentative()

    End Sub
    Protected Sub getAreaRepresenativeid()
        Dim count As Integer = 0
        For Each datarow In gvAreaRepresentative.Rows
            Dim chkbox As CheckBox = CType(datarow.Cells(0).FindControl("chkboxrecords"), CheckBox)
            If chkbox.Checked = True Then
                ViewState("AreaRepID") = CType(datarow.Cells(0).FindControl("chkAreaRepresentative"), HiddenField).Value.Trim()
                count = 1
                Exit For
            End If
        Next
        If count = 0 Then
            CType(gvAreaRepresentative.Rows(0).Cells(0).FindControl("chkboxrecords"), CheckBox).Checked = True
            Exit Sub
        End If
    End Sub
   
    Public Sub EditAreaRepresenative()
        Dim objSetup As New vbStationSetup
        objSetup.StoreNo = Session("storeno")
        objSetup.AreaRepID = ViewState("AreaRepID")
        Dim dtable As New DataTable
        dtable = objSetup.fnGetAreaRepresenativeDetails()
        If dtable.Rows.Count > 0 Then
            mpeaddAreaRepresentative.Show()
            txtAreaLocation.Text = dtable.Rows(0)("AreaLocation").ToString().Trim()
            drpFirstAreaRepresentative.SelectedValue = IIf(IsDBNull(dtable.Rows(0)("AreaRepresentativeID1")), 0, dtable.Rows(0)("AreaRepresentativeID1").ToString().Trim())
            drpSecondAreaRepresentative.SelectedValue = IIf(IsDBNull(dtable.Rows(0)("AreaRepresentativeID2")), 0, dtable.Rows(0)("AreaRepresentativeID2").ToString().Trim())
            txtCommission.Text = dtable.Rows(0)("Commission").ToString().Trim()
            txtARRoyaltyCommission.Text = dtable.Rows(0)("ARRoyaltyCommission").ToString().Trim()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "FocusScript", "setTimeout(function(){$get('" + txtAreaLocation.ClientID + "').focus();}, 100);", True)
        Else
            ScriptManager.RegisterStartupScript(Me.Page, Page.GetType(), "msg", "Alert('Record Not Found');", True)
            mpeaddAreaRepresentative.Show()
            txtAreaLocation.Focus()
        End If
        updtaddAreaRepresentative.Update()
    End Sub

    Public Sub SetAlertMessage(ByVal alertMessage As String)
        'To popup message box using Javascript 
        Dim sb As String
        sb = "alert"
        sb = sb + "("""
        sb = sb + alertMessage
        sb = sb + """);"
        If Not ClientScript.IsStartupScriptRegistered("AlertMessageHandler") Then
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "AlertMessageHandler", sb.ToString(), True)
        End If
    End Sub
    
    Protected Sub lnkPrevCust_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkPrevCust.Click
        ViewState("CurrentPage") = ViewState("CurrentPage") - 1
        If ViewState("CurrentPage") < hdnNoofPage.Value Then
            getAreaRepresentative()

            If ViewState("CurrentPage") = 1 Then
                lnkPrevCust.Enabled = False
            Else
                lnkPrevCust.Enabled = True
            End If
            lnkNextCust.Enabled = True
        End If
    End Sub

    Protected Sub lnkNextCust_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkNextCust.Click
        ViewState("CurrentPage") = ViewState("CurrentPage") + 1
        If hdnNoofPage.Value >= ViewState("CurrentPage") Then
            getAreaRepresentative()

            If hdnNoofPage.Value = ViewState("CurrentPage") Then
                lnkNextCust.Enabled = False
                lnkPrevCust.Enabled = False
            Else
                lnkNextCust.Enabled = True
                lnkPrevCust.Enabled = True
            End If
            lnkPrevCust.Enabled = True
        End If
    End Sub
    Private Sub getEmployeesList()
        Dim ds As New DataSet
        Dim objSetup As New vbStationSetup
        objSetup.StoreNo = Session("storeno").ToString()
        ds = objSetup.fnGetAreaRepresenativeEmployees()
        If Not ds Is Nothing AndAlso ds.Tables.Count > 0 Then
            drpFirstAreaRepresentative.DataSource = ds.Tables(0)
            drpFirstAreaRepresentative.DataTextField = "employee"
            drpFirstAreaRepresentative.DataValueField = "employee_id"
            drpFirstAreaRepresentative.DataBind()
            drpSecondAreaRepresentative.DataSource = ds.Tables(0)
            drpSecondAreaRepresentative.DataTextField = "employee"
            drpSecondAreaRepresentative.DataValueField = "employee_id"
            drpSecondAreaRepresentative.DataBind()
        End If
        drpFirstAreaRepresentative.Items.Insert(0, New ListItem("Select Employee", 0))
        drpSecondAreaRepresentative.Items.Insert(0, New ListItem("Select Employee", 0))
    End Sub

    Protected Sub gvAreaRepresentative_Sorting(sender As Object, e As GridViewSortEventArgs) Handles gvAreaRepresentative.Sorting
        If ViewState("sortcolumn") = e.SortExpression Then
            ViewState("sortcolumn") = e.SortExpression
            If ViewState("sortstatus") = "ASC" Then
                ViewState("sortstatus") = "DESC"
            Else
                ViewState("sortstatus") = "ASC"
            End If
        Else
            ViewState("sortcolumn") = e.SortExpression
            ViewState("sortstatus") = "ASC"
        End If
        Call getAreaRepresentative()
    End Sub

    Protected Sub btnCommissionReport_Click(sender As Object, e As EventArgs) Handles btnCommissionReport.Click
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "FocusSelectDate", "openARCommissionReportSelectDate();", True)
    End Sub
    
End Class
