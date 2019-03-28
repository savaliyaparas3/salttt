Imports System.Data
Imports System.Data.SqlClient
Imports customPaging
Imports System.IO
Partial Class POSAddInventorySKU
    Inherits System.Web.UI.Page
    Public strHelp As String = ""
    Dim strCurrentChar As String = ""
    Dim helpid As Int16 = 0

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        txtHelpDesc.config.toolbar = New Object() {New Object() {"Bold", "Italic", "Underline"}, _
                                            New Object() {"Styles", "Format", "Font", "FontSize"}, New Object() {"TextColor", "BGColor"}}
        If Not IsPostBack Then
            If Convert.ToBoolean(Session("IsCorpOffice")) = True Then
                ViewState("LocationStoreno") = Session("storeno")
                Session("LocationStore") = Session("storeno")
            End If
            ViewState("storeno") = Session("storeno")
            Dim Itemtype As String = ""
            Itemtype = Request.QueryString("type").ToString()
            If Itemtype <> "" AndAlso Itemtype <> "undefined" Then
                hdnclone.Value = "Y"
                If (Itemtype = "I") Then
                    rdoinventory.Enabled = True
                    rdoinventory.Checked = True
                    rdononinventory.Enabled = False
                    rdoCoupon.Enabled = False
                    rdoKitClub.Enabled = False
                    rdoService.Enabled = False
                    rdoPours.Enabled = False
                    rdoMultiPack.Enabled = False
                ElseIf (Itemtype = "N") Then
                    rdoinventory.Enabled = False
                    rdononinventory.Enabled = True
                    rdononinventory.Checked = True
                    rdoCoupon.Enabled = False
                    rdoKitClub.Enabled = False
                    rdoService.Enabled = False
                    rdoPours.Enabled = False
                    rdoMultiPack.Enabled = False
                ElseIf (Itemtype = "C") Then
                    rdoinventory.Enabled = False
                    rdononinventory.Enabled = False
                    rdoCoupon.Enabled = True
                    rdoCoupon.Checked = True
                    rdoKitClub.Enabled = False
                    rdoService.Enabled = False
                    rdoPours.Enabled = False
                    rdoMultiPack.Enabled = False
                ElseIf (Itemtype = "K") Then
                    rdoinventory.Enabled = False
                    rdononinventory.Enabled = False
                    rdoCoupon.Enabled = False
                    rdoMultiPack.Enabled = False
                    rdoKitClub.Enabled = True
                    rdoKitClub.Checked = True
                    rdoService.Enabled = False
                    rdoPours.Enabled = False
                ElseIf (Itemtype = "S") Then
                    rdoinventory.Enabled = False
                    rdononinventory.Enabled = False
                    rdoCoupon.Enabled = False
                    rdoKitClub.Enabled = False
                    rdoPours.Enabled = False
                    rdoMultiPack.Enabled = False
                    rdoService.Enabled = True
                    rdoService.Checked = True
                ElseIf (Itemtype = "P") Then
                    rdoinventory.Enabled = False
                    rdononinventory.Enabled = False
                    rdoCoupon.Enabled = False
                    rdoKitClub.Enabled = False
                    rdoService.Enabled = False
                    rdoMultiPack.Enabled = False
                    rdoPours.Enabled = True
                    rdoPours.Checked = True
                ElseIf (Itemtype = "M") Then
                    rdoinventory.Enabled = False
                    rdononinventory.Enabled = False
                    rdoCoupon.Enabled = False
                    rdoKitClub.Enabled = False
                    rdoService.Enabled = False
                    rdoPours.Enabled = False
                    rdoMultiPack.Enabled = True
                    rdoMultiPack.Checked = True
                End If
            Else
                hdnclone.Value = "N"
            End If
            'If ViewState("storeno").ToString = "2717" OrElse ViewState("storeno").ToString = "7365" Then
            '    rdoMultiPack.Visible = True
            '    imghelpMultipack.Visible = True
            '    trmultipack.Visible = True
            '    trmultipack.Attributes.Add("display", "")
            '    tdsizeupdown.Attributes.Add("style", "height:15px")
            'Else
            '    trmultipack.Attributes.Add("display", "none")
            '    tdsizeupdown.Attributes.Add("style", "height:45px")
            'End If
            txtSKU.Focus()
        End If
    End Sub
    Protected Sub Help_Click(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs)
        Dim j As String = e.CommandName
        ViewState("Button") = e.CommandName
        mpeDisplayHelp.Show()
        imgbtnCloseHelp.Focus()
        Dim helpid As Int16
        helpid = ViewState("Button")
        show_existing_userhelpid(helpid)
    End Sub
    <System.Web.Services.WebMethod()> _
 <System.Web.Script.Services.ScriptMethod()> _
    Public Shared Function GetAutoSKU(ByVal storeno As String) As String
        Dim maxsku As String = ""
        Dim objinv As New vbInventory
        objinv.Storeno = storeno
        maxsku = objinv.fnGetAutoSku()
        If maxsku < 10000 Then
            maxsku = 10000
        End If
        Return maxsku
    End Function

    Protected Sub show_existing_userhelpid(ByVal helpid As Int16)
        Dim strTemp As String = ""
        Dim objhelp As New help
        Dim objDt As DataTable
        objhelp.helpid = ViewState("Button")

        objDt = objhelp.fngetHelp()

        If objDt.Rows.Count > 0 Then
            hidDispHelp.Value = objDt.Rows(0).Item("H-ID").ToString.Trim()
            lnkDispTitle.Text = IIf(IsDBNull(objDt.Rows(0).Item("TitleBar")), "", objDt.Rows(0).Item("TitleBar")).ToString.Trim()

            strHelp = objDt.Rows(0).Item("Description").ToString.Trim()

            For j = 1 To Len(strHelp)
                strCurrentChar = Mid(strHelp, j, 1)
                If Asc(strCurrentChar) = 13 Then
                    strTemp = strTemp & " <br/>"
                Else
                    strTemp = strTemp & strCurrentChar
                End If
            Next

            strHelp = objDt.Rows(0).Item("Description").ToString.Trim()
            lblDispDesc.Text = strTemp
            strTemp = ""

        End If


    End Sub

    Public Function checkSKU() As Integer
        Dim Returnval As Integer = 0
        If Not txtSKU.Text = "" Then
            If txtSKU.Text.Chars(0).ToString.ToUpper = "R" Then
                SetAlertMessage("You can not start SKU with 'R'.")
                txtSKU.Text = ""
                txtSKU.Focus()
            ElseIf txtSKU.Text.Trim.Contains("*") Then
                SetAlertMessage("You can not enter '*' in SKU name.")
                txtSKU.Text = ""
                txtSKU.Focus()
            ElseIf txtSKU.Text.Trim = "SHIP" Then
                SetAlertMessage("'SHIP' cannot be used as SKU name.")
                txtSKU.Text = ""
                txtSKU.Focus()
            Else
                Dim objsku As New vbInventory()
                Dim strReturn As Integer
                If Convert.ToBoolean(Session("IsCorpOffice")) = True Then
                    objsku.Storeno = ViewState("LocationStoreno")
                    objsku.SKU = txtSKU.Text.Trim
                    strReturn = objsku.fncheckSKU()
                    If strReturn > 0 Then
                        lblsku1.Text = txtSKU.Text
                        mpecheck.Show()
                        imgbtnskucheckcontinue.Focus()
                    Else
                        Returnval = 1
                    End If
                Else
                    objsku.Storeno = ViewState("storeno")
                    objsku.SKU = txtSKU.Text.Trim
                    strReturn = objsku.fncheckSKU()
                    If strReturn > 0 Then
                        lblsku1.Text = txtSKU.Text
                        mpecheck.Show()
                        imgbtnskucheckcontinue.Focus()
                    Else
                        Returnval = 1
                    End If
                End If
            End If
        End If
        Return Returnval
    End Function
    Public Sub SetAlertMessage(ByVal alertMessage As String)
        'To popup message box using Javascript 
        Dim sb As String
        sb = "alert"
        sb = sb + "("""
        sb = sb + alertMessage
        sb = sb + """);"
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "AlertMessageHandler", sb.ToString(), True)

    End Sub

    Protected Sub imgbtnskucheckcontinue_Click(sender As Object, e As ImageClickEventArgs) Handles imgbtnskucheckcontinue.Click, imgbtnclosefirst.Click
        mpecheck.Hide()
        txtSKU.Text = ""
        txtSKU.Focus()
        upaddnewsku.Update()
    End Sub

    Protected Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "closePage", "CloseWindow();", True)
    End Sub

    Protected Sub chkAutoSku_CheckedChanged(sender As Object, e As EventArgs) Handles chkAutoSku.CheckedChanged
        If chkAutoSku.Checked Then
            Dim autosku As String = ""
            If Convert.ToBoolean(Session("IsCorpOffice")) = True Then
                autosku = GetAutoSKU(ViewState("LocationStoreno"))
            Else
                autosku = GetAutoSKU(ViewState("storeno"))
            End If
            If rdoMultiPack.Checked = True Then
                txtSKU.Text = GenerateSKUForMultiPack()
            Else
                txtSKU.Text = Convert.ToInt32(autosku) + 1
            End If

            txtSKU.Enabled = False
        Else
            txtSKU.Enabled = True
            If rdoMultiPack.Checked = False Then
                txtSKU.Text = ""
            End If
            txtSKU.Focus()
        End If
    End Sub
    Private Function GenerateSKUForMultiPack() As String

        Dim xNoArray() As Char = "123456789".ToCharArray
        Dim xGenerator As System.Random = New System.Random()
        Dim xStr As String = String.Empty

        While xStr.Length < 4
            xStr &= xNoArray(xGenerator.Next(0, xNoArray.Length))
        End While

        Return "MP" + xStr

    End Function
    Protected Sub btnContinue_Click(sender As Object, e As EventArgs) Handles btnContinue.Click
        Dim chk As Integer = 0
        chk = checkSKU()
        If chk = 1 Then
            ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "Returnvalues", "PassValuesBack()", True)
        End If
    End Sub
    'Protected Sub rdoPours_CheckedChanged(sender As Object, e As EventArgs) Handles rdoPours.CheckedChanged
    '    If Not ViewState("storeno") = "7365" AndAlso Not ViewState("storeno") = "2717" Then
    '        SetAlertMessage("This feature is under development.")
    '        rdoPours.Checked = False
    '        rdoinventory.Checked = True
    '    End If
    'End Sub


    Private Sub insert_update_UserHelp()
        Dim objhelpinsert As New help()

        objhelpinsert.helpid = ViewState("Button")
        objhelpinsert.Title = txttitle.Text.Trim()
        objhelpinsert.Desc = txtHelpDesc.Text.Trim()
        objhelpinsert.fnInsertUpdateHelp()

    End Sub
    Private Sub show_Editable_UserHelp(ByVal strHid As Int16)
        Dim objhelp As New help
        Dim objDt As DataTable
        objhelp.helpid = ViewState("Button")

        objDt = objhelp.fngetHelp()

        If objDt.Rows.Count > 0 Then
            hidHelpId.Value = objDt.Rows(0).Item("H-ID")
            txttitle.Text = IIf(IsDBNull(objDt.Rows(0).Item("TitleBar")), "", objDt.Rows(0).Item("TitleBar"))
            txtHelpDesc.Text = IIf(IsDBNull(objDt.Rows(0).Item("Description")), "", objDt.Rows(0).Item("Description"))
        End If
    End Sub
    Protected Sub imgSaveServiceHelp_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgSaveServiceHelp.Click
        helpid = ViewState("Button")
        insert_update_UserHelp()
        mpeEditHelp.Hide()

    End Sub
    Protected Sub lnkDispTitle_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkDispTitle.Click
        txtemail.Focus()
        mpeDisplayHelp.Hide()
        mpeModalHelp.Show()
    End Sub
    Protected Sub imgOKServiceHelp_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgOKServiceHelp.Click
        Dim strAdminEmail As String = System.Configuration.ConfigurationSettings.AppSettings("strAdminUid") '''' Pick from Web.Config file Key name is strAdminUid 
        Dim strAdminPassword As String = System.Configuration.ConfigurationSettings.AppSettings("strAdminPassword") + Format(DateTime.Now, "MMddyyyy") + "!" '''' Pick from Web.Config file Key name is strAdminPassword 

        If txtemail.Text.Trim = strAdminEmail AndAlso txtpwd.Text.Trim = strAdminPassword Then

            lblreqEmail.Visible = False
            lblreqPwd.Visible = False

            helpid = ViewState("Button")
            show_Editable_UserHelp(helpid)

            mpeModalHelp.Hide()
            mpeEditHelp.Show()
            txtemail.Text = ""
            txtpwd.Text = ""
        Else
            setAlert("Only System Administrator can edit user help.")
            txtemail.Text = ""
            txtpwd.Text = ""
            txtemail.Focus()
        End If
    End Sub
    Public Sub setAlert(ByVal msg As String)
        Dim sb As String
        sb = "alert"
        sb = sb + "('"
        sb = sb + msg
        sb = sb + "');"
        ScriptManager.RegisterStartupScript(Page, Me.GetType(), "AlertMessageHandler", sb.ToString, True)
    End Sub

    Protected Sub imgCancelHelp_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgCancelHelp.Click
        mpeModalHelp.Hide()
    End Sub
    Protected Sub imgCancelServiceHelp_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgCancelServiceHelp.Click
        mpeEditHelp.Hide()
        mpeModalHelp.Hide()
        mpeDisplayHelp.Show()
    End Sub

    Protected Sub rdoinventory_CheckedChanged(sender As Object, e As EventArgs) Handles rdoinventory.CheckedChanged, rdoCoupon.CheckedChanged, rdoKitClub.CheckedChanged, rdoMultiPack.CheckedChanged, rdononinventory.CheckedChanged, rdoPours.CheckedChanged, rdoService.CheckedChanged
        txtSKU.Text = ""
        txtSKU.Focus()
        If rdoMultiPack.Checked = True Then
            txtSKU.Text = GenerateSKUForMultiPack()
        End If
        chkAutoSku_CheckedChanged(Nothing, Nothing)
    End Sub
End Class
