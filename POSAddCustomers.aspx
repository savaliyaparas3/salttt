<%@ Page Language="VB" MasterPageFile="POSMaster2.master" AutoEventWireup="false" CodeFile="POSAddCustomers.aspx.vb" Inherits="POSAddCustomers" title="Computer Perfect-Point of Sale-Add Customers" ValidateRequest="true" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
<tr>
<td width="779" height="32" align="left" valign="middle" background="images/tcrnr8.gif">


<asp:Label ID="lblheader" runat="server" Text="Customer Details" ForeColor="White" CssClass="styleheader"></asp:Label>
</td>
</tr>
</table>

</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
<script type="text/javascript" language="JavaScript" src="myscript/jsPassWord.js"></script>

<script type="text/javascript" language="javascript">


function CreditVisaMastercardNo(txtid,lblid)
	{
	   if(txtid.value.length!=0 && txtid.value.length>=15)
	    {//alert(document.getElementById(lblid).src);
	    document.getElementById('<%=ddlMonth.ClientId %>').disabled=false ;
	               document.getElementById('<%=ddlYear.ClientId %>').disabled=false;
	       if(txtid.value.substring(4,5)=="-")
	       {  var str;
	       //alert('hi');
                    str=txtid.value;
	               // document.getElementById(lblid).innerHTML='';
	                if(str.substring(0,1)=='3' && str.length==17)
                    {//alert('American');
                   document.getElementById(lblid).width='30';
                    document.getElementById(lblid).src ='icon small/card3c.gif';
                        
                    }
                    else if(str.substring(0,1)=='4' && str.length==19)
                     { 
                     document.getElementById(lblid).width='30';
                    document.getElementById(lblid).src ='icon small/card2c.gif';
                        
                     }
                     else if(str.substring(0,1)=='5' && str.length==19)
                     {
                        // alert('Credit');
                        document.getElementById(lblid).width='30';
                        document.getElementById(lblid).src ='icon small/card1c.gif';
                         
                     }
                     else if (str.substring(0,1)=='X')
                     {//alert(document.getElementById(lblid).innerHTML);
                     document.getElementById(lblid).src=document.getElementById(lblid).src
                     }
                     else
                     {
                     document.getElementById(lblid).width='0' ;
                       //document.getElementById(lblid).src='';
                        //alert('Normal');
                     }
	        }
            else
            {//alert('no');
                  if(txtid.value.length==15)  
                  {//alert('test2');
                    var str;
                    str=txtid.value;
                    if(str.substring(0,1)=='3')
                    {//alert('American');
                    document.getElementById(lblid).width='30' ;
                    document.getElementById(lblid).src ='icon small/card3c.gif';
                        txtid.value=str.substring(0,4) +'-'+ str.substring(4,10)+'-'+str.substring(10,15);
                    }
                    else
                    {
                       document.getElementById(lblid).visible=false;  //alert('Normal');
                    }
                  }
	              else if(txtid.value.length==16)
	              {//alert('test3');
	                 var str;
                     str=txtid.value;
                     if(str.substring(0,1)=='4')
                     { document.getElementById(lblid).width='30' ;
                    document.getElementById(lblid).src ='icon small/card2c.gif';
                         txtid.value=str.substring(0,4) +'-'+ str.substring(4,8)+'-'+str.substring(8,12)+'-'+str.substring(12,16);
                     }
                     else if(str.substring(0,1)=='5')
                     {
                        // alert('Credit');
                        document.getElementById(lblid).width='30' ;
                       document.getElementById(lblid).src ='icon small/card1c.gif';
                         txtid.value=str.substring(0,4) +'-'+ str.substring(4,8)+'-'+str.substring(8,12)+'-'+str.substring(12,16);
                     }
                     else
                     {
                    document.getElementById(lblid).width='0' ;
                       //document.getElementById(lblid).src='';
                        //alert('Normal');
                     }
	              }
	              
	              else
	              {
	              document.getElementById(lblid).width='0' ;
	                //document.getElementById(lblid).src ='';
	              }
	        } 
	      }
	    else
	      {
	      document.getElementById(lblid).width='0' ;
	      document.getElementById('<%=ddlMonth.ClientId %>').disabled=true  ;
	               document.getElementById('<%=ddlYear.ClientId %>').disabled=true;
	       //document.getElementById(lblid).src='';
	      }
	}
     
  function PasswordCheck(s,e)
   {
    var txtid=document.getElementById('<%= txtCreditPassWord.ClientId %>');
    bol= passwordValidation(txtid,'','') ;
        if(bol==true)
        {
          e.IsValid=true;
        }
        else
        {
            txtid.value="";
            e.IsValid=false ;
            
        }
   }   
   function comparePass(s,e)
   {
        var passid= document.getElementById('<%=txtCreditPassWord.ClientId %>').value;
        var conpassid= document.getElementById('<%=repassword.ClientId %>').value;
       
        if (passid!=conpassid)
        {
            alert('The password does not match!');
            document.getElementById('<%=repassword.ClientId %>').value=""; 
            document.getElementById('<%=txtCreditPassWord.ClientId %>').select();       
            document.getElementById('<%=txtCreditPassWord.ClientId %>').focus();      
            e.IsValid=false;
        }
        else
        {
        e.IsValid=true;
        }
   }          
 function phnumeric()
    { 
    	if((event.keyCode < 48) || (event.keyCode > 57))
		    {
			    alert("Please enter only numeric value.");
			    //document .getElementById("ct100_ContentPlaceHolder2_txtPhone").tagName 
			    event.keyCode="";
		    }
    }
 function verify()
 {
   
   if (document.getElementById('<%=txtlastname.ClientID %>').value=="")
    {
       alert("Please enter the Last Name");
       return false;
    }
    
  }
  
   function fncInputNumericValuesOnly(evt)	
    {   var e = evt ? evt : window.event;
                 if(window.event)
                {
                    if ((evt.keyCode!=8)&&(evt.keyCode!=37)&&(evt.keyCode!=38)&&(evt.keyCode!=39)&&(evt.keyCode!=40)&&(evt.keyCode<47||evt.keyCode>57))
                      {
                         evt.returnValue=false;
                       }
                        
                }
                else
                {
                    if ((evt.which!=0)&&(evt.which!=8)&&(evt.which!=37)&&(evt.which!=38)&&(evt.which!=39)&&(evt.which!=40)&&(evt.which<47||evt.which>57))
                      {
                            evt.returnValue=false;
                            evt.preventDefault(); 
                        }
                }
   }       
     
   function upperCase(e)
    { 
        var e = e ? e : window.event;
        var KeyCode = e.which ? e.which : e.keyCode;
        
                if(window.event)
                {
                    if ((KeyCode > 96) && (KeyCode < 123))
                           e.keyCode = KeyCode-32;
                            e.returnValue=true ;
                }
                 else
                 {
                    if (((KeyCode > 96) && (KeyCode < 123)) || (KeyCode!=8)&&(KeyCode!=46)&&(KeyCode!=37)&&(KeyCode!=38)&&(KeyCode!=39)&&(KeyCode!=40))
                    {
                            document.getElementById('txtSKU').value+=String.fromCharCode(KeyCode-32);
                           //e.keyCode= KeyCode-32;
                            e.returnValue=false ;
                            e.preventDefault(); 
                            }
                 }
      
    }
    function FirstupperCase(e,id)
    {  
       if (id.value.length == 0)
       {
            var e = e ? e : window.event;
            var KeyCode = e.which ? e.which : e.keyCode;
        
                if(window.event)
                {
                    if ((KeyCode > 96) && (KeyCode < 123))
                           e.keyCode = KeyCode-32;
                            e.returnValue=true ;
                }
                 else
                 {
                    if (((KeyCode > 96) && (KeyCode < 123)) || (KeyCode!=8)&&(KeyCode!=46)&&(KeyCode!=37)&&(KeyCode!=38)&&(KeyCode!=39)&&(KeyCode!=40))
                    {
                            id.value+=String.fromCharCode(KeyCode-32);
                           //e.keyCode= KeyCode-32;
                            e.returnValue=false ;
                            e.preventDefault(); 
                            }
                 }
      
        }
        return true;
     }
    
    
//		function ValidatePhone(strPhone) 
//      {
//        var regPhone = /^(\+\d)*\s*(\(\d{3}\)\s*)*\d{3}(-{0,1}|\s{0,1})\d{2}(-{0,1}|\s{0,1})\d{2}$/; 
//        if (strPhone.match(regPhone))
//        {
//                return true;
//        }
//        else
//        {
//                return false;
//        }

//    }
        
        // ===================== Count Password Character==============
        function Count_PasswordLenght(text,label,max_length) 
        {     
           
            var longitud= text.value.length;
            if(longitud < 0) 
            {              
                text.value=text.value.substr(0,max_length);             
                return false;              
            }         
            document.getElementById('<%=lblErrMsgCount.ClientId %>').innerHTML= longitud;           
        }
 
</script>
  <script type="text/javascript" language="javascript">
        function getCitystate()
    {
        
        var country ='<%= ViewState("country") %>';
        
        var zip = document.getElementById('<%= txtZip.ClientID%>');
        
        switch(country)
        {
            case '':
                break;                
            case '0':
              
               PageMethods.getCitystate(zip.value,country,OnSucceeded,OnFailed);
                 
              
               break;
            case '1':
               PageMethods.getCitystate(zip.value,country,OnSucceeded,OnFailed);
               break;
        }   
    }
    
    function OnSucceeded(result, userContext, methodName) 
    {    
        
         if(methodName == "getCitystate")
         {
            var city = document.getElementById('<%= txtCity.ClientID %>');
            var state = document.getElementById('<%= txtState.ClientID %>');
            var hideCity = document.getElementById('<%= hidcity.ClientID%>');
            var hideState = document.getElementById('<%= hidState.ClientID%>');
            
            if(result == 'N' )
            {
                city.value='';
                state.value = '';
                hideCity.value = '';
                hideState.value = '';
            }
            else
            {
                var cs = result.split(',');
                
                hideCity.value = cs[0];
                city.value = cs[0];
                
                hideState.value = cs[1];
                state.value = cs[1];
                
            }
        }
    }
    
    function OnFailed(error, userContext, methodName) 
    {   
        if(error !== null) 
        {
            alert(error.get_message());
        }
    }
    
// ========================clear date textbox===================
function Onclear(id)
{
    //alert(id);
    var txt = document.getElementById(id);
       txt.value='';
}
 function MpePOPUPHide(evt,id,focusid)
    {
            var e = evt ? evt : window.event;
            var KeyCode = e.which ? e.which : e.keyCode;
        //alert(KeyCode);
        switch(KeyCode)
        {
            case 27:
           // alert(focusid);
            $find(id).hide();
            if(focusid !='')
            document.getElementById(focusid).focus();
            break;
        }
    }
    </script>
<ajax:UpdatePanel ID="updatepanel1" runat="server" UpdateMode="Conditional">
<ContentTemplate>
  <table cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td align="center" valign="middle" bgcolor="#b8c7d3">
                        <center>
                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td width="100" align="center" valign="middle">
                                        <asp:LinkButton ID="lnkList" runat="server" CssClass="style8" CommandArgument="1"
                                            OnCommand="linkList_Click">List</asp:LinkButton>
                                    </td>
                                    <td width="10" align="left" valign="middle" class="style9">
                                        |
                                    </td>
                                    <td width="100" align="center" valign="middle">
                                        <asp:LinkButton ID="lnkDetail" runat="server" CssClass="style8" CommandArgument="2"
                                            OnCommand="linkList_Click">Detail</asp:LinkButton>
                                    </td>
                                    <td width="10" align="left" valign="middle" class="style9">
                                        |
                                    </td>
                                    <td width="125" align="center" valign="middle">
                                        <asp:LinkButton ID="lnkSalesHistory" runat="server" CssClass="style8" CommandArgument="3"
                                            OnCommand="linkList_Click">Sales History</asp:LinkButton>
                                    </td>
                                 
                                </tr>
                            </table>
                        </center>
                    </td>
                </tr>
            </table>
            <table width="100%" cellpadding="0" cellspacing="0">
            <tr>
            <td style="height:2px;"></td>
            </tr>
            </table>
   <asp:MultiView ID="MultiView1" runat="server">
        <asp:View ID="namedetailsview" runat="server">
        <table cellpadding="0" cellspacing="0" width="100%">
           <tr>
                    
                    <td align="left" valign="top" class="tab1">
                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                      <tr>
                        <td height="8" align="left" valign="top"><img src="images/crnr1w.gif" alt="" width="8" height="8" /></td>
                      </tr>
                      <tr>
                        <td height="20" align="left" valign="top">
                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                          <tr>
                            <td width="2%">
                            </td>
                            <td width="20%">
                                <asp:Label ID="Label1" runat="server" AccessKey="L" CssClass="labelall" Text="Last Name"></asp:Label>
                                <img alt="Required Field" src="Images/big1.gif" />
                                <asp:RequiredFieldValidator ID="reqfield" runat="server" 
                                    ControlToValidate="txtlastname" ValidationGroup="addcustomers"><asp:Image ID="img22" runat="server" ImageUrl="~/Images/blink1.gif" /></asp:RequiredFieldValidator>
                            </td>
                            <td width="20%">
                                <asp:Label ID="Label2" runat="server" CssClass="labelall" Text="First Name"></asp:Label>
                            </td>
                            <td width="20%">
                                <asp:Label ID="lblcustype" runat="server" CssClass="labelall" 
                                    Text="Customer Type" Visible="False"></asp:Label>
                           </td>
                          </tr>

                          <tr>
                            <td width="2%">
                                &nbsp;</td>
                            <td width="20%">
                                <asp:TextBox ID="txtlastname" runat="server" Width="90%" TabIndex="1" 
                                    onkeypress="FirstupperCase(event,this);" MaxLength="30"></asp:TextBox>
                            </td>
                            <td width="20%">
                                <asp:TextBox ID="txtFirstName" runat="server" Width="90%" 
                                    onkeypress="FirstupperCase(event,this);" MaxLength="30" TabIndex="2"></asp:TextBox>
                            </td>
                            <td width="20%">
                                <asp:DropDownList ID="ddlCusType" runat="server" CssClass="dropdownall" 
                                    Width="85%" TabIndex="3">
                                </asp:DropDownList>
                                <asp:Label ID="lblNoCustype" runat="server" CssClass="labelall" 
                                    Text="No Customer Type Found" Visible="False"></asp:Label>
                            </td
                          </tr>
                          <tr>
                            <td width="2%">
                            </td>
                            <td width="20%">
                                <asp:Label ID="lblCompany" runat="server" CssClass="labelall" 
                                    Text="Company Name"></asp:Label>
                            </td>
                            <td style="width: 9%">
                                &nbsp;</td>
                            <td width="12%">
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td width="2%">
                                &nbsp;</td>
                            <td colspan="3" width="20%">
                                <asp:TextBox ID="txtCompanyName" onkeypress="FirstupperCase(event,this);" 
                                    runat="server" Width="95%" MaxLength="30" TabIndex="4"></asp:TextBox>
                            </td>
                        </tr>
                          

                        </table></td>
                      </tr>
                      <tr>
                        <td height="8" align="left" valign="bottom" class="tab1c"><img src="images/crnr3w.gif" alt="" width="8" height="8" /></td>
                      </tr>
                    </table></td>
                  
                  </tr>
             <tr>
                <td style="height:5px;"></td>
            </tr>    
            <tr>
                    
                    <td align="left" valign="top" class="tab2">
                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                      <tr>
                        <td height="8" align="left" valign="top" colspan="3"><img src="images/crnr5w.gif" alt="" width="8" height="8" /></td>
                      </tr>
                      <tr>
                      <td width="3%"></td>
                        <td height="20" align="left" valign="top">
                               <table width="100%" border="0" cellspacing="0" cellpadding="0">
                            <tr>
                                 <td colspan="2">
                                    <asp:Label ID="Label6" runat="server" CssClass="labelall" Text="Address"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                               <td width="50%">
                                    <asp:TextBox ID="txtAddress" onkeypress="FirstupperCase(event,this);" runat="server" Width="95%" MaxLength="30" TabIndex="5"></asp:TextBox>
                                </td>
                                <td>
                                  <asp:TextBox ID="txtAddress1" runat="server" MaxLength="30" onkeypress="FirstupperCase(event,this);" TabIndex="6" Width="95%"></asp:TextBox>
                                </td>                                
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <table width="100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label8" runat="server" CssClass="labelall" Text="City"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label17" runat="server" Text="State" CssClass="labelall"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label18" runat="server" Text="Zip" CssClass="labelall"></asp:Label> 
                                            </td>
                                        </tr>
                                         <tr>
                                           <td width="33%">
                                                <asp:TextBox ID="txtCity" runat="server" Width="60%" MaxLength="30" 
                                                    onkeypress="FirstupperCase(event,this);" TabIndex="8"></asp:TextBox>
                                            </td>
                                            <td width="33%">
                                                <asp:TextBox ID="txtState" runat="server" MaxLength="2" Width="60%" 
                                                    onkeypress="upperCase(event);" TabIndex="9"></asp:TextBox>
                                            </td>
                                            <td align="left" width="33%">
                                                <asp:TextBox ID="txtZip" runat="server" MaxLength="5" 
                                                    onkeypress="fncInputNumericValuesOnly(event)" Width="60%" TabIndex="7"></asp:TextBox>
                                             </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <table width="100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                           <td>
                                                <asp:Label ID="Label19" runat="server" Text="Phone 1" CssClass="labelall"></asp:Label>
                                               
                                            </td>
                                            <td>
                                                <asp:Label ID="Label4" runat="server" Text="Phone 2" CssClass="labelall"></asp:Label>
                                              
                                            </td>
                                            <td>
                                                <asp:Label ID="Label7" runat="server" Text="Phone 3" CssClass="labelall"></asp:Label>
                                                
                                            </td>  
                                        </tr>
                                        <tr>
                                             <td>
                                                <asp:TextBox ID="txtPhone" runat="server" Width="60%" MaxLength="19" 
                                                    TabIndex="10"></asp:TextBox>
                                                <ajaxToolkit:MaskedEditExtender ID="mskPhone" runat="server" ClearMaskOnLostFocus="false" masktype="number"
                                                                    CultureName="en-GB" Mask="(999) 999-9999" TargetControlID="txtphone"></ajaxToolkit:MaskedEditExtender>
                                                &nbsp;<asp:DropDownList ID="ddlPhone1" runat="server" CssClass="dropdownall" 
                                                    TabIndex="11">
                                                    <asp:ListItem Value="H">Home</asp:ListItem>
                                                    <asp:ListItem Value="W">Work</asp:ListItem>
                                                    <asp:ListItem Value="F">Fax</asp:ListItem>
                                                    <asp:ListItem Value="M">Mobile</asp:ListItem>
                                                    <asp:ListItem Value="O">Others</asp:ListItem>
                                                </asp:DropDownList>
                                              </td>
                                              <td >
                                                    <asp:TextBox ID="txtPhone2" runat="server" Width="60%" MaxLength="19" 
                                                        TabIndex="12"></asp:TextBox>
                                                    <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" runat="server" ClearMaskOnLostFocus="false" masktype="number"
                                                                        CultureName="en-GB" Mask="(999) 999-9999" TargetControlID="txtphone2"></ajaxToolkit:MaskedEditExtender>
                                                    &nbsp;<asp:DropDownList ID="ddlPhone2" runat="server" CssClass="dropdownall" 
                                                        TabIndex="13">
                                                        <asp:ListItem Value="H">Home</asp:ListItem>
                                                        <asp:ListItem Value="W">Work</asp:ListItem>
                                                        <asp:ListItem Value="F">Fax</asp:ListItem>
                                                        <asp:ListItem Value="M">Mobile</asp:ListItem>
                                                        <asp:ListItem Value="O">Others</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtPhone3" runat="server" Width="60%" MaxLength="19" 
                                                        TabIndex="14"></asp:TextBox>
                                                    <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender2" runat="server" ClearMaskOnLostFocus="false" masktype="number"
                                                                        CultureName="en-GB" Mask="(999) 999-9999" TargetControlID="txtphone3"></ajaxToolkit:MaskedEditExtender>
                                                    &nbsp;<asp:DropDownList ID="ddlPhone3" runat="server" CssClass="dropdownall" 
                                                        TabIndex="15">
                                                        <asp:ListItem Value="H">Home</asp:ListItem>
                                                        <asp:ListItem Value="W">Work</asp:ListItem>
                                                        <asp:ListItem Value="F">Fax</asp:ListItem>
                                                        <asp:ListItem Value="M">Mobile</asp:ListItem>
                                                        <asp:ListItem Value="O">Others</asp:ListItem>
                                                    </asp:DropDownList>
                                               </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label20" runat="server" Text="Primary Email" CssClass="labelall"></asp:Label>
                              
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                                         ValidationGroup="addcustomers"
                                        ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" 
                                        ControlToValidate="txtEmail">
                                        <asp:Image ID="image221" runat="server" ImageUrl="~/Images/blink2.gif" />
                                        </asp:RegularExpressionValidator>
                                        
                                 </td>    
                                <td>
                                    <asp:Label ID="Label5" runat="server" Text="Secondary Email" 
                                        CssClass="labelall"></asp:Label>
                                          
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" 
                                         ValidationGroup="addcustomers"
                                        ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" 
                                        ControlToValidate="txtEmail2">
                                        <asp:Image ID="image1" runat="server" ImageUrl="~/Images/blink2.gif" />
                                        </asp:RegularExpressionValidator>
                                </td>                               
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtEmail" runat="server" Width="95%" MaxLength="40" 
                                        TabIndex="16"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtEmail2" runat="server" Width="95%" MaxLength="40" 
                                        TabIndex="17"></asp:TextBox>
                                </td>                                
                            </tr>                            
                         </table>
                         </td>
                         <td width="2%"></td>
                      </tr>
                      <tr>
                        <td height="8" align="left" colspan="3" valign="bottom" class="tab2c"><img src="images/crnr7w.gif" alt="" width="8" height="8" /></td>
                      </tr>
                    </table></td>
                    
                  </tr>
            <tr>
                <td style="height:5px;"></td>
            </tr>
            <tr>
               
                    <td align="left" valign="top" class="tab3"><table width="100%" border="0" cellspacing="0" cellpadding="0">
                      <tr>
                        <td height="8" align="left" valign="top"><img src="images/crnr9w.gif" alt="" width="8" height="8" /></td>
                      </tr>
                      <tr>
                        <td width="3%">&nbsp;</td>
                        <td height="20" align="left" valign="top">
                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                           <tr>                               
                                <td>
                                    <asp:Label ID="Label21" runat="server" Text="Tax Exempt" CssClass="labelall"></asp:Label>
                                    
                                    <asp:CompareValidator ID="CompareValidator3" runat="server" Operator="DataTypeCheck" Enabled="false"
                                        Type="Double" ControlToValidate="txtTax" ValidationGroup="addcustomers"> <asp:Image ID="Image3" runat="server" ImageUrl="~/Images/blink3.gif" /></asp:CompareValidator>
                                        
                                
                                </td>
                                <td >
                                    <asp:Label ID="Label11" runat="server" CssClass="labelall" 
                                        Text="Credit Card No"></asp:Label> &nbsp;
                                     <asp:RequiredFieldValidator ID="regValCredit" runat="server"  
                                        ControlToValidate="txtcard" ValidationGroup="addcustomers" Display="Dynamic"> <asp:Image ID="Image4" runat="server" ImageUrl="~/Images/blink3.gif" /></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                    <asp:Label ID="Label12" runat="server" CssClass="labelall" Text="Expiry Date"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblLastPurchaseTitle" runat="server" CssClass="labelall" Text="Last Purchase"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label14" runat="server" CssClass="labelall" 
                                        Text="Total Purchase"></asp:Label> &nbsp;
                                    </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtTax" runat="server" Width="150px" MaxLength="25" TabIndex="18"></asp:TextBox>
                                </td>
                                <td align="left"><img iD="lblCardType" runat="server" src="" alt="" style="visibility:visible" />
                                    &nbsp;<asp:Label ID="lblCard" runat="server" Visible="false"></asp:Label><asp:TextBox ValidationGroup="addcustomers" AutoCompleteType="Disabled"  ID="txtcard" onkeypress="fncInputNumericValuesOnly(event);" runat="server" Columns="19" MaxLength="19" Visible="false"></asp:TextBox>
                                    <asp:TextBox ID="txtCreditCard" AutoCompleteType="Disabled"  
                                        onkeypress="fncInputNumericValuesOnly(event);"  runat="server" Width="150px" 
                                        MaxLength="19" onfocus="this.select();" TabIndex="19"></asp:TextBox>
                                    &nbsp;
                                    <asp:ImageButton ID="lnkView" ImageUrl="~/icon small/view.gif" runat="server" 
                                        TabIndex="20" ImageAlign="Bottom"></asp:ImageButton>
                                    
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlMonth" runat="server" CssClass="dropdownall" 
                                        TabIndex="21">
                                        <asp:ListItem Value="01">Jan</asp:ListItem>
                                        <asp:ListItem Value="02">Feb</asp:ListItem>
                                        <asp:ListItem Value="03">Mar</asp:ListItem>
                                        <asp:ListItem Value="04">Apr</asp:ListItem>
                                        <asp:ListItem Value="05">May</asp:ListItem>
                                        <asp:ListItem Value="06">Jun</asp:ListItem>
                                        <asp:ListItem Value="07">Jul</asp:ListItem>
                                        <asp:ListItem Value="08">Aug</asp:ListItem>
                                        <asp:ListItem Value="09">Sep</asp:ListItem>
                                        <asp:ListItem Value="10">Oct</asp:ListItem>
                                        <asp:ListItem Value="11">Nov</asp:ListItem>
                                        <asp:ListItem Value="12">Dec</asp:ListItem>
                                    </asp:DropDownList>
                                    <font  size="3"><asp:Label ID="Label36" runat="server" Text="  /  "></asp:Label></font> 
                                    <asp:DropDownList ID="ddlYear" runat="server" CssClass="dropdownall" 
                                        TabIndex="22">
                                    </asp:DropDownList>
                                   
                                </td>
                                <td><asp:Label ID="lblLastPurchase" runat="server" Text="/ /" CssClass="labelall"></asp:Label>
                                 </td>
                                <td>
                                    <asp:Label ID="lblTotalPurchase" runat="server" Text="0.00" CssClass="labelall"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label16" runat="server" CssClass="labelall" Text="Price Level"></asp:Label>
                                    
                                </td>
                                <td><asp:Label ID="lblLoyalty" runat="server" CssClass="labelall" Text="Loyalty Card #:" Visible="False"></asp:Label></td>
                                <td>
                                    <asp:Label ID="lbllock" runat="server" CssClass="labelall" Text="Lock"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label22" col="2" runat="server" CssClass="labelall" Text="Lock Notes"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                              <td>
                                    <asp:DropDownList ID="ddlpricelevel" runat="server" AutoPostBack="True" Width="50px" TabIndex="23">
                                        <asp:ListItem>1</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td><asp:TextBox ID="txtLoyaltyCard" runat="server" MaxLength="25" Visible="False" Width="75%" TabIndex="24"></asp:TextBox></td>
                                <td>
                                    <asp:DropDownList ID="ddlLock" runat="server" AutoPostBack="True" 
                                        CssClass="dropdownall" Width="50px" TabIndex="25">
                                        <asp:ListItem Value="Y">Yes</asp:ListItem>
                                        <asp:ListItem Selected="True" Value="N">No</asp:ListItem>
                                    </asp:DropDownList>
                                 </td>
                                <td colspan="2" >
                                    <asp:TextBox ID="txtLockNotes" runat="server" TextMode="MultiLine" Width="90%" 
                                        MaxLength="16" TabIndex="26"></asp:TextBox>
                                 </td>
                            </tr> 
                            <tr>                                
                                <td colspan="4">
                                    <asp:Label ID="lblNotes" runat="server" CssClass="labelall" Text="Notes" Width="60px"></asp:Label> &nbsp;
                                    <asp:Label ID="lblNotesDetail" runat="server" CssClass="labelall" Visible="false"></asp:Label>&nbsp;
                                    <asp:LinkButton ID="lnkAddNotes" runat="server" Text="Add Notes" CssClass="style8"  style="text-decoration:underline"></asp:LinkButton>
                                </td>                     
                            </tr>                    
                        </table></td><td width="2%"></td>
                      </tr>
                      <tr>
                        <td height="8" colspan="3" align="left" valign="bottom" class="tab3c"><img src="images/crnr11w.gif" alt="" width="8" height="8" /></td>
                      </tr>
                    </table></td>
                  
                  </tr>
            <tr>
                <td height="5">
                </td>
           </tr>
           
        </table>
        <div id="footerButton">
            <table width="78%">
                <tr>
                            <td align="right">
                                    <input id="hidcity" type="hidden" runat="server" />
                                    <input id="hidState" type="hidden" runat="server" /> 
                                    <asp:ImageButton id="btnSave" runat="server"  ImageUrl="~/icon small/save.gif" CssClass="buttonall" 
                                        ValidationGroup="addcustomers" TabIndex="27"></asp:ImageButton>
                                    &nbsp;
                                <asp:ImageButton id="btnCancel1" runat="server" ImageUrl="~/icon small/cancel.gif" 
                                        CausesValidation="false" CssClass="buttonall" TabIndex="28" />
                                    &nbsp; <asp:ImageButton ID="btnAdd" runat="server" 
                                        ImageUrl="~/icon small/add.gif" />
                                    &nbsp;<asp:ImageButton ID="imagebutton2" runat="server" ImageUrl="~/icon small/edit.gif" /> 
                                     
                                    &nbsp;<asp:ImageButton ID="btnDelete" runat="server" 
                                        ImageUrl="~/icon small/delete.gif" />
                                     &nbsp; 
                                    <asp:ImageButton ID="imgexit" runat="server"  ImageUrl="~/icon small/exit.gif" 
                                        TabIndex="27" />   
                                        </td>
                                    </tr>
            </table>
        </div>
   </asp:View> </asp:MultiView> 

                             
 
</ContentTemplate>
</ajax:UpdatePanel>
<ajaxToolkit:modalpopupextender DropShadow="false" ID="ModalPopupExtenderDelete" runat="server" BackgroundCssClass="modalBackground" TargetControlID="btnDeleteno" PopupControlID="pnlDelete" ></ajaxToolkit:modalpopupextender>
  <asp:Button ID="btnDeleteno" runat="server" style="display:none"/>
  <asp:Panel ID="pnlDelete" runat="server" CssClass="modalPopup"  style="display:none" Width="500px">
     <ajax:UpdatePanel ID="UpdatePanelDelete" runat="server">
       <ContentTemplate>
              <table width="500" border="0" align="center" cellpadding="0" cellspacing="0">
              <tr>
                <td width="500" height="27" background="images/popup_01.gif"><table width="100%" border="0" cellspacing="0" cellpadding="0">
                  <tr>
                    <td width="15">&nbsp;</td>
                    <td width="293">
                    <asp:Label ID="lblHdrdelete" CssClass="stylePopupHeader" runat="server" Text="Delete Customer" ></asp:Label>
                    </td>
                  </tr>
                </table></td>
              </tr>
              <tr>
                <td width="500" height="120" align="left" valign="top" background="images/popup_06n.gif"><img src="images/spacer.gif" alt="" width="1" height="1" />
                  <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                      <td width="20">&nbsp;</td>
                      <td width="460" align="left" valign="top">
                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                        <tr>
                          <td width="380" align="left" valign="top">
                          <asp:Label ID="lblStationnodelete" runat="server" CssClass="stylePopuplabel"> </asp:Label><br />
                          <asp:Label id="lblSuredelete" Text="Are you sure you want to delete ?" runat="server" CssClass="stylePopuplabel"> </asp:Label>
                          </td>
                          <td width="80" align="right" valign="middle"><img src="images/popup_07.gif" alt="" width="50" height="50" /></td>
                        </tr>
                      </table></td>
                      <td width="20">&nbsp;</td>
                    </tr>
                      <tr>
                          <td width="20">
                              &nbsp;</td>
                          <td width="460">
                              &nbsp;</td>
                          <td width="20">
                              &nbsp;</td>
                      </tr>
                      <tr>
                          <td width="20">
                              &nbsp;</td>
                          <td width="460">
                              &nbsp;</td>
                          <td width="20">
                              &nbsp;</td>
                      </tr>
                      <tr>
                          <td width="20">
                              &nbsp;</td>
                          <td width="460">
                              &nbsp;</td>
                          <td width="20">
                              &nbsp;</td>
                      </tr>
                    <tr>
                      <td width="20" height="22">&nbsp;</td>
                      <td width="460" height="22" align="right" valign="top">
                          <asp:ImageButton ID="btnYesDelete" runat="server"  CausesValidation="false" ImageUrl="~/icon small/yes.gif" />
                          &nbsp;&nbsp;<asp:ImageButton ID="btnNoDelete" runat="server" CausesValidation="false" ImageUrl="~/icon small/no.gif" />
                          </td>
                      <td width="20" height="22">&nbsp;</td>
                    </tr>
                    <tr>
                      <td>&nbsp;</td>
                      <td>&nbsp;</td>
                      <td>&nbsp;</td>
                    </tr>
                  </table></td>
              </tr>
            </table>
        </ContentTemplate>
        </ajax:UpdatePanel>
  </asp:Panel>
  
  <ajaxToolkit:modalpopupextender DropShadow="false" ID="mpeCard" runat="server" BackgroundCssClass="modalBackground" TargetControlID="btnCard" PopupControlID="pnlCard" BehaviorID="mpeCard" ></ajaxToolkit:modalpopupextender>
  <asp:Button ID="btnCard" runat="server" style="display:none"/>
  <asp:Panel ID="pnlCard" runat="server" CssClass="modalPopup"  style="display:none" Width="500px">
         <table width="500" border="0" align="center" cellpadding="0" cellspacing="0">
              <tr>
                <td width="500" height="27" background="images/popup_01.gif"><table width="100%" border="0" cellspacing="0" cellpadding="0">
                  <tr>
                    <td width="15">&nbsp;</td>
                    <td width="293">
                    <asp:Label ID="Label3" CssClass="stylePopupHeader" runat="server" Text="Card Information" ></asp:Label>
                    </td>
                    <td align="right">
                        <asp:ImageButton ID="btnimgClose" runat="server" ImageUrl="~/Images/close1.gif" OnClientClick="$find('mpeCard').hide();return false;" />
                        &nbsp;&nbsp;
                    </td>
                  </tr>
                </table></td>
              </tr>
              <tr>
                <td width="500" height="50" align="left" valign="top" background="images/popup_06n.gif"><img src="images/spacer.gif" alt="" width="1" height="1" />
                  <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                      <td width="20">&nbsp;</td>
                      <td width="460" align="left" valign="top">
                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                        <tr>
                          <td align="left" valign="top">
                          <asp:Label ID="lblcardname" runat="server" CssClass="stylePopuplabel"> </asp:Label>
                          <asp:Label id="lblno" runat="server" CssClass="stylePopuplabel"> </asp:Label>
                          </td>
                           </tr>
                           <tr>
                          <td  align="left" valign="top">
                          <asp:Label ID="lblDate" runat="server" Text="Valid Thru : " CssClass="stylePopuplabel"> </asp:Label>
                          <asp:Label id="lbldatetime" runat="server" CssClass="stylePopuplabel"> </asp:Label>
                          </td>
                           </tr>
                      </table></td>
                      <td width="20">&nbsp;</td>
                    </tr>
                     
                    <tr>
                      <td width="20" height="22">&nbsp;</td>
                      <td width="460" height="22" align="right" valign="top">
                             <asp:ImageButton ID="ingbtnClose" runat="server" OnClientClick="$find('mpeCard').hide();return false;" ImageUrl="~/icon small/Close.gif" />
                          </td>
                      <td width="20" height="22">&nbsp;</td>
                    </tr>
                    <tr>
                      <td>&nbsp;</td>
                      <td>&nbsp;</td>
                      <td>&nbsp;</td>
                    </tr>
                  </table></td>
              </tr>
            </table>
         </asp:Panel>
   
    <ajaxToolkit:modalpopupextender DropShadow="false" ID="mpeWarning" runat="server" BackgroundCssClass="modalBackground" TargetControlID="btnWarning" PopupControlID="pnlWarning" BehaviorID="mpeWarning" ></ajaxToolkit:modalpopupextender>
  <asp:Button ID="btnWarning" runat="server" style="display:none"/>
  <asp:Panel ID="pnlWarning" runat="server" CssClass="modalPopup"  style="display:none" Width="60%">
  <ajax:UpdatePanel ID="upwarning" runat="server" UpdateMode="Conditional">
  <ContentTemplate>
         <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
              <tr>
                <td height="27" background="images/popup_01.gif"><table width="100%" border="0" cellspacing="0" cellpadding="0">
                  <tr>
                    <td width="15">&nbsp;</td>
                    <td width="293">
                    <asp:Label ID="Label9" CssClass="stylePopupHeader" runat="server" Text="Card Information" ></asp:Label>
                    </td>
                    <td align="right">
                        <asp:ImageButton ID="imgbtnWarning" runat="server" ImageUrl="~/Images/close1.gif" OnClientClick="$find('mpeWarning').hide();return false;" />
                        &nbsp;&nbsp;
                    </td>
                  </tr>
                </table></td>
              </tr>
              <tr>
                <td align="left" valign="top" background="images/popup_06n.gif">
                  <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                      <td width="20">&nbsp;</td>
                      <td align="left" valign="top" colspan="2">
                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                        <tr>
                          <td align="left" valign="top">
                          <asp:Label ID="Label10" runat="server" CssClass="labelHead" Text="Credit Card Security Reminder:"> </asp:Label>
                          
                          </td>
                           </tr>
                           <tr>
                          <td  align="left" valign="middle">
                          <asp:Label id="lblWarning" runat="server" CssClass="stylePopuplabel"> </asp:Label><b><span id="pastWarning" runat="server" style="color:Red">(YOU ARE PAST DUE)</span></b>
                          </td>
                           </tr>
                      </table></td>
                      <td width="20">&nbsp;</td>
                    </tr>
                     <tr>
                     <td>
                        <img src="images/spacer.gif" alt="" width="1" height="1" />
                     </td>
                     </tr>
                    <tr>
                      <td width="20">&nbsp;</td>
                      <td width="230" align="center" valign="top">
                             <asp:ImageButton ID="btnChangepass" runat="server" ImageUrl="~/icon small/changepassword.gif" />
                       </td>
                       <td width="230" align="left" valign="top">
                                <asp:ImageButton ID="btnNoContinue" runat="server" ImageUrl="~/icon small/nojustcontinue.gif" /><br />
                                <li id="warninigli" runat="server" visible="false">You will lose access to view complete credit card numbers.</li>
                       </td>
                      <td width="20">&nbsp;</td>
                    </tr>
                    <tr>
                      <td>&nbsp;</td>
                      <td>&nbsp;</td>
                      <td>&nbsp;</td>
                    </tr>
                  </table></td>
              </tr>
            </table>
        </ContentTemplate> 
        </ajax:UpdatePanel> 
 
         </asp:Panel>
     <%-----------------------Modal popup for Credit Card Acess--------%>
    <ajaxToolkit:ModalPopupExtender DropShadow="false" ID="mpeCreditPassWord" BehaviorID="mpeCreditPassWord"
        runat="server" BackgroundCssClass="modalBackground" TargetControlID="btnCreditPassWord"
        PopupControlID="pnlCreditPassWord">
    </ajaxToolkit:ModalPopupExtender>
    <asp:Button ID="btnCreditPassWord" runat="server" Style="display: none" />
    <asp:Panel ID="pnlCreditPassWord" runat="server" Style="display: none"  CssClass="modalPopup"
        Width="500px">
        <ajax:UpdatePanel ID="upCreditPassWord" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
            <table width="500" border="0" align="center" cellpadding="0" cellspacing="0">
                    <tr>
                        <td width="500" height="27" background="images/popup_01.gif" colspan="3">
                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td width="15">
                                        &nbsp;
                                    </td>
                                    <td width="293">
                                        <asp:Label ID="Label23" CssClass="stylePopupHeader" runat="server" Text="Password"></asp:Label>
                                    </td>
                                    
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td height="1%"><img src="images/spacer.gif" alt="" width="1" height="1" /></td>
                    </tr>
                    <tr><td width="1%">
                    </td>
                           <td align="left" valign="top" class="tab1">
                                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                  <tr>
                                    <td height="8" align="left" valign="top"><img src="images/crnr1w.gif" alt="" width="8" height="8" /></td>
                                  </tr>
                                  <tr><td width="1%"></td>
                                    <td  align="left" valign="top">
                                          <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td class="labelall">    
                                        Password : <img src="Images/big1.gif" alt="" />
                                        <asp:CustomValidator ID="revcust" runat="server" ClientValidationFunction="PasswordCheck" ControlToValidate="txtCreditPassWord" Display="Dynamic" SetFocusOnError="true" ValidateEmptyText="true" ValidationGroup="creditpass"><asp:Image ID="Image7" runat="server" ImageUrl="~/Images/blink1.gif" /></asp:CustomValidator>
                                                         
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCreditPassWord" runat="server" TextMode="Password" CssClass="textboxall" MaxLength="10" style="width:50%"></asp:TextBox>
                                        <font color="red"> <asp:Label ID="lblErrMsg" runat="server" Text="< 10 character maximum"></asp:Label></font>
                                    </td>
                               </tr>
                               <tr>
                                <td class="labelall" width="30%"> Confirm Password :<img src="Images/big1.gif" alt="" />
                                <asp:CustomValidator ClientValidationFunction="comparePass" ID="revcustcomppass" runat="server" ControlToValidate="repassWord" Display="Dynamic" ValidateEmptyText="true" ValidationGroup="creditpass"><asp:Image ID="Image6" runat="server" ImageUrl="~/Images/blink1.gif" /></asp:CustomValidator>
                                           <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="repassword"
                                                ValidationGroup="creditpass" SetFocusOnError="true">
                                                <asp:Image ID="image6" runat="server" ImageUrl="~/Images/blink1.gif" /></asp:RequiredFieldValidator>
                                            <asp:CompareValidator runat="server" ID="CompareValidator1" ErrorMessage="Passwords do not match!"
                                                ValidationGroup="creditpass" SetFocusOnError="true" ControlToCompare="txtCreditPassWord"
                                                ControlToValidate="repassword">--%>
                                                <%--<asp:Image ID="Image9" runat="server" ImageUrl="~/Images/blink1.gif" /></asp:CompareValidator>--%>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="repassword" runat="server" CssClass="textboxall" TextMode="Password" MaxLength="10" style="width:50%"></asp:TextBox>
                                    </td>
                               </tr> 
                               <tr> 
                                   <td colspan="2" align="right">
                                        <asp:ImageButton ID="btnOKPass" runat="server" ImageUrl="~/icon small/ok.gif" ValidationGroup="creditpass"/>&nbsp;&nbsp;<asp:ImageButton ID="btnCancelPass" runat="server" CausesValidation="false" ImageUrl="~/icon small/cancel.gif" />
                                   </td>
                               </tr>
                               <tr> 
                                   <td colspan="2" align="left" class="labelall">
                                        <asp:Label ID="lblErrMsgCount" runat="server" Text="0  "></asp:Label>&nbsp;
                                        <asp:Label ID="lblErCntMsg" runat="server" Text=" of 10 maximum characters entered"> </asp:Label>
                                   </td>
                               </tr>
                              </table> 
                                    </td><td width="1%"></td>
                                  </tr>
                                  <tr>
                                    <td height="8" align="left" valign="bottom" class="tab1c" colspan="3"><img src="images/crnr3w.gif" alt="" width="8" height="8" /></td>
                                  </tr>
                                </table>
                        </td><td width="1%">
                    </td> 
                     </tr>
                     <tr>
                        <td height="1%"><img src="images/spacer.gif" alt="" width="1" height="1" /></td>
                    </tr>
                 </table> 
            </ContentTemplate> 
        </ajax:UpdatePanel> 
    </asp:Panel>
     <ajaxToolkit:modalpopupextender DropShadow="false" ID="mpeLogincredit" runat="server" BackgroundCssClass="modalBackground" TargetControlID="btnLogincredit" PopupControlID="pnlLogincredit" BehaviorID="mpeLogincredit" ></ajaxToolkit:modalpopupextender>
  <asp:Button ID="btnLogincredit" runat="server" style="display:none"/>
  <asp:Panel ID="pnlLogincredit" runat="server" CssClass="modalPopup"  style="display:none" Width="40%">
  <ajax:UpdatePanel ID="upLogin" runat="server" UpdateMode="Conditional">
  <ContentTemplate>
         <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
              <tr>
                <td colspan="3" height="27" background="images/popup_01.gif"><table width="100%" border="0" cellspacing="0" cellpadding="0">
                  <tr>
                    <td width="15">&nbsp;</td>
                    <td width="293">
                    <asp:Label ID="Label15" CssClass="stylePopupHeader" runat="server" Text="Credit Card Login" ></asp:Label>
                    </td>
                    <td align="right">
                        <asp:ImageButton ID="imgbtnLoginClose" runat="server" ImageUrl="~/Images/close1.gif" OnClientClick="$find('mpeLogincredit').hide();return false;" />
                        &nbsp;&nbsp;
                    </td>
                  </tr>
                </table></td>
              </tr>
               <tr>
                        <td height="1%"><img src="images/spacer.gif" alt="" width="1" height="1" /></td>
                    </tr>
                    <tr><td width="1%">
                    </td>
                           <td align="left" valign="top" class="tab1">
                                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                  <tr>
                                    <td height="8" align="left" valign="top"><img src="images/crnr1w.gif" alt="" width="8" height="8" /></td>
                                  </tr>
                                  <tr><td width="1%"></td>
                                    <td  align="left" valign="top">
                                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                        <tr>
                          <td align="left" width="28%">
                            User Name : 
                             <img alt="Required Field" src="Images/big1.gif" />
                              <asp:RequiredFieldValidator ID="reqUsername" runat="server" ControlToValidate="txtUsername" Display="Dynamic" ErrorMessage="*" SetFocusOnError="true" ValidationGroup="valLogin" >
                                        <asp:Image ID="image2" runat="server" ImageUrl="~/Images/blink1.gif" /></asp:RequiredFieldValidator>
                          </td>
                          <td><asp:TextBox ID="txtUsername" runat="server" CssClass="textboxall"></asp:TextBox>
                          </td>
                           </tr>
                           <tr>
                          <td  align="left" >
                          Password : 
                          <img alt="Required Field" src="Images/big1.gif" />
                          <asp:RequiredFieldValidator ID="reqpass" runat="server" ControlToValidate="txtPass" Display="Dynamic" ErrorMessage="*" SetFocusOnError="true" ValidationGroup="valLogin" >
                                        <asp:Image ID="image5" runat="server" ImageUrl="~/Images/blink1.gif" /></asp:RequiredFieldValidator>
                          </td>
                          <td>
                            <asp:TextBox ID="txtPass" runat="server" TextMode="Password" CssClass="textboxall"></asp:TextBox>
                          </td>
                           </tr>
                                     <tr>
                        <td height="4%"><img src="images/spacer.gif" alt="" width="3" height="5" /></td>
                    </tr>
                           <tr>
                            <td align="right" valign="top" colspan="2">
                             <asp:ImageButton ID="btnLoginOK" ValidationGroup="valLogin" runat="server" ImageUrl="~/icon small/ok.gif" />&nbsp;&nbsp;&nbsp;
                           <asp:ImageButton ID="btnLoginCancel" runat="server" CausesValidation="false" ImageUrl="~/icon small/cancel.gif" OnClientClick="$find('mpeLogincredit').hide();return false;"/>
                       </td>
                           </tr>
                      </table>
                                      </td><td width="1%"></td>
                                  </tr>
                                  <tr>
                                    <td height="8" align="left" valign="bottom" class="tab1c" colspan="3"><img src="images/crnr3w.gif" alt="" width="8" height="8" /></td>
                                  </tr>
                                </table>
                        </td><td width="1%">
                    </td> 
                     </tr>
                     <tr>
                        <td height="1%"><img src="images/spacer.gif" alt="" width="1" height="1" /></td>
                    </tr>                         
            </table></ContentTemplate> </ajax:UpdatePanel> 
            
         </asp:Panel>
   
        <%--------------------------------------- Modal popup for Add Notes ------------------------------------%>
  <ajaxToolkit:modalpopupextender DropShadow="false" ID="mpeAddNotes" runat="server" BackgroundCssClass="modalBackground" TargetControlID="btnAddnote" PopupControlID="pnlAddNotes" ></ajaxToolkit:modalpopupextender>
   <asp:Button ID="btnAddnote" runat="server" style="display:none"/>
   <asp:Panel ID="pnlAddNotes" runat="server" CssClass="modalPopup" style="display:none;" Width="400px">
       <ajax:UpdatePanel ID="UpdatePanelAdd" runat="server" UpdateMode="Conditional">
       <ContentTemplate>              
               <table width="400" border="0" align="center" cellpadding="0" cellspacing="0">
              <tr>
                   <td width="400" height="27" background="images/popup_01.gif">
                   <table width="100%" border="0" cellspacing="0" cellpadding="0">
                   <tr>
                        <td width="10">&nbsp;</td>
                        <td width="293"><asp:Label ID="lblhdrAdd"  CssClass="stylePopupHeader" runat="server" Text="Add Notes" ></asp:Label></td>
                  </tr>
                  </table>
                  </td>
              </tr>
              <tr>
                    <td width="400" height="150" align="left" valign="top" background="images/popup_06n.gif"><img src="images/spacer.gif" alt="" width="1" height="1" />
                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                      <td width="20">&nbsp;</td>
                      <td width="360" align="left" valign="top">
                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                        <tr>
                            <td align="left" valign="middle" colspan="2">
                                <asp:TextBox ID="txtNote" runat="server" TextMode="MultiLine" MaxLength="3" CssClass="textboxall" style="width:97%;height:60px"></asp:TextBox></td>
                        </tr>
                        </table>
                      </td>
                      <td width="20">&nbsp;</td>
                    </tr>
                    <tr>
                      <td width="20">&nbsp;</td>
                      <td width="360"><br /><br /></td>
                      <td width="20">&nbsp;</td>
                    </tr>
                    <tr>
                      <td width="20" height="22">&nbsp;</td>
                      <td width="360" align="right" valign="middle"><table width="100%" border="0" cellspacing="0" cellpadding="0">
                        <tr>
                          <td width="288" height="22" align="right" valign="middle">
                          <asp:ImageButton ID="btnSaveadd" runat="server"  ImageUrl="~/Icon small/ok.gif" />                          
                          </td>
                          <td width="10" align="right" valign="middle">&nbsp;</td>
                          <td width="62" align="right" valign="middle">
                          <asp:ImageButton ID="btnCanceladd" runat="server" ImageUrl="~/Icon small/cancel.gif" CausesValidation="False" />
                          </td>
                        </tr>
                      </table></td>
                      <td width="20" height="22">&nbsp;</td>
                    </tr>
                    <tr>
                      <td>&nbsp;</td>
                      <td>&nbsp;</td>
                      <td>&nbsp;</td>
                    </tr>
                  </table></td>
              </tr>
            </table>
               
               
        </ContentTemplate>
        </ajax:UpdatePanel>
   </asp:Panel>
    
</asp:Content>

