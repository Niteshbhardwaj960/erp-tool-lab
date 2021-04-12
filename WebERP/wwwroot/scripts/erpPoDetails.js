if (typeof (WebERP) == "undefined") {
    WebERP = {};
}

WebERP.PurchasingOrders = {
    templateRow: {},
    termtemplateRow: {},
    currentAjaxReq: null,
  
    Init: function () {

        $('#addRow').click(WebERP.PurchasingOrders.AddRowOnClick);
        $('#poDtlTable tbody').on('click', '#deleteRow', WebERP.PurchasingOrders.DeleteRowOnClick);
        $('#poDtlTable tbody').on('click', '#duplicateRow', WebERP.PurchasingOrders.DuplicateRowOnClick);
        $('#poDtlTable tbody').on('change', 'select[name*="ITEM_CODE"]', WebERP.PurchasingOrders.ItemOnChange);
        $('#poForm').on('change', 'input[id="purchaseDate"]', WebERP.PurchasingOrders.GetFinancialYear);

        $('#addTermRow').click(WebERP.PurchasingOrders.AddTermRowOnClick);
        $('#poTermDtlTable tbody').on('click', '#deleteTermRow', WebERP.PurchasingOrders.DeleteRowOnClick);

        $('#poForm').submit(WebERP.PurchasingOrders.FormOnSubmit);

        WebERP.PurchasingOrders.termtemplateRow = $('tr.termtemplate').clone();
        WebERP.PurchasingOrders.templateRow = $('tr.template').clone();

        //Calculation 
        $('#poDtlTable tbody').on('change', 'input[name*="QTY"]', WebERP.PurchasingOrders.QuantityOnChange);
        $('#poDtlTable tbody').on('change', 'input[name*="RATE"]', WebERP.PurchasingOrders.RateOnChange);
        $('#poDtlTable tbody').on('change', 'input[name*="DISC_PER"]', WebERP.PurchasingOrders.DiscRateOnChange);
     
    },
   
    FormOnSubmit: function (e) {        
        e.preventDefault();
        WebERP.PurchasingOrders._SetProperInputNames();
        WebERP.PurchasingOrders._SetProperTermInputNames();
        $(this)[0].submit();
    },

    AddTermRowOnClick: function (e) {
        e.preventDefault();
        WebERP.PurchasingOrders._AddTermRow(WebERP.PurchasingOrders.termtemplateRow, true);
    },

    _AddTermRow: function (sourceRow, appendToEnd) {        
        var clonedRow = sourceRow.clone(true);
        if (appendToEnd) {
            var pogridtable = $('#poTermDtlTable'); //div.container-fluid #dataTable table tbody
            pogridtable.append(clonedRow);
        }
        else {
            clonedRow.insertAfter(sourceRow);
        }

        var sourceInputs = sourceRow.find('textarea');
        var targetInputs = clonedRow.find('textarea');
        for (var i = 0; i < sourceInputs.length; i++) {
            var s = $(sourceInputs[i]);
            var t = $(targetInputs[i]);
            t.val(s.val());
        }   

        WebERP.PurchasingOrders._SetProperTermInputNames();
    },

    _SetProperTermInputNames() {        
        var pogridtableRows = $('#poTermDtlTable tbody tr');
        $.each(pogridtableRows, function (rowInd, row) {            
            var inputs = $('textarea ,select', row);
            $.each(inputs, function (inputInd, input) {                
                var oldName = $(input).attr('name');
                //oldName = oldName.replace('item', 'item[' + rowInd + ']')
                oldName = oldName.replace(/\[\d\]/, '[' + rowInd + ']');
                $(input).attr('name', oldName);
            });
        });
    },

    AddRowOnClick: function (e) {        
        e.preventDefault();        
        WebERP.PurchasingOrders._AddRow(WebERP.PurchasingOrders.templateRow, true);
    },

    _AddRow: function (sourceRow, appendToEnd) {

        var clonedRow = sourceRow.clone(true);
        if (appendToEnd) {
            var pogridtable = $('#poDtlTable'); //div.container-fluid #dataTable table tbody
            pogridtable.append(clonedRow);
        }
        else {
            clonedRow.insertAfter(sourceRow);
        }

        var sourceInputs = sourceRow.find('input, select');
        var targetInputs = clonedRow.find('input, select');
        for (var i = 0; i < sourceInputs.length; i++) {
            var s = $(sourceInputs[i]);
            var t = $(targetInputs[i]);
            t.val(s.val());
        }
        WebERP.PurchasingOrders._SetProperInputNames();
    },

    _SetProperInputNames() {        
        var pogridtableRows = $('#poDtlTable tbody tr');
        $.each(pogridtableRows, function (rowInd, row) {
            debugger
            var inputs = $('input, select , textarea', row);
            $.each(inputs, function (inputInd, input) {
                debugger
                var oldName = $(input).attr('name');
                oldName = oldName.replace(/\[\d\]/, '[' + rowInd + ']');
                $(input).attr('name', oldName);
            });
        });
    },

    DuplicateRowOnClick: function (e) {        
        e.preventDefault();
        var currentRow = $(this).closest('tr');
        WebERP.PurchasingOrders._AddRow(currentRow, false);
    },

    DeleteRowOnClick: function (e) {
        e.preventDefault();
        $(this).closest('tr').remove();
        WebERP.PurchasingOrders.CalculateGrandTotal();
        WebERP.PurchasingOrders.CalculateQTotal();
    },

    ItemOnChange: function (e) {
        debugger
        var currentrow = $(this).closest("tr");
        var nextCell = $(this).closest('td').next().next();
        $('select', nextCell).addClass('hidden');
        $('span', nextCell).remove();
        nextCell.append("<span style='font-size:2em' class='col-md-push-5 glyphicon glyphicon-refresh spin'></span>");
        WebERP.PurchasingOrders.ItemGetList(
            $(this).val(),
            function (data, textStatus, xhr) {
                if (xhr.status == 200) {
                    debugger
                    var itemList = $('select', nextCell);
                    itemList.empty();
                    itemList.addClass("Select2DropDown");
                    itemList.append('<option value="">Select</option>')
                    $.each(data, function (index, val) {
                        var $option;
                        $option = $('<option value="' + val.Value + '">' + val.Text + '</option>');
                        if (val.Selected) {
                        $option.attr('selected', 'selected');
                        }
                        //itemList.append('<option value="' + val.Value + '">' + val.Text + '</option>');
                        itemList.append($option);
                        if (val.Selected) {
                            currentrow.find('input[name*="QTYUOMNAME"]').val(val.Text);
                            currentrow.find('input[name*="QTY_UOM"]').val(val.Value);
                        }
                    });                                       
                    itemList.removeAttr('disabled');
                }
                $('span', nextCell).remove();
                $('select', nextCell).removeClass('hidden');
            },
            function (data, textStatus, errorThrown) {
                if (textStatus != "abort") {
                    alert(errorThrown);
                    $('span', nextCell).remove();
                    $('select', nextCell).removeClass('hidden');
                    $('span', nextCell).empty();
                    $('select', nextCell).attr('disabled', 'disabled');
                }
                else {
                    alert('Unable to Connect while getting List')
                }
            }
        )
    },

    ItemGetList: function (itemCode, successCallBack, errorCallBack) {
        if (WebERP.PurchasingOrders.currentAjaxReq !== null) {
            WebERP.PurchasingOrders.currentAjaxReq.abort();
            WebERP.PurchasingOrders.currentAjaxReq = null;
        }

        WebERP.PurchasingOrders.currentAjaxReq = $.ajax({
            url: "/PO/GetQtyUOMList?itemCode=" + itemCode,
            contentType: 'application/json; charset=utf-8',
            async: true,
            dataType: 'json',
            type: 'GET',
            success: successCallBack,
            error: errorCallBack
        });
    },

    GetFinancialYear() {        
        var date = $(this).val();
        var financial_year = "";
        var today = new Date(date);
        if ((today.getMonth()) == 1 || (today.getMonth()) == 2 || (today.getMonth()) == 3) {
            financial_year = (today.getFullYear() - 1) + "" + today.getFullYear()
        } else {
            financial_year = today.getFullYear() + "" + (today.getFullYear() + 1)
        }
        $("#orderFinancialYear").val(financial_year);       
    },

    RateOnChange: function (e) {       
        var currentRow = $(this).closest('tr');
        var currentRate = $(this).val();
        if (currentRate == '')
            currentRate = 0; 
        var disc_per = currentRow.find("td:eq(5) input[type='number']").val();
        if (disc_per.length) {
            netRate = WebERP.PurchasingOrders.CalculateNetRate(currentRate, disc_per);
        }       
        var netprice = parseFloat(netRate);
        currentRow.find("td:eq(6) input[type='text']").val((currentRate - netprice).toFixed(2));
        currentRow.find("td:eq(7) input[type='text']").val(netRate);
        var currentQ = currentRow.find("td:eq(3) input[type='number']").val();
        if (currentQ.length) {
            rowAmount = WebERP.PurchasingOrders.CalculateFinalAmount(currentQ, netRate);
        } 
        currentRow.find("td:eq(8) input[type='text']").val(rowAmount);
        WebERP.PurchasingOrders.CalculateGrandTotal();
        WebERP.PurchasingOrders.CalculateQTotal();
    },

    DiscRateOnChange: function (e) {
        var currentRow = $(this).closest('tr');
        var disc_per = $(this).val();        
        var currentRate = currentRow.find("td:eq(4) input[type='number']").val();
        if (currentRate == '')
            currentRate = 0;
        if (disc_per.length) {
            netRate = WebERP.PurchasingOrders.CalculateNetRate(currentRate, disc_per);
        }
        var netprice = parseFloat(netRate);
        currentRow.find("td:eq(6) input[type='text']").val((currentRate - netprice).toFixed(2));
        currentRow.find("td:eq(7) input[type='text']").val(netRate);

        var currentQ = currentRow.find("td:eq(3) input[type='number']").val();
        if (currentQ.length) {
            rowAmount = WebERP.PurchasingOrders.CalculateFinalAmount(currentQ, netRate);
        }
        currentRow.find("td:eq(8) input[type='text']").val(rowAmount);
        WebERP.PurchasingOrders.CalculateGrandTotal();
        WebERP.PurchasingOrders.CalculateQTotal();
    },

    QuantityOnChange: function (e) {              
        var currentRow = $(this).closest('tr');
        var currentQ = $(this).val();
        if (currentQ == '') currentQ = 0;
        var netrate = currentRow.find("td:eq(7) input[type='text']").val();
        if (netrate == '') netrate = 0;
        if (netrate.length) {
            rowAmount = WebERP.PurchasingOrders.CalculateFinalAmount(currentQ, netrate);
        }        
        currentRow.find("td:eq(8) input[type='text']").val(rowAmount);
        WebERP.PurchasingOrders.CalculateGrandTotal();
        WebERP.PurchasingOrders.CalculateQTotal();
    },

    CalculateNetRate: function (currentRate, discount) {        
        ratePer = parseFloat(currentRate);
        discount = parseFloat(discount);
        return (ratePer - (ratePer * discount / 100)).toFixed(2); // Net Rate
    },

    CalculateFinalAmount: function (quantity, NetRate) {
        quant = parseFloat(quantity);        
        netrate = parseFloat(NetRate);
        return (netrate * quant).toFixed(2);
    },

    CalculateGrandTotal: function (e) {        
        var grandTotal = 0;
        $.each($('#poDtlTable').find('input[name*="AMOUNT"]'), function () {
            if ($(this).val() != '' && !isNaN($(this).val())) {
                grandTotal += parseFloat($(this).val());
            }
        });
        $("#totalAmount").val(grandTotal);
    },

    CalculateQTotal: function (e) {        
        var grandQTotal = 0;
        $.each($('#poDtlTable').find('.poRowQty'), function () {  //input[name*="QTY"]
            if ($(this).val() != '' && !isNaN($(this).val())) {
                grandQTotal += parseFloat($(this).val());
            }
        });
        $("#totalQuant").val(grandQTotal);
    }

}

