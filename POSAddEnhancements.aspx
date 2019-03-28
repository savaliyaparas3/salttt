<%@ Page Language="VB" MasterPageFile="~/POSAdminMaster.master" AutoEventWireup="false" CodeFile="POSAddEnhancements.aspx.vb" Inherits="POSAddEnhancements" %>

<%--title="Computer Perfect-Point of Sale-Enhancements"--%>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <ajax:UpdatePanel ID="updatepanel1" runat="server">
        <ContentTemplate>
            <table width="100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td width="100%" height="32" align="left" valign="middle" background="images/tcrnr8.gif">
                        <span class="styleheader" style="width:100%">
                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td align="left" valign="bottom">
                                        <asp:Label ID="lblaccessact" Text="Enhancements" Font-Size="11pt" runat="server"></asp:Label>
                                    </td>
                                    <td align="right" valign="bottom">&nbsp;<%--<asp:Label ID="sysadmin" Text="| System Administration |" 
                        ForeColor="#FF9933" runat="server" Font-Size="11pt"></asp:Label>--%>
                                        <asp:LinkButton ID="lbtaddascrepairfacility" runat="server" CausesValidation="False"
                                            CssClass="" Font-Bold="False" Font-Names="Arial" Font-Size="11pt" ForeColor="white"
                                            Style="position: static" PostBackUrl="~/POSEditEnhancements.aspx">| Add Enhancements |</asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                        </span>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </ajax:UpdatePanel>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
    <script language="javascript" type="text/javascript">
        function delete_msg() {
            var response = confirm('Are you sure you want to delete this record? Click [Ok] to proceed or [Cancel] to abort.');
            if (response == true) {
                return true;
            }
            else {
                return false;
            }

        }
    </script>
    <ajax:UpdatePanel ID="updatepanel2" runat="server">
        <ContentTemplate>
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <%-- <tr>
                        <td width="754" height="24" align="left" valign="top" bgcolor="#CEE6FF"><img src="images/crnr1w.gif" alt="" width="8" height="8" /></td>
                        <td width="25" height="24" align="right" valign="top" bgcolor="#CEE6FF"><img src="images/crnr2w.gif" alt="" width="25" height="24" /></td>
                      </tr>
                      <tr>
                        <td colspan="2" align="left" valign="top" bgcolor="#CEE6FF">
                        </td>
                      </tr>
                      <tr>
                        <td width="754" height="14" align="left" valign="bottom" bgcolor="#CEE6FF"><img src="images/crnr3w.gif" alt="" width="8" height="8" /></td>
                        <td width="25" height="14" align="right" valign="bottom" bgcolor="#CEE6FF"><img src="images/spacer.gif" alt="" width="1" height="1" /><img src="images/crnr4w.gif" alt="" width="8" height="8" /></td>
                      </tr>--%>
                <tr>
                    <td colspan="2" style="height: 5px;"></td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:GridView ID="gvEnhancement" runat="server" AutoGenerateColumns="False"
                            Width="100%" BorderColor="White" BorderWidth="1px" AllowSorting="True"
                            EmptyDataText="No matching records found." DataKeyNames="iEnhancementId"
                            AllowPaging="True" PageSize="9">
                            <PagerStyle HorizontalAlign="Right" VerticalAlign="Middle" Font-Size="10pt" />
                            <HeaderStyle CssClass="header" HorizontalAlign="Left" VerticalAlign="Middle" />
                            <AlternatingRowStyle CssClass="alterrow" />
                            <RowStyle CssClass="row" />
                            <EmptyDataRowStyle CssClass="labelall" ForeColor="Red" HorizontalAlign="Center" VerticalAlign="Middle" />
                            <Columns>
                                <asp:BoundField DataField="ienhancementId">
                                    <ItemStyle Width="1%" />
                                    <HeaderStyle Width="1%" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Created Date">
                                    <ItemTemplate>
                                        &nbsp;<asp:Label ID="lblcreateddate" runat="server" Text='<%# Bind("createdate") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderTemplate>
                                        &nbsp;<asp:LinkButton ID="lbtnCreatedate" runat="server" CommandArgument="createdate"
                                            CommandName="Sort" ForeColor="White">Created Date</asp:LinkButton>
                                        <asp:ImageButton ID="imgCreatedate" runat="server" />
                                    </HeaderTemplate>
                                    <HeaderStyle Width="7%" />
                                    <ItemStyle Width="7%" HorizontalAlign="left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Expiration Date">
                                    <ItemTemplate>
                                        &nbsp;<asp:Label ID="lblExpirationdate" runat="server"
                                            Text='<%# Bind("expiryDate") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderTemplate>
                                        &nbsp;<asp:LinkButton ID="lbtnExpiration" runat="server" CommandArgument="expiryDate"
                                            CommandName="Sort" ForeColor="White">Expiration Date</asp:LinkButton>
                                        <asp:ImageButton ID="imgExpirationdate" runat="server" />
                                    </HeaderTemplate>
                                    <HeaderStyle Width="8%" />
                                    <ItemStyle Width="8%" HorizontalAlign="left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Enhancement">
                                    <ItemTemplate>
                                        &nbsp;<asp:Label Style="display: none;" ID="lblEnhancement" runat="server" Text='<%# Bind("enhancement") %>'></asp:Label>
                                        <asp:Literal ID="litenhancement" runat="server"></asp:Literal>
                                        <asp:HiddenField ID="idhiddenenhancement" Visible="false" runat="server" />
                                    </ItemTemplate>
                                    <HeaderTemplate>
                                        &nbsp;<asp:LinkButton ID="lbtnEnhancement" runat="server" CommandArgument="enhancement"
                                            CommandName="Sort" ForeColor="White">Enhancement</asp:LinkButton>
                                        <asp:ImageButton ID="imgEnhancement" runat="server" />
                                    </HeaderTemplate>
                                    <HeaderStyle Width="30%" />
                                    <ItemStyle Width="30%" HorizontalAlign="left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Edit">
                                    <ItemTemplate>
                                        &nbsp;<asp:LinkButton ID="lbtnEdit" runat="server">Edit</asp:LinkButton>
                                    </ItemTemplate>
                                    <HeaderTemplate>
                                        &nbsp;<asp:Label ID="lblEdit" CssClass="labelall" ForeColor="White" runat="server" Text="Edit"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemStyle Width="3%" />
                                    <HeaderStyle Width="3%" />
                                </asp:TemplateField>
                                <asp:CommandField HeaderText="Delete" SelectText="Delete" ShowSelectButton="True" UpdateText="Delete">
                                    <ItemStyle Width="3%" />
                                    <HeaderStyle Width="3%" />
                                </asp:CommandField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="center">
                        <asp:Label ID="lblErrormessage" runat="server" CssClass="labelall" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </ajax:UpdatePanel>
</asp:Content>
