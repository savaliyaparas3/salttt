<%@ Page Language="VB" AutoEventWireup="false" CodeFile="POSAddEditDonation.aspx.vb" Inherits="POSAddEditDonation" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Add/ Edit Donation</title>
    <base target="_self" />
    <script type="text/javascript" language="javascript">

        function CloseWindow() {
            window.returnValue = '0';
            window.close();
            return false;
        }
        function CloseWindowWithRetValue() {
            window.returnValue = '1';
            window.close();
            return false;
        }
    
    </script>
     <link href="1style.css" rel="stylesheet" type="text/css" />
    <link href="CSS/Mainstyle.css" rel="stylesheet" type="text/css" />
    <link href="CSS/ringsalestyle.css" rel="stylesheet" type="text/css" />
    <link type="text/css" rel="stylesheet" href="Myscript/pos_calander_js/calendar.css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
     <ajax:ScriptManager ID="ScriptManager1" EnablePageMethods="true" runat="server">
        </ajax:ScriptManager>
        <ajax:UpdatePanel ID="updatepaneleditpo" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                
                <table cellpadding="0" cellspacing="0" border="0" width="100%" style="background-color:#CEE6FF;">
                    <tr>
                        <td colspan="5" valign="middle" align="left" style="background-image: url(images/popup_01.gif);
                            padding-left: 5px;" height="20px">
                            &nbsp;&nbsp;<asp:Label ID="lblHeader" CssClass="stylePopupHeader" Text="Add/ Edit Donation" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 10px" colspan="5">
                        </td>
                    </tr>
                    <tr>
                        <td width="2%">
                        </td>
                        <td width="33%">
                            <asp:Label ID="lblOrgCode" CssClass="labelall" Text="Organization Code :" runat="server"></asp:Label>
                        </td>
                        <td width="60%">
                        <asp:TextBox ID="txtorgcode" style="text-transform:uppercase;" onfocus="this.select();" TabIndex="1" Width="40%" ValidationGroup="donation" MaxLength="10" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvorgcode" runat="server" ControlToValidate="txtorgcode"
                                                                    Font-Size="Medium" SetFocusOnError="True" Style="position: static" ValidationGroup="donation">
                                                                    <asp:Image ID="imgorgcode" runat="server" ImageUrl="~/Images/blink1.gif" />
                                                                </asp:RequiredFieldValidator>
                        </td>
                        <td width="1%"></td>
                        <td width="1%">
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 10px" colspan="5">
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td valign="top">
                            <asp:Label ID="lblOrgName" CssClass="labelall" Text="Organization Name :" runat="server"></asp:Label>
                        </td>
                        <td valign="bottom">
                      <asp:TextBox ID="txtorgname" onfocus="this.select();" Width="92%" TabIndex="2" ValidationGroup="donation" MaxLength="80" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvorgname" runat="server" ControlToValidate="txtorgname"
                                                                    Font-Size="Medium" SetFocusOnError="True" Style="position: static" ValidationGroup="donation">
                                                                    <asp:Image ID="imgdonationname" runat="server" ImageUrl="~/Images/blink1.gif" />
                                                                </asp:RequiredFieldValidator>
                        </td>
                        <td></td>
                        <td>
                        </td>
                    </tr>
                       <tr>
                        <td style="height: 15px" colspan="5">
                        </td>
                    </tr>
                    <tr id="trstatus" runat="server">
                    <td>
                        </td>
                        <td valign="top">
                            <asp:Label ID="lblstatus" CssClass="labelall" Text="Status :" runat="server"></asp:Label>
                        </td>
                        <td valign="top">
                   
                            <asp:DropDownList ID="ddlstatus" width="40%" TabIndex="3" runat="server">
                            <asp:ListItem Value="1">Active</asp:ListItem>
                             <asp:ListItem Value="0">Archived</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td></td>
                        <td>
                        </td>
                    
                    </tr>
                    <tr>
                        <td style="height: 20px" colspan="5">
                        </td>
                    </tr>
                  <tr>
                  <td></td>
                  <td></td>
                     <td align="right" style="padding-right:4%">
                            <asp:Button ID="btnSave" TabIndex="4" CssClass="alterrow" ValidationGroup="donation" runat="server"
                                Font-Bold="true" Font-Names="Arial" Height="28px"
                                Style="cursor: pointer" Text="Save" Width="70px"  />
                                  <asp:Button ID="btnCancel" TabIndex="5" runat="server" CausesValidation="false"
                                Font-Bold="true" Font-Names="Arial" CssClass="alterrow" Height="28px"
                                Style="cursor: pointer" Text="Cancel" Width="70px"  />
                                
                        </td>
                        <td>
                        </td>
                        <td></td>
                    </tr>
                    <tr>
                    <td colspan="5" style="height:60px"></td>
                    
                    </tr>
                </table>
            </ContentTemplate>
        </ajax:UpdatePanel>
    </div>
    </form>
</body>
</html>
