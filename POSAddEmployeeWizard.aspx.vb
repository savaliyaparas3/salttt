Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Sql
Imports System.Net.Mail
Imports System.String
Imports System.Globalization
Partial Class POSAddEmployeeWizard
    Inherits System.Web.UI.Page
    Dim objDataset As DataSet = New DataSet()
    Dim MyDataTable As New DataTable
    Dim MyDataColumn As New DataColumn
    Public boolNotId As Boolean = False
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        uchelpresetpwd.mpopupHelp = mpopupHelp
        If Convert.ToBoolean(Session("IsCorpOffice")) = True Then
            lblAddEmpwizard.Text = "<p><b>Welcome to the Lightning Corporate Office New Employee Wizard.</b></p><p>Let’s get started, first, please ensure you have the mobile number and email address for your new employee ready. You will be presented with options to set security rights, email alerts, which stores within your network they can access and more. </p><p>Once the new employee logs in for the first time, they will be required to change their password.</p>"
        Else
            lblAddEmpwizard.Text = "<p><b>Welcome to the Lightning Online Point of Sale New Employee Wizard.</b></p><p>Let’s get started, first, please ensure you have the mobile number and email address for your new employee ready.  You will be presented with options to set security rights, discount limits, email alerts and more.</p><p>Once the new employee logs in for the first time, they will be required to change their password.</p> "
        End If
        If Not IsPostBack Then
            MultiViewEmpAdd.ActiveViewIndex = 0
            btnCntinueSave.Text = "Continue"
            btnBack.Visible = False
            ViewState("country") = Session("country")
            ViewState("storeno") = Session("storeno")
            ViewState("storelistsortcolumn") = "storeno"
            ViewState("storelistsortstatus") = "ASC"
            ViewState("allstore") = "N"
            Session("CHECKEDSTORE") = Nothing
            upnlAddEmpWizard.Update()
        End If
       
    End Sub

#Region "Page Method"



    <System.Web.Services.WebMethod()> _
  <System.Web.Script.Services.ScriptMethod()> _
    Public Shared Function GetEmployeeInfo(ByVal ID As String) As String
        Dim objemployee As New vbEmployee
        Dim result As Integer = 0
        objemployee.Employee_Id = ID
        result = objemployee.fnCheckDuplicateEmpId()


        Return result
    End Function
    <System.Web.Services.WebMethod()> _
    <System.Web.Script.Services.ScriptMethod()> _
    Public Shared Function getCitystate(ByVal zip As String, ByVal countrycode As String) As String

        Dim result As String = ""
        Dim objconnection As conClass = New conClass

        objconnection.DBconnect()
        Dim objcommand As SqlCommand = New SqlCommand

        objcommand.CommandType = CommandType.StoredProcedure
        objcommand.Connection = objconnection.con1
        objcommand.CommandText = "proc_ringsale_zipcitystate_getcitystate"
        objcommand.CommandType = CommandType.StoredProcedure
        'cls.mycommand = New SqlCommand(cls.strsql1, cls.con1)
        'cls.mycommand.CommandType = CommandType.StoredProcedure

        Dim objParameter As SqlParameter = New SqlParameter("@returnvalue", "0")
        objParameter.Direction = ParameterDirection.ReturnValue
        objcommand.Parameters.Add(objParameter)
        objcommand.Parameters.AddWithValue("@countrycode", countrycode)
        objcommand.Parameters.AddWithValue("@zip", zip)

        Dim objDataset As New DataSet
        Dim objDataadapter As New SqlDataAdapter
        objDataadapter.SelectCommand = objcommand

        'objconnection.DBconnect()

        objDataadapter.Fill(objDataset, "storedetails")

        If objDataset IsNot Nothing Then
            If objDataset.Tables.Count > 0 Then
                If objDataset.Tables(0).Rows.Count > 0 Then
                    If objcommand.Parameters("@returnvalue").Value > 0 Then
                        result = objDataset.Tables(0).Rows(0)("city").ToString()
                    Else
                        result = "N"
                    End If
                Else
                    result = "N"
                End If
            Else
                result = "N"
            End If
        Else
            result = "N"
        End If

        Return result

    End Function

#End Region
#Region "Employee Add / Save"
    Protected Sub btnCntinueSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCntinueSave.Click
        Dim objEmp As New vbEmployee
        If MultiViewEmpAdd.ActiveViewIndex = 0 Then
            btnCntinueSave.Text = "Continue / Save"
            btnBack.Visible = True
            txtEmpId.Focus()
            If Not Session("IsCorpOffice") Is Nothing AndAlso CType(Session("IsCorpOffice"), Boolean) = False Then
                drpEmproles.Visible = False
                truserrole.Visible = False
            Else
                getEmployeeRoles()
            End If
            MultiViewEmpAdd.ActiveViewIndex = 1
            ScriptManager.RegisterStartupScript(Page, [GetType], "GOOGLEAPI", "initAutocomplete();", True)
        ElseIf MultiViewEmpAdd.ActiveViewIndex = 1 Then
            objEmp.Employee_Id = CultureInfo.CurrentCulture.TextInfo.ToUpper(txtEmpId.Text.Trim)
            lblEmployeeID.Text = CultureInfo.CurrentCulture.TextInfo.ToUpper(txtEmpId.Text.Trim)
            objEmp.Fname = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(txtFName.Text.Trim)
            objEmp.Lname = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(txtLName.Text.Trim)
            objEmp.Address = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(txtaddress.Text.Trim)
            objEmp.Zip = txtZip.Text.Trim

            If hdnCityState.Value.Trim <> "" Then
                Dim str() As String = hdnCityState.Value.Trim.Split(",")
                objEmp.City = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str(0).Trim)
                objEmp.State = CultureInfo.CurrentCulture.TextInfo.ToUpper(str(1).Trim)
            Else
                objEmp.City = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(txtCity.Text.Trim)
                objEmp.State = CultureInfo.CurrentCulture.TextInfo.ToUpper(txtState.Text.Trim)
            End If

            hdnMbBefore.Value = txtMobilePhone.Text.Trim()
            ' objEmp.Phone = Convert.ToUInt64(txtMobilePhone.Text.ToString.Trim.Replace("(", "").Replace(")", "").Replace(" ", "").Replace("-", ""))
            objEmp.Phone = txtMobilePhone.Text.Trim

            objEmp.Email = txtemail.Text.Trim
            objEmp.Storeno = Session("storeno")
            objEmp.Stationno = Session("stationno")
            objEmp.user = Session("emp_id")
            '' FOR SHOWING THE NAME in Initcap format  i.e. Chaudhary Kinjal..... so take this value in the hiden field.
            hdnfirstnameBefore.Value = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(txtFName.Text.Trim)
            hdnlastnameBefore.Value = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(txtLName.Text.Trim)
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            '' Converted text name into lower to use them in IF condition in Java script. Which is require to generate the temporary password from the name
            hdnfirstname.Value = txtFName.Text.Trim.ToLower()
            hdnlastname.Value = txtLName.Text.Trim.ToLower()
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            hdnmobile.Value = objEmp.Phone.ToString.Trim
            hdnEmailID.Value = objEmp.Email.ToString().Trim()
            If Not Session("IsCorpOffice") Is Nothing AndAlso CType(Session("IsCorpOffice"), Boolean) = False Then
                objEmp.UserRole = 2
                objEmp.AreaRepID = 0
            Else
                objEmp.UserRole = IIf(drpEmproles.SelectedValue = Nothing, 1, CType(drpEmproles.SelectedValue, Integer))
                objEmp.AreaRepID = IIf(drpAreaLocation.SelectedValue = "", 0, drpAreaLocation.SelectedValue)
            End If

            Dim fullname As String = hdnfirstname.Value & " " & hdnlastname.Value
            objEmp.Insert_Employee()
            Dim empid As String = objEmp.GetEmployeeId()
            ViewState("employee_id") = empid
            ViewState("emprole") = drpEmproles.SelectedValue
            hdnEmployeeId.Value = empid
            hdnEmpid.Value = txtEmpId.Text.Trim.ToUpper()
            lblFnamePwd.Text = hdnfirstnameBefore.Value + " "
            lblLnamePwd.Text = hdnlastnameBefore.Value
            txtPassword.Focus()
            ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "setHeader", "setHeader(' " & fullname & "  ');", True)
            MultiViewEmpAdd.ActiveViewIndex = 2
        ElseIf MultiViewEmpAdd.ActiveViewIndex = 2 Then
            Dim objEncode As New vbCustomer
            If hdnPassword.Value <> "" Then
                hdnPassword.Value = txtPassword.Text.Trim
            End If
            hdnPassword.Value = txtPassword.Text.Trim
            Dim password = objEncode.base64Encode(txtPassword.Text.Trim)
            objEmp.Password = password
            objEmp.Storeno = Session("storeno")
            objEmp.Employee_Id = txtEmpId.Text.Trim
            objEmp.Update_EmployeePassword()
            Insert_EmployeePasswordSecurityLog()
            'getSecurity()
            'upSecurity.Update()
            ddlOtherProduct.Focus()
            MultiViewEmpAdd.ActiveViewIndex = 3

            If Convert.ToBoolean(Session("IsCorpOffice")) = True Then
                MultiViewEmpAdd.ActiveViewIndex = 8
                GetStoreList()
                Getstoreaccesslist()
                ViewState("AllowIpad") = False
            Else
                MultiViewEmpAdd.ActiveViewIndex = 3
                upnlDepartment.Update()
            End If

        ElseIf MultiViewEmpAdd.ActiveViewIndex = 3 Then
            ViewState("AllowIpad") = False
            If ddlOtherProduct.SelectedValue = "0" Then
                objEmp.AllowIpad = False
                ViewState("AllowIpad") = False
            Else
                objEmp.AllowIpad = True
                ViewState("AllowIpad") = True
            End If
            objEmp.Employee_Id = ViewState("employee_id")
            objEmp.Storeno = Session("storeno")
            getEmployeeDepartmentlist()
            getDepartmentlist()
            objEmp.Update_EmployeeAllowIpad()
            Insert_AllowIpadSecurityLog()

            If Convert.ToBoolean(Session("IsCorpOffice")) = True Then
                MultiViewEmpAdd.ActiveViewIndex = 8
                GetStoreList()
                Getstoreaccesslist()
            Else
                MultiViewEmpAdd.ActiveViewIndex = 4
                upnlDepartment.Update()
            End If


        ElseIf MultiViewEmpAdd.ActiveViewIndex = 4 Then
           
            Apply_Discount()
            getallstoresetting()
            'If Session("storeno") = "7365" Or Session("storeno") = "2717" Or Session("storeno") = "89" Then
            mvEmployeeSecurity.ActiveViewIndex = 1
            Call getEmployeeSecurityCount()
            'Else
            '    mvEmployeeSecurity.ActiveViewIndex = 0
            '    Call getSecurity()
            'End If
            lblFLName.Text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(hdnlastnameBefore.Value) + " " + CultureInfo.CurrentCulture.TextInfo.ToTitleCase(hdnfirstnameBefore.Value)
            upSecurity.Update()
            MultiViewEmpAdd.ActiveViewIndex = 5
        ElseIf MultiViewEmpAdd.ActiveViewIndex = 5 Then
            
            '  getSecurity()
            '  upSecurity.Update()
            'saveEmpSecurity()
            lblEmpIDValue.Text = hdnEmpid.Value
            lblEmpName.Text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(hdnlastnameBefore.Value) + ", " + CultureInfo.CurrentCulture.TextInfo.ToTitleCase(hdnfirstnameBefore.Value)
            lblEmpMobile.Text = hdnMbBefore.Value
            lblEmpEmail.Text = hdnEmailID.Value
            'btnCntinueSave.Visible = False
            'btnBack.Visible = False
            'btnCancel.Visible = False
            'btnFinish.Visible = True
            btnCntinueSave.Visible = True
            btnBack.Visible = True
            btnCancel.Visible = True
            btnFinish.Visible = False
            If Convert.ToBoolean(Session("IsCorpOffice")) = True Then
                StoreAccessSecurity()
                Session("CHECKEDSTORE") = Nothing
            End If
            MultiViewEmpAdd.ActiveViewIndex = 6
            keySecurity()
        ElseIf MultiViewEmpAdd.ActiveViewIndex = 6 Then
            Save_Email_Alert()
            btnCntinueSave.Visible = False
            btnBack.Visible = False
            btnCancel.Visible = False
            btnFinish.Visible = True
            '  getSecurity()
            '  upSecurity.Update()
            'saveEmpSecurity()
            'lblEmpIDValue.Text = hdnEmpid.Value
            'lblEmpName.Text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(hdnlastnameBefore.Value) + ", " + CultureInfo.CurrentCulture.TextInfo.ToTitleCase(hdnfirstnameBefore.Value)
            'lblEmpMobile.Text = hdnMbBefore.Value
            'lblEmpEmail.Text = hdnEmailID.Value
            'btnCntinueSave.Visible = False
            'btnBack.Visible = False
            'btnCancel.Visible = False
            'btnFinish.Visible = True
            'If Convert.ToBoolean(Session("IsCorpOffice")) = True Then
            '    StoreAccessSecurity()
            '    Session("CHECKEDSTORE") = Nothing
            'End If
            MultiViewEmpAdd.ActiveViewIndex = 7
        ElseIf MultiViewEmpAdd.ActiveViewIndex = 8 Then

            If Convert.ToBoolean(Session("IsCorpOffice")) = True Then
                lnkRingSalesSecurity.Enabled = False
                lnkMarketingCenterSecurity.Enabled = False
                lnkCloseThDaySecurity.Enabled = False
                lblCloseThDaySecurity.Visible = False
                lblMarketingCenterSecurity.Visible = False
                lblRingSalesSecurity.Visible = False

                Savestorelist()
            End If
            getallstoresetting()
            mvEmployeeSecurity.ActiveViewIndex = 1
            Call getEmployeeSecurityCount()
            lblFLName.Text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(hdnlastnameBefore.Value) + " " + CultureInfo.CurrentCulture.TextInfo.ToTitleCase(hdnfirstnameBefore.Value)
            upSecurity.Update()
            MultiViewEmpAdd.ActiveViewIndex = 5
        End If
        upnlAddEmpWizard.Update()
    End Sub
    Protected Sub Help_Click(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs)
        ViewState("Button") = e.CommandName
        uchelpresetpwd.helpid = ViewState("Button")
        uchelpresetpwd.mpopupHelp = mpopupHelp
        uchelpresetpwd.showPopup()
        mpopupHelp.Show()
        upnlDisplayHelp.Update()

    End Sub
    Public Sub Insert_EmployeePasswordSecurityLog()
        Dim ds As New DataSet
        Dim objdal As New conClass
        Dim cmd As New SqlCommand
        Dim da As New SqlDataAdapter
        cmd.Connection = objdal.con1
        cmd.Parameters.AddWithValue("@empid", ViewState("employee_id"))
        cmd.Parameters.AddWithValue("@storeno", Session("storeno"))
        cmd.Parameters.AddWithValue("@station", Session("stationno"))
        cmd.Parameters.AddWithValue("@user", Session("emp_id"))
        cmd.CommandText = "Proc_EmployeeInsertLogForPasswordUpdate"
        cmd.CommandType = CommandType.StoredProcedure
        objdal.DBconnect()
        cmd.ExecuteNonQuery()
        objdal.DBDisconnect()
    End Sub
    Public Sub Insert_AllowIpadSecurityLog()
        Dim ds As New DataSet
        Dim objdal As New conClass
        Dim cmd As New SqlCommand
        Dim da As New SqlDataAdapter
        cmd.Connection = objdal.con1
        cmd.Parameters.AddWithValue("@IpadAccess", ViewState("AllowIpad"))
        cmd.Parameters.AddWithValue("@empid", ViewState("employee_id"))
        cmd.Parameters.AddWithValue("@storeno", Session("storeno"))
        cmd.Parameters.AddWithValue("@station", Session("stationno"))
        cmd.Parameters.AddWithValue("@user", Session("emp_id"))
        cmd.CommandText = "Proc_EmployeeInsertLogForAllowIpad"
        cmd.CommandType = CommandType.StoredProcedure
        objdal.DBconnect()
        cmd.ExecuteNonQuery()
        objdal.DBDisconnect()
    End Sub
    Public Sub Insert_DiscountSecurityLog()
        Dim ds As New DataSet
        Dim objdal As New conClass
        Dim cmd As New SqlCommand
        Dim da As New SqlDataAdapter
        cmd.Connection = objdal.con1
        cmd.Parameters.AddWithValue("@mode", "Add")
        cmd.Parameters.AddWithValue("@dept", ViewState("Department"))
        cmd.Parameters.AddWithValue("@deptID", ViewState("DepartmentID"))
        cmd.Parameters.AddWithValue("@discount", ViewState("Discount"))
        cmd.Parameters.AddWithValue("@empid", ViewState("employee_id"))
        cmd.Parameters.AddWithValue("@storeno", Session("storeno"))
        cmd.Parameters.AddWithValue("@station", Session("stationno"))
        cmd.Parameters.AddWithValue("@user", Session("emp_id"))
        cmd.CommandText = "Proc_EmployeeInsertLogForDiscount"
        cmd.CommandType = CommandType.StoredProcedure
        objdal.DBconnect()
        cmd.ExecuteNonQuery()
        objdal.DBDisconnect()
    End Sub
    Public Sub Apply_Discount()

        Dim objemployee As New vbEmployee
        ViewState("Department") = ""
        ViewState("Discount") = ""
        ViewState("DepartmentID") = ""
        objemployee.Storeno = Session("storeno")

        For i As Integer = 0 To gvDepartment.Rows.Count - 1
            objemployee.EmpDiscDeptId = Convert.ToDouble(CType(gvDepartment.Rows(i).FindControl("lbldeptid"), Label).Text.ToString)
            ViewState("DepartmentID") = Convert.ToDouble(CType(gvDepartment.Rows(i).FindControl("lbldeptid"), Label).Text.ToString)
            ViewState("Department") = CType(gvDepartment.Rows(i).FindControl("lblDepartment"), Label).Text.ToString()
            objemployee.EmpDiscEmpId = ViewState("employee_id").ToString()
            objemployee.EmpDiscDiscount = Convert.ToDouble(CType(gvDepartment.Rows(i).FindControl("txtDiscount"), TextBox).Text.Replace("%", ""))
            ViewState("Discount") = Convert.ToDouble(CType(gvDepartment.Rows(i).FindControl("txtDiscount"), TextBox).Text.Replace("%", ""))
            objemployee.fnDiscount()
            Insert_DiscountSecurityLog()
        Next

    End Sub

    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click
        Dim activeindex As Integer = MultiViewEmpAdd.ActiveViewIndex
        lblEmployeeID.Style.Add("display", "block")
        txtEmpId.Style.Add("display", "none")
        If activeindex = 1 Then
            MultiViewEmpAdd.ActiveViewIndex = 0
            btnBack.Visible = False
            If ViewState("employee_id") = Nothing Then
                txtEmpId.Style.Add("display", "block")
            Else
                txtEmpId.Style.Add("display", "none")
                lblEmployeeID.Style.Add("display", "block")
            End If

        ElseIf activeindex = 5 Then
            If Convert.ToBoolean(Session("IsCorpOffice")) = True Then
                MultiViewEmpAdd.ActiveViewIndex = 8
                grdStores.PageIndex = 0
                Getstoreaccesslist()
                GetStoreList()
                PopulateCheckedValues()
            Else
                MultiViewEmpAdd.ActiveViewIndex = 4
                upnlDepartment.Update()
            End If
        ElseIf activeindex = 7 Then
            If Convert.ToBoolean(Session("IsCorpOffice")) = True Then
                Dim objEmp As New vbEmployee
                MultiViewEmpAdd.ActiveViewIndex = 3
                ViewState("AllowIpad") = False
                If ddlOtherProduct.SelectedValue = "0" Then
                    objEmp.AllowIpad = False
                    ViewState("AllowIpad") = False
                Else
                    objEmp.AllowIpad = True
                    ViewState("AllowIpad") = True
                End If
                objEmp.Employee_Id = ViewState("employee_id")
                objEmp.Storeno = Session("storeno")
                getEmployeeDepartmentlist()
                getDepartmentlist()
                objEmp.Update_EmployeeAllowIpad()
                Insert_AllowIpadSecurityLog()
            End If
        ElseIf activeindex = 8 Then
            'If activeindex = 2 Then
            hdnPassword.Value = txtPassword.Text.Trim
            'End If
            If hdnPassword.Value <> "" Then
                txtPassword.Attributes.Add("value", hdnPassword.Value)
                txtConfirmPassword.Attributes.Add("value", hdnPassword.Value)
                txtPassword.Focus()
            End If
            If txtEmpId.Text <> "" Then
                txtEmpId.Focus()
            End If
            txtEmpId.Enabled = False
            MultiViewEmpAdd.ActiveViewIndex = 2

        ElseIf activeindex = 6 Then
            mvEmployeeSecurity.ActiveViewIndex = 1
            Call getEmployeeSecurityCount()
            'Else
            '    mvEmployeeSecurity.ActiveViewIndex = 0
            '    Call getSecurity()
            'End If
            lblFLName.Text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(hdnlastnameBefore.Value) + " " + CultureInfo.CurrentCulture.TextInfo.ToTitleCase(hdnfirstnameBefore.Value)
            upSecurity.Update()
            MultiViewEmpAdd.ActiveViewIndex = 5
            MultiViewEmpAdd.ActiveViewIndex = (activeindex - 1)
        Else
            MultiViewEmpAdd.ActiveViewIndex = (activeindex - 1)

        End If
        upnlAddEmpWizard.Update()
    End Sub

    Protected Sub imgbtnContinue_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        If hdnCheckDup.Value = "Mail" Then
            txtemail.Text = ""
            txtemail.Focus()
        ElseIf hdnCheckDup.Value = "Part" Then
            txtEmpId.Text = ""
            txtEmpId.Focus()
        End If
        MPEEmployeeCheck.Hide()

        upnlAddEmpWizard.Update()
    End Sub
#End Region
#Region "Tree View"
    Protected Sub TreeView1_SelectedNodeChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TreeView1.SelectedNodeChanged
        Dim img As String = ""
        If ViewState("cksecuritymode") = "D" Then

        Else
            Dim objTreeNode As TreeNode = TreeView1.SelectedNode
            If objTreeNode Is Nothing Then
            Else
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "progress", "TreeviewDblClick();", True)
                If objTreeNode.ImageUrl = "images/lock.gif" Then
                    objTreeNode.ImageUrl = "images/unlock.gif"
                    img = "images/unlock.gif"
                Else
                    objTreeNode.ImageUrl = "images/lock.gif"
                    img = "images/lock.gif"
                End If
                TreeView1.SelectedNode.Selected = False
            End If
            If objTreeNode.Value = "MO1" Then
                Call ChangeTreeView(TreeView1, objTreeNode.Value, img)
            ElseIf objTreeNode.Value = "MO2" Then
                Call ChangeTreeView(TreeView1, objTreeNode.Value, img)
            ElseIf objTreeNode.Value = "MO3" Then
                Call ChangeTreeView(TreeView1, objTreeNode.Value, img)
            ElseIf objTreeNode.Value = "MO4" Then
                Call ChangeTreeView(TreeView1, objTreeNode.Value, img)
            ElseIf objTreeNode.Value = "MO5" Then
                Call ChangeTreeView(TreeView1, objTreeNode.Value, img)
            ElseIf objTreeNode.Value = "MO8" Then
                Call ChangeTreeView(TreeView1, objTreeNode.Value, img)
            ElseIf objTreeNode.Value = "MO9" Then
                Call ChangeTreeView(TreeView1, objTreeNode.Value, img)
            ElseIf objTreeNode.Value = "MO10" Then
                Call ChangeTreeView(TreeView1, objTreeNode.Value, img)
            ElseIf objTreeNode.Value = "MO11" Then
                Call ChangeTreeView(TreeView1, objTreeNode.Value, img)
            ElseIf objTreeNode.Value = "MO13" Then
                Call ChangeTreeViewReports(TreeView1, objTreeNode.Value, img)
            ElseIf objTreeNode.Value = "MO14" Then
                Call ChangeTreeViewReports(TreeView1, objTreeNode.Value, img)
            ElseIf objTreeNode.Value = "MO15" Then
                Call ChangeTreeViewReports(TreeView1, objTreeNode.Value, img)
            ElseIf objTreeNode.Value = "MO16" Then
                Call ChangeTreeViewReports(TreeView1, objTreeNode.Value, img)
            ElseIf objTreeNode.Value = "RE1" Then
                Call ChangeTreeViewReports(TreeView1, objTreeNode.Value, img)
            ElseIf objTreeNode.Value = "RE18" Then
                Call ChangeTreeViewReports(TreeView1, objTreeNode.Value, img)
            ElseIf objTreeNode.Value = "RS3" Then
                Call ChangeTreeViewReports(TreeView1, objTreeNode.Value, img)
            ElseIf objTreeNode.Value = "IN2" Then
                Call ChangeTreeViewEditItem(TreeView1, objTreeNode.Value, img)
            End If
            hdnchange.Value = "T"
            upSecurity.Update()
            upnlAddEmpWizard.Update()

            ScriptManager.RegisterStartupScript(Page, [GetType], "SetCursor", "show('" + hdnSelectedNode.Value + "');", True)
        End If
    End Sub
    Public Function getModuleinfo() As DataTable

        Dim objEmployee As New vbEmployee()
        Dim objDatatable As New DataTable()
        objDatatable = objEmployee.fnGetModuleInfo()

        If objDatatable.Rows.Count > 0 Then
            Return objDatatable
        Else
            Return Nothing
        End If

        objConnection.DBDisconnect()

    End Function
    Private Sub ChangeTreeView(ByVal objTreeview As TreeView, ByVal parent_node As String, ByVal img As String)
        If objTreeview.Nodes.Count > 0 Then
            For Each objChildTree As TreeNode In objTreeview.Nodes
                If objChildTree.Value = parent_node.ToString.Trim Then
                    Call ChangeNode(objChildTree, parent_node, img)
                    Exit For
                End If

            Next
        End If
    End Sub
    Private Sub ChangeTreeViewReports(ByVal objTreeview As TreeView, ByVal parent_node As String, ByVal img As String)
        If objTreeview.Nodes.Count > 0 Then
            For Each objChildTree As TreeNode In objTreeview.Nodes
                If parent_node.ToString.Trim = "RE1" Then
                    Call ChangeNodeReports(objChildTree, parent_node, img)
                ElseIf parent_node.ToString.Trim = "RE18" Then
                    Call ChangeNodeReports(objChildTree, parent_node, img)
                ElseIf parent_node.ToString.Trim = "MO13" Then
                    Call ChangeNodeReports(objChildTree, parent_node, img)
                ElseIf parent_node.ToString.Trim = "MO14" Then
                    Call ChangeNodeReports(objChildTree, parent_node, img)
                ElseIf parent_node.ToString.Trim = "MO15" Then
                    Call ChangeNodeReports(objChildTree, parent_node, img)
                ElseIf parent_node.ToString.Trim = "MO16" Then
                    Call ChangeNodeReports(objChildTree, parent_node, img)
                End If

            Next
        End If
    End Sub

    Private Sub ChangeTreeViewEditItem(ByVal objTreeview As TreeView, ByVal parent_node As String, ByVal img As String)
        If objTreeview.Nodes.Count > 0 Then
            For Each objChildTree As TreeNode In objTreeview.Nodes
                If parent_node.ToString.Trim = "IN2" Then
                    Call ChangeNodeEditItem(objChildTree, parent_node, img)
                ElseIf parent_node.ToString.Trim = "IN21" Then
                    Call ChangeNodeEditItem(objChildTree, parent_node, img)
                End If
            Next
        End If
    End Sub

    Private Sub ChangeNodeEditItem(ByVal objTreenode As TreeNode, ByVal parent_node As String, ByVal img As String)
        If objTreenode.ChildNodes.Count > 0 Then
            If objTreenode.Value = "MO2" Then
                For Each objChildTree1 As TreeNode In objTreenode.ChildNodes
                    If objChildTree1.Value = "MO13" Then
                        For Each objChildTree As TreeNode In objChildTree1.ChildNodes
                            If parent_node.ToString.Trim.ToUpper = "IN2" Then
                                If objChildTree.ChildNodes.Count > 0 Then
                                    Dim Cnt As Integer = objChildTree.ChildNodes.Count
                                    For i = 0 To Cnt - 1
                                        objChildTree.ChildNodes(i).ImageUrl = img.ToString.Trim
                                        If DirectCast(objChildTree.ChildNodes(i), System.Web.UI.WebControls.TreeNode).ValuePath = "MO8/RE1/RE18" Then
                                            If DirectCast(DirectCast(objChildTree.ChildNodes(i), System.Web.UI.WebControls.TreeNode).ChildNodes, System.Web.UI.WebControls.TreeNodeCollection).Count > 0 Then
                                                DirectCast(DirectCast(DirectCast(objChildTree.ChildNodes(i), System.Web.UI.WebControls.TreeNode), System.Web.UI.WebControls.TreeNode).ChildNodes(0), System.Web.UI.WebControls.TreeNode).ImageUrl = img.ToString.Trim
                                            End If
                                        End If
                                    Next
                                End If
                            End If
                        Next
                    End If
                Next
            End If
        End If
        If parent_node.ToString.Trim.ToUpper = "IN21" Then
            For Each objChildTree As TreeNode In objTreenode.ChildNodes
                If objTreenode.Value = "MO2" Then
                    For Each objChildTree1 As TreeNode In objTreenode.ChildNodes
                        If objChildTree1.Value = "MO13" Then
                            For Each objChildTree2 As TreeNode In objChildTree1.ChildNodes
                                If objChildTree2.Value = "IN2" Then
                                    If parent_node.ToString.Trim.ToUpper = "IN21" Then
                                        If objChildTree2.ImageUrl = "images/lock.gif" Then
                                            Dim Cnt As Integer = objChildTree2.ChildNodes.Count
                                            For i = 0 To Cnt - 1
                                                objChildTree2.ChildNodes(i).ImageUrl = "images/lock.gif"
                                                'objChildTree2.ChildNodes(i).Value = "N"
                                            Next
                                        Else
                                            If objChildTree2.ChildNodes.Count > 0 Then
                                                Dim Cnt As Integer = objChildTree2.ChildNodes.Count
                                                For i = 0 To Cnt - 1
                                                    objChildTree2.ChildNodes(i).ImageUrl = img.ToString.Trim
                                                Next
                                            End If
                                        End If
                                    End If
                                End If
                            Next
                        End If
                    Next
                End If
            Next
        End If
    End Sub

    Private Sub saveTreeview(ByVal objTreeview As TreeView)
        If objTreeview.Nodes.Count > 0 Then
            For Each objChildTree As TreeNode In objTreeview.Nodes
                Call getAllNode(objChildTree)
            Next
        End If
    End Sub

    Private Sub getAllNode(ByVal objTreenode As TreeNode)
        'Iterate through all subnodes
        Dim access As String = ""
        If objTreenode.ImageUrl = "images/unlock.gif" Or objTreenode.ImageUrl = "images/report.gif" Then
            access = "Y"
        Else
            access = "N"
        End If
        Call updateNodeinformation(objTreenode.Value, access)

        If objTreenode.ChildNodes.Count > 0 Then
            For Each objChildTree As TreeNode In objTreenode.ChildNodes
                If objChildTree.ChildNodes.Count > 0 Then
                    Call getAllNode(objChildTree)
                Else
                    Dim access1 As String = ""
                    If objChildTree.ImageUrl = "images/unlock.gif" Or objChildTree.ImageUrl = "images/report.gif" Then
                        access1 = "Y"
                    Else
                        access1 = "N"
                    End If
                    Call updateNodeinformation(objChildTree.Value, access1)
                End If
            Next
        End If
    End Sub

    Public Sub updateNodeinformation(ByVal modulename As String, ByVal access As String)
        If modulename = "RS16" Then
            objCommand.Parameters.Clear()
            objCommand.Parameters.AddWithValue("@employee_id", ViewState("employee_id").ToString())
            objCommand.Parameters.AddWithValue("@access", access)
            objCommand.Parameters.AddWithValue("@module", modulename)
            objCommand.ExecuteNonQuery()
            If access = "N" Then
                objCommand.Parameters.Clear()
                objCommand.Parameters.AddWithValue("@employee_id", ViewState("employee_id").ToString())
                objCommand.Parameters.AddWithValue("@access", "N")
                objCommand.Parameters.AddWithValue("@module", "RS18")
                objCommand.ExecuteNonQuery()
            End If
        Else
            objCommand.Parameters.Clear()
            objCommand.Parameters.AddWithValue("@employee_id", ViewState("employee_id").ToString())
            objCommand.Parameters.AddWithValue("@access", access)
            objCommand.Parameters.AddWithValue("@module", modulename)
            objCommand.ExecuteNonQuery()
        End If

        'Dim objdal As New DatabaseHelper



    End Sub
    Private Sub ChangeNode(ByVal objTreenode As TreeNode, ByVal parent_node As String, ByVal img As String)
        If objTreenode.ChildNodes.Count > 0 Then
            For Each objChildTree As TreeNode In objTreenode.ChildNodes
                If objTreenode.Value.ToString.Trim.ToUpper = parent_node.ToString.Trim.ToUpper Then
                    objChildTree.ImageUrl = img.ToString.Trim
                    If objChildTree.ChildNodes.Count > 0 Then
                        Dim Cnt As Integer = objChildTree.ChildNodes.Count
                        For i = 0 To Cnt - 1
                            objChildTree.ChildNodes(i).ImageUrl = img.ToString.Trim
                            If DirectCast(objChildTree.ChildNodes(i), System.Web.UI.WebControls.TreeNode).ValuePath = "MO8/RE1/RE18" Then
                                If DirectCast(DirectCast(objChildTree.ChildNodes(i), System.Web.UI.WebControls.TreeNode).ChildNodes, System.Web.UI.WebControls.TreeNodeCollection).Count > 0 Then
                                    DirectCast(DirectCast(DirectCast(objChildTree.ChildNodes(i), System.Web.UI.WebControls.TreeNode), System.Web.UI.WebControls.TreeNode).ChildNodes(0), System.Web.UI.WebControls.TreeNode).ImageUrl = img.ToString.Trim
                                End If


                            End If
                        Next
                    End If
                End If
            Next
        End If
    End Sub
    Private Sub ChangeNodeReports(ByVal objTreenode As TreeNode, ByVal parent_node As String, ByVal img As String)
        If objTreenode.ChildNodes.Count > 0 Then
            If objTreenode.Value = "MO8" Then
                For Each objChildTree As TreeNode In objTreenode.ChildNodes

                    If parent_node.ToString.Trim.ToUpper = "RE1" Then
                        If objChildTree.ChildNodes.Count > 0 Then
                            Dim Cnt As Integer = objChildTree.ChildNodes.Count
                            For i = 0 To Cnt - 1
                                objChildTree.ChildNodes(i).ImageUrl = img.ToString.Trim
                                If DirectCast(objChildTree.ChildNodes(i), System.Web.UI.WebControls.TreeNode).ValuePath = "MO8/RE1/RE18" Then
                                    If DirectCast(DirectCast(objChildTree.ChildNodes(i), System.Web.UI.WebControls.TreeNode).ChildNodes, System.Web.UI.WebControls.TreeNodeCollection).Count > 0 Then
                                        DirectCast(DirectCast(DirectCast(objChildTree.ChildNodes(i), System.Web.UI.WebControls.TreeNode), System.Web.UI.WebControls.TreeNode).ChildNodes(0), System.Web.UI.WebControls.TreeNode).ImageUrl = img.ToString.Trim
                                    End If
                                End If
                            Next
                        End If
                    ElseIf parent_node.ToString.Trim.ToUpper = "RE18" Then
                        If objChildTree.ChildNodes.Count > 0 Then
                            Dim Cnt As Integer = objChildTree.ChildNodes.Count
                            For i = 0 To Cnt - 1
                                If DirectCast(objChildTree.ChildNodes(i), System.Web.UI.WebControls.TreeNode).ValuePath = "MO8/RE1/RE18" Then
                                    If DirectCast(DirectCast(objChildTree.ChildNodes(i), System.Web.UI.WebControls.TreeNode).ChildNodes, System.Web.UI.WebControls.TreeNodeCollection).Count > 0 Then
                                        DirectCast(DirectCast(DirectCast(objChildTree.ChildNodes(i), System.Web.UI.WebControls.TreeNode), System.Web.UI.WebControls.TreeNode).ChildNodes(0), System.Web.UI.WebControls.TreeNode).ImageUrl = img.ToString.Trim
                                    End If
                                End If
                            Next
                        End If



                    End If
                Next
            End If
            If objTreenode.Value = "MO2" Then
                For Each objChildTree As TreeNode In objTreenode.ChildNodes

                    If parent_node.ToString.Trim.ToUpper = "MO13" Then
                        If objChildTree.ChildNodes.Count > 0 Then
                            Dim Cnt As Integer = objChildTree.ChildNodes.Count
                            For i = 0 To Cnt - 1
                                objChildTree.ChildNodes(i).ImageUrl = img.ToString.Trim
                            Next
                        End If
                    ElseIf parent_node.ToString.Trim.ToUpper = "MO14" Then
                        If objChildTree.ChildNodes.Count > 0 Then
                            Dim Cnt As Integer = objChildTree.ChildNodes.Count
                            For i = 0 To Cnt - 1
                                objChildTree.ChildNodes(i).ImageUrl = img.ToString.Trim
                            Next
                        End If
                    ElseIf parent_node.ToString.Trim.ToUpper = "MO15" Then
                        If objChildTree.ChildNodes.Count > 0 Then
                            Dim Cnt As Integer = objChildTree.ChildNodes.Count
                            For i = 0 To Cnt - 1
                                objChildTree.ChildNodes(i).ImageUrl = img.ToString.Trim
                            Next
                        End If
                    ElseIf parent_node.ToString.Trim.ToUpper = "MO16" Then
                        If objChildTree.ChildNodes.Count > 0 Then
                            Dim Cnt As Integer = objChildTree.ChildNodes.Count
                            For i = 0 To Cnt - 1
                                objChildTree.ChildNodes(i).ImageUrl = img.ToString.Trim
                            Next
                        End If

                    End If
                Next
            End If
        End If
    End Sub

    Public Sub bindTreeview(ByVal objDatatable As DataTable)

        'get module information from the table modulemaster
        Dim objModule As DataTable = getModuleinfo()
        Dim objTreenode1 As New TreeNode 'Ringsales
        Dim objTreenode2 As New TreeNode 'inventory
        Dim objTreenode21 As New TreeNode 'Add/Edit/More
        Dim objTreenode211 As New TreeNode 'Editing Items
        Dim objTreenode22 As New TreeNode 'Receive Inventory
        Dim objTreenode23 As New TreeNode 'Ordering Alerts
        Dim objTreenode24 As New TreeNode 'Physical Inventory
        Dim objTreenode3 As New TreeNode 'Customers
        Dim objTreenode4 As New TreeNode 'Vendors
        Dim objTreenode5 As New TreeNode 'Employees
        Dim objTreenode6 As New TreeNode 'Today 's Sales
        Dim objTreenode61 As New TreeNode 'Ring Sale Today 's Sales
        Dim objTreenode7 As New TreeNode 'Close the Day
        Dim objtreenode8 As New TreeNode 'Reports
        Dim objTreenode81 As New TreeNode
        Dim subnode1 As New TreeNode
        Dim objtreenode10 As New TreeNode 'Marketing Center
        Dim objtreenode9 As New TreeNode 'Administration Options
        Dim objTreenode33 As New TreeNode 'data maintainnase
        Dim objTreenode12 As New TreeNode 'SetUp Wizard
        Dim objTreenode11 As New TreeNode 'Blank Line
        Dim i As Integer = 0
        TreeView1.Attributes.Add("onclick", "return OnTreeClick(this.event)")
        If objModule IsNot Nothing Then
            Dim dt As New DataTable
            Dim modle As New DataColumn
            Dim access As New DataColumn
            dt.Columns.Add(modle)
            dt.Columns.Add(access)


            For Each objRow In objModule.Rows
                i = i + 1
                Select Case (objRow("module").ToString.Trim())
                    Case "MO1"
                        objTreenode1.Text = objRow("description").ToString()
                        objTreenode1.ImageUrl = "images/report.gif"
                        'objTreenode1.SelectAction = TreeNodeSelectAction.Expand
                        objTreenode1.Value = "MO1"
                        TreeView1.Nodes.Add(objTreenode1)
                        Exit Select
                    Case "MO2"
                        objTreenode2.Text = objRow("description").ToString()
                        objTreenode2.Value = "MO2"
                        objTreenode2.ImageUrl = "images/report.gif"
                        objTreenode2.Expanded = True
                        'objTreenode2.SelectAction = TreeNodeSelectAction.Expand
                        TreeView1.Nodes.Add(objTreenode2)
                        Exit Select
                    Case "MO3"
                        objTreenode3.Text = objRow("description").ToString()
                        objTreenode3.Value = "MO3"
                        objTreenode3.ImageUrl = "images/report.gif"
                        objTreenode3.Expanded = True
                        'objTreenode3.SelectAction = TreeNodeSelectAction.Expand
                        TreeView1.Nodes.Add(objTreenode3)
                        Exit Select
                    Case "MO4"
                        objTreenode4.Text = objRow("description").ToString()
                        objTreenode4.Value = "MO4"
                        objTreenode4.ImageUrl = "images/report.gif"
                        'objTreenode4.SelectAction = TreeNodeSelectAction.Expand
                        TreeView1.Nodes.Add(objTreenode4)
                        Exit Select
                    Case "MO5"
                        objTreenode5.Text = objRow("description").ToString()
                        objTreenode5.Value = "MO5"
                        objTreenode5.ImageUrl = "images/report.gif"
                        'objTreenode5.SelectAction = TreeNodeSelectAction.Expand
                        TreeView1.Nodes.Add(objTreenode5)
                        Exit Select
                    Case "MO6"
                        Dim objdatarow() As DataRow = objDatatable.Select("module = 'MO6'")
                        objTreenode6.Text = objRow("description").ToString()
                        objTreenode6.Value = "MO6"
                        If objdatarow(0)("access").ToString() = "N" Then
                            objTreenode6.ImageUrl = "images/lock.gif"
                        Else
                            objTreenode6.ImageUrl = "images/unlock.gif"
                        End If
                        TreeView1.Nodes.Add(objTreenode6)
                        Exit Select
                    Case "MO7"
                        Dim objdatarow() As DataRow = objDatatable.Select("module = 'MO7'")
                        objTreenode7.Text = objRow("description").ToString()
                        objTreenode7.Value = "MO7"
                        If objdatarow(0)("access").ToString() = "N" Then
                            objTreenode7.ImageUrl = "images/lock.gif"
                        Else
                            objTreenode7.ImageUrl = "images/unlock.gif"
                        End If
                        TreeView1.Nodes.Add(objTreenode7)
                        Exit Select
                    Case "MO8"
                        objtreenode8.Text = objRow("description").ToString()
                        objtreenode8.Value = "MO8"
                        objtreenode8.ImageUrl = "images/report.gif"
                        TreeView1.Nodes.Add(objtreenode8)
                        Exit Select
                    Case "MO9"
                        objtreenode10.Text = objRow("description").ToString()
                        objtreenode10.Value = "MO9"
                        objtreenode10.ImageUrl = "images/report.gif"
                        TreeView1.Nodes.Add(objtreenode10)
                        Exit Select
                    Case "MO10"
                        objtreenode9.Text = objRow("description").ToString()
                        objtreenode9.Value = "MO10"
                        objtreenode9.ImageUrl = "images/report.gif"
                        TreeView1.Nodes.Add(objtreenode9)
                        Exit Select

                    Case "MO11"
                        objTreenode33.Text = objRow("description").ToString()
                        objTreenode33.Value = "MO11"
                        objTreenode33.ImageUrl = "images/report.gif"
                        TreeView1.Nodes.Add(objTreenode33)
                        Exit Select

                    Case "AD8"
                        Dim objdatarow() As DataRow = objDatatable.Select("module = 'AD8'")
                        objTreenode12.Text = objRow("description").ToString()
                        objTreenode12.Value = "AD8"
                        If objdatarow(0)("access").ToString() = "N" Then
                            objTreenode12.ImageUrl = "images/lock.gif"
                        Else
                            objTreenode12.ImageUrl = "images/unlock.gif"
                        End If
                        TreeView1.Nodes.Add(objTreenode12)
                        Exit Select

                        '---------------------------------------Ringsales-----------------------------------'

                    Case "RS1"
                        Dim objSubnode As New TreeNode
                        Dim objdatarow() As DataRow = objDatatable.Select("module = 'RS1'")
                        objSubnode.Text = objRow("description").ToString()
                        objSubnode.Value = "RS1"
                        If objdatarow(0)("access").ToString() = "N" Then
                            objSubnode.ImageUrl = "images/lock.gif"
                        Else
                            objSubnode.ImageUrl = "images/unlock.gif"
                        End If
                        objTreenode1.ChildNodes.Add(objSubnode)
                        Exit Select
                    Case "RS2"
                        Dim objSubnode As New TreeNode
                        Dim objdatarow() As DataRow = objDatatable.Select("module = 'RS2'")
                        objSubnode.Text = objRow("description").ToString()
                        objSubnode.Value = "RS2"
                        If objdatarow(0)("access").ToString() = "N" Then
                            objSubnode.ImageUrl = "images/lock.gif"
                        Else
                            objSubnode.ImageUrl = "images/unlock.gif"
                        End If
                        objTreenode1.ChildNodes.Add(objSubnode)
                        Exit Select
                    Case "RS3"
                        Dim objdatarow() As DataRow = objDatatable.Select("module = 'RS3'")
                        objTreenode11.Text = objRow("description").ToString()
                        objTreenode11.Value = "RS3"
                        If objdatarow(0)("access").ToString() = "N" Then
                            objTreenode11.ImageUrl = "images/lock.gif"
                        Else
                            objTreenode11.ImageUrl = "images/unlock.gif"
                        End If
                        objTreenode1.ChildNodes.Add(objTreenode11)
                        Exit Select

                        'Dim objSubnode As New TreeNode
                        'Dim objdatarow() As DataRow = objDatatable.Select("module = 'RS3'")
                        'objSubnode.Text = objRow("description").ToString()
                        'objSubnode.Value = "RS3"
                        'If objdatarow(0)("access").ToString() = "N" Then
                        '    objSubnode.ImageUrl = "images/lock.gif"
                        'Else
                        '    objSubnode.ImageUrl = "images/unlock.gif"
                        'End If
                        'objTreenode1.ChildNodes.Add(objSubnode)
                        'Exit Select
                    Case "RS4"
                        Dim objSubnode As New TreeNode
                        Dim objdatarow() As DataRow = objDatatable.Select("module = 'RS4'")
                        objSubnode.Text = objRow("description").ToString()
                        objSubnode.Value = "RS4"
                        If objdatarow(0)("access").ToString() = "N" Then
                            objSubnode.ImageUrl = "images/lock.gif"
                        Else
                            objSubnode.ImageUrl = "images/unlock.gif"
                        End If
                        objTreenode1.ChildNodes.Add(objSubnode)
                        Exit Select
                    Case "RS5"
                        Dim objSubnode As New TreeNode
                        Dim objdatarow() As DataRow = objDatatable.Select("module = 'RS5'")
                        objSubnode.Text = objRow("description").ToString()
                        objSubnode.Value = "RS5"
                        If objdatarow(0)("access").ToString() = "N" Then
                            objSubnode.ImageUrl = "images/lock.gif"
                        Else
                            objSubnode.ImageUrl = "images/unlock.gif"
                        End If
                        objTreenode1.ChildNodes.Add(objSubnode)
                        Exit Select
                    Case "RS6"
                        Dim objSubnode As New TreeNode
                        Dim objdatarow() As DataRow = objDatatable.Select("module = 'RS6'")
                        objSubnode.Text = objRow("description").ToString()
                        objSubnode.Value = "RS6"
                        If objdatarow(0)("access").ToString() = "N" Then
                            objSubnode.ImageUrl = "images/lock.gif"
                        Else
                            objSubnode.ImageUrl = "images/unlock.gif"
                        End If
                        objTreenode1.ChildNodes.Add(objSubnode)
                        Exit Select
                    Case "RS7"
                        Dim objSubnode As New TreeNode
                        Dim objdatarow() As DataRow = objDatatable.Select("module = 'RS7'")
                        objSubnode.Text = objRow("description").ToString()
                        objSubnode.Value = "RS7"
                        If objdatarow(0)("access").ToString() = "N" Then
                            objSubnode.ImageUrl = "images/lock.gif"
                        Else
                            objSubnode.ImageUrl = "images/unlock.gif"
                        End If
                        objTreenode1.ChildNodes.Add(objSubnode)

                        If Session("frequentbuyer").ToString.ToUpper = "Y" Then
                            Dim objSubnode1 As New TreeNode
                            Dim objdatarow1() As DataRow = objDatatable.Select("module = 'RS21'")
                            If objdatarow1.Count > 0 Then
                                Dim objRow1() As DataRow = objModule.Select("module = 'RS21'")
                                objSubnode1.Text = objRow1(0)("description").ToString()
                                objSubnode1.Value = "RS21"
                                If objdatarow1(0)("access").ToString() = "N" Then
                                    objSubnode1.ImageUrl = "images/lock.gif"
                                Else
                                    objSubnode1.ImageUrl = "images/unlock.gif"
                                End If
                                objTreenode1.ChildNodes.Add(objSubnode1)
                            End If
                        End If
                        Exit Select
                    Case "RS8"
                        Dim objSubnode As New TreeNode
                        Dim objdatarow() As DataRow = objDatatable.Select("module = 'RS8'")
                        objSubnode.Text = objRow("description").ToString()
                        objSubnode.Value = "RS8"
                        If objdatarow(0)("access").ToString() = "N" Then
                            objSubnode.ImageUrl = "images/lock.gif"
                        Else
                            objSubnode.ImageUrl = "images/unlock.gif"
                        End If
                        objTreenode1.ChildNodes.Add(objSubnode)
                        Exit Select
                    Case "RS9"
                        Dim objSubnode As New TreeNode
                        Dim objdatarow() As DataRow = objDatatable.Select("module = 'RS9'")
                        objSubnode.Text = objRow("description").ToString()
                        objSubnode.Value = "RS9"
                        If objdatarow(0)("access").ToString() = "N" Then
                            objSubnode.ImageUrl = "images/lock.gif"
                        Else
                            objSubnode.ImageUrl = "images/unlock.gif"
                        End If
                        objTreenode1.ChildNodes.Add(objSubnode)
                        Exit Select
                    Case "RS10"
                        Dim objSubnode As New TreeNode
                        Dim objdatarow() As DataRow = objDatatable.Select("module = 'RS10'")
                        objSubnode.Text = objRow("description").ToString()
                        objSubnode.Value = "RS10"
                        If objdatarow(0)("access").ToString() = "N" Then
                            objSubnode.ImageUrl = "images/lock.gif"
                        Else
                            objSubnode.ImageUrl = "images/unlock.gif"
                        End If
                        objTreenode1.ChildNodes.Add(objSubnode)
                        Exit Select
                    Case "RS11"
                        Dim objSubnode As New TreeNode
                        Dim objdatarow() As DataRow = objDatatable.Select("module = 'RS11'")
                        objSubnode.Text = objRow("description").ToString()
                        objSubnode.Value = "RS11"
                        If objdatarow(0)("access").ToString() = "N" Then
                            objSubnode.ImageUrl = "images/lock.gif"
                        Else
                            objSubnode.ImageUrl = "images/unlock.gif"
                        End If
                        objTreenode1.ChildNodes.Add(objSubnode)
                        Exit Select
                    Case "RS12"
                        Dim objSubnode As New TreeNode
                        Dim objdatarow() As DataRow = objDatatable.Select("module = 'RS12'")
                        objSubnode.Text = objRow("description").ToString()
                        objSubnode.Value = "RS12"
                        If objdatarow(0)("access").ToString() = "N" Then
                            objSubnode.ImageUrl = "images/lock.gif"
                        Else
                            objSubnode.ImageUrl = "images/unlock.gif"
                        End If
                        objTreenode1.ChildNodes.Add(objSubnode)
                        Exit Select
                    Case "RS13"
                        Dim objSubnode As New TreeNode
                        Dim objdatarow() As DataRow = objDatatable.Select("module = 'RS13'")
                        objSubnode.Text = objRow("description").ToString()
                        objSubnode.Value = "RS13"
                        If objdatarow(0)("access").ToString() = "N" Then
                            objSubnode.ImageUrl = "images/lock.gif"
                        Else
                            objSubnode.ImageUrl = "images/unlock.gif"
                        End If
                        objTreenode1.ChildNodes.Add(objSubnode)
                        If ViewState("clubmembership") = True Then
                            Dim objSubnode1 As New TreeNode
                            Dim objdatarow1() As DataRow = objDatatable.Select("module = 'RS22'")
                            If objdatarow1.Count > 0 Then
                                Dim objRow1() As DataRow = objModule.Select("module = 'RS22'")
                                objSubnode1.Text = objRow1(0)("description").ToString()
                                objSubnode1.Value = "RS22"
                                If objdatarow1(0)("access").ToString() = "N" Then
                                    objSubnode1.ImageUrl = "images/lock.gif"
                                Else
                                    objSubnode1.ImageUrl = "images/unlock.gif"
                                End If
                                objTreenode1.ChildNodes.Add(objSubnode1)
                            End If
                        End If
                        Exit Select
                    Case "RS14"
                        Dim objSubnode As New TreeNode
                        Dim objdatarow() As DataRow = objDatatable.Select("module = 'RS14'")
                        objSubnode.Text = objRow("description").ToString()
                        objSubnode.Value = "RS14"
                        If objdatarow(0)("access").ToString() = "N" Then
                            objSubnode.ImageUrl = "images/lock.gif"
                        Else
                            objSubnode.ImageUrl = "images/unlock.gif"
                        End If
                        objTreenode1.ChildNodes.Add(objSubnode)
                        Exit Select
                    Case "RS15"
                        Dim objSubnode As New TreeNode
                        Dim objdatarow() As DataRow = objDatatable.Select("module = 'RS15'")
                        objSubnode.Text = objRow("description").ToString()
                        objSubnode.Value = "RS15"
                        If objdatarow(0)("access").ToString() = "N" Then
                            objSubnode.ImageUrl = "images/lock.gif"
                        Else
                            objSubnode.ImageUrl = "images/unlock.gif"
                        End If
                        objTreenode1.ChildNodes.Add(objSubnode)
                        Exit Select
                        'Case "RS16"
                        '    'Dim objSubnode As New TreeNode
                        '    Dim objdatarow() As DataRow = objDatatable.Select("module = 'RS16'")
                        '    objTreenode61.Text = objRow("description").ToString()
                        '    objTreenode61.Value = "RS16"
                        '    If objdatarow(0)("access").ToString() = "N" Then
                        '        objTreenode61.ImageUrl = "images/lock.gif"
                        '    Else
                        '        objTreenode61.ImageUrl = "images/unlock.gif"
                        '    End If
                        '    objTreenode1.ChildNodes.Add(objTreenode61)
                        '    Exit Select
                    Case "RS17"
                        Dim objSubnode As New TreeNode
                        Dim objdatarow() As DataRow = objDatatable.Select("module = 'RS17'")
                        objSubnode.Text = objRow("description").ToString()
                        objSubnode.Value = "RS17"
                        If objdatarow(0)("access").ToString() = "N" Then
                            objSubnode.ImageUrl = "images/lock.gif"
                        Else
                            objSubnode.ImageUrl = "images/unlock.gif"
                        End If
                        objTreenode1.ChildNodes.Add(objSubnode)
                        Exit Select
                    Case "RS23" ' Cash Drop
                        Dim objSubnode As New TreeNode
                        Dim objdatarow() As DataRow = objDatatable.Select("module = 'RS23'")
                        objSubnode.Text = objRow("description").ToString()
                        objSubnode.Value = "RS23"
                        If objdatarow(0)("access").ToString() = "N" Then
                            objSubnode.ImageUrl = "images/lock.gif"
                        Else
                            objSubnode.ImageUrl = "images/unlock.gif"
                        End If
                        objTreenode1.ChildNodes.Add(objSubnode)
                        Exit Select
                    Case "RS18"
                        Dim objSubnodedependent As New TreeNode
                        objSubnodedependent = objTreenode1.ChildNodes.Item(15)
                        If objSubnodedependent.ImageUrl = "images/lock.gif" Then

                        Else
                            Dim objSubnode As New TreeNode
                            Dim objdatarow() As DataRow = objDatatable.Select("module = 'RS18'")
                            objSubnode.Text = objRow("description").ToString()
                            objSubnode.Value = "RS18"
                            If objdatarow(0)("access").ToString() = "N" Then
                                objSubnode.ImageUrl = "images/lock.gif"
                            Else
                                objSubnode.ImageUrl = "images/unlock.gif"
                            End If
                            objSubnodedependent.ChildNodes.Add(objSubnode)
                        End If
                        Exit Select
                    Case "RS19"
                        Dim objSubnode As New TreeNode
                        Dim objSubnodedependent As New TreeNode
                        objSubnodedependent = objTreenode61
                        Dim objdatarow() As DataRow = objDatatable.Select("module = 'RS19'")
                        objSubnode.Text = objRow("description").ToString()
                        objSubnode.Value = "RS19"
                        If objdatarow.Length > 0 Then
                            If objdatarow(0)("access").ToString() = "N" Then
                                objSubnode.ImageUrl = "images/lock.gif"
                            Else
                                objSubnode.ImageUrl = "images/unlock.gif"
                            End If
                        Else

                            Dim objEmployee As New vbEmployee()
                            objEmployee.EmployeeId = ViewState("employee_id").ToString()
                            If ViewState("admin") = "Y" Then
                                Dim strReturn As Integer = objEmployee.fnInsertEmployeeSecurity("RS19", "Y")
                                objSubnode.ImageUrl = "images/unlock.gif"
                                objDatatable = objEmployee.fnGetSecurity()
                            Else
                                Dim strReturn As Integer = objEmployee.fnInsertEmployeeSecurity("RS19", "Y")
                                objSubnode.ImageUrl = "images/lock.gif"
                                objDatatable = objEmployee.fnGetSecurity()
                            End If
                        End If
                        objSubnodedependent.ChildNodes.Add(objSubnode)
                        Exit Select
                    Case "RS20"
                        Dim objSubnode As New TreeNode
                        Dim objSubnode1 As New TreeNode
                        Dim objdatarow() As DataRow = objDatatable.Select("module = 'RS20'")
                        objSubnode.Text = objRow("description").ToString()
                        objSubnode.Value = "RS20"
                        If objdatarow(0)("access").ToString() = "N" Then
                            objSubnode.ImageUrl = "images/lock.gif"
                        Else
                            objSubnode.ImageUrl = "images/unlock.gif"
                        End If
                        objSubnode1.Text = "Allows a user to further discount an item that is already</br> discounted such as a <i>promotional item, a non-discountable item,</i></br> or an item that is in a <i>Discount Group</i>.  Item will never be allowed </br>to be sold below cost.  NOT RECOMMENEDED"
                        objSubnode.ChildNodes.Add(objSubnode1)
                        objSubnode.Expanded = True
                        objSubnode1.SelectAction = TreeNodeSelectAction.None
                        objTreenode11.ChildNodes.Add(objSubnode)
                        Exit Select
                    Case "TS1"
                        Dim objSubnode As New TreeNode
                        Dim objdatarow() As DataRow = objDatatable.Select("module = 'TS1'")
                        objSubnode.Text = objRow("description").ToString()
                        objSubnode.Value = "TS1"
                        If objdatarow.Length > 0 Then
                            If objdatarow(0)("access").ToString() = "N" Then
                                objSubnode.ImageUrl = "images/lock.gif"
                            Else
                                objSubnode.ImageUrl = "images/unlock.gif"
                            End If
                        Else

                            Dim objEmployee As New vbEmployee()
                            objEmployee.EmployeeId = ViewState("employee_id").ToString()
                            If ViewState("admin") = "Y" Then
                                Dim strReturn As Integer = objEmployee.fnInsertEmployeeSecurity("TS1", "Y")
                                objSubnode.ImageUrl = "images/unlock.gif"
                                objDatatable = objEmployee.fnGetSecurity()
                            Else
                                Dim strReturn As Integer = objEmployee.fnInsertEmployeeSecurity("TS1", "Y")
                                objSubnode.ImageUrl = "images/unlock.gif"
                                objDatatable = objEmployee.fnGetSecurity()
                            End If
                        End If
                        objTreenode6.ChildNodes.Add(objSubnode)
                        Exit Select


                        '--------------------------------------- Employee -----------------------------------'

                    Case "EM1"
                        Dim objSubnode As New TreeNode
                        Dim objdatarow() As DataRow = objDatatable.Select("module = 'EM1'")
                        objSubnode.Text = objRow("description").ToString()
                        objSubnode.Value = "EM1"
                        If objdatarow(0)("access").ToString() = "N" Then
                            objSubnode.ImageUrl = "images/lock.gif"
                        Else
                            objSubnode.ImageUrl = "images/unlock.gif"
                        End If
                        objTreenode5.ChildNodes.Add(objSubnode)
                        Exit Select
                    Case "EM2"
                        Dim objSubnode As New TreeNode
                        Dim objdatarow() As DataRow = objDatatable.Select("module = 'EM2'")
                        objSubnode.Text = objRow("description").ToString()
                        objSubnode.Value = "EM2"
                        If objdatarow(0)("access").ToString() = "N" Then
                            objSubnode.ImageUrl = "images/lock.gif"
                        Else
                            objSubnode.ImageUrl = "images/unlock.gif"
                        End If
                        objTreenode5.ChildNodes.Add(objSubnode)
                        Exit Select
                    Case "EM3"
                        Dim objSubnode As New TreeNode
                        Dim objdatarow() As DataRow = objDatatable.Select("module = 'EM3'")
                        objSubnode.Text = objRow("description").ToString()
                        objSubnode.Value = "EM3"
                        If objdatarow(0)("access").ToString() = "N" Then
                            objSubnode.ImageUrl = "images/lock.gif"
                        Else
                            objSubnode.ImageUrl = "images/unlock.gif"
                        End If
                        objTreenode5.ChildNodes.Add(objSubnode)
                        Exit Select
                    Case "EM4"
                        Dim objSubnode As New TreeNode
                        Dim objdatarow() As DataRow = objDatatable.Select("module = 'EM4'")
                        objSubnode.Text = objRow("description").ToString()
                        objSubnode.Value = "EM4"
                        If objdatarow(0)("access").ToString() = "N" Then
                            objSubnode.ImageUrl = "images/lock.gif"
                        Else
                            objSubnode.ImageUrl = "images/unlock.gif"
                        End If
                        objTreenode5.ChildNodes.Add(objSubnode)
                        Exit Select
                    Case "EM5"
                        Dim objSubnode As New TreeNode
                        Dim objdatarow() As DataRow = objDatatable.Select("module = 'EM5'")
                        objSubnode.Text = objRow("description").ToString()
                        objSubnode.Value = "EM5"
                        If objdatarow(0)("access").ToString() = "N" Then
                            objSubnode.ImageUrl = "images/lock.gif"
                        Else
                            objSubnode.ImageUrl = "images/unlock.gif"
                        End If
                        objTreenode5.ChildNodes.Add(objSubnode)
                        Exit Select
                    Case "EM6"
                        Dim objSubnode As New TreeNode
                        Dim objdatarow() As DataRow = objDatatable.Select("module = 'EM6'")
                        objSubnode.Text = objRow("description").ToString()
                        objSubnode.Value = "EM6"
                        If objdatarow(0)("access").ToString() = "N" Then
                            objSubnode.ImageUrl = "images/lock.gif"
                        Else
                            objSubnode.ImageUrl = "images/unlock.gif"
                        End If
                        objTreenode5.ChildNodes.Add(objSubnode)
                    Case "EM7"
                        Dim objSubnode As New TreeNode
                        Dim objdatarow() As DataRow = objDatatable.Select("module = 'EM7'")
                        objSubnode.Text = objRow("description").ToString()
                        objSubnode.Value = "EM7"
                        If objdatarow(0)("access").ToString() = "N" Then
                            objSubnode.ImageUrl = "images/lock.gif"
                        Else
                            objSubnode.ImageUrl = "images/unlock.gif"
                        End If
                        objTreenode5.ChildNodes.Add(objSubnode)
                    Case "EM8"
                        Dim objSubnode As New TreeNode
                        Dim objdatarow() As DataRow = objDatatable.Select("module = 'EM8'")
                        objSubnode.Text = objRow("description").ToString()
                        objSubnode.Value = "EM8"
                        If objdatarow(0)("access").ToString() = "N" Then
                            objSubnode.ImageUrl = "images/lock.gif"
                        Else
                            objSubnode.ImageUrl = "images/unlock.gif"
                        End If
                        objTreenode5.ChildNodes.Add(objSubnode)
                        Exit Select

                        '---------------------------------------Inventory-----------------------------------'
                    Case "MO13"
                        objTreenode21.Text = objRow("description").ToString()
                        objTreenode21.ImageUrl = "images/report.gif"
                        objTreenode21.Value = "MO13"
                        objTreenode2.ChildNodes.Add(objTreenode21)
                        Exit Select

                    Case "MO14"
                        Dim objdatarow() As DataRow = objDatatable.Select("module = 'MO14'")
                        objTreenode22.Text = objRow("description").ToString()
                        objTreenode22.Value = "MO14"
                        If objdatarow(0)("access").ToString() = "N" Then
                            objTreenode22.ImageUrl = "images/lock.gif"
                        Else
                            objTreenode22.ImageUrl = "images/unlock.gif"
                        End If
                        objTreenode2.ChildNodes.Add(objTreenode22)
                        Exit Select

                    Case "MO15"
                        Dim objdatarow() As DataRow = objDatatable.Select("module = 'MO15'")
                        objTreenode23.Text = objRow("description").ToString()
                        objTreenode23.Value = "MO15"
                        If objdatarow(0)("access").ToString() = "N" Then
                            objTreenode23.ImageUrl = "images/lock.gif"
                        Else
                            objTreenode23.ImageUrl = "images/unlock.gif"
                        End If
                        objTreenode2.ChildNodes.Add(objTreenode23)
                        Exit Select

                    Case "MO16"
                        Dim objdatarow() As DataRow = objDatatable.Select("module = 'MO16'")
                        objTreenode24.Text = objRow("description").ToString()
                        objTreenode24.Value = "MO16"
                        If objdatarow(0)("access").ToString() = "N" Then
                            objTreenode24.ImageUrl = "images/lock.gif"
                        Else
                            objTreenode24.ImageUrl = "images/unlock.gif"
                        End If
                        objTreenode2.ChildNodes.Add(objTreenode24)
                        Exit Select


                    Case "IN1"
                        Dim objSubnode As New TreeNode
                        Dim objdatarow() As DataRow = objDatatable.Select("module = 'IN1'")
                        objSubnode.Text = objRow("description").ToString()
                        objSubnode.Value = "IN1"
                        If objdatarow(0)("access").ToString() = "N" Then
                            objSubnode.ImageUrl = "images/lock.gif"
                        Else
                            objSubnode.ImageUrl = "images/unlock.gif"
                        End If
                        objTreenode21.ChildNodes.Add(objSubnode)
                        Exit Select
                    Case "IN2"
                        'Dim objSubnode As New TreeNode
                        Dim objdatarow() As DataRow = objDatatable.Select("module = 'IN2'")
                        'objSubnode.Text = objRow("description").ToString()
                        'objSubnode.Value = "IN2"
                        'If objdatarow(0)("access").ToString() = "N" Then
                        '    objSubnode.ImageUrl = "images/lock.gif"
                        'Else
                        '    objSubnode.ImageUrl = "images/unlock.gif"
                        'End If
                        objTreenode211.Text = objRow("description").ToString()
                        If objdatarow(0)("access").ToString() = "N" Then
                            objTreenode211.ImageUrl = "images/lock.gif"
                        Else
                            objTreenode211.ImageUrl = "images/unlock.gif"
                        End If
                        objTreenode211.Value = "IN2"
                        objTreenode21.ChildNodes.Add(objTreenode211)
                        Exit Select
                    Case "IN21"
                        Dim objSubnode As New TreeNode
                        Dim objdatarow() As DataRow = objDatatable.Select("module = 'IN21'")
                        objSubnode.Text = objRow("description").ToString()
                        objSubnode.Value = "IN21"
                        If objdatarow(0)("access").ToString() = "N" Then
                            objSubnode.ImageUrl = "images/lock.gif"
                        Else
                            objSubnode.ImageUrl = "images/unlock.gif"
                        End If
                        objTreenode211.ChildNodes.Add(objSubnode)
                        Exit Select
                    Case "IN3"
                        Dim objSubnode As New TreeNode
                        Dim objdatarow() As DataRow = objDatatable.Select("module = 'IN3'")
                        objSubnode.Text = objRow("description").ToString()
                        objSubnode.Value = "IN3"
                        If objdatarow(0)("access").ToString() = "N" Then
                            objSubnode.ImageUrl = "images/lock.gif"
                        Else
                            objSubnode.ImageUrl = "images/unlock.gif"
                        End If
                        objTreenode21.ChildNodes.Add(objSubnode)
                        Exit Select
                    Case "IN4"
                        Dim objSubnode As New TreeNode
                        Dim objdatarow() As DataRow = objDatatable.Select("module = 'IN4'")
                        objSubnode.Text = objRow("description").ToString()
                        objSubnode.Value = "IN4"
                        If objdatarow(0)("access").ToString() = "N" Then
                            objSubnode.ImageUrl = "images/lock.gif"
                        Else
                            objSubnode.ImageUrl = "images/unlock.gif"
                        End If
                        objTreenode21.ChildNodes.Add(objSubnode)
                        Exit Select
                    Case "IN5"
                        Dim objSubnode As New TreeNode
                        Dim objdatarow() As DataRow = objDatatable.Select("module = 'IN5'")
                        objSubnode.Text = objRow("description").ToString()
                        objSubnode.Value = "IN5"
                        If objdatarow(0)("access").ToString() = "N" Then
                            objSubnode.ImageUrl = "images/lock.gif"
                        Else
                            objSubnode.ImageUrl = "images/unlock.gif"
                        End If
                        objTreenode21.ChildNodes.Add(objSubnode)
                        Exit Select
                    Case "IN6"
                        Dim objSubnode As New TreeNode
                        Dim objdatarow() As DataRow = objDatatable.Select("module = 'IN6'")
                        objSubnode.Text = objRow("description").ToString()
                        objSubnode.Value = "IN6"
                        If objdatarow(0)("access").ToString() = "N" Then
                            objSubnode.ImageUrl = "images/lock.gif"
                        Else
                            objSubnode.ImageUrl = "images/unlock.gif"
                        End If
                        objTreenode21.ChildNodes.Add(objSubnode)
                        Exit Select
                    Case "IN7"
                        Dim objSubnode As New TreeNode
                        Dim objdatarow() As DataRow = objDatatable.Select("module = 'IN7'")
                        objSubnode.Text = objRow("description").ToString()
                        objSubnode.Value = "IN7"
                        If objdatarow(0)("access").ToString() = "N" Then
                            objSubnode.ImageUrl = "images/lock.gif"
                        Else
                            objSubnode.ImageUrl = "images/unlock.gif"
                        End If
                        objTreenode21.ChildNodes.Add(objSubnode)
                        Exit Select
                    Case "IN8" 'Spoilage Entry
                        If ViewState("Spoilage") = "1" Then
                            Dim objSubnode As New TreeNode
                            Dim objdatarow() As DataRow = objDatatable.Select("module = 'IN8'")
                            objSubnode.Text = objRow("description").ToString()
                            objSubnode.Value = "IN8"
                            If objdatarow(0)("access").ToString() = "N" Then
                                objSubnode.ImageUrl = "images/lock.gif"
                            Else
                                objSubnode.ImageUrl = "images/unlock.gif"
                            End If
                            objTreenode21.ChildNodes.Add(objSubnode)
                            Exit Select
                        End If

                        '---------------------------------------Vendor---------------------------------'

                    Case "VN1"
                        Dim objSubnode As New TreeNode
                        Dim objdatarow() As DataRow = objDatatable.Select("module = 'VN1'")
                        objSubnode.Text = objRow("description").ToString()
                        objSubnode.Value = "VN1"
                        If objdatarow(0)("access").ToString() = "N" Then
                            objSubnode.ImageUrl = "images/lock.gif"
                        Else
                            objSubnode.ImageUrl = "images/unlock.gif"
                        End If
                        objTreenode4.ChildNodes.Add(objSubnode)
                        Exit Select
                    Case "VN2"
                        Dim objSubnode As New TreeNode
                        Dim objdatarow() As DataRow = objDatatable.Select("module = 'VN2'")
                        objSubnode.Text = objRow("description").ToString()
                        objSubnode.Value = "VN2"
                        If objdatarow(0)("access").ToString() = "N" Then
                            objSubnode.ImageUrl = "images/lock.gif"
                        Else
                            objSubnode.ImageUrl = "images/unlock.gif"
                        End If
                        objTreenode4.ChildNodes.Add(objSubnode)
                        Exit Select
                    Case "VN3"
                        Dim objSubnode As New TreeNode
                        Dim objdatarow() As DataRow = objDatatable.Select("module = 'VN3'")
                        objSubnode.Text = objRow("description").ToString()
                        objSubnode.Value = "VN3"
                        If objdatarow(0)("access").ToString() = "N" Then
                            objSubnode.ImageUrl = "images/lock.gif"
                        Else
                            objSubnode.ImageUrl = "images/unlock.gif"
                        End If
                        objTreenode4.ChildNodes.Add(objSubnode)
                        Exit Select
                    Case "VN4"
                        Dim objSubnode As New TreeNode
                        Dim objdatarow() As DataRow = objDatatable.Select("module = 'VN4'")
                        objSubnode.Text = objRow("description").ToString()
                        objSubnode.Value = "VN4"
                        If objdatarow(0)("access").ToString() = "N" Then
                            objSubnode.ImageUrl = "images/lock.gif"
                        Else
                            objSubnode.ImageUrl = "images/unlock.gif"
                        End If
                        objTreenode4.ChildNodes.Add(objSubnode)
                        Exit Select
                    Case "VN5"
                        Dim objSubnode As New TreeNode
                        Dim objdatarow() As DataRow = objDatatable.Select("module = 'VN5'")
                        objSubnode.Text = objRow("description").ToString()
                        objSubnode.Value = "VN5"
                        If objdatarow(0)("access").ToString() = "N" Then
                            objSubnode.ImageUrl = "images/lock.gif"
                        Else
                            objSubnode.ImageUrl = "images/unlock.gif"
                        End If
                        objTreenode4.ChildNodes.Add(objSubnode)
                        Exit Select
                        'Case "VN6"
                        '    Dim objSubnode As New TreeNode
                        '    Dim objdatarow() As DataRow = objDatatable.Select("module = 'VN6'")
                        '    objSubnode.Text = objRow("description").ToString()
                        '    objSubnode.Value = "VN6"
                        '    If objdatarow(0)("access").ToString() = "N" Then
                        '        objSubnode.ImageUrl = "images/lock.gif"
                        '    Else
                        '        objSubnode.ImageUrl = "images/unlock.gif"
                        '    End If
                        '    objTreenode4.ChildNodes.Add(objSubnode)
                        '    Exit Select


                        '---------------------------------------Customer---------------------------------'

                    Case "CU1"
                        Dim objSubnode As New TreeNode
                        Dim objdatarow() As DataRow = objDatatable.Select("module = 'CU1'")
                        objSubnode.Text = objRow("description").ToString()
                        objSubnode.Value = "CU1"
                        If objdatarow(0)("access").ToString() = "N" Then
                            objSubnode.ImageUrl = "images/lock.gif"
                        Else
                            objSubnode.ImageUrl = "images/unlock.gif"
                        End If
                        objTreenode3.ChildNodes.Add(objSubnode)
                        Exit Select

                    Case "CU2"
                        Dim objSubnode As New TreeNode
                        Dim objdatarow() As DataRow = objDatatable.Select("module = 'CU2'")
                        objSubnode.Text = objRow("description").ToString()
                        objSubnode.Value = "CU2"
                        If objdatarow(0)("access").ToString() = "N" Then
                            objSubnode.ImageUrl = "images/lock.gif"
                        Else
                            objSubnode.ImageUrl = "images/unlock.gif"
                        End If
                        objTreenode3.ChildNodes.Add(objSubnode)
                        Exit Select
                    Case "CU3"
                        Dim objSubnode As New TreeNode
                        Dim objdatarow() As DataRow = objDatatable.Select("module = 'CU3'")
                        objSubnode.Text = objRow("description").ToString()
                        objSubnode.Value = "CU3"
                        If objdatarow(0)("access").ToString() = "N" Then
                            objSubnode.ImageUrl = "images/lock.gif"
                        Else
                            objSubnode.ImageUrl = "images/unlock.gif"
                        End If
                        objTreenode3.ChildNodes.Add(objSubnode)

                        Dim objSubnode1 As New TreeNode
                        Dim objdatarow1() As DataRow = objDatatable.Select("module = 'CU10'")
                        If objdatarow1.Count > 0 Then
                            Dim objRow1() As DataRow = objModule.Select("module = 'CU10'")
                            objSubnode1.Text = objRow1(0)("description").ToString()
                            objSubnode1.Value = "CU10"
                            If objdatarow1(0)("access").ToString() = "N" Then
                                objSubnode1.ImageUrl = "images/lock.gif"
                            Else
                                objSubnode1.ImageUrl = "images/unlock.gif"
                            End If
                            objTreenode3.ChildNodes.Add(objSubnode1)
                        End If

                        Exit Select
                    Case "CU4"
                        Dim objSubnode As New TreeNode
                        Dim objdatarow() As DataRow = objDatatable.Select("module = 'CU4'")
                        objSubnode.Text = objRow("description").ToString()
                        objSubnode.Value = "CU4"
                        If objdatarow(0)("access").ToString() = "N" Then
                            objSubnode.ImageUrl = "images/lock.gif"
                        Else
                            objSubnode.ImageUrl = "images/unlock.gif"
                        End If
                        objTreenode3.ChildNodes.Add(objSubnode)
                        Exit Select

                    Case "CU5"
                        Dim objSubnode As New TreeNode
                        Dim objdatarow() As DataRow = objDatatable.Select("module = 'CU5'")
                        objSubnode.Text = objRow("description").ToString()
                        objSubnode.Value = "CU5"
                        If objdatarow(0)("access").ToString() = "N" Then
                            objSubnode.ImageUrl = "images/lock.gif"
                        Else
                            objSubnode.ImageUrl = "images/unlock.gif"
                        End If
                        objTreenode3.ChildNodes.Add(objSubnode)
                        Exit Select

                    Case "CU6"
                        Dim objSubnode As New TreeNode
                        Dim objdatarow() As DataRow = objDatatable.Select("module = 'CU6'")
                        objSubnode.Text = objRow("description").ToString()
                        objSubnode.Value = "CU6"
                        If objdatarow(0)("access").ToString() = "N" Then
                            objSubnode.ImageUrl = "images/lock.gif"
                        Else
                            objSubnode.ImageUrl = "images/unlock.gif"
                        End If
                        objTreenode3.ChildNodes.Add(objSubnode)
                        Exit Select
                    Case "CU8"
                        Dim objSubnode As New TreeNode
                        Dim objdatarow() As DataRow = objDatatable.Select("module = 'CU8'")
                        objSubnode.Text = objRow("description").ToString()
                        objSubnode.Value = "CU8"
                        If objdatarow.Length > 0 Then
                            If objdatarow(0)("access").ToString() = "N" Then
                                objSubnode.ImageUrl = "images/lock.gif"
                            Else
                                objSubnode.ImageUrl = "images/unlock.gif"
                            End If
                        Else

                            Dim objEmployee As New vbEmployee()
                            objEmployee.EmployeeId = ViewState("employee_id").ToString()
                            If ViewState("admin") = "Y" Then
                                Dim strReturn As Integer = objEmployee.fnInsertEmployeeSecurity("CU8", "Y")
                                objSubnode.ImageUrl = "images/unlock.gif"
                                objDatatable = objEmployee.fnGetSecurity()
                            Else
                                Dim strReturn As Integer = objEmployee.fnInsertEmployeeSecurity("CU8", "Y")
                                objSubnode.ImageUrl = "images/lock.gif"
                                objDatatable = objEmployee.fnGetSecurity()
                            End If
                        End If
                        objTreenode3.ChildNodes.Add(objSubnode)
                        Exit Select
                    Case "CU9"
                        Dim objSubnode As New TreeNode
                        Dim objdatarow() As DataRow = objDatatable.Select("module = 'CU9'")
                        objSubnode.Text = objRow("description").ToString()
                        objSubnode.Value = "CU9"
                        If objdatarow.Length > 0 Then
                            If objdatarow(0)("access").ToString() = "N" Then
                                objSubnode.ImageUrl = "images/lock.gif"
                            Else
                                objSubnode.ImageUrl = "images/unlock.gif"
                            End If
                        Else

                            Dim objEmployee As New vbEmployee()
                            objEmployee.EmployeeId = ViewState("employee_id").ToString()
                            If ViewState("admin") = "Y" Then
                                Dim strReturn As Integer = objEmployee.fnInsertEmployeeSecurity("CU9", "Y")
                                objSubnode.ImageUrl = "images/unlock.gif"
                                objDatatable = objEmployee.fnGetSecurity()
                            Else
                                Dim strReturn As Integer = objEmployee.fnInsertEmployeeSecurity("CU9", "Y")
                                objSubnode.ImageUrl = "images/lock.gif"
                                objDatatable = objEmployee.fnGetSecurity()
                            End If
                        End If
                        objTreenode3.ChildNodes.Add(objSubnode)
                        Exit Select


                        '---------------------------------------Report-----------------------------------'
                    Case "RE1"
                        objTreenode81.Text = objRow("description").ToString()
                        objTreenode81.ImageUrl = "images/report.gif"
                        'objTreenode81.SelectAction = TreeNodeSelectAction.Expand
                        objTreenode81.Value = "RE1"
                        objtreenode8.ChildNodes.Add(objTreenode81)
                        Exit Select
                    Case "RE11"
                        Dim objSubnode As New TreeNode
                        Dim objdatarow() As DataRow = objDatatable.Select("module = 'RE11'")
                        objSubnode.Text = objRow("description").ToString()
                        objSubnode.Value = "RE11"
                        If objdatarow(0)("access").ToString() = "N" Then
                            objSubnode.ImageUrl = "images/lock.gif"
                        Else
                            objSubnode.ImageUrl = "images/unlock.gif"
                        End If
                        objTreenode81.ChildNodes.Add(objSubnode)
                        Exit Select
                    Case "RE12"
                        Dim objSubnode As New TreeNode
                        Dim objdatarow() As DataRow = objDatatable.Select("module = 'RE12'")
                        objSubnode.Text = objRow("description").ToString()
                        objSubnode.Value = "RE12"
                        If objdatarow(0)("access").ToString() = "N" Then
                            objSubnode.ImageUrl = "images/lock.gif"
                        Else
                            objSubnode.ImageUrl = "images/unlock.gif"
                        End If
                        objTreenode81.ChildNodes.Add(objSubnode)
                        Exit Select
                    Case "RE13"
                        Dim objSubnode As New TreeNode
                        Dim objdatarow() As DataRow = objDatatable.Select("module = 'RE13'")
                        objSubnode.Text = objRow("description").ToString()
                        objSubnode.Value = "RE13"
                        If objdatarow(0)("access").ToString() = "N" Then
                            objSubnode.ImageUrl = "images/lock.gif"
                        Else
                            objSubnode.ImageUrl = "images/unlock.gif"
                        End If
                        objTreenode81.ChildNodes.Add(objSubnode)
                        Exit Select
                    Case "RE14"
                        Dim objSubnode As New TreeNode
                        Dim objdatarow() As DataRow = objDatatable.Select("module = 'RE14'")
                        objSubnode.Text = objRow("description").ToString()
                        objSubnode.Value = "RE14"
                        If objdatarow(0)("access").ToString() = "N" Then
                            objSubnode.ImageUrl = "images/lock.gif"
                        Else
                            objSubnode.ImageUrl = "images/unlock.gif"
                        End If
                        objTreenode81.ChildNodes.Add(objSubnode)
                        Exit Select
                    Case "RE15"
                        Dim objSubnode As New TreeNode
                        Dim objdatarow() As DataRow = objDatatable.Select("module = 'RE15'")
                        objSubnode.Text = objRow("description").ToString()
                        objSubnode.Value = "RE15"
                        If objdatarow(0)("access").ToString() = "N" Then
                            objSubnode.ImageUrl = "images/lock.gif"
                        Else
                            objSubnode.ImageUrl = "images/unlock.gif"
                        End If
                        objTreenode81.ChildNodes.Add(objSubnode)
                        Exit Select
                    Case "RE16"
                        Dim objSubnode As New TreeNode
                        Dim objdatarow() As DataRow = objDatatable.Select("module = 'RE16'")
                        objSubnode.Text = objRow("description").ToString()
                        objSubnode.Value = "RE16"
                        If objdatarow(0)("access").ToString() = "N" Then
                            objSubnode.ImageUrl = "images/lock.gif"
                        Else
                            objSubnode.ImageUrl = "images/unlock.gif"
                        End If
                        objTreenode81.ChildNodes.Add(objSubnode)
                        Exit Select
                    Case "RE17"
                        Dim objSubnode As New TreeNode
                        Dim objdatarow() As DataRow = objDatatable.Select("module = 'RE17'")
                        objSubnode.Text = objRow("description").ToString()
                        objSubnode.Value = "RE17"
                        If objdatarow(0)("access").ToString() = "N" Then
                            objSubnode.ImageUrl = "images/lock.gif"
                        Else
                            objSubnode.ImageUrl = "images/unlock.gif"
                        End If
                        objTreenode81.ChildNodes.Add(objSubnode)
                        Exit Select

                    Case "RE18"
                        'Dim objSubnode1 As New TreeNode
                        Dim objdatarow() As DataRow = objDatatable.Select("module = 'RE18'")
                        subnode1.Text = objRow("description").ToString()
                        subnode1.Value = "RE18"
                        If objdatarow(0)("access").ToString() = "N" Then
                            subnode1.ImageUrl = "images/lock.gif"
                        Else
                            subnode1.ImageUrl = "images/unlock.gif"
                        End If
                        objTreenode81.ChildNodes.Add(subnode1)
                        Exit Select
                    Case "RE20"
                        Dim objSubnode As New TreeNode
                        Dim objdatarow() As DataRow = objDatatable.Select("module = 'RE20'")
                        objSubnode.Text = objRow("description").ToString()
                        objSubnode.Value = "RE20"
                        If objdatarow(0)("access").ToString() = "N" Then
                            objSubnode.ImageUrl = "images/lock.gif"
                        Else
                            objSubnode.ImageUrl = "images/unlock.gif"
                        End If
                        subnode1.ChildNodes.Add(objSubnode)
                        Exit Select
                    Case "RE19"
                        Dim objSubnode As New TreeNode
                        Dim objdatarow() As DataRow = objDatatable.Select("module = 'RE19'")
                        objSubnode.Text = objRow("description").ToString()
                        objSubnode.Value = "RE19"
                        If objdatarow(0)("access").ToString() = "N" Then
                            objSubnode.ImageUrl = "images/lock.gif"
                        Else
                            objSubnode.ImageUrl = "images/unlock.gif"
                        End If
                        objTreenode81.ChildNodes.Add(objSubnode)
                        Exit Select
                    Case "RE21" '' Reprint close Out the Day
                        Dim objSubnode As New TreeNode
                        Dim objdatarow() As DataRow = objDatatable.Select("module = 'RE21'")
                        objSubnode.Text = objRow("description").ToString()
                        objSubnode.Value = "RE21"
                        If objdatarow(0)("access").ToString() = "N" Then
                            objSubnode.ImageUrl = "images/lock.gif"
                        Else
                            objSubnode.ImageUrl = "images/unlock.gif"
                        End If
                        objTreenode81.ChildNodes.Add(objSubnode)
                        Exit Select
                    Case "RE22" '' Spoilage Report
                        If ViewState("Spoilage") = "1" Then
                            Dim objSubnode As New TreeNode
                            Dim objdatarow() As DataRow = objDatatable.Select("module = 'RE22'")
                            objSubnode.Text = objRow("description").ToString()
                            objSubnode.Value = "RE22"
                            If objdatarow(0)("access").ToString() = "N" Then
                                objSubnode.ImageUrl = "images/lock.gif"
                            Else
                                objSubnode.ImageUrl = "images/unlock.gif"
                            End If
                            objTreenode81.ChildNodes.Add(objSubnode)
                            Exit Select
                        End If
                    Case "RE23" '' Employee Report
                        Dim objSubnode As New TreeNode
                        Dim objdatarow() As DataRow = objDatatable.Select("module = 'RE23'")
                        objSubnode.Text = objRow("description").ToString()
                        objSubnode.Value = "RE23"
                        If objdatarow(0)("access").ToString() = "N" Then
                            objSubnode.ImageUrl = "images/lock.gif"
                        Else
                            objSubnode.ImageUrl = "images/unlock.gif"
                        End If
                        objTreenode81.ChildNodes.Add(objSubnode)
                        Exit Select
                    Case "RE2"
                        Dim objSubnode As New TreeNode
                        Dim objdatarow() As DataRow = objDatatable.Select("module = 'RE2'")
                        objSubnode.Text = objRow("description").ToString()
                        objSubnode.Value = "RE2"
                        If objdatarow(0)("access").ToString() = "N" Then
                            objSubnode.ImageUrl = "images/lock.gif"
                        Else
                            objSubnode.ImageUrl = "images/unlock.gif"
                        End If
                        objtreenode8.ChildNodes.Add(objSubnode)
                        Exit Select
                    Case "RE3"
                        Dim objSubnode As New TreeNode
                        Dim objdatarow() As DataRow = objDatatable.Select("module = 'RE3'")
                        objSubnode.Text = objRow("description").ToString()
                        objSubnode.Value = "RE3"
                        If objdatarow(0)("access").ToString() = "N" Then
                            objSubnode.ImageUrl = "images/lock.gif"
                        Else
                            objSubnode.ImageUrl = "images/unlock.gif"
                        End If
                        objtreenode8.ChildNodes.Add(objSubnode)
                        Exit Select
                    Case "RE4"
                        Dim objSubnode As New TreeNode
                        Dim objdatarow() As DataRow = objDatatable.Select("module = 'RE4'")
                        objSubnode.Text = objRow("description").ToString()
                        objSubnode.Value = "RE4"
                        If objdatarow(0)("access").ToString() = "N" Then
                            objSubnode.ImageUrl = "images/lock.gif"
                        Else
                            objSubnode.ImageUrl = "images/unlock.gif"
                        End If
                        objtreenode8.ChildNodes.Add(objSubnode)
                        Exit Select
                    Case "RE5"
                        Dim objSubnode As New TreeNode
                        Dim objdatarow() As DataRow = objDatatable.Select("module = 'RE5'")
                        objSubnode.Text = objRow("description").ToString()
                        objSubnode.Value = "RE5"
                        If objdatarow(0)("access").ToString() = "N" Then
                            objSubnode.ImageUrl = "images/lock.gif"
                        Else
                            objSubnode.ImageUrl = "images/unlock.gif"
                        End If
                        objtreenode8.ChildNodes.Add(objSubnode)
                        Exit Select
                    Case "RE6"
                        Dim objSubnode As New TreeNode
                        Dim objdatarow() As DataRow = objDatatable.Select("module = 'RE6'")
                        objSubnode.Text = objRow("description").ToString()
                        objSubnode.Value = "RE6"
                        If objdatarow(0)("access").ToString() = "N" Then
                            objSubnode.ImageUrl = "images/lock.gif"
                        Else
                            objSubnode.ImageUrl = "images/unlock.gif"
                        End If
                        objtreenode8.ChildNodes.Add(objSubnode)
                        Exit Select

                    Case "RE7"
                        Dim objSubnode As New TreeNode
                        Dim objdatarow() As DataRow = objDatatable.Select("module = 'RE7'")
                        objSubnode.Text = objRow("description").ToString()
                        objSubnode.Value = "RE7"
                        If objdatarow(0)("access").ToString() = "N" Then
                            objSubnode.ImageUrl = "images/lock.gif"
                        Else
                            objSubnode.ImageUrl = "images/unlock.gif"
                        End If
                        objtreenode8.ChildNodes.Add(objSubnode)
                        Exit Select

                    Case "RE9" ' Cash Drop Report
                        Dim objSubnode As New TreeNode
                        Dim objdatarow() As DataRow = objDatatable.Select("module = 'RE9'")
                        objSubnode.Text = objRow("description").ToString()
                        objSubnode.Value = "RE9"
                        If objdatarow(0)("access").ToString() = "N" Then
                            objSubnode.ImageUrl = "images/lock.gif"
                        Else
                            objSubnode.ImageUrl = "images/unlock.gif"
                        End If
                        objtreenode8.ChildNodes.Add(objSubnode)
                        Exit Select


                        '---------------------------------------Administation option-----------------------------------'

                    Case "AD1"
                        Dim objSubnode As New TreeNode
                        Dim objdatarow() As DataRow = objDatatable.Select("module = 'AD1'")
                        objSubnode.Text = objRow("description").ToString()
                        objSubnode.Value = "AD1"
                        If objdatarow(0)("access").ToString() = "N" Then
                            objSubnode.ImageUrl = "images/lock.gif"
                        Else
                            objSubnode.ImageUrl = "images/unlock.gif"
                        End If
                        objtreenode9.ChildNodes.Add(objSubnode)
                        Exit Select
                    Case "AD2"
                        Dim objSubnode As New TreeNode
                        Dim objdatarow() As DataRow = objDatatable.Select("module = 'AD2'")
                        objSubnode.Text = objRow("description").ToString()
                        objSubnode.Value = "AD2"
                        If objdatarow(0)("access").ToString() = "N" Then
                            objSubnode.ImageUrl = "images/lock.gif"
                        Else
                            objSubnode.ImageUrl = "images/unlock.gif"
                        End If
                        objtreenode9.ChildNodes.Add(objSubnode)
                        Exit Select
                    Case "AD3"
                        Dim objSubnode As New TreeNode
                        Dim objdatarow() As DataRow = objDatatable.Select("module = 'AD3'")
                        objSubnode.Text = objRow("description").ToString()
                        objSubnode.Value = "AD3"
                        If objdatarow(0)("access").ToString() = "N" Then
                            objSubnode.ImageUrl = "images/lock.gif"
                        Else
                            objSubnode.ImageUrl = "images/unlock.gif"
                        End If
                        objtreenode9.ChildNodes.Add(objSubnode)
                        Exit Select
                    Case "AD4"
                        Dim objSubnode As New TreeNode
                        Dim objdatarow() As DataRow = objDatatable.Select("module = 'AD4'")
                        objSubnode.Text = objRow("description").ToString()
                        objSubnode.Value = "AD4"
                        If objdatarow(0)("access").ToString() = "N" Then
                            objSubnode.ImageUrl = "images/lock.gif"
                        Else
                            objSubnode.ImageUrl = "images/unlock.gif"
                        End If
                        objtreenode9.ChildNodes.Add(objSubnode)
                        Exit Select
                    Case "AD5"
                        Dim objSubnode As New TreeNode
                        Dim objdatarow() As DataRow = objDatatable.Select("module = 'AD5'")
                        objSubnode.Text = objRow("description").ToString()
                        objSubnode.Value = "AD5"
                        If objdatarow(0)("access").ToString() = "N" Then
                            objSubnode.ImageUrl = "images/lock.gif"
                        Else
                            objSubnode.ImageUrl = "images/unlock.gif"
                        End If
                        objtreenode9.ChildNodes.Add(objSubnode)
                        Exit Select
                    Case "AD6"
                        Dim objSubnode As New TreeNode
                        Dim objdatarow() As DataRow = objDatatable.Select("module = 'AD6'")
                        objSubnode.Text = objRow("description").ToString()
                        objSubnode.Value = "AD6"
                        If objdatarow(0)("access").ToString() = "N" Then
                            objSubnode.ImageUrl = "images/lock.gif"
                        Else
                            objSubnode.ImageUrl = "images/unlock.gif"
                        End If
                        objtreenode9.ChildNodes.Add(objSubnode)
                        Exit Select
                    Case "AD7"
                        Dim objSubnode As New TreeNode
                        Dim objdatarow() As DataRow = objDatatable.Select("module = 'AD7'")
                        objSubnode.Text = objRow("description").ToString()
                        objSubnode.Value = "AD7"
                        If objdatarow(0)("access").ToString() = "N" Then
                            objSubnode.ImageUrl = "images/lock.gif"
                        Else
                            objSubnode.ImageUrl = "images/unlock.gif"
                        End If
                        objtreenode9.ChildNodes.Add(objSubnode)
                        Exit Select

                        'Case "AD8"
                        '    Dim objSubnode As New TreeNode
                        '    Dim objdatarow() As DataRow = objDatatable.Select("module = 'AD8'")
                        '    objSubnode.Text = objRow("description").ToString()
                        '    objSubnode.Value = "AD8"
                        '    If objdatarow(0)("access").ToString() = "N" Then
                        '        objSubnode.ImageUrl = "images/lock.gif"
                        '    Else
                        '        objSubnode.ImageUrl = "images/unlock.gif"
                        '    End If
                        '    objtreenode9.ChildNodes.Add(objSubnode)
                        '    Exit Select

                        '---------------------------------------Marketing Center option-----------------------------------'
                    Case "MC1"
                        Dim objSubnode As New TreeNode
                        Dim objdatarow() As DataRow = objDatatable.Select("module = 'MC1'")
                        objSubnode.Text = objRow("description").ToString()
                        objSubnode.Value = "MC1"
                        If objdatarow(0)("access").ToString() = "N" Then
                            objSubnode.ImageUrl = "images/lock.gif"
                        Else
                            objSubnode.ImageUrl = "images/unlock.gif"
                        End If
                        objtreenode10.ChildNodes.Add(objSubnode)
                        Exit Select
                    Case "MC2"
                        Dim objSubnode As New TreeNode
                        Dim objdatarow() As DataRow = objDatatable.Select("module = 'MC2'")
                        objSubnode.Text = objRow("description").ToString()
                        objSubnode.Value = "MC2"
                        If objdatarow(0)("access").ToString() = "N" Then
                            objSubnode.ImageUrl = "images/lock.gif"
                        Else
                            objSubnode.ImageUrl = "images/unlock.gif"
                        End If
                        objtreenode10.ChildNodes.Add(objSubnode)
                        Exit Select
                    Case "MC3"
                        Dim objSubnode As New TreeNode
                        Dim objdatarow() As DataRow = objDatatable.Select("module = 'MC3'")
                        objSubnode.Text = objRow("description").ToString()
                        objSubnode.Value = "MC3"
                        If objdatarow(0)("access").ToString() = "N" Then
                            objSubnode.ImageUrl = "images/lock.gif"
                        Else
                            objSubnode.ImageUrl = "images/unlock.gif"
                        End If
                        objtreenode10.ChildNodes.Add(objSubnode)
                        Exit Select
                    Case "MC4"
                        Dim objSubnode As New TreeNode
                        Dim objdatarow() As DataRow = objDatatable.Select("module = 'MC4'")
                        objSubnode.Text = objRow("description").ToString()
                        objSubnode.Value = "MC4"
                        If objdatarow(0)("access").ToString() = "N" Then
                            objSubnode.ImageUrl = "images/lock.gif"
                        Else
                            objSubnode.ImageUrl = "images/unlock.gif"
                        End If
                        objtreenode10.ChildNodes.Add(objSubnode)
                        Exit Select
                    Case "DT1"
                        Dim objSubnode As New TreeNode
                        Dim objdatarow() As DataRow = objDatatable.Select("module = 'DT1'")
                        objSubnode.Text = objRow("description").ToString()
                        objSubnode.Value = "DT1"
                        If objdatarow(0)("access").ToString() = "N" Then
                            objSubnode.ImageUrl = "images/lock.gif"
                        Else
                            objSubnode.ImageUrl = "images/unlock.gif"
                        End If
                        objTreenode33.ChildNodes.Add(objSubnode)
                        Exit Select


                    Case "DT2"
                        Dim objSubnode As New TreeNode
                        Dim objdatarow() As DataRow = objDatatable.Select("module = 'DT2'")
                        objSubnode.Text = objRow("description").ToString()
                        objSubnode.Value = "DT2"
                        If objdatarow(0)("access").ToString() = "N" Then
                            objSubnode.ImageUrl = "images/lock.gif"
                        Else
                            objSubnode.ImageUrl = "images/unlock.gif"
                        End If
                        objTreenode33.ChildNodes.Add(objSubnode)
                        Exit Select


                    Case "DT4"
                        Dim objSubnode As New TreeNode
                        Dim objdatarow() As DataRow = objDatatable.Select("module = 'DT4'")
                        objSubnode.Text = objRow("description").ToString()
                        objSubnode.Value = "DT4"
                        If objdatarow(0)("access").ToString() = "N" Then
                            objSubnode.ImageUrl = "images/lock.gif"
                        Else
                            objSubnode.ImageUrl = "images/unlock.gif"
                        End If
                        objTreenode33.ChildNodes.Add(objSubnode)
                        Exit Select


                        'Case "DT3"
                        '    Dim objSubnode As New TreeNode
                        '    Dim objdatarow() As DataRow = objDatatable.Select("module = 'DT3'")
                        '    objSubnode.Text = objRow("description").ToString()
                        '    objSubnode.Value = "DT3"
                        '    If objdatarow(0)("access").ToString() = "N" Then
                        '        objSubnode.ImageUrl = "images/lock.gif"
                        '    Else
                        '        objSubnode.ImageUrl = "images/unlock.gif"
                        '    End If
                        '    objTreenode33.ChildNodes.Add(objSubnode)
                        '    Exit Select
                End Select
            Next
            ViewState("dv_old") = objDatatable

        End If
        If ViewState("admin") = "Y" Then
            'pnlTreeView.Enabled = False
            TreeView1.ExpandAll()
        Else
            'pnlTreeView.Enabled = True
        End If
        upSecurity.Update()
    End Sub
    Public Sub AddEmployee(ByVal subnode As TreeNode, ByVal objDatatable As DataTable, ByVal objModule As DataTable)
        For Each objRow In objModule.Rows
            Select Case (objRow("module").ToString.Trim())

                Case "EM1"
                    Dim objSubnode As New TreeNode
                    Dim objdatarow() As DataRow = objDatatable.Select("module = 'EM1'")
                    objSubnode.Text = objRow("description").ToString()
                    objSubnode.Value = "EM1"
                    If Convert.ToBoolean(objdatarow(0)("access").ToString()) = False Then
                        objSubnode.ImageUrl = "images/lock.gif"
                    Else
                        objSubnode.ImageUrl = "images/unlock.gif"
                    End If
                    subnode.ChildNodes.Add(objSubnode)
                    Exit Select

                Case "EM2"
                    Dim objSubnode As New TreeNode
                    Dim objdatarow() As DataRow = objDatatable.Select("module = 'EM2'")
                    objSubnode.Text = objRow("description").ToString()
                    objSubnode.Value = "EM2"
                    If Convert.ToBoolean(objdatarow(0)("access").ToString()) = False Then
                        objSubnode.ImageUrl = "images/lock.gif"
                    Else
                        objSubnode.ImageUrl = "images/unlock.gif"
                    End If
                    subnode.ChildNodes.Add(objSubnode)
                    Exit Select

                Case "EM3"
                    Dim objSubnode As New TreeNode
                    Dim objdatarow() As DataRow = objDatatable.Select("module = 'EM3'")
                    objSubnode.Text = objRow("description").ToString()
                    objSubnode.Value = "EM3"
                    If Convert.ToBoolean(objdatarow(0)("access").ToString()) = False Then
                        objSubnode.ImageUrl = "images/lock.gif"
                    Else
                        objSubnode.ImageUrl = "images/unlock.gif"
                    End If
                    subnode.ChildNodes.Add(objSubnode)
                    Exit Select

                Case "EM4"
                    Dim objSubnode As New TreeNode
                    Dim objdatarow() As DataRow = objDatatable.Select("module = 'EM4'")
                    objSubnode.Text = objRow("description").ToString()
                    objSubnode.Value = "EM4"
                    If Convert.ToBoolean(objdatarow(0)("access").ToString()) = False Then
                        objSubnode.ImageUrl = "images/lock.gif"
                    Else
                        objSubnode.ImageUrl = "images/unlock.gif"
                    End If
                    subnode.ChildNodes.Add(objSubnode)
                    Exit Select

                Case "EM5"
                    Dim objSubnode As New TreeNode
                    Dim objdatarow() As DataRow = objDatatable.Select("module = 'EM5'")
                    objSubnode.Text = objRow("description").ToString()
                    objSubnode.Value = "EM5"
                    If Convert.ToBoolean(objdatarow(0)("access").ToString()) = False Then
                        objSubnode.ImageUrl = "images/lock.gif"
                    Else
                        objSubnode.ImageUrl = "images/unlock.gif"
                    End If
                    subnode.ChildNodes.Add(objSubnode)
                    Exit Select
            End Select
        Next
    End Sub
    Public Sub AddCustomer(ByVal subnode As TreeNode, ByVal objDatatable As DataTable, ByVal objModule As DataTable)
        For Each objRow In objModule.Rows
            Select Case (objRow("module").ToString.Trim())

                Case "CU1"
                    Dim objSubnode As New TreeNode
                    Dim objdatarow() As DataRow = objDatatable.Select("module = 'CU1'")
                    objSubnode.Text = objRow("description").ToString()
                    objSubnode.Value = "CU1"
                    If Convert.ToBoolean(objdatarow(0)("access").ToString()) = False Then
                        objSubnode.ImageUrl = "images/lock.gif"
                    Else
                        objSubnode.ImageUrl = "images/unlock.gif"
                    End If
                    subnode.ChildNodes.Add(objSubnode)
                    Exit Select

                Case "CU2"
                    Dim objSubnode As New TreeNode
                    Dim objdatarow() As DataRow = objDatatable.Select("module = 'CU2'")
                    objSubnode.Text = objRow("description").ToString()
                    objSubnode.Value = "CU2"
                    If Convert.ToBoolean(objdatarow(0)("access").ToString()) = False Then
                        objSubnode.ImageUrl = "images/lock.gif"
                    Else
                        objSubnode.ImageUrl = "images/unlock.gif"
                    End If
                    subnode.ChildNodes.Add(objSubnode)
                    Exit Select

                Case "CU3"
                    Dim objSubnode As New TreeNode
                    Dim objdatarow() As DataRow = objDatatable.Select("module = 'CU3'")
                    objSubnode.Text = objRow("description").ToString()
                    objSubnode.Value = "CU3"
                    If Convert.ToBoolean(objdatarow(0)("access").ToString()) = False Then
                        objSubnode.ImageUrl = "images/lock.gif"
                    Else
                        objSubnode.ImageUrl = "images/unlock.gif"
                    End If
                    subnode.ChildNodes.Add(objSubnode)
                    Exit Select

                Case "CU4"
                    Dim objSubnode As New TreeNode
                    Dim objdatarow() As DataRow = objDatatable.Select("module = 'CU4'")
                    objSubnode.Text = objRow("description").ToString()
                    objSubnode.Value = "CU4"
                    If Convert.ToBoolean(objdatarow(0)("access").ToString()) = False Then
                        objSubnode.ImageUrl = "images/lock.gif"
                    Else
                        objSubnode.ImageUrl = "images/unlock.gif"
                    End If
                    subnode.ChildNodes.Add(objSubnode)
                    Exit Select

                Case "CU5"
                    Dim objSubnode As New TreeNode
                    Dim objdatarow() As DataRow = objDatatable.Select("module = 'CU5'")
                    objSubnode.Text = objRow("description").ToString()
                    objSubnode.Value = "CU5"
                    If Convert.ToBoolean(objdatarow(0)("access").ToString()) = False Then
                        objSubnode.ImageUrl = "images/lock.gif"
                    Else
                        objSubnode.ImageUrl = "images/unlock.gif"
                    End If
                    subnode.ChildNodes.Add(objSubnode)
                    Exit Select
            End Select

        Next
    End Sub
#End Region
#Region "Security"

    Public Sub getSecurity()

        TreeView1.Nodes.Clear()

        Dim objEmployee As New vbEmployee()
        Dim objDatatable As New DataTable()
        objEmployee.EmployeeId = ViewState("employee_id").ToString()
        objDatatable = objEmployee.fnGetSecurity()

        If objDatatable.Rows.Count > 0 Then
            ViewState("security") = "update"

            If objDatatable.Rows(0)("admin") = "Y" Then
                ViewState("admin") = "Y"
                'btnaccesssecurity.Visible = False
                'btnNoaccesssecurity.Visible = False
                'btnSavesecurity.Visible = False
                'dvSecurityAdmin.Visible = True

                lblEmpName.Text = objDatatable.Rows(0)("empname").ToString.Trim
                'upnlSecurityAdmin.Update()
            Else
                ViewState("admin") = "N"
                'btnaccesssecurity.Visible = True
                'btnNoaccesssecurity.Visible = True
                'btnSavesecurity.Visible = True
                'dvSecurityAdmin.Visible = False

            End If
            'If ViewState("isadmin").ToString.ToLower() = "n" Then
            If objDatatable.Rows(0)("admin") = "N" Then
                'lblInstruction.Text = "Click on any option to change it's access status.<br/>(Access/No Access)<br/><br>Click on any parent link (Ring Sales, Inventory,Customers,<br/>Employee's,etc.) to change all of its sub-menu options."
                'dvinstruction.Visible = True
            Else
                'dvinstruction.Visible = False
            End If
            Call bindTreeview(objDatatable)
            TreeView1.Nodes(0).Select()
        Else
            ViewState("security") = "insert"
            'bindAllNodes(objDatatable)
            Call bindTreeview(objDatatable)
        End If
        objConnection.DBDisconnect()
    End Sub
    Dim objConnection As conClass = New conClass()
    Dim objCommand As SqlCommand = New SqlCommand()
    Dim objTransaction As SqlTransaction

    Public Sub saveEmpSecurity()

        objConnection.DBDisconnect()
        objConnection.DBconnect()
        objTransaction = objConnection.con1.BeginTransaction
        objCommand.Transaction = objTransaction
        objCommand.Connection = objConnection.con1
        objCommand.CommandText = "proc_employee_update_security"
        objCommand.CommandType = CommandType.StoredProcedure
        Call saveTreeview(TreeView1)
        objTransaction.Commit()
        objConnection.DBDisconnect()
        TreeView1.CollapseAll()
        TreeView1.Nodes(0).Select()
        objConnection.DBDisconnect()

        Dim dtSecurity As DataTable = getChangedSecurityValue_Table()

        Call saveEmployeeSecurity_SecurityLog(dtSecurity)
    End Sub

    Private Sub saveEmployeeSecurity_SecurityLog(ByVal dt_securityinfo As DataTable)
        If dt_securityinfo.Rows.Count > 0 Then

            Dim str As String = ""

            For m As Integer = 0 To dt_securityinfo.Rows.Count - 1
                str = ""
                If dt_securityinfo.Rows(m)("column1").ToString.ToLower().Trim = "rs" Then
                    str = "Ring Sales"
                    If dt_securityinfo.Rows(m)("column4").ToString.ToLower().Trim = "rs19" Then
                        str = "Ring Sales\Today's Sales"
                    ElseIf dt_securityinfo.Rows(m)("column4").ToString.ToLower().Trim = "rs20" Then
                        str = "Ring Sales\Edit Line"
                    End If
                ElseIf dt_securityinfo.Rows(m)("column1").ToString.ToLower().Trim = "in" Then
                    str = "Inventory\Add/Edit/More"
                ElseIf dt_securityinfo.Rows(m)("column1").ToString.ToLower().Trim = "cu" Then
                    str = "Customers"
                ElseIf dt_securityinfo.Rows(m)("column1").ToString.ToLower().Trim = "vn" Then
                    str = "Vendors"
                ElseIf dt_securityinfo.Rows(m)("column1").ToString.ToLower().Trim = "em" Then
                    str = "Employees"
                ElseIf dt_securityinfo.Rows(m)("column1").ToString.ToLower().Trim = "ts" Then
                    str = "Today's Sales"
                ElseIf dt_securityinfo.Rows(m)("column1").ToString.ToLower().Trim = "re" Then
                    str = "Reports"
                    If dt_securityinfo.Rows(m)("column4").ToString.ToLower().Trim = "re11" Or dt_securityinfo.Rows(m)("column4").ToString.ToLower().Trim = "re12" Or dt_securityinfo.Rows(m)("column4").ToString.ToLower().Trim = "re13" Or dt_securityinfo.Rows(m)("column4").ToString.ToLower().Trim = "re14" Or dt_securityinfo.Rows(m)("column4").ToString.ToLower().Trim = "re15" Or dt_securityinfo.Rows(m)("column4").ToString.ToLower().Trim = "re16" Or dt_securityinfo.Rows(m)("column4").ToString.ToLower().Trim = "re17" Or dt_securityinfo.Rows(m)("column4").ToString.ToLower().Trim = "re18" Or dt_securityinfo.Rows(m)("column4").ToString.ToLower().Trim = "re19" Then
                        str = "Reports\Sales Reports"
                    ElseIf dt_securityinfo.Rows(m)("column4").ToString.ToLower().Trim = "re20" Then
                        str = "Reports\Sales Reports\Gift Cards Report"
                    End If
                ElseIf dt_securityinfo.Rows(m)("column1").ToString.ToLower().Trim = "ad" Then
                    str = "Administration Options"
                ElseIf dt_securityinfo.Rows(m)("column1").ToString.ToLower().Trim = "mc" Then
                    str = "Marketing Center"
                ElseIf dt_securityinfo.Rows(m)("column1").ToString.ToLower().Trim = "dt" Then
                    str = "Data Maintenence"
                ElseIf dt_securityinfo.Rows(m)("column4").ToString.ToLower().Trim = "mo14" OrElse dt_securityinfo.Rows(m)("column4").ToString.ToLower().Trim = "mo15" OrElse dt_securityinfo.Rows(m)("column4").ToString.ToLower().Trim = "mo16" Then
                    str = "Inventory"
                End If
                If dt_securityinfo.Rows(m)("column1").ToString.ToLower().Trim <> "mo" OrElse dt_securityinfo.Rows(m)("column4").ToString.ToLower().Trim = "mo6" OrElse dt_securityinfo.Rows(m)("column4").ToString.ToLower().Trim = "mo14" OrElse dt_securityinfo.Rows(m)("column4").ToString.ToLower().Trim = "mo15" OrElse dt_securityinfo.Rows(m)("column4").ToString.ToLower().Trim = "mo16" Then
                    Dim objEmployee As New vbEmployee
                    objEmployee.EmployeeId = ViewState("employee_id")
                    objEmployee.Stationno = Session("stationno")
                    objEmployee.Storeno = Session("storeno")
                    objEmployee.FieldName = IIf(str <> "", str & "\", "") & dt_securityinfo.Rows(m)("column2").ToString.Trim

                    If dt_securityinfo.Rows(m)("column3").ToString.ToLower().Trim = "y" Then
                        objEmployee.BeforeValue = "No Access"
                        objEmployee.AfterValue = "Access"
                    Else
                        objEmployee.BeforeValue = "Access"
                        objEmployee.AfterValue = "No Access"
                    End If
                    objEmployee.user = Session("emp_id")

                    objEmployee.fnInsertEmployeeSecurityChanges_SecurityLog()
                End If

            Next
        End If
    End Sub

    Private Function getChangedSecurityValue_Table() As DataTable
        Dim dt1 As New DataTable
        Dim objdatatable As New DataTable
        Dim objModule As DataTable = getModuleinfo()
        ViewState("objModule") = objModule
        Dim modle As New DataColumn
        Dim access As New DataColumn
        Dim i As Integer = 0
        dt1.Columns.Add(modle)
        dt1.Columns.Add(access)
        Dim objEmployee As New vbEmployee()
        objEmployee.EmployeeId = ViewState("employee_id").ToString()
        objdatatable = objEmployee.fnGetSecurity()


        For Each objRow In objModule.Rows
            Dim dr As DataRow
            dr = dt1.NewRow
            dr(0) = objRow("module").ToString.Trim()
            dr(1) = objdatatable.Rows(i).Item("access").ToString()
            i = i + 1
            dt1.Rows.Add(dr)
        Next

        Dim dv As New DataView(dt1)
        dv.Sort = "column1 asc"

        Dim old_dt As DataTable
        old_dt = CType(ViewState("dv_old"), DataTable)
        Dim dv_old As DataView = old_dt.DefaultView
        dv_old.Sort = "module asc"
        Dim dt_securityinfo As New DataTable
        Dim modul As New DataColumn
        Dim acces As New DataColumn
        Dim Description As New DataColumn
        Dim modules As New DataColumn

        dt_securityinfo.Columns.Add(modul)
        dt_securityinfo.Columns.Add(Description)
        dt_securityinfo.Columns.Add(acces)
        dt_securityinfo.Columns.Add(modules)

        Dim k As Integer = 0
        For Each objRow In objModule.Rows
            If dv_old.Item(k).Item(3).ToString.Trim = dv.Item(k).Item(0).ToString.Trim Then
                If dv_old.Item(k).Item(4) <> dv.Item(k).Item(1) Then
                    Dim dr_securityinfo As DataRow
                    Dim strtemp As String = ""
                    objEmployee.Modulecheck = dv.Item(k).Item(0)
                    strtemp = objEmployee.GetModule
                    dr_securityinfo = dt_securityinfo.NewRow
                    dr_securityinfo(0) = objModule.Rows(k).Item(0).ToString()
                    dr_securityinfo(1) = strtemp.ToString
                    dr_securityinfo(2) = dv.Item(k).Item(1).ToString
                    dr_securityinfo(3) = dv.Item(k).Item(0).ToString

                    dt_securityinfo.Rows.Add(dr_securityinfo)
                    Dim dvsecurityinfo As DataView = dt_securityinfo.DefaultView
                    dvsecurityinfo.Sort = "column1 asc"
                End If
            End If
            k = k + 1
        Next
        Return dt_securityinfo
    End Function

#End Region
#Region "Department Grid"
    Public Sub getEmployeeDepartmentlist()
        Dim objConnection As conClass = New conClass()
        objConnection.DBconnect()

        'create command for stored procedure
        Dim objCommand As SqlCommand = New SqlCommand()
        objCommand.Connection = objConnection.con1
        objCommand.CommandText = "proc_get_Employee_Department_Discount_limit_list"



        objCommand.CommandType = CommandType.StoredProcedure
        objCommand.Parameters.AddWithValue("@storeno", Session("storeno"))
        'objCommand.Parameters.AddWithValue("@empdisc_emp_id", ViewState("emp_id").ToString())

        Dim ObjDataAdapter As SqlDataAdapter = New SqlDataAdapter()
        ObjDataAdapter.SelectCommand = objCommand

        Dim objDataset As DataSet = New DataSet()


        MyDataTable = New DataTable("stationinfo")
        MyDataColumn = New DataColumn("SrNo", GetType(Integer))

        MyDataColumn.AutoIncrement = True
        MyDataColumn.AutoIncrementSeed = 1
        MyDataTable.Columns.Add(MyDataColumn)
        objDataset.Tables.Add(MyDataTable)
        'Try
        ObjDataAdapter.Fill(objDataset, "stationinfo")

        If Not ViewState("DepartmentId") Is Nothing Then
            Dim total As Integer = 0
            Dim extender As Decimal = objDataset.Tables(0).Rows.Count Mod gvDepartment.PageSize
            If extender = 0 Then
                total = objDataset.Tables(0).Rows.Count / gvDepartment.PageSize
            Else
                total = objDataset.Tables(0).Rows.Count / gvDepartment.PageSize

            End If
            Dim rowcollection() As DataRow = objDataset.Tables(0).Select("Dept_id='" + ViewState("DepartmentId") + "'")
            If ViewState("pageindex") Is Nothing Then
                If rowcollection.Length > 0 Then

                    Dim rowno As Integer = rowcollection(0)("SrNo")
                    'Dim rowno As Integer = ViewState("rowindex")
                    Dim pagesize As Integer = gvDepartment.PageSize
                    For i As Integer = 0 To total
                        If pagesize * (i + 1) >= rowno Then
                            gvDepartment.PageIndex = i
                            Exit For
                        End If

                    Next
                Else
                    gvDepartment.PageIndex = ViewState("pageindex")
                End If
            Else
                gvDepartment.PageIndex = ViewState("pageindex")
            End If

        Else
            ViewState("DepartmentId") = Nothing
        End If


        If objDataset.Tables(0).Rows.Count > 0 Then
            gvDepartment.DataSource = objDataset.Tables(0)
            gvDepartment.DataBind()

            For i As Integer = 0 To gvDepartment.Rows.Count - 1
                Dim txt As New TextBox
                txt = CType(gvDepartment.Rows(i).FindControl("txtDiscount"), TextBox)
                txt.Enabled = True
            Next
            CType(gvDepartment.Rows(0).FindControl("txtDiscount"), TextBox).Focus()

        Else
            gvDepartment.DataSource = objDataset.Tables(0)
            gvDepartment.DataBind()
        End If
        objConnection.DBDisconnect()

    End Sub
    Public Sub getDepartmentlist()

        Dim objConnection As conClass = New conClass()
        objConnection.DBconnect()

        'create command for stored procedure
        Dim objCommand As SqlCommand = New SqlCommand()
        objCommand.Connection = objConnection.con1
        objCommand.CommandText = "proc_department_get_Discount_limit_list"
        objCommand.CommandType = CommandType.StoredProcedure
        objCommand.Parameters.AddWithValue("@storeno", Session("storeno"))
        objCommand.Parameters.AddWithValue("@empdisc_emp_id", ViewState("employee_id").ToString())

        Dim ObjDataAdapter As SqlDataAdapter = New SqlDataAdapter()
        ObjDataAdapter.SelectCommand = objCommand

        MyDataTable = New DataTable("stationinfo")
        MyDataColumn = New DataColumn("SrNo", GetType(Integer))

        MyDataColumn.AutoIncrement = True
        MyDataColumn.AutoIncrementSeed = 1
        MyDataTable.Columns.Add(MyDataColumn)
        objDataset.Tables.Add(MyDataTable)
        'Try
        ObjDataAdapter.Fill(objDataset, "stationinfo")

        If objDataset.Tables(0).Rows.Count > 0 Then
            gvDepartment.DataSource = objDataset.Tables(0)
            gvDepartment.DataBind()

            For i As Integer = 0 To gvDepartment.Rows.Count - 1
                Dim txt As New TextBox
                txt = CType(gvDepartment.Rows(i).FindControl("txtDiscount"), TextBox)
                txt.Enabled = True
            Next
            CType(gvDepartment.Rows(0).FindControl("txtDiscount"), TextBox).Focus()
        Else

            gvDepartment.DataSource = objDataset.Tables(0)
            gvDepartment.DataBind()
        End If

        objConnection.DBDisconnect()
    End Sub
    Protected Sub gvDepartment_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvDepartment.PageIndexChanging
        ViewState("pageindex") = e.NewPageIndex
        ViewState("DepartmentId") = Nothing
        gvDepartment.PageIndex = e.NewPageIndex
        Call getDepartmentlist()

    End Sub
#End Region

    Public Sub getallstoresetting()
        Dim objGlobalsetup As New vbRingsale
        objGlobalsetup.StoreNo = Session("storeno")
        Dim objDataset As DataSet = objGlobalsetup.fnGetgetallstoresetting()
        If objDataset.Tables.Count > 0 Then
            'POS options
            If objDataset.Tables(0).Rows.Count > 0 Then
                ViewState("Spoilage") = IIf(IsDBNull(objDataset.Tables(0).Rows(0).Item("Spoilage")), False, objDataset.Tables(0).Rows(0).Item("Spoilage"))
            End If
        End If
    End Sub

    Private Sub getEmployeeSecurityCount()
        Dim dt, dtAll As DataTable
        Dim objEmp As New vbEmployee
        objEmp.EmployeeId = Convert.ToInt32(ViewState("employee_id"))
        dt = objEmp.fnGetSecurityCount()
        dtAll = objEmp.fnGetAllSecurityCount()
        If dt.Rows.Count > 0 AndAlso dtAll.Rows.Count > 0 Then
            If dt.Rows(0)("RingSaleCount").ToString = dtAll.Rows(0)("RingSaleCount").ToString Then
                lblRingSalesSecurity.Text = "User has No Access"
                lblRingSalesSecurity.Visible = True
            Else
                If dt.Rows(0)("RingSaleCount").ToString = "1" Then
                    lblRingSalesSecurity.Text = dt.Rows(0)("RingSaleCount").ToString + " item marked as No Access"
                    lblRingSalesSecurity.Visible = True
                ElseIf dt.Rows(0)("RingSaleCount").ToString = "0" Then
                    lblRingSalesSecurity.Visible = False
                Else
                    lblRingSalesSecurity.Text = dt.Rows(0)("RingSaleCount").ToString + " items marked as No Access"
                    lblRingSalesSecurity.Visible = True
                End If
            End If

            If dt.Rows(0)("InventoryCount").ToString = dtAll.Rows(0)("InventoryCount").ToString Then
                lblInventorySecurity.Text = "User has No Access"
                lblInventorySecurity.Visible = True
            Else
                If dt.Rows(0)("InventoryCount").ToString = "1" Then
                    lblInventorySecurity.Text = dt.Rows(0)("InventoryCount").ToString + " item marked as No Access"
                    lblInventorySecurity.Visible = True
                ElseIf dt.Rows(0)("InventoryCount").ToString = "0" Then
                    lblInventorySecurity.Visible = False
                Else
                    lblInventorySecurity.Text = dt.Rows(0)("InventoryCount").ToString + " items marked as No Access"
                    lblInventorySecurity.Visible = True
                End If
            End If

            If dt.Rows(0)("CustomersCount").ToString = dtAll.Rows(0)("CustomersCount").ToString Then
                lblCustomersSecurity.Text = "User has No Access"
                lblCustomersSecurity.Visible = True
            Else
                If dt.Rows(0)("CustomersCount").ToString = "1" Then
                    lblCustomersSecurity.Text = dt.Rows(0)("CustomersCount").ToString + " item marked as No Access"
                    lblCustomersSecurity.Visible = True
                ElseIf dt.Rows(0)("CustomersCount").ToString = "0" Then
                    lblCustomersSecurity.Visible = False
                Else
                    lblCustomersSecurity.Text = dt.Rows(0)("CustomersCount").ToString + " items marked as No Access"
                    lblCustomersSecurity.Visible = True
                End If
            End If

            If dt.Rows(0)("VendorsCount").ToString = dtAll.Rows(0)("VendorsCount").ToString Then
                lblVendorsSecurity.Text = "User has No Access"
                lblVendorsSecurity.Visible = True
            Else
                If dt.Rows(0)("VendorsCount").ToString = "1" Then
                    lblVendorsSecurity.Text = dt.Rows(0)("VendorsCount").ToString + " item marked as No Access"
                    lblVendorsSecurity.Visible = True
                ElseIf dt.Rows(0)("VendorsCount").ToString = "0" Then
                    lblVendorsSecurity.Visible = False
                Else
                    lblVendorsSecurity.Text = dt.Rows(0)("VendorsCount").ToString + " items marked as No Access"
                    lblVendorsSecurity.Visible = True
                End If
            End If

            If dt.Rows(0)("EmployeesCount").ToString = dtAll.Rows(0)("EmployeesCount").ToString Then
                lblEmployeesSecurity.Text = "User has No Access"
                lblEmployeesSecurity.Visible = True
            Else
                If dt.Rows(0)("EmployeesCount").ToString = "1" Then
                    lblEmployeesSecurity.Text = dt.Rows(0)("EmployeesCount").ToString + " item marked as No Access"
                    lblEmployeesSecurity.Visible = True
                ElseIf dt.Rows(0)("EmployeesCount").ToString = "0" Then
                    lblEmployeesSecurity.Visible = False
                Else
                    lblEmployeesSecurity.Text = dt.Rows(0)("EmployeesCount").ToString + " items marked as No Access"
                    lblEmployeesSecurity.Visible = True
                End If
            End If

            If dt.Rows(0)("TodaysSalesCount").ToString = dtAll.Rows(0)("TodaysSalesCount").ToString Then
                lblTodaysSalesSecurity.Text = "User has No Access"
                lblTodaysSalesSecurity.Visible = True
            Else
                If dt.Rows(0)("TodaysSalesCount").ToString = "1" Then
                    lblTodaysSalesSecurity.Text = dt.Rows(0)("TodaysSalesCount").ToString + " item marked as No Access"
                    lblTodaysSalesSecurity.Visible = True
                ElseIf dt.Rows(0)("TodaysSalesCount").ToString = "0" Then
                    lblTodaysSalesSecurity.Visible = False
                Else
                    lblTodaysSalesSecurity.Text = dt.Rows(0)("TodaysSalesCount").ToString + " items marked as No Access"
                    lblTodaysSalesSecurity.Visible = True
                End If
            End If

            If dt.Rows(0)("CloseTheDayCount").ToString = dtAll.Rows(0)("CloseTheDayCount").ToString Then
                lblCloseThDaySecurity.Text = "User has No Access"
                lblCloseThDaySecurity.Visible = True
            Else
                If dt.Rows(0)("CloseTheDayCount").ToString = "1" Then
                    lblCloseThDaySecurity.Text = dt.Rows(0)("CloseTheDayCount").ToString + " item marked as No Access"
                    lblCloseThDaySecurity.Visible = True
                ElseIf dt.Rows(0)("CloseTheDayCount").ToString = "0" Then
                    lblCloseThDaySecurity.Visible = False
                Else
                    lblCloseThDaySecurity.Text = dt.Rows(0)("CloseTheDayCount").ToString + " items marked as No Access"
                    lblCloseThDaySecurity.Visible = True
                End If
            End If

            If dt.Rows(0)("ReportsCount").ToString = dtAll.Rows(0)("ReportsCount").ToString Then
                lblReportsSecurity.Text = "User has No Access"
                lblReportsSecurity.Visible = True
            Else
                If dt.Rows(0)("ReportsCount").ToString = "1" Then
                    lblReportsSecurity.Text = dt.Rows(0)("ReportsCount").ToString + " item marked as No Access"
                    lblReportsSecurity.Visible = True
                ElseIf dt.Rows(0)("ReportsCount").ToString = "0" Then
                    lblReportsSecurity.Visible = False
                Else
                    If Convert.ToBoolean(Session("IsCorpOffice")) = True Then
                        lblReportsSecurity.Text = dt.Rows(0)("ReportsCount").ToString + " items marked as No Access"
                        lblReportsSecurity.Visible = True
                    Else
                        Dim reportcount As Integer = 0
                        reportcount = dt.Rows(0)("ReportsCount").ToString
                        reportcount = reportcount - 2
                        lblReportsSecurity.Text = reportcount.ToString() + " items marked as No Access"
                        If reportcount = 0 Then
                            lblReportsSecurity.Visible = False
                        Else
                            lblReportsSecurity.Visible = True
                        End If

                    End If
                End If
            End If

            If dt.Rows(0)("MarketingCenterCount").ToString = dtAll.Rows(0)("MarketingCenterCount").ToString Then
                lblMarketingCenterSecurity.Text = "User has No Access"
                lblMarketingCenterSecurity.Visible = True
            Else
                If dt.Rows(0)("MarketingCenterCount").ToString = "1" Then
                    lblMarketingCenterSecurity.Text = dt.Rows(0)("MarketingCenterCount").ToString + " item marked as No Access"
                    lblMarketingCenterSecurity.Visible = True
                ElseIf dt.Rows(0)("MarketingCenterCount").ToString = "0" Then
                    lblMarketingCenterSecurity.Visible = False
                Else
                    lblMarketingCenterSecurity.Text = dt.Rows(0)("MarketingCenterCount").ToString + " items marked as No Access"
                    lblMarketingCenterSecurity.Visible = True
                End If
            End If

            If dt.Rows(0)("AdministrationCount").ToString = dtAll.Rows(0)("AdministrationCount").ToString Then
                lblAdministrationSecurity.Text = "User has No Access"
                lblAdministrationSecurity.Visible = True
            Else
                If dt.Rows(0)("AdministrationCount").ToString = "1" Then
                    lblAdministrationSecurity.Text = dt.Rows(0)("AdministrationCount").ToString + " item marked as No Access"
                    lblAdministrationSecurity.Visible = True
                ElseIf dt.Rows(0)("AdministrationCount").ToString = "0" Then
                    lblAdministrationSecurity.Visible = False
                Else
                    lblAdministrationSecurity.Text = dt.Rows(0)("AdministrationCount").ToString + " items marked as No Access"
                    lblAdministrationSecurity.Visible = True
                End If
            End If

            If dt.Rows(0)("DataMaintenanceCount").ToString = dtAll.Rows(0)("DataMaintenanceCount").ToString Then
                lblDataMaintenanceSecurity.Text = "User has No Access"
                lblDataMaintenanceSecurity.Visible = True
            Else
                If dt.Rows(0)("DataMaintenanceCount").ToString = "1" Then
                    lblDataMaintenanceSecurity.Text = dt.Rows(0)("DataMaintenanceCount").ToString + " item marked as No Access"
                    lblDataMaintenanceSecurity.Visible = True
                ElseIf dt.Rows(0)("DataMaintenanceCount").ToString = "0" Then
                    lblDataMaintenanceSecurity.Visible = False
                Else
                    lblDataMaintenanceSecurity.Text = dt.Rows(0)("DataMaintenanceCount").ToString + " items marked as No Access"
                    lblDataMaintenanceSecurity.Visible = True
                End If
            End If

            If dt.Rows(0)("MiscellaneousCount").ToString = dtAll.Rows(0)("MiscellaneousCount").ToString Then
                lblMiscellaneousSecurity.Text = "User has No Access"
                lblMiscellaneousSecurity.Visible = True
            Else
                If dt.Rows(0)("MiscellaneousCount").ToString = "1" Then
                    lblMiscellaneousSecurity.Text = dt.Rows(0)("MiscellaneousCount").ToString + " item marked as No Access"
                    lblMiscellaneousSecurity.Visible = True
                ElseIf dt.Rows(0)("MiscellaneousCount").ToString = "0" Then
                    lblMiscellaneousSecurity.Visible = False
                Else
                    lblMiscellaneousSecurity.Text = dt.Rows(0)("MiscellaneousCount").ToString + " items marked as No Access"
                    lblMiscellaneousSecurity.Visible = True
                End If
            End If
            If Convert.ToBoolean(Session("IsCorpOffice")) = True Then
                lnkRingSalesSecurity.Enabled = False
                lnkCloseThDaySecurity.Enabled = False
                lnkMiscellaneousSecurity.Enabled = False
                lnkMarketingCenterSecurity.Enabled = False
                lnkRingSalesSecurity.ForeColor = Drawing.Color.Gray
                lnkCloseThDaySecurity.ForeColor = Drawing.Color.Gray
                lnkMarketingCenterSecurity.ForeColor = Drawing.Color.Gray
                lnkMiscellaneousSecurity.ForeColor = Drawing.Color.Gray
                lblRingSalesSecurity.Font.Italic = True
                lblCloseThDaySecurity.Font.Italic = True
                lblMarketingCenterSecurity.Font.Italic = True
                lblMiscellaneousSecurity.Font.Italic = True
                lblCloseThDaySecurity.Visible = True
                lblMarketingCenterSecurity.Visible = True
                lblRingSalesSecurity.Text = "Does not apply at Corporate Office "
                lblCloseThDaySecurity.Text = "Does not apply at Corporate Office "
                lblMarketingCenterSecurity.Text = "Does not apply at Corporate Office "
                lblMiscellaneousSecurity.Text = "Does not apply at Corporate Office "
            Else
                lnkRingSalesSecurity.Visible = True
                lnkCloseThDaySecurity.Visible = True
                lnkMiscellaneousSecurity.Visible = True
                lnkMarketingCenterSecurity.Visible = True
            End If
        End If
        UpnlNewSecurity.Update()
    End Sub

    Protected Sub hdnbtnEmployeeSecurity_Click(sender As Object, e As EventArgs) Handles hdnbtnEmployeeSecurity.Click
        Call getEmployeeSecurityCount()
        upSecurity.Update()
    End Sub
    Private Sub GetStoreList()
        Dim objAdmin As New vbAdmin
        Dim ds As New DataSet
        Dim objdal As New conClass
        Dim cmd As New SqlCommand
        Dim da As New SqlDataAdapter
        cmd.Connection = objdal.con1
        'objAdmin.StoreNo = ViewState("storeno").ToString
        'objAdmin.UserStatus = "Admin"
        'objAdmin.SortColumn = ViewState("storelistsortcolumn").ToString
        'objAdmin.SortStatus = ViewState("storelistsortstatus").ToString
        'ds = objAdmin.fnGetCorpOfficeStoresSorting        
        Dim Storeaccess As String = ""
        If Not Session("IsCorpOffice") Is Nothing AndAlso CType(Session("IsCorpOffice"), Boolean) = True Then
            Dim objEmp As New vbEmployee
            Dim dt As DataTable
            objEmp.Storeno = ViewState("storeno").ToString
            objEmp.EmployeeId = Convert.ToInt32(ViewState("employee_id"))
            dt = objEmp.fnGetEmployeeStoreAccessList_NewEmployee()
            If dt.Rows.Count > 0 Then
                If dt.Rows(0)("Storeaccess").ToString <> "" Then
                    If dt.Rows(0)("Storeaccess").ToString.Trim = "allstores" Then
                    Else
                        Storeaccess = dt.Rows(0)("Storeaccess").ToString.Trim
                    End If
                End If
            End If
        End If

        cmd.Parameters.AddWithValue("@storeno", ViewState("storeno").ToString)
        cmd.Parameters.AddWithValue("@UserStatus", "Admin")
        cmd.Parameters.AddWithValue("@sortcolumn", ViewState("storelistsortcolumn").ToString)
        cmd.Parameters.AddWithValue("@sortstatus", ViewState("storelistsortstatus").ToString)
        cmd.Parameters.AddWithValue("@storelist", Storeaccess)
        cmd.Parameters.AddWithValue("@emprole", ViewState("emprole"))
        cmd.CommandText = "proc_get_CorpOfficeStores"
        cmd.CommandType = CommandType.StoredProcedure
        objdal.DBconnect()
        da.SelectCommand = cmd
        da.Fill(ds)
        objdal.DBDisconnect()
        If ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then
            grdStores.DataSource = ds.Tables(0)
            grdStores.DataBind()
            ViewState("StoreData") = ds.Tables(0)
            If Not ViewState("emprole") Is Nothing AndAlso ViewState("emprole") = 2 Then
                Call AllCheckeValues()
            End If
        Else
            grdStores.DataSource = Nothing
            grdStores.DataBind()
            ViewState("StoreData") = Nothing
        End If
        
    End Sub
    Protected Sub grdStores_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdStores.PageIndexChanging
        SaveCheckedValues()
        grdStores.PageIndex = e.NewPageIndex
        GetStoreList()
        If chkMainHeader.Checked = True Then
            ViewState("allstore") = "Y"
        Else
            ViewState("allstore") = "N"
        End If
        If ViewState("allstore").ToString.ToUpper = "Y" Then
            If grdStores.Rows.Count > 0 Then
                For Each gvrow As GridViewRow In grdStores.Rows
                    Dim chksub As CheckBox = CType(gvrow.FindControl("chkSelect"), CheckBox)
                    chksub.Checked = True
                Next
                Dim chkmain As CheckBox = CType(grdStores.HeaderRow.Cells(0).FindControl("chkBxHeader"), CheckBox)
                chkmain.Checked = True
            End If
            SaveCheckedValues()
        Else
            PopulateCheckedValues()
        End If
    End Sub
    Protected Sub grdStores_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdStores.RowDataBound

        If e.Row.RowType = DataControlRowType.Header Then
            For Each cell As TableCell In e.Row.Cells
                If cell.HasControls Then
                    Dim imgName As Image
                    Dim imgStoreno As Image
                    Dim imgCity As Image


                    imgName = CType(e.Row.FindControl("imgName"), Image)
                    imgStoreno = CType(e.Row.FindControl("imgStoreno"), Image)
                    imgCity = CType(e.Row.FindControl("imgCity"), Image)

                    If ViewState("storelistsortcolumn") = "name" Then
                        imgName.Visible = True
                        imgStoreno.Visible = False
                        imgCity.Visible = False

                        If ViewState("storelistsortstatus") = "ASC" Then
                            imgName.ImageUrl = "~/Images/arrowup.gif"
                        Else
                            imgName.ImageUrl = "~/Images/arrowdown.gif"
                        End If
                    End If
                    If ViewState("storelistsortcolumn") = "storeno" Then
                        imgName.Visible = False
                        imgStoreno.Visible = True
                        imgCity.Visible = False

                        If ViewState("storelistsortstatus") = "ASC" Then
                            imgStoreno.ImageUrl = "~/Images/arrowup.gif"
                        Else
                            imgStoreno.ImageUrl = "~/Images/arrowdown.gif"
                        End If
                    End If

                    If ViewState("storelistsortcolumn") = "city_st" Then
                        imgName.Visible = False
                        imgStoreno.Visible = False
                        imgCity.Visible = True


                        If ViewState("storelistsortstatus") = "ASC" Then
                            imgCity.ImageUrl = "~/Images/arrowup.gif"
                        Else
                            imgCity.ImageUrl = "~/Images/arrowdown.gif"
                        End If
                    End If


                End If
            Next

        End If
    End Sub

    Protected Sub grdStores_Sorting(sender As Object, e As GridViewSortEventArgs) Handles grdStores.Sorting
        If ViewState("storelistsortcolumn") = e.SortExpression Then
            ViewState("storelistsortcolumn") = e.SortExpression
            If ViewState("storelistsortstatus") = "ASC" Then
                ViewState("storelistsortstatus") = "DESC"
            Else
                ViewState("storelistsortstatus") = "ASC"
            End If
        Else
            ViewState("storelistsortcolumn") = e.SortExpression
            ViewState("storelistsortstatus") = "ASC"

        End If

        GetStoreList()

    End Sub

    Private Sub SaveCheckedValues()
        Dim userdetails As New ArrayList
        Dim index As String = String.Empty
        If Session("CHECKEDSTORE") Is Nothing Then
            Session("CHECKEDSTORE") = userdetails
        End If

        userdetails = TryCast(Session("CHECKEDSTORE"), ArrayList)
        For Each gvrow As GridViewRow In grdStores.Rows
            index = grdStores.DataKeys(gvrow.RowIndex).Value.ToString().Trim
            Dim result As Boolean = CType(gvrow.FindControl("chkSelect"), CheckBox).Checked
            If Not IsDBNull(Session("CHECKEDSTORE")) Then
                If result = True Then
                    If Not userdetails.Contains(index.Trim) Then
                        userdetails.Add(index.Trim)
                    End If
                Else
                    userdetails.Remove(index)
                    ViewState("allstore") = "N"
                End If
                If userdetails.Count > 0 Then
                    Session("CHECKEDSTORE") = userdetails
                End If
            End If
        Next
    End Sub
    Private Sub PopulateCheckedValues()
        Dim userdetails As New ArrayList
        userdetails = Session("CHECKEDSTORE")

        Dim index As String
        If userdetails.Count > 0 Then
            For Each gvrow As GridViewRow In grdStores.Rows
                index = grdStores.DataKeys(gvrow.RowIndex).Value
                If userdetails.Contains(index.Trim) Then
                    Dim chksub As CheckBox = CType(gvrow.FindControl("chkSelect"), CheckBox)
                    chksub.Checked = True
                End If
            Next
        End If
    End Sub
    Private Sub Getstoreaccesslist()
        Dim objEmp As New vbEmployee
        Dim dt As DataTable
        objEmp.Storeno = ViewState("storeno").ToString
        objEmp.EmployeeId = Convert.ToInt32(ViewState("employee_id"))
        dt = objEmp.fnGetEmployeeStoreAccess()
        If dt.Rows.Count > 0 Then
            If dt.Rows(0)("Storeaccess").ToString() <> "" Then
                If dt.Rows(0)("Storeaccess").ToString.Trim = "allstores" Then
                    chkMainHeader.Checked = True
                    Dim chkmain As CheckBox = CType(grdStores.HeaderRow.Cells(0).FindControl("chkBxHeader"), CheckBox)
                    chkmain.Checked = True
                    For Each gvrow As GridViewRow In grdStores.Rows
                        Dim chkbox As CheckBox = CType(gvrow.FindControl("chkSelect"), CheckBox)
                        chkbox.Checked = True
                    Next
                    ViewState("allstore") = "Y"
                Else
                    Dim oldstore As String = dt.Rows(0)("Storeaccess").ToString.Trim
                    Dim userdetails As New ArrayList(oldstore.Split(","))
                    Dim userdetails1 As New ArrayList(userdetails.ToArray())
                    Session("CHECKEDSTORE") = userdetails1
                    PopulateCheckedValues()
                    ViewState("allstore") = "N"
                End If
            Else

            End If

        End If
    End Sub
    Private Sub Savestorelist()
        Dim Result As Integer = 0
        Dim returnval As String = ""
        If chkMainHeader.Checked = True Then
            Session("CHECKEDSTORE") = "allstores"
            Dim objEmp As New vbEmployee
            objEmp.Storeno = ViewState("storeno").ToString
            objEmp.EmployeeId = Convert.ToInt32(ViewState("employee_id"))
            objEmp.Storelist = "allstores"
            Result = objEmp.fnAddEditEmployeeStoreAccess()
            ViewState("allstore") = "Y"
        Else
            SaveCheckedValues()
            If Not Session("CHECKEDSTORE") Is Nothing Then
                Dim userdetails As ArrayList = Session("CHECKEDSTORE")

                If userdetails.Count > 0 Then
                    For Each i As String In userdetails
                        returnval += i & ","
                    Next
                End If
                If Not returnval = "" Then
                    returnval = returnval.Remove(returnval.Length - 1, 1)

                End If
                Dim objEmp As New vbEmployee
                objEmp.Storeno = ViewState("storeno").ToString
                objEmp.EmployeeId = Convert.ToInt32(ViewState("employee_id"))
                objEmp.Storelist = returnval.ToString.Trim
                ViewState("allstore") = "N"
                StoreAccessSecurity()
                Result = objEmp.fnAddEditEmployeeStoreAccess()

            End If
        End If
    End Sub
    Public Sub StoreAccessSecurity()
        Dim ds As New DataSet
        Dim objdal As New conClass
        Dim cmd As New SqlCommand
        Dim da As New SqlDataAdapter
        cmd.Connection = objdal.con1
        Dim storelistsecurity As String = ""
        If ViewState("allstore").ToString.ToUpper = "Y" Then
            storelistsecurity = "All Stores"
        Else
            If Not Session("CHECKEDSTORE") Is Nothing Then
                Dim userdetails As ArrayList = Session("CHECKEDSTORE")

                If userdetails.Count > 0 Then
                    For Each i As String In userdetails
                        storelistsecurity += i & ","
                    Next
                End If
                If Not storelistsecurity = "" Then
                    storelistsecurity = storelistsecurity.Remove(storelistsecurity.Length - 1, 1)
                End If
            End If
        End If
        cmd.Parameters.AddWithValue("@mode", "Add")
        cmd.Parameters.AddWithValue("@employeeID", ViewState("employee_id"))
        cmd.Parameters.AddWithValue("@storelist", storelistsecurity)
        cmd.Parameters.AddWithValue("@storeno", Session("storeno"))
        cmd.Parameters.AddWithValue("@station", Session("stationno"))
        cmd.Parameters.AddWithValue("@user", Session("emp_id"))
        cmd.CommandText = "proc_AddEdit_Storelist_Security"
        cmd.CommandType = CommandType.StoredProcedure
        objdal.DBconnect()
        cmd.ExecuteNonQuery()
        objdal.DBDisconnect()
    End Sub
    Private Sub getEmployeeRoles()
        Dim dtEmpRoles As New DataTable
        Dim objEmp As New vbEmployee
        objEmp.Storeno = Session("storeno").ToString()
        dtEmpRoles = objEmp.fnGetEmployeeRoles()
        drpEmproles.DataSource = dtEmpRoles
        drpEmproles.DataTextField = "role"
        drpEmproles.DataValueField = "role_id"
        drpEmproles.DataBind()
    End Sub
    Public Sub getAreaRepresentative()
        Dim ds As New DataSet
        Dim objSetup As New vbStationSetup
        objSetup.StoreNo = Session("storeno").ToString().Trim
        objSetup.CurrentPage = 0
        objSetup.NoofDisplayRow = 0
        objSetup.Mode = 1
        ds = objSetup.fnGetAreaRepresenative()
        If Not ds Is Nothing AndAlso ds.Tables(0).Rows.Count > 0 Then
            drpAreaLocation.DataSource = ds.Tables(0)
            drpAreaLocation.DataTextField = "AreaLocation"
            drpAreaLocation.DataValueField = "AreaRepID"
            drpAreaLocation.DataBind()
        Else
            drpAreaLocation.DataSource = Nothing
            drpAreaLocation.DataBind()
        End If
        drpAreaLocation.Items.Insert(0, New ListItem("--Not Defined--", 0))
    End Sub

    Protected Sub drpEmproles_SelectedIndexChanged(sender As Object, e As EventArgs) Handles drpEmproles.SelectedIndexChanged
        Dim index As Integer = drpEmproles.SelectedValue
        If index = 2 Then
            lblAreaRepresentative.Visible = True
            drpAreaLocation.Visible = True
            getAreaRepresentative()
        Else
            lblAreaRepresentative.Visible = False
            drpAreaLocation.Visible = False
        End If
    End Sub
    Private Sub AllCheckeValues()
        Dim index As String
        chkMainHeader.Checked = True
        For Each gvrow As GridViewRow In grdStores.Rows
            index = CType(grdStores.DataKeys(gvrow.RowIndex).Value, String)
            Dim chksub As CheckBox = CType(gvrow.FindControl("chkSelect"), CheckBox)
            chksub.Checked = True
        Next
    End Sub

    Protected Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        If Not IsNothing(ViewState("employee_id")) Then
            Dim objEmployee As New vbEmployee()

            objEmployee.EmployeeId = IIf(ViewState("employee_id") Is Nothing, 0, ViewState("employee_id").ToString())
            objEmployee.fndeleteEmployee()
            ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "deleteEmp", "window.parent.ClosePopup();", True)
        Else
            ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "deleteEmp", "window.parent.ClosePopup();", True)
        End If
    End Sub

    Public Sub keySecurity()
        Dim dt As New DataTable
        Dim obj As New vbEmployee()
        dt = obj.fnSelectAll_Event()

        If Convert.ToBoolean(Session("IsCorpOffice")) = True Then
            Dim i As Integer = 0
            Dim cnt As Integer = 0
            For i = 0 To dt.Rows.Count - 1
                If dt.Rows(i)("event_name").ToString() = "Return" Then
                    dt.Rows(i).Delete()
                    dt.AcceptChanges()
                    cnt = cnt + 1
                End If
                If dt.Rows(i)("event_name").ToString() = "Void" Then
                    dt.Rows(i).Delete()
                    dt.AcceptChanges()
                    cnt = cnt + 1
                End If
                If dt.Rows(i)("event_name").ToString() = "Close the Day " Then
                    dt.Rows(i).Delete()
                    dt.AcceptChanges()
                    cnt = cnt + 1
                End If
                If dt.Rows(i)("event_name").ToString() = "Include Employee Summary" Then
                    dt.Rows(i).Delete()
                    dt.AcceptChanges()
                    cnt = cnt + 1
                End If
                If cnt = 4 Then
                    Exit For
                End If
            Next
        End If
        gvevent.DataSource = dt
        gvevent.DataBind()

        Dim objlt As New Literal()
        objlt.Text = "<b>KEY SECURITY FEATURE:</b><br/><br/>"
        If Not Session("IsCorpOffice") Is Nothing AndAlso CType(Session("IsCorpOffice"), Boolean) = False Then
            objlt.Text += "Lightning Online Point of Sale can automatically send an email to the System Administrator when a key event takes place.<br/><br/>"
            objlt.Text += "The following allows you to control if this new employee will trigger an alert to the System Administrator.<br/><br/>"
            
        Else
            objlt.Text += "Lightning Corporate Office can automatically send an email to the System Administrator when a certain key event takes place.<br/><br/>"
            objlt.Text += "The following allows you to control if this new employee will trigger an alert to the System Administrator.<br/><br/>"
           
        End If
        divkeySecurity.InnerHtml = objlt.Text
       
    End Sub
    Protected Sub gvevent_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvevent.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim obj As New vbEmployee()
            If ViewState("employee_id") <> Nothing And ViewState("employee_id") <> "" Then
                obj.empeve_empid = ViewState("employee_id")
            Else
                obj.empeve_empid = 0
            End If
            Dim eventid As Integer = Convert.ToInt32(CType(e.Row.FindControl("lblevent_id"), Label).Text.Trim())
            obj.empeve_eventid = eventid
            'Dim i As Integer
            'i = obj.fnemployee_slect_Eventbyempid()
            'If i <> 0 Then
            CType(e.Row.FindControl("chkSelect"), CheckBox).Checked = True
            'End If
            If CType(e.Row.FindControl("lblEvent_name"), Label).Text.Trim().ToLower() = "close the day" Then
                CType(e.Row.FindControl("chkEmpSummary"), CheckBox).Visible = True
                CType(e.Row.FindControl("lblEmpSummaryEvent_name"), Label).Visible = True
                CType(e.Row.FindControl("chkEmpSummary"), CheckBox).Enabled = False
                CType(e.Row.FindControl("lblEmpSummaryEvent_name"), Label).Enabled = True
            End If

            If CType(e.Row.FindControl("lblEvent_name"), Label).Text.Trim().ToLower() = "close the day" Then
                CType(e.Row.FindControl("lblclosetheday"), Label).Text = "<br/>" & "(Sales figures, total customers, average invoice, etc, will be emailed to the administrator)"
            Else
                CType(e.Row.FindControl("lblclosetheday"), Label).Text = ""
            End If


            'CType(e.Row.FindControl("chkSelect"), CheckBox).Enabled = False
            'CType(e.Row.FindControl("lblEvent_name"), Label).Enabled = True

            If CType(e.Row.FindControl("lblEvent_name"), Label).Text.Trim().ToLower() = "include employee summary" Then
                Dim count As Integer = 0
                For count = 0 To gvevent.Rows.Count - 1
                    If CType(gvevent.Rows(count).FindControl("lblEvent_name"), Label).Text.Trim().ToLower() = "close the day" Then
                        CType(gvevent.Rows(count).FindControl("chkEmpSummary"), CheckBox).Visible = True
                        CType(gvevent.Rows(count).FindControl("lblEmpSummaryEvent_name"), Label).Visible = True
                        CType(gvevent.Rows(count).FindControl("chkEmpSummary"), CheckBox).Enabled = True
                        CType(gvevent.Rows(count).FindControl("lblEmpSummaryEvent_name"), Label).Enabled = True
                        CType(gvevent.Rows(count).FindControl("lblEmpSummaryEvent_name"), Label).Text = CType(e.Row.FindControl("lblEvent_name"), Label).Text
                        CType(gvevent.Rows(count).FindControl("lblEmpSummaryevent_id"), Label).Text = CType(e.Row.FindControl("lblevent_id"), Label).Text.Trim()
                        obj.empeve_eventid = Convert.ToInt32(CType(e.Row.FindControl("lblevent_id"), Label).Text.Trim())
                        'Dim j As Integer
                        'j = obj.fnemployee_slect_Eventbyempid()
                        'If j <> 0 Then
                        CType(gvevent.Rows(count).FindControl("chkEmpSummary"), CheckBox).Checked = True
                        'Else
                        '    CType(gvevent.Rows(count).FindControl("chkEmpSummary"), CheckBox).Checked = False
                        'End If
                    End If
                Next
                e.Row.Visible = False
            End If

            'If Convert.ToBoolean(Session("IsCorpOffice")) = True Then
            '    If e.Row.RowIndex = 0 Or e.Row.RowIndex = 1 Or e.Row.RowIndex = 2 Then
            '        CType(e.Row.FindControl("chkSelect"), CheckBox).Checked = False
            '        CType(e.Row.FindControl("chkSelect"), CheckBox).Enabled = False
            '    End If
            'End If
        End If

    End Sub

    Public Sub Save_Email_Alert()
        Dim obj As New vbEmployee()        

        Dim i As Integer = 0

        ''--------Delete Event information for employee_event table
        'If ViewState("employee_id") <> Nothing And ViewState("employee_id") <> "" Then
        '    obj.empeve_empid = ViewState("employee_id")
        'Else
        '    obj.empeve_empid = 0
        'End If
        'obj.fnemployee_EventDeletedbyempid()


        For i = 0 To gvevent.Rows.Count - 1
            'If CType(gvevent.Rows(i).FindControl("chkSelect"), CheckBox).Checked = True Then

            '---------insert event into table
            If ViewState("employee_id") <> Nothing And ViewState("employee_id") <> "" Then
                obj.empeve_empid = ViewState("employee_id")
            Else
                obj.empeve_empid = 0
            End If
            obj.empeve_eventid = Convert.ToInt32(CType(gvevent.Rows(i).FindControl("lblevent_id"), Label).Text.Trim())
            If CType(gvevent.Rows(i).FindControl("lblEvent_name"), Label).Text.Trim().ToLower() <> "include employee summary" Then
                If CType(gvevent.Rows(i).FindControl("chkSelect"), CheckBox).Checked = True Then
                    obj.empeve_status = "1"
                Else
                    obj.empeve_status = "0"
                End If
                obj.Storeno = Session("storeno")
                obj.Stationno = Session("stationno")
                obj.user = Session("emp_id")
                obj.fnemployee_Event_Wizard()
            End If
            If CType(gvevent.Rows(i).FindControl("lblEvent_name"), Label).Text.Trim().ToLower() = "close the day" Then
                If obj.empeve_status = "1" Then
                    If CType(gvevent.Rows(i).FindControl("chkEmpSummary"), CheckBox).Checked = True Then
                        obj.empeve_status = "1"
                    Else
                        obj.empeve_status = "0"
                    End If
                Else
                    obj.empeve_status = "0"
                End If
                obj.empeve_eventid = Convert.ToInt32(CType(gvevent.Rows(i).FindControl("lblEmpSummaryevent_id"), Label).Text.Trim())
                obj.fnemployee_Event_Wizard()
            End If
            ViewState("Checkedsaved") = True
            'End If
        Next

    End Sub
End Class
