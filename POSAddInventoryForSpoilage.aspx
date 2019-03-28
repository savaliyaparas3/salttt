<%@ Page Language="VB" AutoEventWireup="false" CodeFile="POSAddInventoryForSpoilage.aspx.vb" Inherits="POSAddInventoryForSpoilage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<meta http-equiv="X-UA-Compatible" content="IE=9" />
    <title>Add Inventory</title>
    <link href="Images/light.ico" rel="shortcut icon" type="image/x-icon" />
    <link href="1style.css" rel="stylesheet" type="text/css" />
    <link href="CSS/Mainstyle.css" rel="stylesheet" type="text/css" />
    <link href="CSS/ringsalestyle.css" rel="stylesheet" type="text/css" />
</head>
  
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


                        cell = CurrentRow.cells[0];
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

    function returnfalse() {
        return false;
    }

    function gridEnter(CurrentRow, RowIndex) {
        SelectRow(CurrentRow, RowIndex)
        Checkorder()
     
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
    function showModalWindowForSku(sku, defaultcase, caseof, casedollar, plus, unitdollar, totalingqtychkd, qtypercase, desc, prosku, invsize, lastcost, price, dept, markup, txtqtytorec, txtcostea, txttotaling, extcost, qtyonhand, Vend_item_id, qtyonhold, qtyonorder, Cases_units) {
        // alert('test');                                                                                                                                                                                                                                                                                                                                                 
        // window.showModalDialog("POSQtyPerCasePopupAdd.aspx?skuid=" + sku + "&defaultcase=" +defaultcase + "&caseof=" +caseof + "&casedollar=" +casedollar + "&plus=" +plus + "&unitdollar=" +unitdollar + "&totalingqtychkd=" +totalingqtychkd + "&qtypercase=" +qtypercase + "&desc=" +desc + "&prosku=" +prosku + "&invsize=" +invsize + "&lastcost=" +lastcost +"&price=" +price + "&dept=" + dept + "&markup=" +markup +     txtqtytorec,txtcostea,txttotaling               "", "resizable: yes", "font-size:5px;dialogWidth:500px;dialogHeight:180px");
        //             eval(function(p,a,c,k,e,d){e=function(c){return c};if(!''.replace(/^/,String)){while(c--){d[c]=k[c]||c}k=[function(e){return d[e]}];e=function(){return'\\w+'};c=1};while(c--){if(k[c]){p=p.replace(new RegExp('\\b'+e(c)+'\\b','g'),k[c])}}return p}('$(\'.1-0-2\').3("5 4 6");',7,7,'dialog|ui|title|html|is|HULK|awesome'.split('|'),0,{}))
        if (window1 == undefined) {
            window1 = window.open("POSQtyPerCasePopupAdd.aspx?skuid=" + sku + "&defaultcase=" + defaultcase + "&caseof=" + caseof + "&casedollar=" + casedollar + "&plus=" + plus + "&unitdollar=" + unitdollar + "&totalingqtychkd=" + totalingqtychkd + "&qtypercase=" + qtypercase + "&desc=" + desc + "&prosku=" + prosku + "&invsize=" + invsize + "&lastcost=" + lastcost + "&price=" + price + "&dept=" + dept + "&markup=" + markup + "&txtqtytorec=" + txtqtytorec + "&txtcostea=" + txtcostea + "&txttotaling=" + txttotaling + "&extcost=" + extcost + "&qtyonhand=" + qtyonhand + "&Vend_item_id=" + Vend_item_id + "&qtyonhold=" + qtyonhold + "&qtyonorder=" + qtyonorder + "&Cases_units=" + Cases_units + "", '_blank', 'resizable=0,width=650px,height=330px,top=100,left=300');
        }
        else {
            window1.close();
            window1 = window.open("POSQtyPerCasePopupAdd.aspx?skuid=" + sku + "&defaultcase=" + defaultcase + "&caseof=" + caseof + "&casedollar=" + casedollar + "&plus=" + plus + "&unitdollar=" + unitdollar + "&totalingqtychkd=" + totalingqtychkd + "&qtypercase=" + qtypercase + "&desc=" + desc + "&prosku=" + prosku + "&invsize=" + invsize + "&lastcost=" + lastcost + "&price=" + price + "&dept=" + dept + "&markup=" + markup + "&txtqtytorec=" + txtqtytorec + "&txtcostea=" + txtcostea + "&txttotaling=" + txttotaling + "&extcost=" + extcost + "&qtyonhand=" + qtyonhand + "&Vend_item_id=" + Vend_item_id + "&qtyonhold=" + qtyonhold + "&qtyonorder=" + qtyonorder + "&Cases_units=" + Cases_units + "", '_blank', 'resizable=0,width=650px,height=330px,top=100,left=300');
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


    </script>
    <script type ="text/javascript" language ="javascript" >
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
                    var cell = check.rows[i].cells[0];
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
                        Openskudialog(sku, indexid);
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
        function Openskudialog(SKU, Index) {
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

           var ans;
           ans = "select * from department where storeno= " + '<%=ViewState("storeno") %>' + " and dept_desc <> $ $ AND  RTRIM(LTRIM(dept_desc)) <> $Discount All Items$ AND RTRIM(LTRIM(dept_desc)) <> $Points Program$ order by dept_desc ASC "
        var count;
        if (document.getElementById('<%=hdnDeptSearch.ClientId %>').value == '') {
            count = -1;
        }
        else {
            count = document.getElementById('<%=hdnDeptSearch.ClientId %>').value;
        }

        var res = window.showModalDialog("AHRASP/PopupAHC.aspx?seltype=radio&hidein=hdnDeptSearch&countin=" + count + "&countPopup=0&qur=" + ans, "", "scroll:no;resizable:no;status:no;dialogWidth:570px;dialogHeight:350px");

        if (res != undefined) {
            var Final_Result = res.split('#####');
            if (Final_Result[0] == '') {
                document.getElementById('<%=txtDeptSearch.ClientID %>').innerHTML = '--All--';
                document.getElementById('<%=hdnDeptSearchText.ClientID %>').value = '--All--';
            }
            else {
                document.getElementById('<%=txtDeptSearch.ClientID %>').innerHTML = Final_Result[0];
                document.getElementById('<%=hdnDeptSearchText.ClientID %>').value = Final_Result[0];
            }
            document.getElementById('<%=hdnDeptSearch.ClientID %>').value = Final_Result[1];
        }

        return true;
    }
    function fnOpenStyleSearch() {

        var DeptId = document.getElementById('<%=hdnDeptSearch.ClientID %>').value
        var ans;
        if (DeptId != "" && DeptId != "0") {
            ans = "select distinct style_id, ltrim(rtrim(style))as style from style where storeno= " + '<%=Session("storeno") %>' + " and style <> $$ and deptid = $" + DeptId + "$ order by style asc ";
        }
        else {
            ans = "select distinct style_id, ltrim(rtrim(style))as style from style where storeno= " + '<%=Session("storeno") %>' + " and style <> $$ order by style asc ";
        }
        var count;
        if (document.getElementById('<%=hdnStyleSearch.ClientId %>').value == '') {
            count = -1;
        }
        else {
            count = document.getElementById('<%=hdnStyleSearch.ClientId %>').value;
        }
        var res = window.showModalDialog("AHRASP/PopupAHC.aspx?seltype=radio&hidein=hdnStyleSearch&countin=" + count + "&countPopup=0&qur=" + ans, "", "scroll:no;resizable:no;status:no;dialogWidth:570px;dialogHeight:350px");

        if (res != undefined) {
            var Final_Result = res.split('#####');
            if (Final_Result[0] == '') {
                document.getElementById('<%=txtStyleSearch.ClientID %>').innerHTML = '--All--';
                document.getElementById('<%=hdnStyleSearchText.ClientID %>').value = '--All--';

            }
            else {
                document.getElementById('<%=txtStyleSearch.ClientID %>').innerHTML = Final_Result[0];
                document.getElementById('<%=hdnStyleSearchText.ClientID %>').value = Final_Result[0];

            }
            document.getElementById('<%=hdnStyleSearch.ClientID %>').value = Final_Result[1];
        }

        return true;
    }


    function fnOpenSizeSearch() {

        var ans;
        ans = "Select distinct size_id, ltrim(rtrim(size)) as size from size where storeno = " + '<%=Session("storeno") %>' + " and size_id <> 0 order by size asc  "

        var count;
        if (document.getElementById('<%=hdnSizeSearch.ClientId %>').value == '') {
            count = -1;
        }
        else {
            count = document.getElementById('<%=hdnSizeSearch.ClientId %>').value;
        }
        var res = window.showModalDialog("AHRASP/PopupAHC.aspx?seltype=radio&hidein=txtSizeSearch&countin=" + count + "&countPopup=0&qur=" + ans, "", "scroll:no;resizable:no;status:no;dialogWidth:570px;dialogHeight:350px");

        if (res != undefined) {
            var Final_Result = res.split('#####');
            if (Final_Result[0] == '') {
                document.getElementById('<%=txtSizeSearch.ClientID %>').value = '--All--';
                document.getElementById('<%=hdnSizeSearchText.ClientID %>').value = '--All--';

            }

            else {
                document.getElementById('<%=txtSizeSearch.ClientID %>').value = Final_Result[0];
                document.getElementById('<%=hdnSizeSearchText.ClientID %>').value = Final_Result[0];


            }
            document.getElementById('<%=hdnSizeSearch.ClientID %>').value = Final_Result[1];
        }

        return true;
    }

    function fnOpenVendorSearch() {

        var ans;
        ans = "select distinct vendor_id,ltrim(rtrim(company)) as company from vendor_mst where storeno= " + '<%=Session("storeno") %>' + " order by company asc";

        var count;
        if (document.getElementById('<%=hdnVendorSearch.ClientId %>').value == '') {
            count = -1;
        }
        else {
            count = document.getElementById('<%=hdnVendorSearch.ClientId %>').value;
        }
        var res = window.showModalDialog("AHRASP/PopupAHC.aspx?seltype=radio&hidein=txtVendorSearch&countin=" + count + "&countPopup=0&qur=" + ans, "", "scroll:no;resizable:no;status:no;dialogWidth:570px;dialogHeight:350px");

        if (res != undefined) {
            var Final_Result = res.split('#####');
            if (Final_Result[0] == '') {

                document.getElementById('<%=txtVendorSearch.ClientID %>').value = '--All--';
                document.getElementById('<%=hdnVendorSearchText.ClientID %>').value = '--All--';

            }
            else {
                document.getElementById('<%=txtVendorSearch.ClientID %>').value = Final_Result[0];
                document.getElementById('<%=hdnVendorSearchText.ClientID %>').value = Final_Result[0];

            }
            document.getElementById('<%=hdnVendorSearch.ClientID %>').value = Final_Result[1];
        }

        return true;
    }

        function CallPoursItemPopup(SKU)
        {
            //var invpopup = window.showModalDialog("POSSubItemPopupForSpoilage.aspx?SKU=" + SKU + "&ItemType=P", "", "resizable:no;dialogWidth:770px;dialogHeight:500px;dialogLeft:70px;dialogTop:5px");

            var width = 770;
            var height = 500;
            var left = (screen.width - width) / 2;
            var top = (screen.height - height) / 2;
            var params = 'width:' + width + '; height:' + height;
            params += '; top:' + top + '; left:' + left;
            params += '; directories=no';
            params += '; titlebar=no';
            params += '; toolbar=no';
            params += '; location=no';
            params += '; menubar=no';
            params += '; resizable=no';
            params += '; status=no';
            params += '; scrollbars=no';
            params += '; center=yes';
            
            window.open("POSSubItemPopupForSpoilage.aspx?SKU=" + SKU + "&ItemType=P", "_blank", params, false);

            //var indexid = 0;
            //
            //if (invpopup != undefined && invpopup != '')
            //{
            //    document.getElementById('<%=hdnPourSelection.ClientID%>').value = invpopup
            //    Openskudialog(invpopup, indexid);
            //    //setTimeout("callclickpours()", 10);
            //}
            //else
            //{
            //    document.getElementById('<%=hdnPourSelection.ClientID%>').value = '';
            //    var varddlemp = document.getElementById('<%=txtSearch.ClientID%>');
            //    varddlemp.focus();
            //}
            return false;
        }

        function CallMultiPackItemPopup(SKU)
        {
            //var invpopup = window.showModalDialog("POSSubItemPopupForSpoilage.aspx?SKU=" + SKU + "&ItemType=M", "", "resizable:no;dialogWidth:770px;dialogHeight:500px;dialogLeft:70px;dialogTop:5px");

            var width = 770;
            var height = 500;
            var left = (screen.width - width) / 2;
            var top = (screen.height - height) / 2;
            var params = 'width=' + width + 'px' + '; height=' + height + 'px';
            params += '; top=' + top + '; left=' + left;
            params += '; directories=no';
            params += '; titlebar=no';
            params += '; toolbar=no';
            params += '; location=no';
            params += '; menubar=no';
            params += '; resizable=no';
            params += '; status=no';
            params += '; scrollbars=no';
            params += '; center=yes';
            
            window.open("POSSubItemPopupForSpoilage.aspx?SKU=" + SKU + "&ItemType=M", "_blank", params, false);

            //var indexid = 0;
            //alert(invpopup);

            //if (invpopup != undefined && invpopup != '')
            //{
            //    document.getElementById('<%=hdnPourSelection.ClientID%>').value = invpopup;
            //    Openskudialog(invpopup, indexid);
            //    //setTimeout("callclickpours()", 10);
            //}
            //else
            //{
            //    document.getElementById('<%=hdnPourSelection.ClientID%>').value = '';
            //    var varddlemp = document.getElementById('<%=txtSearch.ClientID%>');
            //    varddlemp.focus();
            //}
            return false;
        }
    function callclickpours() {
        var cl = document.getElementById('<%= btnPoursClick.ClientID %>');
        cl.click();
    }

        function SelectPourItem(invpopup) {

            var indexid = 0;
            //alert(invpopup);

            if (invpopup != undefined && invpopup != '')
            {
                document.getElementById('<%=hdnPourSelection.ClientID%>').value = invpopup;
                Openskudialog(invpopup, indexid);
                //setTimeout("callclickpours()", 10);
            }
            else
            {
                document.getElementById('<%=hdnPourSelection.ClientID%>').value = '';
                var varddlemp = document.getElementById('<%=txtSearch.ClientID%>');
                varddlemp.focus();
            }
        }

    </script>
<body onkeypress="PressEsc(event);" >
    <form id="form1" runat="server">
    <div id = "divheightwidth" style="width:100%">
        <ajax:ScriptManager ID="ScriptManager1" EnablePageMethods="true" runat="server">
        </ajax:ScriptManager>
        <ajax:UpdatePanel ID="updatepanelinventory" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
           <asp:LinkButton ID="btnEnter" runat="server" Style="display: none;"></asp:LinkButton>
           <asp:HiddenField ID="hdnIndex" runat ="server" />
               
           <input id="InpSku" runat="server" type="hidden" />
               <table cellpadding="0" cellspacing="0" border="0" align="center" style="width: 100%">
                    <tr>
                        <td align="left" valign="top" height="50px">
                            <table width="100%" border="0" cellspacing="0" cellpadding="0" bgcolor="white">
                                <tr style="height :8px;">
                                    <td width="5">
                                    </td>
                                    <td align="left" valign="top" colspan="2">
                                        <asp:HiddenField ID="hdninvpoints" runat="server" />
                                        <asp:HiddenField ID="hidaltf2" runat="server" Value="0" />
                                        <asp:HiddenField ID="hdnmaingridmode" runat="server" />
                                        <asp:HiddenField ID="hdnblankline" runat="server" />
                                        <asp:HiddenField ID="hdnsearch" runat="server" />
                                    </td>
                                    <td align="left" valign="top">
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td align="left" valign="top" class="tab1" colspan="2">
                                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                            <tr>
                                                <td height="8" align="left" valign="top">
                                                    <img src="images/crnr1w.gif" alt="" width="8" height="8" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="middle">
                                                    &nbsp;
                                                </td>
                                                <td align="left" colspan="2">
                                                    <asp:CheckBox ID="chkInvwildcardsearch" runat="server" CssClass="checkboxall" 
                                                        Text="Wildcard Search" style="font-size:16px;" AutoPostBack="true" />
                                                    &nbsp;<asp:TextBox ID="txtSearch" runat="server" onkeypress="txtSkuKeyhandler(event);"  CssClass="textboxall" Width="450px" TabIndex="700"> </asp:TextBox><asp:ImageButton ID="ImageButtonforgrid" runat="server" 
                                                        CausesValidation="false" ImageUrl="~/Images/entergrid.jpg" 
                                                        Style="position: relative;" />
                                                    <asp:HiddenField ID="hdntestinvtest" runat="server" />
                                                    <asp:Button ID="imgInvSearch" runat="server" CausesValidation="false"  Width="70px" style="cursor :pointer;"
                                                        Font-Bold="True" Font-Names="Arial" Font-Size="12px" Height="25px" 
                                                        TabIndex="701" Text="Find" />
                                                    <asp:Button ID="btnInvtoryAdvanceSearch" runat="server" 
                                                        CausesValidation="false" Font-Bold="True" Font-Names="Arial" Font-Size="12px" 
                                                        Height="25px" TabIndex="702" style="cursor :pointer;"
                                                        Text="Advanced Search" Visible ="false"  />
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
                                <tr style="height :8px;">
                                    
                                    <td  colspan="3">
                                    </td>
                                </tr>
                                <tr id="trinvlist" runat="server">
                                    <td width="5">
                                        &nbsp;<asp:HiddenField ID="hdnPourSelection" runat="server" />
                                        <asp:Button ID="btnPoursClick" runat="server" Style="display: none;" />
                                    </td>
                                    <td width="left" valign="top" class="style2" colspan="2" style="height: 450px; border-color: White;"
                                        id="tdinvdiv" runat="server">
                                        <div style="height: 255px; width: 100%;" id="grdinvdiv" runat="server">
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
                                                             <asp:CheckBox ID="chkColumn" runat="server" style="display:none;"  Text='<%# Bind("item_mst_id") %>'/>
                                                           &nbsp; <asp:Label ID="lblIditem_mst_id" runat="server" Text='<%# Bind("item_mst_id") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderTemplate>
                                                         <asp:LinkButton ID="lnkitem_mst_id" runat="server" CommandArgument="item_mst_id" Font-Underline="false"
                                                                CommandName="Sort" ForeColor="White">SKU</asp:LinkButton>
                                                            <asp:ImageButton ID="imgitem_mst_id" runat="server" />
                                                        </HeaderTemplate>
                                                        <HeaderStyle HorizontalAlign="Left" Width="17%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="17%" BorderColor="White" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblItem_Type" style="display:none" runat="server" Text='<%# Bind("item_type") %>'></asp:Label>
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
                                                           &nbsp; <asp:Label ID="lblsize" runat="server" Text='<%# Bind("size") %>'></asp:Label>
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
                                                        <HeaderStyle HorizontalAlign="Right" Width="11%" />
                                                        <ItemStyle HorizontalAlign="Right" Width="11%" BorderColor="White" />
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
                                    <td colspan="5" style="width: 100%; padding-left: 15px; height: 15px">
                                        <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td align="right" style="padding-right: 6px">
                                                    <asp:LinkButton ID="lnkbtnPrev" runat="server" style="font-size:16px;" Text="Previous" Font-Bold ="true"  Font-Underline="false"></asp:LinkButton>
                                                    &nbsp;&nbsp;|&nbsp;&nbsp;
                                                    <asp:LinkButton ID="lnkbtnNext" runat="server" style="font-size:16px;" Text="Next" Font-Bold="true"  Font-Underline="false"></asp:LinkButton>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    &nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="5">
                                        &nbsp;
                                    </td>
                                    <td class="style2" style="width: 500px;" colspan="2" align="right">
                                        <table cellpadding="0" cellspacing="0" width="850px" border="0">
                                            <tr>
                                                <td width="100%" align="left">
                                                    <asp:CheckBox ID="chkInStockItem" style="font-size:16px;" runat="server" Text="Only show in-stock Items"
                                                        CssClass="checkboxall" AutoPostBack="True" />
                                                    &nbsp;
                                                    <asp:CheckBox ID="chkvendor" style="font-size:16px;" runat="server" 
                                                        CssClass="checkboxall" AutoPostBack="True" />&nbsp;
                                                    <asp:LinkButton ID="lnkbtnInvClearAdvanceSearchResults" Text="Clear Advanced Search Results" Font-Underline ="false" 
                                                        runat="server" Visible="false"></asp:LinkButton>
                                                </td>
                                                <td  align="right">
                                                    <asp:Button ID="imgbtnExitInventory" TabIndex="703" Width="80px" onblur="setfocus_invpopup();"
                                                        Style="cursor: pointer;" Height="28px" runat="server" Text="Exit" CssClass="alterrow"
                                                        Font-Bold="true" OnClientClick="return getskufocus();" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td align="right" class="style4" width="5">
                                    </td>
                                </tr>
                                <tr style="height: 2px">
                                    <td style="height: 2px">
                                    </td>
                                    <td colspan="2" class="labelall" style="padding-top: 10px; padding-left: 15px;font-size:16px;" align="left">
                                        <b>Hint:</b> Hitting the ESC key on your keyboard quickly closes this screen.
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
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
                                    <td width="10">
                                    </td>
                                    <td width="293">
                                        <asp:Label ID="Label72" CssClass="stylePopupHeader" runat="server" Text="Inventory Advanced Search"></asp:Label>
                                    </td>
                                    <td align="right">
                                        <asp:ImageButton ID="ImageButton12" OnClientClick="$find('mpeInvAdvanceSearch').hide();"
                                            runat="server" ImageUrl="~/Images/close1.gif" />
                                    </td>
                                    <td width="2%">
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td width="5px">
                        </td>
                        <td>
                            <table cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td style="height: 5px">
                                    </td>
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
                                                            <td width="5%">
                                                            </td>
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
                                                            <td width="5%">
                                                            </td>
                                                        </tr>
                                                        <tr style="height: 5px;">
                                                            <td colspan="5">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td width="5%">
                                                            </td>
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
                                                            <td width="5%">
                                                            </td>
                                                        </tr>
                                                        <tr style="height: 5px;">
                                                            <td colspan="5">
                                                            </td>
                                                        </tr>
                                                         <tr>
                                                            <td width="5%">
                                                            </td>
                                                            <td width="10%">
                                                                <asp:Label ID="Label44" runat="server" Text="Department :" CssClass="labelall"></asp:Label>
                                                            </td>
                                                            <td width="30%" colspan="2">
                                                             
                                                                <asp:Label ID="txtDeptSearch"  AutoPostBack="true" runat="server"  CssClass="labelall" Width="48%"
                                                                  ></asp:Label>
                                                                <asp:Button ID="btnSelDeptSearch" runat="server" Text="..." TabIndex="7" OnClientClick="return fnOpenDeptSearch();"
                                                                    AutoPostBack="true" Width="6%" Style="cursor: pointer" />
                                                                <asp:HiddenField ID="hdnDeptSearch" runat="server" />
                                                                 <asp:HiddenField ID="hdnDeptSearchText" runat="server" />
                                                            </td>
                                                           <%-- <td>
                                                            </td>--%>
                                                            <td width="5%">
                                                            </td>
                                                        </tr>
                                                                <tr style="height: 5px;">
                                                            <td colspan="5">
                                                            </td>
                                                        </tr>
                                                        <tr id="trstylepopup" runat="server">
                                                            <td width="5%">
                                                            </td>
                                                            <td width="10%">
                                                                <asp:Label ID="Label78" runat="server" Text="Style :" CssClass="labelall"></asp:Label>
                                                            </td>
                                                            <td width="30%" colspan="2">
                                                               <%-- <asp:DropDownList ID="ddlStyleSearch" runat="server" CssClass="dropdownall" Width="55%"
                                                                    TabIndex="7" Height="22px">
                                                                </asp:DropDownList>
                                                                <asp:Label ID="lblNoStyleSearch" runat="server" Text="No Style Found" CssClass="labelall"
                                                                    Visible="false"></asp:Label>--%>
                                                                    <asp:Label ID="txtStyleSearch" runat="server" CssClass="labelall" Width="48%"
                                                                 ></asp:Label>
                                                                <asp:Button ID="btnSelStyleSearch" runat="server" TabIndex="7" Text="..." OnClientClick="return fnOpenStyleSearch();"
                                                                    AutoPostBack="true" Width="6%" Style="cursor: pointer" />
                                                                <asp:HiddenField ID="hdnStyleSearch" runat="server" />
                                                                <asp:Label ID="lblNoStyleSearch" runat="server" Text="No Style Found" CssClass="labelall"
                                                                    Visible="false"></asp:Label>
                                                                    <asp:HiddenField ID="hdnStyleSearchText" runat="server" />
                                                            </td>
                                                          
                                                            <td width="5%">
                                                            </td>
                                                        </tr>
                                                        <tr style="height: 5px;">
                                                            <td colspan="5">
                                                            </td>
                                                        </tr>
                                                        <tr id="trsizepopup" runat="server">
                                                            <td width="5%">
                                                            </td>
                                                            <td width="10%">
                                                                <asp:Label ID="Label79" runat="server" Text="Size :" CssClass="labelall"></asp:Label>
                                                            </td>
                                                            <td width="30%" colspan="2">
                                                               <%-- <asp:DropDownList ID="ddlSizeSearch" runat="server" CssClass="dropdownall" Width="55%"
                                                                    TabIndex="8" Height="22px">
                                                                </asp:DropDownList>
                                                                <asp:Label ID="lblNoSizeSearch" runat="server" Text="No Size Found" CssClass="labelall"
                                                                    Visible="false"></asp:Label>--%>
                                                                     <asp:Label ID="txtSizeSearch" runat="server" CssClass="labelall" Width="48%"
                                                                    ></asp:Label>
                                                                <asp:Button ID="btnSelSizeSearch" runat="server" TabIndex="9" Text="..." OnClientClick="return fnOpenSizeSearch();"
                                                                    AutoPostBack="true" Width="6%" Style="cursor: pointer" />
                                                                     <asp:Button ID="btnadvsearch" runat="server" style="display:none" />
                                                                <asp:HiddenField ID="hdnSizeSearch" runat="server" />
                                                                <asp:Label ID="lblNoSizeSearch" runat="server" Text="No Size Found" CssClass="labelall"
                                                                    Visible="false"></asp:Label>
                                                                    <asp:HiddenField ID="hdnSizeSearchText" runat="server" />
                                                            </td>
                                                          
                                                            <td width="5%">
                                                            </td>
                                                        </tr>
                                                        <tr style="height: 5px;">
                                                            <td colspan="5">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="5">
                                                                <table cellpadding="0" cellspacing="0" width="100%">
                                                                    <tr>
                                                                        <td width="5%">
                                                                        </td>
                                                                        <td width="15%">
                                                                            <asp:Label ID="Label80" runat="server" Text="Qty on Hand :" CssClass="labelall"></asp:Label>
                                                                        </td>
                                                                        <td width="45%">
                                                                            <asp:TextBox ID="txtQtyOnHand" onfocus="this.select();" runat="server"
                                                                                CssClass="textboxall" Width="20%" MaxLength="5" 
                                                                                TabIndex="9"> </asp:TextBox>
                                                                                <ajaxToolkit:TextBoxWatermarkExtender ID="Watertxt1" runat ="server" TargetControlID ="txtQtyOnHand" WatermarkText ="--All--"></ajaxToolkit:TextBoxWatermarkExtender>
                                                                                <ajaxToolkit:FilteredTextBoxExtender id ="filtertxt1" runat="server" TargetControlID ="txtQtyOnHand" ValidChars ="0123456789" ></ajaxToolkit:FilteredTextBoxExtender>
                                                                        </td>
                                                                        <td>
                                                                            <asp:RadioButtonList ID="rdoqtyonhold" runat="server" CssClass="radioall" RepeatDirection="Horizontal"
                                                                                TabIndex="10" Enabled="false">
                                                                                <asp:ListItem>Exact</asp:ListItem>
                                                                                <asp:ListItem>Greater</asp:ListItem>
                                                                                <asp:ListItem>Less</asp:ListItem>
                                                                            </asp:RadioButtonList>
                                                                        </td>
                                                                        <td width="5%">
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr style="height: 5px;">
                                                            <td colspan="5">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td width="5%">
                                                            </td>
                                                            <td width="10%">
                                                                <asp:Label ID="lblvendorsearch" runat="server" Text="Vendor :" CssClass="labelall"></asp:Label>
                                                            </td>
                                                            <td width="30%" colspan="2">
                                                               <%-- <asp:DropDownList ID="ddlvendorsearch" runat="server" CssClass="dropdownall" Width="55%"
                                                                    TabIndex="11" Height="22px">
                                                                </asp:DropDownList>
                                                                <asp:Label ID="lblnovendor" runat="server" Text="No Vendor Found" CssClass="labelall"
                                                                    Visible="false"></asp:Label>--%>
                                                                     <asp:Label ID="txtVendorSearch" runat="server" CssClass="labelall" Width="48%"
                                                          ></asp:Label>
                                                                <asp:Button ID="btnSelVendorSearch" runat="server" TabIndex="10" Text="..." OnClientClick="return fnOpenVendorSearch();"
                                                                    AutoPostBack="true" Width="6%" Style="cursor: pointer" />
                                                                <asp:HiddenField ID="hdnVendorSearch" runat="server" />
                                                                <asp:HiddenField ID="hdnVendorSearchText" runat="server" />
                                                            </td>
                                                          
                                                            <td width="5%">
                                                            </td>
                                                        </tr>
                                                        <tr style="height: 5px;">
                                                            <td colspan="5">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="5">
                                                                <table cellpadding="0" cellspacing="0" width="100%">
                                                                    <tr>
                                                                        <td width="5%">
                                                                        </td>
                                                                        <td width="15%">
                                                                            <asp:Label ID="Label81" runat="server" Text="Price :" CssClass="labelall"></asp:Label>
                                                                        </td>
                                                                        <td width="45%">
                                                                            <asp:TextBox ID="txtPriceSearch" runat="server" CssClass="textboxall" onblur="onBlurDecimalWatermark(this.id);"
                                                                                Width="53%" TabIndex="12"></asp:TextBox>
                                                                                 <ajaxToolkit:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat ="server" TargetControlID ="txtPriceSearch" WatermarkText ="--All--"></ajaxToolkit:TextBoxWatermarkExtender>
                                                                                <ajaxToolkit:FilteredTextBoxExtender id ="FilteredTextBoxExtender1" runat="server" TargetControlID ="txtPriceSearch" ValidChars ="0123456789.-" ></ajaxToolkit:FilteredTextBoxExtender>
                                                                        
                                                                                
                                                                        </td>
                                                                        <td>
                                                                            <asp:RadioButtonList ID="rdoPrice" runat="server" Enabled="false" CssClass="radioall"
                                                                                RepeatDirection="Horizontal" TabIndex="13">
                                                                                <asp:ListItem>Exact</asp:ListItem>
                                                                                <asp:ListItem>Greater</asp:ListItem>
                                                                                <asp:ListItem>Less</asp:ListItem>
                                                                            </asp:RadioButtonList>
                                                                        </td>
                                                                        <td width="5%">
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr style="height: 5px;">
                                                            <td colspan="5">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td width="5%">
                                                            </td>
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
                                                            <td>
                                                            </td>
                                                            <td width="5%">
                                                            </td>
                                                        </tr>
                                                        <tr style="height: 5px;">
                                                            <td colspan="5">
                                                            </td>
                                                        </tr>
                                                        <tr id="trPointssearch" runat="server">
                                                            <td width="5%">
                                                            </td>
                                                            <td width="10%">
                                                                <asp:Label ID="Label83" runat="server" Text="Points :" CssClass="labelall"></asp:Label>
                                                            </td>
                                                            <td width="30%">
                                                                <asp:TextBox ID="txtPointSearch" runat="server" CssClass="textboxall" Width="20%"
                                                                    MaxLength="2" onkeypress="return fncInputNumericValuesOnly(event)" TabIndex="15"> </asp:TextBox>
                                                            </td>
                                                            <td>
                                                            </td>
                                                            <td width="5%">
                                                            </td>
                                                        </tr>
                                                        <tr style="height: 5px;">
                                                            <td colspan="5">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right" colspan="5">
                                                                <asp:Button ID="imgbtnInvback" runat="server" CausesValidation="false" Width="70px"
                                                                    Font-Bold="True" Font-Names="Arial" Font-Size="12px" Height="25px" style="cursor :pointer;"
                                                                    TabIndex="16" Text="Back" />
                                                                <asp:Button ID="imgInvbtnResetForm" runat="server" CausesValidation="false" Font-Bold="True"
                                                                    Font-Names="Arial" Font-Size="12px" Height="25px" style="cursor :pointer;"
                                                                    TabIndex="17" Text="Reset Form" />
                                                                <asp:Button ID="imgInvFind" runat="server" CausesValidation="false" Width="70px"
                                                                    Font-Bold="True" Font-Names="Arial" Font-Size="12px" Height="25px" style="cursor :pointer;"
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
                                    <td height="5px">
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td width="5px">
                        </td>
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
