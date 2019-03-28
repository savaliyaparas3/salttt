<%@ Page Language="VB" MasterPageFile="~/POSMaster.master" AutoEventWireup="false" CodeFile="POSAddEmployee.aspx.vb" Inherits="POSAddEmployee" title="Computer Perfect - Point of Sale - Add Employee" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <ajax:UpdatePanel ID="UpdatePanelHeader" runat="server">
    <ContentTemplate>
        <table cellpadding="0" cellspacing="0">
        <tr>
            <td width="779" height="32" align="left" valign="middle"  background="images/tcrnr8.gif">
            <span class="styleheader">
            <table border="0" cellspacing="0" cellpadding="0" >
            <tr>
                <td width="110" align="center" valign="bottom" >
                    <asp:Label ID="lblHeader" runat="server" Text="Add Employee"></asp:Label>
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


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">

 
    <script type="text/javascript"  language="JavaScript" src="myscript/date-picker.js"></script>
 <script type="text/javascript"  language="JavaScript" src="myscript/comparedate.js"></script>


<table cellpadding="0" cellspacing="0">
<tr>
    <td>
        <table cellpadding="0" cellspacing="0">
        <tr>
            <td style="height:5px;"></td>
        </tr>
        <tr>
            <td align="left"> 
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                 <td width="754" align="left" valign="top" bgcolor="#CEE6FF">
                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                           <td width="5%" height="24" align="left" valign="top" ><img src="images/crnr1w.gif" alt="" width="8" height="8" /></td>
                           <td width="20%" height="24" align="left" valign="middle">
                                <asp:Label ID="Label1" runat="server" CssClass="labelall" Text="Last Name"></asp:Label></td>
                           <td width="20%" height="24" align="left" valign="middle">
                                <asp:Label ID="Label2" runat="server" CssClass="labelall" Text="First Name"></asp:Label></td>
                           <td width="20%" height="24" align="left" valign="middle">
                                <asp:Label ID="Label3" runat="server" CssClass="labelall" Text="Middle Name"></asp:Label></td>
                   </tr>
                   </table>
                 </td>
                 <td width="25" height="24" align="right" valign="top" bgcolor="#CEE6FF"><img src="images/crnr2w.gif" alt="" width="25" height="24" /></td>
            </tr>
            <tr>
                <td width="754" colspan="2" align="left" valign="top" bgcolor="#CEE6FF">
                <table width="100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td width="5%"></td>
                    <td width="20%">
                        <asp:TextBox ID="txtlastname" runat="server" CssClass="textboxall" 
                            Width="150px" TabIndex="1"></asp:TextBox>
                    </td>
                    <td width="20%">
                        <asp:TextBox ID="txtFirstName" runat="server" CssClass="textboxall" 
                            Width="150px" TabIndex="2"></asp:TextBox>
                    </td>
                    <td width="20%">
                        <asp:TextBox ID="txtMiddleName" runat="server"  CssClass="textboxall" 
                            Width="150px" TabIndex="3"  ></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td width="5%">&nbsp;</td>
                    <td width="20%">
                        <asp:Label ID="Label21" runat="server" CssClass="labelall" Text="User ID"></asp:Label>
                    &nbsp;<asp:RequiredFieldValidator ID="reqUserid" runat="server" 
                            ControlToValidate="txtUsername" ErrorMessage="Employee ID can't be empty.">*</asp:RequiredFieldValidator>
                    </td>
                    <td width="20%">
                        <asp:Label ID="Label22" runat="server" CssClass="labelall" Text="Password"></asp:Label>
                    &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                            ControlToValidate="txtPassword" 
                            ErrorMessage="Employee Password can't be empty.">*</asp:RequiredFieldValidator>
                    </td>
                    <td width="20%">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td width="5%">&nbsp;</td>
                    <td width="20%">
                        <asp:TextBox ID="txtUsername" runat="server" CssClass="textboxall" TabIndex="4" 
                            Width="150px"></asp:TextBox>
                    </td>
                    <td width="20%">
                        <asp:TextBox ID="txtPassword" runat="server" CssClass="textboxall" TabIndex="5" 
                            Width="150px" TextMode="Password"></asp:TextBox>
                    </td>
                    <td width="20%">
                        &nbsp;</td>
                </tr>
                 </table>
                </td>
            </tr>
            <tr>
                <td width="754" height="14" align="left" valign="bottom" bgcolor="#CEE6FF"><img src="images/crnr3w.gif" alt="" width="8" height="8" /></td>
                <td width="25" height="14" align="right" valign="bottom" bgcolor="#CEE6FF"><img src="images/spacer.gif" alt="" width="1" height="1" /><img src="images/crnr4w.gif" alt="" width="8" height="8" /></td>
            </tr>
            </table>
        </td>
        </tr>
        <tr>
            <td style="height:5px;"></td>
        </tr>
        <tr>
            <td align="left"> 
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td width="754" height="24" align="left" valign="top" bgcolor="#c5f2fa">
                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                           <td width="5%" height="24" align="left" valign="top" >
                                <img src="images/crnr5w.gif" alt="" width="8" height="8" /></td>
                           <td width="20%" height="24" align="left" valign="middle">
                                <asp:Label ID="Label6" runat="server" Text="Address" CssClass="labelall"></asp:Label></td>
                           <td width="20%" height="24" align="left" valign="middle">
                                </td>
                           <td width="20%" height="24" align="left" valign="middle">
                                </td>
                   </tr>
                   </table>
                   </td>         
                   <td width="25" height="24" align="right" valign="top" bgcolor="#c5f2fa"><img src="images/crnr6w.gif" alt="" width="25" height="24" /></td>
                </tr>
                <tr>
                    <td width="754" colspan="2" align="left" valign="top" bgcolor="#c5f2fa">
                    <table width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                            <td width="5%">
                                &nbsp;</td>
                            <td colspan="3">
                                <asp:TextBox ID="txtAddress" runat="server" Width="88%" CssClass="textboxall" 
                                    TabIndex="6"></asp:TextBox>
                            </td>
                          
                    </tr>
                    <tr>
                            <td width="5%">
                                &nbsp;</td>
                            <td width="20%">
                                <asp:Label ID="Label8" runat="server" CssClass="labelall" Text="City"></asp:Label>
                            </td>
                            <td width="20%">
                                <asp:Label ID="Label17" runat="server" Text="State" CssClass="labelall"></asp:Label>
                            </td>
                            <td width="20%">
                                <asp:Label ID="Label18" runat="server" Text="Zip:" CssClass="labelall"></asp:Label>
                            </td>
                    </tr>
                    <tr>
                            <td>
                                </td>
                            <td>
                                <asp:TextBox ID="txtCity" runat="server" Width="150px" CssClass="textboxall" 
                                    TabIndex="7"></asp:TextBox>
                            </td>
                            <td style="width: 9%">
                                <asp:TextBox ID="txtState" runat="server" Width="75px" MaxLength="2" 
                                    CssClass="textboxall" TabIndex="8"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtZip" runat="server" Width="150px" 
                                    onkeypress="javascript:return phnumeric();" MaxLength="6" 
                                    CssClass="textboxall" TabIndex="9"></asp:TextBox>
                            </td>
                    </tr>
                    <tr>
                            <td></td>
                            <td>
                                <asp:Label ID="Label19" runat="server" Text="Phone" CssClass="labelall"></asp:Label></td>
                            <td>
                                <asp:Label ID="Label20" runat="server" Text="Email" CssClass="labelall"></asp:Label></td>
                            <td></td>
                    </tr>
                    <tr>
                            <td></td>
                            <td>
                                <asp:TextBox ID="txtPhone" runat="server" Width="150px" 
                                    onkeypress="javascript:return phnumeric();" CssClass="textboxall" 
                                    TabIndex="10"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="txtEmail" runat="server" Width="150px" CssClass="textboxall" 
                                    TabIndex="11"></asp:TextBox></td>
                            <td></td>
                    </tr>
                    </table>
                    </td>
               </tr>
               <tr>
                     <td width="754" height="14" align="left" valign="bottom" bgcolor="#c5f2fa"><img src="images/crnr7w.gif" alt="" width="8" height="8" /></td>
                     <td width="25" height="14" align="right" valign="bottom" bgcolor="#c5f2fa"><img src="images/spacer.gif" alt="" width="1" height="1" /><img src="images/crnr8w.gif" alt="" width="8" height="8" /></td>
               </tr>
               </table>
           </td>
        </tr>
        <tr>
            <td style="height:5px;"></td>
        </tr>
        <tr>
            <td align="left"> 
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                 <td width="754" height="24" align="left" valign="top" bgcolor="#DCDEFF">
                 <table width="100%" border="0" cellspacing="0" cellpadding="0">
                 <tr>
                           <td width="5%" height="24" align="left" valign="top" >
                                <img src="images/crnr9w.gif" alt="" width="8" height="8" /></td>
                           <td width="20%" height="24" align="left" valign="middle">
                                <asp:Label ID="Label11" runat="server" CssClass="labelall" Text="S/S Number"></asp:Label></td>
                           <td width="20%" height="24" align="left" valign="middle">
                                <asp:Label ID="Label12" runat="server" CssClass="labelall" Text="Hourly rate"></asp:Label></td>
                           <td width="20%" height="24" align="left" valign="middle">
                                <asp:Label ID="Label14" runat="server" CssClass="labelall" Text="Us Citizen"></asp:Label></td>
                 </tr>
                 </table>
                 </td>
                 <td width="25" height="24" align="right" valign="top" bgcolor="#DCDEFF"><img src="images/crnr10w.gif" alt="" width="25" height="24" /></td>
            </tr>
            <tr>
                 <td width="754" colspan="2" align="left" valign="top" bgcolor="#DCDEFF">
                 <table width="100%" cellpadding="0" cellspacing="0">
                 <tr>
                           <td width="5%">
                               </td>
                           <td width="20%">
                               <asp:TextBox ID="txtssnumber" runat="server" Width="150px" 
                                   onkeypress="javascript:return_phnumeric();" CssClass="textboxall" 
                                   TabIndex="12"></asp:TextBox>
                           </td>
                           <td width="20%">
                               <asp:TextBox ID="txtHourlyrate" runat="server" Width="150px" 
                                   CssClass="textboxall"></asp:TextBox>
                           </td>
                           <td width="20%">
                                <asp:DropDownList ID="ddlUscitizen" runat="server" TabIndex="13" Width="152px" 
                                    CssClass="dropdownall">
                                    <asp:ListItem Value="1">Yes</asp:ListItem>
                                    <asp:ListItem Value="0">No</asp:ListItem>
                                </asp:DropDownList>
                           </td>
                  </tr>
                  <tr>
                        <td></td>
                        <td>
                            <asp:Label ID="Label13" runat="server" CssClass="labelall" 
                                Text="Birthdate"></asp:Label>
                         </td>
                        <td>
                            <asp:Label ID="Label16" runat="server" CssClass="labelall" Text="Employed"></asp:Label>
                         </td>
                        <td>
                            <asp:Label ID="lbllock" runat="server" CssClass="labelall " Text="Terminated"></asp:Label>
                        </td>
                  </tr>
                   <tr>
                        <td></td>
                        <td>
                            <asp:TextBox ID="txtBirthdate" runat="server" Width="150px" 
                                CssClass="textboxall" TabIndex="14"></asp:TextBox>
                        </td>
                        <td>
                             <input id="txtemployed" type="text" runat="server" 
                                 style="width: 150px; position:relative;" class="textboxall"/>
                                   &nbsp;&nbsp;<input id="btndatefrom" name="btndatefrom" 
                                   onclick="javascript:show_calendar(document.getElementById('ctl00_ContentPlaceHolder2_txtemployed'));" 
                                   onmouseout="window.status='';return true;" 
                                   onmouseover="window.status='Date Picker';return true;" 
                                   style="BORDER-TOP-WIDTH: 0px; BORDER-LEFT-WIDTH: 0px; BACKGROUND-IMAGE: url(Images/Calendar.bmp); BORDER-BOTTOM-WIDTH: 0px; WIDTH: 24px; POSITION: static; HEIGHT: 22px; BORDER-RIGHT-WIDTH: 0px" 
                                   type="button" tabindex="15" />
                        </td>
                        <td>
                             <input id="txtTermineted" style="width: 150px;position:relative;" type="text" 
                                 runat="server" class="textboxall"/>
                                        &nbsp;&nbsp;<input id="btndateto" name="btndateto" 
                                        onclick="javascript:show_calendar(document.getElementById('ctl00_ContentPlaceHolder2_txtTermineted'));" 
                                        onmouseover="window.status='Date Picker';return true;" 
                                        style="border-top-width : 0px; border-left-width : 0px; background-image: url(Images/Calendar.bmp); border-bottom-width: 0px; width: 24px; position: static; height: 22px; border-right-width: 0px" 
                                        type="button" tabindex="16" />
                       </td>
                    </tr>
                   <tr>
                        <td>&nbsp;</td>
                        <td>
                            <asp:Label ID="lblNotes" runat="server" CssClass="labelall" Text="Notes"></asp:Label>
                        </td>
                        <td>
                             &nbsp;</td>
                        <td>
                             &nbsp;</td>
                    </tr>
                   <tr>
                        <td>&nbsp;</td>
                        <td colspan="3">
                            <asp:TextBox ID="txtNotes" runat="server" TextMode="MultiLine" Width="88%" 
                                CssClass="textboxall"></asp:TextBox>
                        </td>
                    </tr>
                 </table>
                 </td>
            </tr>
            <tr>
                  <td width="754" height="14" align="left" valign="bottom" bgcolor="#DCDEFF"><img src="images/crnr11w.gif" alt="" width="8" height="8" /></td>
                  <td width="25" height="14" align="right" valign="bottom" bgcolor="#DCDEFF"><img src="images/spacer.gif" alt="" width="1" height="1" /><img src="images/crnr12w.gif" alt="" width="8" height="8" /></td>
            </tr>
            </table>
            </td>
         </tr>
         <tr>
            <td style="height:5px;"></td>
            <td></td>
         </tr>
         <tr>
            <td align="right" colspan="2">
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:ImageButton id="btnSave" runat="server" Width="60px" 
                        ImageUrl="~/Images/save.gif" CssClass="buttonall" 
                        ValidationGroup="storedetails" Height="20px" CausesValidation="true"></asp:ImageButton>
                    &nbsp;
                <asp:ImageButton id="btnCancel1" runat="server" ImageUrl="~/Images/cancel.gif" Width="60px" Height="20px" CausesValidation="false" CssClass="buttonall" /></asp:ImageButton>
                    &nbsp;&nbsp; </td>
         </tr>
         </table>
    </td>
</tr>
</table>









<%--<table cellpadding="0" cellspacing="0" width="100%">
   <tr>
        <td style="height:20px;"></td>
   </tr>
   <tr>
        <td align="left"> 
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td width="754" height="24" align="left" valign="top" bgcolor="#CEE6FF"><img src="images/crnr1w.gif" alt="" width="8" height="8" /></td>
                <td width="25" height="24" align="right" valign="top" bgcolor="#CEE6FF"><img src="images/crnr2w.gif" alt="" width="25" height="24" /></td>
            </tr>
            <tr>
                <td colspan="2" align="left" valign="top" bgcolor="#CEE6FF">
                <table width="100%" cellspacing="0" cellpadding="0">
                <tr>
                    <td width="2%"></td>
                    <td width="14%" align="right">
                        <asp:Label ID="Label8" runat="server" Text="First Name"></asp:Label>
                    </td>
                    <td width="15%" class="style1">&nbsp;</td>
                    <td align="right" width="10%">
                      </td>
                    <td align="left">&nbsp;</td>    
                 </tr>
                </table>
                </td>
            </tr>
            <tr>
                <td width="754" height="14" align="left" valign="bottom" bgcolor="#CEE6FF"><img src="images/crnr3w.gif" alt="" width="8" height="8" /></td>
                <td width="25" height="14" align="right" valign="bottom" bgcolor="#CEE6FF"><img src="images/spacer.gif" alt="" width="1" height="1" /><img src="images/crnr4w.gif" alt="" width="8" height="8" /></td>
            </tr>
            </table>
        </td>
   </tr>
   <tr>
        <td style="height:20px;"></td>
   </tr>
   <tr>
        <td align="left"> 
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td width="754" height="24" align="left" valign="top" bgcolor="#c5f2fa"><img src="images/crnr5w.gif" alt="" width="8" height="8" /></td>
                <td width="25" height="24" align="right" valign="top" bgcolor="#c5f2fa"><img src="images/crnr6w.gif" alt="" width="25" height="24" /></td>
            </tr>
            <tr>
                <td colspan="2" align="left" valign="top" bgcolor="#c5f2fa">
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td width="2%"></td>
                    <td width="15%" align="right">
                        <asp:Label ID="lblAddress" runat="server" Text="Address : " CssClass="labelall"></asp:Label>
                     </td>
                    <td colspan="6">
                        &nbsp;<asp:TextBox ID="txtStoreaddress" runat="server" Width="76%" 
                            CssClass="textboxall" MaxLength="30"></asp:TextBox>
                     </td>
                </tr>
                <tr>
                    <td></td>
                    <td align="right">
                        <asp:Label ID="lblCity" runat="server" Text="City : " CssClass="labelall"></asp:Label>
                     </td>
                    <td width="15%" >
                        &nbsp;<asp:TextBox ID="txtStorecity" runat="server" CssClass="textboxall" 
                            MaxLength="30"></asp:TextBox>
                     </td>
                    <td align="right" width="10%">
                        <asp:Label ID="lblState" runat="server" Text="State :" CssClass="labelall"></asp:Label>
                     </td>
                    <td width="11%">
                        &nbsp;<asp:TextBox ID="txtStorestate" runat="server" Width="50px" 
                            CssClass="textboxall" MaxLength="2"></asp:TextBox>
                     </td>
                    <td align="right" width="10%">
                        <asp:Label ID="lblZip" runat="server" Text="Zip :" CssClass="labelall"></asp:Label>
                     </td>
                     <td>
                         &nbsp;<asp:TextBox ID="txtStorezip" runat="server" CssClass="textboxall" 
                             MaxLength="5"></asp:TextBox>
                                        </td>
                </tr>
                <tr>
                    <td></td>
                    <td align="right">
                        <asp:Label ID="lblPhone" runat="server" Text="Phone :" CssClass="labelall"></asp:Label>
                     </td>
                    <td>
                        &nbsp;<asp:TextBox ID="txtStorephone" runat="server" CssClass="textboxall" 
                            MaxLength="19"></asp:TextBox>
                     </td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td align="right">
                        <asp:Label ID="lblFax" runat="server" Text="Fax :" CssClass="labelall"></asp:Label>
                     </td>
                    <td align="left">
                        &nbsp;<asp:TextBox ID="txtStorefax" runat="server" CssClass="textboxall" 
                            MaxLength="19"></asp:TextBox>
                     </td>
                </tr>
                </table>
                </td>
            </tr>
            <tr>
                <td width="754" height="14" align="left" valign="bottom" bgcolor="#c5f2fa"><img src="images/crnr7w.gif" alt="" width="8" height="8" /></td>
                <td width="25" height="14" align="right" valign="bottom" bgcolor="#c5f2fa"><img src="images/spacer.gif" alt="" width="1" height="1" /><img src="images/crnr8w.gif" alt="" width="8" height="8" /></td>
            </tr>
            </table>
        </td>
  </tr>
    <tr>
        <td style="height:20px;"></td>
    </tr>
    <tr>
        <td align="left"> 
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
              <tr>
                <td width="754" height="24" align="left" valign="top" bgcolor="#DCDEFF"><img src="images/crnr9w.gif" alt="" width="8" height="8" /></td>
                <td width="25" height="24" align="right" valign="top" bgcolor="#DCDEFF"><img src="images/crnr10w.gif" alt="" width="25" height="24" /></td>
              </tr>
              <tr>
                <td colspan="2" align="left" valign="top" bgcolor="#DCDEFF">
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                  <tr>
                  <td width="2%"></td>
                    <td width="15%" align="right">
                        <asp:Label ID="Label7" runat="server" Text="Web Address :" CssClass="labelall"></asp:Label>
                      </td>
                    <td>
                        &nbsp;<asp:TextBox ID="txtStorewebadd" runat="server" Width="76%" 
                            CssClass="textboxall" MaxLength="40"></asp:TextBox>
                      </td>
                  </tr>
                </table></td>
              </tr>
              <tr>
                <td width="754" height="14" align="left" valign="bottom" bgcolor="#DCDEFF"><img src="images/crnr11w.gif" alt="" width="8" height="8" /></td>
                <td width="25" height="14" align="right" valign="bottom" bgcolor="#DCDEFF"><img src="images/spacer.gif" alt="" width="1" height="1" /><img src="images/crnr12w.gif" alt="" width="8" height="8" /></td>
              </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td style="height:95px;">
        </td>
    </tr>
    <tr>
        <td style="height:26px;" align="right">
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:ImageButton id="btnOK1" runat="server" Width="60px" ImageUrl="~/Images/ok.gif" CssClass="buttonall" 
                ValidationGroup="storedetails" Height="20px" ></asp:ImageButton>
            &nbsp;
        <asp:ImageButton id="btnCancel1" runat="server" ImageUrl="~/Images/cancel.gif" Width="60px" Height="20px" CausesValidation="False" CssClass="buttonall"></asp:ImageButton>
            &nbsp;&nbsp;
        </td>
    </tr>
</table>--%>



</asp:Content>

