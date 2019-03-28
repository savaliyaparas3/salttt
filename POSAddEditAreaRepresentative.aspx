<%@ Page Language="VB" AutoEventWireup="false" CodeFile="POSAddEditAreaRepresentative.aspx.vb" Inherits="POSAddEditAreaRepresentative" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Area Representative</title>
    <link href="1style.css" rel="stylesheet" type="text/css" />
    <link href="CSS/Mainstyle.css" rel="stylesheet" type="text/css" />
    <link href="CSS/ringsalestyle.css" rel="stylesheet" type="text/css" />
  

    <script type="text/javascript">
       

        function changecolor1(id) {
            var tbox = document.getElementById(id);
            tbox.style.backgroundColor = "Yellow";
            return false;
        }

        function normalcolor(id) {
            var tbox = document.getElementById(id);
            tbox.style.backgroundColor = "White";
            return false;
        }
        function getfloatwithDollar(id, intPrecision) {


            var price = document.getElementById(id);
            var num = parseFloat(price.value).toFixed(intPrecision);
            if (isNaN(num)) {
                price.value = parseFloat(0, intPrecision);
            }

            else {
                price.value = num;
            }
            if (price.value > 99.99) {
                price.value = 99.99;
            }
            return false;
        }
        function OnlyNumeric(evt, objID) {

            var e = evt ? evt : window.event;
            var KeyCode = evt.which ? evt.which : evt.keyCode;

            if (window.event) {
                if ((evt.keyCode != 46) && (evt.keyCode != 45) && (evt.keyCode != 8) && (evt.keyCode < 48 || evt.keyCode > 57)) {
                    evt.returnValue = false;
                    evt.preventDefault();
                }
                else {
                    if (objID.value.split('.').length > 1) {
                        if ((objID.selectionEnd - objID.selectionStart) < 0 || (objID.value.split('.')[1].length > 1 && (objID.selectionEnd - objID.selectionStart) == 0 && objID.value.indexOf('.') < objID.selectionEnd)) {
                            evt.returnValue = false;
                            evt.preventDefault();
                        }
                        else if (evt.keyCode == 46) {
                            evt.returnValue = false;
                            evt.preventDefault();
                        }
                    }
                }

            }
            else {
                if ((evt.which != 0) && (evt.which != 46) && (evt.keyCode != 45) && (evt.which != 8) && (evt.which < 48 || evt.which > 57)) {
                    evt.returnValue = false;
                    evt.preventDefault();
                }
                else {
                    if (objID.value.split('.').length > 1) {
                        if ((objID.selectionEnd - objID.selectionStart) < 0 || (objID.value.split('.')[1].length > 1 && (objID.selectionEnd - objID.selectionStart) == 0 && objID.value.indexOf('.') < objID.selectionEnd)) {
                            evt.returnValue = false;
                            evt.preventDefault();
                        }
                        else if (evt.keyCode == 46) {
                            evt.returnValue = false;
                            evt.preventDefault();
                        }
                    }
                }

            }
            if (objID.value > 99.99) {
                evt.returnValue = false;
                evt.preventDefault();
            }
            e.cancelBubble = true;
        }
        function uncheckOthers(id) {
            var elm = document.getElementsByTagName('input');
            for (var i = 0; i < elm.length; i++) {
                if (elm.item(i).id.substring(id.id.lastIndexOf('_')) == id.id.substring(id.id.lastIndexOf('_'))) {

                    if (elm.item(i).type == "checkbox" && elm.item(i) != id) {
                        elm.item(i).checked = false;
                    }
                    if (elm.item(i).type == "checkbox" && elm.item(i) == id) {
                        elm.item(i).checked = true;
                    }

                }
            }
        }

        function CheckOne(obj) {
            var row = obj.parentNode.parentNode;
            var DiscId = row.cells[0].getElementsByTagName('input')[1].value;
            document.getElementById('<%= checkAreaRepresentative.ClientID%>').value = DiscId;
            var grid = obj.parentNode.parentNode.parentNode;
            var inputs = grid.getElementsByTagName("input");
            for (var i = 0; i < inputs.length; i++) {
                if (inputs[i].type == "checkbox") {
                    if (obj.checked && inputs[i] != obj && inputs[i].checked) {
                        inputs[i].checked = false;
                    }
                }
            }
        }
        function deletemsg() {
            var response = confirm('Are you sure you want to delete this record? Click [Ok] to proceed or [Cancel] to abort.');
            if (response == true) {
                return true;
            }
            else {
                return false;
            }

        }
        function openARCommissionReportSelectDate() {

            var width = 525;
            var height = 240;
            var left = (screen.width - width) / 2;
            var top = (screen.height - height) / 2;
            var params = 'width=' + width + 'px' + ', height=' + height + 'px';
            params += ', top=' + top + ', left=' + left;
            params += ', directories=no';
            params += ', location=no';
            params += ', menubar=no';
            params += ', resizable=0';
            params += ', status=0';
            params += ', scrollbars=1';

            var Result = window.open('POSSelectReportDate.aspx', '_blank', params);
            Result.focus();
        }
        
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <ajax:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" LoadScriptsBeforeUI="true">
        </ajax:ScriptManager>
        <div>
            <table align="left" border="0" cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td>
                        <table align="center" style="margin-top: 5px;" width="100%">
                            <tr>
                                <td style="padding-left: 6px; width: 1%;"></td>
                                <td align="left" style="padding-left: 0px; width: 100%; height: 250px; overflow: auto;"
                                    valign="top">
                                    
                                    <center>
                                        <font color="red">
                                            <asp:Label ID="lblMsgAreaRepresentative" runat="server" Text="No Records found." Visible="false"></asp:Label>
                                        </font>
                                    </center>
                                    <ajax:UpdatePanel ID="pnlgrid" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <div style="height: 365px;">
                                                <asp:HiddenField ID="checkAreaRepresentative" runat="server" />
                                                <asp:GridView ID="gvAreaRepresentative" runat="server" AllowSorting="true" 
                                                    AutoGenerateColumns="False" BorderColor="White" BorderWidth="1px" ShowFooter="false"
                                                    Width="100%">
                                                    <PagerStyle HorizontalAlign="Right" VerticalAlign="Middle" />
                                                    <SelectedRowStyle CssClass="selectrow" />
                                                    <HeaderStyle CssClass="header" HorizontalAlign="Left" VerticalAlign="Middle" />
                                                    <AlternatingRowStyle CssClass="alterrow" />
                                                    <RowStyle CssClass="row" />
                                                    <EmptyDataRowStyle ForeColor="Red" HorizontalAlign="Center" />
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkboxrecords" runat="server" onclick="CheckOne(this)" />
                                                                <asp:HiddenField ID="chkAreaRepresentative" runat="server" Value='<%#Bind("AreaRepID")%>' />
                                                            </ItemTemplate>
                                                            <HeaderStyle Width="5%" HorizontalAlign="Left" />
                                                            <ItemStyle HorizontalAlign="Left" Width="5%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Area Location">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblAreaLocation" runat="server" Text=' <%# Bind("AreaLocation")%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderTemplate>
                                                                &nbsp;<asp:LinkButton ID="lnkbtnArealocation" runat="server" CommandArgument="arealocation"
                                                                    CommandName="Sort" ForeColor="White" Font-Underline="false"  >Area Location</asp:LinkButton>
                                                                <asp:ImageButton ID="imgAreaLocation" runat="server" />
                                                            </HeaderTemplate>
                                                            <HeaderStyle HorizontalAlign="left" VerticalAlign="Middle" Width="30%" />
                                                            <ItemStyle HorizontalAlign="left" Width="30%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Area Representative 1">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblAreaRepresentative1" runat="server" Text='<%# Bind("FirstAreaRepresentative")%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderTemplate>
                                                                &nbsp;<asp:LinkButton ID="lnkbtnFirstRepresentative" runat="server" CommandArgument="e1.lname"
                                                                    CommandName="Sort" ForeColor="White"  Font-Underline="false"  >Area Representative 1</asp:LinkButton>
                                                                <asp:ImageButton ID="imgAreaRepresentative1" runat="server" />
                                                            </HeaderTemplate>
                                                            <HeaderStyle HorizontalAlign="left" VerticalAlign="Middle" Width="30%" />
                                                            <ItemStyle HorizontalAlign="left" Width="30%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Area Representative 2">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblAreaRepresentative2" runat="server" Text=' <%# Bind("SecondAreaRepresentative")%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderTemplate>
                                                                &nbsp;<asp:LinkButton ID="lnkbtnSecondRepresentative" runat="server" CommandArgument="e2.lname"
                                                                    CommandName="Sort" ForeColor="White"  Font-Underline="false"  >Area Representative 2</asp:LinkButton>
                                                                <asp:ImageButton ID="imgAreaRepresentative2" runat="server" />
                                                            </HeaderTemplate>
                                                            <HeaderStyle HorizontalAlign="left" VerticalAlign="Middle" Width="30%" />
                                                            <ItemStyle Width="30%" HorizontalAlign="left" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                <tr style="height: 5px;">
                                                    <td></td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" align="right">
                                                        <asp:LinkButton ID="lnkPrevCust" runat="server" CommandName="Page" CommandArgument="Prev"
                                                            Font-Bold="true" Font-Underline="false" Font-Names="Arial" ForeColor="#0101DF">Previous</asp:LinkButton>
                                                        &nbsp;&nbsp;|&nbsp;&nbsp;
                                        <asp:LinkButton ID="lnkNextCust" runat="server" CommandName="Page" CommandArgument="Next"
                                            Font-Bold="true" Font-Underline="false" Font-Names="Arial" ForeColor="#0101DF">Next</asp:LinkButton>
                                                        <asp:HiddenField ID="hdnNoofPage" runat="server" />
                                                    </td>
                                                </tr>
                                            </table>
                                            <br />
                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                <tr>
                                                    <td valign="middle" align="right">
                                                        <asp:Button ID="btnCommissionReport" runat="server" Text="AR Commission Report" Style="cursor: pointer;margin-right:30%" Width="180px"
                                                            Height="28px" CssClass="alterrow" Font-Bold="true" />&nbsp; &nbsp;&nbsp;
                                                        <asp:Button ID="btnAddAreaPresentative" runat="server" Text="Add" Style="cursor: pointer;" Width="70px"
                                                            Height="28px" CssClass="alterrow" Font-Bold="true" />&nbsp;                                                    
                                                     <asp:Button ID="btnEditAreaRepresentative" runat="server" Text="Edit" Style="cursor: pointer;" Width="70px"
                                                         Height="28px" CssClass="alterrow" Font-Bold="true" />&nbsp;
                                                    <asp:Button ID="btnDeleteAreaRepresentative" runat="server" Text="Delete" Style="cursor: pointer;" Width="70px"
                                                        Height="28px" CssClass="alterrow" Font-Bold="true" OnClientClick="return deletemsg();" />&nbsp;                                                    
                                                    <button id="imgExitAreaRepresentative" style="cursor: pointer; width: 70px; height: 28px; font-weight: bold" class="alterrow" onclick="javascript:window.close();">Exit</button>
                                                    </td>
                                                </tr>
                                            </table>
                                            
                                        </ContentTemplate>
                                    </ajax:UpdatePanel>
                                </td>
                                <td align="right" style="padding-right: 6px; width: 1%;"></td>
                            </tr>

                        </table>
                    </td>
                </tr>

            </table>

        </div>

        <div>
            <%------------------------------------------------ POPUP For Add/Edit Area Representative --------------------------------%>
            <ajaxToolkit:ModalPopupExtender ID="mpeaddAreaRepresentative" DropShadow="false" BehaviorID="mpeaddAreaRepresentative"
                RepositionMode="None" runat="server" PopupDragHandleControlID="pnladdgiftcard"
                Drag="true" BackgroundCssClass="modalBackground" PopupControlID="pnladdAreaRepresentative"
                TargetControlID="btnadd1AreaRepresentative">
            </ajaxToolkit:ModalPopupExtender>
            <asp:Button ID="btnadd1AreaRepresentative" runat="server" Style="display: none" />
            <asp:Panel ID="pnladdAreaRepresentative" runat="server" Width="75%" Style="display: none">
                <ajax:UpdatePanel ID="updtaddAreaRepresentative" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table cellpadding="0" cellspacing="0" border="0" align="center" width="100%" style="background-color: White;">
                            <tr>
                                <td height="27" colspan="3" background="images/popup_01.gif">
                                    <table cellpadding="0" cellspacing="0" width="100%">
                                        <tr>
                                            <td width="10"></td>
                                            <td width="293">
                                                <asp:Label ID="lblAreaRepresentative" CssClass="stylePopupHeader" runat="server" Text="Area Representative"></asp:Label>
                                            </td>
                                            <td align="right">
                                                <asp:ImageButton ID="imsbtnclose"
                                                    runat="server" ImageUrl="~/Images/close1.gif" OnClientClick="$find('mpeaddAreaRepresentative').hide();" />
                                            </td>
                                            <td width="2%"></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td style="padding-left: 10px; padding-right: 10px;">
                                    <table cellpadding="0" cellspacing="0" width="100%">
                                        <tr>
                                            <td style="height: 4px"></td>
                                        </tr>
                                        <tr>
                                            <td align="left" valign="top">
                                                <table width="100%" border="0" cellspacing="0" cellpadding="0" class="tab1">
                                                    <tr>
                                                        <td height="8" align="left" valign="top">
                                                            <img src="images/crnr1w.gif" alt="" width="8" height="12" style="margin: 0px 2px 0px 0px" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td height="6" align="left" valign="top"></td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <table cellpadding="2" cellspacing="2" border="0" width="100%">
                                                                <tr>
                                                                    <td width="2%">&nbsp;
                                                                    </td>
                                                                    <td width="30%">
                                                                        <asp:Label ID="lblAreaLocation" runat="server" CssClass="labelall" Style="color: black; font-size: 12px; font-family: Arial" Text="Area Location:"></asp:Label>
                                                                        <asp:RequiredFieldValidator ID="arealocationrequired" runat="server" ControlToValidate="txtAreaLocation" SetFocusOnError="True" ValidationGroup="SaveAreaLocation" Text="<img src='images/blink1.gif' alt=''/>">
                                                                        </asp:RequiredFieldValidator>

                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtAreaLocation" AutoCompleteType="Disabled" onfocus="this.select();changecolor1(this.id);" onblur="normalcolor(this.id);" MaxLength="20" Width="155px"
                                                                            CssClass="labelall" runat="server"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr height="5px">
                                                                    <td colspan="3"></td>
                                                                </tr>
                                                                <tr>
                                                                    <td>&nbsp;
                                                                    </td>
                                                                    <td width="30%">
                                                                        <asp:Label ID="lblFirstAreaRepresentative" runat="server" CssClass="labelall" Style="color: black; font-size: 12px; font-family: Arial" Text="Area Representative 1:"></asp:Label>
                                                                        <asp:RequiredFieldValidator ID="drpFirstAreaRepresentativeRequired" runat="server" ControlToValidate="drpFirstAreaRepresentative" SetFocusOnError="True" ValidationGroup="SaveAreaLocation" Text="<img src='images/blink1.gif' alt=''/>"></asp:RequiredFieldValidator>
                                                                        <asp:Image ID="imgAr1" runat="server" ImageUrl="~/images/blink1.gif" Visible="false" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:DropDownList ID="drpFirstAreaRepresentative" Style="width: 80%" runat="server" AutoCompleteType="Disabled">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                                <tr height="5px">
                                                                    <td colspan="3"></td>
                                                                </tr>
                                                                <tr>
                                                                    <td>&nbsp;
                                                                    </td>
                                                                    <td width="30%">
                                                                        <asp:Label ID="lblSecondAreaRepresentative" runat="server" CssClass="labelall" Style="color: black; font-size: 12px; font-family: Arial" Text="Area Representative 2:"></asp:Label>
                                                                        <asp:RequiredFieldValidator ID="drpSecondAreaRepresentativeRequired" runat="server" ControlToValidate="drpSecondAreaRepresentative" SetFocusOnError="True" ValidationGroup="SaveAreaLocation" Text="<img src='images/blink1.gif' alt=''/>"></asp:RequiredFieldValidator>
                                                                        <%--<asp:Image ID="imgAr2" runat="server" ImageUrl="~/images/blink1.gif" Visible="false" />--%>
                                                                    </td>
                                                                    <td>
                                                                        <asp:DropDownList ID="drpSecondAreaRepresentative" Style="width: 80%" runat="server" AutoCompleteType="Disabled">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>

                                                                <tr height="5px">
                                                                    <td colspan="3"></td>
                                                                </tr>
                                                                <tr>
                                                                    <td width="2%">&nbsp;
                                                                    </td>
                                                                    <td width="30%">
                                                                        <asp:Label ID="lblCommission" runat="server" CssClass="labelall" Style="color: black; font-size: 12px; font-family: Arial" Text="Royalty Fee:"></asp:Label>
                                                                        <asp:RequiredFieldValidator ID="CommissionRequiredFieldValidator" runat="server" ControlToValidate="txtCommission" SetFocusOnError="True" ValidationGroup="SaveAreaLocation" Text="<img src='images/blink1.gif' alt=''/>">
                                                                        </asp:RequiredFieldValidator>

                                                                    </td>
                                                                    <td>
                                                                        <%--<asp:TextBox ID="txtCommission" AutoCompleteType="Disabled" onfocus="this.select();changecolor1(this.id);" OnKeyPress="return OnlyNumeric(event, this)" onblur="normalcolor(this.id);getfloatwithDollar(this.id,2);" MaxLength="6" Width="80px"
                                                                            CssClass="labelall" runat="server" Text="40.00"></asp:TextBox>--%>
                                                                        <asp:TextBox ID="txtCommission" AutoCompleteType="Disabled" onfocus="this.select();changecolor1(this.id);" OnKeyPress="return OnlyNumeric(event, this)" onblur="normalcolor(this.id);getfloatwithDollar(this.id,2);" MaxLength="6" Width="80px"
                                                                            CssClass="labelall" runat="server" Text="5.00"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr height="5px">
                                                                    <td colspan="3"></td>
                                                                </tr>
                                                                <tr>
                                                                    <td width="2%">&nbsp;
                                                                    </td>
                                                                    <td width="30%">
                                                                        <asp:Label ID="lblARRoyaltyCommission" runat="server" CssClass="labelall" Style="color: black; font-size: 12px; font-family: Arial" Text="AR Royalty Commission:"></asp:Label>
                                                                        <asp:RequiredFieldValidator ID="lblARRoyaltyCommissionRequiredFieldValidator" runat="server" ControlToValidate="txtARRoyaltyCommission" SetFocusOnError="True" ValidationGroup="SaveAreaLocation" Text="<img src='images/blink1.gif' alt=''/>">
                                                                        </asp:RequiredFieldValidator>
                                                                    </td>
                                                                    <td>
                                                                        <%--<asp:TextBox ID="txtARRoyaltyCommission" AutoCompleteType="Disabled" onfocus="this.select();changecolor1(this.id);" OnKeyPress="return OnlyNumeric(event, this)" onblur="normalcolor(this.id);getfloatwithDollar(this.id,2);" MaxLength="6" Width="80px"
                                                                            CssClass="labelall" runat="server" Text="5.00"></asp:TextBox>--%>
                                                                        <asp:TextBox ID="txtARRoyaltyCommission" AutoCompleteType="Disabled" onfocus="this.select();changecolor1(this.id);" OnKeyPress="return OnlyNumeric(event, this)" onblur="normalcolor(this.id);getfloatwithDollar(this.id,2);" MaxLength="6" Width="80px"
                                                                            CssClass="labelall" runat="server" Text="40.00"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr height="5px">
                                                                    <td width="2%">&nbsp;
                                                                    </td>
                                                                    <td colspan="2">
                                                                        <p style="color: black; font-size: 12px; font-family: Arial">
                                                                            The calculation above is designed for franchise operations where an Area Representative would receive a portion of the royalties from each store within their region.  
                                                                        </p>
                                                                        <p style="color: black; font-size: 12px; font-family: Arial">
                                                                            Example:<br />
                                                                            Sales = $ 10,000 * 5% Royalty Fee = $ 500 * 40% AR Royalty = $ 200.00
                                                                        </p>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td height="8" align="left" valign="bottom" class="tab1c">
                                                            <img src="images/crnr3w.gif" alt="" width="8" height="8" style="margin: 0px 0px -4px 0px" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" style="padding-top: 6px; padding-bottom: 8px; padding-right: 8px;">

                                    <asp:Button ID="imgsaveAreaRepresentative" runat="server" Text="Save" Style="cursor: pointer;" Width="70px"
                                        Height="28px" CssClass="alterrow" Font-Bold="true" ValidationGroup="SaveAreaLocation" />
                                    &nbsp;<asp:Button ID="imgcancleAreaRepresentative" runat="server" Text="Cancel" Style="cursor: pointer;" Width="70px"
                                        Height="28px" CssClass="alterrow" Font-Bold="true" OnClientClick="$find('mpeaddAreaRepresentative').hide();" CausesValidation="false" />

                                </td>
                            </tr>

                        </table>
                    </ContentTemplate>
                </ajax:UpdatePanel>
            </asp:Panel>

        </div>

    </form>
</body>

</html>
