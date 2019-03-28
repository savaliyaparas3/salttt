<%@ Page Language="VB" AutoEventWireup="false" CodeFile="POSAddInventorySKU.aspx.vb" Inherits="POSAddInventorySKU" %>
<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<%@ Register TagPrefix="UC" TagName="Help" Src="~/UserControls/ucHelp.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Add New Item</title>
    <base target="_self" />
    <link href="1style.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .stylePopupHeader {
            font-size: 12px;
            font-family: Arial, Helvetica, sans-serif;
            font-weight: bold;
            color: White;
        }

        .stylePopuplabel {
            font-family: Arial, Helvetica, sans-serif;
            font-size: 12px;
            color: #577386;
        }
       
        .dialog {
            top:20% !important;
            left:20% !important;
            width:700px !important;
        } 
    
    </style>
    <script type="text/javascript">
        function PassValuesBack() {
           
            var txtSKUname = document.getElementById('<%=txtSKU.ClientID%>').value;
            var Inventory = document.getElementById('<%= rdoinventory.ClientID%>');
            var NonInventory = document.getElementById('<%= rdononinventory.ClientID%>');
            var Coupon = document.getElementById('<%= rdoCoupon.ClientID%>');
            var Kitclub = document.getElementById('<%= rdoKitClub.ClientID%>');
            var Service = document.getElementById('<%= rdoService.ClientID%>');
            var Pours = document.getElementById('<%= rdoPours.ClientID%>');
            var clone = document.getElementById('<%= hdnclone.ClientID%>');
            var Multipack = document.getElementById('<%= rdoMultiPack.ClientID%>');

            if (Inventory.checked) {
                var ItemType = "Inventory";
            }
            else if (NonInventory.checked) {
                var ItemType = "NonInventory";
            }
            else if (Coupon.checked) {
                var ItemType = "Coupon";
            }
            else if (Kitclub.checked) {
                var ItemType = "Kitclub";
            }
            else if (Service.checked) {
                var ItemType = "Service";
            }
            else if (Pours.checked) {
                var ItemType = "Pours";
            }
            else if (Multipack.checked) {
                var ItemType = "Multipack";
            }
            var objReturnValue = new Object();
            objReturnValue.SKU = txtSKUname;
            objReturnValue.type = ItemType;
            objReturnValue.CloneType = clone.value;
            var BrowserName = navigator.userAgent.toLowerCase();
            if (BrowserName.indexOf("chrome") != -1) {
                parent.showModalDialog.returnValue(objReturnValue);
            }
            else {
                window.returnValue = objReturnValue;
                window.close();
            }
        }
        function changecolor(id) {
            var tbox = document.getElementById(id);
            tbox.style.backgroundColor = "Yellow";
            return false;
        }
        function changecolorWhite(id) {
            var tbox = document.getElementById(id);
            tbox.style.backgroundColor = "White";
            return false;
        }
        function fnPreventSpecialChars() {
            var ch = String.fromCharCode(window.event.keyCode)
            var KeyCode = window.event.keyCode
            if (KeyCode == 13) {
                var check = document.getElementById('<%= btnContinue.ClientID%>').click();
            }
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

        function CloseWindow() {
            var BrowserName = navigator.userAgent.toLowerCase();
            if (BrowserName.indexOf("chrome") != -1) {
                parent.showModalDialog.returnValue("");
            }
            else {
                window.close();
            }
        }

        function openDupInventoryCorp(SKU, storeid, type) {
            
            var width = 800;
            var height = 600;
            var left = (screen.width - width) / 2;
            var top = (screen.height - height) / 2;
            var params = 'dialogWidth:' + width + 'px' + '; dialogHeight:' + height + 'px';
            params += '; dialogTop:' + top + '; dialogLeft:' + left;
            params += '; directories=no';
            params += '; location=no';
            params += '; menubar=no';
            params += '; resizable=0';
            params += '; status=0';
            params += '; scroll=0';
            
            var Result = window.showModalDialog('POSDupInv_Corp.aspx?sku=' + SKU + '&storeid=' + storeid + '&type=' + type + '', '_blank', params);
           
            if (Result == 1) {
                PassValuesBack();
                
            }
            else {
                window.close();
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <ajax:ScriptManager ID="scrmg" runat="server"></ajax:ScriptManager>
        <ajax:UpdatePanel ID="upaddnewsku" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Panel ID="pnlAddnewsku" runat="server">
                    <table cellpadding="0" cellspacing="0" width="100%" border="0">
                        <tr>
                            <td valign="top">
                                <table cellpadding="0" cellspacing="0" border="0" width="100%" style="padding: 10px 10px 10px 10px">
                                    <tr>
                                        <td align="left" valign="top" class="tab1" width="100%">
                                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                <tr>
                                                    <td height="7" align="left" valign="top">
                                                        <img src="images/crnr1w.gif" style="vertical-align: top;" alt="" width="8" height="7" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                            <tr>
                                                                <td style="padding-left: 5px; width: 100%;" colspan="2">
                                                                    <asp:Label ID="lblAddnewitem" runat="server" CssClass="labelall" Font-Size="11pt" Font-Bold="true" Text="Add New Item:"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="padding-left: 5px; padding-top: 10px; width: 30%;">
                                                                    <asp:Label ID="Label1" runat="server" CssClass="labelall" Font-Size="11pt" Font-Bold="true" Text="Type:"></asp:Label>
                                                                </td>
                                                                <td style="padding-top: 5px; width: 70%;">
                                                                    <asp:RadioButton ID="rdoinventory" runat="server" Font-Size="11pt" Checked="true" CssClass="labelall"
                                                                        GroupName="rblItem" AutoPostBack="true"
                                                                        Text="Inventory" />&nbsp;
                                                                      <asp:ImageButton ID="imghelpinventory" runat="server" CommandName="24" ImageUrl="~/images/Helpicon.gif"
                                                                          OnCommand="Help_click" CausesValidation="false" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="padding-left: 5px; padding-top: 5px; width: 30%;"></td>
                                                                <td style="padding-top: 5px; width: 70%;">
                                                                    <asp:RadioButton ID="rdononinventory" runat="server" Font-Size="11pt" AutoPostBack="true" CssClass="labelall"
                                                                        GroupName="rblItem" Text="Non-Inventory" />&nbsp;
                                                                      <asp:ImageButton ID="imghelpnoninventory" runat="server" CommandName="25" ImageUrl="~/images/Helpicon.gif"
                                                                          OnCommand="Help_click" CausesValidation="false" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="padding-left: 5px; padding-top: 5px; width: 30%;"></td>
                                                                <td style="padding-top: 5px; width: 70%;">
                                                                    <asp:RadioButton ID="rdoService" runat="server" CssClass="labelall" Font-Size="11pt" GroupName="rblItem"
                                                                        Text="Service" AutoPostBack="true" />&nbsp;
                                                                      <asp:ImageButton ID="imghelpService" runat="server" CommandName="26" ImageUrl="~/images/Helpicon.gif"
                                                                          OnCommand="Help_click" CausesValidation="false" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="padding-left: 5px; padding-top: 5px; width: 30%;"></td>
                                                                <td style="padding-top: 5px; width: 70%;">
                                                                    <asp:RadioButton ID="rdoCoupon" runat="server" CssClass="labelall" Font-Size="11pt" GroupName="rblItem"
                                                                        Text="Coupon" AutoPostBack="true" />&nbsp;
                                                                      <asp:ImageButton ID="imghelpCoupon" runat="server" CommandName="28" ImageUrl="~/images/Helpicon.gif"
                                                                          OnCommand="Help_click" CausesValidation="false" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="padding-left: 5px; padding-top: 5px; width: 30%;"></td>
                                                                <td style="padding-top: 5px; width: 70%;">
                                                                    <asp:RadioButton ID="rdoKitClub" runat="server" CssClass="labelall" Font-Size="11pt" GroupName="rblItem"
                                                                        Text="Kit/Club" AutoPostBack="true" />&nbsp;
                                                                     <asp:ImageButton ID="imghelpKitClub" runat="server" CommandName="120" ImageUrl="~/images/Helpicon.gif"
                                                                         OnCommand="Help_click" CausesValidation="false" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="padding-left: 5px; padding-top: 5px; width: 30%;"></td>
                                                                <td style="padding-top: 5px; width: 70%;">
                                                                    <asp:RadioButton ID="rdoPours" runat="server" CssClass="labelall" Font-Size="11pt" GroupName="rblItem"
                                                                        Text="Pours" AutoPostBack="true" />&nbsp;
                                                                     <asp:ImageButton ID="imghelpPours" runat="server" CommandName="121" ImageUrl="~/images/Helpicon.gif"
                                                                         OnCommand="Help_click" CausesValidation="false" />
                                                                </td>
                                                            </tr>
                                                            <tr > 
                                                                <td style="padding-left: 5px; padding-top: 5px; width: 30%;"></td>
                                                                <td style="padding-top: 5px; width: 70%;">
                                                                    <asp:RadioButton ID="rdoMultiPack" runat="server" CssClass="labelall" Font-Size="11pt" GroupName="rblItem"
                                                                        Text="Multipack" AutoPostBack="true"  />&nbsp;
                                                                     <asp:ImageButton ID="imghelpMultipack" runat="server" CommandName="122" ImageUrl="~/images/Helpicon.gif"
                                                                         OnCommand="Help_click" CausesValidation="false"/>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="padding-left: 5px; padding-top: 10px; width: 30%;">
                                                                    <asp:Label ID="Label2" runat="server" CssClass="labelall" Font-Size="11pt" Font-Bold="true" Text="SKU:"></asp:Label>
                                                                    <img alt="Required Field" src="Images/big1.gif" />
                                                                    <asp:RequiredFieldValidator ID="reqlabel" runat="server" ValidationGroup="addInventory"
                                                                        ControlToValidate="txtSKU">
                                                                        <asp:Image ID="Image2" runat="server" Style="display: none;" ImageUrl="~/Images/blink1.gif" />
                                                                    </asp:RequiredFieldValidator>
                                                                </td>
                                                                <td style="padding-top: 10px; width: 70%;">
                                                                    <asp:TextBox ID="txtSKU" runat="server" CssClass="textboxall" onkeypress="return fnPreventSpecialChars();"
                                                                        MaxLength="16" Style="text-transform: uppercase" onfocus="changecolor(this.id);this.select();"
                                                                        onblur="changecolorWhite(this.id);" TabIndex="1" Width="130px"></asp:TextBox>
                                                                    &nbsp;
                                                                      <asp:CheckBox ID="chkAutoSku" runat="server" TabIndex="2" Text="Assign SKU" CssClass="labelall"
                                                                          AutoPostBack="true" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 100%; height: 15px;" colspan="2" id="tdsizeupdown" runat="server"></td>
                                                            </tr>

                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td height="8" align="left" valign="bottom" class="tab1c">
                                                        <img alt="" height="8" style="vertical-align: bottom" src="images/crnr3w.gif" width="8" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 100%; text-align: right; padding-right: 10px;">
                                <asp:Button ID="btnContinue" runat="server" Text="Continue" Style="cursor: pointer;" Width="70px" TabIndex="3"
                                    Height="28px" CssClass="alterrow" Font-Bold="true" CausesValidation="true" ValidationGroup="emp" />&nbsp;       
                              <asp:Button ID="btnCancel" runat="server" Text="Cancel" Style="cursor: pointer;" Width="70px" TabIndex="4"
                                  Height="28px" CssClass="alterrow" Font-Bold="true" CausesValidation="false" />
                                <asp:HiddenField ID="hdnclone" runat="server" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </ContentTemplate>
        </ajax:UpdatePanel>
        <ajaxToolkit:ModalPopupExtender ID="mpeDisplayHelp" BehaviorID="mpeDisplayHelp" runat="server"
            TargetControlID="btnDisplayHelp" PopupControlID="pnlDisplayHelp" BackgroundCssClass="modalBackground"
            DropShadow="false" Drag="false" />
        <asp:Button ID="btnDisplayHelp" Style="display: none;" runat="server" />
        <asp:Panel ID="pnlDisplayHelp" CssClass="modalPopup" Style="display: none; width: 452px;"
            runat="server" Width="155px">
            <ajax:UpdatePanel ID="updDisplayHelp" runat="server">
                <ContentTemplate>
                    <table style="width: 451px;">
                        <tr style="font-weight: bold; color: #ffffff; font-family: Verdana">
                            <td width="500">
                                <table style="width: 100%;" cellspacing="0">
                                    <tr style="background-image: url(images/popup_01.gif);">
                                        <td>&nbsp;<asp:LinkButton ID="lnkDispTitle" runat="server" Style="position: static; text-decoration: none;" CssClass="stylePopupHeader"></asp:LinkButton>
                                        </td>
                                        <td align="right">
                                            <asp:ImageButton ID="btndisplayhelp1" runat="server" OnClientClick="$find('mpeDisplayHelp').hide();"
                                                ImageUrl="~/Images/close1.gif" />
                                        </td>
                                    </tr>
                                </table>
                                <table style="width: 100%; height: 200px;" cellspacing="0">
                                    <tr style="height: 3px;">
                                        <td colspan="3"></td>
                                    </tr>
                                    <tr>
                                        <td align="left"></td>
                                        <td align="left" class="style2a" valign="top" colspan="2">
                                            <asp:Panel ID="pnlDispDesc" runat="server" Height="190px" ScrollBars="Vertical">
                                                <asp:Label ID="lblDispDesc" runat="server" Style="position: static" Width="394px"
                                                    CssClass="labelHead"></asp:Label>
                                            </asp:Panel>
                                        </td>
                                        <td align="left"></td>
                                    </tr>
                                </table>
                                <table style="width: 100%;" cellspacing="0">
                                    <tr>
                                        <td>
                                            <hr style="color: Black;" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Button ID="imgbtnCloseHelp" runat="server" Text="Close" Style="cursor: pointer;"
                                                Width="70px" Height="28px" CssClass="alterrow" Font-Bold="true" OnClientClick="$find('mpeDisplayHelp').hide();" />
                                            <asp:HiddenField ID="hidDispHelp" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </ajax:UpdatePanel>
        </asp:Panel>

        <ajaxToolkit:ModalPopupExtender DropShadow="false" ID="mpecheck" runat="server"
            BackgroundCssClass="modalBackground" TargetControlID="btncheckSKU1" PopupControlID="pnlcheckSKU1">
        </ajaxToolkit:ModalPopupExtender>
        <asp:Button ID="btncheckSKU1" runat="server" Style="display: none" />
        <asp:Panel ID="pnlcheckSKU1" runat="server" CssClass="modalPopup" Style="display: none"
            Width="350px">
            <ajax:UpdatePanel ID="UpdatePanel3" runat="server">
                <ContentTemplate>
                    <table width="350" border="0" align="center" cellpadding="0" cellspacing="0">
                        <tr>
                            <td width="350" height="27" background="images/popup_01.gif">
                                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td width="15">&nbsp;
                                        </td>
                                        <td width="293">
                                            <asp:Label ID="Label19" CssClass="stylePopupHeader" runat="server" Text="Message"></asp:Label>
                                        </td>
                            </td>
                            <td align="right">
                                <asp:ImageButton ID="imgbtnclosefirst" runat="server" ImageUrl="~/Images/close1.gif" />
                                &nbsp;&nbsp;
                            </td>
                        </tr>
                    </table>
                    </td> </tr>
                <tr>
                    <td width="500" height="120" align="left" valign="top" background="images/popup_06n.gif">
                        <img src="images/spacer.gif" alt="" width="1" height="1" />
                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                            <tr>
                                <td width="20">&nbsp;
                                </td>
                                <td width="460" align="left" valign="top">
                                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                        <tr>
                                            <td width="380" align="left" valign="top">
                                                <asp:Label ID="Label22" runat="server" CssClass="stylePopuplabel"> </asp:Label><br />
                                                <asp:Label ID="Label25" Text="SKU " runat="server" CssClass="stylePopuplabel"> </asp:Label>
                                                <asp:Label ID="lblsku1" Text="xxx" runat="server" CssClass="stylePopuplabel"> </asp:Label>
                                                <asp:Label ID="Label64" Text="already exists." runat="server" CssClass="stylePopuplabel"> </asp:Label><br />
                                                <asp:Label ID="Label65" Text="Please try another one." runat="server" CssClass="stylePopuplabel"> </asp:Label>
                                            </td>
                                            <td width="80" align="right" valign="middle">
                                                <img src="images/popup_07.gif" alt="" width="50" height="50" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td width="20">&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td width="20">&nbsp;
                                </td>
                                <td width="460">&nbsp;
                                </td>
                                <td width="20">&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td width="20">&nbsp;
                                </td>
                                <td width="460">&nbsp;
                                </td>
                                <td width="20">&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td width="20">&nbsp;
                                </td>
                                <td width="460">&nbsp;
                                </td>
                                <td width="20">&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td width="20" height="22">&nbsp;
                                </td>
                                <td width="460" height="22" align="right" valign="top">&nbsp;&nbsp;<asp:ImageButton ID="imgbtnskucheckcontinue" runat="server" CausesValidation="false"
                                    ImageUrl="~/icon small/continue.gif" />
                                </td>
                                <td width="20" height="22">&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td>&nbsp;
                                </td>
                                <td>&nbsp;
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                    </table>
                </ContentTemplate>
            </ajax:UpdatePanel>
        </asp:Panel>
        <%--------------------------------------------------------- Modal Popup for Help ----------------------------------------------------%>
        <ajaxToolkit:ModalPopupExtender ID="mpeModalHelp" BehaviorID="mpeModalHelp" runat="server"
            TargetControlID="Button2" PopupControlID="pnlhelp" BackgroundCssClass="modalBackground"
            DropShadow="false" PopupDragHandleControlID="pnlhelp" />
        <asp:Button ID="Button2" runat="server" Style="display: none;" />
        <asp:Panel ID="pnlhelp" CssClass="modalPopup" Style="display: none; width: 333px;"
            runat="server">
            <ajax:UpdatePanel ID="UpdatePanel6" runat="server">
                <ContentTemplate>
                    <table style="width: 100%;" cellspacing="0" cellpadding="0" border="0">
                        <tr style="background-image: url(images/popup_01.gif);">
                            <td align="left">
                                <asp:Label ID="lblLoginHelp" Text="Edit Help Screen Log In" runat="server" CssClass="stylePopupHeader"></asp:Label>
                            </td>
                            <td align="right">
                                <asp:ImageButton ID="btncloseHelp" runat="server" OnClientClick="$find('mpeModalHelp').hide();"
                                    ImageUrl="~/Images/close1.gif" />
                                
                            </td>
                        </tr>
                    </table>
                    <table style="width: 100%;" cellspacing="0" cellpadding="0" border="0">
                        <tr>
                            <td align="left" class="style1a" style="height: 4px" colspan="4"></td>
                        </tr>
                        <tr>
                            <td align="left" class="style1a">
                                <asp:Label ID="lblEmail" Text="Email:" runat="server" CssClass="labelHead"></asp:Label>
                                <asp:Label ID="lblreqEmail" Text="*" runat="server" Font-Bold="True" Visible="false"
                                    Font-Names="Arial" Font-Size="10pt" ForeColor="red"></asp:Label>
                            </td>
                            <td align="left" class="style2a" colspan="2">
                                <asp:TextBox ID="txtemail" runat="server" Style="position: static" Width="184px"
                                    TabIndex="1" CausesValidation="False" CssClass="textboxall"></asp:TextBox>
                            </td>
                            <td align="left"></td>
                            <td align="left"></td>
                        </tr>
                        <tr>
                            <td align="left" class="style1a" style="height: 2px" colspan="4"></td>
                        </tr>
                        <tr>
                            <td align="left" class="style1a">
                                <asp:Label ID="lblpwd" Text="Password:" runat="server" CssClass="labelHead"></asp:Label>
                                <asp:Label ID="lblreqPwd" Text="*" Visible="false" runat="server" Font-Bold="True"
                                    Font-Names="Arial" Font-Size="10pt" ForeColor="red"></asp:Label>
                            </td>
                            <td align="left" class="style2a">
                                <asp:TextBox ID="txtpwd" runat="server" Style="position: static" Width="184px" TextMode="Password"
                                    MaxLength="13" TabIndex="2" CausesValidation="False" CssClass="textboxall"></asp:TextBox>
                            </td>
                            <td align="left"></td>
                            <td align="left"></td>
                            <td></td>
                        </tr>
                        <tr>
                            <td align="left" class="style1a" style="height: 4px" colspan="4"></td>
                        </tr>
                    </table>
                    <table style="width: 100%;" cellspacing="0" cellpadding="0" border="0">
                        <tr>
                             <td style="width:10%"></td>
                            <td style="width:10%"></td>
                            <td style="width:10%"></td>
                            <td class="style12"></td>
                            <td class="style11" align="left" colspan="2">
                                <asp:ImageButton ID="imgOKServiceHelp" runat="server" ImageUrl="~/icon small/ok.gif"
                                    TabIndex="3" />
                                <asp:ImageButton ID="imgCancelHelp" runat="server" ImageUrl="~/icon small/cancel.gif"
                                    TabIndex="4" />&nbsp;&nbsp;
                            </td>
                           
                        </tr>
                        <tr>
                            <td colspan="7">
                                <span class="labelall">NOTE:&nbsp; Only officials of Computer Perfect are authorized
                                to modify all help screens. </span>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </ajax:UpdatePanel>
        </asp:Panel>
        <!------------------------------------------ Edit Help Popup ------------------------------------------->
        <ajaxToolkit:ModalPopupExtender ID="mpeEditHelp" BehaviorID="mpeEditHelp" runat="server"
            TargetControlID="Button1" PopupControlID="pnlEdit" BackgroundCssClass="modalBackground"
            DropShadow="false" PopupDragHandleControlID="pnlEdit" X="40" Y="5" />
        <asp:Button ID="Button1" Style="display: none;" runat="server" />
        <asp:Panel ID="pnlEdit" CssClass="modalPopup" Style="display: none; width: 452px;"
            runat="server">
            <ajax:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>
                    <table style="width: 451px;" cellspacing="0" cellpadding="0" border="0">
                        <tr style="font-weight: bold; color: #ffffff; font-family: Verdana">
                            <%--  <td style="width: 4px"></td>--%>
                            <td width="500">
                                <table style="width: 100%;" cellspacing="0" cellpadding="0" border="0">
                                    <tr style="background-image: url(images/popup_01.gif);">
                                        <td align="left">
                                            <asp:Label ID="lnkEditHelp" runat="server" CssClass="stylePopupHeader" Style="position: static">Edit - Help</asp:Label>
                                        </td>
                                        <td align="right">
                                            <asp:ImageButton ID="btnedithelpclose" runat="server" OnClientClick="$find('mpeEditHelp').hide();"
                                                ImageUrl="~/Images/close1.gif" />
                                        </td>
                                    </tr>
                                </table>
                                <table style="width: 100%; height: 190px;" cellspacing="0"> 
                                    <tr style="height:3px">
                                        <td align="left"></td>
                                        <td align="left" valign="middle" class="style2a" colspan="2">
                                           
                                        </td>
                                        <td align="left">
                                        </td>
                                        <td align="left">
                                        </td>
                                    </tr>                                   
                                    <tr>
                                        <td align="left"></td>
                                        <td align="left" valign="middle" class="style2a" colspan="2">
                                            <asp:TextBox ID="txttitle" runat="server" Style="width:100%;padding-top:2px"
                                                TabIndex="1" CssClass="textboxall"></asp:TextBox>
                                        </td>
                                        <td align="left">
                                        </td>
                                        <td align="left">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left"></td>
                                        <td align="left" class="style2a" valign="top" colspan="2">
                                            <%--<asp:TextBox ID="txtHelpDesc" runat="server" Style="position: static" Width="397px"
                                                MaxLength="1000" TextMode="MultiLine" TabIndex="2" Height="158px" CssClass="textboxall"></asp:TextBox>--%>
                                            <CKEditor:CKEditorControl ID="txtHelpDesc" BasePath="~/ckeditor" runat="server" Width="99%" Height="130px" >
                                            </CKEditor:CKEditorControl>
                                        </td>
                                        <td align="left">&nbsp;
                                        </td>
                                        <td></td>
                                    </tr>
                                   
                                </table>
                                <table style="width: 100%;">
                                    <tr>
                                        <td></td>
                                        <td class="style9" width="73px">
                                            <asp:ImageButton ID="imgSaveServiceHelp" runat="server" ImageUrl="~/icon small/saveexit.gif"
                                                TabIndex="3" />
                                        </td>
                                        <td class="style3" width="75px">
                                            <asp:ImageButton ID="imgCancelServiceHelp" runat="server" ImageUrl="~/icon small/cancelexit.gif"
                                                TabIndex="4" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <%--<td style="width: 7px"></td>--%>
                            <td>
                                <asp:HiddenField ID="hidHelpId" runat="server" />
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </ajax:UpdatePanel>
        </asp:Panel>
    </form>
</body>
</html>
