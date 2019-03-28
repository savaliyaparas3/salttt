<%@ Page Language="VB" AutoEventWireup="false" CodeFile="POSAddEmployeeWizard.aspx.vb" Inherits="POSAddEmployeeWizard" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">

    <title></title>
    <link href="1style.css" rel="stylesheet" type="text/css" />
    <link href="CSS/Mainstyle.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" language="javascript">
        <%--function setDivScroll() {
            var divobj = $get('<%= divTreeview.ClientID %>');
            var hdnobj = $get('<%= hdndivtree.ClientID %>');
            if (hdnobj) hdnobj.value = divobj.scrollTop;
        }

        function pageload() {
            var divObj = $get('<%= divTreeview.ClientID %>');
            var obj = $get('<%= hdndivtree.ClientID %>');
            if (divObj) divObj.scrollTop = obj.value;
        } --%>
        function setHeader(name) {
            var str = "<font style='text-transform:capitalize' size='2' >&nbsp;<b>-</b>&nbsp;" + name + "</font>"
            window.parent.document.getElementById('ctl00_ContentPlaceHolder2_lblAddEmployee').innerHTML = "Add Employee Wizard" + str;
        }
        function TreeviewDblClick() {

            var updateProgress1 = document.getElementById('<%= UpdateProgress1.ClientID %>');
            updateProgress1.style.display = 'block';
            // var obj = document.getElementById('<%=btn1.ClientID %>'); 
            // obj.click();

        }

        function show(paraId) {
            var element = document.getElementById(paraId);
            element.scrollIntoView(true);
        }

        function openSecurity(view) {
            var Store = '<%= Session("StoreNo")%>'
            var Employee = document.getElementById('<%=hdnEmployeeId.ClientID%>').value;
            
            var width = 800;
            var height = 560;
            var left = (screen.width - width) / 2;
            var top = (screen.height - height) / 2;
            var params = 'width=' + width + 'px' + '; height=' + height + 'px';
            params += '; top=' + top + '; left=' + left;
            params += '; directories=no';
            params += '; location=no';
            params += '; menubar=no';
            params += '; resizable=0';
            params += '; status=0';
            params += '; scroll=0';
            
            var tmp = window.open("POSEmployeeSecurity.aspx?storeno=" + Store + "&View=" + view + "&Emp_id=" + Employee + "", '_blank', params);
            
        }
        function SecuritySaveClose()
        {
            document.getElementById('<%= hdnbtnEmployeeSecurity.ClientID%>').click();
        }
        function selectallstore() {
            var chkallstore = document.getElementById('<%=chkMainHeader.ClientID%>');
            var chksubstore = document.getElementById('grdStores_ctl01_chkBxHeader');
            if (chkallstore.checked) {
                chksubstore.checked = true;
            }
            else { chksubstore.checked = false; }

        }
        function HeaderClick(CheckBox) {
            //Get target base & child control.
            var TargetBaseControl = document.getElementById('<%=grdStores.ClientID%>');
            var TargetChildControl = "chkSelect";

            //Get all the control of the type INPUT in the base control.
            var Inputs = TargetBaseControl.getElementsByTagName("input");

            //Checked/Unchecked all the checkBoxes in side the GridView.
            for (var n = 0; n < Inputs.length; ++n)
                if (Inputs[n].type == 'checkbox' && Inputs[n].disabled == false && Inputs[n].id.indexOf(TargetChildControl, 0) >= 0)
                    Inputs[n].checked = CheckBox.checked;


        }
        function ChildClick(objRef) {


            var row = objRef.parentNode.parentNode;

            //Get the reference of GridView

            var GridView = row.parentNode;

            //Get all input elements in Gridview

            var inputList = GridView.getElementsByTagName("input");

            for (var i = 0; i < inputList.length; i++) {

                //The First element is the Header Checkbox

                var headerCheckBox = inputList[0];

                //Based on all or none checkboxes

                //are checked check/uncheck Header Checkbox

                var checked = true;

                if (inputList[i].type == "checkbox" && inputList[i] != headerCheckBox) {

                    if (!inputList[i].checked) {

                        checked = false;

                        break;

                    }

                }

            }

            headerCheckBox.checked = checked;
            if (headerCheckBox.checked) {
            }
            else {
                var chkallstore = document.getElementById('<%=chkMainHeader.ClientID%>');
                chkallstore.checked = false;
            }
        }
    </script>
    <style type="text/css">
        .labelgreentitle {
            font-family: Arial;
            font-size: 11pt;
            color: #06484E;
        }

        .labelgreentitlewithoutbold {
            font-family: Arial;
            font-size: 11pt;
            color: #06484E;
        }

        .textbox {
            font-family: Arial;
            font-size: 11pt;
            height: 22px;
            color: #06484E;
        }

        input[type="checkbox"] {
            -moz-appearance: none;
            transition: none;
            width: 15px;
            height: 15px;
            margin: 0px 0px 0px;
            margin-right: 5px;
        }
    </style>
    <style type="text/css">
        .Bottom1 {
            position: absolute;
            bottom: 35px;
            right: 22px;
        }

        .GridAlign {
            padding-right: 3px;
            padding-left: 3px;
        }

        .WhiteBorder {
            border-right-color: White;
        }

        .auto-style4 {
            width: 4%;
        }

        .auto-style5 {
            width: 11%;
        }

        .auto-style6 {
            width: 7%;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <ajax:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" EnablePartialRendering="true">
        </ajax:ScriptManager>
        <asp:Button ID="hdnbtnEmployeeSecurity" runat="server" Style="display: none;" />
        <div id="divmaincontent" runat="server">
            <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                <tr>
                    <td>
                        <div>
                            <asp:UpdatePanel ID="upnlAddEmpWizard" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:MultiView ID="MultiViewEmpAdd" runat="server">
                                        <asp:View ID="ViewAddInfo" runat="server">
                                            <table width="100%" cellpadding="0" cellspacing="0" style="background-color: #CCE7FF; height: 485px;">
                                                <tr>
                                                    <td style="height: 10px; font-family: Arial; padding-left: 5px; padding-top: 10px" valign="top ">
                                                        <asp:Label ID="lblAddEmpwizard" runat="server" Text="" CssClass="labelall"></asp:Label>

                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="vertical-align: middle; text-align: center">
                                                        <img src="img/EmployeeWizardImg.png" />

                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:View>
                                        <asp:View ID="ViewAddEmp" runat="server">
                                            <table width="100%" cellpadding="0" cellspacing="0" style="background-color: #CCE7FF; height: 485px;">
                                                <tr>
                                                    <td style="height: 10px; padding-left: 10px; padding-top: 15px">
                                                        <span id="Span1" class="labelgreentitle" runat="server">OK, let's start with some basic
                                                        information.</span>&nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td valign="top">
                                                        <table width="100%" cellpadding="0" cellspacing="0">
                                                            <tr>
                                                                <td>
                                                                    <table border="0" cellpadding="0" cellspacing="0" width="99%" style="position: static;"
                                                                        align="center">

                                                                        <tr>
                                                                            <td style="width: 100%; padding-left: 10px; padding-top: 10px;" valign="top" align="left">
                                                                                <table border="0" cellpadding="0" cellspacing="0" width="100%" style="position: static">
                                                                                    <tr>
                                                                                        <td align="left" style="padding-bottom: 3px; width: 30%;">
                                                                                            <span class="labelgreentitlewithoutbold">Employee ID</span>  <span style="font-family: Arial; color: red;">*</span>
                                                                                        </td>
                                                                                        <td align="left" style="padding-bottom: 3px; width: 30%;">
                                                                                            <span class="labelgreentitlewithoutbold">Last Name</span> <span style="font-family: Arial; color: red;">*</span>
                                                                                        </td>
                                                                                        <td align="left" style="padding-bottom: 3px; width: 30%;">
                                                                                            <span class="labelgreentitlewithoutbold">First Name</span> <span style="font-family: Arial; color: red;">*</span>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td align="left" style="padding-bottom: 1px; width: 30%;" valign="top">
                                                                                            <asp:Label ID="lblEmployeeID" runat="server" CssClass="labelgreentitle" style="display:none;"></asp:Label>
                                                                                            <asp:TextBox ID="txtEmpId" runat="server" BorderColor="White" Font-Bold="False" MaxLength="10" CssClass="textbox" Enabled="true" 
                                                                                                Style="position: static; text-transform: uppercase" Width="50%" TabIndex="1" onkeydown="return EmailValidation(event);"
                                                                                                ValidationGroup="EmpAdd" onfocus="changecolor(this,'focus');setCursor(this,0);" onblur="changecolor(this,'blur');javascript:checkUser();"></asp:TextBox>
                                                                                            <asp:RequiredFieldValidator ID="reqLName" runat="server" ControlToValidate="txtEmpId"
                                                                                                SetFocusOnError="True" Style="position: static" ErrorMessage="*" Font-Size="Large"
                                                                                                ForeColor="Red" ValidationGroup="EmpAdd"></asp:RequiredFieldValidator>
                                                                                            <span id="errorpart" style="color: Red;" runat="server"></span>&nbsp;<asp:Label ID="lblEmpId"
                                                                                                runat="server" CssClass="labelgreentitle" Visible="false"></asp:Label>
                                                                                              
                                                                                        </td>
                                                                                        <td align="left" style="padding-bottom: 1px; width: 30%;" valign="top">
                                                                                            <asp:TextBox ID="txtLName" runat="server" BorderColor="White" Font-Bold="False" MaxLength="15" CssClass="textbox"
                                                                                                Style="position: static; text-transform: capitalize;" Width="80%" ValidationGroup="EmpAdd"
                                                                                                TabIndex="2" onfocus="changecolor(this,'focus')" onblur="changecolor(this,'blur')"></asp:TextBox>
                                                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtLName"
                                                                                                SetFocusOnError="True" Style="position: static" ErrorMessage="*" Font-Size="Large"
                                                                                                ForeColor="Red" ValidationGroup="EmpAdd"></asp:RequiredFieldValidator>
                                                                                        </td>
                                                                                        <td align="left" style="padding-bottom: 1px; width: 30%;" valign="top">
                                                                                            <asp:TextBox ID="txtFName" runat="server" BorderColor="White" Font-Bold="False" MaxLength="15" CssClass="textbox"
                                                                                                Style="position: static; text-transform: capitalize" Width="80%" TabIndex="3"
                                                                                                ValidationGroup="EmpAdd" onfocus="changecolor(this,'focus')" onblur="changecolor(this,'blur')"></asp:TextBox>
                                                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtFName"
                                                                                                SetFocusOnError="True" Style="position: static" ErrorMessage="*" Font-Size="Large"
                                                                                                ForeColor="Red" ValidationGroup="EmpAdd"></asp:RequiredFieldValidator>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td colspan="3" align="left" style="padding-bottom: 10px; width: 30%;">
                                                                                            <span class="labelgreentitlewithoutbold" style="font-weight: bold;">Usually your initials.</span>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td colspan="3" align="left" style="padding-bottom: 3px; padding-top: 5px; width: 30%;">
                                                                                            <span class="labelgreentitlewithoutbold">Address</span>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td align="left" style="padding-bottom: 15px; width: 30%;" colspan="3">
                                                                                            <asp:TextBox ID="txtaddress" runat="server" BorderColor="White" Font-Bold="False" CssClass="textbox"
                                                                                                MaxLength="40" Style="position: static; text-transform: capitalize;"
                                                                                                Width="60%" TabIndex="4" onfocus="changecolor(this,'focus');geolocate();" onblur="changecolor(this,'blur')"> </asp:TextBox>
                                                                                        </td>

                                                                                    </tr>

                                                                                    <tr>
                                                                                        <td align="left" style="padding-bottom: 3px; width: 30%;" valign="top">
                                                                                            <span class="labelgreentitlewithoutbold">Zip Code </span>
                                                                                        </td>
                                                                                        <td align="left" style="padding-bottom: 3px; width: 30%;">
                                                                                            <span class="labelgreentitlewithoutbold" style="Width: 60%;">City </span>

                                                                                        </td>
                                                                                        <td align="left" style="padding-bottom: 3px;">
                                                                                            <span class="labelgreentitlewithoutbold">State </span>

                                                                                        </td>
                                                                                    </tr>

                                                                                    <tr>
                                                                                        <td align="left" style="padding-bottom: 15px; width: 30%;" valign="top">
                                                                                            <asp:TextBox ID="txtZip" runat="server" BorderColor="White" Font-Bold="False" MaxLength="5" CssClass="textbox"
                                                                                                Style="position: static; text-transform: capitalize; font-weight: normal;" Width="50%" OnKeyPress="return fncInputNumericValuesOnly(event)"
                                                                                                TabIndex="5" onfocus="changecolor(this,'focus')" onblur="changecolor(this,'blur');getCitystate();"> </asp:TextBox>

                                                                                        </td>
                                                                                        <td align="left" style="padding-bottom: 15px; width: 30%;" valign="top">
                                                                                            <asp:TextBox ID="txtCity" runat="server" BorderColor="White" Font-Bold="False" MaxLength="30" CssClass="textbox"
                                                                                                Style="position: static; text-transform: initial; font-weight: normal;" Width="65%"
                                                                                                TabIndex="5" onfocus="changecolor(this,'focus')" onblur="changecolor(this,'blur');"> </asp:TextBox>
                                                                                        </td>
                                                                                        <td align="left" style="padding-bottom: 15px;" valign="top">

                                                                                            <asp:TextBox ID="txtState" runat="server" BorderColor="White" Font-Bold="False" MaxLength="2" CssClass="textbox"
                                                                                                Style="position: static; text-transform: uppercase; font-weight: normal;" Width="20%"
                                                                                                TabIndex="5" onfocus="changecolor(this,'focus')" onblur="changecolor(this,'blur');"> </asp:TextBox>
                                                                                            <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" FilterType="LowercaseLetters, UppercaseLetters" TargetControlID="txtState" />
                                                                                            <asp:HiddenField ID="hdnCityState" runat="server" />
                                                                                        </td>
                                                                                    </tr>

                                                                                    <tr>
                                                                                        <td align="left" style="padding-bottom: 3px;">
                                                                                            <span class="labelgreentitlewithoutbold">Mobile #</span>
                                                                                            <span style="font-family: Arial; color: red;">*</span>
                                                                                        </td>
                                                                                        <td align="left" style="padding-bottom: 3px;">

                                                                                            <span class="labelgreentitlewithoutbold">Email</span> <span style="font-family: Arial; color: red;">*</span>
                                                                                            <asp:RegularExpressionValidator ID="regexpWIPemail" runat="server" ValidationGroup="EmpAdd"
                                                                                                ControlToValidate="txtemail" ErrorMessage="*" Font-Size="Medium" ValidationExpression="^(([A-Za-z0-9]+_+)|([A-Za-z0-9]+\-+)|([A-Za-z0-9]+\.+)|([A-Za-z0-9]+\++))*[A-Za-z0-9]+@((\w+\-+)|(\w+\.))*\w{1,63}\.[a-zA-Z]{2,6}$"> </asp:RegularExpressionValidator>
                                                                                        </td>
                                                                                        <td>&nbsp;</td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td align="left" style="padding-bottom: 15px;" valign="top">
                                                                                            <asp:TextBox ID="txtMobilePhone" runat="server" BorderColor="White" Font-Bold="False" ValidationGroup="EmpAdd" CssClass="textbox"
                                                                                                MaxLength="14" Style="position: static" Width="55%" TabIndex="6" onfocus="changecolor(this,'focus')"
                                                                                                onblur="changecolor(this,'blur')"></asp:TextBox>
                                                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtMobilePhone"
                                                                                                SetFocusOnError="True" Style="position: static" ErrorMessage="*" Font-Size="Large" InitialValue="(___) ___-____"
                                                                                                ForeColor="Red" ValidationGroup="EmpAdd"></asp:RequiredFieldValidator>
                                                                                            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" runat="server" ClearMaskOnLostFocus="false"
                                                                                                MaskType="number" CultureName="en-GB" Mask="(999) 999-9999" TargetControlID="txtMobilePhone"
                                                                                                AutoComplete="false" AutoCompleteValue="false">
                                                                                            </ajaxToolkit:MaskedEditExtender>

                                                                                        </td>

                                                                                        <td align="left" style="padding-bottom: 3px;">
                                                                                            <asp:TextBox ID="txtemail" runat="server" BorderColor="White" Font-Bold="False" Style="position: static" ValidationGroup="EmpAdd"
                                                                                                Width="92%" TabIndex="7" onfocus="changecolor(this,'focus');setCursor(this,0);" onblur="changecolor(this,'blur');" CssClass="textbox"
                                                                                                MaxLength="50" onkeydown="return EmailValidation(event);"></asp:TextBox>
                                                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtemail"
                                                                                                SetFocusOnError="True" Style="position: static" ErrorMessage="*" Font-Size="Large"
                                                                                                ForeColor="Red" ValidationGroup="EmpAdd"></asp:RequiredFieldValidator>
                                                                                            <asp:HiddenField ID="hdnEmail" runat="server" />
                                                                                            <asp:HiddenField ID="hdnMode" runat="server" />
                                                                                            <asp:Label ID="lblEMailAddressOnFile" runat="server" CssClass="labelgreentitlewithoutbold" Font-Bold="True"
                                                                                                Font-Names="Arial"></asp:Label>
                                                                                        </td>
                                                                                        <td>&nbsp;</td>
                                                                                    </tr>
                                                                                    <tr id="truserrole" runat="server">
                                                                                        <td align="left" style="padding-bottom: 3px;">
                                                                                            <span class="labelgreentitlewithoutbold">User Role:</span>
                                                                                            &nbsp;
                                                                                            <asp:ImageButton ID="helpmenuUserRole" runat="server" CommandName="124" ImageUrl="~/images/Helpicon.gif"
                                                                                                OnCommand="Help_click" Style="vertical-align: middle;" />
                                                                                        </td>
                                                                                        <td align="left" style="padding-bottom: 3px;">
                                                                                            <asp:Label ID="lblAreaRepresentative" runat="server" Visible="false"><span class="labelgreentitlewithoutbold">Area Representative:</span> </asp:Label>
                                                                                        </td>
                                                                                        <td></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td align="left" style="padding-bottom: 15px; width: 30%;" valign="top">
                                                                                            <asp:DropDownList ID="drpEmproles" runat="server" CssClass="labelall" Style="width: 55.6%; height: 25px" AutoPostBack="true" TabIndex="8"></asp:DropDownList>

                                                                                        </td>
                                                                                        <td align="left" style="padding-bottom: 15px; width: 30%;" valign="top">
                                                                                            <asp:DropDownList ID="drpAreaLocation" runat="server" CssClass="labelall" Style="width: 93.6%; height: 25px" Visible="false" TabIndex="9"></asp:DropDownList>
                                                                                        </td>
                                                                                        <td align="left" style="padding-bottom: 15px;" valign="top"></td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:View>
                                        <asp:View ID="ViewAddPassword" runat="server">
                                            <table width="100%" cellpadding="0" cellspacing="0" style="background-color: #CCE7FF; height: 485px;">
                                                <tr>
                                                    <td colspan="2" align="left"
                                                        style="padding-bottom: 3px; padding-top: 15px; padding-left: 10px;">
                                                        <span id="Span2" class="labelgreentitle" runat="server">Now that the basic information
                                                        is complete, let's continue.</span>&nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" align="left" style="padding-bottom: 3px; padding-left: 10px;">
                                                        <span id="Span3" class="labelgreentitle" runat="server">It is time to assign a temporary
                                                        password. When 
                                                          <asp:Label ID="lblFnamePwd" Font-Bold="true" CssClass="labelgreentitle" runat="server"></asp:Label>
                                                            <asp:Label ID="lblLnamePwd" Font-Bold="true" CssClass="labelgreentitle" runat="server"></asp:Label>
                                                            logs in for the first time he/she will be prompted
                                                        to change their password.</span>&nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" align="left" style="padding-bottom: 3px; padding-left: 10px;">
                                                        <span id="Span4" class="labelgreentitle" runat="server">Passwords must be between five
                                                        and fifteen characters.</span>&nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" style="padding-bottom: 3px; width: 50%; padding-left: 10px;" valign="bottom">
                                                        <span class="labelgreentitlewithoutbold">Password</span>
                                                    </td>

                                                    <td align="left" style="padding-bottom: 3px; width: 50%; padding-left: 10px;" valign="bottom">
                                                        <span class="labelgreentitlewithoutbold">Confirm Password</span>
                                                        <asp:CustomValidator ClientValidationFunction="comparePass" ID="revcustcomppass"
                                                            runat="server" ControlToValidate="txtConfirmPassword" ValidateEmptyText="true" ValidationGroup="EmpAdd" />
                                                    </td>

                                                </tr>
                                                <tr>
                                                    <td align="left" style="padding-bottom: 3px; padding-left: 10px;" valign="top">
                                                        <asp:TextBox ID="txtPassword" runat="server" BorderColor="White" Font-Bold="False" CssClass="textbox"
                                                            Style="position: static; font-family: Arial" Width="82%" TabIndex="7" onfocus="changecolor(this,'focus');setCursor(this,0);"
                                                            onblur="changecolor(this,'blur');" MaxLength="20"></asp:TextBox>
                                                        &nbsp;&nbsp;
                                                    </td>
                                                    <td align="left" style="padding-bottom: 3px; padding-left: 10px;" valign="top">
                                                        <asp:TextBox ID="txtConfirmPassword" runat="server" BorderColor="White" Font-Bold="False" CssClass="textbox"
                                                            Style="position: static" Width="82%" TabIndex="7" onfocus="changecolor(this,'focus')"
                                                            onblur="changecolor(this,'blur');" MaxLength="20"></asp:TextBox>

                                                    </td>

                                                </tr>
                                                <tr>
                                                    <td align="left" style="padding-bottom: 3px; padding-left: 10px;" valign="top">
                                                        <asp:CheckBox ID="chkSuggestTmpPwd" runat="server"
                                                            Text="Suggest temporary password" onclick="GeneratePass();" CssClass="labelgreentitle" />

                                                    </td>
                                                    <td></td>
                                                </tr>
                                                <tr>
                                                    <td>&nbsp;
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:View>

                                        <asp:View ID="ViewOtherProducts" runat="server">
                                            <table width="100%" cellpadding="0" cellspacing="0" style="background-color: #CCE7FF; height: 485px;">
                                                <tr style="height: 20px">
                                                    <td colspan="2" align="left"
                                                        style="padding-bottom: 3px; padding-top: 5px; padding-left: 10px;">
                                                        <span id="Span5" class="labelgreentitle" runat="server">Lightning Online Point of Sale has few mobile products of:</span>&nbsp;
                                                    </td>
                                                </tr>
                                                <tr style="height: 60px">
                                                    <td colspan="2" align="left" valign="top"
                                                        style="padding-bottom: 3px; padding-left: 20px;">
                                                        <ul>
                                                            <li style="padding-bottom: 3px" class="labelgreentitle">Lightning Online Point of Sale for the iPad
                                                            </li>
                                                            <li class="labelgreentitle">Lightning Online Point of Sale for the iPhone / iPod Touch  </li>
                                                        </ul>
                                                        &nbsp;
                                                    </td>
                                                </tr>

                                                <tr style="height: 20px">
                                                    <td align="left"
                                                        style="padding-bottom: 3px; padding-left: 10px; white-space: nowrap;" colspan="2">
                                                        <span id="Span11" class="labelgreentitlewithoutbold" runat="server">Do you wish to have this employee access your stores data from these devices?</span>&nbsp;&nbsp;&nbsp;
                                                         <asp:DropDownList ID="ddlOtherProduct" CssClass="labelgreentitlewithoutbold" runat="server">
                                                             <asp:ListItem Text="No" Selected="True" Value="0"></asp:ListItem>
                                                             <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                         </asp:DropDownList>
                                                    </td>

                                                </tr>
                                                <tr style="height: 140px">
                                                    <td>&nbsp;</td>
                                                </tr>
                                            </table>
                                        </asp:View>
                                        <asp:View ID="ViewDiscount" runat="server">
                                            <table width="100%" cellpadding="0" cellspacing="0" style="background-color: #CCE7FF; height: 485px;">
                                                <tr>
                                                    <td colspan="2" align="left" style="padding-bottom: 3px; padding-left: 10px;"
                                                        class="style55">
                                                        <span id="Span7" class="labelgreentitle" runat="server">Let us assign the maximum discount 
                                                          <asp:Label ID="Label1" Font-Bold="true" CssClass="labelgreentitle" runat="server"></asp:Label>
                                                            <asp:Label ID="Label3" Font-Bold="true" CssClass="labelgreentitle" runat="server"></asp:Label>
                                                            this employee can give, by department. By default, all departments are set to 20%.</span>&nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" align="left" style="padding-bottom: 3px; padding-left: 10px;"
                                                        class="style53">
                                                        <span id="Span12" class="labelgreentitle" runat="server">This only applies to manual discounts for this employee.  It does not limit the automatic discounts setup in the system such as case prices, discount groups and promotions.</span>&nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">

                                                        <div style="overflow: auto; height: 330px;" runat="server" id="dvDepartment">
                                                            <ajax:UpdatePanel ID="upnlDepartment" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <asp:Panel ID="pnldis" runat="server">
                                                                        <asp:GridView ID="gvDepartment" PageSize="13" runat="server" AutoGenerateColumns="False"
                                                                            Width="100%" BorderColor="White" BorderWidth="1px" AllowSorting="True" AllowPaging="false"
                                                                            EmptyDataText="No Matching Data Found">
                                                                            <PagerStyle HorizontalAlign="Right" VerticalAlign="Middle" />
                                                                            <SelectedRowStyle CssClass="selectrow" />
                                                                            <HeaderStyle CssClass="header" HorizontalAlign="Left" VerticalAlign="Middle" BackColor="#B8C7D3" />
                                                                            <FooterStyle CssClass="header" HorizontalAlign="Left" VerticalAlign="Middle" />
                                                                            <AlternatingRowStyle CssClass="alterrow" VerticalAlign="Middle" />
                                                                            <RowStyle CssClass="row" VerticalAlign="Middle" />
                                                                            <EmptyDataRowStyle ForeColor="Red" HorizontalAlign="Center" />
                                                                            <Columns>
                                                                                <asp:TemplateField HeaderText="Department">
                                                                                    <HeaderStyle CssClass="header" HorizontalAlign="Left" VerticalAlign="Middle" />
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblDepartment" Style="padding-left: 15px" runat="server" CssClass="labelgreentitlewithoutbold" Text='<%# Eval("dept_desc") %>' ForeColor="Black"></asp:Label>
                                                                                        <asp:Label ID="lbldeptid" runat="server" Visible="false" Text='<%# Bind("empdisc_dept_id") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <HeaderTemplate>
                                                                                        <asp:Label ID="lbtnDeptdesc" Style="padding-left: 15px" runat="server" CommandArgument="dept_desc" CssClass="labelgreentitle"
                                                                                            CommandName="Sort" ForeColor="Black">Department</asp:Label>
                                                                                    </HeaderTemplate>
                                                                                    <HeaderStyle Width="20%" CssClass="GridAlign" />
                                                                                    <ItemStyle Width="20%" HorizontalAlign="left" CssClass="GridAlign" ForeColor="Black" Font-Bold="false" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Maximum Discount %">
                                                                                    <HeaderTemplate>
                                                                                        <asp:Label ID="lbtnDeptdis" Style="padding-left: 15px" runat="server" CssClass="labelgreentitle"
                                                                                            ForeColor="Black">Maximum Discount %</asp:Label>
                                                                                    </HeaderTemplate>
                                                                                    <ItemTemplate>
                                                                                        <asp:TextBox ID="txtDiscount" Enabled="false" onfocus="this.select();" onblur="changeformat(this.id,2);"
                                                                                            runat="server" Style="text-align: right" Width="30%" MaxLength="6" TabIndex="10" Text='<%# Bind("empdisc_discount","{0:###,###,##0.00}") %>'></asp:TextBox>
                                                                                        <ajaxToolkit:FilteredTextBoxExtender ID="filtertxtdiscount" runat="server" Enabled="True"
                                                                                            TargetControlID="txtDiscount" ValidChars=".0123456789">
                                                                                        </ajaxToolkit:FilteredTextBoxExtender>
                                                                                    </ItemTemplate>
                                                                                    <HeaderStyle Width="10%" HorizontalAlign="Right" CssClass="GridAlign" />
                                                                                    <ItemStyle Width="5%" HorizontalAlign="Right" CssClass="GridAlign" ForeColor="Black" Font-Bold="false" />
                                                                                </asp:TemplateField>
                                                                            </Columns>
                                                                        </asp:GridView>
                                                                    </asp:Panel>
                                                                </ContentTemplate>
                                                            </ajax:UpdatePanel>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" valign="middle"><span id="lbl" class="labelgreentitlewithoutbold" runat="server" style="padding-left: 4px; padding-top: 2px">* Please use 'Tab' to move between fields, not enter.</span>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:View>
                                        <asp:View ID="ViewSecurity" runat="server">
                                            <ajax:UpdatePanel ID="upSecurity" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:MultiView ID="mvEmployeeSecurity" runat="server">
                                                        <asp:View ID="viewEmpSecurityOld" runat="server">
                                                            <table width="100%" cellpadding="0" cellspacing="0" style="background-color: #CCE7FF; height: 485px;">
                                                                <tr style="height: 20px; padding-top: 10px;">
                                                                    <td style="padding-left: 12%;">
                                                                        <ajax:UpdateProgress ID="UpdateProgress1" runat="server">
                                                                            <ProgressTemplate>
                                                                                <div align="left">
                                                                                    <img alt="" src="images/spinner.gif" />&nbsp;** Working on your request **
                                                                                </div>
                                                                            </ProgressTemplate>
                                                                        </ajax:UpdateProgress>
                                                                    </td>
                                                                </tr>
                                                                <tr style="height: 200px;">
                                                                    <td width="49%" align="left">
                                                                        <table cellpadding="0" cellspacing="0" width="100%" style="background-color: White;">
                                                                            <tr>
                                                                                <td width="5%"></td>
                                                                                <td width="100%" align="left">
                                                                                    <div id="divTreeview" runat="server" style="overflow: auto; width: 100%; height: 350px; direction: rtl">
                                                                                        <asp:HiddenField ID="hdndivtree" runat="server" />
                                                                                        <div style="direction: ltr;">
                                                                                            <asp:TreeView ID="TreeView1" runat="server" ForeColor="Black" CssClass="labelgreentitlewithoutbold" Width="80%" onmouseover="return getid(event);">
                                                                                                <SelectedNodeStyle BackColor="#0A3E6D" ForeColor="White" CssClass="labelgreentitlewithoutbold" />
                                                                                            </asp:TreeView>
                                                                                        </div>
                                                                                    </div>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                    <td width="2%"></td>
                                                                    <td width="44%" valign="top">
                                                                        <table>
                                                                          <%--  <tr>
                                                                                <td align="left" style="padding-bottom: 3px; padding-left: 10px; padding-top: 10px;"
                                                                                    class="style55">
                                                                                    <span id="Span13" class="labelgreentitle" runat="server">Almost done, our last step is to assign security rights. </span>&nbsp;
                                                                                </td>
                                                                            </tr>--%>
                                                                            <tr>
                                                                                <td align="left" style="padding-bottom: 3px; padding-left: 10px;"
                                                                                    class="style55">
                                                                                    <span id="Span6" class="labelgreentitle" runat="server">Example:  Can 
                                                                                        <asp:Label ID="lblFLName" Font-Bold="true" CssClass="labelgreentitle" runat="server"></asp:Label>
                                                                                        <asp:Label ID="Label5" Font-Bold="true" CssClass="labelgreentitle" runat="server"></asp:Label>
                                                                                        void a transaction?</span>&nbsp;
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="left" style="padding-bottom: 3px; padding-left: 10px; padding-top: 10px;"
                                                                                    class="style55">
                                                                                    <span id="Span14" class="labelgreentitle" runat="server">Simply click on the option you wish to restrict access to.</span>&nbsp;
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="left" style="padding-bottom: 3px; padding-left: 10px;"
                                                                                    class="style55">
                                                                                    <span id="Span15" class="labelgreentitle" runat="server">By default a few items are already marked as no access.</span>&nbsp;
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td></td>
                                                                </tr>
                                                            </table>
                                                        </asp:View>
                                                        <asp:View ID="viewEmpSecurityNew" runat="server">
                                                            <ajax:UpdatePanel ID="UpnlNewSecurity" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <table cellpadding="0" style="background-color: #CCE7FF; height: 485px;" cellspacing="0" width="100%">
                                                                        <tr style="height: 17px; padding-top: 1px;">
                                                                            <td style="padding-top: 3px; padding-left: 10px;">
                                                                                <%--<span class="labelgreentitle" runat="server">Almost done, our last step is to assign security rights. </span>&nbsp;--%>
                                                                                <ajax:UpdateProgress ID="upnlEmpSecurityNew" runat="server">
                                                                                    <ProgressTemplate>
                                                                                        <div align="left">
                                                                                            <img alt="" src="images/spinner.gif" />&nbsp;** Working on your request **
                                                                                        </div>
                                                                                    </ProgressTemplate>
                                                                                </ajax:UpdateProgress>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="padding-top: 1px;">
                                                                                <table cellpadding="0" cellspacing="0" width="100%" border="1" style="border-bottom-width: thick;">
                                                                                    <tr>
                                                                                        <td style="background-color: #CCE7FF; width: 15px;" rowspan="9"></td>
                                                                                        <td colspan="3" style="height: 15px; background-color: #CCE7FF"></td>
                                                                                        <td style="background-color: #CCE7FF; width: 15px;" rowspan="9"></td>
                                                                                        <asp:HiddenField ID="HiddenField1" runat="server" />
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td style="height: 70px; width: 32%; padding-left: 15px; padding-bottom: 15px; background-color: #DCDEFF;">
                                                                                            <asp:LinkButton ID="lnkRingSalesSecurity" runat="server" CssClass="linkHead" Font-Underline="true" Font-Size="12pt" CommandArgument="RS" OnClientClick="openSecurity('RS');" Text="Ring Sales"></asp:LinkButton><br />
                                                                                            <br />
                                                                                            &nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lblRingSalesSecurity" runat="server" Visible="true" Font-Size="11pt" CssClass="labelall"></asp:Label>
                                                                                        </td>
                                                                                        <td style="height: 70px; width: 32%; padding-left: 15px; padding-bottom: 15px; background-color: #DCDEFF;">
                                                                                            <asp:LinkButton ID="lnkInventorySecurity" runat="server" CssClass="linkHead" Font-Underline="true" Font-Size="12pt" CommandArgument="IN" OnClientClick="openSecurity('IN');" Text="Inventory"></asp:LinkButton><br />
                                                                                            <br />
                                                                                            &nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lblInventorySecurity" runat="server" Visible="true" Font-Size="11pt" CssClass="labelall"></asp:Label>
                                                                                        </td>
                                                                                        <td style="height: 70px; width: 32%; padding-left: 15px; padding-bottom: 15px; background-color: #DCDEFF;">
                                                                                            <asp:LinkButton ID="lnkCustomersSecurity" runat="server" CssClass="linkHead" Font-Underline="true" Font-Size="12pt" CommandArgument="CU" OnClientClick="openSecurity('CU');" Text="Customers"></asp:LinkButton><br />
                                                                                            <br />
                                                                                            &nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lblCustomersSecurity" runat="server" Visible="true" Font-Size="11pt" CssClass="labelall"></asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td colspan="3" style="height: 15px; background-color: #CCE7FF"></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td style="height: 70px; width: 32%; padding-left: 15px; padding-bottom: 15px; background-color: #DCDEFF;">
                                                                                            <asp:LinkButton ID="lnkVendorsSecurity" runat="server" CssClass="linkHead" Font-Underline="true" Font-Size="12pt" CommandArgument="VN" OnClientClick="openSecurity('VN');" Text="Vendors"></asp:LinkButton><br />
                                                                                            <br />
                                                                                            &nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lblVendorsSecurity" runat="server" Visible="true" Font-Size="11pt" CssClass="labelall"></asp:Label>
                                                                                        </td>
                                                                                        <td style="height: 70px; width: 32%; padding-left: 15px; padding-bottom: 15px; background-color: #DCDEFF;">
                                                                                            <asp:LinkButton ID="lnkEmployeesSecurity" runat="server" CssClass="linkHead" Font-Underline="true" Font-Size="12pt" CommandArgument="EM" OnClientClick="openSecurity('EM');" Text="Employees"></asp:LinkButton><br />
                                                                                            <br />
                                                                                            &nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lblEmployeesSecurity" runat="server" Visible="true" Font-Size="11pt" CssClass="labelall"></asp:Label>
                                                                                        </td>
                                                                                        <td style="height: 70px; width: 32%; padding-left: 15px; padding-bottom: 15px; background-color: #DCDEFF;">
                                                                                            <asp:LinkButton ID="lnkTodaysSalesSecurity" runat="server" CssClass="linkHead" Font-Underline="true" Font-Size="12pt" CommandArgument="TS" OnClientClick="openSecurity('TS');" Text="Today's Sales"></asp:LinkButton><br />
                                                                                            <br />
                                                                                            &nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lblTodaysSalesSecurity" runat="server" Visible="true" Font-Size="11pt" CssClass="labelall"></asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td colspan="3" style="height: 15px; background-color: #CCE7FF"></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td style="height: 70px; width: 32%; padding-left: 15px; padding-bottom: 15px; background-color: #DCDEFF;">
                                                                                            <asp:LinkButton ID="lnkCloseThDaySecurity" runat="server" CssClass="linkHead" Font-Underline="true" Font-Size="12pt" CommandArgument="CD" OnClientClick="openSecurity('CD');" Text="Close The Day"></asp:LinkButton><br />
                                                                                            <br />
                                                                                            &nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lblCloseThDaySecurity" runat="server" Visible="true" Font-Size="11pt" CssClass="labelall"></asp:Label>
                                                                                        </td>
                                                                                        <td style="height: 70px; width: 32%; padding-left: 15px; padding-bottom: 15px; background-color: #DCDEFF;">
                                                                                            <asp:LinkButton ID="lnkReportsSecurity" runat="server" CssClass="linkHead" Font-Underline="true" Font-Size="12pt" CommandArgument="RE" OnClientClick="openSecurity('RE');" Text="Reports"></asp:LinkButton><br />
                                                                                            <br />
                                                                                            &nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lblReportsSecurity" runat="server" Visible="true" Font-Size="11pt" CssClass="labelall"></asp:Label>
                                                                                        </td>
                                                                                        <td style="height: 70px; width: 32%; padding-left: 15px; padding-bottom: 15px; background-color: #DCDEFF;">
                                                                                            <asp:LinkButton ID="lnkMarketingCenterSecurity" runat="server" CssClass="linkHead" Font-Underline="true" Font-Size="12pt" CommandArgument="MC" OnClientClick="openSecurity('MC');" Text="Marketing Center"></asp:LinkButton><br />
                                                                                            <br />
                                                                                            &nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lblMarketingCenterSecurity" runat="server" Visible="true" Font-Size="11pt" CssClass="labelall"></asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td colspan="3" style="height: 15px; background-color: #CCE7FF"></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td style="height: 70px; width: 32%; padding-left: 15px; padding-bottom: 15px; background-color: #DCDEFF;">
                                                                                            <asp:LinkButton ID="lnkAdministrationSecurity" runat="server" CssClass="linkHead" Font-Underline="true" Font-Size="12pt" CommandArgument="AD" OnClientClick="openSecurity('AD');" Text="Administration"></asp:LinkButton><br />
                                                                                            <br />
                                                                                            &nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lblAdministrationSecurity" runat="server" Visible="true" Font-Size="11pt" CssClass="labelall"></asp:Label>
                                                                                        </td>
                                                                                        <td style="height: 70px; width: 32%; padding-left: 15px; padding-bottom: 15px; background-color: #DCDEFF;">
                                                                                            <asp:LinkButton ID="lnkDataMaintenanceSecurity" runat="server" CssClass="linkHead" Font-Underline="true" Font-Size="12pt" CommandArgument="DM" OnClientClick="openSecurity('DT');" Text="Data Maintenance"></asp:LinkButton><br />
                                                                                            <br />
                                                                                            &nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lblDataMaintenanceSecurity" runat="server" Visible="true" Font-Size="11pt" CssClass="labelall"></asp:Label>
                                                                                        </td>
                                                                                        <td style="height: 70px; width: 32%; padding-left: 15px; padding-bottom: 15px; background-color: #DCDEFF;">
                                                                                            <asp:LinkButton ID="lnkMiscellaneousSecurity" runat="server" CssClass="linkHead" Font-Underline="true" Font-Size="12pt" CommandArgument="Mis" OnClientClick="openSecurity('Mis');" Text="Miscellaneous"></asp:LinkButton><br />
                                                                                            <br />
                                                                                            &nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lblMiscellaneousSecurity" runat="server" Visible="true" Font-Size="11pt" CssClass="labelall"></asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td colspan="3" style="height: 15px; background-color: #CCE7FF"></td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </ContentTemplate>
                                                            </ajax:UpdatePanel>
                                                        </asp:View>
                                                    </asp:MultiView>
                                                </ContentTemplate>
                                            </ajax:UpdatePanel>
                                        </asp:View>


                                        <asp:View ID="ViewkeySecurity" runat="server">
                                            <table width="100%" cellpadding="0" cellspacing="0" style="background-color: #CCE7FF; height: 485px; vertical-align: top;">
                                                <tr style ="vertical-align :top;">
                                                    <td>
                                                        <table width="100%" cellpadding="0" cellspacing="0" style="background-color: #CCE7FF; vertical-align: top;">

                                                            <tr>

                                                                <td align="left" style="padding-bottom: 3px; padding-left: 10px;" valign="top"
                                                                    class="style53">

                                                                    <span runat="server" id="divkeySecurity" class="labelgreentitle"></span>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" style="padding-bottom: 3px; padding-left: 10px;" valign="top">
                                                                    <asp:GridView ID="gvevent" runat="server" AutoGenerateColumns="False" Width="98%"
                                                                        BorderColor="White" BorderWidth="1px" ShowHeader="false" AllowSorting="True"
                                                                        AllowPaging="True" EmptyDataText="No Matching Data Found">
                                                                        <PagerStyle HorizontalAlign="Right" VerticalAlign="Middle" />
                                                                        <SelectedRowStyle CssClass="selectrow" />
                                                                        <HeaderStyle CssClass="header" HorizontalAlign="Left" VerticalAlign="Middle" />
                                                                        <AlternatingRowStyle CssClass="alterrow" />
                                                                        <RowStyle CssClass="row" />
                                                                        <EmptyDataRowStyle ForeColor="Red" HorizontalAlign="Center" />
                                                                        <Columns>
                                                                            <asp:TemplateField>
                                                                                <ItemTemplate>
                                                                                    <asp:CheckBox ID="chkSelect" runat="server" />
                                                                                    <div style="display: none">
                                                                                        <asp:Label ID="lblevent_id" runat="server" Text='<%# Bind("Event_id") %>'></asp:Label>
                                                                                    </div>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle Width="2%" HorizontalAlign="center" CssClass="GridAlign" />
                                                                                <ItemStyle Width="2%" HorizontalAlign="center" VerticalAlign="Top" CssClass="WhiteBorder" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Event Name">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblEvent_name" runat="server" Text='<%# Bind("Event_name") %>'></asp:Label>
                                                                                    <asp:Label ID="lblclosetheday" runat="server" Text=""></asp:Label>
                                                                                    <br />
                                                                                    <asp:CheckBox ID="chkEmpSummary" runat="server" Style="padding-left: 10px" Visible="false" />
                                                                                    <asp:Label ID="lblEmpSummaryEvent_name" runat="server" Visible="false" Text='' ></asp:Label>
                                                                                    <div style="display: none">
                                                                                        <asp:Label ID="lblEmpSummaryevent_id" runat="server" Text='<%# Bind("Event_id") %>'></asp:Label>
                                                                                    </div>
                                                                                </ItemTemplate>
                                                                                <HeaderTemplate>
                                                                                    <asp:LinkButton ID="lbtnEmpid" runat="server" CommandArgument="employee_id" CommandName="Sort"
                                                                                        ForeColor="White">Event Name</asp:LinkButton><asp:ImageButton ID="imgEmpid" runat="server" />
                                                                                </HeaderTemplate>
                                                                                <HeaderStyle HorizontalAlign="Left" CssClass="GridAlign" />
                                                                                <ItemStyle HorizontalAlign="left" CssClass="GridAlign" />
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                    </asp:GridView>
                                                                </td>
                                                            </tr>
                                                            <asp:Panel runat="server" ID="pnlLighning" Visible="false">
                                                                <tr>

                                                                    <td align="left" style="padding-bottom: 3px; padding-left: 10px;" valign="top">
                                                                        <asp:CheckBox runat="server" ID="chk1" Text="Return" CssClass="labelgreentitle" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" style="padding-bottom: 3px; padding-left: 10px;" valign="top">
                                                                        <asp:CheckBox runat="server" ID="CheckBox1" Text="Void" CssClass="labelgreentitle" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" style="padding-bottom: 3px; padding-left: 10px;" valign="top">
                                                                        <asp:CheckBox runat="server" ID="CheckBox2" Text="Close the Day" CssClass="labelgreentitle" />
                                                                        <br />
                                                                        <asp:Label runat="server" Text="   (Sales figures, total customers, average invoice, etc.) " class="labelgreentitle" Style="padding-left: 20px;"></asp:Label>
                                                                        <br />
                                                                        <asp:CheckBox runat="server" ID="CheckBox4" Text="Include Employee Summary" CssClass="labelgreentitle" Style="padding-left: 20px;" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" style="padding-bottom: 3px; padding-left: 10px;" valign="top">
                                                                        <asp:CheckBox runat="server" ID="CheckBox3" Text="When an employee is added" CssClass="labelgreentitle" />
                                                                        <br />
                                                                        <asp:Label ID="Label2" runat="server" Text="ETC. " class="labelgreentitle"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </asp:Panel>

                                                            <asp:Panel runat="server" ID="PnlCorporate" Visible="false">

                                                                <tr>
                                                                    <td align="left" style="padding-bottom: 3px; padding-left: 10px;" valign="top">
                                                                        <asp:CheckBox runat="server" ID="CheckBox9" Text="When an employee is added" CssClass="labelgreentitle" />
                                                                        <br />
                                                                        <asp:Label ID="Label6" runat="server" Text="ETC. " class="labelgreentitle"></asp:Label><br />
                                                                        <asp:Label ID="Label7" runat="server" Text="ETC. " class="labelgreentitle">
                                                             <b>NOTE:</b>  The following options don’t apply in the corporate office so please don’t display them. Return, Void, Close the Day, Include Employee Summary,

                                                                        </asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </asp:Panel>

                                                        </table>
                                                    </td>
                                                </tr>
                                                
                                            </table>
                                        </asp:View>

                                        <asp:View ID="ViewConfirmation" runat="server">
                                            <table width="100%" cellpadding="0" cellspacing="0" style="background-color: #CCE7FF; height: 485px;">
                                                <tr>
                                                    <td colspan="2" style="height: 10px; padding-left: 10px; padding-top: 10px;">
                                                        <span id="Span16" class="labelgreentitle" runat="server">Congratulations, a new employee has been added to Lightning Online Point of Sale.</span>&nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" style="padding-bottom: 10px; padding-left: 10px; padding-top: 10px;">
                                                        <span id="Span17" class="labelgreentitle" style="font-weight: bold;" runat="server">Recap:</span>&nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" align="left" style="padding-bottom: 3px; padding-left: 20px" valign="top"></span> <span class="labelgreentitlewithoutbold" style="font-weight: bold;">Employee ID:</span>
                                                        <asp:Label ID="lblEmpIDValue" CssClass="labelgreentitlewithoutbold" runat="server" Font-Bold="false" Text="Name"></asp:Label>
                                                    </td>

                                                </tr>
                                                <tr>
                                                    <td colspan="2" align="left" style="padding-bottom: 3px; padding-left: 20px" valign="top"></span> <span class="labelgreentitlewithoutbold" style="font-weight: bold;">Name:</span>
                                                        <asp:Label ID="lblEmpName" CssClass="labelgreentitlewithoutbold" runat="server" Font-Bold="false" Text="Name"></asp:Label>
                                                    </td>

                                                </tr>
                                                <tr>
                                                    <td colspan="2" align="left" style="padding-bottom: 3px; padding-left: 20px" valign="top"></span> <span class="labelgreentitlewithoutbold" style="font-weight: bold;">Mobile:</span>
                                                        <asp:Label ID="lblEmpMobile" CssClass="labelgreentitlewithoutbold" Font-Bold="false" runat="server" Text="Mobile"></asp:Label>
                                                    </td>

                                                </tr>
                                                <tr>
                                                    <td colspan="2" align="left" style="padding-bottom: 3px; padding-left: 20px" valign="top"></span> <span class="labelgreentitlewithoutbold" style="font-weight: bold;">Email:</span>
                                                        <asp:Label ID="lblEmpEmail" CssClass="labelgreentitlewithoutbold" Font-Bold="false" runat="server" Text="Email"></asp:Label>
                                                    </td>

                                                </tr>
                                                <tr>
                                                    <td align="left" colspan="2" style="padding-bottom: 3px; padding-left: 20px; width: 10%;" valign="top"></span> 
                                                        <span class="labelgreentitle" style="font-weight: bold;">Discount Limits have been applied.</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" style="width: 40%; padding-left: 20px; padding-bottom: 3px;" valign="top">
                                                        <span id="Span18" class="labelgreentitle" runat="server" style="font-weight: bold;">A temporary password has been established.</span>&nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" style="padding-bottom: 8px; width: 40%; padding-left: 20px" valign="top">
                                                        <span id="Span19" class="labelgreentitle" runat="server" style="font-weight: bold;">Security rights are set.</span>&nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" style="padding-left: 10px; padding-top: 5px; width: 40%; padding-left: 20px" valign="top">
                                                        <span id="Span20" class="labelgreentitle" runat="server">Remember, when an employee 
                                                        </span>
                                                        <asp:Label ID="lblEmpNameEnd" CssClass="labelgreentitle" runat="server"></asp:Label>
                                                        <span id="spntolog" class="labelgreentitle" runat="server">logs in for the first time the system will prompt for a new password. </span>&nbsp;
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:View>
                                        <asp:View ID="ViewStorelist" runat="server">
                                            <table style="width: 100%; padding-top: 5px;" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <div id="Div2" runat="server" style="cursor: pointer; width: 100%; background-color: #CCE7FF; height: 485px;">
                                                            <table style="width: 100%;" cellpadding="0" cellspacing="0">
                                                                <tr>
                                                                    <td height="27" background="images/popup_01.gif" valign="middle">
                                                                        <asp:CheckBox ID="chkMainHeader" onclick="javascript:selectallstore();HeaderClick(this);" runat="server" Height="15" Width="17" />
                                                                        <asp:Label ID="lblSelectallstore" CssClass="stylePopupHeader" runat="server" Text="Select/ Unselect all stores"></asp:Label>
                                                                    </td>

                                                                </tr>
                                                            </table>
                                                            <asp:GridView ID="grdStores" runat="server" AllowSorting="True" AutoGenerateColumns="False" DataKeyNames="storeno"
                                                                BorderColor="White" PageSize="14" BorderWidth="1px" Width="100%" EmptyDataText="No matching records found." AllowPaging="true">
                                                                <SelectedRowStyle CssClass="selectrow" />
                                                                <HeaderStyle CssClass="header" HorizontalAlign="Left" VerticalAlign="Middle" />
                                                                <AlternatingRowStyle CssClass="alterrow" />
                                                                <PagerSettings Mode="numeric" />
                                                                <PagerStyle HorizontalAlign="Right" />
                                                                <RowStyle CssClass="row" />
                                                                <Columns>
                                                                    <asp:TemplateField>
                                                                        <HeaderTemplate>
                                                                            <asp:CheckBox ID="chkBxHeader" onclick="javascript:HeaderClick(this);" runat="server" Style="display: none" />
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="chkSelect" runat="server" onclick="javascript:ChildClick(this);" />
                                                                        </ItemTemplate>
                                                                        <HeaderStyle Width="2%" />
                                                                        <ItemStyle Width="2%" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Store #">
                                                                        <ItemTemplate>
                                                                            &nbsp;<asp:Label ID="lblStoreNo" runat="server" Text='<%# Bind("storeno")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderTemplate>
                                                                            &nbsp;<asp:LinkButton ID="lbtnStoreNo" runat="server" CommandArgument="storeno"
                                                                                CommandName="Sort" ForeColor="White">Store #</asp:LinkButton>
                                                                            <asp:ImageButton ID="imgStoreno" runat="server" />
                                                                        </HeaderTemplate>
                                                                        <HeaderStyle Width="16%" HorizontalAlign="Left" />
                                                                        <ItemStyle Width="16%" HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Store Name">
                                                                        <ItemTemplate>
                                                                            &nbsp;<asp:Label ID="lblStoreName" runat="server" Text='<%# Bind("name")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderTemplate>
                                                                            &nbsp;<asp:LinkButton ID="lbtnStorreName" runat="server" CommandArgument="name" CommandName="Sort"
                                                                                ForeColor="White">Store Name</asp:LinkButton>
                                                                            <asp:ImageButton ID="imgName" runat="server" />
                                                                        </HeaderTemplate>
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="City, State">
                                                                        <ItemTemplate>
                                                                            &nbsp;<asp:Label ID="lblStoreCity" runat="server" Text='<%# Bind("City_St")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderTemplate>
                                                                            &nbsp;<asp:LinkButton ID="lbtnStoreCity" runat="server" CommandArgument="city_st" CommandName="Sort"
                                                                                ForeColor="White">City, State</asp:LinkButton>
                                                                            <asp:ImageButton ID="imgCity" runat="server" />
                                                                        </HeaderTemplate>
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>

                                                                </Columns>
                                                            </asp:GridView>
                                                        </div>

                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:View>
                                    </asp:MultiView>
                                    <div id="Div1" if="footer" runat="server">
                                        <table width="100%" border="0" align="center" style="background-color: #CCE7FF" cellpadding="0"
                                            cellspacing="0">
                                            <tr>
                                                <td align="right" valign="bottom" class="style1" style="padding-bottom: 10px; padding-right: 10px;">
                                                    <asp:Button ID="btnBack" runat="server" Text="  Back  " Font-Bold="true" Style="cursor: pointer;"
                                                        Height="28px" BackColor="#B6E0F7" TabIndex="12" />
                                                    <asp:Button ID="btnCntinueSave" runat="server" Text="Continue / Save" Font-Bold="true"
                                                        Style="cursor: pointer;" Height="28px" BackColor="#B6E0F7" TabIndex="11" ValidationGroup="EmpAdd" />
                                                    <asp:Button ID="btnCancel" runat="server" Text=" Cancel " Font-Bold="true" Style="cursor: pointer;"
                                                        Height="28px" BackColor="#B6E0F7" TabIndex="13" />
                                                    <asp:Button ID="btnFinish" runat="server" Text=" Finish " Font-Bold="true" Style="cursor: pointer;"
                                                        Height="28px" BackColor="#B6E0F7" Visible="false" TabIndex="14" OnClientClick="CloseWindow();" />
                                                    <asp:HiddenField ID="hdnfirstname" runat="server" />
                                                    <asp:HiddenField ID="hdnlastname" runat="server" />
                                                    <asp:HiddenField ID="hdnmobile" runat="server" />
                                                    <asp:HiddenField ID="hdnMbBefore" runat="server" />
                                                    <asp:HiddenField ID="hdnfirstnameBefore" runat="server" />
                                                    <asp:HiddenField ID="hdnlastnameBefore" runat="server" />
                                                    <asp:HiddenField ID="hdnEmailID" runat="server" />
                                                    <asp:HiddenField ID="hdnPosition" runat="server" />
                                                    <asp:Button ID="btn1" runat="server" Style="display: none" />
                                                    <asp:HiddenField ID="hdfTree" runat="server" />
                                                    <asp:HiddenField ID="hdnPassword" runat="server" />
                                                    <asp:HiddenField ID="hdnEmpid" runat="server" />
                                                    <asp:HiddenField ID="hdnEmployeeId" runat="server" />
                                                    <asp:HiddenField ID="hdnCheckDup" runat="server" />
                                                    <asp:HiddenField ID="hdnSelectedNode" runat="server" />
                                                    <input id="hdnchange" type="hidden" value="F" runat="server" />
                                                    <input id="hdnchangesave" type="hidden" runat="server" />
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </td>
                </tr>
            </table>

            <div>
                <%--****************************************  Modal POPUP   ********************************************--%>
                <ajaxToolkit:ModalPopupExtender ID="MPEEmployeeCheck" runat="server" PopupDragHandleControlID="pnlEmployeeCheck"
                    RepositionMode="None" TargetControlID="btnEmployeeCheck" PopupControlID="pnlEmployeeCheck"
                    BackgroundCssClass="modalBackground" DropShadow="false" />
                <asp:Button ID="btnEmployeeCheck" Style="display: none;" runat="server" />
                <asp:Panel ID="pnlEmployeeCheck" CssClass="modalPopup" Style="display: none; width: 433px;"
                    runat="server" Width="100%">
                    <ajax:UpdatePanel ID="updEmployeeCheck" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table style="background-color: #ffffff; width: 433px;" cellpadding="0" cellspacing="0">
                                <tr>
                                    <%--<td class="popupheader"></td>--%>
                                    <td class="popupheader">&nbsp;<asp:Label ID="lblExitheader" runat="server" Font-Bold="True" Font-Names="Arial"
                                        Font-Size="12px" ForeColor="White" Text="Message"></asp:Label>
                                    </td>
                                    <td class="popupheader"></td>
                                </tr>
                                <tr>
                                    <%--<td>&nbsp;</td>--%>
                                    <td>&nbsp;
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <%--<td></td>--%>
                                    <td align="left">&nbsp;
                                <asp:Label ID="lblDuplicateEmployee" runat="server" CssClass="stylePopuplabel" Font-Bold="True"
                                    Font-Names="Arial"></asp:Label>
                                        <asp:Label ID="lblEmployeeOnFile" runat="server" CssClass="stylePopuplabel" Font-Bold="True"
                                            Font-Names="Arial"></asp:Label>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr runat="server" id="statusmain" visible="false">
                                    <%--<td></td>--%>
                                    <td align="left">
                                        <asp:Label ID="lblAnotherEmployee" runat="server" CssClass="stylePopuplabel" Font-Bold="True"
                                            Font-Names="Arial" Font-Size="10pt" Text="Please select another employee id."></asp:Label>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr runat="server" id="statusmain2" visible="false">
                                    <%--<td></td>--%>
                                    <td align="left">
                                        <asp:Label ID="Label13" runat="server" CssClass="stylePopuplabel" Font-Bold="True"
                                            Font-Names="Arial" Font-Size="10pt" Text="Please select another email id."></asp:Label>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <%--<td>&nbsp;</td>--%>
                                    <td>&nbsp;
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <%-- <td></td>--%>
                                    <td align="center">
                                        <asp:ImageButton ID="imgbtnContinue" runat="server" ImageUrl="~/white_images/continue.gif"
                                            OnClick="imgbtnContinue_Click" />
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <%--<td>
                        &nbsp;</td>--%>
                                    <td align="center">&nbsp;
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </ajax:UpdatePanel>
                </asp:Panel>
            </div>
        </div>
        <%--------------------------------------------------------- Modal Popup for Help ----------------------------------------------------%>
        <ajaxToolkit:ModalPopupExtender ID="mpopupHelp" runat="server" TargetControlID="btnDisplayHelp"
            PopupDragHandleControlID="pnlDisplayHelp" RepositionMode="None" PopupControlID="pnlDisplayHelp"
            BackgroundCssClass="modalBackground" DropShadow="false" />
        <asp:Button ID="btnDisplayHelp" Style="display: none;" runat="server" />
        <asp:Panel ID="pnlDisplayHelp" Style="display: none; width: 452px;" runat="server"
            Width="155px">
            <ajax:UpdatePanel ID="upnlDisplayHelp" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <UC:Help runat="server" ID="uchelpresetpwd" />
                </ContentTemplate>
            </ajax:UpdatePanel>
        </asp:Panel>
    </form>

    <script type="text/javascript">

        function SetFocus() {
            setTimeout("document.getElementById('<%=txtEmpId.ClientId %>').focus()", 500);
        }

        function setCursor(El, Position) {
            El.selectionStart = Position;
            El.selectionEnd = Position;
        }

        function changecolor(element, from) {
            if (from == 'focus')
                element.style.backgroundColor = "#F4FA58";
            else
                element.style.backgroundColor = "White";
        }

        function ShowModalPopup() {
            var modal = $find('MPEEmployeeCheck');
            modal.show();
        }

        function checkUser() {
            var ID;
            ID = document.getElementById('<%=txtEmpId.ClientID%>').value;
            if (ID != '') {
                PageMethods.GetEmployeeInfo(ID, onTrue, onFalse);
            }
            return false;
        }
        function onTrue(result, userContext, methodName) {
            if (result == -1) {
                alert("I'm sorry, but the Employee ID you entered is reserved for another user, not necessary associated with your store. Please try again.");
                var txtusename = document.getElementById('<%=txtEmpId.ClientID%>');
                txtusename.value = '';
                txtusename.focus();
                return false;
            }
        }
        function onFalse() {
        }

        function GeneratePass() {
            if (document.getElementById('<%= chkSuggestTmpPwd.ClientID %>').checked == true) {
                var name = "";

                var firstname = document.getElementById('<%= hdnfirstname.ClientID %>').value.toString();
                var lastname = document.getElementById('<%= hdnlastname.ClientID %>').value;
                var mobile = document.getElementById('<%= hdnmobile.ClientID %>').value;

                var mainFname = "";
                var mainLname = "";
                var mainPhone = "";
                for (var i = 0; i < firstname.length; i++) {
                    if (firstname[i] != "o" && firstname[i] != "l")
                        mainFname += firstname[i];
                }
                for (var i = 0; i < lastname.length; i++) {
                    if (lastname[i] != "o" && lastname[i] != "l")
                        mainLname += lastname[i];
                }
                var count = 0;
                for (var i = 0; i < mobile.length; i++) {
                    if (mobile[i] != "0" && mobile[i] != "1")
                        mainPhone += mobile[i];
                    count = count + 1;
                }

                mainPhone = mainPhone.substring(count, mainPhone.length - 2);
                name = mainFname + mainLname;
                var temppass = name.substring(0, 4) + mainPhone + "$";
                // var ans = confirm("Suggested Password is '" + temppass + "'. Do you want to keep this password?");


                document.getElementById('<%= txtPassword.ClientID %>').value = temppass;
                document.getElementById('<%= txtConfirmPassword.ClientID %>').value = temppass;

            }
        }
        function getCitystate() {

            document.getElementById('<%=txtZip.ClientID %>').style.backgroundColor = "White";
            var country = '<%= ViewState("country") %>';

            var zip = document.getElementById('<%= txtZip.ClientID%>');

            switch (country) {
                case '':
                    break;
                case '0':

                    PageMethods.getCitystate(zip.value, country, OnSucceeded, OnFailed);


                    break;
                case '1':
                    PageMethods.getCitystate(zip.value, country, OnSucceeded, OnFailed);
                    break;
            }
        }

        function OnSucceeded(result, userContext, methodName) {

            if (methodName == "getCitystate") {
                var State = document.getElementById('<%= txtState.ClientID%>');
                var city = document.getElementById('<%= txtCity.ClientID%>');
                var hideCityState = document.getElementById('<%= hdnCityState.ClientID%>');

                if (result == 'N') {
                    city.value = '';
                    State.value = '';
                    hideCityState.value = '';
                }
                else {
                    var cs = result.split(',');

                    hideCityState.value = result;
                    city.value = cs[0];
                    State.value = cs[1];
                }
            }
        }

        function OnFailed(error, userContext, methodName) {
            if (error !== null) {
                alert(error.get_message());
            }
        }

        function fncInputNumericValuesOnly(evt) {
            var e = evt ? evt : window.event;
            if (window.event) {
                if ((evt.keyCode != 8) && (evt.keyCode < 48 || evt.keyCode > 57)) {
                    evt.returnValue = false;
                    evt.preventDefault();
                }
            }
            else {
                if ((evt.which != 0) && (evt.which != 8) && (evt.which < 48 || evt.which > 57)) {
                    evt.returnValue = false;
                    evt.preventDefault();
                }
            }
        }



        function changeformat(id, intPrecision) {


            var price = document.getElementById(id);
            if (price.value > 100.00) {
                price.value = "100.00";
            }
            var num = parseFloat(price.value).toFixed(intPrecision);
            //alert(num);
            if (isNaN(num)) {
                price.value = parseFloat(0, intPrecision);
            }

            else {
                price.value = num;
            }
            return false;
        }
        function OnTreeClick(evt) {


            var src = window.event != window.undefined ? window.event.srcElement : evt.target;
            var nodeClick = src.tagName.toLowerCase() == "a";
            if (src.tagName.toLowerCase() == "img") {
                src = src.parentElement;
            }
            document.getElementById('<%=hdnSelectedNode.ClientID%>').value = src.id
            if (nodeClick) {
                //innerText works in IE but fails in Firefox (I'm sick of browser anomalies), so use innerHTML as well
                var nodeText = src.innerText || src.innerHTML;
                var nodeValue = GetNodeValue(src);
                if (nodeValue == 'MO1' || nodeValue == 'MO2' || nodeValue == 'MO3' || nodeValue == 'MO4' || nodeValue == 'MO5' || nodeValue == 'MO6' || nodeValue == 'MO8'
                || nodeValue == 'MO9' || nodeValue == 'MO10' || nodeValue == 'MO11' || nodeValue == 'MO13' || nodeValue == 'RE1') {
                    var msgconfirm = confirm("By clicking on the parent item this will set all of the sub-options \n to access or no access for this employee.", "Please Confirm");
                    return msgconfirm
                }
                else {
                    return true;
                }
                //alert("Text: " + nodeText + "," + "Value: " + nodeValue);
            }
            return true; //comment this if you want postback on node click
        }

        function GetNodeValue(node) {
            var nodeValue = "";
            var nodePath = node.href.substring(node.href.indexOf(",") + 2, node.href.length - 2);
            var nodeValues = nodePath.split("\\");
            if (nodeValues.length > 1)
                nodeValue = nodeValues[nodeValues.length - 1];
            else
                nodeValue = nodeValues[0].substr(1);

            return nodeValue;
        }

        var q;
        function getid(e) {

            q = e.clientX;
            document.getElementById('<%=hdfTree.ClientId %>').value = q;

        }

        function comparePass(s, e) {
            var passid = document.getElementById('<%=txtPassword.ClientId %>').value;
            var conpassid = document.getElementById('<%=txtConfirmPassword.ClientId %>').value;

            if (passid != conpassid) {
                alert('Password and Confirm password should be same!');
                //document.getElementById('<%=txtPassword.ClientId %>').value = "";
                document.getElementById('<%=txtPassword.ClientId %>').select();
                document.getElementById('<%=txtPassword.ClientId %>').focus();
                e.IsValid = false;
            }
            else {
                e.IsValid = true;
            }
        }

        function CheckValue() {
            var email = document.getElementById('<%=txtPassword.ClientId %>').value;
            var phone = document.getElementById('<%=txtPassword.ClientId %>').value;
        }

        function CloseWindow() {
            window.parent.BindGridEmp();
            return false;
        }

        function EmailValidation(evt) {

            var charCode = (evt.which) ? evt.which : event.keyCode;

            if ((charCode == 32)) {
                return false;
            }
        }
    </script>

</body>

<script type="text/javascript">
    var placeSearch, autocomplete;
    var componentForm = {
        street_number: 'short_name',
        route: 'long_name',
        locality: 'long_name',
        administrative_area_level_1: 'short_name',
        postal_code: 'short_name'
    };

    function initAutocomplete() {
        // Create the autocomplete object, restricting the search to geographical
        // location types.
        autocomplete = new google.maps.places.Autocomplete(
            /** @type {!HTMLInputElement} */(document.getElementById('<%= txtaddress.ClientID%>')),
                  { types: ['geocode'] });

        // When the user selects an address from the dropdown, populate the address
        // fields in the form.
        autocomplete.addListener('place_changed', fillInAddress);
    }

    function fillInAddress() {
        // Get the place details from the autocomplete object.
        var place = autocomplete.getPlace();

        for (var component in componentForm) {
            if (component == 'street_number')
                component = '<%= txtaddress.ClientID%>';
            else if (component == 'route')
                component = '<%= txtaddress.ClientID%>';
            else if (component == 'locality')
                component = '<%= txtCity.ClientID%>';
            else if (component == 'administrative_area_level_1')
                component = '<%= txtState.ClientID%>';
                  else if (component == 'postal_code')
                      component = '<%= txtZip.ClientID%>';
    document.getElementById(component).value = '';
            //document.getElementById(component).disabled = false;
}

        // Get each component of the address from the place details
        // and fill the corresponding field on the form.
    for (var i = 0; i < place.address_components.length; i++) {
        var addressType = place.address_components[i].types[0];
        if (componentForm[addressType]) {
            var val = place.address_components[i][componentForm[addressType]];
            if (addressType == 'street_number')
                addressType = '<%= txtaddress.ClientID%>';
            else if (addressType == 'route')
                addressType = '<%= txtaddress.ClientID%>';
            else if (addressType == 'locality')
                addressType = '<%= txtCity.ClientID%>';
            else if (addressType == 'administrative_area_level_1')
                addressType = '<%= txtState.ClientID%>';
            else if (addressType == 'postal_code')
                addressType = '<%= txtZip.ClientID%>';



            // if (addressType == '<%= txtaddress.ClientID%>' & document.getElementById('<%= txtaddress.ClientID%>').value == "") {
            //     document.getElementById('txtaddress').value = val;
            //
            // }
            // else if (addressType == '<%= txtaddress.ClientID%>' & document.getElementById('<%= txtaddress.ClientID%>').value != "") {
            //     document.getElementById('<%= txtaddress.ClientID%>').value += ',' + val;
            // }
            // else {
            // document.getElementById(addressType).value = val;
            //}

            if (addressType == '<%= txtAddress.ClientID%>' & document.getElementById('<%= txtAddress.ClientID%>').value == "") {
                document.getElementById('<%= txtAddress.ClientID%>').value = val;

            }
            else if (addressType == '<%= txtAddress.ClientID%>' & document.getElementById('<%= txtAddress.ClientID%>').value != "") {
                document.getElementById('<%= txtAddress.ClientID%>').value += ' ' + val;
            }
            else {
                document.getElementById(addressType).value = val;
            }

    }
}
}

// Bias the autocomplete object to the user's geographical location,
// as supplied by the browser's 'navigator.geolocation' object.

function geolocate() {

    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(function (position) {
            var geolocation = {
                lat: position.coords.latitude,
                lng: position.coords.longitude
            };
            var circle = new google.maps.Circle({
                center: geolocation,
                radius: position.coords.accuracy
            });
            autocomplete.setBounds(circle.getBounds());
        });
    }
}

</script>
<script src="https://maps.googleapis.com/maps/api/js?key=AIzaSyASEp2onrkLsqGpONGfjC6ezqZtkArXmfI&libraries=places"
    async defer></script>
</html>
