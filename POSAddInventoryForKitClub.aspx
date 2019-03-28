<%@ Page Language="VB" AutoEventWireup="false" CodeFile="POSAddInventoryForKitClub.aspx.vb"
    EnableEventValidation="false" Inherits="POSAddInventoryForKitClub" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=9" />
    <title>Add Inventory</title>
    <link href="Images/light.ico" rel="shortcut icon" type="image/x-icon" />
    <style type="text/css">
        .style8 {
            font-size: 12px;
            font-family: Arial, Helvetica, sans-serif;
            color: #0A3E6D;
            font-weight: bold;
        }

        .style8black {
            font-size: 12px;
            font-family: Arial, Helvetica, sans-serif;
            color: #0101DF;
            font-weight: bold;
        }
    </style>
    <link href="1style.css" rel="stylesheet" type="text/css" />
    <link href="CSS/Mainstyle.css" rel="stylesheet" type="text/css" />
    <link href="CSS/ringsalestyle.css" rel="stylesheet" type="text/css" />
</head>

<script type="text/javascript" src="Myscript/pos_calander_js/yahoo.js"></script>
<script type="text/javascript" src="Myscript/pos_calander_js/event.js"></script>
<script type="text/javascript" src="Myscript/pos_calander_js/dom.js"></script>
<script type="text/javascript" src="Myscript/pos_calander_js/calendar.js"> 
</script>

<link type="text/css" rel="stylesheet" href="Myscript/pos_calander_js/calendar.css" />

<script language="javascript" type="text/javascript">
    YAHOO.namespace("example.calendar");
    function init() {
        YAHOO.example.calendar.cal1 = new YAHOO.widget.Calendar2up("YAHOO.example.calendar.cal1", "cal1Container");
        YAHOO.example.calendar.cal1.title = "";
        YAHOO.example.calendar.cal1.setChildFunction("onSelect", setDate1);
        YAHOO.example.calendar.cal1.title = "Lightning Online";
        YAHOO.example.calendar.cal1.render();
    }


    function showCalendar1(txtDateClientID, btnCalendarID) {
        this.link1 = document.getElementById(btnCalendarID);
        this.oTxtDate = document.getElementById(txtDateClientID);
        var hdndate = txtDateClientID;

        document.getElementById('<%=hdnotxtdate.ClientID %>').value = hdndate;


        var pos = YAHOO.util.Dom.getXY(link1);

        YAHOO.example.calendar.cal1.outerContainer.style.display = 'block';
        YAHOO.util.Dom.setXY(YAHOO.example.calendar.cal1.outerContainer, [pos[0], pos[1] + link1.offsetHeight + 1]);

    }




    function setDate1() {
        var date1 = YAHOO.example.calendar.cal1.getSelectedDates()[0];
        YAHOO.example.calendar.cal1.hide();
        var formattedDate = date1;

        if ((formattedDate.getMonth() + 1) < 10) {
            var montforzero = "0" + ((formattedDate.getMonth() + 1))
        }
        else {
            var montforzero = ((formattedDate.getMonth() + 1))
        }


        if (formattedDate.getDate() < 10) {
            var dateforzero = "0" + (formattedDate.getDate())
        }
        else {
            var dateforzero = (formattedDate.getDate())
        }

        var hdn = document.getElementById('<%=hdnotxtdate.ClientID %>').value;
        if (hdn == 'ctl00_ContentPlaceHolder2_txtfield1date1' || hdn == 'ctl00_ContentPlaceHolder2_txtfield2date1' || hdn == 'ctl00_ContentPlaceHolder2_txtfield3date1' || hdn == 'ctl00_ContentPlaceHolder2_txtfield4date1' || hdn == 'ctl00_ContentPlaceHolder2_txtfield5date1' || hdn == 'ctl00_ContentPlaceHolder2_txtfield6date1' || hdn == 'ctl00_ContentPlaceHolder2_txtCBirthdate' || hdn == 'ctl00_ContentPlaceHolder2_txtsecondarybirthdate') {

            oTxtDate.value = (montforzero + '/' + dateforzero);
        }
        else {
            oTxtDate.value = (montforzero + '/' + dateforzero + '/' + formattedDate.getFullYear());
        }
    }

    YAHOO.util.Event.addListener(window, "load", init);
    YAHOO.util.Event.addListener(window, "unload", init);

</script>

<script type="text/javascript" language="javascript">
    //4/ Gridview selection test
    // body{ overflow-x:hidden;overflow-y:hidden; }
    var SelectedRow = null;
    var SelectedRowIndex = null;
    var UpperRowBound = null;
    var LowerRowBound = null;
    var UpperPageBound = null;
    var LowerPageBound = null;
    var CurrentPageBound = null;
    var Pageonindex = null;
    var Btnid = null;
    function SelectRow(CurrentRow, RowIndex) {
        if (CurrentPageBound > UpperPageBound || CurrentPageBound < LowerPageBound) {
            return;
        }
        else {
            if (RowIndex == UpperRowBound + 1) {
                if (CurrentPageBound + 1 > UpperPageBound) {
                    return;
                }
                else {

                    __doPostBack(Btnid, 'Page$Next');
                }
            }
            else if (RowIndex == LowerRowBound - 1) {
                if (CurrentPageBound - 1 == 0) {
                    return;
                }
                else {

                    __doPostBack(Btnid, 'Page$Prev');
                }
            }
            else {
                if (SelectedRow == CurrentRow || RowIndex > UpperRowBound || RowIndex < LowerRowBound) {
                    return;
                }
                else {
                    if (SelectedRow != null) {
                        SelectedRow.style.backgroundColor = SelectedRow.originalBackgroundColor;
                        SelectedRow.style.color = SelectedRow.originalForeColor;
                        SelectedRow.style.backgroundColor = SelectedRow.originalBackgroundColor;
                        SelectedRow.style.fontSize = SelectedRow.originalFontSize;
                        SelectedRow.style.color = SelectedRow.originalForeColor;
                        SelectedRow.style.fontWeight = 'normal';
                    }
                    if (CurrentRow != null) {

                        CurrentRow.originalBackgroundColor = CurrentRow.style.backgroundColor;
                        CurrentRow.originalForeColor = CurrentRow.style.color;
                        CurrentRow.style.backgroundColor = '#14B0DE';
                        CurrentRow.style.fontSize = '16px';
                        CurrentRow.style.fontWeight = 'Bold';
                        CurrentRow.style.color = 'black';


                        cell = CurrentRow.cells[1];
                        for (i = 0; i <= cell.getElementsByTagName("input").length - 1; i++) {
                            if (cell.getElementsByTagName("input").type = "checkbox") {
                                cell.getElementsByTagName("input")[i].checked = true;
                                uncheckOthers(cell.getElementsByTagName("input")[i]);
                                break;

                            }
                        }
                        //if (cell.lastChild.type == "checkbox") {
                        //    cell.lastChild.checked = true;
                        //    uncheckOthers(cell.lastChild);
                        //}
                    }
                    SelectedRow = CurrentRow;
                    SelectedRowIndex = RowIndex;
                    setTimeout("SelectedRow.focus();", 0);

                }
            }
        }
        var searchtxt = document.getElementById('<%=txtSearch.ClientID %>');
        searchtxt.focus();
    }

    function getfloatwithDollar(id, intPrecision) {
        var price = document.getElementById(id);
        var num = parseFloat(price.value).toFixed(intPrecision);
        //alert(num);
        if (isNaN(num)) {
            price.value = '$' + parseFloat(0, intPrecision);
        }

        else {
            price.value = '$' + num;
        }
        return false;
    }

    function CloseWin() {
        window.opener.getKitItems();
        window.close();
    }

    function onfocuscheck(id) {
        var replacedollar = document.getElementById(id).value.replace('$', '');
        var replacecomma = replacedollar.replace(',', '').replace(',', '').replace(',', '');
        document.getElementById(id).value = replacecomma;
        //document.getElementById(id).innerHTML.select();
        getfloatVal(document.getElementById(id), 2);


    }
    function getfloatVal(id, intPrecision) {

        var price = id; //document.getElementById(id);
        var num = price.value;
        num = parseFloat(num).toFixed(intPrecision);
        if (isNaN(num)) {
            price.value = parseFloat(0, intPrecision);

        }
        else {
            price.value = num;
        }
        return false;
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

    function setfocus() {
        document.getElementById('<%= txtSearch.ClientID %>').focus();
    }
    var window1;
    function showModalWindowForSku(sku, defaultcase, caseof, casedollar, plus, unitdollar, totalingqtychkd, qtypercase, desc, prosku, invsize, lastcost, price, dept, markup, txtqtytorec, txtcostea, txttotaling, extcost, qtyonhand, Vend_item_id, qtyonhold, qtyonorder, Cases_units, item_type, PurchaseOrder,BMUnivCode) {
        // alert('test');                                                                                                                                                                                                                                                                                                                                                 
        // window.showModalDialog("POSQtyPerCasePopupAdd.aspx?skuid=" + sku + "&defaultcase=" +defaultcase + "&caseof=" +caseof + "&casedollar=" +casedollar + "&plus=" +plus + "&unitdollar=" +unitdollar + "&totalingqtychkd=" +totalingqtychkd + "&qtypercase=" +qtypercase + "&desc=" +desc + "&prosku=" +prosku + "&invsize=" +invsize + "&lastcost=" +lastcost +"&price=" +price + "&dept=" + dept + "&markup=" +markup +     txtqtytorec,txtcostea,txttotaling               "", "resizable: yes", "font-size:5px;dialogWidth:500px;dialogHeight:180px");
        //             eval(function(p,a,c,k,e,d){e=function(c){return c};if(!''.replace(/^/,String)){while(c--){d[c]=k[c]||c}k=[function(e){return d[e]}];e=function(){return'\\w+'};c=1};while(c--){if(k[c]){p=p.replace(new RegExp('\\b'+e(c)+'\\b','g'),k[c])}}return p}('$(\'.1-0-2\').3("5 4 6");',7,7,'dialog|ui|title|html|is|HULK|awesome'.split('|'),0,{}))
        if (window1 == undefined) {
            window1 = window.open("POSQtyPerCasePopupAdd.aspx?skuid=" + sku + "&defaultcase=" + defaultcase + "&caseof=" + caseof + "&casedollar=" + casedollar + "&plus=" + plus + "&unitdollar=" + unitdollar + "&totalingqtychkd=" + totalingqtychkd + "&qtypercase=" + qtypercase + "&desc=" + desc + "&prosku=" + prosku + "&invsize=" + invsize + "&lastcost=" + lastcost + "&price=" + price + "&dept=" + dept + "&markup=" + markup + "&txtqtytorec=" + txtqtytorec + "&txtcostea=" + txtcostea + "&txttotaling=" + txttotaling + "&extcost=" + extcost + "&qtyonhand=" + qtyonhand + "&Vend_item_id=" + Vend_item_id + "&qtyonhold=" + qtyonhold + "&qtyonorder=" + qtyonorder + "&Cases_units=" + Cases_units + "&item_type=" + item_type + "&PurchaseOrderInventory=" + PurchaseOrder + "&BMUnivCode=" + BMUnivCode + "", '_blank', 'resizable=0,width=650px,height=390px,top=100,left=300');
        }
        else {
            window1.close();
            window1 = window.open("POSQtyPerCasePopupAdd.aspx?skuid=" + sku + "&defaultcase=" + defaultcase + "&caseof=" + caseof + "&casedollar=" + casedollar + "&plus=" + plus + "&unitdollar=" + unitdollar + "&totalingqtychkd=" + totalingqtychkd + "&qtypercase=" + qtypercase + "&desc=" + desc + "&prosku=" + prosku + "&invsize=" + invsize + "&lastcost=" + lastcost + "&price=" + price + "&dept=" + dept + "&markup=" + markup + "&txtqtytorec=" + txtqtytorec + "&txtcostea=" + txtcostea + "&txttotaling=" + txttotaling + "&extcost=" + extcost + "&qtyonhand=" + qtyonhand + "&Vend_item_id=" + Vend_item_id + "&qtyonhold=" + qtyonhold + "&qtyonorder=" + qtyonorder + "&Cases_units=" + Cases_units + "&item_type=" + item_type + "&PurchaseOrderInventory=" + PurchaseOrder + "&BMUnivCode=" + BMUnivCode + "", '_blank', 'resizable=0,width=650px,height=390px,top=100,left=300');
        }
    }

    function SelectSibling(e) {
        var e = e ? e : window.event;
        var KeyCode = e.which ? e.which : e.keyCode;

        if (SelectedRow == null) {
            SelectedRowIndex = -1;
        }

        if (KeyCode == 40) {
            SelectRow(SelectedRow.nextSibling, SelectedRowIndex + 1);
        }
        else if (KeyCode == 38) {
            SelectRow(SelectedRow.previousSibling, SelectedRowIndex - 1);
        }
    }
    function PressEsc(e) {
        var e = e ? e : window.event;
        var KeyCode = e.which ? e.which : e.keyCode;
        if (KeyCode == 27) {
            if ('<%=ViewState("isInv").ToString() %>' == 'Y') {
                window.opener.document.getElementById('ctl00_ContentPlaceHolder2_txtSKU').focus();
            }
            window.close();
            return false;
        }
    }
    function getskufocus() {
        if ('<%=ViewState("isInv").ToString() %>' == 'Y') {
            window.opener.focus();
            if (window.opener.document.getElementById('ctl00_ContentPlaceHolder2_txtSKU') != null) {
                window.opener.document.getElementById('ctl00_ContentPlaceHolder2_txtSKU').focus();
            }
        }
        window.close();
        return false;
    }

    function initializeGridinv() {
        Pageonindex = parseInt(document.getElementById('<%=hdnPageonindexinv.ClientID %>').value);

        CurrentPageBound = parseInt(document.getElementById('<%=hdnCurrentpageinv.ClientID %>').value);
        UpperPageBound = parseInt(document.getElementById('<%=hdnPagecountinv.ClientID %>').value);
        LowerPageBound = 0;

        UpperRowBound = parseInt(document.getElementById('<%= hdnPagesizeinv.ClientID %>').value) - 1;
        LowerRowBound = 0;

        Btnid = document.getElementById('<%=hdnBtnidinv.ClientID %>').value;
        SelectedRowIndex = -1;
    }
</script>
<script type="text/javascript" language="javascript">
    function onBlurDecimalWatermark(id) {
        var price = document.getElementById(id);
        if (price.value != '' && price.value != '--All--') {
            price.value = parseFloat(price.value).toFixed(2);
        }
    }
    function setfocus_invpopup() {
        try {
            var vartxtsearch = document.getElementById('<%=txtSearch.ClientID%>')
            if (vartxtsearch != null) {
                if (vartxtsearch != '') {
                    document.getElementById('<%=txtSearch.ClientID%>').focus();
                    return true
                }
            }
        }
        catch (err) {

        }
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

    function onKeyDown(e) {
        if (e && e.keyCode == Sys.UI.Key.esc) {
            // if the key pressed is the escape key, dismiss the dialog

            if (popup == 'hide') {
                var exit = document.getElementById('<%=imgbtnExitInventory.ClientID %>');
                exit.click();
            }


        }
    }
    function mphide() {
        $find('mpeQtyperCase').hide();
        document.getElementById('<%=txtSearch.ClientID%>').focus();
    }

</script>
<script type="text/javascript" language="javascript">
    var popupWindow = null;
    var popupReturns = null;
    var popupCalledFor = null;
    var w = 630;
    var h = 370;
    var wLeft = window.screenLeft ? window.screenLeft : window.screenX;
    var wTop = window.screenTop ? window.screenTop : window.screenY;
    function PoQty_Blur() {
        var txtPoQty_ = document.getElementById("<%=txtQtytoReceive.ClientId %>").value;
        var txtUnitCost_ = document.getElementById("<%=txtCostEa.ClientId %>").value;
        document.getElementById("<%=txtTotaling.ClientId %>").value = parseFloat(txtPoQty_ * txtUnitCost_).toFixed(2);
    }
    function UnitCost_Blur() {
        var txtCostExt_ = document.getElementById("<%=txtTotaling.ClientId %>").value;
        var txtPoQty_ = document.getElementById("<%=txtQtytoReceive.ClientId %>").value;
        if (txtCostExt_ != '0.00' && txtPoQty_ != '0') {
            document.getElementById("<%=txtCostEa.ClientId %>").value = parseFloat(txtCostExt_ / txtPoQty_).toFixed(2);
        }
        else {
            document.getElementById("<%=txtCostEa.ClientId %>").value = '0.00';
        }
    }
    function POQtyBlur() {
        var txtCasesInv = document.getElementById("<%= txtDefaultCase.ClientId %>").value;
        var txtQtyPerCaseInv = document.getElementById("<%= txtCaseOf.ClientId %>").value;
        var txtExtraInv = document.getElementById("<%= txtPlus.ClientId %>").value;
        var txtUnitCostInv = document.getElementById("<%= txtUnitDollar.ClientId %>").value;
        var totalQty = parseInt(txtCasesInv * txtQtyPerCaseInv) + parseInt(txtExtraInv);
        document.getElementById("<%= txtPoTotal.ClientId %>").value = totalQty
        document.getElementById("<%=txtcaseDollar.ClientId %>").value = parseFloat(txtQtyPerCaseInv * txtUnitCostInv).toFixed(2);
        document.getElementById("<%=txttotalingQtyChecked .ClientId %>").value = parseFloat(totalQty * txtUnitCostInv).toFixed(2);
    }
    function CostExtInvBlur() {
        var txtCasesInv = document.getElementById("<%= txtDefaultCase.ClientId %>").value;
        var txtQtyPerCaseInv = document.getElementById("<%= txtCaseOf.ClientId %>").value;
        var txtExtraInv = document.getElementById("<%= txtPlus.ClientId %>").value;
        var CostExtInv = document.getElementById("<%=txttotalingQtyChecked .ClientId %>").value;
        var txtUnitCostInv = document.getElementById("<%= txtUnitDollar.ClientId %>").value;
        var totalQty = parseInt(txtCasesInv * txtQtyPerCaseInv) + parseInt(txtExtraInv);
        if (totalQty != '0' && CostExtInv != '0.00') {
            document.getElementById("<%= txtUnitDollar.ClientId %>").value = parseFloat(CostExtInv / totalQty).toFixed(2);
        }
        else {
            document.getElementById("<%= txtUnitDollar.ClientId %>").value = '0.00';
        }
    }
    function ExtraInvBlur() {
        var txtQtyPerCaseInv = document.getElementById("<%= txtCaseOf.ClientId %>").value;
        var txtExtraInv = document.getElementById("<%= txtPlus.ClientId %>").value;
        var txtCasesInv = document.getElementById("<%= txtDefaultCase.ClientId %>").value;
        if (txtQtyPerCaseInv == txtExtraInv) {
            if (txtQtyPerCaseInv != '0' && txtExtraInv != '0') {
                document.getElementById("<%= txtDefaultCase.ClientId %>").value = parseInt(txtCasesInv) + 1;
                document.getElementById("<%= txtPlus.ClientId %>").value = 0;
            }
        }
        else if (txtQtyPerCaseInv < txtExtraInv) {
            if (txtQtyPerCaseInv != '0') {
                var ExtraResult = txtExtraInv % txtQtyPerCaseInv;
                var result = txtExtraInv / txtQtyPerCaseInv;
                result = parseFloat(result).toFixed(2);
                var Final_Result = result.split('.');
                document.getElementById("<%= txtDefaultCase.ClientId %>").value = parseInt(txtCasesInv) + parseInt(Final_Result[0]);
                document.getElementById("<%= txtPlus.ClientId %>").value = ExtraResult;
            }
        }
    POQtyBlur();
}
function LTrim(value) {
    var re = /\s*((\S+\s*)*)/;
    return value.replace(re, "$1");
}
function RTrim(value) {
    var re = /((\s*\S+)*)\s*/;
    return value.replace(re, "$1");
}
function trim(value) {
    return LTrim(RTrim(value));
}
function checkblank(element) {
    if (trim(element.value) == '')
    { element.value = '0'; }
}
function onBlurDecimal(id) {
    var price = document.getElementById(id);
    var num = parseFloat(price.value).toFixed(2);
    if (num == '') {
        price.value = '0.00';
    }
    else {
        price.value = num;
    }

    return false;
}
function fnOpenDept() {
    //debugger;
    var left = wLeft + (window.innerWidth / 2) - (w / 2);
    var top = wTop + (window.innerHeight / 2) - (h / 2);
    var ans;
    ans = "select dept_id as $ID$,dept_desc as $Desc$ from department where storeno= " + '<%=ViewState("storeno") %>' + " and dept_desc <> $ $ AND  RTRIM(LTRIM(dept_desc)) <> $Discount All Items$ AND RTRIM(LTRIM(dept_desc)) <> $Points Program$ order by dept_desc ASC "
    var count;
    if (document.getElementById('<%=hdnDept.ClientId %>').value == '') {
            count = -1;
        }
        else {
            count = document.getElementById('<%=hdnDept.ClientId %>').value;
        }

     // var res = window.showModalDialog("POPUpDSSV.aspx?seltype=radio&hidein=txtDeptSearch&countin=" + count + "&countPopup=0&qur=" + ans + "&searchcol=dept_desc&popupfor=Department", "", "scroll:no;resizable:no;status:no;dialogWidth:400px;dialogHeight:430px");
   // var res = window.open("POPUpDSSV.aspx?seltype=radio&hidein=txtDeptSearch&countin=" + count + "&countPopup=0&qur=" + ans + "&searchcol=dept_desc&popupfor=Department", "", "scroll:no;resizable:no;status:no;dialogWidth:400px;dialogHeight:430px");
    
    //if (res != undefined) {
    // var Final_Result = res.split('#####');
    //if (Final_Result[0] == '') {
    //document.getElementById('<%=lblDeptddl.ClientID %>').innerHTML = '--Select--';
    //document.getElementById('<%=hdnDeptText.ClientID %>').value = '--Select--';
    //}
    //else {
    //document.getElementById('<%=lblDeptddl.ClientID %>').innerHTML = Final_Result[0];
    //document.getElementById('<%=hdnDeptText.ClientID %>').value = Final_Result[0];
    //}
    ////
    //document.getElementById('<%=hdnDept.ClientID %>').value = Final_Result[1];
    //return true;
    //}
    //else {
    //return false;
    //
    //}
    popupReturns = null;
    popupCalledFor = 'Department';
    popupWindow = window.open("POPUpDSSV.aspx?seltype=radio&hidein=txtDeptSearch&countin=" + count + "&countPopup=0&qur=" + ans + "&searchcol=dept_desc&popupfor=" + popupCalledFor + "&DefaultValue=NS", "", "directories=no,titlebar=no,toolbar=no,location=no,status=no,menubar=no,scrollbars=no,resizable=no,center=yes,width=400,height=430,top=" + top + ",left=" + left, false);

    popupWindow.focus();

    return false;
    }
    function fnOpenSize() {
        var left = wLeft + (window.innerWidth / 2) - (w / 2);
        var top = wTop + (window.innerHeight / 2) - (h / 2);
        var ans;
        ans = "Select distinct size_id as $ID$, ltrim(rtrim(size)) as $Desc$,size from size where storeno = " + '<%=Session("storeno") %>' + " and size_id <> 0 order by size asc  "

        var count;
        if (document.getElementById('<%=hdnSize.ClientId %>').value == '') {
        count = -1;
         }
        else {
        count = document.getElementById('<%=hdnSize.ClientId %>').value;
        }
        //var res = window.showModalDialog("POPUpDSSV.aspx?seltype=radio&hidein=txtSize&countin=" + count + "&countPopup=0&qur=" + ans + "&searchcol=size&popupfor=Size&DefaultValue=NS", "", "scroll:no;resizable:no;status:no;dialogWidth:400px;dialogHeight:430px");

        //if (res != undefined) {
        //   var Final_Result = res.split('#####');
        //   if (Final_Result[0] == '') {
        //   document.getElementById('<%=lblSizeddl.ClientID %>').value = '--Select--';
        //     document.getElementById('<%=hdnSizeText.ClientID %>').value = '--Select--';

        //  }

        //   else {
        //    document.getElementById('<%=lblSizeddl.ClientID %>').value = Final_Result[0];
        //   document.getElementById('<%=hdnSizeText.ClientID %>').value = Final_Result[0];


        //   }

        //  document.getElementById('<%=hdnSize.ClientID %>').value = Final_Result[1];
        //     return true;
        //  }
        // else {
        //  return false;

        // }
        popupReturns = null;
        popupCalledFor = 'Size';
        popupWindow = window.open("POPUpDSSV.aspx?seltype=radio&hidein=txtSize&countin=" + count + "&countPopup=0&qur=" + ans + "&searchcol=size&popupfor=Size&DefaultValue=NS", "", "directories=no,titlebar=no,toolbar=no,location=no,status=no,menubar=no,scrollbars=no,resizable=no,center=yes,width=400,height=430,top=" + top + ",left=" + left, false);
        popupWindow.focus();
        return false;
    }


    function fnOpenStyle() {
        var left = wLeft + (window.innerWidth / 2) - (w / 2);
        var top = wTop + (window.innerHeight / 2) - (h / 2);
        var DeptId = document.getElementById('<%=hdnDept.ClientID %>').value
        var ans;
        if (DeptId != "" && DeptId != "0") {
            ans = "select distinct style_id as $ID$, ltrim(rtrim(style))as $Desc$,style from style where storeno= " + '<%=Session("storeno") %>' + " and style <> $$ and deptid = $" + DeptId + "$ order by style asc ";
        }
        else {
            ans = "select distinct style_id as $ID$, ltrim(rtrim(style))as $Desc$,style from style where storeno= " + '<%=Session("storeno") %>' + " and style <> $$ order by style asc ";
        }

        var count;
        if (document.getElementById('<%=hdnStyle.ClientId %>').value == '') {
            count = -1;
        }
        else {
            count = document.getElementById('<%=hdnStyle.ClientId %>').value;
        }
        // var res = window.showModalDialog("POPUpDSSV.aspx?seltype=radio&hidein=hdnStyle&countin=" + count + "&countPopup=0&qur=" + ans + "&searchcol=style&popupfor=Style&DefaultValue=NS", "", "scroll:no;resizable:no;status:no;dialogWidth:400px;dialogHeight:430px");

        //if (res != undefined) {
            //var Final_Result = res.split('#####');

            // if (Final_Result[0] == '') {
                //  document.getElementById('<%=lblStyleddl.ClientID %>').innerHTML = '--Select--';
                //  document.getElementById('<%=hdnStyleText.ClientID %>').value = '--Select--';

                //  }
            // else {
                //   document.getElementById('<%=lblStyleddl.ClientID %>').innerHTML = Final_Result[0];
                // document.getElementById('<%=hdnStyleText.ClientID %>').value = Final_Result[0];

                // }


            //document.getElementById('<%=hdnStyle.ClientID %>').value = Final_Result[1];
            // return true;
            // }
        //  else {
            //return false;
            // }


        popupReturns = null;
        popupCalledFor = 'Style';
        popupWindow = window.open("POPUpDSSV.aspx?seltype=radio&hidein=hdnStyle&countin=" + count + "&countPopup=0&qur=" + ans + "&searchcol=style&popupfor=Style&DefaultValue=NS", "", "directories=no,titlebar=no,toolbar=no,location=no,status=no,menubar=no,scrollbars=no,resizable=no,center=yes,width=400,height=430,top=" + top + ",left=" + left, false);
        popupWindow.focus();
        return false;
    }
    // add new code

    function setValue(val1) {
        // do your logic here
        //alert(val1);
        popupReturns = val1;
        //waitForClosePopup();
        parent_disable('Y');
    }

    function waitForClosePopup() {
        if (popupWindow && !popupWindow.closed) {

            var timer = setInterval(function () {
                if (popupWindow.closed) {
                    clearInterval(timer);
                    parent_disable();
                }
            }, 500);

        }

    }

    function parent_disable(ConsiderClosed) {
        //debugger;
        if (popupWindow && !popupWindow.closed && ConsiderClosed != 'Y') {

            popupWindow.focus();

            if (document.getElementById("dvDisableBackground") != null) {

                if (document.getElementById("dvDisableBackground").style.display != "") {
                    document.getElementById("dvDisableBackground").style.display = "";
                }
            }
        }
        else if ((popupWindow != undefined && popupWindow.closed) || ConsiderClosed == 'Y') {
            if (document.getElementById("dvDisableBackground") != null) {
                if (document.getElementById("dvDisableBackground").style.display != "none") {
                    document.getElementById("dvDisableBackground").style.display = "none";
                }
            }

            //alert(popupReturns);
            if (popupReturns != null && popupReturns != undefined) {
                var Final_Result = popupReturns.split('#####');
                popupReturns = null;

                if (popupCalledFor != null) {
                    switch (popupCalledFor) {
                        case 'VendorSearch':
                            //Vendor Search Popup
                            if (Final_Result[0] == '') {
                                document.getElementById('<%=txtVendorSearch.ClientID %>').innerHTML = '--All--';
                                        document.getElementById('<%=hdnVendorSearchText.ClientID %>').value = '--All--';
                                    }
                                    else {
                                        document.getElementById('<%=txtVendorSearch.ClientID %>').innerHTML = Final_Result[0];
                                        document.getElementById('<%=hdnVendorSearchText.ClientID %>').value = Final_Result[0];
                                    }
                                    document.getElementById('<%=hdnVendorSearch.ClientID %>').value = Final_Result[1];
                                    
                                    break;

                                case 'DepartmentSearch':
                                    //Department Search Popup
                                    if (Final_Result[0] == '') {
                                        document.getElementById('<%=txtDeptSearch.ClientID %>').innerHTML = '--All--';
                                        document.getElementById('<%=hdnDeptSearchText.ClientID %>').value = '--All--';
                                    }
                                    else {
                                        document.getElementById('<%=txtDeptSearch.ClientID %>').innerHTML = Final_Result[0];
                                        document.getElementById('<%=hdnDeptSearchText.ClientID %>').value = Final_Result[0];
                                    }
                                    document.getElementById('<%=hdnDeptSearch.ClientID %>').value = Final_Result[1];
                                    
                                    break;

                                case 'StyleSearch':
                                    //Style Search Popup
                                    //alert('Style Popup');
                                    if (Final_Result[0] == '') {
                                        document.getElementById('<%=txtStyleSearch.ClientID %>').innerHTML = '--All--';
                                         document.getElementById('<%=hdnStyleSearchText.ClientID %>').value = '--All--';
                                     }
                                     else {
                                         document.getElementById('<%=txtStyleSearch.ClientID %>').innerHTML = Final_Result[0];
                                         document.getElementById('<%=hdnStyleSearchText.ClientID %>').value = Final_Result[0];
                                     }
                                     document.getElementById('<%=hdnStyleSearch.ClientID %>').value = Final_Result[1];
                                     
                                     break;

                                 case 'SizeSearch':
                                     //Size Search Popup
                                     //alert('Size Popup');
                                     if (Final_Result[0] == '') {
                                         document.getElementById('<%=txtSizeSearch.ClientID %>').innerHTML = '--All--';
                                        document.getElementById('<%=hdnSizeSearchText.ClientID %>').value = '--All--';
                                    }
                                    else {
                                        document.getElementById('<%=txtSizeSearch.ClientID %>').innerHTML = Final_Result[0];
                                        document.getElementById('<%=hdnSizeSearchText.ClientID %>').value = Final_Result[0];
                                    }
                                    document.getElementById('<%=hdnSizeSearch.ClientID %>').value = Final_Result[1];
                                    
                                    break;

                                case 'Department':
                                    //Department Popup
                                    if (Final_Result[0] == '') {
                                        document.getElementById('<%=lblDeptddl.ClientID %>').innerHTML = '--Select--';
                                    document.getElementById('<%=hdnDeptText.ClientID %>').value = '--Select--';
                                }
                                else {
                                        document.getElementById('<%=lblDeptddl.ClientID %>').innerHTML = Final_Result[0];
                                    document.getElementById('<%=hdnDeptText.ClientID %>').value = Final_Result[0];
                                }
                                document.getElementById('<%=hdnDept.ClientID %>').value = Final_Result[1];
                                
                                break;

                            case 'Style':
                                //Style Popup
                                if (Final_Result[0] == '') {
                                    document.getElementById('<%=lblStyleddl.ClientID%>').innerHTML = '--Select--';
                                    document.getElementById('<%=hdnStyleText.ClientID %>').value = '--Select--';
                                }
                                else {
                                    document.getElementById('<%=lblStyleddl.ClientID%>').innerHTML = Final_Result[0];
                                    document.getElementById('<%=hdnStyleText.ClientID %>').value = Final_Result[0];

                                }
                                document.getElementById('<%=hdnStyle.ClientID %>').value = Final_Result[1];
                                break;

                        case 'Size':
                           // debugger;
                                //Size Popup
                                if (Final_Result[0] == '') {
                                    document.getElementById('<%=lblSizeddl.ClientID%>').value = '--Select--';
                                    document.getElementById('<%=hdnSizeText.ClientID %>').value = '--Select--';
                                }
                                else {
                                    document.getElementById('<%=lblSizeddl.ClientID%>').innerHTML = Final_Result[0];
                                    document.getElementById('<%=hdnSizeText.ClientID %>').value = Final_Result[0];
                                }
                            document.getElementById('<%=hdnSize.ClientID %>').value = Final_Result[1];


                                                            break;
                                //

                                
                            
                            default:
                                break;
                        }
                    }
                }
            }
    }
</script>
<script type="text/javascript" language="javascript">
    var popupWindow = null;
    var popupReturns = null;
    var popupCalledFor = null;
    var w = 630;
    var h = 370;
    var wLeft = window.screenLeft ? window.screenLeft : window.screenX;
    var wTop = window.screenTop ? window.screenTop : window.screenY;
    function txtSkuKeyhandler1(e) {

        var e = e ? e : window.event;
        var KeyCode = e.which ? e.which : e.keyCode;
        if (KeyCode == 13) {
            Checkorder();
        }
    }

    function Checkorder() {
        var sku;
        check = document.getElementById('<%=gvInventoryList.ClientID %>');
        if (check != "") {
            for (var i = 1; i <= check.rows.length - 1; i++) {
                var cell = check.rows[i].cells[1];
                var ChkBox;
                for (j = 0; j <= cell.getElementsByTagName("input").length - 1; j++) {
                    if (cell.getElementsByTagName("input").type = "checkbox") {
                        ChkBox = cell.getElementsByTagName("input")[j];
                        var lbl = cell.getElementsByTagName("label")[j];
                        sku = lbl.innerHTML.trim();
                        break;

                    }
                }

                if (ChkBox.checked == true) {

                    /*if (/Firefox[\/\s](\d+\.\d+)/.test(navigator.userAgent)) {
                        var sku = check.rows[i].cells[1].childNodes[1].textContent.trim();
                    }
                        //else if (/MSIE([0-9]{1,}[\.0-9]{0,})/.test(navigator.userAgent))

                    else {
                        var ua = navigator.userAgent;
                        var re = new RegExp("MSIE ([0-9]{1,}[\.0-9]{0,})");
                        if (re.exec(ua) != null)
                            rv = parseFloat(RegExp.$1);
                        if (rv == 9.0) {
                            var sku = check.rows[i].cells[1].childNodes[1].textContent.trim();
                        }
                        else {
                            var sku = check.rows[i].cells[1].firstChild.innerHTML.trim();
                        }
                    }
                    */
                    var indexid = i - 1;
                    openquantitypopup(sku, indexid);
                    break;
                }

            }
        }
    }



    function Checkorder1() {

        check = document.getElementById('<%=gvInventoryList.ClientID %>');

            if (check != "") {
                for (var i = 1; i <= check.rows.length - 1; i++) {
                    //   alert(check.rows[i].style.backgroundColor);
                    if (check.rows[i].style.backgroundColor != '') {

                        if (/Firefox[\/\s](\d+\.\d+)/.test(navigator.userAgent)) {
                            var sku = check.rows[i].cells[1].childNodes[1].textContent.trim();
                        }
                        else {
                            var ua = navigator.userAgent;
                            var re = new RegExp("MSIE ([0-9]{1,}[\.0-9]{0,})");
                            if (re.exec(ua) != null)
                                rv = parseFloat(RegExp.$1);
                            if (rv == 9.0) {
                                var sku = check.rows[i].cells[1].childNodes[1].textContent.trim();
                            }
                            else {
                                var sku = check.rows[i].cells[1].firstChild.innerHTML.trim();
                            }
                        }
                        var indexid = i - 1;
                        openquantitypopup(sku, indexid);
                        break;
                    }
                }
            }
        }
        function openquantitypopup(SKU, Index) {
            document.getElementById('<%=InpSku.ClientID %>').value = SKU;
            document.getElementById('<%=hdnIndex.ClientID %>').value = Index;
            var txtsearch = document.getElementById('<%=txtSearch.ClientID %>');
            if (txtsearch.value == '') {
                var endt = document.getElementById('<%=btnEnter.ClientID %>');
                endt.click();
            }
            else {
                var endt = document.getElementById('<%=imgInvSearch.ClientID %>');
                endt.click();
            }

            return false;
        }
        function fenter() {
            Checkorder();
        }
        function txtSkuKeyhandler(e) {

            var flag = false;
            var shortcut = null;
            if (/Firefox[\/\s](\d+\.\d+)/.test(navigator.userAgent)) {
                shortcut = ["f1", "f2", "f3", "f4", "esc", "f10", "f12", "f7", "enter", "alt+f", "tab", "ctrl+f", "up", "down"];
            }
            else {
                shortcut = ["alt+f1", "alt+f2", "alt+f3", "f10", "esc", "alt+f7", "enter", "alt+f", "tab", "ctrl+f", "up", "down"];

            }
            e = e || window.event;
            //Find Which key is pressed
            if (e.keyCode) code = e.keyCode;
            else if (e.which) code = e.which;
            var character = String.fromCharCode(code).toLowerCase();
            //Work around for stupid Shift key bug created by using lowercase - as a result the shift+num combination was broken
            var shift_nums = {
                "`": "~",
                "1": "!",
                "2": "@",
                "3": "#",
                "4": "$",
                "5": "%",
                "6": "^",
                "7": "&",
                "8": "*",
                "9": "(",
                "0": ")",
                "-": "_",
                "=": "+",
                ";": ":",
                "'": "\"",
                ",": "<",
                ".": ">",
                "/": "?",
                "\\": "|"
            }
            //Special Keys - and their codes
            var special_keys = {
                'enter': 13,
            }

            for (var count = 0; count < shortcut.length; count++) {
                if (flag == false) {
                    var key = shortcut[count];
                    var keys = shortcut[count].split("+");
                    //Key Pressed - counts the number of valid keypresses - if it is same as the number of keys, the shortcut function is invoked
                    var kp = 0;
                    for (var i = 0; k = keys[i], i < keys.length; i++) {
                        //Modifiers
                        if (k == 'ctrl' || k == 'control') {
                            if (e.ctrlKey) kp++;

                        } else if (k == 'shift') {
                            if (e.shiftKey) kp++;

                        } else if (k == 'alt') {
                            if (e.altKey) kp++;

                        } else if (k.length > 1) { //If it is a special key
                            if (special_keys[k] == code) kp++;

                        } else { //The special keys did not match
                            if (character == k) kp++;
                            else {
                                if (shift_nums[character] && e.shiftKey) { //Stupid Shift key bug created by using lowercase
                                    character = shift_nums[character];
                                    if (character == k) kp++;
                                }
                            }
                        }
                    }

                    if (kp == keys.length) {
                        switch (key) {

                            case 'enter':
                                fenter(e);
                                break;

                            default:
                                break;
                        }

                        if (true) { //Stop the event
                            //e.cancelBubble is supported by IE - this will kill the bubbling process.
                            e.cancelBubble = true;
                            e.returnValue = false;

                            //e.stopPropagation works only in Firefox.
                            if (e.stopPropagation) {
                                e.stopPropagation();
                                e.preventDefault();
                            }
                            return false;
                        }
                        flag = true;
                    }
                }
                else {
                    break;
                }
            }
            //  }
        }

        function fnOpenDeptSearch() {
            var left = wLeft + (window.innerWidth / 2) - (w / 2);
            var top = wTop + (window.innerHeight / 2) - (h / 2);

            var ans;
            ans = "select dept_id as $ID$,dept_desc as $Desc$ from department where storeno= " + '<%=ViewState("storeno") %>' + " and dept_desc <> $ $ AND  RTRIM(LTRIM(dept_desc)) <> $Discount All Items$ AND RTRIM(LTRIM(dept_desc)) <> $Points Program$ order by dept_desc ASC "
           var count;
           if (document.getElementById('<%=hdnDeptSearch.ClientId %>').value == '') {
            count = -1;
        }
        else {
            count = document.getElementById('<%=hdnDeptSearch.ClientId %>').value;
        }

       // var res = window.showModalDialog("POPUpDSSV.aspx?seltype=radio&hidein=txtDeptSearch&countin=" + count + "&countPopup=0&qur=" + ans + "&searchcol=dept_desc&popupfor=Department", "", "scroll:no;resizable:no;status:no;dialogWidth:400px;dialogHeight:430px");
            //var res = window.showModalDialog("POPUpDSSV.aspx?seltype=radio&hidein=txtDeptSearch&countin=" + count + "&countPopup=0&qur=" + ans + "&searchcol=dept_desc&popupfor=Department", "", "scroll:no;resizable:no;status:no;dialogWidth:400px;dialogHeight:430px");

        //if (res != undefined) {
          //  var Final_Result = res.split('#####');
            //if (Final_Result[0] == '') {
              //  document.getElementById('<%=txtDeptSearch.ClientID %>').innerHTML = '--All--';
                //document.getElementById('<%=hdnDeptSearchText.ClientID %>').value = '--All--';
            //}
            //else {
              //  document.getElementById('<%=txtDeptSearch.ClientID %>').innerHTML = Final_Result[0];
                //document.getElementById('<%=hdnDeptSearchText.ClientID %>').value = Final_Result[0];
            //}
            //document.getElementById('<%=hdnDeptSearch.ClientID %>').value = Final_Result[1];
        //}
            popupReturns = null;
            popupCalledFor = 'DepartmentSearch';
            popupWindow = window.open("POPUpDSSV.aspx?seltype=radio&hidein=txtDeptSearch&countin=" + count + "&countPopup=0&qur=" + ans + "&searchcol=dept_desc&popupfor=Department", "", "directories=no,titlebar=no,toolbar=no,location=no,status=no,menubar=no,scrollbars=no,resizable=no,center=yes,width=400,height=430,top=" + top + ",left=" + left, false);
            popupWindow.focus();
        return false;
    }
    function fnOpenStyleSearch() {
        var left = wLeft + (window.innerWidth / 2) - (w / 2);
        var top = wTop + (window.innerHeight / 2) - (h / 2);
        var DeptId = document.getElementById('<%=hdnDeptSearch.ClientID %>').value
        var ans;
        if (DeptId != "" && DeptId != "0") {
            ans = "select distinct style_id as $ID$, ltrim(rtrim(style))as $Desc$,style from style where storeno= " + '<%=Session("storeno") %>' + " and style <> $ $  and deptid = $" + DeptId + "$ order by style asc ";
        }
        else {
            ans = "select distinct style_id as $ID$, ltrim(rtrim(style))as $Desc$,style from style where storeno= " + '<%=Session("storeno") %>' + " and style <> $$ order by style asc ";
        }
        var count;
        if (document.getElementById('<%=hdnStyleSearch.ClientId %>').value == '') {
            count = -1;
        }
        else {
            count = document.getElementById('<%=hdnStyleSearch.ClientId %>').value;
        }
       // var res = window.showModalDialog("POPUpDSSV.aspx?seltype=radio&hidein=txtStyleSearch&countin=" + count + "&countPopup=0&qur=" + ans + "&searchcol=style&popupfor=Style", "", "scroll:no;resizable:no;status:no;dialogWidth:400px;dialogHeight:430px");

        // if (res != undefined) {
        //    var Final_Result = res.split('#####');
        //  if (Final_Result[0] == '') {
        //       document.getElementById('<%=txtStyleSearch.ClientID %>').innerHTML = '--All--';
        //      document.getElementById('<%=hdnStyleSearchText.ClientID %>').value = '--All--';

        //   }
        //   else {
        //        document.getElementById('<%=txtStyleSearch.ClientID %>').innerHTML = Final_Result[0];
        //       document.getElementById('<%=hdnStyleSearchText.ClientID %>').value = Final_Result[0];

        // }
        //  document.getElementById('<%=hdnStyleSearch.ClientID %>').value = Final_Result[1];
        //  }
        popupReturns = null;
        popupCalledFor = 'StyleSearch';
        popupWindow = window.open("POPUpDSSV.aspx?seltype=radio&hidein=txtsizesearch&countin=" + count + "&countPopup=0&qur=" + ans + "&searchcol=size&popupfor=Size", "", "directories=no,titlebar=no,toolbar=no,location=no,status=no,menubar=no,scrollbars=no,resizable=no,center=yes,width=400,height=430,top=" + top + ",left=" + left, false);
        popupWindow.focus();

        return false;
    }


    function fnOpenSizeSearch() {
        var left = wLeft + (window.innerWidth / 2) - (w / 2);
        var top = wTop + (window.innerHeight / 2) - (h / 2);
        var ans;
        ans = "Select distinct size_id as $ID$, ltrim(rtrim(size)) as $Desc$,size from size where storeno = " + '<%=Session("storeno") %>' + " and size_id <> 0 order by size asc  "
        var count;
        if (document.getElementById('<%=hdnSizeSearch.ClientId %>').value == '') {
            count = -1;
        }
        else {
            count = document.getElementById('<%=hdnSizeSearch.ClientId %>').value;
        }
      //  var res = window.showModalDialog("POPUpDSSV.aspx?seltype=radio&hidein=txtsizesearch&countin=" + count + "&countPopup=0&qur=" + ans + "&searchcol=size&popupfor=Size", "", "scroll:no;resizable:no;status:no;dialogWidth:400px;dialogHeight:430px");

       // if (res != undefined) {
         //   var Final_Result = res.split('#####');
        //   if (Final_Result[0] == '') {
        //      document.getElementById('<%=txtSizeSearch.ClientID %>').value = '--All--';
        //     document.getElementById('<%=hdnSizeSearchText.ClientID %>').value = '--All--';
        //
        //   }
        //
        //    else {
        //    document.getElementById('<%=txtSizeSearch.ClientID %>').value = Final_Result[0];
        //       document.getElementById('<%=hdnSizeSearchText.ClientID %>').value = Final_Result[0];


        // }
        // document.getElementById('<%=hdnSizeSearch.ClientID %>').value = Final_Result[1];
        // }


        popupReturns = null;
        popupCalledFor = 'SizeSearch';
        popupWindow = window.open("POPUpDSSV.aspx?seltype=radio&hidein=txtsizesearch&countin=" + count + "&countPopup=0&qur=" + ans + "&searchcol=size&popupfor=Size", "", "directories=no,titlebar=no,toolbar=no,location=no,status=no,menubar=no,scrollbars=no,resizable=no,center=yes,width=400,height=430,top=" + top + ",left=" + left, false);
        popupWindow.focus();
        return false;
    }

    function fnOpenVendorSearch() {
        var left = wLeft + (window.innerWidth / 2) - (w / 2);
        var top = wTop + (window.innerHeight / 2) - (h / 2);
        var ans;
        ans = "select distinct vendor_id as $ID$,ltrim(rtrim(company)) as $Desc$,company from vendor_mst where storeno= " + '<%=Session("storeno") %>' + " order by company asc";
        var count;
        if (document.getElementById('<%=hdnVendorSearch.ClientId %>').value == '') {
            count = -1;
        }
        else {
            count = document.getElementById('<%=hdnVendorSearch.ClientId %>').value;
        }
       // var res = window.showModalDialog("POPUpDSSV.aspx?seltype=radio&hidein=txtVendorName&countin=" + count + "&countPopup=0&qur=" + ans + "&searchcol=company&popupfor=Vendor", "", "scroll:no;resizable:no;status:no;dialogWidth:400px;dialogHeight:430px");

        //if (res != undefined) {
          //  var Final_Result = res.split('#####');
         //   if (Final_Result[0] == '') {

          //      document.getElementById('<%=txtVendorSearch.ClientID %>').value = '--All--';
           //     document.getElementById('<%=hdnVendorSearchText.ClientID %>').value = '--All--';

         //   }
          //  else {
           //     document.getElementById('<%=txtVendorSearch.ClientID %>').value = Final_Result[0];
            //    document.getElementById('<%=hdnVendorSearchText.ClientID %>').value = Final_Result[0];

           // }
            //document.getElementById('<%=hdnVendorSearch.ClientID %>').value = Final_Result[1];
       // }

        popupReturns = null;
        popupCalledFor = 'VendorSearch';
        popupWindow = window.open("POPUpDSSV.aspx?seltype=radio&hidein=txtVendorName&countin=" + count + "&countPopup=0&qur=" + ans + "&searchcol=company&popupfor=Vendor", "", "directories=no,titlebar=no,toolbar=no,location=no,status=no,menubar=no,scrollbars=no,resizable=no,center=yes,width=400,height=430,top=" + top + ",left=" + left, false);
        popupWindow.focus();
        //return false;

        return false;
    }

    function CallPoursItemPopup(SKU) {
        //var Out = window.showModalDialog("POSVerifoneSignature.aspx?MXCOM=" + MXCOM + "&Message=" + Message + "&TranType=S", "", "scroll:no;resizable:no;dialogWidth:500px;dialogHeight:600px;center:yes;");
        if (navigator.appName.indexOf("Internet") != -1) {

            var invpopup = window.showModalDialog("POSInventoryPoursPopup.aspx?SKU=" + SKU + "&ringsales=1&ItemType=m", "", "resizable:no;dialogWidth:770px;dialogHeight:500px;dialogLeft:70px;dialogTop:5px");

            if (invpopup != undefined && invpopup != '') {
                document.getElementById('<%=hdnPourSelection.ClientID%>').value = invpopup
                var btn = document.getElementById('<%=btnmultipack.ClientID%>');
                btn.click();
               // setTimeout("CloseWin()", 10);


        }
        else {
            document.getElementById('<%=hdnPourSelection.ClientID%>').value = '';
           
        }
    }
    else {
        w = 770;
        h = 500;

        var left = wLeft + (window.innerWidth / 2) - (w / 2);
        var top = wTop + (window.innerHeight / 2) - (h / 2);
        var win = window.open('POSInventoryPoursPopup.aspx?SKU=' + SKU + '&ringsales=1', '', 'resizable=no,width=' + w + ',height=' + h + ',top=' + top + ',left=' + left);

    }
    return false;
    }
    function pouritem(popupsku) {
        document.getElementById('<%=hdnPourSelection.ClientID%>').value = popupsku
        var btn = document.getElementById('<%=btnmultipack.ClientID%>');
        btn.click();
        setTimeout("CloseWin()", 10);
    }
    function alertmultipack() {
        alert("You can't add a Multi-Pack item to a Kit/Club item");
    }

</script>
<body onkeypress="PressEsc(event);">
    <form id="form1" runat="server">
        <div id="divheightwidth" style="width: 100%">
            <ajax:ScriptManager ID="ScriptManager1" EnablePageMethods="true" runat="server">
            </ajax:ScriptManager>
            <ajax:UpdatePanel ID="updatepanelinventory" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:LinkButton ID="btnEnter" runat="server" Style="display: none;"></asp:LinkButton>
                    <asp:HiddenField ID="hdnIndex" runat="server" />

                    <input id="InpSku" runat="server" type="hidden" />

                    <div>
                        <table cellpadding="0" cellspacing="0" width="100%">
                            <tr>
                                <td align="left" valign="middle" bgcolor="#B8C7D3">
                                    <center>
                                        <table border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="#b6c5d1">
                                            <tr>

                                                <td id="td4" class="labelHead" runat="server" style="padding-left: 10px; text-align: center; border-right: 1px solid gray; color: #0101DF; align-content: center; align-items: center; align-self: center; vertical-align: middle; background-color: #B6C5D1;" height="25px" width="32%">
                                                    <asp:LinkButton ID="lnkList" runat="server" CommandArgument="1" CssClass="style8black"
                                                        Font-Underline="False" OnCommand="lnk_ClickNew" Text="List"></asp:LinkButton>
                                                </td>

                                                <td id="td5" class="labelHead" runat="server" style="padding-left: 10px; text-align: center; border-right: 1px solid gray; color: #74909C; align-content: center; align-items: center; align-self: center; vertical-align: middle; background-color: #B6C5D1;" height="25px" width="32%">
                                                    <asp:LinkButton ID="lnkSalesHistory" runat="server" CommandArgument="2" CssClass="style8"
                                                        Font-Underline="False" OnCommand="lnk_ClickNew" Text="Sales History"></asp:LinkButton>
                                                </td>
                                                <td id="td6" class="labelHead" runat="server" style="padding-left: 10px; text-align: center; color: #74909C; padding-right: 10px; align-content: center; align-items: center; align-self: center; vertical-align: middle; background-color: #B6C5D1;" height="25px" width="32%">
                                                    <asp:LinkButton ID="lnkPurchseHistory" runat="server" CommandArgument="3" CssClass="style8"
                                                        Font-Underline="False" OnCommand="lnk_ClickNew" Text="Purchase History"></asp:LinkButton>
                                                </td>

                                            </tr>

                                        </table>
                                    </center>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:MultiView ID="InvMultiview" runat="server">
                                        <asp:View ID="VwYesterday" runat="server">
                                            <table cellpadding="0" cellspacing="0" border="0" align="center" style="width: 100%">
                                                <tr>
                                                    <td align="left" valign="top" height="50px">
                                                        <table width="100%" border="0" cellspacing="0" cellpadding="0" bgcolor="white">
                                                            <tr style="height: 8px;">
                                                                <td width="5"></td>
                                                                <td align="left" valign="top" colspan="2">
                                                                    <asp:HiddenField ID="hdninvpoints" runat="server" />
                                                                    <asp:HiddenField ID="hidaltf2" runat="server" Value="0" />
                                                                    <asp:HiddenField ID="hdnmaingridmode" runat="server" />
                                                                    <asp:HiddenField ID="hdnblankline" runat="server" />
                                                                    <asp:HiddenField ID="hdnsearch" runat="server" />
                                                                </td>
                                                                <td align="left" valign="top"></td>
                                                            </tr>
                                                            <tr>
                                                                <td>&nbsp;
                                                                </td>
                                                                <td align="left" valign="top" class="tab1" colspan="2">
                                                                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                        <tr>
                                                                            <td height="8" align="left" valign="top">
                                                                                <img src="images/crnr1w.gif" alt="" width="8" height="8" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td valign="middle">&nbsp;
                                                                            </td>
                                                                            <td align="left" colspan="2">
                                                                                <asp:CheckBox ID="chkItemvendor" runat="server" CssClass="labelall" Text="Item #" Style="font-size: 16px; margin-right: 7%" AutoPostBack="true" />&nbsp;&nbsp;&nbsp;
                                                    <asp:CheckBox ID="chkInvwildcardsearch" runat="server" CssClass="checkboxall"
                                                        Text="Wildcard Search" Style="font-size: 16px;" AutoPostBack="true" />
                                                                                &nbsp;<asp:TextBox ID="txtSearch" runat="server" onkeypress="txtSkuKeyhandler(event);" CssClass="textboxall" Width="420px" TabIndex="700"> </asp:TextBox><asp:ImageButton ID="ImageButtonforgrid" runat="server"
                                                                                    CausesValidation="false" ImageUrl="~/Images/entergrid.jpg"
                                                                                    Style="position: relative;" />
                                                                                <asp:HiddenField ID="hdntestinvtest" runat="server" />
                                                                                <asp:Button ID="imgInvSearch" runat="server" CausesValidation="false" Width="70px" Style="cursor: pointer;"
                                                                                    Font-Bold="True" Font-Names="Arial" Font-Size="12px" Height="25px"
                                                                                    TabIndex="701" Text="Find" />
                                                                                <asp:Button ID="btnInvtoryAdvanceSearch" runat="server"
                                                                                    CausesValidation="false" Font-Bold="True" Font-Names="Arial" Font-Size="12px"
                                                                                    Height="25px" TabIndex="702" Style="cursor: pointer;"
                                                                                    Text="Advanced Search" />
                                                                                <asp:CheckBox ID="chkinventorylist" runat="server" AutoPostBack="false"
                                                                                    csscalss="labelall" Font-Bold="true" ForeColor="Black" Style="display: none;"
                                                                                    Text="Show Records" />
                                                                                <input id="hdnPageonindexinv" runat="server" type="hidden" />
                                                                                <input id="hdnCurrentpageinv" runat="server" type="hidden" />
                                                                                <input id="hdnPagecountinv" runat="server" type="hidden" />
                                                                                <input id="hdnPagesizeinv" runat="server" type="hidden" />
                                                                                <input id="hdnBtnidinv" runat="server" type="hidden" />
                                                                                <asp:HiddenField ID="hdnsku" runat="server" />
                                                                                <asp:HiddenField ID="hdnskucommand" runat="server" />
                                                                                <asp:HiddenField ID="hdnPourSelection" runat="server" />
                                                                                 <asp:Button ID="btnmultipack" runat="server" Style="display: none;" />
                                                                            </td>
                                                                        </tr>

                                                                        <tr>
                                                                            <td height="8" align="left" valign="bottom" class="tab1c" colspan="4">
                                                                                <img src="images/crnr3w.gif" alt="" width="8" height="8" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr style="height: 8px;">

                                                                <td colspan="3"></td>
                                                            </tr>
                                                            <tr id="trinvlist" runat="server">
                                                                <td width="5">&nbsp;
                                                                </td>
                                                                <td width="left" valign="top" class="style2" colspan="2" style="height: 410px; border-color: White;"
                                                                    id="tdinvdiv" runat="server">
                                                                    <div style="height: 240px; width: 100%;" id="grdinvdiv" runat="server">
                                                                        <asp:GridView ID="gvInventoryList" runat="server" AllowPaging="false" AllowSorting="True"
                                                                            AutoGenerateColumns="False" BorderColor="White" BorderWidth="0px" EmptyDataText="No Matching Data Found"
                                                                            Width="100%" PageSize="10">
                                                                            <RowStyle CssClass="row" Font-Size="16px" Height="20px" />
                                                                            <EmptyDataRowStyle ForeColor="Red" HorizontalAlign="Center" />
                                                                            <PagerStyle HorizontalAlign="Right" />
                                                                            <SelectedRowStyle />
                                                                            <HeaderStyle CssClass="header" Font-Size="16px" HorizontalAlign="Left" VerticalAlign="Middle" />
                                                                            <AlternatingRowStyle CssClass="alterrow" />
                                                                            <Columns>
                                                                                <%--  <asp:TemplateField>
                                                        <ItemTemplate>
                                                           
                                                              <asp:Label ID="lblsku" runat="server" style="display:none;"  Text='<%# Bind("item_mst_id") %>'></asp:Label>
                                                        </ItemTemplate>
                                                       
                                                         <ItemStyle Width="0.5%" BorderColor="White" />
                                                    </asp:TemplateField>--%>
                                                                                <asp:TemplateField>
                                                                                    <ItemTemplate>
                                                                                        &nbsp;
                                                                                        <asp:Label ID="lblIditem_mst_id" runat="server" Text='<%# Bind("item_mst_id") %>'></asp:Label>
                                                                                         <asp:Label ID="lblitemtype" runat="server" Text='<%# Bind("item_type")%>' Style="display:none"></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <HeaderTemplate>
                                                                                        <asp:LinkButton ID="lnkitem_mst_id" runat="server" CommandArgument="item_mst_id" Font-Underline="false"
                                                                                            CommandName="Sort" ForeColor="White">SKU</asp:LinkButton>
                                                                                        <asp:ImageButton ID="imgitem_mst_id" runat="server" />
                                                                                    </HeaderTemplate>
                                                                                    <HeaderStyle HorizontalAlign="Left" Width="17%" />
                                                                                    <ItemStyle HorizontalAlign="Left" Width="17%" BorderColor="White" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField Visible="false">
                                                                                    <ItemTemplate>
                                                                                        &nbsp;
                                                                                        <asp:Label ID="lblvend_item_id" runat="server" Text='<%# Bind("vend_item_id")%>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <HeaderTemplate>
                                                                                        <asp:LinkButton ID="lnkvend_item_id" runat="server" CommandArgument="vend_item_id" Font-Underline="false"
                                                                                            CommandName="Sort" ForeColor="White">Item #</asp:LinkButton>
                                                                                        <asp:ImageButton ID="imgvend_item_id" runat="server" />
                                                                                    </HeaderTemplate>
                                                                                    <HeaderStyle HorizontalAlign="Left" Width="17%" />
                                                                                    <ItemStyle HorizontalAlign="Left" Width="17%" BorderColor="White" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField>
                                                                                    <ItemTemplate>
                                                                                        <asp:CheckBox ID="chkColumn" runat="server" Style="display: none;" Text='<%# Bind("item_mst_id") %>' />
                                                                                        &nbsp;<asp:Label ID="lbldesc1" runat="server" Text='<%# Bind("desc1") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <HeaderTemplate>
                                                                                        <asp:LinkButton ID="lnkdesc1" runat="server" CommandArgument="desc1" CommandName="Sort" Font-Underline="false"
                                                                                            ForeColor="White">Description</asp:LinkButton>
                                                                                        <asp:ImageButton ID="imgdesc1" runat="server" />
                                                                                    </HeaderTemplate>
                                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                                    <ItemStyle HorizontalAlign="Left" BorderColor="White" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField>
                                                                                    <ItemTemplate>
                                                                                        &nbsp;
                                                                                        <asp:Label ID="lblsize" runat="server" Text='<%# Bind("size") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <HeaderTemplate>
                                                                                        <asp:LinkButton ID="lnksize" runat="server" CommandArgument="size" CommandName="Sort" Font-Underline="false"
                                                                                            ForeColor="White">Size</asp:LinkButton>
                                                                                        <asp:ImageButton ID="imgsize" runat="server" />
                                                                                    </HeaderTemplate>
                                                                                    <HeaderStyle HorizontalAlign="Left" Width="12%" />
                                                                                    <ItemStyle HorizontalAlign="Left" Width="12%" BorderColor="White" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField>
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblprice" runat="server" Text='<%# Bind("price") %>'></asp:Label>&nbsp;
                                                                                    </ItemTemplate>
                                                                                    <HeaderTemplate>
                                                                                        <asp:LinkButton ID="lnkprice" runat="server" CommandArgument="price" CommandName="Sort" Font-Underline="false"
                                                                                            ForeColor="White">Price</asp:LinkButton>
                                                                                        <asp:ImageButton ID="imgprice" runat="server" />
                                                                                    </HeaderTemplate>
                                                                                    <HeaderStyle HorizontalAlign="Right" Width="9%" />
                                                                                    <ItemStyle HorizontalAlign="Right" Width="9%" BorderColor="White" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField>
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblqty_on_hand" runat="server" Text='<%# Bind("qty_on_hand") %>'></asp:Label>&nbsp;
                                                                                    </ItemTemplate>
                                                                                    <HeaderTemplate>
                                                                                        <asp:LinkButton ID="lnkqty_on_hand" runat="server" CommandArgument="qty_on_hand" Font-Underline="false"
                                                                                            CommandName="Sort" ForeColor="White">Qty on Hand</asp:LinkButton>
                                                                                        <asp:ImageButton ID="imgqty_on_hand" runat="server" />
                                                                                    </HeaderTemplate>
                                                                                    <HeaderStyle HorizontalAlign="Right" Width="15%" />
                                                                                    <ItemStyle HorizontalAlign="Right" Width="15%" BorderColor="White" />
                                                                                </asp:TemplateField>
                                                                            </Columns>
                                                                        </asp:GridView>
                                                                        <center>
                                                                            <asp:Label ID="lblInvMessage" CssClass="labelall" runat="server" ForeColor="Red"></asp:Label>
                                                                            <asp:HiddenField ID="lblvalueinventory" runat="server" />
                                                                        </center>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="5" style="width: 100%; padding-left: 15px; height: 10px">
                                                                    <table width="100%" border="0" cellpadding="0" cellspacing="0" style="padding-top: 2px;">
                                                                        <tr>
                                                                            <td align="left" style="font-size: 16px;">
                                                                                <asp:CheckBox ID="chkInStockItem" Style="font-size: 16px;" runat="server" Text="Only show in-stock Items"
                                                                                    CssClass="checkboxall" AutoPostBack="True" />
                                                                                &nbsp;
                                                    <asp:CheckBox ID="chkvendor" Style="font-size: 16px;" runat="server"
                                                        CssClass="checkboxall" AutoPostBack="True" />&nbsp;
                                                    <asp:LinkButton ID="lnkbtnInvClearAdvanceSearchResults" Text="Clear Advanced Search Results" Font-Underline="false"
                                                        runat="server" Visible="false"></asp:LinkButton>
                                                                            </td>
                                                                            <td align="right" style="padding-right: 6px">
                                                                                <asp:LinkButton ID="lnkbtnPrev" runat="server" Style="font-size: 16px;" Text="Previous" Font-Bold="true" Font-Underline="false"></asp:LinkButton>
                                                                                &nbsp;&nbsp;|&nbsp;&nbsp;
                                                    <asp:LinkButton ID="lnkbtnNext" runat="server" Style="font-size: 16px;" Text="Next" Font-Bold="true" Font-Underline="false"></asp:LinkButton>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>&nbsp;
                                                                </td>
                                                                <td class="style2" style="width: 500px;" colspan="2" align="right">
                                                                    <table cellpadding="0" cellspacing="0" width="960px" border="0" style="padding-top: 5px;">
                                                                        <tr>
                                                                            <td width="100%" align="left" style="font-size: 16px;" class="labelall">
                                                                                <b>Hint:</b> Hitting the ESC key on your keyboard quickly closes this screen.
                                                                            </td>
                                                                            <td align="right">
                                                                                <asp:Button ID="imgbtnExitInventory" TabIndex="703" Width="80px" onblur="setfocus_invpopup();"
                                                                                    Style="cursor: pointer;" Height="28px" runat="server" Text="Exit" CssClass="alterrow"
                                                                                    Font-Bold="true" OnClientClick="return getskufocus();" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                                <td align="right" class="style4" width="5"></td>
                                                            </tr>

                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:View>
                                        <asp:View ID="ViewSalesHistory" runat="server">
                                            <ajax:UpdatePanel ID="upSalesHistory" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table width="100%" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td style="height: 5px"></td>
                                                            <td></td>
                                                        </tr>
                                                        <tr id="DateRange" runat="server">
                                                            <td align="left" valign="top" class="tab1">
                                                                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                    <tr>
                                                                        <td height="8" align="left" valign="top">
                                                                            <img src="images/crnr1w.gif" alt="" width="8" height="8" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="left" valign="middle">
                                                                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                                <tr>
                                                                                    <td width="2%"></td>
                                                                                    <td width="70px">&nbsp;<asp:Label ID="lbldate" runat="server" Text="Start Date: " CssClass="labelall"></asp:Label>
                                                                                    </td>
                                                                                    <td width="100px">
                                                                                        <asp:TextBox ID="txtSalesFrom" Enabled="false" runat="server" Width="133px" Style="position: relative;"></asp:TextBox>
                                                                                    </td>
                                                                                    <td width="70px">
                                                                                        <a id="chooseeday1" style="vertical-align: bottom;" onclick="showCalendar1('<% =txtSalesFrom.ClientID %>','<% =imgecalander.ClientID %>');showcustomdate('0');"
                                                                                            href="javascript:void(null)">
                                                                                            <img id="imgecalander" runat="server" border="0" align="top" alt="" src="Images/C1.jpg" />
                                                                                        </a>
                                                                                    </td>
                                                                                    <td width="70px">
                                                                                        <asp:Label ID="lblEnddate" runat="server" CssClass="labelall" Text="End Date:"></asp:Label>
                                                                                    </td>
                                                                                    <td width="100px">
                                                                                        <asp:TextBox ID="txtSalesTo" Enabled="false" runat="server" Width="133px" Style="position: relative;"></asp:TextBox>
                                                                                    </td>
                                                                                    <td width="70px">
                                                                                        <a id="A3" style="vertical-align: bottom;" onclick="showCalendar1('<% =txtSalesTo.ClientID %>','<% =img4.ClientID %>');showcustomdate('0');"
                                                                                            href="javascript:void(null)">
                                                                                            <img id="img4" runat="server" border="0" align="top" alt="" src="Images/C1.jpg" />
                                                                                        </a>
                                                                                    </td>
                                                                                    <td width="100px;">
                                                                                        <asp:Label ID="lblPoursSubItem" runat="server" CssClass="labelall" Text="Select Sub Items:" Visible="false"></asp:Label>
                                                                                    </td>
                                                                                    <td width="130px;">
                                                                                        <asp:DropDownList ID="ddlPoursSubItem" runat="server" CssClass="dropdownall" AutoPostBack="true" Visible="false">
                                                                                        </asp:DropDownList>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:Button ID="imgbtnsearch" runat="server" Text="Find" Style="cursor: pointer;"
                                                                                            Height="28px" Width="70px" Font-Bold="true" Font-Names="Arial" CausesValidation="false" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td height="8" align="left" valign="bottom" class="tab1c">
                                                                            <img src="images/crnr3w.gif" alt="" width="8" height="8" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <table width="100%">
                                                                    <tr id="Tr12" style="height: 230px;" runat="server">
                                                                        <td colspan="2" valign="top" style="height: 230px;">
                                                                            <table width="100%" cellpadding="0" cellspacing="0">
                                                                                <tr>
                                                                                    <td align="right">
                                                                                        <table>
                                                                                            <tr>
                                                                                                <td width="95px;">
                                                                                                    <asp:Label ID="lblPoursSubItem1" runat="server" CssClass="labelall" Text="Select Sub Items:" Visible="false"></asp:Label>
                                                                                                </td>
                                                                                                <td width="96px;">
                                                                                                    <asp:DropDownList ID="ddlPoursSubItem1" runat="server" CssClass="dropdownall" AutoPostBack="true" Visible="false">
                                                                                                    </asp:DropDownList>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:ImageButton ID="imgbtnReturn" runat="server" ImageUrl="~/Images/printer1.gif" />
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:CheckBox ID="chkReturnQty" Text="Return Only" runat="server" CssClass="checkboxall"
                                                                                                        AutoPostBack="True" Visible="true" />
                                                                                                    <asp:CheckBox ID="chkPromoSalesQty" runat="server" Text="Promotional Sales Only"
                                                                                                        CssClass="checkboxall" AutoPostBack="True" />
                                                                                                    <asp:CheckBox ID="chkShowinCase" runat="server" Text="Show In Cases"
                                                                                                        CssClass="checkboxall" AutoPostBack="True" Checked="false" />
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:RadioButtonList ID="rdoGraph" runat="server" CssClass="labelgreen" AutoPostBack="true"
                                                                                                        Font-Size="9pt" RepeatDirection="Horizontal">
                                                                                                        <asp:ListItem Selected="True" Value="1">Units Graph</asp:ListItem>
                                                                                                        <asp:ListItem Value="2">Revenue Graph</asp:ListItem>
                                                                                                        <asp:ListItem Value="3">Profitability Graph</asp:ListItem>
                                                                                                    </asp:RadioButtonList>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:RadioButtonList ID="rdoSalesWeeklyGraph" runat="server" CssClass="labelgreen" AutoPostBack="true"
                                                                                                        Font-Size="9pt" RepeatDirection="Horizontal" Font-Bold="true">
                                                                                                        <asp:ListItem Selected="True" Value="1">18 Months</asp:ListItem>
                                                                                                        <asp:ListItem Value="2">78 Weeks</asp:ListItem>
                                                                                                    </asp:RadioButtonList>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td align="left" colspan="2">
                                                                                        <asp:GridView ID="gvInventoryHistory" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                                                                            BorderColor="White" BorderWidth="1px" Width="100%" EmptyDataText="No matching records found."
                                                                                            AllowPaging="True">
                                                                                            <PagerStyle HorizontalAlign="Right" VerticalAlign="Middle" />
                                                                                            <SelectedRowStyle CssClass="selectrow" />
                                                                                            <HeaderStyle CssClass="header" HorizontalAlign="Left" VerticalAlign="Middle" />
                                                                                            <AlternatingRowStyle CssClass="alterrow" />
                                                                                            <RowStyle CssClass="row" />
                                                                                            <EmptyDataRowStyle ForeColor="Red" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                                            <EmptyDataTemplate>
                                                                                                <asp:Table ID="gvInventoryReceivetable" runat="server" Width="100%" CssClass="header"
                                                                                                    GridLines="Both" CellPadding="0" CellSpacing="0" BorderWidth="1">
                                                                                                    <asp:TableRow ID="gvInventoryReceiveheader" runat="server" HorizontalAlign="Left"
                                                                                                        Width="100%">
                                                                                                        <%--2--%>
                                                                                                        <asp:TableCell ID="gvInventoryReceivecol2" runat="server" Width="20%">
                                                                                                            &nbsp;<asp:Label ID="lblgvToservicescol2" runat="server" Text="Date - Time - Station"></asp:Label>
                                                                                                        </asp:TableCell>
                                                                                                        <%--3--%>
                                                                                                        <asp:TableCell ID="gvInventoryReceivecol3" runat="server" Width="12%">
                                                                                                            <asp:Label ID="lblgvToservicescol3" runat="server" Text="Invoice"></asp:Label>
                                                                                                            &nbsp;
                                                                                                        </asp:TableCell>
                                                                                                        <%--4--%>
                                                                                                        <asp:TableCell ID="gvInventoryReceivecol4" runat="server" Width="12%" HorizontalAlign="Right">
                                                                                                            &nbsp;<asp:Label ID="lblgvToservicescol4" runat="server" Text="Qty"></asp:Label>
                                                                                                        </asp:TableCell>
                                                                                                        <%--5--%>
                                                                                                        <asp:TableCell ID="gvInventoryReceivecol5" runat="server" Width="12%" HorizontalAlign="Right">
                                                                                                            &nbsp;<asp:Label ID="Label70" runat="server" Text="Price Ea."></asp:Label>
                                                                                                        </asp:TableCell>
                                                                                                        <%--6--%>
                                                                                                        <asp:TableCell ID="gvInventoryReceivecol6" runat="server" Width="12%" HorizontalAlign="Right">
                                                                                                            &nbsp;<asp:Label ID="Label12" runat="server" Text="Ext."></asp:Label>
                                                                                                        </asp:TableCell>
                                                                                                        <%--7--%>
                                                                                                        <asp:TableCell ID="gvInventoryReceivecol7" runat="server">
                                                                                                            <asp:Label ID="lblgvToservicescol6" runat="server" Text="Customer"></asp:Label>&nbsp;
                                                                                                        </asp:TableCell>
                                                                                                    </asp:TableRow>
                                                                                                </asp:Table>
                                                                                            </EmptyDataTemplate>
                                                                                            <Columns>
                                                                                                <asp:TemplateField HeaderText="Date - Time - Station">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblDate" runat="server" Text='<%# Bind("inv_date1","{0:MM/dd/yy hh:mm tt}")%> '></asp:Label>
                                                                                                        <asp:Label ID="lblStation" runat="server" Text='<%# Bind("station") %>'></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                    <HeaderTemplate>
                                                                                                        &nbsp;<asp:LinkButton ID="lnkDate" runat="server" CommandArgument="Date" CommandName="Sort"
                                                                                                            ForeColor="White">Date - Time - Station</asp:LinkButton>
                                                                                                    </HeaderTemplate>
                                                                                                    <HeaderStyle Width="20%" Wrap="false" HorizontalAlign="Left" CssClass="Margin1" />
                                                                                                    <ItemStyle Width="20%" HorizontalAlign="Left" CssClass="Margin1" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="Invoice">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblInvoice" runat="server" Text='<%# Bind("invoice_mst_id") %>'></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                    <HeaderTemplate>
                                                                                                        <asp:LinkButton ID="lnkInvoice" runat="server" CommandArgument="Invoice" CommandName="Sort"
                                                                                                            ForeColor="White">Invoice</asp:LinkButton>
                                                                                                    </HeaderTemplate>
                                                                                                    <HeaderStyle Width="12%" HorizontalAlign="right" CssClass="Margin1" />
                                                                                                    <ItemStyle Width="12%" HorizontalAlign="right" CssClass="Margin1" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="Qty" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblQty" runat="server" Text='<%# Bind("qty") %>'></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                    <HeaderTemplate>
                                                                                                        <asp:LinkButton ID="lnkQty" runat="server" CommandArgument="Qty" CommandName="Sort"
                                                                                                            ForeColor="White">Qty</asp:LinkButton>
                                                                                                    </HeaderTemplate>
                                                                                                    <HeaderStyle Width="12%" CssClass="Margin1" />
                                                                                                    <ItemStyle Width="12%" CssClass="Margin1" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="Price Ea." HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblPriceEa" runat="server" Text='<%# Bind("price","{0:###,###,##0.00}") %>'></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                    <HeaderTemplate>
                                                                                                        <asp:LinkButton ID="lnkPriceEa" runat="server" CommandArgument="Price Ea." CommandName="Sort"
                                                                                                            ForeColor="White">Price Ea.</asp:LinkButton>
                                                                                                    </HeaderTemplate>
                                                                                                    <HeaderStyle Width="12%" CssClass="Margin1" />
                                                                                                    <ItemStyle Width="12%" CssClass="Margin1" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="Ext." HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblPriceExt" runat="server" Text='<%# Bind("initprice","{0:###,###,##0.00}") %>'></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                    <HeaderTemplate>
                                                                                                        <asp:LinkButton ID="lnkPriceExt" runat="server" CommandArgument="Price Ext." CommandName="Sort"
                                                                                                            ForeColor="White">Ext.</asp:LinkButton>
                                                                                                    </HeaderTemplate>
                                                                                                    <HeaderStyle Width="12%" CssClass="Margin1" />
                                                                                                    <ItemStyle Width="12%" CssClass="Margin1" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="Customer">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblCustomer" runat="server" Text='<%# Bind("name") %>'></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                    <HeaderTemplate>
                                                                                                        <asp:LinkButton ID="lnkCustomer" runat="server" CommandArgument="Customer" CommandName="Sort"
                                                                                                            ForeColor="White">Customer</asp:LinkButton>
                                                                                                    </HeaderTemplate>
                                                                                                    <HeaderStyle CssClass="Margin1" HorizontalAlign="Left" />
                                                                                                    <ItemStyle CssClass="Margin1" HorizontalAlign="Left" />
                                                                                                </asp:TemplateField>
                                                                                            </Columns>
                                                                                        </asp:GridView>
                                                                                        <asp:Panel runat="server" Width="100%" Height="100%" ID="pnlSalesWeeklyGraph" Visible="false">
                                                                                            <ajax:UpdatePanel ID="upSalesWeeklyGraph" runat="server" UpdateMode="Conditional">
                                                                                                <ContentTemplate>
                                                                                                    <asp:MultiView ID="mulSalesWeeklyGraphHistory" runat="server">
                                                                                                        <asp:View ID="ViewSalesGraph" runat="server">
                                                                                                            <%-- <asp:Panel runat="server" Width="100%" Height="430px" ID="pnlgraph">--%>
                                                                                                            <iframe id="partssaleshistory" runat="server" frameborder="0" height="440px" marginheight="1"
                                                                                                                marginwidth="0" scrolling="auto" width="100%"></iframe>
                                                                                                            <%--  </asp:Panel>--%>
                                                                                                        </asp:View>
                                                                                                        <asp:View ID="ViewSalesWeeklyHistory" runat="server">
                                                                                                            <table style="width: 100%;">
                                                                                                                <tr>
                                                                                                                    <td style="width: 80%;">
                                                                                                                        <asp:DataList ID="dlWeeklySales" runat="server" RepeatColumns="8" RepeatDirection="Vertical" Visible="false" Width="100%" OnItemDataBound="dlWeeklySales_ItemDataBound">
                                                                                                                            <ItemStyle Width="5%" HorizontalAlign="Right" CssClass="Margin1" />
                                                                                                                            <HeaderTemplate>
                                                                                                                                <table>
                                                                                                                                    <tr>
                                                                                                                                        <td>&nbsp;
                                                                                                                                        </td>
                                                                                                                                    </tr>
                                                                                                                                    <tr>
                                                                                                                                        <td>
                                                                                                                                            <asp:Label ID="lblHeading" runat="server" Text="Item sold week beginning Monday :" Font-Bold="true" Font-Size="12pt"></asp:Label></td>
                                                                                                                                    </tr>
                                                                                                                                    <tr>
                                                                                                                                        <td>&nbsp;
                                                                                                                                        </td>
                                                                                                                                    </tr>
                                                                                                                                </table>

                                                                                                                                <br />
                                                                                                                            </HeaderTemplate>
                                                                                                                            <ItemTemplate>
                                                                                                                                <asp:Label ID="lblWeeklyDate" runat="server" Text='<%# Bind("dt", "{0:MM/dd/yy}")%>'></asp:Label>
                                                                                                                                <br />
                                                                                                                                <asp:Label ID="lblTotalSoldItem" runat="server" Text='<%#Bind("TotalSoldItem")%>'></asp:Label>
                                                                                                                            </ItemTemplate>

                                                                                                                        </asp:DataList>
                                                                                                                    </td>
                                                                                                                    <td style="width: 20%; vertical-align: top; padding-top: 65px;">
                                                                                                                        <table style="text-align: left; width: 100%; vertical-align: top;">
                                                                                                                            <tr>
                                                                                                                                <td style="width: 80%">
                                                                                                                                    <asp:Label ID="lblHeadinghighweek" runat="server" Text="High Week :" Font-Bold="true" ForeColor="Green" Font-Size="12pt"></asp:Label>
                                                                                                                                    <asp:HiddenField ID="hmaxvalue" runat="server" />
                                                                                                                                    <asp:HiddenField ID="hminvalue" runat="server" />
                                                                                                                                </td>
                                                                                                                                <td style="width: 20%">
                                                                                                                                    <asp:Label ID="lblhighWeek" runat="server" Text="" Font-Bold="true" ForeColor="Green" Font-Size="12pt"></asp:Label>

                                                                                                                                </td>
                                                                                                                            </tr>
                                                                                                                            <tr>
                                                                                                                                <td colspan="2">&nbsp;
                                                                                                                                </td>
                                                                                                                            </tr>
                                                                                                                            <tr>
                                                                                                                                <td>
                                                                                                                                    <asp:Label ID="Label20" runat="server" Text="Average Week :" Font-Bold="true" Font-Size="12pt"></asp:Label>
                                                                                                                                </td>
                                                                                                                                <td>
                                                                                                                                    <asp:Label ID="lblaverageWeek" runat="server" Text="" Font-Bold="true" Font-Size="12pt"></asp:Label>
                                                                                                                                </td>
                                                                                                                            </tr>
                                                                                                                            <tr>
                                                                                                                                <td colspan="2">&nbsp;
                                                                                                                                </td>
                                                                                                                            </tr>
                                                                                                                            <tr>
                                                                                                                                <td>
                                                                                                                                    <asp:Label ID="Label1" runat="server" Text="Low Week :" Font-Bold="true" ForeColor="Purple" Font-Size="12pt"></asp:Label>
                                                                                                                                </td>
                                                                                                                                <td>
                                                                                                                                    <asp:Label ID="lbllowweek" runat="server" Text="" Font-Bold="true" ForeColor="Purple" Font-Size="12pt"></asp:Label>
                                                                                                                                </td>
                                                                                                                            </tr>
                                                                                                                        </table>
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                            </table>

                                                                                                        </asp:View>
                                                                                                        <asp:View ID="ViewProfitability" runat="server">
                                                                                                            <asp:Panel ID="pnlProfitabilityGraph" runat="server" Visible="false">

                                                                                                                <table cellpadding="0" cellspacing="0" border="0" style="padding: 10px; width: 100%">
                                                                                                                    <tr>
                                                                                                                        <td align="left" style="padding-bottom: 5px; width: 44%; vertical-align: top;" colspan="2">
                                                                                                                            <table id="tdProfitability" cellpadding="0" cellspacing="0" border="0" width="100%"
                                                                                                                                runat="server">
                                                                                                                                <tr>
                                                                                                                                    <td align="left" valign="top" id="Td14"></td>
                                                                                                                                    <td colspan="2" align="left">
                                                                                                                                        <table>
                                                                                                                                            <tr>
                                                                                                                                                <td align="center" colspan="2">&nbsp;
                                                                                                                                                </td>
                                                                                                                                            </tr>
                                                                                                                                            <tr>
                                                                                                                                                <td align="left" colspan="2">
                                                                                                                                                    <span style="font-size: medium; font-weight: bold;">Profitability : </span>
                                                                                                                                                    <asp:DropDownList ID="ddlprofitability" runat="server" CssClass="dropdownall" Width="40%" AutoPostBack="true">
                                                                                                                                                    </asp:DropDownList>
                                                                                                                                                </td>
                                                                                                                                            </tr>
                                                                                                                                            <tr>
                                                                                                                                                <td align="center" colspan="2">&nbsp;
                                                                                                                                                </td>
                                                                                                                                            </tr>
                                                                                                                                            <tr>
                                                                                                                                                <td align="left" width="50%">
                                                                                                                                                    <span style="font-size: small">Total : </span>
                                                                                                                                                </td>
                                                                                                                                                <td align="right" width="50%">
                                                                                                                                                    <asp:Label ID="lblSubtotal" runat="server"></asp:Label>
                                                                                                                                                </td>
                                                                                                                                            </tr>
                                                                                                                                            <tr>
                                                                                                                                                <td align="left">
                                                                                                                                                    <span style="font-size: small">Cost of Goods sold : </span>
                                                                                                                                                </td>
                                                                                                                                                <td align="right">
                                                                                                                                                    <asp:Label ID="lblsales" runat="server"></asp:Label>
                                                                                                                                                </td>
                                                                                                                                            </tr>
                                                                                                                                            <tr>
                                                                                                                                                <td align="left">
                                                                                                                                                    <span style="font-size: small">Profit / Dollars : </span>
                                                                                                                                                </td>
                                                                                                                                                <td align="right">
                                                                                                                                                    <asp:Label ID="lblProfitD" runat="server"></asp:Label>
                                                                                                                                                </td>
                                                                                                                                            </tr>
                                                                                                                                            <tr>
                                                                                                                                                <td align="left">
                                                                                                                                                    <span style="font-size: small">Profit / Margin % : </span>&nbsp;
                                                                                                                                                </td>
                                                                                                                                                <td align="right">
                                                                                                                                                    <asp:Label ID="lblMargin" runat="server"></asp:Label>
                                                                                                                                                </td>
                                                                                                                                            </tr>
                                                                                                                                            <tr>
                                                                                                                                                <td align="left">
                                                                                                                                                    <span style="font-size: small">Profit / Mark up % :</span>
                                                                                                                                                </td>
                                                                                                                                                <td align="right">
                                                                                                                                                    <asp:Label ID="lblMarkUp" runat="server"></asp:Label>
                                                                                                                                                </td>
                                                                                                                                            </tr>
                                                                                                                                            <tr>
                                                                                                                                                <td id="tdpricecostprofit" runat="server" colspan="2">
                                                                                                                                                    <span style="background-color: #97F997">Price :
                                                                                                                        <asp:Label ID="lblprof_price" runat="server" Text="Price"></asp:Label>
                                                                                                                                                    </span>
                                                                                                                                                </td>
                                                                                                                                            </tr>
                                                                                                                                            <tr id="tdpricecostprofit1" runat="server" colspan="2">
                                                                                                                                                <td id="td1" runat="server">
                                                                                                                                                    <span style="background-color: #FFC8CC">Cost :
                                                                                                                        <asp:Label ID="lblCost" runat="server" Text="Cost"></asp:Label>
                                                                                                                                                    </span>
                                                                                                                                                </td>
                                                                                                                                            </tr>
                                                                                                                                            <tr id="tdpricecostprofit2" runat="server" colspan="2">
                                                                                                                                                <td id="td2" runat="server">
                                                                                                                                                    <span style="background-color: Yellow">Profit :
                                                                                                                            <asp:Label ID="lblprofit" runat="server" Text="prifit"></asp:Label>
                                                                                                                                                    </span>
                                                                                                                                                </td>
                                                                                                                                            </tr>

                                                                                                                                        </table>
                                                                                                                                    </td>
                                                                                                                                    <td align="left" width="15%" valign="middle" colspan="2"></td>
                                                                                                                                </tr>
                                                                                                                            </table>
                                                                                                                        </td>
                                                                                                                        <td colspan="2" align="center" width="1%"></td>
                                                                                                                        <td colspan="3" align="center" width="55%">
                                                                                                                            <table width="100%" align="center">
                                                                                                                                <tr width="100%">
                                                                                                                                    <td width="100%">
                                                                                                                                        <div id="div3" runat="server" style="height: 100%; width: 100%;">
                                                                                                                                            <asp:Chart ID="Chart1" runat="server" Style="height: 430px; width: 85%;">
                                                                                                                                                <Series>
                                                                                                                                                    <asp:Series Name="Series1">
                                                                                                                                                    </asp:Series>
                                                                                                                                                </Series>
                                                                                                                                                <ChartAreas>
                                                                                                                                                    <asp:ChartArea Name="ChartArea1">
                                                                                                                                                    </asp:ChartArea>
                                                                                                                                                </ChartAreas>
                                                                                                                                            </asp:Chart>
                                                                                                                                        </div>
                                                                                                                                    </td>

                                                                                                                                </tr>
                                                                                                                            </table>
                                                                                                                        </td>
                                                                                                                    </tr>

                                                                                                                </table>
                                                                                                            </asp:Panel>
                                                                                                        </asp:View>
                                                                                                    </asp:MultiView>
                                                                                                </ContentTemplate>
                                                                                            </ajax:UpdatePanel>
                                                                                        </asp:Panel>



                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        <table width="100%">
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <div style="bottom: 32px; right: 10px; position: absolute;">
                                                                                                        <asp:RadioButtonList ID="rdoMonthSummary" runat="server" AutoPostBack="True" Font-Bold="False"
                                                                                                            CssClass="labelgreen" RepeatDirection="Horizontal" Width="100%">
                                                                                                            <asp:ListItem Selected="True" Text="Current Month History" Value="0"></asp:ListItem>
                                                                                                            <asp:ListItem Text="Two Months History" Value="1"></asp:ListItem>
                                                                                                            <asp:ListItem Text="Three Months History" Value="2"></asp:ListItem>
                                                                                                            <asp:ListItem Text="Custom Date Range" Value="3"></asp:ListItem>
                                                                                                            <asp:ListItem Value="4"><font color="red">Show Graph</font></asp:ListItem>

                                                                                                        </asp:RadioButtonList>
                                                                                                </td>
                                                                                                <td align="right">
                                                                                                    <asp:ImageButton ID="imgbtnExit" runat="server" ImageUrl="~/icon small/exit.gif"
                                                                                                        Visible="false" />
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="2" align="center"></td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                            </ajax:UpdatePanel>
                                        </asp:View>
                                        <asp:View ID="ViewPurchaseHistory" runat="server">
                                            <ajax:UpdatePanel ID="uppurchasehistory" UpdateMode="Conditional" runat="server">
                                                <ContentTemplate>
                                                    <table width="100%" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td style="height: 5px"></td>
                                                            <td></td>
                                                        </tr>
                                                        <tr id="trpurchasehistory" runat="server">
                                                            <td align="left" valign="top" class="tab1">
                                                                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                    <tr>
                                                                        <td height="8" align="left" valign="top">
                                                                            <img src="images/crnr1w.gif" alt="" width="8" height="8" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="left" valign="middle">
                                                                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                                <tr>
                                                                                    <td width="2%"></td>
                                                                                    <td style="width: 70px;">&nbsp;<asp:Label ID="lblpurchasestdate" runat="server" Text="Start Date: " CssClass="labelall"></asp:Label>
                                                                                    </td>
                                                                                    <td style="width: 100px;">
                                                                                        <asp:TextBox ID="txtpurchasesfromdate" Enabled="false" runat="server" Width="100px"
                                                                                            Style="position: relative;"></asp:TextBox>
                                                                                    </td>
                                                                                    <td style="width: 70px;">
                                                                                        <a id="A4" style="vertical-align: bottom;" onclick="showCalendar1('<% =txtpurchasesfromdate.ClientID %>','<% =img1.ClientID %>');showcustomdate('1');"
                                                                                            href="javascript:void(null)">
                                                                                            <img id="img1" runat="server" border="0" align="top" alt="" src="Images/C1.jpg" />
                                                                                        </a>
                                                                                    </td>
                                                                                    <td style="width: 70px;">
                                                                                        <asp:Label ID="lblpurchaseenddate" runat="server" CssClass="labelall" Text="End Date:"></asp:Label>
                                                                                    </td>
                                                                                    <td style="width: 100px;">
                                                                                        <asp:TextBox ID="txtpurchasestodate" Enabled="false" runat="server" Width="100px"
                                                                                            Style="position: relative;"></asp:TextBox>
                                                                                    </td>
                                                                                    <td style="width: 50px;">
                                                                                        <a id="A5" style="vertical-align: bottom;" onclick="showCalendar1('<% =txtpurchasestodate.ClientID %>','<% =img2.ClientID %>');showcustomdate('1');"
                                                                                            href="javascript:void(null)">
                                                                                            <img id="img2" runat="server" border="0" align="top" alt="" src="Images/C1.jpg" />
                                                                                        </a>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:Button ID="imgbtnpurchasefind" runat="server" Text="Find" Style="cursor: pointer;"
                                                                                            Height="28px" Width="70px" Font-Bold="true" Font-Names="Arial" CausesValidation="false" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td height="8" align="left" valign="bottom" class="tab1c">
                                                                            <img src="images/crnr3w.gif" alt="" width="8" height="8" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <table width="100%">
                                                                    <tr>
                                                                        <td valign="top" style="width: 100%;">
                                                                            <div id="divpurchasedata" runat="server" style="height: 380px; width: 100%; overflow: auto;"
                                                                                visible="false">
                                                                                <asp:GridView ID="gvInventoryPurchaseHistory" runat="server" AllowSorting="True" PageSize="10"
                                                                                    AutoGenerateColumns="False" BorderColor="White" BorderWidth="1px" Width="100%"
                                                                                    EmptyDataText="No matching records found." AllowPaging="True">
                                                                                    <PagerStyle HorizontalAlign="Right" VerticalAlign="Middle" />
                                                                                    <SelectedRowStyle CssClass="selectrow" />
                                                                                    <HeaderStyle CssClass="header" HorizontalAlign="Left" VerticalAlign="Middle" />
                                                                                    <AlternatingRowStyle CssClass="alterrow" />
                                                                                    <RowStyle CssClass="row" />
                                                                                    <EmptyDataRowStyle ForeColor="Red" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                                    <EmptyDataTemplate>
                                                                                        <asp:Table ID="gvInventoryReceivetable" runat="server" Width="100%" CssClass="header"
                                                                                            GridLines="Both" CellPadding="0" CellSpacing="0" BorderWidth="1">
                                                                                            <asp:TableRow ID="gvInventoryReceiveheader" runat="server" HorizontalAlign="Left"
                                                                                                Width="100%">
                                                                                                <%--2--%>
                                                                                                <asp:TableCell ID="gvPurchasecol0" runat="server" Width="15%">
                                                                                                    &nbsp;<asp:Label ID="lblgvToservicescol2" runat="server" Text="Date - Time - Invoice #"></asp:Label>
                                                                                                </asp:TableCell>
                                                                                                <asp:TableCell ID="TableCell1" runat="server" Width="9%" HorizontalAlign="Right">
                                                                                                    &nbsp;<asp:Label ID="Label63" runat="server" Text="Starting QOH"></asp:Label>
                                                                                                </asp:TableCell>
                                                                                                <%--3--%>
                                                                                                <asp:TableCell ID="gvPurchasecol1" runat="server" Width="9%" HorizontalAlign="Right">
                                                                                                    &nbsp;<asp:Label ID="lblgvToservicescol3" runat="server" Text="Qty Per Case"></asp:Label>
                                                                                                </asp:TableCell>
                                                                                                <%--4--%>
                                                                                                <asp:TableCell ID="gvPurchasecol2" runat="server" Width="9%" HorizontalAlign="Right">
                                                                                                    &nbsp;<asp:Label ID="lblgvToservicescol4" runat="server" Text="Total Cases"></asp:Label>
                                                                                                </asp:TableCell>
                                                                                                <%--5--%>
                                                                                                <asp:TableCell ID="gvPurchasecol3" runat="server" Width="5%" HorizontalAlign="Right">
                                                                                                    &nbsp;<asp:Label ID="Label70" runat="server" Text="Plus"></asp:Label>
                                                                                                </asp:TableCell>
                                                                                                <%--6--%>
                                                                                                <asp:TableCell ID="gvPurchasecol4" runat="server" Width="5%" HorizontalAlign="Right">
                                                                                                    &nbsp;<asp:Label ID="Label12" runat="server" Text="Qty"></asp:Label>
                                                                                                </asp:TableCell>
                                                                                                <%--7--%>
                                                                                                <asp:TableCell ID="gvPurchasecol5" runat="server" Width="7%" HorizontalAlign="center">
                                                                                                    &nbsp;<asp:Label ID="lblgvToservicescol6" runat="server" Text="Cost Each"></asp:Label>
                                                                                                </asp:TableCell>
                                                                                                <asp:TableCell ID="gvPurchasecol6" runat="server" Width="7%" HorizontalAlign="center">
                                                                                                    &nbsp;<asp:Label ID="Label2" runat="server" Text="Case Cost"></asp:Label>
                                                                                                </asp:TableCell>
                                                                                                <asp:TableCell ID="gvPurchasecol7" runat="server" Width="7%" HorizontalAlign="Right">
                                                                                                    &nbsp;<asp:Label ID="Label45" runat="server" Text="Extension"></asp:Label>
                                                                                                </asp:TableCell>
                                                                                                <asp:TableCell ID="gvPurchasecol8" runat="server" Width="8%" HorizontalAlign="Right">
                                                                                                    &nbsp;<asp:Label ID="Label46" runat="server" Text="Percent +/-"></asp:Label>
                                                                                                </asp:TableCell>
                                                                                                <asp:TableCell ID="gvPurchasecol9" runat="server" Width="15%">
                                                                                                    &nbsp;<asp:Label ID="Label47" runat="server" Text="Vendor"></asp:Label>
                                                                                                </asp:TableCell>
                                                                                            </asp:TableRow>
                                                                                        </asp:Table>
                                                                                    </EmptyDataTemplate>
                                                                                    <Columns>
                                                                                        <asp:TemplateField HeaderText="Date - Time - Invoice #" HeaderStyle-HorizontalAlign="Left"
                                                                                            ItemStyle-HorizontalAlign="Left">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblDate" runat="server" Text='<%# Bind("time","{0:hh:mmtt}")%> '></asp:Label>
                                                                                                <asp:Label ID="lblVenderInvoiceNo" runat="server" Text='<%# Bind("Vender_InvNo")%> '></asp:Label>
                                                                                                <asp:Label ID="lblStation" runat="server" Text='<%# Bind("station_no") %>' Visible="false"></asp:Label>
                                                                                            </ItemTemplate>
                                                                                            <HeaderTemplate>
                                                                                                <asp:LinkButton ID="lnkDate" runat="server" CommandArgument="Date" CommandName="Sort"
                                                                                                    ForeColor="White">Date - Time - Invoice #</asp:LinkButton>
                                                                                            </HeaderTemplate>
                                                                                            <HeaderStyle Width="15%" Wrap="false" HorizontalAlign="Left" CssClass="Margin1" />
                                                                                            <ItemStyle Width="15%" HorizontalAlign="Left" CssClass="Margin1" />
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Starting" HeaderStyle-HorizontalAlign="Right"
                                                                                            ItemStyle-HorizontalAlign="Right">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblstarting" runat="server" Text='<%# Bind("SQOH")%>'></asp:Label>
                                                                                            </ItemTemplate>
                                                                                            <HeaderTemplate>
                                                                                                <asp:LinkButton ID="lnkStarting" runat="server" CommandArgument="SQOH" CommandName="Sort"
                                                                                                    ForeColor="White">Starting QOH</asp:LinkButton>
                                                                                            </HeaderTemplate>
                                                                                            <HeaderStyle HorizontalAlign="right" Width="5%" CssClass="Margin1" />
                                                                                            <ItemStyle Width="5%" CssClass="Margin1" />
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Qty Per Case" HeaderStyle-HorizontalAlign="Right"
                                                                                            ItemStyle-HorizontalAlign="Right">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblqtypercase" runat="server" Text='<%# Bind("qtypercase") %>'></asp:Label>
                                                                                            </ItemTemplate>
                                                                                            <HeaderTemplate>
                                                                                                <asp:LinkButton ID="lnkDescription" runat="server" CommandArgument="qtypercase" CommandName="Sort"
                                                                                                    ForeColor="White">Qty Per Case</asp:LinkButton>
                                                                                            </HeaderTemplate>
                                                                                            <HeaderStyle HorizontalAlign="right" Width="5%" CssClass="Margin1" />
                                                                                            <ItemStyle Width="5%" CssClass="Margin1" />
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Total Cases" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lbltotalcase" runat="server" Text='<%#Bind("caseqty") %>'></asp:Label>
                                                                                            </ItemTemplate>
                                                                                            <HeaderTemplate>
                                                                                                <asp:LinkButton ID="lnktotalcase" runat="server" CommandArgument="caseqty" CommandName="Sort"
                                                                                                    ForeColor="White">Total Cases</asp:LinkButton>
                                                                                            </HeaderTemplate>
                                                                                            <HeaderStyle Width="5%" CssClass="Margin1" />
                                                                                            <ItemStyle Width="5%" CssClass="Margin1" />
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Plus" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblPlus" runat="server" Text='<%# Bind("extra") %>'></asp:Label>
                                                                                            </ItemTemplate>
                                                                                            <HeaderTemplate>
                                                                                                <asp:LinkButton ID="lnkPlus" runat="server" CommandArgument="extra" CommandName="Sort"
                                                                                                    ForeColor="White">Plus</asp:LinkButton>
                                                                                            </HeaderTemplate>
                                                                                            <HeaderStyle Width="5%" CssClass="Margin1" />
                                                                                            <ItemStyle Width="5%" CssClass="Margin1" />
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Qty">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblQty" runat="server" Text='<%# Bind("qty") %>'></asp:Label>
                                                                                            </ItemTemplate>
                                                                                            <HeaderStyle HorizontalAlign="right" Width="5%" CssClass="Margin1" />
                                                                                            <ItemStyle HorizontalAlign="right" Width="5%" CssClass="Margin1" />
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Cost Each">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblCostEa" runat="server" Text='<%#FormatNumber(IIf(IsDBNull(Eval("cost")), "0.00", Eval("cost")), 2, TriState.True)%>'></asp:Label>
                                                                                            </ItemTemplate>
                                                                                            <HeaderStyle HorizontalAlign="right" Width="9%" CssClass="Margin1" />
                                                                                            <ItemStyle HorizontalAlign="right" Width="9%" CssClass="Margin1" />
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Case Cost">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblCaseCost1" runat="server" Text='<%#FormatNumber(IIf(IsDBNull(Eval("casecost")), "0.00", Eval("casecost")), 2, TriState.True)%>'></asp:Label>
                                                                                            </ItemTemplate>
                                                                                            <HeaderStyle HorizontalAlign="right" Width="9%" CssClass="Margin1" />
                                                                                            <ItemStyle HorizontalAlign="right" Width="9%" CssClass="Margin1" />
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Extension">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblextension" runat="server" Text='<%#FormatNumber(IIf(IsDBNull(Eval("extention")), "0.00", Eval("extention")), 2, TriState.True)%>'></asp:Label>
                                                                                            </ItemTemplate>
                                                                                            <HeaderStyle HorizontalAlign="right" Width="7%" CssClass="Margin1" />
                                                                                            <ItemStyle HorizontalAlign="right" Width="7%" CssClass="Margin1" />
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Percent +/-">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblpercent" runat="server" Text='<%# Bind("percentChg") %>'></asp:Label>
                                                                                            </ItemTemplate>
                                                                                            <HeaderStyle HorizontalAlign="right" Width="7%" CssClass="Margin1" />
                                                                                            <ItemStyle HorizontalAlign="right" Width="7%" CssClass="Margin1" />
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Price" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblPrice" runat="server" Text='<%#FormatNumber(IIf(IsDBNull(Eval("price")), "0.00", Eval("price")), 2, TriState.True)%>'></asp:Label>
                                                                                            </ItemTemplate>
                                                                                            <HeaderTemplate>
                                                                                                <asp:LinkButton ID="lnkprice" runat="server" CommandArgument="price" CommandName="Sort"
                                                                                                    ForeColor="White">Price</asp:LinkButton>
                                                                                            </HeaderTemplate>
                                                                                            <HeaderStyle Width="7%" CssClass="Margin1" />
                                                                                            <ItemStyle Width="7%" CssClass="Margin1" />
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Vendor" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblVendor" runat="server" Text='<%# Bind("vendor") %>'></asp:Label>
                                                                                            </ItemTemplate>
                                                                                            <HeaderTemplate>
                                                                                                <asp:LinkButton ID="lnkvendor" runat="server" CommandArgument="vendor" CommandName="Sort"
                                                                                                    ForeColor="White">Vendor</asp:LinkButton>
                                                                                            </HeaderTemplate>
                                                                                            <HeaderStyle Width="24%" CssClass="Margin1" />
                                                                                            <ItemStyle Width="24%" CssClass="Margin1" />
                                                                                        </asp:TemplateField>
                                                                                    </Columns>
                                                                                </asp:GridView>
                                                                            </div>
                                                                            <%--<div id="dvPurchaseGraph" runat="server" style="height: 430px; width: 100%;" visible="false">--%>
                                                                            <iframe id="ifrmPurchaseHistory" runat="server" frameborder="0" height="450px" marginheight="1"
                                                                                marginwidth="0" scrolling="auto" width="100%" style="flex-item-align: start;"></iframe>
                                                                            <%--</div>--%>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td valign="bottom">
                                                                            <div>
                                                                                <table width="90%">
                                                                                    <tr>
                                                                                        <td align="left" width="50%">
                                                                                            <asp:RadioButtonList ID="rdoPGraph" runat="server" CssClass="labelgreen" AutoPostBack="true"
                                                                                                Font-Size="9pt" RepeatDirection="Horizontal">
                                                                                                <asp:ListItem Selected="True" Value="1">Show Units Graph</asp:ListItem>
                                                                                                <asp:ListItem Value="2">Show Revenue Graph</asp:ListItem>
                                                                                            </asp:RadioButtonList>
                                                                                        </td>
                                                                                        <td align="left" width="10%"></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td align="left" width="90%">
                                                                                            <div style="bottom: 32px; right: 20px; position: absolute;">
                                                                                                <asp:RadioButtonList ID="rdopurchasesummary" runat="server" AutoPostBack="True" Font-Bold="False"
                                                                                                    CssClass="labelgreen" RepeatDirection="Horizontal" Width="100%">
                                                                                                    <asp:ListItem Selected="True" Text="Current Month History" Value="0"></asp:ListItem>
                                                                                                    <asp:ListItem Text="Two Months History" Value="1"></asp:ListItem>
                                                                                                    <asp:ListItem Text="Three Months History" Value="2"></asp:ListItem>
                                                                                                    <asp:ListItem Text="One Year History" Value="5"></asp:ListItem>
                                                                                                    <asp:ListItem Text="Custom Date Range" Value="3"></asp:ListItem>
                                                                                                    <asp:ListItem Text="Show Graph" Value="4"></asp:ListItem>
                                                                                                </asp:RadioButtonList>
                                                                                            </div>
                                                                                        </td>
                                                                                        <td align="right">
                                                                                            <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/icon small/exit.gif"
                                                                                                Visible="false" />
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="center"></td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <asp:HiddenField ID="hdnotxtdate" runat="server" />
                                                </ContentTemplate>
                                            </ajax:UpdatePanel>
                                        </asp:View>
                                    </asp:MultiView>
                                </td>
                            </tr>

                        </table>
                    </div>
                </ContentTemplate>
            </ajax:UpdatePanel>
        </div>
        <ajaxToolkit:ModalPopupExtender ID="mpeInvAdvanceSearch" DropShadow="false" BehaviorID="mpeInvAdvanceSearch"
            RepositionMode="None" runat="server"
            BackgroundCssClass="modalBackground" PopupControlID="pnlInvAdvanceSearch" TargetControlID="btnInvAdvanceSearch">
        </ajaxToolkit:ModalPopupExtender>
        <asp:Button ID="btnInvAdvanceSearch" runat="server" Style="display: none" />
        <asp:Panel ID="pnlInvAdvanceSearch" runat="server" Width="75%" Style="display: none">
            <ajax:UpdatePanel ID="UpdatePanelInvAdvanceSearch" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <table cellpadding="0" cellspacing="0" border="0" align="center" width="100%" style="background-color: White;">
                        <tr>
                            <td height="27" colspan="3" background="images/popup_01.gif">
                                <asp:HiddenField ID="hdnendfocus" runat="server" />
                                <table cellpadding="0" cellspacing="0" width="100%">
                                    <tr>
                                        <td width="10"></td>
                                        <td width="293">
                                            <asp:Label ID="Label72" CssClass="stylePopupHeader" runat="server" Text="Inventory Advanced Search"></asp:Label>
                                        </td>
                                        <td align="right">
                                            <asp:ImageButton ID="ImageButton12" OnClientClick="$find('mpeInvAdvanceSearch').hide();"
                                                runat="server" ImageUrl="~/Images/close1.gif" />
                                        </td>
                                        <td width="2%"></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td width="5px"></td>
                            <td>
                                <table cellpadding="0" cellspacing="0" width="100%">
                                    <tr>
                                        <td style="height: 5px"></td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="top" class="tab1">
                                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                <tr>
                                                    <td height="8" align="left" valign="top">
                                                        <img src="images/crnr1w.gif" alt="" width="8" height="8" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td height="10" align="left" valign="top">
                                                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                            <tr>
                                                                <td width="5%"></td>
                                                                <td width="15%">
                                                                    <asp:Label ID="Label76" runat="server" Text="SKU :" CssClass="labelall"></asp:Label>
                                                                </td>
                                                                <td width="45%">
                                                                    <asp:TextBox ID="txtSKUSearch" runat="server" CssClass="textboxall" Width="53%" MaxLength="16"
                                                                        TabIndex="1"> </asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:RadioButton ID="rbtnSKUBegins" runat="server" CssClass="radioall" GroupName="SKUSearch"
                                                                        Text="Begins" TabIndex="2" />
                                                                    &nbsp;
                                                                <asp:RadioButton ID="rbtnSKUContains" runat="server" CssClass="radioall" GroupName="SKUSearch"
                                                                    Checked="true" Text="Contains" TabIndex="3" />
                                                                </td>
                                                                <td width="5%"></td>
                                                            </tr>
                                                            <tr style="height: 5px;">
                                                                <td colspan="5"></td>
                                                            </tr>
                                                            <tr>
                                                                <td width="5%"></td>
                                                                <td width="10%">
                                                                    <asp:Label ID="Label77" runat="server" Text="Description :" CssClass="labelall"></asp:Label>
                                                                </td>
                                                                <td width="30%">
                                                                    <asp:TextBox ID="txtDescSearch" runat="server" CssClass="textboxall" Width="94%"
                                                                        MaxLength="40" TabIndex="4"> </asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:RadioButton ID="rbtnDescBegins" runat="server" CssClass="radioall" GroupName="DescSearch"
                                                                        Text="Begins" TabIndex="5" />
                                                                    &nbsp;
                                                                <asp:RadioButton ID="rbtnDescContanis" runat="server" CssClass="radioall" GroupName="DescSearch"
                                                                    Checked="true" Text="Contains" TabIndex="6" />
                                                                </td>
                                                                <td width="5%"></td>
                                                            </tr>
                                                            <tr style="height: 5px;">
                                                                <td colspan="5"></td>
                                                            </tr>
                                                            <tr>
                                                                <td width="5%"></td>
                                                                <td width="10%">
                                                                    <asp:Label ID="Label44" runat="server" Text="Department :" CssClass="labelall"></asp:Label>
                                                                </td>
                                                                <td width="30%" colspan="2">

                                                                    <asp:Label ID="txtDeptSearch" AutoPostBack="true" runat="server" CssClass="labelall" Width="48%"></asp:Label>
                                                                    <asp:Button ID="btnSelDeptSearch" runat="server" Text="..." TabIndex="7" OnClientClick="return fnOpenDeptSearch();"
                                                                        AutoPostBack="true" Width="6%" Style="cursor: pointer" />
                                                                    <asp:HiddenField ID="hdnDeptSearch" runat="server" />
                                                                    <asp:HiddenField ID="hdnDeptSearchText" runat="server" />
                                                                </td>
                                                                <%-- <td>
                                                            </td>--%>
                                                                <td width="5%"></td>
                                                            </tr>
                                                            <tr style="height: 5px;">
                                                                <td colspan="5"></td>
                                                            </tr>
                                                            <tr id="trstylepopup" runat="server">
                                                                <td width="5%"></td>
                                                                <td width="10%">
                                                                    <asp:Label ID="Label78" runat="server" Text="Style :" CssClass="labelall"></asp:Label>
                                                                </td>
                                                                <td width="30%" colspan="2">
                                                                    <%-- <asp:DropDownList ID="ddlStyleSearch" runat="server" CssClass="dropdownall" Width="55%"
                                                                    TabIndex="7" Height="22px">
                                                                </asp:DropDownList>
                                                                <asp:Label ID="lblNoStyleSearch" runat="server" Text="No Style Found" CssClass="labelall"
                                                                    Visible="false"></asp:Label>--%>
                                                                    <asp:Label ID="txtStyleSearch" runat="server" CssClass="labelall" Width="48%"></asp:Label>
                                                                    <asp:Button ID="btnSelStyleSearch" runat="server" TabIndex="7" Text="..." OnClientClick="return fnOpenStyleSearch();"
                                                                        AutoPostBack="true" Width="6%" Style="cursor: pointer" />
                                                                    <asp:HiddenField ID="hdnStyleSearch" runat="server" />
                                                                    <asp:Label ID="lblNoStyleSearch" runat="server" Text="No Style Found" CssClass="labelall"
                                                                        Visible="false"></asp:Label>
                                                                    <asp:HiddenField ID="hdnStyleSearchText" runat="server" />
                                                                </td>

                                                                <td width="5%"></td>
                                                            </tr>
                                                            <tr style="height: 5px;">
                                                                <td colspan="5"></td>
                                                            </tr>
                                                            <tr id="trsizepopup" runat="server">
                                                                <td width="5%"></td>
                                                                <td width="10%">
                                                                    <asp:Label ID="Label79" runat="server" Text="Size :" CssClass="labelall"></asp:Label>
                                                                </td>
                                                                <td width="30%" colspan="2">
                                                                    <%-- <asp:DropDownList ID="ddlSizeSearch" runat="server" CssClass="dropdownall" Width="55%"
                                                                    TabIndex="8" Height="22px">
                                                                </asp:DropDownList>
                                                                <asp:Label ID="lblNoSizeSearch" runat="server" Text="No Size Found" CssClass="labelall"
                                                                    Visible="false"></asp:Label>--%>
                                                                    <asp:Label ID="txtSizeSearch" runat="server" CssClass="labelall" Width="48%"></asp:Label>
                                                                    <asp:Button ID="btnSelSizeSearch" runat="server" TabIndex="9" Text="..." OnClientClick="return fnOpenSizeSearch();"
                                                                        AutoPostBack="true" Width="6%" Style="cursor: pointer" />
                                                                    <asp:Button ID="btnadvsearch" runat="server" Style="display: none" />
                                                                    <asp:HiddenField ID="hdnSizeSearch" runat="server" />
                                                                    <asp:Label ID="lblNoSizeSearch" runat="server" Text="No Size Found" CssClass="labelall"
                                                                        Visible="false"></asp:Label>
                                                                    <asp:HiddenField ID="hdnSizeSearchText" runat="server" />
                                                                </td>

                                                                <td width="5%"></td>
                                                            </tr>
                                                            <tr style="height: 5px;">
                                                                <td colspan="5"></td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="5">
                                                                    <table cellpadding="0" cellspacing="0" width="100%">
                                                                        <tr>
                                                                            <td width="5%"></td>
                                                                            <td width="15%">
                                                                                <asp:Label ID="Label80" runat="server" Text="Qty on Hand :" CssClass="labelall"></asp:Label>
                                                                            </td>
                                                                            <td width="45%">
                                                                                <asp:TextBox ID="txtQtyOnHand" onfocus="this.select();" runat="server"
                                                                                    CssClass="textboxall" Width="20%" MaxLength="5"
                                                                                    TabIndex="9"> </asp:TextBox>
                                                                                <ajaxToolkit:TextBoxWatermarkExtender ID="Watertxt1" runat="server" TargetControlID="txtQtyOnHand" WatermarkText="--All--"></ajaxToolkit:TextBoxWatermarkExtender>
                                                                                <ajaxToolkit:FilteredTextBoxExtender ID="filtertxt1" runat="server" TargetControlID="txtQtyOnHand" ValidChars="0123456789"></ajaxToolkit:FilteredTextBoxExtender>
                                                                            </td>
                                                                            <td>
                                                                                <asp:RadioButtonList ID="rdoqtyonhold" runat="server" CssClass="radioall" RepeatDirection="Horizontal"
                                                                                    TabIndex="10" Enabled="false">
                                                                                    <asp:ListItem>Exact</asp:ListItem>
                                                                                    <asp:ListItem>Greater</asp:ListItem>
                                                                                    <asp:ListItem>Less</asp:ListItem>
                                                                                </asp:RadioButtonList>
                                                                            </td>
                                                                            <td width="5%"></td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr style="height: 5px;">
                                                                <td colspan="5"></td>
                                                            </tr>
                                                            <tr>
                                                                <td width="5%"></td>
                                                                <td width="10%">
                                                                    <asp:Label ID="lblvendorsearch" runat="server" Text="Vendor :" CssClass="labelall"></asp:Label>
                                                                </td>
                                                                <td width="30%" colspan="2">
                                                                    <%-- <asp:DropDownList ID="ddlvendorsearch" runat="server" CssClass="dropdownall" Width="55%"
                                                                    TabIndex="11" Height="22px">
                                                                </asp:DropDownList>
                                                                <asp:Label ID="lblnovendor" runat="server" Text="No Vendor Found" CssClass="labelall"
                                                                    Visible="false"></asp:Label>--%>
                                                                    <asp:Label ID="txtVendorSearch" runat="server" CssClass="labelall" Width="48%"></asp:Label>
                                                                    <asp:Button ID="btnSelVendorSearch" runat="server" TabIndex="10" Text="..." OnClientClick="return fnOpenVendorSearch();"
                                                                        AutoPostBack="true" Width="6%" Style="cursor: pointer" />
                                                                    <asp:HiddenField ID="hdnVendorSearch" runat="server" />
                                                                    <asp:HiddenField ID="hdnVendorSearchText" runat="server" />
                                                                </td>

                                                                <td width="5%"></td>
                                                            </tr>
                                                            <tr style="height: 5px;">
                                                                <td colspan="5"></td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="5">
                                                                    <table cellpadding="0" cellspacing="0" width="100%">
                                                                        <tr>
                                                                            <td width="5%"></td>
                                                                            <td width="15%">
                                                                                <asp:Label ID="Label81" runat="server" Text="Price :" CssClass="labelall"></asp:Label>
                                                                            </td>
                                                                            <td width="45%">
                                                                                <asp:TextBox ID="txtPriceSearch" runat="server" CssClass="textboxall" onblur="onBlurDecimalWatermark(this.id);"
                                                                                    Width="53%" TabIndex="12"></asp:TextBox>
                                                                                <ajaxToolkit:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" TargetControlID="txtPriceSearch" WatermarkText="--All--"></ajaxToolkit:TextBoxWatermarkExtender>
                                                                                <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtPriceSearch" ValidChars="0123456789.-"></ajaxToolkit:FilteredTextBoxExtender>


                                                                            </td>
                                                                            <td>
                                                                                <asp:RadioButtonList ID="rdoPrice" runat="server" Enabled="false" CssClass="radioall"
                                                                                    RepeatDirection="Horizontal" TabIndex="13">
                                                                                    <asp:ListItem>Exact</asp:ListItem>
                                                                                    <asp:ListItem>Greater</asp:ListItem>
                                                                                    <asp:ListItem>Less</asp:ListItem>
                                                                                </asp:RadioButtonList>
                                                                            </td>
                                                                            <td width="5%"></td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr style="height: 5px;">
                                                                <td colspan="5"></td>
                                                            </tr>
                                                            <tr>
                                                                <td width="5%"></td>
                                                                <td width="10%">
                                                                    <asp:Label ID="Label82" runat="server" Text="Discountable :" CssClass="labelall"></asp:Label>
                                                                </td>
                                                                <td width="30%">
                                                                    <asp:DropDownList ID="ddlDiscountableSearch" runat="server" CssClass="dropdownall"
                                                                        Width="55%" TabIndex="14" Height="22px">
                                                                        <asp:ListItem Value="All">--All--</asp:ListItem>
                                                                        <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                                                        <asp:ListItem Value="No">No</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td></td>
                                                                <td width="5%"></td>
                                                            </tr>
                                                            <tr style="height: 5px;">
                                                                <td colspan="5"></td>
                                                            </tr>
                                                            <tr id="trPointssearch" runat="server">
                                                                <td width="5%"></td>
                                                                <td width="10%">
                                                                    <asp:Label ID="Label83" runat="server" Text="Points :" CssClass="labelall"></asp:Label>
                                                                </td>
                                                                <td width="30%">
                                                                    <asp:TextBox ID="txtPointSearch" runat="server" CssClass="textboxall" Width="20%"
                                                                        MaxLength="2" onkeypress="return fncInputNumericValuesOnly(event)" TabIndex="15"> </asp:TextBox>
                                                                </td>
                                                                <td></td>
                                                                <td width="5%"></td>
                                                            </tr>
                                                            <tr style="height: 5px;">
                                                                <td colspan="5"></td>
                                                            </tr>
                                                            <tr>
                                                                <td align="right" colspan="5">
                                                                    <asp:Button ID="imgbtnInvback" runat="server" CausesValidation="false" Width="70px"
                                                                        Font-Bold="True" Font-Names="Arial" Font-Size="12px" Height="25px" Style="cursor: pointer;"
                                                                        TabIndex="16" Text="Back" />
                                                                    <asp:Button ID="imgInvbtnResetForm" runat="server" CausesValidation="false" Font-Bold="True"
                                                                        Font-Names="Arial" Font-Size="12px" Height="25px" Style="cursor: pointer;"
                                                                        TabIndex="17" Text="Reset Form" />
                                                                    <asp:Button ID="imgInvFind" runat="server" CausesValidation="false" Width="70px"
                                                                        Font-Bold="True" Font-Names="Arial" Font-Size="12px" Height="25px" Style="cursor: pointer;"
                                                                        TabIndex="18" Text="Find" />&nbsp;&nbsp;
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td height="8" align="left" valign="bottom" class="tab1c">
                                                        <img src="images/crnr3w.gif" alt="" width="8" height="8" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td height="5px"></td>
                                    </tr>
                                </table>
                            </td>
                            <td width="5px"></td>
                        </tr>
                    </table>
                </ContentTemplate>
            </ajax:UpdatePanel>
        </asp:Panel>

        <%---------------------------------- Modal Popup For Set Price --------------------------------------%>
        <ajaxToolkit:ModalPopupExtender ID="mpeSetPrice" BehaviorID="mpeSetPrice" RepositionMode="None"
            runat="server" BackgroundCssClass="modalBackground" TargetControlID="btnSetPrice"
            PopupControlID="pnlSetPrice" DropShadow="false" />
        <asp:Button ID="btnSetPrice" Style="display: none;" runat="server" />
        <asp:Panel ID="pnlSetPrice" CssClass="modalPopup" runat="server" Width="500px" Style="display: none;">
            <ajax:UpdatePanel ID="upnlSetPrice" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <table width="500" border="0" align="center" cellpadding="0" cellspacing="0">
                        <tr>
                            <td width="500" height="27" background="images/popup_01.gif" colspan="2">
                                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td width="10">&nbsp;
                                        </td>
                                        <td width="350">
                                            <asp:Label ID="lblItemNameSize" CssClass="stylePopupHeader" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td align="right">
                                            <asp:ImageButton ID="btnCloseSetPrice" OnClientClick="$find('mpeSetPrice').hide();"
                                                runat="server" ImageUrl="~/Images/close1.gif" />
                                        </td>
                                        <td width="2%"></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td width="500" align="left" valign="top" colspan="2">
                                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td style="height: 4px;"></td>
                                    </tr>
                                    <tr>
                                        <td width="4%"></td>
                                        <td style="white-space: nowrap" colspan="2">
                                            <asp:Label ID="lblcurrprice" runat="server" Text="Your current price is set to $ 0.00. Please set your selling price:"
                                                CssClass="labelall"></asp:Label>
                                            <asp:HiddenField ID="hdnLastCost" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 10px;"></td>
                                    </tr>
                                    <tr>
                                        <td width="4%"></td>
                                        <td width="25%">
                                            <asp:Label ID="Label14" runat="server" Text="SKU:" CssClass="labelall"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblsku" runat="server" CssClass="labelall"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 10px;"></td>
                                    </tr>
                                    <tr>
                                        <td width="4%"></td>
                                        <td width="25%">
                                            <asp:Label ID="Label18" runat="server" Text="Description:" CssClass="labelall"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblDescription" runat="server" CssClass="labelall"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 10px;"></td>
                                    </tr>
                                    <tr>
                                        <td width="3%"></td>
                                        <td width="25%">
                                            <asp:Label ID="Label5" runat="server" Text="Department:" CssClass="labelall"></asp:Label>
                                        </td>
                                        <td>
                                            <%-- <asp:DropDownList ID="ddldept" TabIndex="1" runat="server" Width="80%" AutoPostBack="true" CssClass="dropdownall">
                                  </asp:DropDownList>
                                        <asp:Label ID="lblDepartment" runat="server" Text="" CssClass="labelall"></asp:Label>--%>
                                            <asp:Label ID="lblDeptddl" AutoPostBack="true" runat="server" CssClass="labelall" Width="80%"></asp:Label>
                                            <asp:Button ID="btnSelectDept" runat="server" Text="..." OnClientClick="return fnOpenDept();" Height="22px" TabIndex="3"
                                                AutoPostBack="true" Width="8%" Style="cursor: pointer" />
                                            <asp:HiddenField ID="hdnDept" runat="server" />
                                            <asp:HiddenField ID="hdnDeptText" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 10px;"></td>
                                    </tr>
                                    <tr id="trstyle" runat="server">
                                        <td width="3%"></td>
                                        <td width="25%">
                                            <asp:Label ID="lblStyle" runat="server" Text="Style:" CssClass="labelall"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblStyleddl" CssClass="labelall" runat="server" Width="80%"></asp:Label>
                                            <asp:Button ID="btnSelectStyle" runat="server" Text="..." OnClientClick="return fnOpenStyle();" Height="22px" TabIndex="4"
                                                AutoPostBack="true" Width="8%" Style="cursor: pointer" />
                                            <asp:HiddenField ID="hdnStyle" runat="server" />
                                            <asp:HiddenField ID="hdnStyleText" runat="server" />
                                            <%--  <asp:DropDownList ID="ddlStyle" TabIndex="2" runat="server" Width="80%" AutoPostBack="true" CssClass="dropdownall">
                                  </asp:DropDownList>
                                        <asp:Label ID="lblNoStyle" runat="server" Text="" CssClass="labelall"></asp:Label>--%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 10px;"></td>
                                    </tr>
                                    <tr id="trsize" runat="server">
                                        <td width="3%"></td>
                                        <td width="25%">
                                            <asp:Label ID="Label3" runat="server" CssClass="labelall" Text="Size:"></asp:Label>
                                        </td>
                                        <td>

                                            <asp:Label ID="lblSizeddl" CssClass="labelall" runat="server" Width="80%"></asp:Label>

                                            <asp:Button ID="btnSelectSize" runat="server" Text="..." OnClientClick="return fnOpenSize();" Height="22px" TabIndex="5"
                                                AutoPostBack="true" Width="8%" Style="cursor: pointer" />
                                            <asp:Label ID="lblNoSize" runat="server" Text="No Size Found" CssClass="labelall"
                                                Visible="false"></asp:Label>
                                            <asp:HiddenField ID="hdnSize" runat="server" />
                                            <asp:HiddenField ID="hdnSizeText" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 10px;"></td>
                                    </tr>
                                    <tr id="trcosteach" runat="server">
                                        <td width="4%"></td>
                                        <td width="25%">
                                            <asp:Label ID="Label4" runat="server" Text="Cost:" CssClass="labelall"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblCostEach" runat="server" Text="" CssClass="labelall"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 10px;"></td>
                                    </tr>
                                    <%--  <tr>
                                    <td width="3%">
                                    </td>
                                    <td width="18%">
                                        <asp:Label ID="Label15" runat="server" Text="Recommended Markup:" CssClass="labelall"></asp:Label>
                                        
                                    </td>
                                    <td>
                                        <asp:Label ID="lblRecomMarkup" runat="server"  CssClass="labelall"></asp:Label>
                                         <asp:TextBox ID="txtRecomMarkup" runat="server" Width="50%" TabIndex="1" onkeypress="return fncInputNumericdecimalValuesOnly(event)"
                                                MaxLength="7" onfocus="onfocuscheckPercent(this.id);this.select();" CssClass="textboxRight"  onblur="getfloatwithpercentage(this.id,2); CalculateRecommendedPrice()"></asp:TextBox>
                                  </td>
                                </tr>--%>
                                    <tr>
                                        <td style="height: 10px;"></td>
                                    </tr>
                                    <tr id="trpriceeach" runat="server">
                                        <td width="3%"></td>
                                        <td valign="top">
                                            <asp:Label ID="Label16" runat="server" Text="Price:" CssClass="labelall"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtRecomPrice" runat="server" Width="25%" TabIndex="3" onkeypress="return fncInputNumericdecimalValuesOnly(event)"
                                                MaxLength="7" onfocus="onfocuscheck(this.id);this.select();" CssClass="textboxRight"
                                                onblur="getfloatwithDollar(this.id,2); "></asp:TextBox><br />
                                            <br />
                                            <br />
                                            <asp:Label ID="Label15" runat="server" Text="(Note: Please set price based on recommended markup, via the department that is defined in the list management section.)"
                                                CssClass="labelall"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 10px;"></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td align="left" valign="bottom"></td>
                            <td align="right">
                                <asp:Button ID="btnOKSetPrice" CssClass="alterrow" Font-Bold="true" Text="Set Price" Width="90px" Height="28px" runat="server" CausesValidation="false"
                                    TabIndex="4" />
                                &nbsp;&nbsp;
                            <asp:Button ID="btnCancelSetPrice" CssClass="alterrow" Font-Bold="true" Text="Cancel" Width="90px" Height="28px" runat="server" CausesValidation="true" ValidationGroup="receive"
                                TabIndex="5" OnClientClick="$find('mpeSetPrice').hide();" />&nbsp;&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 10px;"></td>
                        </tr>
                    </table>
                </ContentTemplate>
            </ajax:UpdatePanel>
        </asp:Panel>




        <%---------------------------------- Modal Popup SKU/qty per case popup Add--------------------------------------%>
        <ajaxToolkit:ModalPopupExtender ID="mpeQtyperCase" BehaviorID="mpeQtyperCase" RepositionMode="None"
            runat="server" BackgroundCssClass="modalBackground" TargetControlID="btnQtyperCase"
            PopupControlID="pnlQtyperCase" DropShadow="false" />
        <asp:Button ID="btnQtyperCase" Style="display: none;" runat="server" />
        <asp:Panel ID="pnlQtyperCase" DefaultButton="imgOKQtyPerCase" CssClass="modalPopup"
            runat="server" Width="500px" Style="display: none;">
            <ajax:UpdatePanel ID="upnlQtyperCase" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                        <tr>
                            <td height="27" background="images/popup_01.gif" colspan="2">
                                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td colspan="3">&nbsp;
                                        <asp:Label ID="lblQtyPerCaseHeader" CssClass="stylePopupHeader" runat="server"></asp:Label>
                                        </td>
                                        <td align="right" valign="middle">
                                            <asp:ImageButton ID="ImageButton9" OnClientClick="mphide();"
                                                runat="server" ImageUrl="~/Images/close1.gif" />&nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" valign="top" colspan="2">
                                <div id="dvQtyChecked" runat="server" visible="False">
                                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                        <tr>
                                            <td colspan="6" style="height: 7px;"></td>
                                        </tr>
                                        <tr>
                                            <td style="width: 10%;">&nbsp;&nbsp;<asp:Label ID="Label10" runat="server" CssClass="labelall" Text="Cases"></asp:Label></td>
                                            <td align="left" style="width: 10%;">
                                                <asp:TextBox ID="txtDefaultCase" runat="server" CssClass="textboxRight" MaxLength="3"
                                                    onfocus="this.select();" onblur="checkblank(this);POQtyBlur();" TabIndex="1"
                                                    Width="60px"></asp:TextBox>
                                                <ajaxToolkit:FilteredTextBoxExtender ID="Fdefaultcase" runat="server" FilterMode="ValidChars"
                                                    TargetControlID="txtDefaultCase" ValidChars="0123456789,-">
                                                </ajaxToolkit:FilteredTextBoxExtender>
                                            </td>
                                            <td align="right" style="width: 25%;">
                                                <asp:Label ID="Label6" runat="server" CssClass="labelall" Text="Qty Per Case"></asp:Label>
                                            </td>
                                            <td align="left" style="width: 19%;">&nbsp;
                                            <asp:TextBox ID="txtCaseOf" runat="server" CssClass="textboxRight" Width="60px" TabIndex="2"
                                                MaxLength="5" onfocus="this.select();" onblur="checkblank(this);POQtyBlur();"></asp:TextBox>
                                                <ajaxToolkit:FilteredTextBoxExtender ID="FCaseOf" FilterMode="ValidChars" ValidChars="0123456789,-"
                                                    TargetControlID="txtCaseOf" runat="server">
                                                </ajaxToolkit:FilteredTextBoxExtender>
                                            </td>
                                            <td align="right" style="width: 20%;">
                                                <asp:Label ID="Label7" runat="server" CssClass="labelall" Text="Case Cost"></asp:Label>
                                                &nbsp;
                                            </td>
                                            <td align="right" style="width: 16%;">
                                                <asp:TextBox ID="txtcaseDollar" runat="server" CssClass="textboxRight" Width="60px"
                                                    Enabled="false" MaxLength="7" onfocus="this.select();" onblur="checkblank(this);onBlurDecimal(this.id);"> </asp:TextBox>
                                                &nbsp;
                                            <ajaxToolkit:FilteredTextBoxExtender ID="FcaseDollar" FilterMode="ValidChars" ValidChars="0123456789,-,."
                                                TargetControlID="txtcaseDollar" runat="server">
                                            </ajaxToolkit:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="6" style="height: 7px;"></td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;
                                            </td>
                                            <td></td>
                                            <td align="right">
                                                <asp:Label ID="Label8" runat="server" CssClass="labelall" Text="Extras"></asp:Label>
                                            </td>
                                            <td align="left">&nbsp;
                                            <asp:TextBox ID="txtPoTotal" runat="server" Style="display: none"></asp:TextBox>
                                                <asp:TextBox ID="txtPlus" runat="server" CssClass="textboxRight" Width="60px" TabIndex="3"
                                                    MaxLength="3" onfocus="this.select();" onblur="checkblank(this);ExtraInvBlur();"></asp:TextBox>
                                            </td>
                                            <td align="right">
                                                <asp:Label ID="Label9" runat="server" CssClass="labelall" Text="Unit Cost"></asp:Label>
                                                &nbsp;
                                            </td>
                                            <td align="right">
                                                <asp:TextBox ID="txtUnitDollar" runat="server" CssClass="textboxRight" Width="60px"
                                                    Enabled="false" onfocus="this.select();" onblur="checkblank(this);onBlurDecimal(this.id);"
                                                    MaxLength="7"></asp:TextBox>
                                                &nbsp;
                                            <ajaxToolkit:FilteredTextBoxExtender ID="FUnitDollar" FilterMode="ValidChars" ValidChars="0123456789,-,."
                                                TargetControlID="txtUnitDollar" runat="server">
                                            </ajaxToolkit:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="6" style="height: 7px;"></td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;
                                            </td>
                                            <td></td>
                                            <td></td>
                                            <td></td>
                                            <td align="right">
                                                <asp:Label ID="Label13" runat="server" CssClass="labelall" Text="Cost Ext"></asp:Label>
                                                &nbsp;
                                            </td>
                                            <td align="right">
                                                <asp:TextBox ID="txttotalingQtyChecked" runat="server" CssClass="textboxRight" Width="60px"
                                                    TabIndex="4" MaxLength="7" onfocus="this.select();" onblur="checkblank(this);onBlurDecimal(this.id);CostExtInvBlur();"></asp:TextBox>
                                                &nbsp;
                                            <ajaxToolkit:FilteredTextBoxExtender ID="Ftotalqty" FilterMode="ValidChars" ValidChars="0123456789,-,."
                                                TargetControlID="txttotalingQtyChecked" runat="server">
                                            </ajaxToolkit:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div id="dvQtyNotChecked" runat="server" width="35%">
                                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                        <tr>
                                            <td colspan="3">&nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="6%">&nbsp;
                                            </td>
                                            <td width="25%">&nbsp;&nbsp;&nbsp;
                                            <asp:Label ID="lblQtytoReceive" runat="server" Text="PO Qty :" CssClass="labelall"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtQtytoReceive" runat="server" Width="25%" TabIndex="7" MaxLength="5"
                                                    onfocus="this.select();" onblur="checkblank(this);PoQty_Blur();" CssClass="textboxRight"></asp:TextBox>
                                                <ajaxToolkit:FilteredTextBoxExtender ID="FQtyReceive" FilterMode="ValidChars" ValidChars="0123456789,-,."
                                                    TargetControlID="txtQtytoReceive" runat="server">
                                                </ajaxToolkit:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3" style="height: 7px;"></td>
                                        </tr>
                                        <tr>
                                            <td width="6%">&nbsp;
                                            </td>
                                            <td width="25%">&nbsp;&nbsp;&nbsp;
                                            <asp:Label ID="lblCostEa" runat="server" Text="Unit Cost :" CssClass="labelall"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtCostEa" runat="server" Width="50%" TabIndex="8" MaxLength="7"
                                                    onfocus="this.select();" onblur="checkblank(this);onBlurDecimal(this.id);PoQty_Blur();"
                                                    CssClass="textboxRight"></asp:TextBox>
                                                <ajaxToolkit:FilteredTextBoxExtender ID="FCostEa" FilterMode="ValidChars" ValidChars="0123456789,-,."
                                                    TargetControlID="txtCostEa" runat="server">
                                                </ajaxToolkit:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3" style="height: 7px;"></td>
                                        </tr>
                                        <tr>
                                            <td width="4%">&nbsp;
                                            </td>
                                            <td width="25%">&nbsp;&nbsp;&nbsp;
                                            <asp:Label ID="lblTotaling" runat="server" Text="Cost Ext :" CssClass="labelall"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTotaling" runat="server" Width="50%" TabIndex="9" MaxLength="7"
                                                    onfocus="this.select();" onblur="checkblank(this);onBlurDecimal(this.id);UnitCost_Blur();"
                                                    CssClass="textboxRight"></asp:TextBox>
                                                <ajaxToolkit:FilteredTextBoxExtender ID="FTotaling" FilterMode="ValidChars" ValidChars="0123456789,-,."
                                                    TargetControlID="txtTotaling" runat="server">
                                                </ajaxToolkit:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td align="right" valign="bottom">&nbsp;
                        <td align="right">
                            <asp:Button ID="imgOKQtyPerCase" runat="server" TabIndex="10" CssClass="alterrow"
                                Font-Bold="true" CausesValidation="false" Height="28px" Width="70px" Style="vertical-align: top"
                                Text="OK" />
                            &nbsp;&nbsp;
                            <asp:Button ID="imgCancelQtyPerCase" runat="server" CausesValidation="false" CssClass="alterrow"
                                Font-Bold="true" Height="28px" Width="70px" Style="vertical-align: top" TabIndex="11"
                                OnClientClick="mphide();" Text="Cancel" />&nbsp;
                        </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="height: 5px;"></td>
                        </tr>
                    </table>
                </ContentTemplate>
            </ajax:UpdatePanel>
        </asp:Panel>
    </form>
    <script type="text/javascript" language="javascript">

        Sys.Browser.WebKit = {}; //Safari 3 is considered WebKit
        if (navigator.userAgent.indexOf('WebKit/') > -1) {
            Sys.Browser.agent = Sys.Browser.WebKit;
            Sys.Browser.version = parseFloat(navigator.userAgent.match(/WebKit\/(\d+(\.\d+)?)/)[1]);
            Sys.Browser.name = 'WebKit';
        }



        if (Sys.Browser.agent == Sys.Browser.Firefox) {
            var mouse;
            window.onmousemove = function storeMouse(e) {
                if (!e) e = window.event;
                mouse = { clientX: e.clientX, clientX: e.clientY };
            }

            window.onbeforeunload = function (evt) {
                if (mouse.clientX < 10) {
                    if ('<%=ViewState("isInv").ToString() %>' == 'Y') {
                        window.opener.document.getElementById('ctl00_ContentPlaceHolder2_txtSKU').focus();
                    }
                }
            }
        }
        else {
            window.onbeforeunload = function () {
                if ((window.event.clientX < 0) || (window.event.clientY < 0)) // close button
                {
                    if ('<%=ViewState("isInv").ToString() %>' == 'Y') {
                        window.opener.document.getElementById('ctl00_ContentPlaceHolder2_txtSKU').focus();
                    }
                }
                if (window.event.altKey == true || window.event.ctrlKey == true) // ALT + F4
                {
                    if ('<%=ViewState("isInv").ToString() %>' == 'Y') {
                        window.opener.document.getElementById('ctl00_ContentPlaceHolder2_txtSKU').focus();
                    }

                }
            }
        }




    </script>
</body>
</html>
