Imports System.Data
Imports System.Data.SqlClient
Imports customPaging
Imports System.IO
Partial Class PosAddInventory
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("emp_id") Is Nothing Then
            Session.Abandon()
            Response.Redirect("POSLogin.aspx")
        End If
        ViewState("User") = Session("emp_id")

        If Not IsPostBack Then
            
            If Request.QueryString("frm") = "PO" Then
                btnSavedetails.Text = "Save / Order Now"
            Else
                btnSavedetails.Text = "Save / Receive Now"
            End If

            If Session("stationno") IsNot Nothing And Session("storeno") IsNot Nothing Then
                Dim objnickname As New vbStationSetup
                objnickname.StationNo = Session("stationno")
                objnickname.StoreNo = Session("storeno")
                Dim objname As DataTable
                objname = objnickname.fnGetStations_Name()

                objnickname.StationNo = Session("stationno")
                objnickname.StoreNo = Session("storeno")
                Dim dt As DataTable
                dt = objnickname.fngetPrinterDetails()
            End If
            ViewState("AllSku") = "1"
            Dim objsecuritycheck As New vbSecuritycheck
            ViewState("deptid") = ""
            ViewState("Dept") = "0"
            ViewState("storeno") = Session("storeno")
            ViewState("stationno") = Session("stationno")
            ViewState("Type") = "A"
            ViewState("sortstatus") = "ASC"
            If ViewState("sortcolumn") Is Nothing Then
                ViewState("sortcolumn") = "desc1"
            Else
            End If
            Dim check As String = ""
            Dim objinv As New vbInventory
            objinv.Storeno = ViewState("storeno")
            check = objinv.fngetreordersdetails()
            If check IsNot Nothing Then
                If check.ToLower = "y" Then
                    trNormalStock.Visible = True
                    trReorder.Visible = True
                Else
                    trNormalStock.Visible = False
                    trReorder.Visible = False
                End If
            Else
            End If
            check = objinv.fngetStyleinfo()
            If check IsNot Nothing Then
                If check.ToLower = "y" Then
                    trstyle.Visible = True
                    trstyle.Visible = True
                Else
                    trstyle.Visible = False
                    trstyle.Visible = False
                End If
            Else
            End If


            ViewState("GridCustompage") = CInt(ConfigurationManager.AppSettings("GridCustompage"))
            ViewState("Inventoryadvancesearch") = False  ' this viewstate used for calling advance search function  or normal function
            getsalesTax()
            txtPoints.Text = "1"
            checkpoints()
            getGlobalSetupInventory()
            ViewState("emp_id") = Session("emp_id")
            ViewState("storeno") = Session("storeno")
            ViewState("isadmin") = Session("isAdmin")
            ViewState("namecksecurity") = Session("name")
            ViewState("stationo") = Session("stationno")
            Call getEmployeesecurity()
            ViewState("editsecurity") = objsecuritycheck.fncheckSecurityformodule(vbSecuritycheck.modulename.EditingItemsIN, ViewState("security"))
            ViewState("addescurity") = objsecuritycheck.fncheckSecurityformodule(vbSecuritycheck.modulename.AddingItemsIN, ViewState("security"))

            Call getdeptvalues()
            Call getDivision()
            Call getDiscGroupvalues()
            Call getsizevalues()
            Call fnGetBottlenoseEcomm()
            Call CheckBottleDeposit()
            Call getallstoresetting()
            ddlStyle.Visible = False

            txtSKU.Text = Request.QueryString("sku").ToString()
            txtDesc.Text = ""
            txtVendorid.Text = ""
            txtNormalStock.Text = "0"
            txtQtypercase.Text = "0"
            txtReorder.Text = "0"
            txtFlattax.Text = ViewState("flatvalue").ToString
            txtDesc.Enabled = True
            txtQtypercase.Enabled = True
            txtNormalStock.Enabled = True
            txtReorder.Enabled = True
            txtSKU.Enabled = True
            'ddlDiscGroup.Enabled = True
            chkBarcode.Checked = False
            chksalestax.Checked = True
            chkwinetax.Checked = True
            chkmisctax.Checked = True
            chkBarcode.Enabled = True
            chksalestax.Enabled = True
            chkwinetax.Enabled = True
            chkmisctax.Enabled = True
            txtFlattax.Enabled = True
            'ddlvendor.Enabled = True
            ddldept.Enabled = True
            ddlStyle.Enabled = True
            ddlSize.Enabled = True
            chksalestax.Checked = False
            chkmisctax.Checked = False
            chkwinetax.Checked = False

            If ViewState("IsItemScript") = True Then
                lnkItemScript.Visible = True
            Else
                lnkItemScript.Visible = False
            End If

        Else
        End If
        If Session("IsDiscGroup") = False Then
            'ddlDiscGroup.Enabled = False
            trdiscount.Visible = False
        Else
            trdiscount.Visible = True
        End If
        txtDesc.Focus()
        'ScriptManager.RegisterStartupScript(Me.Page, Page.GetType(), "DescFocus", "document.getElementById(" + txtDesc.ClientID + ").focus();", False)
    End Sub
    Public Sub CheckBottleDeposit()
        Dim check As String = ""
        Dim objinv As New vbInventory
        objinv.Storeno = ViewState("storeno")
        check = objinv.fncheckbottledeposit()

        Dim amount As Double = 0.0
        objinv.Storeno = ViewState("storeno")
        amount = objinv.fngetbottledeposit()
        ViewState("bottledepositamount") = amount

        If check IsNot Nothing Then
            If check.ToLower = "y" Then
                trdiscgroup.Visible = True
                chkbottledeposit.Checked = False
                txtbottledeposit.Visible = False
            Else
                trdiscgroup.Visible = False
            End If
        End If
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
            If objdataset.Tables(0).Rows.Count > 0 Then
                ddldept.Visible = True
                lblnodept.Visible = False
                ddldept.DataSource = objdataset.Tables(0)
                ddldept.DataTextField = "dept_desc"
                ddldept.DataValueField = "dept_id"
                ddldept.DataBind()
                ddldept.Items.Insert(0, New ListItem("--Select--", ""))
                ddldept.SelectedIndex = 0
            Else
                ddldept.Visible = False
                lblnodept.Visible = True
            End If
        Catch ex As Exception
            Response.Write(ex.ToString)
        Finally
            objconnection.DBDisconnect()
        End Try
    End Sub
    Public Sub getDiscGroupvalues()
        Dim objDiscGrp As New vbDiscountgroup
        objDiscGrp.Storeno = Session("storeno")
        Dim dt As New DataTable
        dt = objDiscGrp.fnSelectAll_DiscountGroupInfo()
        Try
            If dt.Rows.Count > 0 Then
                ddlDiscGroup.Items.Clear()
                ddlDiscGroup.DataTextField = "Disc_Grp_Name"
                ddlDiscGroup.DataValueField = "DiscGroup_Id"
                ddlDiscGroup.DataSource = dt
                ddlDiscGroup.DataBind()
                ddlDiscGroup.Items.Insert(0, New ListItem("<Not Assigned>", 0))
                ddlDiscGroup.SelectedIndex = 0
            Else
                ddlDiscGroup.Items.Clear()
                ddlDiscGroup.Items.Insert(0, New ListItem("<Not Assigned>", 0))
                ddlDiscGroup.SelectedIndex = 0
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Public Sub getsizevalues()
        Dim dt As New DataTable
        Dim objinv As New vbInventory
        objinv.Storeno = ViewState("storeno")
        Dim check As String = ""
        check = objinv.fngetsizeinfo()
        If check.ToLower = "y" Then
            trsize.Visible = True
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
            objdataadapter.Fill(objdataset)
            If objdataset.Tables(0).Rows.Count > 0 Then
                If ViewState("SearchCriteria") = "All" Then
                Else
                    ddlSize.Visible = True
                    lblNoSize.Visible = False
                    ddlSize.DataSource = objdataset.Tables(0)
                    ddlSize.DataTextField = "size"
                    ddlSize.DataValueField = "size_id"
                    ddlSize.DataBind()
                    ddlSize.Items.Insert(0, New ListItem("--Select--", ""))
                End If
            Else
                ddlSize.Visible = False
                lblNoSize.Visible = True
            End If
            objconnection.DBDisconnect()
        Else
            trsize.Visible = False
        End If
    End Sub
    Public Sub getsalesTax()
        Dim objConnection As conClass = New conClass()
        objConnection.DBconnect()
        Dim ObjTransaction As SqlTransaction
        ObjTransaction = objConnection.con1.BeginTransaction
        Dim objCommand As SqlCommand = New SqlCommand()
        objCommand.Connection = objConnection.con1
        objCommand.CommandText = "proc_gs_get_layout_salestax"
        objCommand.CommandType = CommandType.StoredProcedure
        objCommand.Transaction = ObjTransaction
        objCommand.Parameters.AddWithValue("@storeno", ViewState("storeno"))
        Dim ObjDataAdapter As SqlDataAdapter = New SqlDataAdapter()
        ObjDataAdapter.SelectCommand = objCommand
        Dim objDataset As DataSet = New DataSet()

        ObjDataAdapter.Fill(objDataset, "salestax")

        If objDataset.Tables(0).Rows.Count > 0 Then
            lblsalestax.Text = objDataset.Tables(0).Rows(0)("taxname").ToString().Trim() + "  "
            lblwinetax.Text = objDataset.Tables(0).Rows(0)("tax2name").ToString().Trim() + "  "
            lblmisctax.Text = objDataset.Tables(0).Rows(0)("tax3name").ToString().Trim() + "  "
            lblflattax.Text = objDataset.Tables(0).Rows(0)("flatname").ToString().Trim() + "  "
            If objDataset.Tables(0).Rows(0)("Tax") = True Then
                trsalestax.Visible = True
                lblsalestax.Visible = True
            Else
                trsalestax.Visible = False
                lblsalestax.Visible = False
            End If
            If objDataset.Tables(0).Rows(0)("Tax2") = True Then
                trwinetax.Visible = True
                lblwinetax.Visible = True
            Else
                trwinetax.Visible = False
                lblwinetax.Visible = False
            End If
            If objDataset.Tables(0).Rows(0)("Tax3") = True Then
                trmisctax.Visible = True
                lblmisctax.Visible = True
            Else
                trmisctax.Visible = False
                lblmisctax.Visible = False
            End If
            If objDataset.Tables(0).Rows(0)("flatTax") = True Then
                txtFlattax.Visible = True
                lblflattax.Visible = True
                ViewState("flattax") = True
                ViewState("flatvalue") = objDataset.Tables(0).Rows(0)("flat$")
            Else
                txtFlattax.Text = ""
                txtFlattax.Visible = False
                lblflattax.Visible = False
                ViewState("flattax") = False
                ViewState("flatvalue") = "0.00"
            End If
        End If

    End Sub
    Public Sub checkpoints()
        Dim objinventory As New vbInventory
        Dim dt As New DataTable
        objinventory.Storeno = ViewState("storeno")

        dt = objinventory.fnCheckpoints

        If dt.Rows.Count > 0 Then
            ViewState("virtualinventory") = IIf(dt.Rows(0)("virtual_inv").ToString.Trim = True, "Y", "N")
            If dt.Rows(0)("FrequentBuyer").ToString.Trim = "Y" Then
                If dt.Rows(0)("pointbasis").ToString.Trim = "onItem" Then
                    ViewState("points") = "Y"
                    trloyalty.Visible = True
                    lblPoints.Enabled = True
                    txtPoints.Enabled = True
                Else
                    ViewState("points") = "N"
                    trloyalty.Visible = True
                    lblPoints.Enabled = False
                    txtPoints.Text = ""
                    txtPoints.Enabled = False

                End If
            Else
                ViewState("points") = "N"
                trloyalty.Visible = True
                lblPoints.Enabled = False
                txtPoints.Text = ""
                txtPoints.Enabled = False
            End If
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
                If ViewState("qtypercase") = "N" Then
                    trQtypercase.Visible = False
                    trqtypercasereceive.Visible = False
                Else
                    trQtypercase.Visible = True
                    trqtypercasereceive.Visible = True
                End If
            Else
                ViewState("size") = "N"
                ViewState("qtypercase") = "N"
                trQtypercase.Visible = False
                trqtypercasereceive.Visible = False
            End If
        End If
    End Sub
    Private Sub getEmployeesecurity()
        Dim objemployee As New vbSecuritycheck
        objemployee.Employee_Id = ViewState("emp_id")
        objemployee.Storeno = ViewState("storeno")
        objemployee.Modulecheck = "IN"
        Dim objDatatable As DataTable
        objDatatable = objemployee.fngetallmoduleSecurity()
        If objemployee.BolError = False Then
            If objDatatable.Rows.Count > 0 Then
                If objDatatable.Rows(0).Item("employee_id") > 0 Then
                    ViewState("security") = objDatatable
                Else
                    ViewState("security") = Nothing
                End If
            End If
        Else
            ViewState("security") = Nothing
        End If
    End Sub
    Protected Sub ddldept_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddldept.SelectedIndexChanged
        getdepttaxvalues()
        Dim objinv As New vbInventory
        objinv.Storeno = ViewState("storeno")
        Dim check As String = objinv.fngetStyleinfo()
        If ddldept.SelectedItem.Value = "" Or ddldept.SelectedIndex = 0 Then
            ddlStyle.Visible = False
            chksalestax.Checked = True
            chkmisctax.Checked = True
            chkwinetax.Checked = True
            ddldept.Focus()
            Exit Sub
        End If
        If check = "" Or check.ToLower = "n" Then
            trstyle.Visible = False
            If ddldept.SelectedItem.Text <> "" Then
                Dim deptid As Integer = ddldept.SelectedItem.Value
                ViewState("deptid") = deptid
            End If
            If ddlStyle.Visible = True Then
                ddlStyle.Focus()
            ElseIf ddlSize.Visible = True Then
                ddlSize.Focus()
            ElseIf txtQtypercase.Visible = True Then
                txtQtypercase.Focus()
            Else
                'ddlvendor.Focus()
            End If

            'ddldept.Focus()
        ElseIf check.ToLower = "y" Then
            trstyle.Visible = True
            If ddldept.SelectedItem.Text <> "" Then
                Dim deptid As Integer = ddldept.SelectedItem.Value
                ViewState("deptid") = deptid
                Call getstylevalues()

            End If
            If ddlStyle.Visible = True Then
                ddlStyle.Focus()
            ElseIf ddlSize.Visible = True Then
                ddlSize.Focus()
            Else
                txtQtypercase.Focus()
            End If
            'ddldept.Focus()

        End If
    End Sub


    Public Sub getstylevalues()
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
            If ViewState("SearchCriteria") = "All" Then

            Else
                ddlStyle.Visible = True
                lblNoStyle.Visible = False
                ddlStyle.DataSource = objdataset.Tables(0)
                ddlStyle.DataTextField = "style"
                ddlStyle.DataValueField = "style_id"
                ddlStyle.DataBind()
                ddlStyle.Items.Insert(0, New ListItem("--Select--", ""))

            End If
            ddlStyle.Visible = True
        Else
            ddlStyle.Visible = False
            lblNoStyle.Visible = True

        End If
        objconnection.DBDisconnect()
    End Sub
    Public Sub getdepttaxvalues()
        Dim objstyle As New vbInventory()
        Dim dt As New DataTable
        objstyle.Storeno = ViewState("storeno")
        objstyle.Department = IIf(ddldept.SelectedValue = "", 0, ddldept.SelectedValue)
        dt = objstyle.fnGettaxDepartment()
        If dt.Rows.Count > 0 Then
            ddldept.SelectedItem.Text = dt.Rows(0)("dept_desc").ToString.Trim
            ViewState("InvDepartment") = dt.Rows(0)("dept_desc").ToString.Trim
            hdMarkupDept.Value = dt.Rows(0)("dept_desc").ToString.Trim
            chksalestax.Checked = dt.Rows(0)("tax1").ToString
            chkwinetax.Checked = dt.Rows(0)("tax2").ToString
            chkmisctax.Checked = dt.Rows(0)("tax3").ToString
            chkDiscountable.Checked = dt.Rows(0)("discountable").ToString
            If ViewState("points") <> "N" Then
                txtPoints.Text = dt.Rows(0)("loyaltypts").ToString
            Else
                txtPoints.Text = ""
            End If
            ViewState("ItemMarkup") = dt.Rows(0)("markup").ToString
            ViewState("Dept") = IIf(ddlStyle.SelectedValue = "", 0, ddldept.SelectedValue)
        End If
    End Sub
    Protected Sub BindDivision() ' Kinjal [02-12-2013]
        Dim objinv As New vbInventory
        Dim dt As New DataTable
        Dim dtcmb As New DataTable
        objinv.Storeno = ViewState("storeno")
        objinv.VendorInv = Request.QueryString("vendorid").ToString()
        hdnvendorid.Value = Request.QueryString("vendorid").ToString()
        dt = objinv.fnGetDivision()
        If dt.Rows.Count > 0 Then
            'Dim dr() As DataRow = dt.Select("value <> ''")
            Dim dr() As DataRow = dt.[Select]("value <> ''", "rownumber ASC")
            If dr.Count > 0 Then
                dtcmb = dt.Clone
                For Each drrow In dr
                    dtcmb.ImportRow(drrow)
                Next
                ddlDivision.DataTextField = "value"
                ddlDivision.DataValueField = "rownumber"
                ddlDivision.DataSource = dtcmb
                ddlDivision.DataBind()
                ddlDivision.Items.Insert(0, New ListItem("--Select--", " "))
                ddlDivision.SelectedIndex = 0
                trDivision.Visible = True
                ddlDivision.Focus()
            Else
                txtVendorid.Focus()
                trDivision.Visible = False
            End If
            txtVendorid.Focus()
        End If
    End Sub
    Protected Sub btnSavedetails_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSavedetails.Click
        If Page.IsValid.ToString = False Then
            Exit Sub
        End If
        Try
           Dim objconnection As conClass = New conClass
            objconnection.DBDisconnect()
            objconnection.DBconnect()
            ''create sqlcommand
            Dim objcommand As SqlCommand = New SqlCommand
            Dim objparameterreturn As SqlParameter = New SqlParameter("returnvalue", SqlDbType.VarChar, 50)
            objparameterreturn.Direction = ParameterDirection.ReturnValue
            objcommand.Parameters.Clear()
            objcommand.Connection = objconnection.con1
            objcommand.CommandType = CommandType.StoredProcedure
            objcommand.CommandText = "proc_inventory_add_now"
            objcommand.Parameters.AddWithValue("@storeno", ViewState("storeno"))
            objcommand.Parameters.AddWithValue("@item_id", DBNull.Value)
            objcommand.Parameters.AddWithValue("@stationno", ViewState("stationno"))
            objcommand.Parameters.AddWithValue("@user", ViewState("User"))
            objcommand.Parameters.AddWithValue("@item_type", ViewState("itemtype"))
            objcommand.Parameters.AddWithValue("@item", ViewState("Type"))
            If Not ViewState("Type") = "C" Then
                If txtSKU.Text.Trim = "" Then
                    objcommand.Parameters.AddWithValue("@sku", "0")
                Else
                    objcommand.Parameters.AddWithValue("@sku", txtSKU.Text.Trim.ToUpper)
                End If
                objcommand.Parameters.AddWithValue("@desc1", VbGenralfunctions.MakeInitialCaps(txtDesc.Text.Trim))

                If chkBarcode.Checked = True Then
                    objcommand.Parameters.AddWithValue("@barcode", "Y")
                Else
                    objcommand.Parameters.AddWithValue("@barcode", "N")
                End If

                If chkselloninternet.Checked = True Then
                    objcommand.Parameters.AddWithValue("@sn", "Y")

                Else
                    objcommand.Parameters.AddWithValue("@sn", "N")
                End If

                'objcommand.Parameters.AddWithValue("@salestax", chksalestax.Checked)
                'objcommand.Parameters.AddWithValue("@winetax", chkwinetax.Checked)
                'objcommand.Parameters.AddWithValue("@misctax", chkmisctax.Checked)

                objcommand.Parameters.AddWithValue("@salestax", IIf(chksalestax.Visible = True, chksalestax.Checked, False))
                objcommand.Parameters.AddWithValue("@winetax", IIf(chkwinetax.Visible = True, chkwinetax.Checked, False))
                objcommand.Parameters.AddWithValue("@misctax", IIf(chkmisctax.Visible = True, chkmisctax.Checked, False))

                objcommand.Parameters.AddWithValue("@Flattax", ViewState("flattax"))
                If txtFlattax.Text <> "" Then
                    objcommand.Parameters.AddWithValue("@Flatvalue", txtFlattax.Text.Trim)
                Else
                    objcommand.Parameters.AddWithValue("@Flatvalue", "0.00")
                End If

                If chkDiscountable.Checked = True Then
                    objcommand.Parameters.AddWithValue("@Discountable", "Y")
                Else
                    objcommand.Parameters.AddWithValue("@Discountable", "N")
                End If

                If trQtypercase.Visible = True Then
                    If txtQtypercase.Text.Trim = "" Then
                        objcommand.Parameters.AddWithValue("@qtypercase", "0")
                    Else
                        objcommand.Parameters.AddWithValue("@qtypercase", txtQtypercase.Text.Trim)
                    End If
                Else
                    objcommand.Parameters.AddWithValue("@qtypercase", "0")
                End If

                If trqtypercasereceive.Visible = True Then
                    If rdoUnitandcase.Checked Then
                        objcommand.Parameters.AddWithValue("@Cases_units", False)
                    Else
                        objcommand.Parameters.AddWithValue("@Cases_units", True)
                    End If
                Else
                    objcommand.Parameters.AddWithValue("@Cases_units", False)
                End If
                

                If txtNormalStock.Text.Trim = "" Then
                    objcommand.Parameters.AddWithValue("@normal_stk", "0")
                Else
                    objcommand.Parameters.AddWithValue("@normal_stk", txtNormalStock.Text.Trim)
                End If

                If txtReorder.Text.Trim = "" Then
                    objcommand.Parameters.AddWithValue("@reorder", "0")
                Else
                    objcommand.Parameters.AddWithValue("@reorder", txtReorder.Text.Trim)
                End If

                If txtVendorid.Text = "" Then
                    objcommand.Parameters.AddWithValue("@vend_item_id", DBNull.Value)
                Else
                    objcommand.Parameters.AddWithValue("@vend_item_id", txtVendorid.Text.Trim)
                End If

                If lblVendorName.Text = "" Then
                    objcommand.Parameters.AddWithValue("@vendor_id", "0")
                Else
                    objcommand.Parameters.AddWithValue("@vendor_id", Request.QueryString("vendorid").ToString())
                End If

                If trDivision.Visible = True Then
                    If ddlDivision.SelectedValue.Trim = "" Then
                        objcommand.Parameters.AddWithValue("@division", "")
                    Else
                        If ddlDivision.SelectedIndex = 0 Then
                            objcommand.Parameters.AddWithValue("@division", "")
                        Else
                            objcommand.Parameters.AddWithValue("@division", ddlDivision.SelectedValue)
                        End If
                    End If
                Else
                    objcommand.Parameters.AddWithValue("@division", "0")
                End If


                If ddldept.SelectedValue.Trim = "" Then
                    ViewState("deptid") = "0"
                Else
                    If ddldept.SelectedIndex = 0 Then
                        ViewState("deptid") = "0"
                    End If
                End If

                If trdiscgroup.Visible = True Then 'Functionality for add deposit
                    If chkbottledeposit.Checked = True Then
                        objcommand.Parameters.AddWithValue("@Deposit", IIf(txtbottledeposit.Text = "", DBNull.Value, txtbottledeposit.Text))
                        objcommand.Parameters.AddWithValue("@FlagDeposit", "Y")
                    Else
                        objcommand.Parameters.AddWithValue("@Deposit", DBNull.Value)
                        objcommand.Parameters.AddWithValue("@FlagDeposit", "N")
                    End If
                Else
                    objcommand.Parameters.AddWithValue("@Deposit", DBNull.Value)
                    objcommand.Parameters.AddWithValue("@FlagDeposit", "N")
                End If


                objcommand.Parameters.AddWithValue("@department", ViewState("deptid"))

                If ddlStyle.SelectedValue.Trim = "" Then
                    objcommand.Parameters.AddWithValue("@style", "0")
                Else
                    If ddlStyle.SelectedIndex = 0 Then
                        objcommand.Parameters.AddWithValue("@style", "0")
                    Else
                        objcommand.Parameters.AddWithValue("@style", ddlStyle.SelectedValue)
                    End If
                End If

                objcommand.Parameters.AddWithValue("@DiscGrpid", CInt(ddlDiscGroup.SelectedValue))

                If trsize.Visible = True Then
                    If ddlSize.SelectedValue.Trim = "" Then
                        objcommand.Parameters.AddWithValue("@size", "0")
                    Else
                        If ddlSize.SelectedIndex = 0 Then
                            objcommand.Parameters.AddWithValue("@size", "0")
                        Else
                            objcommand.Parameters.AddWithValue("@size", ddlSize.SelectedValue)
                        End If
                    End If
                Else
                    objcommand.Parameters.AddWithValue("@size", "0")
                End If


                If txtPoints.Enabled = True Then
                    objcommand.Parameters.AddWithValue("@points", txtPoints.Text.Trim.ToString)
                Else
                    objcommand.Parameters.AddWithValue("@points", 0)
                End If

                If Session("InventoryItemScript") <> "" Then
                    objcommand.Parameters.AddWithValue("@ItemScript", Session("InventoryItemScript"))

                End If

            Else
            End If
            objcommand.Parameters.Add(objparameterreturn)
            'Try
            objcommand.ExecuteNonQuery()
            Dim i As String = objcommand.Parameters("returnvalue").Value
            If i >= 0 Then
                If Session("IsCorpOffice") Then
                    objcommand.Connection = objconnection.con1
                    objcommand.CommandType = CommandType.StoredProcedure
                    objcommand.CommandText = "proc_inventory_add_now_corporate_store"

                    objcommand.CommandTimeout = 1000
                    objcommand.ExecuteScalar()
                End If
                objconnection.DBDisconnect()
            Else
                If i = -2 Then
                    Call SetAlertMessage("A product with this SKU already exists")
                End If
            End If

            objconnection.DBDisconnect()
        Catch ex As Exception
            '    ScriptManager.RegisterStartupScript(Page, [GetType], "Error", "alert('" & ex.Message & "');", True)
        End Try
        Session("InventoryItemScript") = ""
        Dim jscriptString1 As String = "<script language='JavaScript' type='text/javascript'>" _
                                     & " window.returnValue = '" & txtSKU.Text & "';" _
                                     & " window.close();" _
                                     & " </script>"
        ScriptManager.RegisterStartupScript(Me.Page, Page.GetType(), "jscriptString", jscriptString1, False)
    End Sub
    Public Sub SetAlertMessage(ByVal alertMessage As String)
        'To popup message box using Javascript 
        Dim sb As String
        sb = "alert"
        sb = sb + "("""
        sb = sb + alertMessage
        sb = sb + """);"
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "AlertMessageHandler", sb.ToString(), True)
    End Sub
    Protected Sub ddlStyle_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlStyle.SelectedIndexChanged
        getDepartment()
    End Sub
    Public Sub fnGetBottlenoseEcomm()
        Dim objBottlenose As New vbStationSetup()
        Dim dt As New DataTable
        objBottlenose.StoreNo = ViewState("storeno")

        dt = objBottlenose.fnGetBottlenoseEcomm
        If dt.Rows.Count > 0 Then
            If dt.Rows(0)("bottlenoseEcomm").ToString = "True" Then
                TrSaleonInternet.Visible = True
            Else
                TrSaleonInternet.Visible = False
            End If

        End If
        ddlSize.Focus()
    End Sub
    Public Sub getDepartment()
        Dim objstyle As New vbInventory()
        Dim dt As New DataTable
        objstyle.Storeno = ViewState("storeno")
        objstyle.StyleId = IIf(ddlStyle.SelectedValue = "", 0, ddlStyle.SelectedValue)
        dt = objstyle.fnGetStyleDepartment()
        If dt.Rows.Count > 0 Then
            'lblDepartment.Text = dt.Rows(0)("dept_desc").ToString.Trim
            ddldept.SelectedItem.Text = dt.Rows(0)("dept_desc").ToString.Trim
            ViewState("InvDepartment") = dt.Rows(0)("dept_desc").ToString.Trim
            hdMarkupDept.Value = dt.Rows(0)("dept_desc").ToString.Trim
            chksalestax.Checked = dt.Rows(0)("tax1").ToString
            chkwinetax.Checked = dt.Rows(0)("tax2").ToString
            chkmisctax.Checked = dt.Rows(0)("tax3").ToString
            'chkFlattax.Checked = dt.Rows(0)("flattax").ToString
            chkDiscountable.Checked = dt.Rows(0)("discountable").ToString
            ViewState("ItemMarkup") = dt.Rows(0)("markup").ToString

            If ViewState("points") <> "N" Then
                txtPoints.Text = dt.Rows(0)("loyaltypts").ToString
            Else
                txtPoints.Text = ""
            End If




            ViewState("Dept") = IIf(ddlStyle.SelectedValue = "", 0, dt.Rows(0)("deptId").ToString)
        End If
        If trsize.Visible = True Then
            ddlSize.Focus()
        Else
            txtQtypercase.Focus()
        End If
    End Sub

    Public Sub getDivision()

        'Code for checking if there exists division in vendor or not. Kinjal [02-12-2013]
        Dim dt1 As DataTable
        Dim objinv As New vbInventory()
        objinv.VendorInv = Request.QueryString("vendorid").ToString()
        objinv.Storeno = ViewState("storeno")

        dt1 = objinv.fnCheckVendor()
        If dt1.Rows.Count > 0 Then
            lblVendorName.Visible = True
            lblVendorName.Text = dt1.Rows(0)("company").ToString().Trim()
            Session("Vendor") = lblVendorName.Text.Trim()
            'lblVendorName.Text = objinv.VendorName
            Dim chkven As Integer = dt1.Select("division1 <> '' or division2 <> '' or division3 <> '' or division4 <> '' or division5 <> ''").Count
            If chkven > 0 Then
                trDivision.Visible = True
                BindDivision()
            End If
        End If

        ' End code 

        txtVendorid.Focus()

    End Sub

    Protected Sub ddlSize_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlSize.SelectedIndexChanged
        If ViewState("qtypercase") = "Y" Then
            Dim objsize As New vbInventory
            Dim dt As New DataTable
            objsize.Storeno = ViewState("storeno")
            objsize.Size = IIf(ddlSize.SelectedValue = "", 0, ddlSize.SelectedValue)
            dt = objsize.fnGetSizeQty()
            If dt.Rows.Count > 0 Then
                txtQtypercase.Text = dt.Rows(0)("qtypercase").ToString.Trim
                txtQtypercase.Focus()
            Else
                txtQtypercase.Focus()
            End If
        End If
    End Sub

    Protected Sub chkbottledeposit_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkbottledeposit.CheckedChanged
        If chkbottledeposit.Checked Then
            txtbottledeposit.Visible = True
            txtbottledeposit.Text = FormatNumber(ViewState("bottledepositamount").ToString(), 2, TriState.True)
            txtbottledeposit.Focus()
        Else
            txtbottledeposit.Visible = False
        End If
    End Sub

    Public Sub getallstoresetting()
        Dim objGlobalsetup As New vbRingsale
        'If Convert.ToBoolean(Session("IsCorpOffice")) = True Then
        '    objGlobalsetup.StoreNo = ViewState("LocationStoreno").ToString.Trim
        'Else
        objGlobalsetup.StoreNo = Session("storeno")
        ' End If

        Dim objDataset As DataSet = objGlobalsetup.fnGetgetallstoresetting()
        If objDataset.Tables.Count > 0 Then
            'POS options
            If objDataset.Tables(0).Rows.Count > 0 Then
                ViewState("IsItemScript") = IIf(IsDBNull(objDataset.Tables(0).Rows(0).Item("IsItemScript")), False, objDataset.Tables(0).Rows(0).Item("IsItemScript"))

            End If
        End If

    End Sub

    Protected Sub btnCanceldetails_Click(sender As Object, e As EventArgs) Handles btnCanceldetails.Click
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "CloseWindow", "CloseWindow();", True)
        Session("InventoryItemScript") = ""
    End Sub

    'Protected Sub lnkItemScript_Click(sender As Object, e As EventArgs)
    '    ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "ItemScriptOpen", "OpenItemScriptPopup('" + txtSKU.Text + "')", True)
    'End Sub

    Protected Sub rdoUnitandcase_CheckedChanged(sender As Object, e As EventArgs) Handles rdoUnitandcase.CheckedChanged
        trQtypercase.Visible = True
    End Sub

    Protected Sub rdoUnits_CheckedChanged(sender As Object, e As EventArgs) Handles rdoUnits.CheckedChanged
        trQtypercase.Visible = False
    End Sub
End Class
