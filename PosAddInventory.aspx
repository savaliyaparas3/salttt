<%@ Page Language="VB" AutoEventWireup="false" CodeFile="PosAddInventory.aspx.vb"
    Inherits="PosAddInventory" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script type="text/javascript" language="javascript">
        function changeformat(id, intPrecision) {
            var price = document.getElementById(id);
            if (price.value > 99.99) {
                price.value = "99.99";
            }
            var num = parseFloat(price.value).toFixed(intPrecision);
            if (isNaN(num)) {
                price.value = parseFloat(0, intPrecision);
            }

            else {
                price.value = num;
            }
            return false;
        }

        function matchHTML(obj) {

            var pattern = /^(.*?\<.*?>)|(\<.*?>)$/;
            if (pattern.test(obj.value)) {
                alert('Invalid characters entered.');
                obj.focus();
            }
        }
        function changecolor(id) {
            var tbox = document.getElementById(id);
            tbox.style.backgroundColor = "Yellow";
            return false;
        }
        function normalcolor(id) {
            var tbox = document.getElementById(id);
            tbox.style.backgroundColor = "White";
            return false;
        }

        function VendorFocus() {
            document.getElementById('<%= txtVendorid.ClientID %>').focus();
        }

        function fncInputNumericValuesOnly(evt) {
            var e = evt ? evt : window.event;
            if (window.event) {
                if ((evt.keyCode != 8) && (evt.keyCode != 37) && (evt.keyCode != 38) && (evt.keyCode != 39) && (evt.keyCode != 40) && (evt.keyCode < 47 || evt.keyCode > 57)) {
                    evt.returnValue = false;
                }
            }
            else {
                if ((evt.which != 0) && (evt.which != 8) && (evt.which != 37) && (evt.which != 38) && (evt.which != 39) && (evt.which != 40) && (evt.which < 47 || evt.which > 57)) {
                    evt.returnValue = false;
                    evt.preventDefault();
                }
            }
        }

        function fncInputNumericdecimalValuesOnly(evt) {
            var e = evt ? evt : window.event;
            if (window.event) {
                if ((evt.keyCode != 46) && (evt.keyCode != 8) && (evt.keyCode != 37) && (evt.keyCode != 38) && (evt.keyCode != 39) && (evt.keyCode != 40) && (evt.keyCode < 46 || evt.keyCode > 57)) {
                    evt.returnValue = false;
                }

            }
            else {
                if ((evt.which != 0) && (evt.which != 46) && (evt.which != 8) && (evt.which != 37) && (evt.which != 38) && (evt.which != 39) && (evt.which != 40) && (evt.which < 46 || evt.which > 57)) {
                    evt.returnValue = false;
                    evt.preventDefault();
                }
            }

        }
        var txtid;

        function upperCase(e) {
            var e = e ? e : window.event;
            var KeyCode = e.which ? e.which : e.keyCode;

            if (window.event) {
                if ((KeyCode > 96) && (KeyCode < 123))
                    e.keyCode = KeyCode - 32;
                e.returnValue = true;
            }
            else {
                if (((KeyCode > 96) && (KeyCode < 123)) || (KeyCode != 8) && (KeyCode != 46) && (KeyCode != 37) && (KeyCode != 38) && (KeyCode != 39) && (KeyCode != 40)) {
                    document.getElementById('<%= txtSKU.ClientID %>').value += String.fromCharCode(KeyCode - 32);

                e.returnValue = false;
                e.preventDefault();
            }
        }
    }


    function flattax() {
        var value = document.getElementById('<%= txtFlattax.ClientID%>').value;

        if (value > 99.99) {
            document.getElementById('<%= txtFlattax.ClientID%>').focus();
            document.getElementById('<%= txtFlattax.ClientID%>').value = "00.00";

            set_focus();
            return false;
        }
        else {
            return false;
            //return true;
        }
    }


    function PressEsc(e) {
        var e = e ? e : window.event;
        var KeyCode = e.which ? e.which : e.keyCode;
        if (KeyCode == 27) {
            var BrowserName = navigator.userAgent.toLowerCase();
            if (BrowserName.indexOf("chrome") != -1) {
                parent.showModalDialog.returnValue("");
            }
            else {
                window.close();
                return false;
            }

        }
    }

    function ReturnValue() {
        var txtSKU = document.getElementById('<%=txtsku.ClientID%>').value;
        var BrowserName = navigator.userAgent.toLowerCase();
        if (BrowserName.indexOf("chrome") != -1) {
            parent.showModalDialog.returnValue(txtSKU);
        }
        else {
            window.returnValue = txtSKU;
        }
    }

        function CloseWindow() {
            var BrowserName = navigator.userAgent.toLowerCase();
            if (BrowserName.indexOf("chrome") != -1) {
                parent.showModalDialog.returnValue("");
            }
            else {
                window.close();
            }
    }

    function fnPreventSpecialChars() {
        var ch = String.fromCharCode(window.event.keyCode)
        if (ch.match(/[^a-zA-Z0-9]/g)) {
            window.event.cancelBubble = true;
            window.event.returnValue = false;

            if (window.event.stopPropagation && navigator.userAgent.search("MSIE") < 0) { //&& navigator.userAgent.search("MSIE") < 0
                window.event.stopPropagation();
                window.event.preventDefault();
            }
            return false;
        }
        return true;
    }

    function OpenItemScriptPopup() {
        //debugger;
        var BrowserName = navigator.userAgent.toLowerCase();
        if (BrowserName.indexOf("chrome") != -1) {
            var tmp = window.showModalDialog("POSInventoryItemScript.aspx?InvReceive=Y&SKU=" + document.getElementById('<%=txtSKU.ClientID%>').value + "", "", '{"scroll":"no","resizable":"no","status":"no","dialogWidth":"600px","dialogHeight":"350px","center":"yes"}').then(function (tmp) {
                if (tmp == '1') {
                    return false;
                }
            });
        }
        else {
            var tmp = window.showModalDialog("POSInventoryItemScript.aspx?InvReceive=Y&SKU=" + document.getElementById('<%=txtSKU.ClientID%>').value + "", "", "scroll:no;resizable:no;status:no;dialogWidth:600px;dialogHeight:350px;dialogLeft:70px;dialogTop:70px");
            if (tmp == '1') {
                return false;
            }
        }

    }
    </script>
    <title></title>
    <link href="1style.css" rel="stylesheet" type="text/css" />
    <link href="CSS/Mainstyle.css" rel="stylesheet" type="text/css" />
    <link href="CSS/ringsalestyle.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        body {
            background-color: #CEE6FF;
        }

        .Bottom1 {
            position: absolute;
            bottom: 10px;
            right: 4px;
        }

        .style20 {
            height: 24px;
        }

        .style21 {
            font-size: 12px;
            font-family: Arial;
            color: Black;
            height: 24px;
        }

        .style23 {
            font-size: 12px;
            font-family: Arial;
            color: Black;
            height: 31px;
        }

        .style24 {
            height: 31px;
        }

        .style25 {
            height: 31px;
        }

        .style26 {
            height: 26px;
        }

        .style27 {
            font-size: 12px;
            font-family: Arial;
            color: Black;
            height: 26px;
        }
    </style>
</head>
<body onkeypress="PressEsc(event);">
    <form id="form1" runat="server">
        <ajax:ScriptManager ID="ScriptManager1" runat="server">
        </ajax:ScriptManager>
        <ajax:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <table width="100%" cellspacing="0" cellpadding="2px">
                    <tr style="height: 5px;">
                        <td colspan="3">
                            <asp:HiddenField ID="hdMarkupDept" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="style25">&nbsp;
                        </td>
                        <td class="style23">SKU: &nbsp;<span style="color: Red; font-weight: bold;">*</span>
                        </td>
                        <td style="white-space: nowrap;" class="style24">
                            <asp:TextBox ID="txtSKU" runat="server" MaxLength="16" Width="78%" Style="text-transform: uppercase" onkeypress="return fnPreventSpecialChars();"
                                CssClass="textboxall" onfocus="changecolor(this.id);" onblur="normalcolor(this.id);">0</asp:TextBox>
                            <asp:RequiredFieldValidator ID="reqtxtSKU" runat="server" ControlToValidate="txtSKU"
                                ErrorMessage="*"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;
                        </td>
                        <td class="labelall" valign="top">Description:&nbsp;<span style="color: Red; font-weight: bold;">*</span>
                        </td>
                        <td>
                            <asp:TextBox ID="txtDesc" runat="server" MaxLength="80" Width="78%" TextMode="MultiLine" Style="text-transform: capitalize; font-size: 12px; font-family: Arial; color: Black; width: 88%;"
                                Rows="2" onfocus="changecolor(this.id);" onblur="normalcolor(this.id);matchHTML(this);"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtDesc"
                                ErrorMessage="*"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;
                        </td>
                        <td class="labelall">Department:
                        </td>
                        <td>
                            <asp:DropDownList ID="ddldept" runat="server" Width="80%" AutoPostBack="true" CssClass="dropdownall" onfocus="changecolor(this.id);" onblur="normalcolor(this.id);">
                            </asp:DropDownList>
                            <asp:Label ID="lblnodept" runat="server" CssClass="labelall" Text="NO Department Found"></asp:Label>
                        </td>
                    </tr>
                    <tr id="trstyle" runat="server">
                        <td>&nbsp;
                        </td>
                        <td class="labelall">Style:
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlStyle" runat="server" Width="80%" CssClass="dropdownall"
                                AutoPostBack="true" onfocus="changecolor(this.id);" onblur="normalcolor(this.id);">
                            </asp:DropDownList>
                            <asp:Label ID="lblNoStyle" runat="server" Text="No Style Found" CssClass="labelall"
                                Visible="false"></asp:Label>
                        </td>
                    </tr>
                    <tr id="trsize" runat="server">
                        <td>&nbsp;
                        </td>
                        <td class="labelall">Size:
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlSize" runat="server" CssClass="dropdownall" Width="80%"
                                AutoPostBack="true" onfocus="changecolor(this.id);" onblur="normalcolor(this.id);">
                            </asp:DropDownList>
                            <asp:Label ID="lblNoSize" runat="server" Text="No Size Found" CssClass="labelall"
                                Visible="false"></asp:Label>
                        </td>
                    </tr>
                    <tr id="trqtypercasereceive" runat="Server">
                        <td>&nbsp;
                        </td>
                        <td class="labelall">Receive by:
                        </td>
                        <td style="padding-left: 0px;">
                            <asp:RadioButton ID="rdoUnitandcase" runat="server" CssClass="radioall" GroupName="caseunit" Text="Cases & Units" Checked="true" AutoPostBack="true" />
                            &nbsp;&nbsp;
                            <asp:RadioButton ID="rdoUnits" runat="server" CssClass="radioall" GroupName="caseunit" Text="Only Units" AutoPostBack="true" />
                        </td>
                    </tr>
                    <tr id="trQtypercase" runat="Server">
                        <td>&nbsp;
                        </td>
                        <td class="labelall">Qty Per Case:
                        </td>
                        <td>
                            <asp:TextBox ID="txtQtypercase" runat="server" MaxLength="5" Width="30%" onkeypress="return fncInputNumericValuesOnly(event)"
                                CssClass="textboxall" onfocus="changecolor(this.id);this.select();" onblur="normalcolor(this.id);" style="text-align:right;">0</asp:TextBox>
                        </td>
                    </tr>

                    <tr id="trVendorName" runat="Server">
                        <td>&nbsp;
                        </td>
                        <td class="labelall">Vendor:
                        </td>
                        <td>
                            <asp:Label ID="lblVendorName" runat="server" CssClass="labelall" Visible="false"></asp:Label>
                        </td>
                    </tr>
                    <tr id="trDivision" runat="Server" visible="false">
                        <td>&nbsp;
                        </td>
                        <td class="labelall">Division:
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlDivision" runat="server" CssClass="dropdownall" Width="80%" AutoPostBack="false" onchange="VendorFocus();" onfocus="changecolor(this.id);" onblur="normalcolor(this.id);"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;
                        </td>
                        <td class="labelall">Vendor #:
                        </td>
                        <td>
                            <asp:TextBox ID="txtVendorid" runat="server" Class="textboxall" MaxLength="16" Width="30%" onfocus="changecolor(this.id);this.select();" onblur="normalcolor(this.id);"></asp:TextBox>
                            <asp:HiddenField ID="hdnvendorid" runat="server" />
                        </td>
                    </tr>
                    <tr id="trdiscount" runat="server">
                        <td>&nbsp;
                        </td>
                        <td class="labelall">Discount Group:
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlDiscGroup" CssClass="dropdownall" runat="server" onfocus="changecolor(this.id);" onblur="normalcolor(this.id);">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr id="trdiscgroup" runat="server">
                        <td></td>
                        <td class="labelall">Deposit
                        </td>
                        <td>
                            <asp:CheckBox ID="chkbottledeposit" runat="server" Text="" Checked="false" AutoPostBack="true" />
                            &nbsp;&nbsp;
                       <asp:TextBox ID="txtbottledeposit" runat="server" Text="0.00" Width="40px" MaxLength="5" style="text-align:right;"
                           onfocus="changecolor(this.id);this.select();" onblur="normalcolor(this.id);changeformat(this.id,2);"></asp:TextBox>
                        </td>
                    </tr>
                    <tr id="trReorder" runat="Server">
                        <td>&nbsp;
                        </td>
                        <td align="left" class="labelall">Reorder Point:
                        </td>
                        <td>
                            <asp:TextBox ID="txtReorder" runat="server" MaxLength="5" CssClass="textboxall" Width="30%"
                                onkeypress="return fncInputNumericValuesOnly(event)" onfocus="changecolor(this.id);" onblur="normalcolor(this.id);" style="text-align:right;">0</asp:TextBox>
                        </td>
                    </tr>
                    <tr id="trNormalStock" runat="Server">
                        <td class="style26">&nbsp;
                        </td>
                        <td class="style27">Normal Qty:
                        </td>
                        <td class="style26">
                            <asp:TextBox ID="txtNormalStock" runat="server" MaxLength="5" Width="30%" onkeypress="return fncInputNumericValuesOnly(event)"
                                CssClass="textboxall" onfocus="changecolor(this.id);" onblur="normalcolor(this.id);" style="text-align:right;">0</asp:TextBox>
                        </td>
                    </tr>
                    <tr id="trsalestax" runat="Server">
                        <td>&nbsp;
                        </td>
                        <td align="left" class="labelall">
                            <asp:Label ID="lblsalestax" runat="server" CssClass="labelall" Text=""></asp:Label>
                        </td>
                        <td class="labelall">
                            <asp:CheckBox ID="chksalestax" runat="server" />
                        </td>
                    </tr>
                    <tr id="trwinetax" runat="Server">
                        <td>&nbsp;
                        </td>
                        <td align="left" class="labelall">
                            <asp:Label ID="lblwinetax" runat="server" CssClass="labelall" Text=""></asp:Label>
                        </td>
                        <td class="labelall">
                            <asp:CheckBox ID="chkwinetax" runat="server" />
                        </td>
                    </tr>
                    <tr id="trmisctax" runat="Server">
                        <td class="style20">&nbsp;
                        </td>
                        <td align="left" class="style21">
                            <asp:Label ID="lblmisctax" runat="server" CssClass="labelall" Text=""></asp:Label>
                        </td>
                        <td class="labelall">
                            <asp:CheckBox ID="chkmisctax" runat="server" />
                        </td>
                    </tr>
                    <tr id="trflattax" runat="Server">
                        <td>&nbsp;
                        </td>
                        <td align="left" class="labelall">
                            <asp:Label ID="lblflattax" runat="server" CssClass="labelall" Text=""></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtFlattax" onkeypress="return fncInputNumericdecimalValuesOnly(event)" onchange="flattax();"
                                runat="server" CssClass="textboxall" MaxLength="5" Width="13%" onfocus="changecolor(this.id);" onblur="normalcolor(this.id);"></asp:TextBox>
                        </td>
                    </tr>
                    <tr id="tr5" runat="Server">
                        <td>&nbsp;
                        </td>
                        <td align="left" class="labelall">Discountable:
                        </td>
                        <td class="labelall">
                            <asp:CheckBox ID="chkDiscountable" runat="server" />
                        </td>
                    </tr>
                    <tr id="tr6" runat="Server">
                        <td>&nbsp;
                        </td>
                        <td align="left" class="labelall">Print Barcode:
                        </td>
                        <td class="labelall">
                            <asp:CheckBox ID="chkBarcode" runat="server" Checked="false" />
                        </td>
                    </tr>
                    <tr id="TrSaleonInternet" runat="Server">
                        <td>&nbsp;
                        </td>
                        <td align="left" class="labelall">Sell on Internet:
                        </td>
                        <td class="labelall">
                            <asp:CheckBox ID="chkselloninternet" runat="server" Checked="false" />
                        </td>
                    </tr>
                    <tr id="trloyalty" runat="Server">
                        <td>&nbsp;
                        </td>
                        <td align="left" class="labelall">
                            <asp:Label ID="lblPoints" runat="server" CssClass="labelall" Text="Loyalty Points:"></asp:Label>
                        </td>
                        <td class="labelall">
                            <asp:TextBox ID="txtPoints" runat="server" Width="17%" Text="1" MaxLength="2" onkeypress="return fncInputNumericValuesOnly(event)"
                                onfocus="changecolor(this.id);" onblur="normalcolor(this.id);" style="text-align:right;"></asp:TextBox>
                        </td>
                    </tr>
                    <tr id="trItemScript">
                        <td>&nbsp;
                        </td>
                        <td align="left" class="labelall">
                            <asp:LinkButton ID="lnkItemScript" runat="server" Text="Item Script" CssClass="style8" Style="text-decoration: underline" OnClientClick="return OpenItemScriptPopup();"></asp:LinkButton>
                        </td>
                        <td>&nbsp</td>
                    </tr>
                    <tr>
                        <td>&nbsp;
                        </td>
                        <td class="labelall">&nbsp;
                        </td>
                        <td>&nbsp;
                        </td>
                    </tr>
                </table>
                <div class="Bottom1">
                    <asp:Button ID="btnSavedetails" runat="server" CausesValidation="true" Style="cursor: pointer;"></asp:Button>
                    <asp:Button ID="btnCanceldetails" runat="server" Text="Cancel" Style="cursor: pointer;"
                        CausesValidation="false" />
                </div>
            </ContentTemplate>
        </ajax:UpdatePanel>
    </form>
</body>
</html>



