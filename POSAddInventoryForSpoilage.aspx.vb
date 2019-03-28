Imports System
Imports System.Data
Imports System.Data.SqlClient
Partial Class POSAddInventoryForSpoilage
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session("storeno") Is Nothing Then
            Session.Abandon()
            Response.Redirect("POSLogin.aspx")
        End If


        If Not Page.IsPostBack Then
            Call SetSortstatus()
            ViewState("CurrentPage") = 1
            ViewState("first_limit") = 0
            ViewState("last_limit ") = 17
            ViewState("inventorysearch") = ""
            'ViewState("storeno") = Session("storeno")

            If Convert.ToBoolean(Session("IsCorpOffice")) = True Then
                ViewState("storeno") = Session("LocationStore")
            Else
                ViewState("storeno") = Session("storeno")
            End If

            ViewState("stationno") = Session("stationno")
            'ViewState("invsortcolumn") = "desc1"
            ViewState("invsortstatus") = "ASC"
            ViewState("coupon") = "N"
            ViewState("item_mst_id") = Nothing
            ViewState("AllSku") = "1"
            txtSearch.Attributes.Add("autocomplete", "off")
            getGlobalSetupInventory()
            ViewState("vendorid") = ""
            If Session("chkwildcardsearch") = Nothing Then
                chkInvwildcardsearch.Checked = False
            Else
                chkInvwildcardsearch.Checked = True
            End If
            If Request.QueryString("Inv") = "Y" Then
                ViewState("isInv") = "Y"
            Else
                ViewState("isInv") = "N"
            End If

            If Session("IsCorpLockDown") IsNot Nothing Then
                ViewState("IsCorpLockDown") = Session("IsCorpLockDown").ToString
            End If


            If Request.QueryString("search") IsNot Nothing Then
                If Request.QueryString("search") <> "" Then
                    ViewState("invsearch") = Request.QueryString("search").ToString().Replace("""", "'")
                    getinventorylistSearch()
                Else
                    getinventorylist()
                End If
            Else
                getinventorylist()
            End If

            If Request.QueryString("vendorid") IsNot Nothing AndAlso Request.QueryString("vendorid") > 0 Then
                Dim objInventory As New vbInventory
                Dim dtvendor As New DataTable
                ViewState("vendorid") = Request.QueryString("vendorid")
                objInventory.Storeno = ViewState("storeno")
                objInventory.vendorvalue = Request.QueryString("vendorid")

                dtvendor = objInventory.fnGetVendordetails()
                If dtvendor.Rows.Count > 0 Then
                    chkvendor.Visible = True
                    chkvendor.Text = dtvendor.Rows(0)("company").ToString + " Items Only"
                Else
                    chkvendor.Visible = False
                    ViewState("vendorid") = ""
                End If
            Else
                chkvendor.Visible = False
                ViewState("vendorid") = ""
            End If
            updatepanelinventory.Update()
        End If
    End Sub

    Public Sub getinventorylist()
        Dim objInventory As New vbInventory
        objInventory.Storeno = ViewState("storeno")
        objInventory.SearchColumn = ViewState("invsortcolumn")
        If ViewState("invsearch") <> "" Then
            objInventory.SearchText = ViewState("invsearch")
        Else
            objInventory.SearchText = ""
        End If
        objInventory.SortColumn = ViewState("invsortcolumn")
        objInventory.SortStatus = ViewState("invsortstatus")
        objInventory.FirstLimit = ViewState("first_limit")
        objInventory.LastLimit = ViewState("last_limit ")
        objInventory.checkStock = chkInStockItem.Checked
        objInventory.wildcard = chkInvwildcardsearch.Checked
        If chkvendor.Visible = True Then
            If chkvendor.Checked = True Then
                objInventory.vendorvalue = ViewState("vendorid").ToString
            Else
                objInventory.vendorvalue = ""
            End If
        Else
            objInventory.vendorvalue = ""
        End If

        Dim dtInventory As New DataTable
        If ViewState("inventorysearch") IsNot Nothing And ViewState("inventorysearch") <> "" Then
            objInventory.SearchText = ViewState("inventorysearch")
        End If
        dtInventory = objInventory.fnSelectInventoryforKitClub()

        If Not ViewState("item_mst_id") Is Nothing Then
            Dim total As Integer = 0
            Dim extender As Decimal = dtInventory.Rows.Count Mod gvInventoryList.PageSize
            If extender = 0 Then
                total = dtInventory.Rows.Count / gvInventoryList.PageSize
            Else
                total = dtInventory.Rows.Count / gvInventoryList.PageSize

            End If
            Dim rowno As Integer = 0
            If ViewState("pageindexInv") Is Nothing Then
                Dim pagesize As Integer = gvInventoryList.PageSize
                For i As Integer = 0 To total
                    If pagesize * (i + 1) >= rowno Then
                        gvInventoryList.PageIndex = i
                        Exit For
                    End If

                Next
            Else
                gvInventoryList.PageIndex = ViewState("pageindexInv")
            End If

        Else
            ViewState("item_mst_id") = Nothing
        End If

        If objInventory.BolError = False Then
            If dtInventory.Rows.Count > 0 Then
                ViewState("Advance_Search") = "False"
                gvInventoryList.DataSource = dtInventory
                gvInventoryList.DataBind()
                lblInvMessage.Text = ""
                lblvalueinventory.Value = "1"
                lblInvMessage.Enabled = False
                gvInventoryList.Visible = True
                btnInvtoryAdvanceSearch.Enabled = True
                chkInvwildcardsearch.Enabled = True
            Else
                lblInvMessage.Text = "No Records Found."
                lblInvMessage.Enabled = True
                lblvalueinventory.Value = "0"
                gvInventoryList.Visible = False

            End If

        Else
            lblInvMessage.Text = "No Records Found."
            lblInvMessage.Enabled = True
            lblvalueinventory.Value = "0"
            gvInventoryList.Visible = False
        End If

        hdnPageonindexinv.Value = gvInventoryList.PageSize
        hdnCurrentpageinv.Value = (gvInventoryList.PageIndex + 1).ToString
        hdnPagecountinv.Value = gvInventoryList.PageCount.ToString()
        hdnPagesizeinv.Value = gvInventoryList.Rows.Count.ToString
        hdnBtnidinv.Value = gvInventoryList.ClientID.Replace("_", "$")
        lnkbtnInvClearAdvanceSearchResults.Visible = False
        trinvlist.Style.Add("display", "")
        hdnPageonindexinv.Value = gvInventoryList.PageSize

        'GC.Collect()
        If ViewState("first_limit") <= 0 Or (ViewState("first_limit") = 1 And ViewState("last_limit ") = 17) Then
            lnkbtnPrev.Enabled = False
        Else
            lnkbtnPrev.Enabled = True
        End If

        If dtInventory.Rows.Count < 17 Then
            lnkbtnNext.Enabled = False
        Else
            lnkbtnNext.Enabled = True
        End If

        If ViewState("size") = "N" Then
            gvInventoryList.Columns(2).Visible = False
        End If
    End Sub
    Protected Sub getinventorylisttmp()



        Dim objInventory As New vbInventory
        objInventory.Storeno = ViewState("storeno")
        objInventory.SearchColumn = ViewState("invsortcolumn")
        If ViewState("invsearch") <> "" Then
            objInventory.SearchText = ViewState("invsearch")
        Else
            objInventory.SearchText = ""
        End If
        objInventory.SortColumn = ViewState("invsortcolumn")
        objInventory.SortStatus = ViewState("invsortstatus")
        objInventory.FirstLimit = ViewState("first_limit")
        objInventory.LastLimit = ViewState("last_limit ")
        objInventory.checkStock = chkInStockItem.Checked
        objInventory.wildcard = chkInvwildcardsearch.Checked
        Dim dtInventory As New DataTable
        If ViewState("inventorysearch") IsNot Nothing And ViewState("inventorysearch") <> "" Then
            objInventory.SearchText = ViewState("inventorysearch")
        End If
        dtInventory = objInventory.fnSelectInventoryforKitClub()


        If objInventory.BolError = False Then
            If dtInventory.Rows.Count > 0 Then
                If Not ViewState("item_mst_id") Is Nothing Then
                    Dim total As Integer = 0
                    Dim extender As Decimal = dtInventory.Rows.Count Mod gvInventoryList.PageSize
                    If extender = 0 Then
                        total = dtInventory.Rows.Count / gvInventoryList.PageSize
                    Else
                        total = dtInventory.Rows.Count / gvInventoryList.PageSize
                    End If
                    Dim rowno As Integer = 0
                    If ViewState("pageindexInv") Is Nothing Then
                        Dim pagesize As Integer = gvInventoryList.PageSize
                        For i As Integer = 0 To total
                            If pagesize * (i + 1) >= rowno Then
                                gvInventoryList.PageIndex = i
                                Exit For
                            End If

                        Next
                    Else
                        gvInventoryList.PageIndex = ViewState("pageindexInv")
                    End If
                Else
                    ViewState("item_mst_id") = Nothing
                End If

                gvInventoryList.DataSource = dtInventory
                gvInventoryList.DataBind()
                ViewState("Advance_Search") = "False"
                gvInventoryList.DataSource = dtInventory
                gvInventoryList.DataBind()
                lblInvMessage.Text = ""
                lblvalueinventory.Value = "1"
                lblInvMessage.Enabled = False
                gvInventoryList.Visible = True
                btnInvtoryAdvanceSearch.Enabled = True
                chkInvwildcardsearch.Enabled = True
            Else
                lblInvMessage.Text = "No Records Found."
                lblInvMessage.Enabled = True
                lblvalueinventory.Value = "0"
                gvInventoryList.Visible = False

            End If
        Else
            lblInvMessage.Text = "No Records Found."
            lblInvMessage.Enabled = True
            lblvalueinventory.Value = "0"
            gvInventoryList.Visible = False

        End If

        If ViewState("size") = "N" Then
            gvInventoryList.Columns(2).Visible = False
        End If
    End Sub
    Public Sub getinventorylistSearch()
        Dim objInventory As New vbInventory
        objInventory.Storeno = ViewState("storeno")
        objInventory.SearchColumn = ViewState("invsortcolumn")
        objInventory.SearchText = ViewState("invsearch")
        objInventory.SortColumn = ViewState("invsortcolumn")
        objInventory.SortStatus = ViewState("invsortstatus")
        objInventory.FirstLimit = ViewState("first_limit")
        objInventory.LastLimit = ViewState("last_limit ")
        objInventory.checkStock = chkInStockItem.Checked
        objInventory.wildcard = chkInvwildcardsearch.Checked
        objInventory.Coupon = ViewState("coupon").ToString
        If chkvendor.Visible = True Then
            If chkvendor.Checked = True Then
                objInventory.vendorvalue = ViewState("vendorid").ToString
            Else
                objInventory.vendorvalue = ""
            End If
        Else
            objInventory.vendorvalue = ""
        End If
        Dim dtInventory As New DataTable

        If ViewState("inventorysearch") IsNot Nothing And ViewState("inventorysearch") <> "" Then
            objInventory.SearchText = ViewState("inventorysearch")
        End If

        dtInventory = objInventory.fnSelectAllInventory1()

        If Not ViewState("item_mst_id") Is Nothing Then
            Dim total As Integer = 0
            Dim extender As Decimal = dtInventory.Rows.Count Mod gvInventoryList.PageSize
            If extender = 0 Then
                total = dtInventory.Rows.Count / gvInventoryList.PageSize
            Else
                total = dtInventory.Rows.Count / gvInventoryList.PageSize

            End If
            Dim rowno As Integer = 0
            For i As Integer = 0 To dtInventory.Rows.Count - 1
                If dtInventory.Rows(i)("item_mst_id").ToString.Trim = ViewState("item_mst_id").ToString.Trim Then
                    rowno = i
                End If
            Next
            If ViewState("pageindexInv") Is Nothing Then
                Dim pagesize As Integer = gvInventoryList.PageSize
                For i As Integer = 0 To total
                    If pagesize * (i + 1) >= rowno Then
                        gvInventoryList.PageIndex = i
                        Exit For
                    End If

                Next
            Else
                gvInventoryList.PageIndex = ViewState("pageindexInv")
            End If

        Else
            ViewState("item_mst_id") = Nothing
        End If

        If objInventory.BolError = False Then
            If dtInventory.Rows.Count > 0 Then
                gvInventoryList.DataSource = dtInventory
                gvInventoryList.DataBind()
                lblInvMessage.Text = ""
                lblvalueinventory.Value = "1"
                lblInvMessage.Enabled = False
                gvInventoryList.Visible = True
                btnInvtoryAdvanceSearch.Enabled = True
                chkInvwildcardsearch.Enabled = True
            Else
                lblInvMessage.Text = "No Records Found."
                lblInvMessage.Enabled = True
                lblvalueinventory.Value = "0"
                gvInventoryList.Visible = False

            End If

        Else
            lblInvMessage.Text = "No Records Found."
            lblInvMessage.Enabled = True
            lblvalueinventory.Value = "0"
            gvInventoryList.Visible = False

        End If

        hdnPageonindexinv.Value = gvInventoryList.PageSize
        hdnCurrentpageinv.Value = (gvInventoryList.PageIndex + 1).ToString
        hdnPagecountinv.Value = gvInventoryList.PageCount.ToString()
        hdnPagesizeinv.Value = gvInventoryList.Rows.Count.ToString
        hdnBtnidinv.Value = gvInventoryList.ClientID.Replace("_", "$")
        lnkbtnInvClearAdvanceSearchResults.Visible = False
        trinvlist.Style.Add("display", "")
        hdnPageonindexinv.Value = gvInventoryList.PageSize
        'lnkbtnNext.Enabled = False
        'lnkbtnPrev.Enabled = False

        'GC.Collect()
        If ViewState("first_limit") <= 0 Or (ViewState("first_limit") = 1 And ViewState("last_limit ") = 17) Then
            lnkbtnPrev.Enabled = False
        Else
            lnkbtnPrev.Enabled = True
        End If

        If dtInventory.Rows.Count < 10 Then
            lnkbtnNext.Enabled = False
        Else
            lnkbtnNext.Enabled = True
        End If

        If ViewState("size") = "N" Then
            gvInventoryList.Columns(2).Visible = False
        End If
    End Sub
    Public Sub SetSortstatus()
        Dim objInventory As New vbInventory
        Dim dtLook As New DataTable
        If ViewState("storeno") Is Nothing Then
            ViewState("storeno") = Session("storeno")
        End If
        objInventory.Storeno = ViewState("storeno").ToString.Trim
        dtLook = objInventory.fnInventoryLookup
        For i As Integer = 0 To dtLook.Rows.Count - 1
            If ViewState("invsortcolumn") Is Nothing And dtLook.Rows(i)("sortOrder").ToString = "*" Then
                ViewState("invsortcolumn") = dtLook.Rows(i)("originalcolname").ToString
            End If
        Next
    End Sub
 

    Protected Sub gvInventoryList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvInventoryList.PageIndexChanging
        ViewState("pageindexInv") = e.NewPageIndex
        ViewState("item_mst_id") = Nothing
        ViewState("oldindex") = gvInventoryList.PageIndex
        ViewState("newindex") = e.NewPageIndex
        gvInventoryList.PageIndex = e.NewPageIndex

        Call getinventorylist()
        'If ViewState("invsortcolumn") = "item_mst_id" Then
        '    Call getinventorylisttmp()
        'Else
        '    Call getinventorylist()
        'End If

    End Sub

    Protected Sub gvInventoryList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvInventoryList.RowDataBound

        If (e.Row.RowType = DataControlRowType.DataRow) Then
            'e.Row.Attributes.Add("onmousedown", (ClientScript.GetPostBackEventReference(gvInventoryList, "Select$" + e.Row.RowIndex.ToString())))
            e.Row.Style.Add("cursor", "pointer")

            'e.Row.Attributes.Add("onclick", String.Format("javascript:return gridEnter ($get('" + e.Row.ClientID + "'), {0});", e.Row.RowIndex))

            Dim MainItemSKU As String = CType(e.Row.Cells(1).FindControl("lblIditem_mst_id"), Label).Text.Trim()

            'If CType(e.Row.Cells(1).FindControl("lblItem_Type"), Label).Text.Trim() = "P" Then
            '    ViewState("mainitem_mst_id") = MainItemSKU
            '    'ScriptManager.RegisterStartupScript(Page, [GetType], "InventoryPours", "CallPoursItemPopup('" & MainItemSKU & "');", True)
            '    e.Row.Attributes.Add("onclick", "CallPoursItemPopup('" & MainItemSKU & "');")
            'ElseIf CType(e.Row.Cells(1).FindControl("lblItem_Type"), Label).Text.Trim() = "M" Then
            '    ViewState("mainitem_mst_id") = MainItemSKU
            '    'ScriptManager.RegisterStartupScript(Page, [GetType], "InventoryPours", "CallMultiPackItemPopup('" & MainItemSKU & "');", True)
            '    e.Row.Attributes.Add("onclick", "CallMultiPackItemPopup('" & MainItemSKU & "');")
            'Else
            '    e.Row.Attributes.Add("onclick", String.Format("javascript:return gridEnter ($get('" + e.Row.ClientID + "'), {0});", e.Row.RowIndex))
            'End If

            If CType(e.Row.Cells(1).FindControl("lblItem_Type"), Label).Text.Trim() = "M" Then
                ViewState("mainitem_mst_id") = MainItemSKU
                'ScriptManager.RegisterStartupScript(Page, [GetType], "InventoryPours", "CallMultiPackItemPopup('" & MainItemSKU & "');", True)
                e.Row.Attributes.Add("onclick", "CallMultiPackItemPopup('" & MainItemSKU & "');")
            Else
                e.Row.Attributes.Add("onclick", String.Format("javascript:return gridEnter ($get('" + e.Row.ClientID + "'), {0});", e.Row.RowIndex))
            End If

            txtSearch.Attributes.Add("onkeydown", "javascript:return SelectSibling(event);txtSkuKeyhandler(event);")

            'e.Row.Attributes("onselectstart") = "javascript:return false;"
            e.Row.Attributes("onselectstart") = "returnfalse();"
            Dim script As String = ""
            If ViewState("item_mst_id") Is Nothing Then
                If ViewState("oldindex") = ViewState("newindex") Then
                    If e.Row.RowIndex = 0 Then
                        script = String.Format("javascript:initializeGridinv();SelectRow($get('" + e.Row.ClientID + "'), {0});", e.Row.RowIndex)
                        ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "init", script, True)
                        ScriptManager.GetCurrent(Me.Page).SetFocus(txtSearch.ClientID)
                    End If
                ElseIf ViewState("oldindex") < ViewState("newindex") Then
                    If e.Row.RowIndex = 0 Then
                        script = String.Format("javascript:initializeGridinv();SelectRow($get('" + e.Row.ClientID + "'), {0});", e.Row.RowIndex)
                        ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "init", script, True)
                        ScriptManager.GetCurrent(Me.Page).SetFocus(txtSearch.ClientID)
                    End If
                ElseIf ViewState("oldindex") > ViewState("newindex") Then
                    If e.Row.RowIndex = gvInventoryList.PageSize - 1 Then
                        script = String.Format("javascript:initializeGridinv();SelectRow($get('" + e.Row.ClientID + "'), {0});", e.Row.RowIndex)
                        ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "init", script, True)
                        ScriptManager.GetCurrent(Me.Page).SetFocus(txtSearch.ClientID)
                    End If
                End If
                ScriptManager.GetCurrent(Me.Page).SetFocus(txtSearch.ClientID)
            Else
                If (ViewState("item_mst_id").ToString.Trim() = CType(e.Row.Cells(1).FindControl("lblIditem_mst_id"), Label).Text.Trim()) Then
                    CType(e.Row.Cells(0).FindControl("chkColumn"), CheckBox).Checked = True
                    script = String.Format("javascript:initializeGridinv();SelectRow($get('" + e.Row.ClientID + "'), {0});", e.Row.RowIndex)
                    ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "init", script, True)
                    ScriptManager.GetCurrent(Me.Page).SetFocus(txtSearch.ClientID)
                Else
                    CType(e.Row.Cells(0).FindControl("chkColumn"), CheckBox).Checked = False
                End If
                ScriptManager.GetCurrent(Me.Page).SetFocus(txtSearch.ClientID)
            End If



        End If
        If e.Row.RowType = DataControlRowType.Header Then
            For Each cell As TableCell In e.Row.Cells
                If cell.HasControls Then
                    For Each objControl As Control In cell.Controls
                        If TypeOf (objControl) Is LinkButton Then
                            Dim obj As LinkButton = CType(objControl, LinkButton)
                            If ViewState("invsortcolumn") = obj.ID Then
                                ViewState("editcolname") = obj.CommandName
                            End If
                        Else
                            If TypeOf (objControl) Is Image Then
                                Dim obj As Image = CType(objControl, Image)
                                Dim sortcolumn As String = "img" + ViewState("invsortcolumn").ToString()
                                If obj.ID = sortcolumn Then
                                    obj.Visible = True
                                    If ViewState("invsortstatus") = "ASC" Then
                                        obj.ImageUrl = "~/Images/arrowup.gif"
                                    Else
                                        obj.ImageUrl = "~/Images/arrowdown.gif"
                                    End If
                                Else
                                    obj.Visible = False
                                End If
                            End If
                        End If
                    Next
                End If
            Next
        End If
    End Sub

    Protected Sub gvInventoryList_SelectedIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSelectEventArgs) Handles gvInventoryList.SelectedIndexChanging
        Dim Script As String = String.Format("javascript:initializeGridinv();SelectRow($get('" + gvInventoryList.Rows(e.NewSelectedIndex).ClientID + "'), {0});", e.NewSelectedIndex)
        ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "init", Script, True)

        

        If ViewState("isInv") = "Y" Then
            ViewState("SKU") = CType(gvInventoryList.Rows(e.NewSelectedIndex).Cells(1).FindControl("lblIditem_mst_id"), Label).Text.Trim()
            'Call getSKUdetails()
        Else
            Session("skukit") = CType(gvInventoryList.Rows(e.NewSelectedIndex).Cells(1).FindControl("lblIditem_mst_id"), Label).Text.Trim()
            Dim jscriptString1 As String = "<script language='JavaScript' type='text/javascript'>" _
                                         & "window.returnValue='Selectsku';" _
                                         & "window.close();" _
                                         & "</script>"
            ScriptManager.RegisterStartupScript(Me.Page, Page.GetType(), "jscriptString", jscriptString1, False)


        End If
    End Sub

    
    Private Function converttofloat(ByVal value As String) As String
        '' Function To convert a variable to float
        Dim strInput = value.Trim
        Dim strCurrentChar As String = ""
        Dim strTemp As String = ""
        Dim counter As Integer = 0

        For I = 1 To Len(strInput)
            strCurrentChar = Mid(strInput, I, 1)
            Dim k As Integer = Asc(strCurrentChar)
            If (k >= 46 And k <= 57) Or k = 48 Or k = 46 Or k = 45 Then
                If k = 45 Then
                    If I = 1 Then
                        strTemp = strTemp & strCurrentChar
                    End If
                ElseIf k = 46 Then
                    counter = counter + 1
                    If counter = 0 Or counter = 1 Then
                        strTemp = strTemp & strCurrentChar
                    End If
                Else
                    strTemp = strTemp & strCurrentChar
                End If
            Else

            End If
        Next
        If IsNumeric(strTemp.Trim) Then

        Else
            strTemp = "0.00"
        End If

        Return strTemp.Trim
    End Function
    Protected Sub gvInventoryList_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvInventoryList.Sorting

        ViewState("first_limit") = 0
        ViewState("last_limit ") = 17
        ViewState("CurrentPage") = 1
        If ViewState("invsortcolumn") = e.SortExpression Then
            ViewState("invsortcolumn") = e.SortExpression
            If ViewState("invsortstatus") = "ASC" Then
                ViewState("invsortstatus") = "DESC"
            Else
                ViewState("invsortstatus") = "ASC"
            End If
        Else
            ViewState("invsortcolumn") = e.SortExpression
            ViewState("invsortstatus") = "ASC"
        End If

        ViewState("item_mst_id") = Nothing

        If ViewState("Advance_Search") = "True" Then
            imgInvFind_Click(Nothing, Nothing)
        Else
            Call getinventorylist()
        End If

        'If ViewState("invsortcolumn") = "item_mst_id" Then
        '    Call getinventorylisttmp()
        'Else
        '    Call getinventorylist()
        'End If

        ScriptManager.GetCurrent(Me.Page).SetFocus(txtSearch.ClientID)
    End Sub
    Public Sub getdeptvalues()

        Dim objconnection As conClass = New conClass

        objconnection.DBconnect()

        Dim objcommand As SqlCommand = New SqlCommand
        objcommand.CommandType = CommandType.StoredProcedure
        objcommand.CommandText = "proc_inventory_get_dept_values"
        objcommand.Connection = objconnection.con1
        objcommand.Parameters.AddWithValue("@storeno", ViewState("storeno"))

        Dim objdataadapter As SqlDataAdapter = New SqlDataAdapter
        Dim objdataset As DataSet = New DataSet
        objdataadapter.SelectCommand = objcommand

        Try
            objdataadapter.Fill(objdataset)
            If objdataset.Tables.Count > 0 Then
                If objdataset.Tables(0).Rows.Count > 0 Then


                    txtDeptSearch.Text = "--All--"
                    hdnDeptSearch.Value = ""
                Else

                End If
            End If
        Catch ex As Exception
            Response.Write(ex.ToString)
        Finally
            objconnection.DBDisconnect()
        End Try
    End Sub
    Protected Sub lnkbtnNext_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkbtnNext.Click

        ViewState("first_limit") = (17 * (ViewState("CurrentPage")) + 1)
        ViewState("last_limit ") = 17 * (ViewState("CurrentPage") + 1)
        ViewState("CurrentPage") = ViewState("CurrentPage") + 1

        If ViewState("Advance_Search") = "True" Then
            imgInvFind_Click(Nothing, Nothing)
        Else
            Call getinventorylist()
            'If ViewState("invsortcolumn") = "item_mst_id" Then
            '    'Call getinventorylisttmp()
            'Else
            '    Call getinventorylist()
            'End If
        End If

    End Sub

    Protected Sub lnkbtnPrev_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkbtnPrev.Click

        ViewState("CurrentPage") = ViewState("CurrentPage") - 1
        ViewState("first_limit") = (17 * (ViewState("CurrentPage") - 1) + 1)
        ViewState("last_limit ") = 17 * (ViewState("CurrentPage"))

        If ViewState("Advance_Search") = "True" Then
            imgInvFind_Click(Nothing, Nothing)
        Else
            Call getinventorylist()
            'If ViewState("invsortcolumn") = "item_mst_id" Then

            '    'Call getinventorylisttmp()
            'Else
            '    Call getinventorylist()
            'End If
        End If

    End Sub

    Public Sub getGlobalSetupInventory()
        Dim ds As New DataSet
        Dim objinv As New vbInventory

        objinv.Storeno = ViewState("storeno")
        ds = objinv.fnGetGlobalSetupInventory()

        If ds.Tables.Count > 1 Then
            If ds.Tables(0).Rows.Count > 0 Then
                ViewState("size") = IIf(ds.Tables(0).Rows(0)("yesno").ToString.Trim = "Y", "Y", "N")
                ViewState("qtypercase") = IIf(ds.Tables(1).Rows(0)("yesno").ToString.Trim = "Y", "Y", "N")
                ViewState("styleflag") = IIf(ds.Tables(2).Rows(0)("yesno").ToString.Trim = "Y", "Y", "N")
                ViewState("caseprice") = IIf(ds.Tables(3).Rows(0)("yesno").ToString.Trim = "Y", "Y", "N")
            Else
                ViewState("size") = "N"
                ViewState("qtypercase") = "N"
                ViewState("styleflag") = "N"
                ViewState("caseprice") = "N"
            End If
        End If
    End Sub

    Protected Sub imgInvSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles imgInvSearch.Click
        ViewState("CurrentPage") = 1
        ViewState("inventorysearch") = txtSearch.Text.Trim
        hdnsearch.Value = ViewState("inventorysearch")
        If txtSearch.Text.Replace("'", "''").Trim = "" Then
            ViewState("searchtext") = txtSearch.Text.Trim
        Else
            If chkInvwildcardsearch.Checked = True Then
                ViewState("searchtext") = "%" & txtSearch.Text.Replace("'", "''").Trim() & "%"
            Else
                ViewState("searchtext") = txtSearch.Text.Replace("'", "''").Trim()
            End If
        End If
        gvInventoryList.PageIndex = 0
        ViewState("item_mst_id") = Nothing
        ViewState("oldindex") = ViewState("newindex") = 0
        ViewState("first_limit") = 1
        ViewState("last_limit ") = 17

        Call getinventorylist()

        tdinvdiv.Height = 305
        grdinvdiv.Style.Add("height", "305")
        txtSearch.Text = ""
        txtSearch.Focus()
        txtSearch.Text = String.Empty
    End Sub
    Protected Sub btnInvtoryAdvanceSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnInvtoryAdvanceSearch.Click
        mpeInvAdvanceSearch.Show()
        ViewState("CurrentPage") = 1
        ViewState("first_limit") = 0
        ViewState("last_limit ") = 17
        If ((ViewState("FrequentBuyer") = "Y") And (ViewState("points")) = "onItem") Then
            trPointssearch.Visible = True
        Else
            trPointssearch.Visible = False
        End If
        txtSKUSearch.Focus()
        ViewState("SearchCriteria") = "All"
        getsizevalues()
        getstylesizeval()
        getdeptvalues()

        'getstylevalues()
        getvendorvaluesforsearch()
        btnSelStyleSearch.Visible = False
        txtStyleSearch.Visible = False
        hdnStyleSearch.Value = ""
        txtStyleSearch.Text = "--All--"
        rdoqtyonhold.Enabled = True
        rdoqtyonhold.Items(1).Selected = True
        rdoPrice.Enabled = True
        rdoPrice.Items(2).Selected = True

        UpdatePanelInvAdvanceSearch.Update()
        ViewState("Inventoryadvancesearch") = True
    End Sub
    Protected Sub getstylesizeval()
        Dim objinv As New vbInventory
        Dim strcheck As String = ""
        Dim dt As New DataTable
        objinv.Storeno = ViewState("storeno")
        dt = objinv.fncheckstylesizeval()
        If dt.Rows.Count > 0 Then
            If dt.Rows(0)("captions").ToString.Trim.ToLower = "style" And dt.Rows(0)("yesno").ToString.ToLower = "n" Then
                trstylepopup.Visible = False
            Else
                trstylepopup.Visible = True
                getstylevalues1()
            End If
            If dt.Rows(1)("captions").ToString.Trim.ToLower = "size" And dt.Rows(1)("yesno").ToString.ToLower = "n" Then
                trsizepopup.Visible = False
            Else
                trsizepopup.Visible = True
                getsizevalues()
                btnSelSizeSearch.Visible = True
                txtSizeSearch.Visible = True
                txtSizeSearch.Text = "--All--"

            End If
        End If
    End Sub
    Protected Sub btnSelStyleSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelStyleSearch.Click
        If hdnStyleSearchText.Value.Trim = "" Then
            txtStyleSearch.Text = "--All--"
        Else
            txtStyleSearch.Text = hdnStyleSearchText.Value

        End If
        Dim objstyle As New vbInventory()
        Dim dt As New DataTable
        objstyle.Storeno = ViewState("storeno")
        objstyle.StyleId = IIf(hdnStyleSearch.Value = "", 0, hdnStyleSearch.Value)
        dt = objstyle.fnGetStyleDepartment()
        If dt.Rows.Count > 0 Then
            txtDeptSearch.Text = dt.Rows(0)("dept_desc").ToString.Trim
            hdnDeptSearch.Value = dt.Rows(0)("deptid").ToString.Trim
        End If
    End Sub
    Protected Sub btnSelSizeSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelSizeSearch.Click
        If hdnSizeSearchText.Value.Trim = "" Then
            txtSizeSearch.Text = "--All--"
        Else
            txtSizeSearch.Text = hdnSizeSearchText.Value
        End If
    End Sub

    Protected Sub btnSelVendorSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelVendorSearch.Click
        If hdnVendorSearchText.Value = "" Then
            txtVendorSearch.Text = "--All--"
        Else
            txtVendorSearch.Text = hdnVendorSearchText.Value
        End If
    End Sub
    Protected Sub btnSelDeptSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelDeptSearch.Click
        If hdnDeptSearchText.Value.Trim = "" Then
            txtDeptSearch.Text = "--All--"
        Else
            txtDeptSearch.Text = hdnDeptSearchText.Value

        End If
        If (txtDeptSearch.Text.Replace("--All--", "") <> "" And hdnDeptSearch.Value <> "" And hdnDeptSearch.Value <> "0") Then
            Dim objinv As New vbInventory
            objinv.Storeno = ViewState("storeno")
            Dim check As String = objinv.fngetStyleinfo()
            If check = "" Or check.ToLower = "n" Then
                If hdnDeptSearch.Value <> "" And hdnDeptSearch.Value <> "0" Then
                    Dim deptid As Integer = hdnDeptSearch.Value
                    ViewState("deptid") = deptid
                    If ViewState("styleflag") = "Y" Then
                        btnSelStyleSearch.Visible = True
                        txtStyleSearch.Visible = True
                        hdnStyleSearch.Value = ""
                        txtStyleSearch.Text = "--All--"
                        btnSelStyleSearch.Focus()
                    End If
                Else
                    If ViewState("styleflag") = "Y" Then
                        btnSelStyleSearch.Visible = False
                        txtStyleSearch.Visible = False
                        hdnStyleSearch.Value = ""
                        txtStyleSearch.Text = "--All--"
                        btnSelDeptSearch.Focus()
                    End If

                End If
                If txtStyleSearch.Visible = True Then
                    btnSelStyleSearch.Focus()
                ElseIf txtSizeSearch.Visible = True Then
                    btnSelSizeSearch.Focus()


                End If
            ElseIf check.ToLower = "y" Then
                If txtDeptSearch.Text <> "" And txtDeptSearch.Text <> "--All--" Then
                    Dim deptid As Integer = hdnDeptSearch.Value
                    ViewState("deptid") = deptid
                    Call getstylesearchvalues()
                    If ViewState("styleflag") = "Y" Then
                        btnSelStyleSearch.Visible = True
                        txtStyleSearch.Visible = True
                        hdnStyleSearch.Value = ""
                        txtStyleSearch.Text = "--All--"
                        btnSelStyleSearch.Focus()
                    End If
                Else

                    If ViewState("styleflag") = "Y" Then
                        btnSelStyleSearch.Visible = False
                        txtStyleSearch.Visible = False
                        hdnStyleSearch.Value = ""
                        txtStyleSearch.Text = "--All--"
                        btnSelDeptSearch.Focus()
                    End If
                End If
                If txtStyleSearch.Visible = True Then
                    btnSelStyleSearch.Focus()
                ElseIf txtSizeSearch.Visible = True Then
                    btnSelSizeSearch.Focus()

                End If
            End If
        Else
            ViewState("SearchCriteria") = "All"
            getstylevalues1()
            If ViewState("styleflag") = "Y" Then
                btnSelStyleSearch.Visible = False
                txtStyleSearch.Visible = False
                hdnStyleSearch.Value = ""
                txtStyleSearch.Text = "--All--"
                btnSelDeptSearch.Focus()
            End If
        End If
        txtStyleSearch.Text = "--All--"
        hdnStyleSearch.Value = ""
    End Sub
    Public Sub getstylevalues1()
        Dim objconnection As conClass = New conClass
        objconnection.DBconnect()
        Dim objcommand As SqlCommand = New SqlCommand
        objcommand.CommandType = CommandType.StoredProcedure
        'objcommand.CommandText = "proc_inventory_get_style_values"
        objcommand.CommandText = "getstyledata1"
        objcommand.Connection = objconnection.con1
        objcommand.Parameters.AddWithValue("@storeno", ViewState("storeno"))
        Dim objdataadapter As SqlDataAdapter = New SqlDataAdapter
        Dim objdataset As DataSet = New DataSet
        objdataadapter.SelectCommand = objcommand
        objdataadapter.Fill(objdataset)
        If objdataset.Tables.Count > 0 Then
            If objdataset.Tables(0).Rows.Count > 0 Then
                If ViewState("SearchCriteria") = "All" Then

                    txtStyleSearch.Visible = True
                    btnSelStyleSearch.Visible = True
                    lblNoStyleSearch.Visible = False

                    txtStyleSearch.Text = "--All--"
                    hdnStyleSearch.Value = ""

                Else
                End If
            Else
                txtStyleSearch.Visible = False
                btnSelStyleSearch.Visible = False
                lblNoStyleSearch.Visible = True
            End If

        End If
        objconnection.DBDisconnect()
    End Sub
    Public Sub getstylesearchvalues()
        Dim objconnection As conClass = New conClass
        objconnection.DBconnect()
        Dim objcommand As SqlCommand = New SqlCommand
        objcommand.CommandType = CommandType.StoredProcedure
        objcommand.CommandText = "getstyledata"
        objcommand.Connection = objconnection.con1
        objcommand.Parameters.AddWithValue("@storeno", ViewState("storeno"))
        objcommand.Parameters.AddWithValue("@deptid", ViewState("deptid"))

        Dim objdataadapter As SqlDataAdapter = New SqlDataAdapter
        Dim objdataset As DataSet = New DataSet
        objdataadapter.SelectCommand = objcommand
        objdataadapter.Fill(objdataset)
        If objdataset.Tables.Count > 0 Then
            If objdataset.Tables(0).Rows.Count > 0 Then
                If ViewState("SearchCriteria") = "All" Then

                    txtStyleSearch.Visible = True
                    btnSelStyleSearch.Visible = True
                    lblNoStyleSearch.Visible = False

                    txtStyleSearch.Text = "--All--"
                    hdnStyleSearch.Value = ""

                Else

                End If
            Else


                txtStyleSearch.Visible = True
                btnSelStyleSearch.Visible = True
                lblNoStyleSearch.Visible = True
            End If
        End If
        objconnection.DBDisconnect()
    End Sub
    Public Sub getsizevalues()

        Dim dt As New DataTable
        Dim objinv As New vbInventory
        objinv.Storeno = ViewState("storeno")
        Dim check As String = ""
        check = objinv.fngetsizeinfo()
        If check.ToLower = "y" Then

            Dim objconnection As conClass = New conClass

            objconnection.DBconnect()

            Dim objcommand As SqlCommand = New SqlCommand
            objcommand.CommandType = CommandType.StoredProcedure
            objcommand.CommandText = "proc_inventory_get_size_values"
            objcommand.Connection = objconnection.con1
            objcommand.Parameters.AddWithValue("@storeno", ViewState("storeno"))

            Dim objdataadapter As SqlDataAdapter = New SqlDataAdapter
            Dim objdataset As DataSet = New DataSet
            objdataadapter.SelectCommand = objcommand

            'Try
            objdataadapter.Fill(objdataset)
            If objdataset.Tables.Count > 0 Then
                If objdataset.Tables(0).Rows.Count > 0 Then
                    If ViewState("SearchCriteria") = "All" Then

                        txtSizeSearch.Visible = True
                        btnSelSizeSearch.Visible = True
                        lblNoSizeSearch.Visible = False

                        txtSizeSearch.Text = "--All--"
                        hdnSizeSearch.Value = ""

                    Else

                        txtSizeSearch.Visible = True
                        btnSelSizeSearch.Visible = True
                        txtSizeSearch.Text = "--Select--"
                        hdnSizeSearch.Value = ""
                        lblNoSizeSearch.Visible = False

                    End If

                Else

                    txtSizeSearch.Visible = False
                    btnSelSizeSearch.Visible = False
                    lblNoSizeSearch.Visible = True

                    txtSizeSearch.Visible = False
                    btnSelSizeSearch.Visible = False
                    lblNoSizeSearch.Visible = True
                End If
            End If
            objconnection.DBDisconnect()
        Else

        End If

    End Sub

    Public Sub getstylevalues()
        Dim objconnection As conClass = New conClass

        objconnection.DBconnect()

        Dim objcommand As SqlCommand = New SqlCommand
        objcommand.CommandType = CommandType.StoredProcedure
        'objcommand.CommandText = "proc_inventory_get_style_values"
        objcommand.CommandText = "getstyledata"
        objcommand.Connection = objconnection.con1
        objcommand.Parameters.AddWithValue("@storeno", ViewState("storeno"))
        objcommand.Parameters.AddWithValue("@deptid", ViewState("deptid"))

        Dim objdataadapter As SqlDataAdapter = New SqlDataAdapter
        Dim objdataset As DataSet = New DataSet
        objdataadapter.SelectCommand = objcommand
        objdataadapter.Fill(objdataset)
        If objdataset.Tables.Count > 0 Then
            If objdataset.Tables(0).Rows.Count > 0 Then
                If ViewState("SearchCriteria") = "All" Then
                Else

                End If

            Else

            End If
        End If
        objconnection.DBDisconnect()
    End Sub

    Public Sub getvendorvaluesforsearch()

        Dim objconnection As conClass = New conClass
        ' Dim objreader As SqlDataReader
        objconnection.DBconnect()

        Dim objcommand As SqlCommand = New SqlCommand
        objcommand.CommandType = CommandType.StoredProcedure
        objcommand.CommandText = "proc_inventory_get_vendor_values"
        objcommand.Connection = objconnection.con1
        objcommand.Parameters.AddWithValue("@storeno", ViewState("storeno"))

        Dim objdataadapter As SqlDataAdapter = New SqlDataAdapter
        Dim objdataset As DataSet = New DataSet
        objdataadapter.SelectCommand = objcommand

        Try
            objdataadapter.Fill(objdataset)
            If objdataset.Tables.Count > 0 Then
                If objdataset.Tables(0).Rows.Count > 0 Then
                    'ddlvendorsearch.Visible = True
                    txtVendorSearch.Visible = True
                    btnSelVendorSearch.Visible = True
                    lblvendorsearch.Visible = True
                    'ddlvendorsearch.DataSource = objdataset.Tables(0)
                    'ddlvendorsearch.DataTextField = "company"
                    'ddlvendorsearch.DataValueField = "vendor_id"
                    'ddlvendorsearch.DataBind()
                    txtVendorSearch.Text = "--All--"
                    hdnVendorSearch.Value = ""
                    'ddlvendorsearch.Items.Insert(0, New ListItem("--All--", ""))
                    'ddlvendorsearch.SelectedIndex = 0
                Else
                    btnSelVendorSearch.Visible = False
                    txtVendorSearch.Visible = False
                    'ddlvendor.Visible = False
                    lblvendorsearch.Visible = False

                End If
            End If
        Catch ex As Exception
            Response.Write(ex.ToString)
        Finally
            objconnection.DBDisconnect()
        End Try
    End Sub
    Protected Sub imgInvFind_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles imgInvFind.Click
        'ViewState("CurrentPage") = 1
        Dim objInv As New vbInventory
        Dim dt As New DataTable
        objInv.Storeno = ViewState("storeno")
        objInv.SKU = txtSKUSearch.Text
        objInv.Desc = txtDescSearch.Text.Trim
        objInv.SortColumn = ViewState("invsortcolumn")
        objInv.SortStatus = ViewState("invsortstatus")
        objInv.FirstLimit = ViewState("first_limit")
        objInv.LastLimit = ViewState("last_limit ")
        objInv.checkStock = chkInStockItem.Checked
        objInv.Coupon = ViewState("Coupon")
        objInv.SearchText = txtSearch.Text.Trim
        Dim qty_on_hand As String = txtQtyOnHand.Text.Trim()
        If IsNumeric(qty_on_hand) Then
            objInv.qty_on_hand = txtQtyOnHand.Text.Trim()
        Else
            objInv.qty_on_hand = ""
        End If

        If rdoqtyonhold.Items(0).Selected = True Then
            objInv.QtyCriteria = "="
        ElseIf rdoqtyonhold.Items(1).Selected = True Then
            objInv.QtyCriteria = ">"
        ElseIf rdoqtyonhold.Items(2).Selected = True Then
            objInv.QtyCriteria = "<"
        Else
            objInv.QtyCriteria = "="
        End If

        Dim price As String = txtPriceSearch.Text.Trim.Replace("$", "").Replace(",", "")
        If IsNumeric(price) Then
            objInv.Price = price
        Else
            objInv.Price = ""
        End If
        objInv.points = txtPointSearch.Text.Trim
        If (txtSizeSearch.Text.Replace("--All--", "") <> "" And hdnSizeSearch.Value <> "" And hdnSizeSearch.Value <> "0") Then
            objInv.Size = hdnSizeSearch.Value
        End If
        If (txtStyleSearch.Text.Replace("--All--", "") <> "" And hdnStyleSearch.Value <> "" And hdnStyleSearch.Value <> "0") Then
            objInv.style = hdnStyleSearch.Value
        End If
        If (txtDeptSearch.Text <> "" And txtDeptSearch.Text <> "--All--") Then
            objInv.Department = hdnDeptSearch.Value
        End If

        If (txtVendorSearch.Text.Replace("--All--", "") <> "" And hdnVendorSearch.Value <> "" And hdnVendorSearch.Value <> "0") Then
            objInv.vendorvalue = hdnVendorSearch.Value
        End If
        If (ddlDiscountableSearch.SelectedIndex <> 0) Then
            objInv.Discountable = IIf(ddlDiscountableSearch.SelectedValue = "Yes", "Y", "N")
        End If

        If rbtnSKUBegins.Checked Then
            objInv.SKUCriteria = "begins"
        Else
            objInv.SKUCriteria = "contains"
        End If
        If rbtnDescBegins.Checked Then
            objInv.DescCriteria = "begins"
        Else
            objInv.DescCriteria = "contains"
        End If
        If rdoPrice.Items(0).Selected = True Then
            objInv.PriceCriteria = "="
        ElseIf rdoPrice.Items(1).Selected = True Then
            objInv.PriceCriteria = ">"
        ElseIf rdoPrice.Items(2).Selected = True Then
            objInv.PriceCriteria = "<"
        Else
            objInv.PriceCriteria = "="
        End If
        objInv.Storeno = ViewState("storeno")
        dt = objInv.fnInventoryAdvanceSearch_CustomForClub()
        If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
            ViewState("Advance_Search") = "True"
            ViewState("ID") = Nothing
            gvInventoryList.DataSource = dt
            gvInventoryList.DataBind()
            trinvlist.Style.Add("display", "")
            lblInvMessage.Text = ""
            lblInvMessage.Enabled = False
            lblvalueinventory.Value = "1"
            gvInventoryList.Visible = True
        Else
            lblInvMessage.Text = "No Records Found."
            lblInvMessage.Enabled = True
            lblvalueinventory.Value = "0"
            gvInventoryList.Visible = False
            trinvlist.Style.Add("display", "")
        End If
        lnkbtnInvClearAdvanceSearchResults.Visible = True
        ViewState("SearchCriteria") = Nothing
        hdnPageonindexinv.Value = gvInventoryList.PageSize
        hdnCurrentpageinv.Value = (gvInventoryList.PageIndex + 1).ToString
        hdnPagecountinv.Value = gvInventoryList.PageCount.ToString()
        hdnPagesizeinv.Value = gvInventoryList.Rows.Count.ToString
        hdnBtnidinv.Value = gvInventoryList.ClientID.Replace("_", "$")

        mpeInvAdvanceSearch.Hide()
        txtSearch.Focus()
        updatepanelinventory.Update()
        If ViewState("first_limit") <= 0 Or (ViewState("first_limit") = 1 And ViewState("last_limit ") = 17) Then
            lnkbtnPrev.Enabled = False
        Else
            lnkbtnPrev.Enabled = True
        End If
        If dt.Rows.Count < 17 Then
            lnkbtnNext.Enabled = False
        Else
            lnkbtnNext.Enabled = True
        End If
    End Sub

    Protected Sub imgInvbtnResetForm_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles imgInvbtnResetForm.Click
        clearInvAdvanceControls()
        ViewState("SearchCriteria") = Nothing
        txtSearch.Focus()
        updatepanelinventory.Update()
    End Sub

    Public Sub clearInvAdvanceControls()
        txtSKUSearch.Text = ""
        txtDescSearch.Text = ""
        txtQtyOnHand.Text = "--All--"
        txtPointSearch.Text = ""
        txtPriceSearch.Text = "--All--"
        ddlDiscountableSearch.SelectedIndex = 0
        txtSizeSearch.Text = "--All--"
        hdnSizeSearch.Value = ""
        txtStyleSearch.Text = "--All--"
        hdnStyleSearch.Value = ""
        txtDeptSearch.Text = "--All--"
        hdnDeptSearch.Value = ""
        txtVendorSearch.Text = "--All--"
        hdnVendorSearch.Value = ""
    End Sub
    Protected Sub imgbtnInvback_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles imgbtnInvback.Click
        mpeInvAdvanceSearch.Hide()
        lnkbtnInvClearAdvanceSearchResults.Visible = False
        ViewState("SearchCriteria") = Nothing
        txtSearch.Focus()
        updatepanelinventory.Update()
    End Sub
    Public Sub getSKUInfoForDeptStyle(ByVal value As String)
        '''' to get item details from Inventory table
        Dim dt As New DataTable
        Dim objinv As New vbInventory
        objinv.Storeno = ViewState("storeno")
        objinv.SKU = IIf(ViewState("cmd") = "text", value, ViewState("prod_sku"))
        dt = objinv.fncheckReceivedSKU()

        If dt.Rows.Count > 0 Then



            ViewState("desc") = dt.Rows(0)("desc1").ToString.Trim
            ViewState("prod_sku") = dt.Rows(0)("item_mst_id").ToString.Trim
            ViewState("invSize") = dt.Rows(0)("Size").ToString.Trim
            ViewState("last_cost") = dt.Rows(0)("last_cost").ToString.Trim
            ViewState("stylevalue") = dt.Rows(0)("style").ToString.Trim
            ViewState("price") = dt.Rows(0)("price").ToString.Trim
            ViewState("department") = dt.Rows(0)("dept_desc").ToString.Trim
            ViewState("markup") = dt.Rows(0)("markup").ToString.Trim
            ViewState("deptid") = dt.Rows(0)("dept_id").ToString.Trim

            ViewState("styleid") = dt.Rows(0)("style_id").ToString.Trim
            ViewState("sizeid") = dt.Rows(0)("Size_Id").ToString.Trim


        Else
            ViewState("invsearchcolumn") = "item_mst_id"
            ViewState("Price") = "0.00"
            ViewState("tmprecprice") = "0.00"
            ViewState("department") = "Not Defined"
            ViewState("deptid") = 0
            ViewState("styleid") = 0
            ViewState("stylevalue") = "Not Defined"
            ViewState("markup") = "0.00"
            ViewState("invSize") = "Not Defined"
            ViewState("sizeid") = 0
        End If


    End Sub


    
   
    
    Public Sub getmarkupvalue()
        Dim objconnection As conClass = New conClass
        objconnection.DBconnect()
        Dim objcommand As SqlCommand = New SqlCommand
        objcommand.CommandType = CommandType.StoredProcedure
        objcommand.CommandText = "getstyledata"
        objcommand.Connection = objconnection.con1
        objcommand.Parameters.AddWithValue("@storeno", ViewState("storeno"))
        objcommand.Parameters.AddWithValue("@deptid", ViewState("deptid"))

        Dim objdataadapter As SqlDataAdapter = New SqlDataAdapter
        Dim objdataset As DataSet = New DataSet
        objdataadapter.SelectCommand = objcommand
        objdataadapter.Fill(objdataset)
        If objdataset.Tables(0).Rows.Count > 0 Then
            ViewState("markupvalue") = objdataset.Tables(1).Rows(0)("markup").ToString.Trim
        Else
            ViewState("markupvalue") = "0.00"
        End If
        objconnection.DBDisconnect()
    End Sub

    Protected Sub lnkbtnInvClearAdvanceSearchResults_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkbtnInvClearAdvanceSearchResults.Click
        ViewState("searchtext") = ""
        Call getinventorylist()
        txtSearch.Focus()
    End Sub

    Protected Sub btnEnter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEnter.Click
        Dim Script As String = String.Format("javascript:initializeGridinv();SelectRow($get('" + gvInventoryList.Rows(hdnIndex.Value.ToString.Trim).ClientID + "'), {0});", hdnIndex.Value.ToString.Trim)
        ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "init", Script, True)

        Dim strBrowser As String = Request.UserAgent.ToString()
        
        Session("SpoilageLookupSku") = InpSku.Value
        Dim jscriptString1 As String = ""

        If strBrowser.Contains("Chrome") Then
            jscriptString1 = "<script language='JavaScript' type='text/javascript'>" _
                                        & "window.opener.AddInventoryForSpoilage();" _
                                        & "window.close();" _
                                        & "</script>"
        Else
            jscriptString1 = "<script language='JavaScript' type='text/javascript'>" _
                                        & "window.returnValue='Selectsku';" _
                                        & "window.close();" _
                                        & "</script>"
        End If

        ScriptManager.RegisterStartupScript(Me.Page, Page.GetType(), "jscriptString", jscriptString1, False)

    End Sub

    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        If Request.ServerVariables("http_user_agent").IndexOf("Chrome", StringComparison.CurrentCultureIgnoreCase) <> -1 Then
            Page.ClientTarget = "uplevel"
        End If
        If Request.ServerVariables("http_user_agent").IndexOf("Safari", StringComparison.CurrentCultureIgnoreCase) <> -1 Then
            Page.ClientTarget = "uplevel"
        End If
    End Sub

    Protected Sub chkInvwildcardsearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkInvwildcardsearch.CheckedChanged
        If chkInvwildcardsearch.Checked = True Then
            Session("chkwildcardsearch") = "false"
        Else
            Session("chkwildcardsearch") = Nothing
        End If
        Dim objinv As New vbInventory
        objinv.Storeno = ViewState("storeno")
        objinv.StationNo = Session("stationno")
        objinv.wildcard = IIf(Session("chkwildcardsearch") = "false", 1, 0)
        Dim check As String = objinv.fnUpdateWildCardFlag()
    End Sub

    Protected Sub SearchInv()

        ViewState("CurrentPage") = 1
        ViewState("inventorysearch") = txtSearch.Text.Trim
        hdnsearch.Value = ViewState("inventorysearch")
        If txtSearch.Text.Replace("'", "''").Trim = "" Then
            ViewState("searchtext") = txtSearch.Text.Trim
        Else
            If chkInvwildcardsearch.Checked = True Then
                ViewState("searchtext") = "%" & txtSearch.Text.Replace("'", "''").Trim() & "%"
            Else
                ViewState("searchtext") = txtSearch.Text.Replace("'", "''").Trim()
            End If
        End If
        gvInventoryList.PageIndex = 0
        ViewState("item_mst_id") = Nothing
        ViewState("oldindex") = ViewState("newindex") = 0
        ViewState("first_limit") = 1
        ViewState("last_limit ") = 17

        Call getinventorylist()

        tdinvdiv.Height = 305
        grdinvdiv.Style.Add("height", "305")
        txtSearch.Text = ""
        txtSearch.Focus()
        txtSearch.Text = String.Empty

    End Sub

    Protected Sub chkInStockItem_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkInStockItem.CheckedChanged
        SearchInv()
    End Sub

    Protected Sub chkvendor_CheckedChanged(sender As Object, e As EventArgs) Handles chkvendor.CheckedChanged
        If chkvendor.Checked Then
            If ViewState("invsearch") <> "" Then
                getinventorylistSearch()
            Else
                getinventorylist()
            End If
        Else
            If ViewState("invsearch") <> "" Then
                getinventorylistSearch()
            Else
                getinventorylist()
            End If
        End If
    End Sub

    Protected Sub btnPoursClick_Click(sender As Object, e As EventArgs) Handles btnPoursClick.Click
        If hdnPourSelection.Value.ToString() <> "" AndAlso hdnPourSelection.Value.ToString() <> "undefined" Then

            ViewState("sku") = hdnPourSelection.Value.ToString()
            hdnsku.Value = ViewState("sku")
            hdnPourSelection.Value = ""

            
        End If
    End Sub

End Class


