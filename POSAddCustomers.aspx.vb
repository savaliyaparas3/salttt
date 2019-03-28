Imports System.Data
Imports System.Data.SqlClient
Imports System.Text
Imports System.Text.Decoder

Partial Class POSAddCustomers
    Inherits System.Web.UI.Page
    Dim dataset As DataSet = New DataSet
    Dim strtemp As String
    Dim strinput As String = ""
    Dim strcurrentchar As String

    
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSave.Click
        Call savecustomerdetails()
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ViewState("storeno") = Session("storeno")
        ViewState("stationno") = Session("stationno")
        ViewState("country") = Session("country")
        'btnSave.Attributes.Add("onclick", "javascript:return verify();")
        txtZip.Attributes.Add("onblur", "javascript:getCitystate();")
     
        lblcustype.Visible = True
        If Session("emp_id") Is Nothing Then
            Session.Abandon()
            Response.Redirect("POSLogin.aspx")
        End If

        MultiView1.ActiveViewIndex = 0
        'txtLockNotes.Visible = False
        'Call getCustypevalues()
        txtlastname.Focus()
        'GetRequirefields()
        lblLastPurchase.ForeColor = Drawing.Color.Black
        lblLastPurchaseTitle.ForeColor = Drawing.Color.Black
        If Not Page.IsPostBack Then
            btnAdd.Visible = False
            btnDelete.Visible = False
            imgexit.Visible = False
            imagebutton2.Visible = False
            ViewState("emp_id") = Session("emp_id")
            BindYear()

            ddlMonth.SelectedValue = Month(Date.Today)
            ddlYear.SelectedValue = Year(Date.Today)
            If Request.QueryString("id") Is Nothing Then
                ViewState("id") = 0
                lnkView.Visible = False
            Else
                ViewState("id") = Request.QueryString("id")
                lnkView.Visible = True
            End If

            Dim intNotes As Integer
            If Request.QueryString("type") = "view" Then
                'lock_controls()
                'lblheader.Text = "Customer Details"

                ViewState("type") = "view"
                Call getCustypevalues()
                Call getcustomerdetails()
                GetAccessRights()
                If ViewState("NotesDetail") IsNot Nothing Then
                    intNotes = ViewState("NotesDetail").ToString().Length()
                    If intNotes < 56 Then
                        lblNotesDetail.Text = (ViewState("NotesDetail").ToString())
                        lnkAddNotes.Text = "View Notes"
                    Else
                        lblNotesDetail.Text = (ViewState("NotesDetail").ToString().Substring(0, 55))
                        lnkAddNotes.Text = "More..."
                    End If
                    lblNotesDetail.Visible = True
                    'txtNote.Enabled = False
                    'lblhdrAdd.Text = "View Notes"
                    'If intNotes = 0 Then
                    '    lnkAddNotes.Visible = False
                    '    lblNotesDetail.Text = "-"
                    'Else
                    '    lnkAddNotes.Visible = True
                    'End If
                End If
                Call lock_controls()
            ElseIf Request.QueryString("type") = "edit" Then
                'lblheader.Text = "Edit Customer Details"
                ViewState("type") = "edit"
                Call getCustypevalues()
                Call getcustomerdetails()

                GetAccessRights()
                If ViewState("NotesDetail") IsNot Nothing Then
                    intNotes = ViewState("NotesDetail").ToString().Length()
                    If intNotes < 56 Then
                        lblNotesDetail.Text = (ViewState("NotesDetail").ToString())
                        'lnkAddNotes.Text = "Edit Notes"
                    Else
                        lblNotesDetail.Text = (ViewState("NotesDetail").ToString().Substring(0, 55))
                        lnkAddNotes.Text = "More..."
                    End If
                    lblNotesDetail.Visible = True
                    'txtNote.Enabled = True
                    'lblhdrAdd.Text = "Edit Notes"
                    'If intNotes = 0 Then
                    '    lnkAddNotes.Visible = True
                    '    lblNotesDetail.Text = "-"
                    '    lnkAddNotes.Text = "Add Notes"
                    '    lblhdrAdd.Text = "Add Notes"
                    '    txtNote.Enabled = True
                    'End If
                    Call unlockcontrols()
                End If
            Else
                ViewState("type") = "add"
                Call getCustypevalues()
                Call getcustomerdetails()
                ddlPhone2.SelectedValue = "W"
                ddlPhone3.SelectedValue = "M"
            End If

            'btnyes.Visible = False
            txtLockNotes.Visible = False
            Label22.Visible = False
            ''' for loyalty card
            Dim strcheck = getActivateCardno()
            If strcheck.Trim = "Y" Then
                lblLoyalty.Visible = True
                txtLoyaltyCard.Visible = True
            End If
        End If
        txtCreditCard.Attributes.Add("onblur", "javascript:return CreditVisaMastercardNo(this,'" & lblCardType.ClientID & "');")
        pnlCard.Attributes.Add("onkeypress", "javascript:return MpePOPUPHide(event,'" & mpeCard.ID & "','" & txtcard.ClientID & "');")
        pnlWarning.Attributes.Add("onkeypress", "javascript:return MpePOPUPHide(event,'" & mpeWarning.ID & "','');")
        pnlLogincredit.Attributes.Add("onkeypress", "javascript:return MpePOPUPHide(event,'" & mpeLogincredit.ID & "','');")
        pnlCreditPassWord.Attributes.Add("onkeypress", "javascript:return MpePOPUPHide(event,'" & mpeCreditPassWord.ID & "','');")

        txtCreditPassWord.Attributes.Add("onKeyUp", "javaScript:return Count_PasswordLenght(this,'lblErrMsgCount',10);")
        repassword.Attributes.Add("onKeyUp", "javaScript:return Count_PasswordLenght(this,'lblErrMsgCount',10);")

    End Sub

    Sub BindYear()
        ddlYear.Items.Insert(0, Year(Date.Today))
        ddlYear.Items.Insert(1, Year(Date.Today) + 1)
        ddlYear.Items.Insert(2, Year(Date.Today) + 2)
        ddlYear.Items.Insert(3, Year(Date.Today) + 3)
        ddlYear.Items.Insert(4, Year(Date.Today) + 4)
        ddlYear.Items.Insert(5, Year(Date.Today) + 5)
        ddlYear.Items.Insert(6, Year(Date.Today) + 6)
        ddlYear.DataBind()
    End Sub
    Sub GetAccessRights()
        Dim objEmployee As New vbEmployee
        objEmployee.Storeno = ViewState("storeno")
        objEmployee.Employee_Id = ViewState("emp_id")
        objEmployee.fnGetRWPassOfEmployee()
        If objEmployee.CC Then
            ViewState("Expdate") = objEmployee.ExpDate
            lnkView.Visible = True
        Else
            lnkView.Visible = False
        End If
    End Sub
    '' for customer fetching details
    'Public Sub customerdetails()

    '    Dim objconnection As conClass = New conClass
    '    objconnection.DBconnect()
    '    '' create sqlcommand
    '    Dim objcommand As SqlCommand = New SqlCommand
    '    objcommand.Connection = objconnection.con1
    '    objcommand.CommandText = ""
    '    objcommand.CommandType = CommandType.StoredProcedure
    '    objcommand.Parameters.AddWithValue("@", "")

    '    Dim objdataadapter As SqlDataAdapter = New SqlDataAdapter
    '    objdataadapter.SelectCommand = objcommand

    '    Dim objdataset As New DataSet

    '    Try
    '        objdataadapter.Fill(objdataset, "")
    '        If objdataset.Tables(0).Rows.Count > 0 Then

    '        End If
    '    Catch ex As Exception

    '    End Try

    'End Sub
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
    Public Sub unlockcontrols()
        If ViewState("type") = "add" Then
            txtAddress.Text = ""
            txtAddress1.Text = ""
            txtCity.Text = ""
            txtCompanyName.Text = ""
            txtCreditCard.Text = ""
            txtEmail.Text = ""
            lblCard.Visible = False
            txtcard.Visible = False
            txtCreditCard.Text = ""
            txtFirstName.Text = ""
            ddlCusType.SelectedIndex = 0
            'txtLastPurchase.Enabled = True
            txtlastname.Text = ""
            txtLockNotes.Text = ""
            'txtMiddleName.Enabled = True
            txtPhone.Text = ""
            txtState.Text = ""
            txtTax.Text = ""
            'txtTotalPurchase.Enabled = True
            txtZip.Text = ""
            ddlLock.SelectedIndex = 1
            btnAdd.Visible = False
            btnDelete.Visible = False
            imgexit.Visible = False
            imagebutton2.Visible = False
            btnSave.Visible = True
            btnCancel1.Visible = True
            txtLoyaltyCard.Text = ""
            ddlpricelevel.SelectedIndex = 0
            txtPhone2.Text = ""
            txtPhone3.Text = ""
            ddlYear.SelectedIndex = 0
            ddlMonth.SelectedIndex = 0
            If txtCreditCard.Text.Trim <> "" Then
                ddlYear.Enabled = True
                ddlMonth.Enabled = True
            End If
            txtEmail2.Text = ""
            ddlPhone2.SelectedValue = "W"
            ddlPhone3.SelectedValue = "M"
            'btnCallastpur.Enabled = True
            lblCardType.Width = 0
            btnDelete.Visible = False
            'txtExpiry.Width = 116
            'txtLastPurchase.Width = 117
            txtAddress.Enabled = True
            txtAddress1.Enabled = True
            txtCity.Enabled = True
            txtCompanyName.Enabled = True
            txtCreditCard.Enabled = True
            txtEmail.Enabled = True
            lblCard.Visible = False
            txtcard.Visible = False
            txtCreditCard.Visible = True
            txtFirstName.Enabled = True
            ddlCusType.Enabled = True
            'txtLastPurchase.Enabled = True
            txtlastname.Enabled = True
            txtLockNotes.Enabled = True
            'txtMiddleName.Enabled = True
            txtPhone.Enabled = True
            txtState.Enabled = True
            txtTax.Enabled = True
            'txtTotalPurchase.Enabled = True
            txtZip.Enabled = True
            ddlLock.Enabled = True
            
            txtLoyaltyCard.Enabled = True
            ddlpricelevel.Enabled = True
            txtPhone2.Enabled = True
            txtPhone3.Enabled = True
         
            txtEmail2.Enabled = True
            ddlPhone1.Enabled = True
            ddlPhone2.Enabled = True
            ddlPhone3.Enabled = True
            'btnCallastpur.Enabled = True

            btnDelete.Visible = False
            'txtExpiry.Width = 116
            'txtLastPurchase.Width = 117
            
        Else
            txtAddress.Enabled = True
            txtAddress1.Enabled = True
            txtCity.Enabled = True
            txtCompanyName.Enabled = True
            txtCreditCard.Enabled = True
            txtEmail.Enabled = True
            lblCard.Visible = False
            txtcard.Visible = False
            txtCreditCard.Visible = True
            txtFirstName.Enabled = True
            ddlCusType.Enabled = True
            'txtLastPurchase.Enabled = True
            txtlastname.Enabled = True
            txtLockNotes.Enabled = True
            'txtMiddleName.Enabled = True
            txtPhone.Enabled = True
            txtState.Enabled = True
            txtTax.Enabled = True
            'txtTotalPurchase.Enabled = True
            txtZip.Enabled = True
            ddlLock.Enabled = True
            btnSave.Enabled = True
            btnCancel1.Enabled = True
            txtLoyaltyCard.Enabled = True
            ddlpricelevel.Enabled = True
            txtPhone2.Enabled = True
            txtPhone3.Enabled = True
            If txtCreditCard.Text.Trim <> "" Then
                ddlYear.Enabled = True
                ddlMonth.Enabled = True
            End If
            txtEmail2.Enabled = True
            ddlPhone1.Enabled = True
            ddlPhone2.Enabled = True
            ddlPhone3.Enabled = True
            'btnCallastpur.Enabled = True

            btnDelete.Visible = False
            'txtExpiry.Width = 116
            'txtLastPurchase.Width = 117
            btnAdd.Visible = False
            imgexit.Visible = False
            txtNote.Enabled = True
            'imagebutton2.ImageUrl = "~/icon small/save.gif"
        End If
        If ViewState("NotesDetail").ToString().Length() = 0 Then
            lnkAddNotes.Visible = True
            lblNotesDetail.Text = "-"
            lnkAddNotes.Text = "Add Notes"
            lblhdrAdd.Text = "Add Notes"

        Else
            lblhdrAdd.Text = "Edit Notes"
        End If
       
    End Sub
    Public Sub lock_controls()
        txtAddress.Enabled = False
        txtAddress1.Enabled = False
        txtCity.Enabled = False
        txtCompanyName.Enabled = False
        txtCreditCard.Enabled = False
        txtEmail.Enabled = False

        txtFirstName.Enabled = False
        'txtLastPurchase.Enabled = False
        txtlastname.Enabled = False
        lblCard.Visible = True
        txtcard.Visible = False
        txtCreditCard.Visible = False
        txtLockNotes.Enabled = False
        'txtMiddleName.Enabled = False
        txtPhone.Enabled = False
        txtState.Enabled = False
        txtTax.Enabled = False
        ' txtTotalPurchase.Enabled = False
        txtZip.Enabled = False
        ddlLock.Enabled = False
        ddlCusType.Enabled = False
        btnSave.Visible = False
        btnCancel1.Visible = False
        imagebutton2.Visible = True
        imgexit.Visible = True
        ' btnCallastpur.Enabled = False
        txtLoyaltyCard.Enabled = False
        ddlpricelevel.Enabled = False
        'txtExpiry.Width = 150
        'txtLastPurchase.Width = 150
        btnAdd.Visible = True
        btnDelete.Visible = True
        txtPhone2.Enabled = False
        txtPhone3.Enabled = False
        ddlYear.Enabled = False
        ddlMonth.Enabled = False
        txtEmail2.Enabled = False
        ddlPhone1.Enabled = False
        ddlPhone2.Enabled = False
        ddlPhone3.Enabled = False

        btnAdd.Visible = True
        imgexit.Visible = True
        txtNote.Enabled = False
        lblhdrAdd.Text = "View Notes"
        If ViewState("NotesDetail").ToString().Length() = 0 Then
            lnkAddNotes.Visible = False
            lblNotesDetail.Text = "-"
        Else
            lnkAddNotes.Visible = True
        End If
    End Sub
    Public Sub savecustomerdetails()
        '''For Creating Three tire architecture of customer

        Dim objCustomer As New vbCustomer()
        objCustomer.CustomerId = ViewState("id")
        objCustomer.Fname = txtFirstName.Text.Trim
        objCustomer.Lname = txtlastname.Text.Trim
        objCustomer.Company = txtCompanyName.Text.Trim
        objCustomer.Address1 = txtAddress.Text.Trim
        objCustomer.Address2 = txtAddress1.Text.Trim
        objCustomer.City = txtCity.Text.Trim
        objCustomer.State = txtState.Text.Trim
        objCustomer.Zip = txtZip.Text.Trim
        objCustomer.Storeno = ViewState("storeno").ToString
        objCustomer.Phone = txtPhone.Text
        objCustomer.Ph_type1 = ddlPhone1.SelectedValue
        objCustomer.Phone2 = txtPhone2.Text
        objCustomer.Ph_type2 = ddlPhone2.SelectedValue
        objCustomer.Phone3 = txtPhone3.Text
        objCustomer.Ph_type3 = ddlPhone3.SelectedValue
        objCustomer.Internet2 = txtEmail2.Text

        If ViewState("id") = "0" Or txtCreditCard.Visible Then
            If txtCreditCard.Text.Contains("X") Then
                Dim strcardno As String = txtcard.Text
                Dim objEncode As New vbCustomer
                objCustomer.CC_Number = objEncode.base64Encode(strcardno)
            Else
                Dim strcardno As String = txtCreditCard.Text
                Dim objEncode As New vbCustomer
                objCustomer.CC_Number = objEncode.base64Encode(strcardno)
            End If
        Else
            If txtCreditCard.Text.Contains("X") Then
                Dim strcardno As String = txtcard.Text
                Dim objEncode As New vbCustomer
                objCustomer.CC_Number = objEncode.base64Encode(strcardno)
            Else
                Dim strcardno As String = txtCreditCard.Text
                Dim objEncode As New vbCustomer
                objCustomer.CC_Number = objEncode.base64Encode(strcardno)
            End If
        End If

        objCustomer.Email = txtEmail.Text.Trim
        objCustomer.TotalPurchase = ""
        objCustomer.CC_ExpiryDate = ddlMonth.SelectedValue & "/" & Date.DaysInMonth(ddlYear.SelectedValue, ddlMonth.SelectedValue) & "/" & ddlYear.SelectedItem.Text
        objCustomer.LastPurchaseDate = ""
        objCustomer.LockNotes = txtLockNotes.Text.Trim
        objCustomer.LockStatus = ddlLock.SelectedValue
        objCustomer.LoyaltyCard = txtLoyaltyCard.Text.Trim
        objCustomer.PriceLevel = ddlpricelevel.SelectedItem.Text
        objCustomer.TaxExempt = txtTax.Text.Trim
        If ddlCusType.SelectedValue = "" Then
            objCustomer.CustTypeID = 0
        Else
            objCustomer.CustTypeID = ddlCusType.SelectedValue
        End If
        objCustomer.Notes = txtNote.Text.ToString().Trim
        Dim strReturn As String = objCustomer.fnInsertCustomer()

        If strReturn.ToString = "" Then
            strReturn = ViewState("id")

        End If
        Response.Redirect("POSCustomers.aspx?ID=" & strReturn)

    End Sub


    Protected Sub ddlLock_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlLock.SelectedIndexChanged
        If ddlLock.SelectedValue = "Y" Then
            'btnyes.Visible = True
            txtLockNotes.Visible = True
            Label22.Visible = True
            ' txtLockNotes.Visible = True
        End If
        If ddlLock.SelectedValue = "N" Then

            'btnyes.Visible = False
            txtLockNotes.Visible = False
            Label22.Visible = False
        End If
    End Sub
    ''' <summary>
    ''' 'for get active check card display
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function getActivateCardno() As String
        Dim objconnection As conClass = New conClass
        'Dim id As String = Request.QueryString("id")
        objconnection.DBconnect()
        '' SQL Command
        Dim objcommand As SqlCommand = New SqlCommand
        objcommand.Connection = objconnection.con1
        objcommand.CommandText = "proc_get_LoyaltyCheck"
        objcommand.CommandType = CommandType.StoredProcedure
        objcommand.Parameters.AddWithValue("@Storeno", Session("storeno").ToString)

        'Try
        Return objcommand.ExecuteScalar().ToString

    End Function
    Protected Sub btnCancel1_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnCancel1.Click
        Response.Redirect("POSCustomers.aspx")
        'Call getcustomerdetails()

    End Sub

    Public Sub getcustomerdetails()
        Dim objconnection As conClass = New conClass
        'Dim id As String = Request.QueryString("id")
        objconnection.DBconnect()
        '' SQL Command
        Dim objcommand As SqlCommand = New SqlCommand
        objcommand.Connection = objconnection.con1
        objcommand.CommandText = "proc_get_customerlist_info"
        objcommand.CommandType = CommandType.StoredProcedure
        objcommand.Parameters.AddWithValue("@custid", ViewState("id").ToString.Trim)

        Dim objadapter As SqlDataAdapter = New SqlDataAdapter
        objadapter.SelectCommand = objcommand

        Dim objdataset As DataSet = New DataSet()

        'Try
        Call getCustypevalues()
        objadapter.Fill(objdataset, "customer_mst")
        objconnection.DBDisconnect()
        If objdataset.Tables(0).Rows.Count > 0 Then
            txtAddress.Text = objdataset.Tables(0).Rows(0).Item("Address1").ToString.Trim
            txtAddress1.Text = objdataset.Tables(0).Rows(0).Item("Address2").ToString.Trim
            txtEmail.Text = objdataset.Tables(0).Rows(0).Item("email").ToString().Trim
            txtCity.Text = objdataset.Tables(0).Rows(0).Item("city").ToString().Trim()
            txtCompanyName.Text = objdataset.Tables(0).Rows(0).Item("company_name").ToString().Trim
            'txtExpiry.Text = objdataset.Tables(0).Rows(0).Item("cc_exp_dt").ToString().Trim
            If (objdataset.Tables(0).Rows(0).Item("cc_exp_dt").ToString() <> "") Then
                
                ddlMonth.SelectedValue = objdataset.Tables(0).Rows(0).Item("cc_exp_dt").ToString().Trim.Split("/")(0)
                ddlYear.SelectedIndex = ddlYear.Items.IndexOf(ddlYear.Items.FindByText(objdataset.Tables(0).Rows(0).Item("cc_exp_dt").ToString().Trim.Split("/")(2)))
                If CDate(CInt(ddlMonth.SelectedValue) & "/" & objdataset.Tables(0).Rows(0).Item("cc_exp_dt").ToString().Trim.Split("/")(1) & "/" & CInt(ddlYear.SelectedValue)) < Today.Date Then
                    Label12.ForeColor = Drawing.Color.Red
                    txtCreditCard.ForeColor = Drawing.Color.Red
                    Label11.ForeColor = Drawing.Color.Red
                End If
            End If
            txtLoyaltyCard.Text = objdataset.Tables(0).Rows(0).Item("loyalitycard").ToString.Trim
            txtPhone2.Text = objdataset.Tables(0).Rows(0).Item("phone2").ToString.Trim
            txtPhone3.Text = objdataset.Tables(0).Rows(0).Item("phone3").ToString.Trim
            ddlPhone1.SelectedValue = objdataset.Tables(0).Rows(0).Item("ph1_type").ToString.Trim
            ddlPhone2.SelectedValue = objdataset.Tables(0).Rows(0).Item("ph2_type").ToString.Trim
            ddlPhone3.SelectedValue = objdataset.Tables(0).Rows(0).Item("ph3_type").ToString.Trim
            txtEmail2.Text = objdataset.Tables(0).Rows(0).Item("internet2").ToString.Trim
            Dim ccno As String = objdataset.Tables(0).Rows(0).Item("cc_number").ToString().Trim

            Dim objed As New vbCustomer
            Dim strDecode As String = objed.base64Decode(objdataset.Tables(0).Rows(0).Item("cc_number").ToString().Trim)
            txtcard.Text = strDecode
            If strDecode.Length = 17 Then
                lblCard.Text = "XXXX-XXXXXX-X" & strDecode.Substring(strDecode.Length - 4, 4)
                txtcard.Visible = False
                txtcard.Visible = False
                txtCreditCard.Text = "XXXX-XXXXXX-X" & strDecode.Substring(strDecode.Length - 4, 4)
                lblCard.Visible = True
            ElseIf strDecode.Length = 19 Then
                lblCard.Text = "XXXX-XXXX-XXXX-" & strDecode.Substring(strDecode.Length - 4, 4)
                txtcard.Visible = False
                txtcard.Visible = False
                txtCreditCard.Text = "XXXX-XXXX-XXXX-" & strDecode.Substring(strDecode.Length - 4, 4)
                lblCard.Visible = True
            Else
                txtCreditCard.Visible = True
                txtCreditCard.Text = strDecode
                txtcard.Visible = False
                lblCard.Visible = False
                lblCard.Text = strDecode
                txtcard.Text = strDecode
            End If

            If strDecode.Length <= 19 And strDecode.Length > 0 Then
                If strDecode.Length = 17 And strDecode.Chars(0) = "3" Then
                    lblCardType.Visible = True
                    lblCardType.Src = "icon small/card3c.gif"
                    lblcardname.Text = "American Express Card No : "
                    lblno.Text = strDecode
                    lbldatetime.Text = objdataset.Tables(0).Rows(0).Item("cc_exp_dt").ToString()
                    'txtCreditCard.Text = ccno.Substring(0, 4) & "-" & ccno.Substring(4, 6) & "-" & ccno.Substring(10, 5)
                ElseIf strDecode.Length = 19 And strDecode.Chars(0) = "4" Then
                    lblCardType.Visible = True
                    lblCardType.Src = "icon small/card2c.gif"
                    lblcardname.Text = "Visa Card No : "
                    lblno.Text = strDecode
                    lbldatetime.Text = objdataset.Tables(0).Rows(0).Item("cc_exp_dt").ToString()

                    'txtCreditCard.Text = ccno.Substring(0, 4) & "-" & ccno.Substring(4, 4) & "-" & ccno.Substring(8, 4) & "-" & ccno.Substring(12, 4)
                ElseIf strDecode.Length = 19 And strDecode.Chars(0) = "5" Then
                    lblCardType.Visible = True
                    lblCardType.Src = "icon small/card1c.gif"
                    lblcardname.Text = "Master Card No : "
                    lblno.Text = strDecode
                    lbldatetime.Text = objdataset.Tables(0).Rows(0).Item("cc_exp_dt").ToString()

                    'txtCreditCard.Text = ccno.Substring(0, 4) & "-" & ccno.Substring(4, 4) & "-" & ccno.Substring(8, 4) & "-" & ccno.Substring(12, 4)
                Else
                    lblCardType.Width = 0
                    lnkView.Visible = False
                    txtCreditCard.Text = strDecode
                End If
            Else
                lnkView.Visible = False
                lblCardType.Width = 0
                txtCreditCard.Text = strDecode
                txtcard.Text = strDecode
            End If
            If strDecode = "" Then
                ddlMonth.Enabled = False
                ddlYear.Enabled = False
            End If
            

            txtFirstName.Text = objdataset.Tables(0).Rows(0).Item("f_name").ToString().Trim
            txtlastname.Text = objdataset.Tables(0).Rows(0).Item("l_name").ToString().Trim
            ' txtMiddleName.Text = objdataset.Tables(0).Rows(0).Item("m_name").ToString().Trim
            lblLastPurchase.Text = objdataset.Tables(0).Rows(0).Item("last_purch_dt").ToString().Trim
            'If lblLastPurchase.Text <> "" Then
            '    If DateDiff("YYYY", System.Convert.ToDateTime(lblLastPurchase.Text), Date.Now) >= 1 Then
            '        lblLastPurchase.ForeColor = Drawing.Color.Red
            '        lblLastPurchaseTitle.ForeColor = Drawing.Color.Red
            '    Else
            '        lblLastPurchase.ForeColor = Drawing.Color.Black
            '        lblLastPurchaseTitle.ForeColor = Drawing.Color.Black

            '    End If
            'Else
            '    lblLastPurchase.ForeColor = Drawing.Color.Black
            '    lblLastPurchaseTitle.ForeColor = Drawing.Color.Black

            'End If
            txtLockNotes.Text = objdataset.Tables(0).Rows(0).Item("lock_note").ToString().Trim
            txtPhone.Text = objdataset.Tables(0).Rows(0).Item("phone").ToString().Trim
            txtState.Text = objdataset.Tables(0).Rows(0).Item("state").ToString().Trim
            txtTax.Text = objdataset.Tables(0).Rows(0).Item("tax_excempt").ToString().Trim
            lblTotalPurchase.Text = objdataset.Tables(0).Rows(0).Item("life_purch").ToString().Trim
            txtZip.Text = objdataset.Tables(0).Rows(0).Item("zip").ToString().Trim
            ddlLock.SelectedValue = objdataset.Tables(0).Rows(0).Item("lock").ToString().Trim
            ddlLock_SelectedIndexChanged(Nothing, Nothing)
            txtNote.Text = objdataset.Tables(0).Rows(0).Item("notes").ToString().Trim
            ViewState("NotesDetail") = objdataset.Tables(0).Rows(0).Item("notes").ToString().Trim
            If objdataset.Tables(0).Rows(0).Item("custtype_id") <> 0 Then
                ddlCusType.SelectedValue = objdataset.Tables(0).Rows(0).Item("custtype_id").ToString().Trim
            Else
                ddlCusType.SelectedIndex = 0
            End If
        Else
            lblCardType.Width = 0
        End If
       

    End Sub

    
    'Protected Sub btnyes_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnyes.Click
    '    Label22.Visible = True
    '    txtLockNotes.Visible = True
    'End Sub

    Protected Sub imagebutton2_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imagebutton2.Click
        btnSave.Visible = True
        btnCancel1.Visible = True
        imagebutton2.Visible = False
        ViewState("type") = "edit"
        Call unlockcontrols()
        'If ViewState("NotesDetail").ToString().Length() = 0 Then
        '    lnkAddNotes.Visible = True
        '    lblNotesDetail.Text = "-"
        '    lnkAddNotes.Text = "Add Notes"
        '    lblhdrAdd.Text = "Add Notes"
        '    txtNote.Enabled = True
        'End If
        ' updatepanel1.Update()
    End Sub

    Protected Sub imgexit_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgexit.Click
        Response.Redirect("POSCustomers.aspx")

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

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnAdd.Click
        ViewState("type") = "add"
        ViewState("NotesDetail") = ""
        unlockcontrols()

    End Sub
    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnDelete.Click
        'Call customerid()
        ViewState("name") = txtlastname.Text.Trim
        lblSuredelete.Visible = True
        lblStationnodelete.Text = "Customer - " + ViewState("name").ToString() + " (" + ViewState("id").ToString() + ") "
        btnYesDelete.Visible = True
        btnNoDelete.Visible = True

        imagebutton2.Visible = True
        imgexit.Visible = True
        ModalPopupExtenderDelete.Show()
    End Sub
    Protected Sub btnYesDelete_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnYesDelete.Click
        ' ViewState("customerid") = id
        ' creating connection with database
        Dim objconnection As conClass = New conClass
        objconnection.DBconnect()
        ' create sqlcommand
        Dim objcommand As SqlCommand = New SqlCommand
        ' various sqlcommand properties
        objcommand.Connection = objconnection.con1
        objcommand.CommandType = CommandType.StoredProcedure
        objcommand.CommandText = "proc_delete_customer_info"
        objcommand.Parameters.AddWithValue("@customerid", ViewState("id"))
        'Try
        objcommand.ExecuteNonQuery()
        objconnection.DBDisconnect()

        ModalPopupExtenderDelete.Hide()
        imagebutton2.Visible = True
        imgexit.Visible = True
        ViewState.Remove("id")
      
        Call getcustomerdetails()
       
        objconnection.DBDisconnect()
        'End Try

        'Call getcustomerdetails()
    End Sub
    'Public Sub customerid()
    '    Dim id As String
    '    id = ""
    '    'Dim admin As String
    '    'admin = ""

    '    Dim strrow As GridViewRow
    '    For Each strrow In gvcustomers.rows
    '        Dim checkbox As CheckBox = CType(strrow.Cells(0).FindControl("chkbox"), CheckBox)
    '        If checkbox.Checked = True Then
    '            ViewState("name") = CType(strrow.Cells(2).FindControl("lblname"), Label).Text.Trim
    '            ViewState("customerid") = CType(strrow.Cells(1).FindControl("labelno"), Label).Text.Trim
    '            Exit For
    '            'If id = "" Then
    '            '    id = strrow.Cells(1).Text
    '            'Else
    '            '    id = id & "#" & strrow.Cells(1).Text
    '            'End If
    '        End If
    '    Next
    'End Sub
    'Public Sub deleteuser()

    '    ' ViewState("customerid") = id
    '    ' creating connection with database
    '    Dim objconnection As conClass = New conClass
    '    objconnection.DBconnect()
    '    ' create sqlcommand
    '    Dim objcommand As SqlCommand = New SqlCommand
    '    ' various sqlcommand properties
    '    objcommand.Connection = objconnection.con1
    '    objcommand.CommandType = CommandType.StoredProcedure
    '    objcommand.CommandText = "proc_delete_customer_info"
    '    objcommand.Parameters.AddWithValue("@customerid", ViewState("customerid"))
    '    Try
    '        objcommand.ExecuteNonQuery()
    '        objconnection.DBDisconnect()
    '    Catch ex As Exception
    '        Response.Write(ex.ToString())

    '    Finally
    '        objconnection.DBDisconnect()
    '    End Try
    '    Call getcustomerdetails()
    'End Sub
    Protected Sub btnNoDelete_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnNoDelete.Click
        ModalPopupExtenderDelete.Hide()
        imagebutton2.Visible = True
        imgexit.Visible = True
        btnDelete.Visible = True
        btnAdd.Visible = True
    End Sub

    Public Sub getCustypevalues()
        Dim objconnection As conClass = New conClass
        ' Dim objreader As SqlDataReader
        objconnection.DBconnect()

        Dim objcommand As SqlCommand = New SqlCommand
        objcommand.CommandType = CommandType.StoredProcedure
        objcommand.CommandText = "proc_Custype_get_values"
        objcommand.Connection = objconnection.con1
        objcommand.Parameters.AddWithValue("@storeno", ViewState("storeno"))

        Dim objdataadapter As SqlDataAdapter = New SqlDataAdapter
        Dim objdataset As DataSet = New DataSet
        objdataadapter.SelectCommand = objcommand

        'Try
        objdataadapter.Fill(objdataset)
        If objdataset.Tables(0).Rows.Count > 0 Then
            If ViewState("type") = "edit" Or ViewState("type") = "view" Then
                ddlCusType.DataSource = objdataset.Tables(0)
                ddlCusType.DataTextField = "cusType_name"
                ddlCusType.DataValueField = "cusType_id"
                ddlCusType.DataBind()
                ddlCusType.Items.Insert(0, New ListItem("-NONE-", ""))
            Else
                ddlCusType.DataSource = objdataset.Tables(0)
                ddlCusType.DataTextField = "cusType_name"
                ddlCusType.DataValueField = "cusType_id"
                ddlCusType.DataBind()
            End If
            ddlCusType.Visible = True
            lblNoCustype.Visible = False
        Else
            ddlCusType.Visible = False
            lblNoCustype.Visible = True
        End If

        objconnection.DBDisconnect()
    End Sub

#Region "Credit card Password Security"
    Protected Sub lnkView_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles lnkView.Click
        If DateDiff(DateInterval.Day, Date.Today, CDate(ViewState("Expdate"))) <= 14 And DateDiff(DateInterval.Day, Date.Today, CDate(ViewState("Expdate"))) > 0 Then
            pastWarning.Visible = False
            lblWarning.Text = "You were required to change your password no later than 11:59 PM, " & CDate(ViewState("Expdate")).Date
            upwarning.Update()
            mpeWarning.Show()
        ElseIf DateDiff(DateInterval.Day, Date.Today, CDate(ViewState("Expdate")).Date) <= 0 Then
            warninigli.Visible = True
            pastWarning.Visible = True
            lblWarning.Text = "You were required to change your password by 11:59 PM, " & CDate(ViewState("Expdate")).Date
            upwarning.Update()
            mpeWarning.Show()
        Else
            txtUsername.Text = ""
            txtPass.Text = ""
            mpeLogincredit.Show()
            txtUsername.Focus()
        End If

    End Sub

    Protected Sub btnLoginOK_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnLoginOK.Click
        Dim objEmployee As New vbEmployee
        objEmployee.Employee_Id = txtUsername.Text.Trim
        objEmployee.Password = txtPass.Text.Trim
        objEmployee.Storeno = ViewState("storeno")
        If objEmployee.BolError = False Then
            If objEmployee.fnGetEmployeeCreditCardLogin > 0 Then
                mpeCard.Show()
                btnimgClose.Focus()
                mpeLogincredit.Hide()
            Else
                ScriptManager.RegisterStartupScript(Page, [GetType], "Message", "alert('User Name and Password do not match!');", True)
                txtUsername.Text = ""
                txtPass.Text = ""
                txtUsername.Focus()
                mpeLogincredit.Show()
                upLogin.Update()
            End If
        End If
    End Sub

    Protected Sub btnNoContinue_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnNoContinue.Click
        If DateDiff(DateInterval.Day, Date.Today, CDate(ViewState("Expdate"))) <= 0 Then
            Dim objEmployee As New vbEmployee
            objEmployee.Employee_Id = ViewState("emp_id")
            objEmployee.Storeno = ViewState("storeno")
            objEmployee.fnExpiryPassword()
            lnkView.Visible = False
            mpeWarning.Hide()
        Else
            mpeLogincredit.Show()
            txtUsername.Focus()
            upLogin.Update()
        End If
    End Sub

    Protected Sub btnChangepass_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnChangepass.Click

        mpeCreditPassWord.Show()
        txtCreditPassWord.Focus()
        mpeWarning.Hide()
        upCreditPassWord.Update()
    End Sub
    Protected Sub btnCancelPass_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnCancelPass.Click
        txtCreditPassWord.Text = ""
        repassword.Text = ""
        mpeCreditPassWord.Hide()
        'btnimgClose.Focus()

        If DateDiff(DateInterval.Day, Date.Today, CDate(ViewState("Expdate"))) <= 0 Then
            lnkView.Visible = False
        Else
            mpeCard.Show()
            
        End If

    End Sub

    Protected Sub btnOKPass_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnOKPass.Click
        Dim objEmployee As New vbEmployee
        objEmployee.Employee_Id = ViewState("emp_id")
        objEmployee.Password = txtCreditPassWord.Text.Trim
        objEmployee.fnChangePassword()
        mpeCreditPassWord.Hide()
        btnimgClose.Focus()
        mpeCard.Show()
    End Sub
#End Region
    ''' ''' Function to get requires fields as Selected from Global Setup --> POS Options
    'Public Sub GetRequirefields()
    '    Dim objcust As New vbStationSetup
    '    Dim dt As New DataTable

    '    objcust.StationNo = ViewState("stationno")
    '    objcust.StoreNo = ViewState("storeno")
    '    dt = objcust.fnGetCustomerlookup()

    '    If dt.Rows.Count > 0 Then
    '        If dt.Rows(0)("Custype") = "Y" Then
    '            imgCusType.Visible = True
    '            rfvCustype.Enabled = True
    '        End If
    '        If dt.Rows(0)("email") = "Y" Then
    '            imgEmail1.Visible = True
    '            rfvEmail1.Enabled = True
    '        End If
    '        If dt.Rows(0)("secondemail") = "Y" Then
    '            imgEmail2.Visible = True
    '            rfvEmail2.Enabled = True
    '        End If
    '        If dt.Rows(0)("phone") = "Y" Then
    '            imgPhone1.Visible = True
    '            rfvPhone1.Enabled = True
    '        End If
    '        If dt.Rows(0)("phone2") = "Y" Then
    '            imgPhone2.Visible = True
    '            rfvPhone2.Enabled = True
    '        End If
    '        If dt.Rows(0)("phone3") = "Y" Then
    '            imgPhone3.Visible = True
    '            rfvPhone3.Enabled = True
    '        End If
    '        If dt.Rows(0)("lastpurchase") = "Y" Then
    '            'lblLastPurchase.ForeColor = Drawing.Color.Red
    '            'lblLastPurchaseTitle.ForeColor = Drawing.Color.Red
    '        End If
    '        If dt.Rows(0)("pricelevel") = "Y" Then
    '            imgPricelevel.Visible = True
    '        End If
    '        If dt.Rows(0)("taxexempt") = "Y" Then
    '            imgTaxExempt.Visible = True
    '            rfvTaxExempt.Enabled = True
    '        End If
    '    Else
    '        imgCusType.Visible = False
    '        rfvCustype.Enabled = False
    '        imgEmail1.Visible = False
    '        rfvEmail1.Enabled = False
    '        imgEmail2.Visible = False
    '        rfvEmail2.Enabled = False
    '        imgPhone1.Visible = False
    '        rfvPhone1.Enabled = False
    '        imgPhone2.Visible = False
    '        rfvPhone2.Enabled = False
    '        imgPhone3.Visible = False
    '        rfvPhone3.Enabled = False
    '        imgTaxExempt.Visible = False
    '        rfvTaxExempt.Enabled = False
    '        imgPricelevel.Visible = False
    '        lblLastPurchase.ForeColor = Drawing.Color.Black
    '        lblLastPurchaseTitle.ForeColor = Drawing.Color.Black
    '    End If


    'End Sub

#Region "Add Notes"

    Protected Sub lnkAddNotes_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkAddNotes.Click
        If ViewState("type") = "add" Then
            lblhdrAdd.Text = "Add Notes"
            txtNote.Focus()
            'txtNote.Enabled = True
            txtNote.Text = ""
        End If

        If ViewState("type") = "edit" Then
            txtNote.Focus()
            'txtNote.Enabled = True
        End If
        mpeAddNotes.Show()
        UpdatePanelAdd.Update()
        updatepanel1.Update()
    End Sub

    Protected Sub btnCanceladd_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnCanceladd.Click
        mpeAddNotes.Hide()
        If ViewState("mode") <> "detail" Then

        End If

    End Sub

    Protected Sub btnSaveadd_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSaveadd.Click
        mpeAddNotes.Hide()
        If ViewState("type") <> "view" Then
            If txtNote.Text.ToString().Length < 56 Then
                lblNotesDetail.Text = txtNote.Text.ToString()
            Else
                lblNotesDetail.Text = txtNote.Text.ToString().Substring(0, 55)
            End If
            lblNotesDetail.Visible = True
            lnkAddNotes.Text = "More..."
        End If
        updatepanel1.Update()
    End Sub
#End Region

#Region "Link"
    Protected Sub linkList_Click(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs)
        Dim id As String = e.CommandArgument
        If id = 1 Then
            Dim strReturn As String

            'lblHeader.Text = "Customer List"
            lnkList.CssClass = "style8black"
            strReturn = ViewState("id")
            Response.Redirect("POSCustomers.aspx?ID=" & strReturn)
        ElseIf id = 2 Then
            'lblHeader.Text = "Customer Details"
            '  Call detailuser()
            lnkDetail.CssClass = "style8black"
        ElseIf id = 3 Then
            lnkSalesHistory.CssClass = "style8black"
            'lblHeader.Text = "Sales History"
        End If
        Call bindview(id)

    End Sub

    Protected Sub bindview(ByVal id As String)
        lnkDetail.CssClass = "style8"
        lnkList.CssClass = "style8"
        lnkSalesHistory.CssClass = "style8"


        If id = 1 Then
            lnkList.CssClass = "style8black"
            '   mulCustomers.ActiveViewIndex = 0

        ElseIf id = 2 Then
            lnkDetail.CssClass = "style8black"
            ' mulCustomers.ActiveViewIndex = 1
            ViewState("mode") = "detail"


        ElseIf id = 3 Then
            lnkSalesHistory.CssClass = "style8black"
            ' mulCustomers.ActiveViewIndex = 2

        End If
    End Sub
#End Region
    
End Class
