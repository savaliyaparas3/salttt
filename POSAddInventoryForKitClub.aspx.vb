Imports System
Imports System.Data
Imports System.Data.SqlClient
Partial Class POSAddInventoryForKitClub
    Inherits System.Web.UI.Page
    Dim MyDataTable As New DataTable
    Dim MyDataColumn As New DataColumn
    Public strHelp As String = ""
    Dim strCurrentChar As String = ""
    Dim helpid As Int16 = 0
    Dim dateCondition As String = ""
    Dim Qtyonhand_old As Integer = 0
    Dim objTime As New VbGenralfunctions
    Public receiptptinter As String = ""
    Public intX As String = ""
    Public lastrecord As Boolean = False
    Public firstrecord As Boolean = False
    Dim rowIndex As Integer = 1
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
            ViewState("storeno") = Session("storeno")
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

            Call getSizeinfo()
            gvInventoryHistory.PageSize = 10
            gvInventoryPurchaseHistory.PageSize = 10
            InvMultiview.ActiveViewIndex = 0
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
        'objInventory.SearchColumn = ViewState("invsortcolumn")
        If ViewState("invsortcolumn") <> "" Then
            objInventory.SearchColumn = ViewState("invsortcolumn")
        Else
            objInventory.SearchColumn = ""
        End If
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
                ViewState("itemtype") = dtInventory.Rows(0)("item_type").ToString.Trim.ToUpper
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

        ' 'GC.Collect()
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
            'gvInventoryList.Columns(3).Visible = False
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
            'gvInventoryList.Columns(3).Visible = False
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

        ''GC.Collect()
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
            'gvInventoryList.Columns(3).Visible = False
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
    'Protected Sub gvInventoryList_Init(sender As Object, e As EventArgs) Handles gvInventoryList.Init
    '    Dim objInventory As New vbInventory
    '    Dim dtLook As New DataTable
    '    If ViewState("storeno") Is Nothing Then
    '        ViewState("storeno") = Session("storeno")
    '    End If
    '    objInventory.Storeno = ViewState("storeno").ToString.Trim
    '    dtLook = objInventory.fnInventoryLookup
    '    For i As Integer = 0 To dtLook.Rows.Count - 1
    '        If ViewState("invsortcolumn") Is Nothing And dtLook.Rows(i)("sortOrder").ToString = "*" Then
    '            ViewState("invsortcolumn") = dtLook.Rows(i)("editablecolname").ToString
    '        End If
    '    Next
    'End Sub

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

    Protected Sub gvInventoryList_PreRender(sender As Object, e As EventArgs) Handles gvInventoryList.PreRender
        If ViewState("display_size").ToString().ToLower() = "n" Then
            gvInventoryList.Columns(3).Visible = False
        End If
        If chkItemvendor.Checked = True Then
            gvInventoryList.Columns(0).Visible = False
            gvInventoryList.Columns(1).Visible = True
        Else
            gvInventoryList.Columns(1).Visible = False
            gvInventoryList.Columns(0).Visible = True
        End If
    End Sub

    Protected Sub gvInventoryList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvInventoryList.RowDataBound

        If (e.Row.RowType = DataControlRowType.DataRow) Then
            e.Row.Attributes.Add("onmousedown", (ClientScript.GetPostBackEventReference(gvInventoryList, "Select$" + e.Row.RowIndex.ToString())))

            ' e.Row.Attributes.Add("onclick", "showModalWindowForSku('" & CType(e.Row.FindControl("lblIditem_mst_id"), Label).Text & "')")

            gvInventoryList.Columns(0).Visible = True
            e.Row.Style.Add("cursor", "pointer")

            txtSearch.Attributes.Add("onkeydown", "javascript:return SelectSibling(event);txtSkuKeyhandler(event);")

            e.Row.Attributes("onselectstart") = "javascript:return false;"
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
            Call getSKUdetails()
        Else
            Dim itemtype As String = CType(gvInventoryList.Rows(e.NewSelectedIndex).Cells(1).FindControl("lblitemtype"), Label).Text.Trim()
            If itemtype <> "M" Then


                Session("skukit") = CType(gvInventoryList.Rows(e.NewSelectedIndex).Cells(1).FindControl("lblIditem_mst_id"), Label).Text.Trim()

                'Dim jscriptString1 As String = "<script language='JavaScript' type='text/javascript'>" _
                '                             & "window.returnValue='Selectsku';" _
                '                             & "window.close();" _
                '                             & "</script>"
                'ScriptManager.RegisterStartupScript(Me.Page, Page.GetType(), "jscriptString", jscriptString1, False)

                ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "Returnvalues", "CloseWin()", True)

            Else
                'Dim mainSku As String = CType(gvInventoryList.Rows(e.NewSelectedIndex).Cells(1).FindControl("lblIditem_mst_id"), Label).Text.Trim()
                ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "Returnvalues", "alertmultipack()", True)

            End If
        End If
    End Sub

    Public Sub getSKUdetails()
        '''' to get item details from Inventory table
        Dim dt As New DataTable
        Dim objinv As New vbInventory
        objinv.Storeno = ViewState("storeno")
        objinv.SKU = ViewState("SKU")
        dt = objinv.fncheckReceivedSKU()
        Dim qtyonhand, qtyonhold, qtyonorder As String
        Dim Cases_unit As Boolean
        Dim TotalReceiveingQty As String = "1"
        Dim TotalQtyperCase_1 As String = ""
        Dim TotalQtyperCase_2 As String = ""
        Dim PurchaseOrder As String = ""
        Dim BMUnivCode As String = ""
        If dt.Rows.Count > 0 Then

            BMUnivCode = dt.Rows(0)("BMUnivProd").ToString.Trim
            qtyonhand = dt.Rows(0)("qty_on_hand").ToString.Trim
            qtyonhold = dt.Rows(0)("on_hold").ToString.Trim
            qtyonorder = dt.Rows(0)("on_order").ToString.Trim
            Cases_unit = dt.Rows(0)("Cases_units").ToString.Trim
            ViewState("itemtype") = dt.Rows(0)("item_type").ToString.Trim
            ' mpeQtyperCase.Show()
            'upnlQtyperCase.Update()
            If ViewState("qtypercase") = "Y" AndAlso Cases_unit = False Then
                TotalReceiveingQty = IIf(dt.Rows(0)("QtyReceive").ToString.Trim = 0 Or dt.Rows(0)("QtyReceive").ToString.Trim < 0, "1", dt.Rows(0)("QtyReceive").ToString.Trim)
                TotalQtyperCase_1 = (Convert.ToInt32(Val(TotalReceiveingQty.Trim)) / Convert.ToInt32(Val(IIf(dt.Rows(0)("qtypercase").ToString.Trim = "0", "1", dt.Rows(0)("qtypercase").ToString.Trim)))).ToString.Trim
                TotalQtyperCase_2 = (Convert.ToInt32(Val(TotalReceiveingQty.Trim)) Mod Convert.ToInt32(Val(IIf(dt.Rows(0)("qtypercase").ToString.Trim = "0", "1", dt.Rows(0)("qtypercase").ToString.Trim)))).ToString.Trim
                dvQtyChecked.Visible = True
                dvQtyNotChecked.Visible = False
                'txtDefaultCase.Text = "1"
                txtDefaultCase.Text = IIf(Val(TotalQtyperCase_1) < 0, "1", Math.Floor(Val(TotalQtyperCase_1)))
                txtCaseOf.Text = dt.Rows(0)("qtypercase").ToString.Trim
                txtcaseDollar.Text = dt.Rows(0)("cases").ToString.Trim
                'txtPlus.Text = "0"
                txtPlus.Text = IIf(Val(TotalQtyperCase_2) >= 1, TotalQtyperCase_2, "0")
                If Not Session("TypeOfPageInPOQty") Is Nothing Then
                    If Session("TypeOfPageInPOQty") = "IR" Then
                        txtDefaultCase.Text = "1"
                        txtPlus.Text = "0"
                    End If
                End If
                If Val(dt.Rows(0)("last_cost").ToString.Trim) = 0 Then
                    If Val(dt.Rows(0)("cost").ToString.Trim) <> 0 Then
                        txtUnitDollar.Text = dt.Rows(0)("cost").ToString.Trim
                    Else
                        txtUnitDollar.Text = dt.Rows(0)("last_cost").ToString.Trim
                    End If
                Else
                    txtUnitDollar.Text = dt.Rows(0)("last_cost").ToString.Trim
                End If
                txttotalingQtyChecked.Text = Math.Abs(CType(FormatNumber((Val(txtDefaultCase.Text.Trim) * converttofloat(txtcaseDollar.Text.Trim)) + (Val(txtPlus.Text.Trim) * converttofloat(txtUnitDollar.Text.Trim)), 4, TriState.True), Decimal))
                lblQtyPerCaseHeader.Text = "Purchase :" & dt.Rows(0)("desc1").ToString.Trim & " " & dt.Rows(0)("Size").ToString.Trim
                txtDefaultCase.Focus()
                txtDefaultCase.Focus()
                '  ScriptManager.RegisterStartupScript(Me, Me.GetType(), "FocusScript", "setTimeout(function(){$get('" + txtDefaultCase.ClientID + "').focus();}, 50);", True)

                txtQtytoReceive.Text = "0"
                txtCostEa.Text = "0"
                txtTotaling.Text = "0"
            Else

                dvQtyChecked.Visible = False
                dvQtyNotChecked.Visible = True
                'txtQtytoReceive.Text = "1"
                txtQtytoReceive.Text = IIf(dt.Rows(0)("QtyReceive").ToString.Trim = 0 Or dt.Rows(0)("QtyReceive").ToString.Trim < 0, "1", dt.Rows(0)("QtyReceive").ToString.Trim)
                If Not Session("TypeOfPageInPOQty") Is Nothing Then
                    If Session("TypeOfPageInPOQty") = "IR" Then
                        txtQtytoReceive.Text = "1"
                    End If
                End If
                If Val(dt.Rows(0)("last_cost").ToString.Trim) = 0 Then
                    If Val(dt.Rows(0)("cost").ToString.Trim) <> 0 Then
                        txtCostEa.Text = dt.Rows(0)("cost").ToString.Trim
                    Else
                        txtCostEa.Text = dt.Rows(0)("last_cost").ToString.Trim
                    End If
                Else
                    txtCostEa.Text = dt.Rows(0)("last_cost").ToString.Trim
                End If
                If Val(dt.Rows(0)("last_cost").ToString.Trim) = 0 Then
                    If Val(dt.Rows(0)("cost").ToString.Trim) <> 0 Then
                        txtTotaling.Text = Convert.ToInt32(Val(dt.Rows(0)("cost").ToString.Trim)) * Convert.ToInt32(Val(txtQtytoReceive.Text.Trim))
                    Else
                        txtTotaling.Text = Convert.ToInt32(Val(dt.Rows(0)("last_cost").ToString.Trim)) * Convert.ToInt32(Val(txtQtytoReceive.Text.Trim))
                    End If
                Else
                    txtTotaling.Text = Convert.ToInt32(Val(dt.Rows(0)("last_cost").ToString.Trim)) * Convert.ToInt32(Val(txtQtytoReceive.Text.Trim))
                End If
                lblQtyPerCaseHeader.Text = "Purchase :" & dt.Rows(0)("desc1").ToString.Trim
                txtQtytoReceive.Focus()
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "FocusScript", "setTimeout(function(){$get('" + txtQtytoReceive.ClientID + "').focus();}, 50);", True)
            End If
            ViewState("desc") = dt.Rows(0)("desc1").ToString.Trim
            ViewState("prod_sku") = dt.Rows(0)("item_mst_id").ToString.Trim
            ViewState("invSize") = Replace(dt.Rows(0)("Size").ToString.Trim, "'", "\'")
            ViewState("last_cost") = dt.Rows(0)("last_cost").ToString.Trim
            ViewState("price") = dt.Rows(0)("price").ToString.Trim
            ViewState("department") = dt.Rows(0)("dept_desc").ToString.Trim
            ViewState("markup") = dt.Rows(0)("markup").ToString.Trim
            ViewState("Vend_item_id") = dt.Rows(0)("Vend_item_id").ToString.Trim
            ViewState("Vend_item_idE") = HttpUtility.UrlEncode(dt.Rows(0)("Vend_item_id").ToString.Trim)
        Else
            qtyonhand = 0
            qtyonhold = 0
            qtyonorder = 0
            Cases_unit = False
        End If

        If String.IsNullOrEmpty(Request.QueryString("PurchaseOrderInventory")) = False Then
            PurchaseOrder = Convert.ToString(Request.QueryString("PurchaseOrderInventory"))
        End If


        'txtDefaultCase.Focus()
        ' ScriptManager.RegisterStartupScript(Me, Me.GetType(), "FocusScript", "setTimeout(function(){$get('" + txtDefaultCase.ClientID + "').focus();}, 50);", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "test", "showModalWindowForSku('" & ViewState("SKU").Replace("'", "\'") & "', ' " & txtDefaultCase.Text & " ',' " & txtCaseOf.Text & " ',' " & txtcaseDollar.Text & " ',' " & txtPlus.Text & " ',' " & txtUnitDollar.Text & " ',' " & txttotalingQtyChecked.Text & " ','" & ViewState("qtypercase") & "','" & HttpUtility.UrlEncode(ViewState("desc")).ToString.Replace("'", "\'") & "','" & ViewState("prod_sku").Replace("'", "\'") & "','" & ViewState("invSize") & "','" & ViewState("last_cost") & "','" & ViewState("price") & "','" & HttpUtility.UrlEncode(ViewState("department")).ToString().Replace("'", "\'") & "','" + ViewState("markup") + "','" + txtQtytoReceive.Text + "','" + txtCostEa.Text + "','" + txtTotaling.Text + "','" + txtTotaling.Text + "','" + qtyonhand.ToString().Trim + "','" & ViewState("Vend_item_idE").Replace("'", "\'") & "','" & qtyonhold & "','" & qtyonorder & "','" & Cases_unit & "','" & ViewState("itemtype") & "','" & PurchaseOrder & "','" & BMUnivCode & "');", True)
        'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "test", "showModalWindowForSku('" & ViewState("SKU") & "', ' " & txtDefaultCase.Text & " ',' " & txtCaseOf.Text & " ',' " & txtcaseDollar.Text & " ',' " & txtPlus.Text & " ',' " & txtUnitDollar.Text & " ',' " & txttotalingQtyChecked.Text & " ','" & ViewState("qtypercase") & "','" & HttpUtility.UrlEncode(ViewState("desc")).ToString.Replace("'", "\'") & "','" & ViewState("prod_sku") & "','" & ViewState("invSize") & "','" & ViewState("last_cost") & "','" & ViewState("price") & "','" & HttpUtility.UrlEncode(ViewState("department")).ToString().Replace("'", "\'") & "','" + ViewState("markup") + "');", True)

        'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "test", "alert('test');", True)



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

                    lblStyleddl.Visible = True
                    btnSelectStyle.Visible = True
                End If

                lblStyleddl.Visible = True
                btnSelectStyle.Visible = True
            Else

                lblStyleddl.Visible = False
                btnSelectStyle.Visible = False


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
            hdnDept.Value = dt.Rows(0)("dept_id").ToString.Trim
            ViewState("styleid") = dt.Rows(0)("style_id").ToString.Trim
            ViewState("sizeid") = dt.Rows(0)("Size_Id").ToString.Trim

            upnlQtyperCase.Update()
        Else
            ViewState("invsearchcolumn") = "item_mst_id"
            ViewState("Price") = "0.00"
            ViewState("tmprecprice") = "0.00"
            ViewState("department") = "Not Defined"
            hdnDept.Value = 0
            ViewState("deptid") = 0
            ViewState("styleid") = 0
            ViewState("stylevalue") = "Not Defined"
            ViewState("markup") = "0.00"
            ViewState("invSize") = "Not Defined"
            ViewState("sizeid") = 0
        End If


    End Sub


    Protected Sub imgOKQtyPerCase_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles imgOKQtyPerCase.Click
        'Session("Inv") = Nothing
        ''Note for the Developers. if you want to print "♠" special character then press Alt + 123654.
        'Dim Str As String
        'Str = ViewState("price") & "♠" & ViewState("desc") & "♠" & ViewState("invSize") & "♠" & ViewState("prod_sku") & "♠"
        'If ViewState("qtypercase") = "Y" Then
        '    Str += txtDefaultCase.Text & "♠" & txtUnitDollar.Text & "♠" & txtcaseDollar.Text & "♠" & txtCaseOf.Text & "♠" & txtPoTotal.Text & "♠" & txtPlus.Text & "♠" & txttotalingQtyChecked.Text & "♠" & ViewState("last_cost").ToString()
        'Else
        '    Str += txtCostEa.Text & "♠" & txtQtytoReceive.Text & "♠" & txtTotaling.Text & "♠" & ViewState("last_cost").ToString()
        'End If
        'Session("Inv") = Str
        'mpeQtyperCase.Hide()
        Dim Str() As String

        Str = Session("Inv").ToString.Trim.Split("♠")
        ViewState("prod_sku") = Str(3).Trim
        Dim recommPrice As Double
        Dim price, oldcost, recomMarkup As String
        getSKUInfoForDeptStyle(ViewState("prod_sku"))
        recomMarkup = ViewState("markup")
        price = ViewState("price")
        If ViewState("qtypercase") = "Y" Then

            hdnLastCost.Value = Str(5).Trim
          
        Else
            hdnLastCost.Value = Str(4).Trim
           
        End If

        ''''''''
        If ViewState("IsCorpLockDown") = "Y" AndAlso Session("co_storeno").ToString <> "" Then
        Else
            If price = "0.00" Or ViewState("department") = "Not Defined" Or (ViewState("stylevalue") = "Not Defined" And ViewState("styleflag") <> "N") Or ((ViewState("invSize") = "Not Defined" Or ViewState("invSize") = "") And ViewState("size") <> "N") Then
                'If objinv.fnCheckInvPrice(ViewState("storeno").ToString.Trim, ViewState("prod_sku").ToString.Trim) = False Then

                If ViewState("itemtype") = "P" Or ViewState("itemtype") = "M" Then
                    Label4.Text = "Cost Per Ounce:"
                    trpriceeach.Attributes.Add("style", "display:none")
                    If ViewState("itemtype") = "P" Then
                        trsize.Attributes.Add("style", "display:none")
                    Else
                        'trsize.Attributes.Add("style", "display:block")
                    End If

                Else
                    Label4.Text = "Cost:"
                    trpriceeach.Attributes.Add("style", "display:block")
                    'trsize.Attributes.Add("style", "display:block")
                End If
                'mpeSetPrice.Show()
                'btnOKSetPrice.Focus()
                recommPrice = IIf(recomMarkup = "0.00", "0.00", (hdnLastCost.Value * recomMarkup / 100) + hdnLastCost.Value)
                lblItemNameSize.Text = ViewState("desc")
                lblCostEach.Text = IIf(hdnLastCost.Value = "", "0.00", "$" & FormatNumber(hdnLastCost.Value.Trim, 2))

                If price <> "0.00" Or ((ViewState("itemtype") = "P" Or ViewState("itemtype") = "M") AndAlso (ViewState("department") = "Not Defined" Or (ViewState("stylevalue") = "Not Defined" And ViewState("styleflag") <> "N") Or ((ViewState("invSize") = "Not Defined" Or ViewState("invSize") = "") And ViewState("size") <> "N"))) Then
                    lblcurrprice.Text = "The item you are receiving is missing some information. Please complete the form below."
                    trcosteach.Visible = False
                    trpriceeach.Visible = False
                    btnOKSetPrice.Text = "OK"
                    mpeSetPrice.Show()
                    btnOKSetPrice.Focus()
                ElseIf ViewState("itemtype") <> "P" AndAlso ViewState("itemtype") <> "M" Then
                    lblcurrprice.Text = "Your current price is set to $ 0.00. Please set your selling price:"
                    trcosteach.Visible = True
                    trpriceeach.Visible = True
                    btnOKSetPrice.Text = "Set Price"
                    If ViewState("itemtype") = "P" Then
                        btnOKSetPrice.Text = "OK"
                    End If
                    mpeSetPrice.Show()
                    btnOKSetPrice.Focus()
                Else
                    Dim jscriptString1 As String = "<script language='JavaScript' type='text/javascript'>"
                    jscriptString1 += " window.opener.document.getElementById('ctl00_ContentPlaceHolder2_btn2').click();"
                    jscriptString1 += "self.focus();"
                    jscriptString1 += "</script>"
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "jscriptString", jscriptString1, False)
                    'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "FocusScript", "setTimeout(function setfocus(), 50);", True)
                    txtSearch.Focus()
                    Exit Sub
                End If

                If ViewState("department") = "Not Defined" Then
                    btnSelectDept.Visible = True

                    lblDeptddl.Text = "--Select--"

                Else

                    btnSelectDept.Visible = False
                    lblDeptddl.Text = ViewState("department")
                End If
                If ViewState("deptid") <> 0 Then
                    If ViewState("stylevalue") = "Not Defined" Then

                        btnSelectStyle.Visible = True
                        lblStyleddl.Visible = True
                        lblStyleddl.Text = "--Select--"
                    Else

                        lblStyleddl.Visible = True
                        lblStyleddl.Text = ViewState("stylevalue")
                        btnSelectStyle.Visible = False


                    End If
                Else

                    btnSelectStyle.Visible = False
                    lblStyleddl.Visible = False
                End If

                If ViewState("styleflag") <> "N" Then
                    trstyle.Visible = True
                Else
                    trstyle.Visible = False
                End If

                If ViewState("invSize") = "Not Defined" Or ViewState("invSize") = "" Then

                    btnSelectSize.Visible = True
                    lblSizeddl.Visible = True
                    lblSizeddl.Text = "--Select--"
                Else

                    lblSizeddl.Visible = True
                    lblSizeddl.Text = ViewState("invSize")
                    btnSelectSize.Visible = False


                End If
                If ViewState("size") <> "N" Then
                    trsize.Visible = True
                Else
                    trsize.Visible = False
                End If

                lblsku.Text = ViewState("prod_sku")
                lblDescription.Text = ViewState("desc")
                'txtRecomPrice.Text = IIf(ViewState("department") = "Not Defined", "0.00", "$" & Math.Abs(CType(FormatNumber(recommPrice, 2, TriState.True), Decimal)))
                txtRecomPrice.Text = If(price = "0.00", "$" & Math.Abs(CType(FormatNumber(recommPrice, 2, TriState.True), Decimal)), "$" & Math.Abs(CType(FormatNumber(price, 2, TriState.True), Decimal)))
                upnlSetPrice.Update()
                txtRecomPrice.Focus()
                'End If
                btnOKSetPrice.Focus()
                Exit Sub
            End If
        End If
        '''''''


        Dim jscriptString As String = "<script language='JavaScript' type='text/javascript'>"
        jscriptString += " window.opener.document.getElementById('ctl00_ContentPlaceHolder2_btn2').click();"
        jscriptString += "self.focus();"
        jscriptString += "</script>"
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "jscriptString", jscriptString, False)
        'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "FocusScript", "setTimeout(function setfocus(), 50);", True)
        txtSearch.Focus()
    End Sub
    Protected Sub btnOKSetPrice_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOKSetPrice.Click
        '''' To set Price of an Item in inventory table
        mpeSetPrice.Hide()
        Dim objinv As New vbInventory
        objinv.SKU = ViewState("prod_sku")
        objinv.Storeno = ViewState("storeno")
        objinv.StationNo = ViewState("stationno")
        If trcosteach.Visible = True Then
            ViewState("tmpprice") = txtRecomPrice.Text.Replace("$", "")
            objinv.Price = txtRecomPrice.Text.Replace("$", "")
            objinv.fnUpdateInventorytmpPrice()
            Session("Inv") = Session("Inv").ToString.Remove(0, Session("Inv").ToString.IndexOf("♠"))
            Session("Inv") = ViewState("tmpprice") + Session("Inv")
        End If


        If btnSelectDept.Visible = True Then
            objinv.Department = hdnDept.Value
        Else
            objinv.Department = ViewState("deptid")
        End If
        If btnSelectStyle.Visible = True Then
            objinv.style = hdnStyle.Value
        Else
            objinv.style = ViewState("styleid")
        End If
        If btnSelectSize.Visible = True Then
            objinv.Size = IIf(IsNumeric(hdnSize.Value.Trim), hdnSize.Value, "0")
        Else
            objinv.Size = ViewState("sizeid")
        End If
        objinv.Desc = ViewState("desc")
        objinv.user = Session("Emp_Alert_empid")

        objinv.fnInsertInventorySecurityLog()

        objinv.fnUpdateInventoryDeptStyle()

        'getReceiveProductlist()
        Dim jscriptString As String = "<script language='JavaScript' type='text/javascript'>"
        jscriptString += " window.opener.document.getElementById('ctl00_ContentPlaceHolder2_btn2').click();"
        jscriptString += "self.focus();"
        jscriptString += "</script>"
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "jscriptString", jscriptString, False)
        'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "FocusScript", "setTimeout(function setfocus(), 50);", True)
        txtSearch.Focus()

    End Sub
    Protected Sub btnSelectDept_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelectDept.Click
        Dim objinv As New vbInventory
        objinv.Storeno = ViewState("storeno")
        Dim check As String = objinv.fngetStyleinfo()
        If hdnDeptText.Value.Trim = "" Then
            lblDeptddl.Text = "--Select--"

        Else
            lblDeptddl.Text = hdnDeptText.Value
        End If
        If hdnDept.Value = "" Or hdnDept.Value = "0" Then

            btnSelectStyle.Visible = False
            lblStyleddl.Visible = False
            hdnStyle.Value = ""
            lblStyleddl.Text = "--Select--"
            txtRecomPrice.Text = ViewState("price")
            btnSelectDept.Focus()

            Exit Sub
        End If
        If check = "" Or check.ToLower = "n" Then
            trstyle.Visible = False
            If lblDeptddl.Text <> "" And lblDeptddl.Text <> "--Select--" Then
                Dim deptid As Integer = hdnDept.Value
                ViewState("deptid") = deptid
            End If
            If lblStyleddl.Visible = True Then
                btnSelectStyle.Focus()
            Else
                txtRecomPrice.Focus()
            End If
        ElseIf check.ToLower = "y" Then
            trstyle.Visible = True
            If lblDeptddl.Text <> "" And lblDeptddl.Text <> "--Select--" Then
                Dim deptid As Integer = hdnDept.Value
                ViewState("deptid") = deptid
                Call getstylevalues()


            End If
            If lblStyleddl.Visible = True Then
                btnSelectStyle.Focus()

            Else
                txtRecomPrice.Focus()
            End If
        End If
        hdnStyle.Value = "0"
        lblStyleddl.Text = "--Select--"
        getmarkupvalue()
        Dim price, oldcost, recomMarkup As String
        Dim recommPrice As String
        price = ViewState("price")
        Dim costvalue As String = lblCostEach.Text.ToString.Trim.Replace("$", "")
        oldcost = IIf(costvalue = "", "0.00", costvalue)
        recomMarkup = ViewState("markupvalue")
        If hdnDept.Value <> "" AndAlso hdnDept.Value <> 0 AndAlso ViewState("markupvalue") <> "0.00" Then
            recommPrice = IIf(recomMarkup = "0.00", "0.00", (oldcost * recomMarkup / 100) + oldcost)
            txtRecomPrice.Text = FormatNumber(recommPrice, 2, TriState.True)
        Else
            txtRecomPrice.Text = "0.00"
        End If
    End Sub

    Protected Sub btnSelectStyle_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelectStyle.Click
        If hdnStyleText.Value.Trim = "" Then
            lblStyleddl.Text = "--Select--"
        Else
            lblStyleddl.Text = hdnStyleText.Value

        End If
    End Sub

    Protected Sub btnSelectSize_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelectSize.Click
        If hdnSizeText.Value.Trim = "" Then
            lblSizeddl.Text = "--Select--"
        Else
            lblSizeddl.Text = hdnSizeText.Value

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
        If ViewState("isInv") = "Y" Then
            ViewState("SKU") = InpSku.Value
            Call getSKUdetails()
        Else
            Session("skukit") = InpSku.Value
            Dim jscriptString1 As String = "<script language='JavaScript' type='text/javascript'>" _
                                         & "window.returnValue='Selectsku';" _
                                         & "window.close();" _
                                         & "</script>"
            ScriptManager.RegisterStartupScript(Me.Page, Page.GetType(), "jscriptString", jscriptString1, False)
        End If
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
    Protected Sub getSizeinfo()
        Dim objconnection As conClass = New conClass

        'create sqlcommand
        objconnection.DBconnect()
        Dim objcommand As SqlCommand = New SqlCommand

        objcommand.CommandType = CommandType.StoredProcedure
        objcommand.Connection = objconnection.con1
        objcommand.CommandText = "proc_sizeinfo"
        objcommand.Parameters.AddWithValue("@storeno", IIf(IsDBNull(Session("storeno")), 0, Session("storeno")))
        Dim objdataadapter As New SqlDataAdapter
        Dim objDataset As New DataSet
        objdataadapter.SelectCommand = objcommand

        objdataadapter.Fill(objDataset, "inv_layout_mst")

        If objDataset.Tables(0).Rows.Count > 0 Then
            ViewState("display_size") = IIf(IsDBNull(objDataset.Tables(0).Rows(0).Item("yesno")), "N", objDataset.Tables(0).Rows(0).Item("yesno").ToString.Trim)
        Else
            ViewState("display_size") = "N"
        End If

    End Sub
    Protected Sub lnk_ClickNew(ByVal s As Object, ByVal e As CommandEventArgs)
        If e.CommandArgument = 1 Then
            lnkList.CssClass = "style8black"
            lnkSalesHistory.CssClass = "style8"
            lnkPurchseHistory.CssClass = "style8"
            InvMultiview.ActiveViewIndex = 0
            Call getinventorylist()
        ElseIf e.CommandArgument = 2 Then
            Call getinventoryid()
            lnkList.CssClass = "style8"
            lnkSalesHistory.CssClass = "style8black"
            lnkPurchseHistory.CssClass = "style8"
            InvMultiview.ActiveViewIndex = 1
            Call SalesHistory()
            Call bindview(3)
        ElseIf e.CommandArgument = 3 Then
            Call getinventoryid()
            lnkList.CssClass = "style8"
            lnkSalesHistory.CssClass = "style8"
            lnkPurchseHistory.CssClass = "style8black"
            InvMultiview.ActiveViewIndex = 2
            Call Check_inv_type()
            Call PurchaseHistory()
            Call bindview(4)
        End If
    End Sub
#Region "Sales History"
    Protected Sub bindview(ByVal id As String)
        
        If id = 3 Then
            lnkSalesHistory.CssClass = "style8black"
            InvMultiview.ActiveViewIndex = 1
            mulSalesWeeklyGraphHistory.ActiveViewIndex = 0
            pnlSalesWeeklyGraph.Visible = True
            rdoSalesWeeklyGraph.Visible = True
            rdoSalesWeeklyGraph.Items(0).Selected = True
            rdoSalesWeeklyGraph.Items(1).Selected = False
            rdoMonthSummary.Visible = True
            rdoMonthSummary.Items(0).Selected = False
            rdoMonthSummary.Items(1).Selected = False
            rdoMonthSummary.Items(2).Selected = False
            rdoMonthSummary.Items(3).Selected = False
            rdoMonthSummary.Items(4).Selected = True


            dlWeeklySales.Visible = False
            DateRange.Visible = False
            imgbtnsearch.Visible = False
            gvInventoryHistory.Visible = False
            partssaleshistory.Visible = True
            rdoGraph.Visible = True
            imgbtnReturn.Visible = False
            chkReturnQty.Visible = False
            chkPromoSalesQty.Visible = False
            Dim ShowinStock As Boolean = False
            
            If ViewState("itemtype") = "P" Or ViewState("itemtype") = "M" Then
                ddlPoursSubItem.Visible = False
                lblPoursSubItem.Visible = False
                ddlPoursSubItem1.Visible = True
                lblPoursSubItem1.Visible = True

                Call FillPours_SubItem()
            Else
                ddlPoursSubItem1.Visible = False
                lblPoursSubItem1.Visible = False
                ddlPoursSubItem.Visible = False
                lblPoursSubItem.Visible = False
            End If
            If rdoGraph.Items(0).Selected = True Then
                ViewState("graphtype") = "unit"
                If ViewState("qtypercase").ToString.ToUpper() = "Y" Then
                    If ViewState("QtyPerCaseAmt") IsNot Nothing Then
                        If Val(ViewState("QtyPerCaseAmt").ToString()) > 0 Then
                            chkShowinCase.Visible = True
                            If chkShowinCase.Checked = True Then
                                ShowinStock = True
                            Else
                                ShowinStock = False
                            End If

                        Else
                            chkShowinCase.Visible = False
                            chkShowinCase.Checked = False
                            ShowinStock = False

                        End If
                    Else
                        chkShowinCase.Visible = False
                        chkShowinCase.Checked = False
                        ShowinStock = False
                    End If
                Else
                    chkShowinCase.Visible = False
                    chkShowinCase.Checked = False
                    ShowinStock = False
                End If
            ElseIf rdoGraph.Items(1).Selected = True Then
                ViewState("graphtype") = "revenue"
                chkShowinCase.Visible = False
                chkShowinCase.Checked = False
            ElseIf rdoGraph.Items(2).Selected = True Then
                ViewState("graphtype") = "profitability"
                chkShowinCase.Visible = False
                chkShowinCase.Checked = False
            Else
                ViewState("graphtype") = "revenue"
                chkShowinCase.Visible = False
                chkShowinCase.Checked = False
            End If

            Dim sku As String = HttpUtility.UrlEncode(ViewState("item_mst_id").ToString.Trim())
            Dim storeno As String
            If Convert.ToBoolean(Session("IsCorpOffice")) = True Then
                storeno = HttpUtility.UrlEncode(ViewState("LocationStoreno").ToString.Trim)
            Else
                storeno = HttpUtility.UrlEncode(ViewState("storeno"))
            End If

            Dim Graphtype As String = ViewState("graphtype")
            Dim type As String = ViewState("itemtype")
            Dim graphfor As String = ""
            If ViewState("itemtype").ToString.Trim = "P" Or ViewState("itemtype").ToString.Trim = "M" Then
                graphfor = ViewState("itemtype").ToString.Trim
            ElseIf ViewState("itemtype").ToString.Trim().ToUpper() = "S" Or ViewState("itemtype").ToString.Trim().ToUpper() = "N" Then
                graphfor = ViewState("itemtype").ToString.Trim
            Else
                graphfor = "I"
            End If
            Dim poursitemsubid As String = ""
            If ViewState("itemtype") = "P" Or ViewState("itemtype") = "M" Then
                poursitemsubid = IIf(ddlPoursSubItem1.SelectedIndex <> 0, (ddlPoursSubItem1.SelectedItem.Value).ToString, "0")
            Else
                poursitemsubid = "0"
            End If
            If rdoGraph.Items(0).Selected = True Then
                partssaleshistory.Attributes.Add("src", "POSInventoryGraphRpt.aspx?storeno=" + storeno + "&mode=S&type=qty&graphfor=" & graphfor.Trim & "&sku=" + sku + "&itemtype=" + ViewState("itemtype") + "&graphtype=" + ViewState("graphtype") + "&ShowInStock=" + ShowinStock.ToString() + "&poursid=" + poursitemsubid)
            ElseIf rdoGraph.Items(1).Selected = True Then
                partssaleshistory.Attributes.Add("src", "POSInventoryGraphRpt.aspx?storeno=" + storeno + "&mode=S&type=revenue&graphfor=" & graphfor.Trim & "&sku=" + sku + "&itemtype=" + ViewState("itemtype") + "&graphtype=" + ViewState("graphtype") + "&poursid=" + poursitemsubid)
                gvInventoryHistory.PageIndex = 0
                lnkSalesHistory.CssClass = "style8black"
                
            ElseIf rdoGraph.Items(2).Selected = True Then
                'partssaleshistory.Attributes.Add("src", "POSInventoryGraphRpt.aspx?storeno=" + storeno + "&mode=S&type=profitability&graphfor=" & graphfor.Trim & "&sku=" + sku + "&itemtype=" + ViewState("itemtype") + "&graphtype=" + ViewState("graphtype") + "&poursid=" + poursitemsubid)
                FillProfitabilityMonth()
                Profitabilityflow()
            End If
        ElseIf id = 4 Then
            InvMultiview.ActiveViewIndex = 2
            trpurchasehistory.Visible = True
            imgbtnpurchasefind.Visible = True
            divpurchasedata.Visible = True
            rdopurchasesummary.Items(0).Selected = False
            rdopurchasesummary.Items(1).Selected = False
            rdopurchasesummary.Items(2).Selected = False
            rdopurchasesummary.Items(3).Selected = True
            rdopurchasesummary.Items(4).Selected = False
            chkShowinCase.Visible = False
        End If

    End Sub
    Public Sub SalesHistory()
        Dim strfirstdate As DateTime
        Dim mnth As String = ""
        mnth = DateTime.Now.Month.ToString()
        strfirstdate = mnth + "/1/" + DateTime.Now.Year.ToString()
        strfirstdate = DateAdd(DateInterval.Month, -2, strfirstdate)
        ViewState("SalesStartDate") = strfirstdate.ToString("MM/dd/yyyy")
        strfirstdate = DateTime.Now.Month.ToString() + "/1/" + DateTime.Now.Year.ToString()
        strfirstdate = DateAdd(DateInterval.Month, 1, strfirstdate)
        ViewState("SalesEndDate") = Date.Now.ToString("MM/dd/yyyy")
        txtSalesFrom.Text = ViewState("SalesStartDate")
        txtSalesTo.Text = ViewState("SalesEndDate")
        Call GetSalesHistory(ViewState("SalesStartDate"), ViewState("SalesEndDate"))
        ViewState("sortstatus1") = "ASC"
        ViewState("sortcolumn1") = "inv_date"
    End Sub
    Protected Sub getinventoryid()
        'Dim datarow As GridViewRow
        For Each datarow In gvInventoryList.Rows
            Dim chkbox As CheckBox = CType(datarow.Cells(0).FindControl("chkColumn"), CheckBox)
            If chkbox.Checked = True Then
                ViewState("item_mst_id") = CType(datarow.Cells(1).FindControl("lblIditem_mst_id"), Label).Text.Trim()
                ViewState("name") = CType(datarow.Cells(2).FindControl("lbldesc1"), Label).Text.Trim
                ViewState("Email_alert_desc1") = CType(datarow.FindControl("lbldesc1"), Label).Text.Trim
                'ViewState("QtyPerCase") = CType(DataRow.FindControl("lblQtyperCase"), Label).Text.Trim
                Exit For
            End If
        Next
    End Sub
    Protected Sub GetSalesHistory(ByVal startdate As String, ByVal enddate As String)
        ''Protected Sub GetSalesHistory()
        Dim objTime As New VbGenralfunctions
        Dim objconnection As conClass = New conClass
        objconnection.DBconnect()
        '' create sqlcommand
        Dim objcommand As SqlCommand = New SqlCommand
        objcommand.CommandType = CommandType.StoredProcedure
        objcommand.Connection = objconnection.con1
        objcommand.CommandText = "proc_inventory_get_saleshistory"
        '' parameters declaration
        If Convert.ToBoolean(Session("IsCorpOffice")) = True Then
            objcommand.Parameters.AddWithValue("@storeno", ViewState("LocationStoreno"))
        Else
            objcommand.Parameters.AddWithValue("@storeno", ViewState("storeno"))
        End If
        objcommand.Parameters.AddWithValue("@sku", ViewState("item_mst_id").ToString.Trim)
        objcommand.Parameters.AddWithValue("@startdate", objTime.GetTimeZoneDate(startdate, True, False))
        objcommand.Parameters.AddWithValue("@enddate", objTime.GetTimeZoneDate(enddate, True, True))
        If ViewState("itemtype") = "P" Or ViewState("itemtype").ToString.Trim = "M" Then
            If ddlPoursSubItem.SelectedIndex <> -1 AndAlso ddlPoursSubItem.SelectedIndex <> 0 Then
                objcommand.Parameters.AddWithValue("@poursitemsubid", IIf(ddlPoursSubItem.SelectedIndex <> 0, ddlPoursSubItem.SelectedItem.Value, 0))
            End If

        Else
            objcommand.Parameters.AddWithValue("@poursitemsubid", 0)
        End If
        If rdoMonthSummary.Items(3).Selected = True Then
            ViewState("SalesStartDate") = startdate
            ViewState("SalesEndDate") = enddate
        End If

        If chkReturnQty.Checked = True Then
            objcommand.Parameters.AddWithValue("@return", "Y")
            ViewState("ReturnItem") = "Y"
        Else
            objcommand.Parameters.AddWithValue("@return", "N")
            ViewState("ReturnItem") = "N"
        End If
        If chkPromoSalesQty.Checked = True Then
            objcommand.Parameters.AddWithValue("@promo", "Y")
            ViewState("PromoItem") = "Y"
        Else
            objcommand.Parameters.AddWithValue("@promo", "N")
            ViewState("PromoItem") = "N"
        End If

        Dim objdataadapter As SqlDataAdapter = New SqlDataAdapter
        Dim objdataset As DataSet = New DataSet
        objdataadapter.SelectCommand = objcommand


        MyDataTable = New DataTable("detail")
        MyDataColumn = New DataColumn("SrNo.", GetType(Integer))

        MyDataColumn.AutoIncrement = True
        MyDataColumn.AutoIncrementSeed = 1
        MyDataTable.Columns.Add(MyDataColumn)
        objdataset.Tables.Add(MyDataTable)
        objdataadapter.Fill(objdataset, "detail")

        If objdataset.Tables(0).Rows.Count > 0 Then
            objTime.GetTimeByZone(objdataset.Tables(0), "inv_date1", VbGenralfunctions.AllDateFormat.DateFormat1)
            gvInventoryHistory.DataSource = objdataset
            gvInventoryHistory.DataBind()
        Else
            gvInventoryHistory.DataSource = objdataset
            gvInventoryHistory.DataBind()

        End If
        objconnection.DBDisconnect()
        upSalesHistory.Update()
        If rdoMonthSummary.Items(4).Selected = False Then '
            If gvInventoryHistory.Rows.Count = 0 Then
                imgbtnReturn.Visible = False
            Else
                imgbtnReturn.Visible = True
            End If
        End If


    End Sub
    Protected Sub GetWeekly_Sales_History()
        Dim objConnection As conClass = New conClass()
        objConnection.DBconnect()

        Dim objCommand As SqlCommand = New SqlCommand()

        objCommand.Connection = objConnection.con1
        objCommand.CommandText = "proc_WeeklySalesHistory"
        objCommand.CommandType = CommandType.StoredProcedure

        If Convert.ToBoolean(Session("IsCorpOffice")) = True Then
            objCommand.Parameters.AddWithValue("@Storeno", ViewState("LocationStoreno"))
        Else
            objCommand.Parameters.AddWithValue("@Storeno", ViewState("storeno"))
        End If
        objCommand.Parameters.AddWithValue("@SKU", ViewState("item_mst_id").ToString.Trim)
        objCommand.Parameters.AddWithValue("@CurrDate", objTime.CurrentDateTime(VbGenralfunctions.AllDateFormat.DateFormat1))
        Dim ObjDataAdapter As SqlDataAdapter = New SqlDataAdapter()
        ObjDataAdapter.SelectCommand = objCommand

        Dim objDataset As DataSet = New DataSet()

        ObjDataAdapter.Fill(objDataset, "WeeklySales")

        If objDataset.Tables(1).Rows.Count > 0 Then
            hminvalue.Value = 0
            hmaxvalue.Value = 0
            If objDataset.Tables(1).Rows(0)("maxValue").ToString <> objDataset.Tables(1).Rows(0)("minValue").ToString Then
                lblhighWeek.Text = IIf(IsDBNull(objDataset.Tables(1).Rows(0)("maxValue").ToString), "0", objDataset.Tables(1).Rows(0)("maxValue").ToString.Trim)
                lblaverageWeek.Text = IIf(IsDBNull(objDataset.Tables(1).Rows(0)("averageValue").ToString), "0", objDataset.Tables(1).Rows(0)("averageValue").ToString.Trim)
                'Integer.Parse(78 - (Integer.Parse(objDataset.Tables(1).Rows(0)("cntMaxValue").ToString) + Integer.Parse(objDataset.Tables(1).Rows(0)("cntMinValue").ToString)))
                lbllowweek.Text = IIf(IsDBNull(objDataset.Tables(1).Rows(0)("minValue").ToString), "0", objDataset.Tables(1).Rows(0)("minValue").ToString.Trim)
                hminvalue.Value = IIf(IsDBNull(objDataset.Tables(1).Rows(0)("minValue").ToString), "0", objDataset.Tables(1).Rows(0)("minValue").ToString.Trim)
                hmaxvalue.Value = objDataset.Tables(1).Rows(0)("maxValue").ToString
            Else
                lblhighWeek.Text = "0"
                lblaverageWeek.Text = IIf(IsDBNull(objDataset.Tables(1).Rows(0)("averageValue").ToString), "0", objDataset.Tables(1).Rows(0)("averageValue").ToString.Trim)
                lbllowweek.Text = IIf(IsDBNull(objDataset.Tables(1).Rows(0)("minValue").ToString), "0", objDataset.Tables(1).Rows(0)("minValue").ToString.Trim)
                hminvalue.Value = IIf(IsDBNull(objDataset.Tables(1).Rows(0)("maxValue").ToString), "0", objDataset.Tables(1).Rows(0)("maxValue").ToString.Trim)
                hmaxvalue.Value = objDataset.Tables(1).Rows(0)("maxValue").ToString
            End If
        End If
        If objDataset.Tables(0).Rows.Count > 0 Then
            dlWeeklySales.DataSource = objDataset
            dlWeeklySales.DataBind()
        Else
            dlWeeklySales.DataSource = objDataset
            dlWeeklySales.DataBind()
        End If
    End Sub
    Protected Sub rdoSalesWeeklyGraph_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdoSalesWeeklyGraph.SelectedIndexChanged
        If rdoSalesWeeklyGraph.Items(0).Selected = True Then
            If ViewState("itemtype") = "P" Or ViewState("itemtype") = "M" Then
                lblPoursSubItem1.Visible = True
                ddlPoursSubItem1.Visible = True
            Else
                lblPoursSubItem1.Visible = False
                ddlPoursSubItem1.Visible = False
            End If
            rdoMonthSummary.Visible = True
            mulSalesWeeklyGraphHistory.ActiveViewIndex = 0
            pnlSalesWeeklyGraph.Visible = True
            DateRange.Visible = False
            imgbtnsearch.Visible = False
            gvInventoryHistory.Visible = False
            partssaleshistory.Visible = True
            rdoGraph.Visible = True

            rdoGraph.Items(0).Selected = True
            rdoGraph.Items(1).Selected = False
            rdoGraph.Items(2).Selected = False
            imgbtnReturn.Visible = False
            chkReturnQty.Visible = False
            chkPromoSalesQty.Visible = False

            If rdoGraph.Items(0).Selected = True Then
                ViewState("graphtype") = "unit"
            ElseIf rdoGraph.Items(1).Selected = True Then

                ViewState("graphtype") = "revenue"
            ElseIf rdoGraph.Items(2).Selected = True Then
                ViewState("graphtype") = "profitability"
            Else
                ViewState("graphtype") = "revenue"
            End If

            Dim ShowinStock As Boolean = False
            If rdoGraph.Items(0).Selected = True Then
                ViewState("graphtype") = "unit"
                If ViewState("qtypercase").ToString.ToUpper() = "Y" Then
                    If ViewState("QtyPerCaseAmt") IsNot Nothing Then
                        If ViewState("QtyPerCaseAmt") > 0 Then
                            chkShowinCase.Visible = True
                            If chkShowinCase.Checked = True Then
                                ShowinStock = True
                            Else
                                ShowinStock = False
                            End If
                            'chkShowinCase.Checked = True
                        Else
                            chkShowinCase.Visible = False
                            chkShowinCase.Checked = False
                            ShowinStock = False
                        End If
                    Else
                        chkShowinCase.Visible = False
                        chkShowinCase.Checked = False
                        ShowinStock = False
                    End If
                Else
                    chkShowinCase.Visible = False
                    chkShowinCase.Checked = False
                    ShowinStock = False
                End If
            ElseIf rdoGraph.Items(1).Selected = True Then
                ViewState("graphtype") = "revenue"
                chkShowinCase.Visible = False
                chkShowinCase.Checked = False
            ElseIf rdoGraph.Items(2).Selected = True Then
                ViewState("graphtype") = "profitability"
                chkShowinCase.Visible = False
                chkShowinCase.Checked = False
            End If

            Dim sku As String = HttpUtility.UrlEncode(ViewState("item_mst_id").ToString.Trim())
            Dim storeno As String
            If Convert.ToBoolean(Session("IsCorpOffice")) = True Then
                storeno = HttpUtility.UrlEncode(ViewState("LocationStoreno"))
            Else
                storeno = HttpUtility.UrlEncode(ViewState("storeno"))
            End If

            Dim type As String = ViewState("itemtype")
            Dim Graphtype As String = ViewState("graphtype")
            Dim graphfor As String = ""
            If ViewState("itemtype").ToString.Trim = "P" Or ViewState("itemtype").ToString.Trim = "M" Then
                graphfor = ViewState("itemtype").ToString.Trim
            ElseIf ViewState("itemtype").ToString.Trim().ToUpper() = "S" Or ViewState("itemtype").ToString.Trim().ToUpper() = "N" Then
                graphfor = ViewState("itemtype").ToString.Trim
            Else
                graphfor = "I"
            End If
            Dim poursitemsubid As String = ""
            If ViewState("itemtype") = "P" Or ViewState("itemtype") = "M" Then
                poursitemsubid = IIf(ddlPoursSubItem1.SelectedIndex <> 0, (ddlPoursSubItem1.SelectedItem.Value).ToString, "0")
            Else
                poursitemsubid = "0"
            End If
            partssaleshistory.Attributes.Add("src", "POSInventoryGraphRpt.aspx?storeno=" + storeno + "&mode=S&type=qty&graphfor=" & graphfor.ToString & "&sku=" + sku + "&itemtype=" + ViewState("itemtype") + "&graphtype=" + ViewState("graphtype") + "&poursid=" + poursitemsubid)
            gvInventoryHistory.PageIndex = 0
            dlWeeklySales.Visible = False
            'chkShowinCase.Visible = False

            Call SalesHistory()
        ElseIf rdoSalesWeeklyGraph.Items(1).Selected = True Then
            If ViewState("itemtype") = "P" Or ViewState("itemtype") = "M" Then
                lblPoursSubItem1.Visible = False
                ddlPoursSubItem1.Visible = False
            Else
                lblPoursSubItem1.Visible = False
                ddlPoursSubItem1.Visible = False
            End If
            mulSalesWeeklyGraphHistory.ActiveViewIndex = 1
            pnlSalesWeeklyGraph.Visible = True
            DateRange.Visible = False
            imgbtnsearch.Visible = False
            gvInventoryHistory.Visible = False
            partssaleshistory.Visible = False
            rdoGraph.Visible = False
            chkReturnQty.Visible = False
            chkPromoSalesQty.Visible = False
            gvInventoryHistory.PageIndex = 0
            imgecalander.Disabled = False
            dlWeeklySales.Visible = True
            chkShowinCase.Visible = False
            imgbtnReturn.Visible = False
            Call GetWeekly_Sales_History()
            rdoMonthSummary.Visible = False

        End If
        upSalesWeeklyGraph.Update()
    End Sub
    Private Sub FillPours_SubItem()
        Dim dt As DataTable
        Dim objconnection As conClass = New conClass
        ' Dim objreader As SqlDataReader
        objconnection.DBconnect()

        Dim objcommand As SqlCommand = New SqlCommand
        objcommand.CommandType = CommandType.StoredProcedure
        objcommand.CommandText = "Proc_Get_InventorySubItems_SalesHistory"
        objcommand.Connection = objconnection.con1

        If Convert.ToBoolean(Session("IsCorpOffice")) = True Then
            objcommand.Parameters.AddWithValue("@Storeno", ViewState("LocationStoreno"))
        Else
            objcommand.Parameters.AddWithValue("@Storeno", Session("storeno"))
        End If

        'objcommand.Parameters.AddWithValue("@Storeno", ViewState("storeno"))
        objcommand.Parameters.AddWithValue("@Item_mst_ID", ViewState("item_mst_id"))



        Dim objdataadapter As SqlDataAdapter = New SqlDataAdapter
        Dim objdataset As DataSet = New DataSet
        objdataadapter.SelectCommand = objcommand
        objdataadapter.Fill(objdataset)


        If objdataset.Tables(0).Rows.Count > 0 Then
            ddlPoursSubItem.Items.Clear()
            ddlPoursSubItem.DataTextField = "Description"
            ddlPoursSubItem.DataValueField = "ID"
            ddlPoursSubItem.DataSource = objdataset.Tables(0)
            ddlPoursSubItem.DataBind()
            ddlPoursSubItem.Items.Insert(0, New ListItem("All Variations", 0))
            ddlPoursSubItem.SelectedIndex = 0

            ddlPoursSubItem1.Items.Clear()
            ddlPoursSubItem1.DataTextField = "Description"
            ddlPoursSubItem1.DataValueField = "ID"
            ddlPoursSubItem1.DataSource = objdataset.Tables(0)
            ddlPoursSubItem1.DataBind()
            ddlPoursSubItem1.Items.Insert(0, New ListItem("All Variations", 0))
            ddlPoursSubItem1.SelectedIndex = 0
        Else
            ddlPoursSubItem.Items.Clear()
            ddlPoursSubItem.Items.Insert(0, New ListItem("All Variations", 0))
            ddlPoursSubItem.SelectedIndex = 0

            ddlPoursSubItem1.Items.Clear()
            ddlPoursSubItem1.Items.Insert(0, New ListItem("All Variations", 0))
            ddlPoursSubItem1.SelectedIndex = 0
        End If
        objconnection.DBDisconnect()
    End Sub
    Protected Sub dlWeeklySales_ItemDataBound(sender As Object, e As DataListItemEventArgs)
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim drv As DataRowView
            drv = DirectCast(e.Item.DataItem, DataRowView)

            Dim lblwdate As Label = CType(e.Item.FindControl("lblWeeklyDate"), Label)
            Dim lblsolditem As Label = CType(e.Item.FindControl("lblTotalSoldItem"), Label)

            If hmaxvalue.Value <> hminvalue.Value Then


                If lblsolditem.Text = hmaxvalue.Value Then
                    e.Item.ForeColor = Drawing.Color.Green
                ElseIf lblsolditem.Text = hminvalue.Value Then
                    e.Item.ForeColor = Drawing.Color.Purple
                Else
                    e.Item.ForeColor = Drawing.Color.Black
                End If
            Else
                e.Item.ForeColor = Drawing.Color.Purple
            End If


        End If

    End Sub
    Protected Sub rdoMonthSummary_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdoMonthSummary.SelectedIndexChanged

        Dim strfirstdate As DateTime
        Dim mnth As String = ""
        If ViewState("itemtype") = "P" Or ViewState("itemtype") = "M" Then
            lblPoursSubItem.Visible = True
            ddlPoursSubItem.Visible = True
            lblPoursSubItem1.Visible = False
            ddlPoursSubItem1.Visible = False
            Call FillPours_SubItem()
        Else
            lblPoursSubItem.Visible = False
            ddlPoursSubItem.Visible = False
        End If
        If rdoMonthSummary.Items(0).Selected = True Then
            rdoSalesWeeklyGraph.Visible = False
            DateRange.Visible = True
            imgbtnsearch.Visible = True
            gvInventoryHistory.Visible = True
            partssaleshistory.Visible = False
            rdoGraph.Visible = False
            chkReturnQty.Visible = True
            chkPromoSalesQty.Visible = True
            mnth = DateTime.Now.Month.ToString()
            strfirstdate = mnth + "/1/" + DateTime.Now.Year.ToString()
            ViewState("SalesStartDate") = strfirstdate.ToString("MM/dd/yyyy")
            txtSalesFrom.Text = strfirstdate
            strfirstdate = DateAdd(DateInterval.Month, 1, strfirstdate)
            ViewState("SalesEndDate") = strfirstdate.ToString("MM/dd/yyyy")
            txtSalesTo.Text = DateTime.Now.ToString("MM/dd/yyyy")
            gvInventoryHistory.PageIndex = 0
            'ScriptManager.RegisterStartupScript(Page, [GetType], "message", "javascript:radioDisable(0)", True)
            imgecalander.Disabled = True
            dlWeeklySales.Visible = False
            chkShowinCase.Visible = True
            imgbtnReturn.Visible = True
            pnlSalesWeeklyGraph.Visible = False
        ElseIf rdoMonthSummary.Items(1).Selected = True Then
            rdoSalesWeeklyGraph.Visible = False
            DateRange.Visible = True
            imgbtnsearch.Visible = True
            gvInventoryHistory.Visible = True
            partssaleshistory.Visible = False
            rdoGraph.Visible = False
            chkReturnQty.Visible = True
            chkPromoSalesQty.Visible = True
            mnth = DateTime.Now.Month.ToString()
            strfirstdate = mnth + "/1/" + DateTime.Now.Year.ToString()
            strfirstdate = DateAdd(DateInterval.Month, -1, strfirstdate)
            ViewState("SalesStartDate") = strfirstdate.ToString("MM/dd/yyyy")
            txtSalesFrom.Text = strfirstdate

            strfirstdate = DateTime.Now.Month.ToString() + "/1/" + DateTime.Now.Year.ToString()
            strfirstdate = DateAdd(DateInterval.Month, 1, strfirstdate)
            ViewState("SalesEndDate") = strfirstdate.ToString("MM/dd/yyyy")
            txtSalesTo.Text = DateTime.Now.ToString("MM/dd/yyyy")
            gvInventoryHistory.PageIndex = 0
            'ScriptManager.RegisterStartupScript(Page, [GetType], "message", "javascript:radioDisable(0)", True)
            imgecalander.Disabled = True
            dlWeeklySales.Visible = False
            chkShowinCase.Visible = True
            imgbtnReturn.Visible = True
            pnlSalesWeeklyGraph.Visible = False
            upSalesHistory.Update()
        ElseIf rdoMonthSummary.Items(2).Selected = True Then
            rdoSalesWeeklyGraph.Visible = False
            DateRange.Visible = True
            imgbtnsearch.Visible = True
            gvInventoryHistory.Visible = True
            partssaleshistory.Visible = False
            rdoGraph.Visible = False
            chkReturnQty.Visible = True
            chkPromoSalesQty.Visible = True
            mnth = DateTime.Now.Month.ToString()
            strfirstdate = mnth + "/1/" + DateTime.Now.Year.ToString()
            strfirstdate = DateAdd(DateInterval.Month, -2, strfirstdate)
            ViewState("SalesStartDate") = strfirstdate.ToString("MM/dd/yyyy")
            txtSalesFrom.Text = strfirstdate

            strfirstdate = DateTime.Now.Month.ToString() + "/1/" + DateTime.Now.Year.ToString()
            strfirstdate = DateAdd(DateInterval.Month, 1, strfirstdate)
            ViewState("SalesEndDate") = strfirstdate.ToString("MM/dd/yyyy")
            txtSalesTo.Text = DateTime.Now.ToString("MM/dd/yyyy")
            gvInventoryHistory.PageIndex = 0
            'ScriptManager.RegisterStartupScript(Page, [GetType], "message", "javascript:radioDisable(0)", True)
            imgecalander.Disabled = True
            dlWeeklySales.Visible = False
            imgbtnReturn.Visible = True
            chkShowinCase.Visible = True
            pnlSalesWeeklyGraph.Visible = False
        ElseIf rdoMonthSummary.Items(3).Selected = True Then
            rdoSalesWeeklyGraph.Visible = False
            DateRange.Visible = True
            imgbtnsearch.Visible = True
            gvInventoryHistory.Visible = True
            partssaleshistory.Visible = False
            rdoGraph.Visible = False
            chkReturnQty.Visible = True
            chkPromoSalesQty.Visible = True
            mnth = DateTime.Now.Month.ToString()
            strfirstdate = mnth + "/1/" + DateTime.Now.Year.ToString()
            strfirstdate = DateAdd(DateInterval.Month, -6, strfirstdate)
            txtSalesFrom.Text = strfirstdate.ToString("MM/dd/yyyy")
            ViewState("SalesStartDate") = txtSalesFrom.Text
            txtSalesTo.Text = DateTime.Now.ToString("MM/dd/yyyy")
            ViewState("SalesEndDate") = txtSalesTo.Text
            gvInventoryHistory.PageIndex = 0
            'ScriptManager.RegisterStartupScript(Page, [GetType], "message", "javascript:radioDisable(1)", True)
            imgecalander.Disabled = False
            dlWeeklySales.Visible = False
            imgbtnReturn.Visible = True
            chkShowinCase.Visible = True
            pnlSalesWeeklyGraph.Visible = False
        ElseIf rdoMonthSummary.Items(4).Selected = True Then
            If ViewState("itemtype") = "P" Or ViewState("itemtype") = "M" Then
                lblPoursSubItem.Visible = False
                ddlPoursSubItem.Visible = False
                lblPoursSubItem1.Visible = True
                ddlPoursSubItem1.Visible = True
            Else
                lblPoursSubItem.Visible = False
                ddlPoursSubItem.Visible = False
                lblPoursSubItem1.Visible = False
                ddlPoursSubItem1.Visible = False
            End If
            rdoSalesWeeklyGraph.Visible = True
            mulSalesWeeklyGraphHistory.ActiveViewIndex = 0
            rdoSalesWeeklyGraph.Items(0).Selected = True
            rdoSalesWeeklyGraph.Items(1).Selected = False
            pnlSalesWeeklyGraph.Visible = True
            DateRange.Visible = False
            imgbtnsearch.Visible = False
            gvInventoryHistory.Visible = False
            partssaleshistory.Visible = True
            rdoGraph.Visible = True

            rdoGraph.Items(0).Selected = True
            rdoGraph.Items(1).Selected = False
            rdoGraph.Items(2).Selected = False
            imgbtnReturn.Visible = False
            chkReturnQty.Visible = False
            chkPromoSalesQty.Visible = False

            If rdoGraph.Items(0).Selected = True Then
                ViewState("graphtype") = "unit"
            ElseIf rdoGraph.Items(1).Selected = True Then
                ViewState("graphtype") = "revenue"
            ElseIf rdoGraph.Items(2).Selected = True Then
                ViewState("graphtype") = "profitability"
            Else
                ViewState("graphtype") = "revenue"
            End If

            Dim ShowinStock As Boolean = False
            If rdoGraph.Items(0).Selected = True Then
                ViewState("graphtype") = "unit"
                If ViewState("qtypercase").ToString.ToUpper() = "Y" Then
                    If ViewState("QtyPerCaseAmt") IsNot Nothing Then
                        If ViewState("QtyPerCaseAmt") > 0 Then
                            chkShowinCase.Visible = True
                            If chkShowinCase.Checked = True Then
                                ShowinStock = True
                            Else
                                ShowinStock = False
                            End If
                        Else
                            chkShowinCase.Visible = False
                            chkShowinCase.Checked = False
                            ShowinStock = False
                        End If
                    Else
                        chkShowinCase.Visible = False
                        chkShowinCase.Checked = False
                        ShowinStock = False
                    End If
                Else
                    chkShowinCase.Visible = False
                    chkShowinCase.Checked = False
                    ShowinStock = False
                End If
            ElseIf rdoGraph.Items(1).Selected = True Then
                ViewState("graphtype") = "revenue"
                chkShowinCase.Visible = False
                chkShowinCase.Checked = False
            ElseIf rdoGraph.Items(2).Selected = True Then
                ViewState("graphtype") = "profitability"
                chkShowinCase.Visible = False
                chkShowinCase.Checked = False
            End If

            Dim sku As String = HttpUtility.UrlEncode(ViewState("item_mst_id").ToString.Trim())
            Dim storeno As String
            Dim type As String = ViewState("itemtype")
            Dim Graphtype As String = ViewState("graphtype")
            Dim graphfor As String = ""


            If Convert.ToBoolean(Session("IsCorpOffice")) = True Then
                storeno = HttpUtility.UrlEncode(ViewState("LocationStoreno"))
            Else
                storeno = HttpUtility.UrlEncode(ViewState("storeno"))
            End If


            If ViewState("itemtype").ToString.Trim = "P" Or ViewState("itemtype").ToString.Trim = "M" Then
                graphfor = ViewState("itemtype").ToString.Trim
            ElseIf ViewState("itemtype").ToString.Trim().ToUpper() = "S" Or ViewState("itemtype").ToString.Trim().ToUpper() = "N" Then
                graphfor = ViewState("itemtype").ToString.Trim
            Else
                graphfor = "I"
            End If
            Dim poursitemsubid As String = ""
            If ViewState("itemtype") = "P" Or ViewState("itemtype") = "M" Then
                poursitemsubid = IIf(ddlPoursSubItem1.SelectedIndex <> 0, (ddlPoursSubItem1.SelectedItem.Value).ToString, "0")
            Else
                poursitemsubid = "0"
            End If
            partssaleshistory.Attributes.Add("src", "POSInventoryGraphRpt.aspx?storeno=" + storeno + "&mode=S&type=qty&graphfor=" & graphfor.ToString & "&sku=" + sku + "&itemtype=" + ViewState("itemtype") + "&graphtype=" + ViewState("graphtype") + "&poursid=" + poursitemsubid)
            gvInventoryHistory.PageIndex = 0
            dlWeeklySales.Visible = False

            Call SalesHistory()
        End If
        ViewState("dateCondition") = dateCondition
        Call GetSalesHistory(ViewState("SalesStartDate"), ViewState("SalesEndDate"))
    End Sub
    Public Sub Profitabilityflow()
        rdoMonthSummary.Visible = False
        mulSalesWeeklyGraphHistory.ActiveViewIndex = 2

        Dim objsales As New vbReport
        Dim dsPI As New DataSet
        Dim Mon As Int32 = 0
        'objsales.Storeno = Session("storeno")


        If Convert.ToBoolean(Session("IsCorpOffice")) = True Then
            objsales.StoreNo = ViewState("LocationStoreno")
        Else
            objsales.StoreNo = Session("storeno")
        End If
        objsales.SKU = HttpUtility.UrlEncode(ViewState("item_mst_id").ToString.Trim())
        If ddlprofitability.SelectedItem.Value = 0 Then
            objsales.startdate = New Date(Date.Today.Year, Date.Today.Month, Date.Today.Day).AddMonths(-18).ToString("MM/dd/yyyy")
            objsales.enddate = Date.Today
        Else
            Mon = Convert.ToInt32("-" & Convert.ToInt32((Convert.ToInt32(ddlprofitability.SelectedItem.Value) - 1)))
            objsales.startdate = New Date(Date.Today.Year, Date.Today.Month, 1).AddMonths(Mon).ToString("MM/dd/yyyy")
            objsales.enddate = New Date(Date.Today.Year, Date.Today.Month, Date.DaysInMonth(Date.Today.Year, Date.Today.Month)).AddMonths(Mon).ToString("MM/dd/yyyy")
        End If

        dsPI = objsales.Salea_history_profitabilitygraph()
        If dsPI.Tables(0).Rows.Count > 0 Then
            pnlProfitabilityGraph.Visible = True
            Dim profitdollers As Double
            Dim dbcost As Double
            Dim dbavgprice As Double
            Dim UnitMarkUp As Double
            Dim UnitMargin As Double
            profitdollers = (dsPI.Tables(0).Rows(0).Item("sales") - dsPI.Tables(0).Rows(0).Item("cost"))
            dbcost = dsPI.Tables(0).Rows(0).Item("cost")
            dbavgprice = dsPI.Tables(0).Rows(0).Item("sales")
            If dbavgprice <> 0.0 And dbcost <> 0.0 Then
                UnitMarkUp = FormatNumber((((dbavgprice - dbcost) / dbcost) * 100), 2)
                UnitMargin = FormatNumber((((dbavgprice - dbcost) / dbavgprice) * 100), 2)
            Else
                UnitMarkUp = "0.00"
                UnitMargin = "0.00"
            End If

            Dim zVal As Double() = {0, 0, 0}
            zVal(0) = dsPI.Tables(0).Rows(0).Item("sales").ToString
            zVal(1) = dsPI.Tables(0).Rows(0).Item("cost").ToString
            zVal(2) = dsPI.Tables(0).Rows(0).Item("Profit").ToString

            If zVal(0) = 0 AndAlso zVal(1) = 0 AndAlso zVal(2) = 0 Then
                tdpricecostprofit.Visible = False
                tdpricecostprofit1.Visible = False
                tdpricecostprofit2.Visible = False
            Else
                tdpricecostprofit.Visible = True
                tdpricecostprofit1.Visible = True
                tdpricecostprofit2.Visible = True
            End If
            lblprof_price.Text = "(" + " $ " + FormatNumber(dsPI.Tables(0).Rows(0).Item("sales").ToString, 2) + ")"
            lblCost.Text = "(" + " $ " + FormatNumber(dsPI.Tables(0).Rows(0).Item("cost").ToString, 2) + ")"
            lblprofit.Text = "(" + "  $ " + FormatNumber(dsPI.Tables(0).Rows(0).Item("Profit").ToString, 2) + ")"

            lblSubtotal.Text = "$ " + " " + FormatNumber(dsPI.Tables(0).Rows(0).Item("sales").ToString, 2)
            lblsales.Text = "$ " + " " + FormatNumber(dsPI.Tables(0).Rows(0).Item("cost").ToString, 2)
            lblProfitD.Text = "$ " + " " + FormatNumber(dsPI.Tables(0).Rows(0).Item("Profit").ToString, 2)
            lblMargin.Text = " " + " " + Convert.ToString(UnitMargin) + " %"
            lblMarkUp.Text = " " + " " + Convert.ToString(UnitMarkUp) + " %"
            Dim yVAL As Double()
            yVAL = zVal
            Dim xName As String() = {" ", " ", " "}
            Chart1.Series(0).Points.DataBindXY(xName, yVAL)
            Chart1.Series(0).Points(0).Color = System.Drawing.Color.PaleGreen
            Chart1.Series(0).Points(1).Color = System.Drawing.Color.Pink
            Chart1.Series(0).Points(2).Color = System.Drawing.Color.Yellow
            Chart1.Series(0).ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Pie
        Else
            pnlProfitabilityGraph.Visible = False

        End If
        upSalesHistory.Update()
    End Sub
    Protected Sub rdoGraph_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdoGraph.SelectedIndexChanged
        rdoMonthSummary.Visible = True
        mulSalesWeeklyGraphHistory.ActiveViewIndex = 0
        If ViewState("itemtype") = "P" Or ViewState("itemtype") = "M" Then
            lblPoursSubItem.Visible = False
            ddlPoursSubItem.Visible = False
            lblPoursSubItem1.Visible = True
            ddlPoursSubItem1.Visible = True
        Else
            lblPoursSubItem.Visible = False
            ddlPoursSubItem.Visible = False
            lblPoursSubItem1.Visible = False
            ddlPoursSubItem1.Visible = False
        End If

        If rdoGraph.Items(0).Selected = True Then
            ViewState("graphtype") = "unit"
        ElseIf rdoGraph.Items(1).Selected = True Then

            ViewState("graphtype") = "revenue"
        ElseIf rdoGraph.Items(2).Selected = True Then

            ViewState("graphtype") = "profitability"
        Else
            ViewState("graphtype") = "revenue"
        End If

        Dim ShowinStock As Boolean = False
        If rdoGraph.Items(0).Selected = True Then
            ViewState("graphtype") = "unit"
            If ViewState("qtypercase").ToString.ToUpper() = "Y" Then
                If ViewState("QtyPerCaseAmt") IsNot Nothing Then
                    If ViewState("QtyPerCaseAmt") > 0 Then
                        chkShowinCase.Visible = True
                        If chkShowinCase.Checked = True Then
                            ShowinStock = True
                        Else
                            ShowinStock = False
                        End If
                        'chkShowinCase.Checked = True
                    Else
                        chkShowinCase.Visible = False
                        chkShowinCase.Checked = False
                        ShowinStock = False
                    End If
                Else
                    chkShowinCase.Visible = False
                    chkShowinCase.Checked = False
                    ShowinStock = False
                End If
            Else
                chkShowinCase.Visible = False
                chkShowinCase.Checked = False
                ShowinStock = False
            End If
        ElseIf rdoGraph.Items(1).Selected = True Then
            ViewState("graphtype") = "revenue"
            chkShowinCase.Visible = False
            chkShowinCase.Checked = False
        ElseIf rdoGraph.Items(2).Selected = True Then
            ViewState("graphtype") = "profitability"
            chkShowinCase.Visible = False
            chkShowinCase.Checked = False
        End If


        Dim sku As String = HttpUtility.UrlEncode(ViewState("item_mst_id").ToString.Trim())
        Dim storeno As String

        If Convert.ToBoolean(Session("IsCorpOffice")) = True Then
            storeno = HttpUtility.UrlEncode(ViewState("LocationStoreno"))
        Else
            storeno = HttpUtility.UrlEncode(ViewState("storeno"))
        End If
        Dim type As String = ViewState("itemtype")
        Dim Graphtype As String = ViewState("graphtype")
        Dim graphfor As String = ""

        If ViewState("itemtype").ToString.Trim = "P" Or ViewState("itemtype").ToString.Trim = "M" Then
            graphfor = ViewState("itemtype").ToString.Trim
        ElseIf ViewState("itemtype").ToString.Trim().ToUpper() = "S" Or ViewState("itemtype").ToString.Trim().ToUpper() = "N" Then
            graphfor = ViewState("itemtype").ToString.Trim
        Else
            graphfor = "I"
        End If
        Dim poursitemsubid As String = ""
        If ViewState("itemtype") = "P" Or ViewState("itemtype") = "M" Then
            poursitemsubid = IIf(ddlPoursSubItem1.SelectedIndex <> 0, (ddlPoursSubItem1.SelectedItem.Value).ToString, "0")
        Else
            poursitemsubid = "0"
        End If
        If rdoGraph.Items(0).Selected = True Then
            partssaleshistory.Attributes.Add("src", "POSInventoryGraphRpt.aspx?storeno=" + storeno + "&mode=S&type=qty&graphfor=" & graphfor.ToString & "&sku=" + sku + "&itemtype=" + ViewState("itemtype") + "&graphtype=" + ViewState("graphtype") + "&ShowInStock=" + ShowinStock.ToString() + "&poursid=" + poursitemsubid)
        ElseIf rdoGraph.Items(1).Selected = True Then
            partssaleshistory.Attributes.Add("src", "POSInventoryGraphRpt.aspx?storeno=" + storeno + "&mode=S&type=revenue&graphfor=" & graphfor.ToString & "&sku=" + sku + "&itemtype=" + ViewState("itemtype") + "&graphtype=" + ViewState("graphtype") + "&poursid=" + poursitemsubid)
        ElseIf rdoGraph.Items(2).Selected = True Then
            'partssaleshistory.Attributes.Add("src", "POSInventoryGraphRpt.aspx?storeno=" + storeno + "&mode=S&type=profitability&graphfor=" & graphfor.Trim & "&sku=" + sku + "&itemtype=" + ViewState("itemtype") + "&graphtype=" + ViewState("graphtype") + "&poursid=" + poursitemsubid)
            FillProfitabilityMonth()
            Profitabilityflow()
        End If
    End Sub
    Private Sub FillProfitabilityMonth()

        Dim objconnection As conClass = New conClass
        objconnection.DBconnect()

        Dim objcommand As SqlCommand = New SqlCommand
        objcommand.CommandType = CommandType.StoredProcedure
        objcommand.CommandText = "proc_ProfitabilityMonth"
        objcommand.Connection = objconnection.con1

        Dim objdataadapter As SqlDataAdapter = New SqlDataAdapter
        Dim objdataset As DataSet = New DataSet
        objdataadapter.SelectCommand = objcommand
        objdataadapter.Fill(objdataset)


        If objdataset.Tables(0).Rows.Count > 0 Then
            ddlprofitability.Items.Clear()
            ddlprofitability.DataTextField = "month"
            ddlprofitability.DataValueField = "monthorder"
            ddlprofitability.DataSource = objdataset.Tables(0)
            ddlprofitability.DataBind()

            ddlprofitability.SelectedIndex = 0
        End If
        objconnection.DBDisconnect()
    End Sub
    Protected Sub imgbtnsearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles imgbtnsearch.Click

        Call GetSalesHistory(txtSalesFrom.Text.Trim, txtSalesTo.Text.Trim)

    End Sub

    Protected Sub gvInventoryHistory_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvInventoryHistory.PageIndexChanging
        gvInventoryHistory.PageIndex = e.NewPageIndex
        If DateRange.Visible = True Then
            Call GetSalesHistory(txtSalesFrom.Text, txtSalesTo.Text)
        Else
            Call GetSalesHistory(ViewState("SalesStartDate"), ViewState("SalesEndDate"))
        End If
    End Sub

    Protected Sub gvInventoryHistory_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvInventoryHistory.Sorting
        If ViewState("sortcolumn1") = e.SortExpression Then
            ViewState("sortcolumn1") = e.SortExpression
            If ViewState("sortstatus1") = "ASC" Then
                ViewState("sortstatus1") = "DESC"
            Else
                ViewState("sortstatus1") = "ASC"
            End If
        Else
            ViewState("sortcolumn1") = e.SortExpression
            ViewState("sortstatus1") = "ASC"
        End If
        Call GetSalesHistory(txtSalesFrom.Text, txtSalesTo.Text)
    End Sub

    Protected Sub chkReturnQty_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkReturnQty.CheckedChanged
        Call GetSalesHistory(txtSalesFrom.Text.Trim, txtSalesTo.Text.Trim)
    End Sub

    Protected Sub chkPromoSalesQty_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkPromoSalesQty.CheckedChanged
        Call GetSalesHistory(txtSalesFrom.Text.Trim, txtSalesTo.Text.Trim)
    End Sub

    Protected Sub imgbtnReturn_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbtnReturn.Click
        Dim strStoreNo As String = ""
        Dim strStationNo As String = ""
        Dim strStartDate As String = ""
        Dim strEndDate As String = ""
        Dim strSku As String = ""
        Dim strReturn As String = ""
        Dim strPromo As String = ""

        If ViewState("storeno") IsNot Nothing Then
            If Convert.ToBoolean(Session("IsCorpOffice")) = True Then
                strStoreNo = ViewState("LocationStoreno").ToString().Trim()
            Else
                strStoreNo = ViewState("storeno").ToString().Trim()
            End If
        Else
            strStoreNo = ""
        End If
        If ViewState("stationno") IsNot Nothing Then
            strStationNo = ViewState("stationno").ToString().Trim()
        Else
            strStationNo = ""
        End If
        If ViewState("SalesStartDate") IsNot Nothing Then
            strStartDate = ViewState("SalesStartDate").ToString().Trim()
        Else
            strStartDate = ""
        End If
        If ViewState("SalesEndDate") IsNot Nothing Then
            strEndDate = ViewState("SalesEndDate").ToString().Trim()
        Else
            strEndDate = ""
        End If
        If ViewState("item_mst_id") IsNot Nothing Then
            strSku = ViewState("item_mst_id").ToString().Trim()
        Else
            strSku = ""
        End If
        If ViewState("ReturnItem") IsNot Nothing Then
            strReturn = ViewState("ReturnItem").ToString().Trim()
        Else
            strReturn = ""
        End If
        If ViewState("PromoItem") IsNot Nothing Then
            strPromo = ViewState("PromoItem").ToString().Trim()
        Else
            strPromo = ""
        End If

        Dim StrOpen As String
        StrOpen = "window.open('POSInventorySalesHistoryRpt.aspx?Store=" & strStoreNo & "&Station=" & strStationNo & "&StartDt=" & strStartDate & "&EndDt=" & strEndDate & "&SKU=" & strSku & "&Return=" & strReturn & "&Promo=" & strPromo & "', '_blank', 'resizable=1,width=600,height=600,top=5,left=70');"
        ScriptManager.RegisterStartupScript(Page, [GetType], "InventorySalesHistory", StrOpen, True)
    End Sub

#End Region
#Region "Purchase History"
    Public Sub PurchaseHistory()
        Dim strfirstdate As DateTime
        Dim mnth As String = ""
        mnth = DateTime.Now.Month.ToString()
        strfirstdate = mnth + "/1/" + DateTime.Now.Year.ToString()
        strfirstdate = DateAdd(DateInterval.Year, -1, strfirstdate)
        ViewState("purchaseStartDate") = strfirstdate.ToString("MM/dd/yyyy")
        strfirstdate = DateTime.Now.Month.ToString() + "/1/" + DateTime.Now.Year.ToString()
        strfirstdate = DateAdd(DateInterval.Month, 1, strfirstdate)
        ViewState("purchaseEndDate") = Date.Now.ToString("MM/dd/yyyy")
        txtpurchasesfromdate.Text = ViewState("purchaseStartDate")
        txtpurchasestodate.Text = ViewState("purchaseEndDate")
        Call GetPurchaseHistory(txtpurchasesfromdate.Text, txtpurchasestodate.Text)
    End Sub

    Protected Sub GetPurchaseHistory(ByVal startpurchasedate As String, ByVal endpurchasedate As String)
        Dim objTime As New VbGenralfunctions
        Dim objconnection As conClass = New conClass
        objconnection.DBconnect()
        '' create sqlcommand
        Dim objcommand As SqlCommand = New SqlCommand
        objcommand.CommandType = CommandType.StoredProcedure
        objcommand.Connection = objconnection.con1
        objcommand.CommandText = "proc_inventory_getpurchasehistory"
        '' parameters declaration
        If Convert.ToBoolean(Session("IsCorpOffice")) = True Then
            objcommand.Parameters.AddWithValue("@storeno", ViewState("LocationStoreno"))
        Else
            objcommand.Parameters.AddWithValue("@storeno", ViewState("storeno"))
        End If
        objcommand.Parameters.AddWithValue("@sku", ViewState("item_mst_id").ToString.Trim)
        objcommand.Parameters.AddWithValue("@startdate", objTime.GetTimeZoneDate(startpurchasedate, True, False))
        objcommand.Parameters.AddWithValue("@enddate", objTime.GetTimeZoneDate(endpurchasedate, True, True))

        Dim objdataadapter As SqlDataAdapter = New SqlDataAdapter
        Dim objdataset As DataSet = New DataSet
        objdataadapter.SelectCommand = objcommand
        objdataadapter.Fill(objdataset)
        If rdopurchasesummary.Items(4).Selected = True Then
            ViewState("purchaseStartDate") = startpurchasedate
            ViewState("purchaseEndDate") = endpurchasedate
        End If

        If objdataset.Tables.Count > 0 Then
            If objdataset.Tables(0).Rows.Count > 0 Then
                objTime.GetTimeByZone(objdataset.Tables(0), "time", VbGenralfunctions.AllDateFormat.DateFormat7)
                gvInventoryPurchaseHistory.DataSource = objdataset
                gvInventoryPurchaseHistory.DataBind()
            Else
                gvInventoryPurchaseHistory.DataSource = objdataset
                gvInventoryPurchaseHistory.DataBind()

            End If
        End If
        uppurchasehistory.Update()
        objconnection.DBDisconnect()
    End Sub
    Protected Sub Check_inv_type()
        Dim objConnection As conClass = New conClass()
        objConnection.DBconnect()
        Dim objCommand As SqlCommand = New SqlCommand()
        objCommand.Connection = objConnection.con1
        objCommand.CommandText = "proc_inventory_get_details"
        objCommand.CommandType = CommandType.StoredProcedure

        objCommand.Parameters.AddWithValue("@item_id", ViewState("item_mst_id"))
        If Convert.ToBoolean(Session("IsCorpOffice")) = True Then
            objCommand.Parameters.AddWithValue("@storeno", ViewState("LocationStoreno"))
        Else
            objCommand.Parameters.AddWithValue("@storeno", ViewState("storeno"))
        End If


        Dim ObjDataAdapter As SqlDataAdapter = New SqlDataAdapter()
        ObjDataAdapter.SelectCommand = objCommand

        Dim objDataset As DataSet = New DataSet()
        'Try
        ObjDataAdapter.Fill(objDataset, "inventory")

        If objDataset.Tables.Count > 0 Then
            If objDataset.Tables(0).Rows.Count > 0 Then
                If objDataset.Tables(0).Rows(0)("item_type").ToString.Trim = "K" Or objDataset.Tables(0).Rows(0)("item_type").ToString.Trim = "C" Or objDataset.Tables(0).Rows(0)("item_type").ToString.Trim = "S" Or objDataset.Tables(0).Rows(0)("item_type").ToString.Trim = "N" Then
                    ViewState("ShowPurchaseHistory") = False
                ElseIf ViewState("item_mst_id") = "F9" Then
                    ViewState("ShowPurchaseHistory") = False
                Else
                    ViewState("ShowPurchaseHistory") = True
                End If

            End If
        End If

    End Sub
    Public Sub Purchasehisinvflow()
        If ViewState("salesflow") Is Nothing Then
            Call getinventoryid()
        End If
        Call PurchaseHistory()
        'lblHeader.Text = "Inventory Purchase History - "
        'lblheaderitemname.Text = IIf(IsNothing(ViewState("name")), "", ViewState("name"))
        If IIf(IsNothing(ViewState("size")), "", ViewState("size")) = "Y" Then
            'getsizedetail(ViewState("item_mst_id"))
            If ViewState("Sizeheader") <> "" Then
                'lblheadersize.Text = "Size - " & ViewState("Sizeheader")
            Else
                'lblheadersize.Text = ""
            End If
        Else
            'lblheadersize.Text = ""
        End If
        Call bindview(4)
        'ScriptManager.RegisterStartupScript(Page, [GetType], "message", "javascript:purcahseDisable(0)", True)
        rdoPGraph.Visible = False
        divpurchasedata.Visible = True
        ifrmPurchaseHistory.Visible = False
    End Sub
    Protected Sub gvInventoryPurchaseHistory_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvInventoryPurchaseHistory.PageIndexChanging
        gvInventoryPurchaseHistory.PageIndex = e.NewPageIndex
        Call GetPurchaseHistory(txtpurchasesfromdate.Text, txtpurchasestodate.Text)
    End Sub

    Protected Sub rdopurchasesummary_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdopurchasesummary.SelectedIndexChanged
        Dim strfirstdate As DateTime
        Dim mnth As String = ""
        ViewState("CheckdiffDate") = "0"
        If rdopurchasesummary.Items(0).Selected = True Then
            trpurchasehistory.Visible = True
            imgbtnpurchasefind.Visible = True
            divpurchasedata.Visible = True
            mnth = DateTime.Now.Month.ToString()
            strfirstdate = mnth + "/1/" + DateTime.Now.Year.ToString()
            ViewState("purchaseStartDate") = strfirstdate.ToString("MM/dd/yyyy")
            txtpurchasesfromdate.Text = strfirstdate
            strfirstdate = DateAdd(DateInterval.Month, 1, strfirstdate)
            ViewState("purchaseEndDate") = strfirstdate.ToString("MM/dd/yyyy")
            txtpurchasestodate.Text = DateTime.Now.ToString("MM/dd/yyyy")
            gvInventoryPurchaseHistory.PageIndex = 0
            divpurchasedata.Visible = True
            ifrmPurchaseHistory.Visible = False
            rdoPGraph.Visible = False
            'ScriptManager.RegisterStartupScript(Page, [GetType], "message", "javascript:purcahseDisable(0)", True)

        ElseIf rdopurchasesummary.Items(1).Selected = True Then
            trpurchasehistory.Visible = True
            imgbtnpurchasefind.Visible = True
            divpurchasedata.Visible = True
            mnth = DateTime.Now.Month.ToString()
            strfirstdate = mnth + "/1/" + DateTime.Now.Year.ToString()
            strfirstdate = DateAdd(DateInterval.Month, -1, strfirstdate)
            ViewState("purchaseStartDate") = strfirstdate.ToString("MM/dd/yyyy")
            txtpurchasesfromdate.Text = strfirstdate
            strfirstdate = DateTime.Now.Month.ToString() + "/1/" + DateTime.Now.Year.ToString()
            strfirstdate = DateAdd(DateInterval.Month, 1, strfirstdate)
            ViewState("purchaseEndDate") = strfirstdate.ToString("MM/dd/yyyy")
            txtpurchasestodate.Text = DateTime.Now.ToString("MM/dd/yyyy")
            gvInventoryPurchaseHistory.PageIndex = 0
            divpurchasedata.Visible = True
            ifrmPurchaseHistory.Visible = False
            rdoPGraph.Visible = False
            'ScriptManager.RegisterStartupScript(Page, [GetType], "message", "javascript:purcahseDisable(0)", True)
        ElseIf rdopurchasesummary.Items(2).Selected = True Then
            trpurchasehistory.Visible = True
            imgbtnpurchasefind.Visible = True
            divpurchasedata.Visible = True
            mnth = DateTime.Now.Month.ToString()
            strfirstdate = mnth + "/1/" + DateTime.Now.Year.ToString()
            strfirstdate = DateAdd(DateInterval.Month, -2, strfirstdate)
            ViewState("purchaseStartDate") = strfirstdate.ToString("MM/dd/yyyy")
            txtpurchasesfromdate.Text = strfirstdate
            strfirstdate = DateTime.Now.Month.ToString() + "/1/" + DateTime.Now.Year.ToString()
            strfirstdate = DateAdd(DateInterval.Month, 1, strfirstdate)
            ViewState("purchaseEndDate") = strfirstdate.ToString("MM/dd/yyyy")
            txtpurchasestodate.Text = DateTime.Now.ToString("MM/dd/yyyy")
            gvInventoryPurchaseHistory.PageIndex = 0
            divpurchasedata.Visible = True
            ifrmPurchaseHistory.Visible = False
            rdoPGraph.Visible = False
            'ScriptManager.RegisterStartupScript(Page, [GetType], "message", "javascript:purcahseDisable(0)", True)
        ElseIf rdopurchasesummary.Items(3).Selected = True Then
            trpurchasehistory.Visible = True
            imgbtnpurchasefind.Visible = True
            divpurchasedata.Visible = True
            mnth = DateTime.Now.Month.ToString()
            strfirstdate = mnth + "/1/" + DateTime.Now.Year.ToString()
            strfirstdate = DateAdd(DateInterval.Year, -1, strfirstdate)
            ViewState("purchaseStartDate") = strfirstdate.ToString("MM/dd/yyyy")
            txtpurchasesfromdate.Text = strfirstdate
            strfirstdate = DateTime.Now.Month.ToString() + "/1/" + DateTime.Now.Year.ToString()
            strfirstdate = DateAdd(DateInterval.Month, 1, strfirstdate)
            ViewState("purchaseEndDate") = strfirstdate.ToString("MM/dd/yyyy")
            txtpurchasestodate.Text = DateTime.Now.ToString("MM/dd/yyyy")
            gvInventoryPurchaseHistory.PageIndex = 0
            divpurchasedata.Visible = True
            ifrmPurchaseHistory.Visible = False
            rdoPGraph.Visible = False
            'ScriptManager.RegisterStartupScript(Page, [GetType], "message", "javascript:purcahseDisable(0)", True)
        ElseIf rdopurchasesummary.Items(4).Selected = True Then
            trpurchasehistory.Visible = True
            imgbtnpurchasefind.Visible = True
            divpurchasedata.Visible = True
            mnth = DateTime.Now.Month.ToString()
            strfirstdate = mnth + "/1/" + DateTime.Now.Year.ToString()
            strfirstdate = DateAdd(DateInterval.Month, -6, strfirstdate)
            txtpurchasesfromdate.Text = strfirstdate.ToString("MM/dd/yyyy")
            ViewState("purchaseStartDate") = txtpurchasesfromdate.Text
            txtpurchasestodate.Text = DateTime.Now.ToString("MM/dd/yyyy")
            ViewState("purchaseEndDate") = txtpurchasestodate.Text
            gvInventoryPurchaseHistory.PageIndex = 0
            divpurchasedata.Visible = True
            ifrmPurchaseHistory.Visible = False
            rdoPGraph.Visible = False
            ScriptManager.RegisterStartupScript(Page, [GetType], "message", "javascript:purcahseDisable(1)", True)
        ElseIf rdopurchasesummary.Items(5).Selected = True Then
            trpurchasehistory.Visible = False
            imgbtnpurchasefind.Visible = False
            divpurchasedata.Visible = False
            ifrmPurchaseHistory.Visible = True
            rdoPGraph.Visible = True

            rdoPGraph.Items(0).Selected = True
            rdoPGraph.Items(1).Selected = False
            Dim sku As String = HttpUtility.UrlEncode(ViewState("item_mst_id").ToString.Trim())
            Dim storeno As String
            If Convert.ToBoolean(Session("IsCorpOffice")) = True Then
                storeno = HttpUtility.UrlEncode(ViewState("LocationStoreno"))
            Else
                storeno = HttpUtility.UrlEncode(ViewState("storeno"))
            End If
            Dim graphfor As String = ""
            If ViewState("itemtype").ToString.Trim = "P" Or ViewState("itemtype").ToString.Trim.ToUpper = "M" Then
                graphfor = ViewState("itemtype").ToString.Trim
            Else
                graphfor = "I"
            End If
            ifrmPurchaseHistory.Attributes.Add("src", "POSInventoryGraphRpt.aspx?storeno=" + storeno + "&mode=P&type=qty&graphfor=" & graphfor.ToString & "&sku=" + sku)
            'ScriptManager.RegisterStartupScript(Page, [GetType], "Sales History", "alert('This feature is temporarily unavailable...!');", True)
        End If
        ViewState("dateCondition") = dateCondition
        Call GetPurchaseHistory(ViewState("purchaseStartDate"), ViewState("purchaseEndDate"))
    End Sub
    Protected Sub rdoPGraph_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdoPGraph.SelectedIndexChanged

        Dim graphfor As String = ""
        If ViewState("itemtype").ToString.Trim = "P" Or ViewState("itemtype").ToString.Trim = "M" Then
            graphfor = ViewState("itemtype").ToString
        ElseIf ViewState("itemtype").ToString.Trim().ToUpper() = "S" Or ViewState("itemtype").ToString.Trim().ToUpper() = "N" Then
            graphfor = ViewState("itemtype").ToString.Trim
        Else
            graphfor = "I"
        End If
        If rdoPGraph.Items(0).Selected = True Then  '' to get Units Sales graph
            Dim sku As String = HttpUtility.UrlEncode(ViewState("item_mst_id").ToString.Trim())
            Dim storeno As String
            If Convert.ToBoolean(Session("IsCorpOffice")) = True Then
                storeno = HttpUtility.UrlEncode(ViewState("LocationStoreno"))
            Else
                storeno = HttpUtility.UrlEncode(ViewState("storeno"))
            End If
            ifrmPurchaseHistory.Attributes.Add("src", "POSInventoryGraphRpt.aspx?storeno=" + storeno + "&mode=P&type=qty&graphfor=" & graphfor.ToString & "&sku=" + sku)

        ElseIf rdoPGraph.Items(1).Selected = True Then   '' to get Revenue Sales graph
            Dim sku As String = HttpUtility.UrlEncode(ViewState("item_mst_id").ToString.Trim())
            Dim storeno As String
            If Convert.ToBoolean(Session("IsCorpOffice")) = True Then
                storeno = HttpUtility.UrlEncode(ViewState("LocationStoreno"))
            Else
                storeno = HttpUtility.UrlEncode(ViewState("storeno"))
            End If
            ifrmPurchaseHistory.Attributes.Add("src", "POSInventoryGraphRpt.aspx?storeno=" + storeno + "&mode=P&type=revenue&graphfor=" & graphfor.ToString & "&sku=" + sku)
        End If
    End Sub

    Protected Sub gvInventoryPurchaseHistory_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvInventoryPurchaseHistory.PreRender
        If ViewState("qtypercase") = "N" Then
            'gvInventoryPurchaseHistory.Columns(1).Visible = False
            gvInventoryPurchaseHistory.Columns(2).Visible = False
            gvInventoryPurchaseHistory.Columns(3).Visible = False
            gvInventoryPurchaseHistory.Columns(4).Visible = False
            gvInventoryPurchaseHistory.Columns(7).Visible = False
            gvInventoryPurchaseHistory.Columns(8).Visible = False
        Else
            gvInventoryPurchaseHistory.Columns(5).Visible = False
        End If

    End Sub
    Protected Sub gvInventoryPurchaseHistory_RowCreated(sender As Object, e As GridViewRowEventArgs) Handles gvInventoryPurchaseHistory.RowCreated
        Dim newRow As Boolean = False
        If (DataBinder.Eval(e.Row.DataItem, "time") IsNot Nothing) Then

            If ViewState("CheckdiffDate") = "0" Then
                newRow = True
                ViewState("CheckdiffDate") = DataBinder.Eval(e.Row.DataItem, "time", "{0:MM/dd/yy}").ToString() 'CType(e.Row.Cells(0).FindControl("lblDate"), Label).Text
                rowIndex = 1
            ElseIf ViewState("CheckdiffDate") = DataBinder.Eval(e.Row.DataItem, "time", "{0:MM/dd/yy}").ToString() Then 'CType(e.Row.Cells(0).FindControl("lblDate"), Label).Text Then
                newRow = False
            ElseIf ViewState("CheckdiffDate") <> DataBinder.Eval(e.Row.DataItem, "time", "{0:MM/dd/yy}").ToString() Then 'CType(e.Row.Cells(0).FindControl("lblDate"), Label).Text Then
                newRow = True
                ViewState("CheckdiffDate") = DataBinder.Eval(e.Row.DataItem, "time", "{0:MM/dd/yy}").ToString() 'CType(e.Row.Cells(0).FindControl("lblDate"), Label).Text
            End If

        End If
        If newRow Then
            Dim GridView1 As GridView = DirectCast(sender, GridView)

            Dim NewTotalRow As New GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert)
            NewTotalRow.Font.Bold = True
            NewTotalRow.BackColor = System.Drawing.Color.FromArgb(165, 197, 216)

            NewTotalRow.ForeColor = System.Drawing.Color.Black
            Dim HeaderCell As New TableCell()
            HeaderCell.Text = DataBinder.Eval(e.Row.DataItem, "time", "{0:MM/dd/yy}").ToString() + " " + "Inv #" 'CType(e.Row.Cells(0).FindControl("lblDate"), Label).Text
            HeaderCell.HorizontalAlign = HorizontalAlign.Left
            HeaderCell.ColumnSpan = 12
            NewTotalRow.Cells.Add(HeaderCell)

            GridView1.Controls(0).Controls.AddAt(e.Row.RowIndex + rowIndex, NewTotalRow)


            rowIndex += 1

        End If

    End Sub
    Protected Sub gvInventoryPurchaseHistory_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvInventoryPurchaseHistory.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            If CType(e.Row.Cells(0).FindControl("lblpercent"), Label).Text.Contains("-") Then '' If Last Cost Increases
                CType(e.Row.Cells(0).FindControl("lblpercent"), Label).ForeColor = Drawing.Color.Red
                CType(e.Row.Cells(0).FindControl("lblpercent"), Label).Text = "+ " & CType(e.Row.Cells(0).FindControl("lblpercent"), Label).Text.Replace("-", "")
            ElseIf CType(e.Row.Cells(0).FindControl("lblpercent"), Label).Text = "0.00" Then
                CType(e.Row.Cells(0).FindControl("lblpercent"), Label).Text = "0.00" '' If last cost is equal to current Cost
                CType(e.Row.Cells(0).FindControl("lblpercent"), Label).ForeColor = Drawing.Color.Black
            Else
                CType(e.Row.Cells(0).FindControl("lblpercent"), Label).Text = "- " & CType(e.Row.Cells(0).FindControl("lblpercent"), Label).Text
                CType(e.Row.Cells(0).FindControl("lblpercent"), Label).ForeColor = Drawing.Color.Green '' If last cost decreases
            End If

        End If

        If e.Row.RowType = DataControlRowType.EmptyDataRow Then

            If ViewState("qtypercase") = "N" Then

                CType(e.Row.FindControl("gvPurchasecol1"), TableCell).Visible = False
                CType(e.Row.FindControl("gvPurchasecol2"), TableCell).Visible = False
                CType(e.Row.FindControl("gvPurchasecol3"), TableCell).Visible = False
                CType(e.Row.FindControl("gvPurchasecol6"), TableCell).Visible = False
                CType(e.Row.FindControl("gvPurchasecol7"), TableCell).Visible = False
            Else
                CType(e.Row.FindControl("gvPurchasecol4"), TableCell).Visible = False
            End If
        End If
        If e.Row.RowType = DataControlRowType.Pager Then
            e.Row.Cells(0).Attributes.Add("Colspan", "11")
        End If
    End Sub
#End Region

    Protected Sub chkItemvendor_CheckedChanged(sender As Object, e As EventArgs) Handles chkItemvendor.CheckedChanged
        Call getinventorylist()
    End Sub



    Protected Sub btnmultipack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnmultipack.Click
        Session("skukit") = hdnPourSelection.Value
        ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "Returnvalues", "CloseWin()", True)
    End Sub
End Class

