if (typeof (WebERP) == "undefined") {
    WebERP = {};
}

WebERP.SalesGrid = {    
    currentAjaxReq: null,
    tempRow: {},

    Init: function () {        
        $('#salesDtlTable tbody').on('change', 'select[name*="DISCPER_TAG"]', WebERP.SalesGrid.DisTagOnChange);
        $('#addSalesRow').click(WebERP.SalesGrid.AddSalesRowOnClick);        
        $('#salesDtlTable tbody').on('click', '#deleteSalesRow', WebERP.SalesGrid.DeleteSalesRowOnClick);
        $('#salesDtlTable tbody').on('change', 'select[name*="GODOWN_CODE"]', WebERP.SalesGrid.GoDownOnChange);
        $('#salesDtlTable tbody').on('change', 'select[name*="ITEM_CODE"]', WebERP.SalesGrid.ItemOnChange);
        $('#salesDtlTable tbody').on('change', 'select[name*="ARTICAL_CODE"]', WebERP.SalesGrid.ArticalOnChange);
        $('#salesForm').on('change', 'input[id="salesDocDate"]', WebERP.SalesGrid.GetFinancialYear);               
        $('#salesDtlTable tfoot').on('change', 'select[name*="TAX_CODE"]', WebERP.SalesGrid.TaxOnChange);
        //Calculation
        WebERP.SalesGrid.tempRow = $("#salesDtlTable tbody tr:last").clone();
        $('#salesDtlTable tbody').on('change', 'input[name*="QTY"]', WebERP.SalesGrid.QuantityOnChange);
        $('#salesDtlTable tfoot').on('change', 'input[name*="OTH_AMT"]', WebERP.SalesGrid.OtherAmount);
        $('#salesDtlTable tbody').on('change', 'input[name*="RATE"]', WebERP.SalesGrid.RateOnChange);
        $('#salesDtlTable tbody').on('change', 'input[name*="DISC_PER"]', WebERP.SalesGrid.DiscRateOnChange);

    },

    AddSalesRowOnClick: function (e) {
        debugger
        e.preventDefault();
        WebERP.SalesGrid._AddRow(WebERP.SalesGrid.tempRow, true);
    },
    _AddRow: function (sourceRow, appendToEnd) {
        debugger
        //sourceRow.find(".Select2DropDown").each(function (index) {
        //    $(this).select2('destroy');
        //});
        var clonedRow = sourceRow.clone(true);
        if (appendToEnd) {
            var salesgridtable = $('#salesDtlTable');
            salesgridtable.append(clonedRow);
            //$('.Select2DropDown').select2();
            //$('.Select2DropDown').last().next().next().remove();
        }
        else {
            clonedRow.insertAfter(sourceRow);
        }

        WebERP.SalesGrid.CalculateGrandTotal();
        WebERP.SalesGrid.CalculateQTotal();
       WebERP.SalesGrid._SetProperInputNames();
    },

    _SetProperInputNames() {
        var salesgridtableRows = $('#salesDtlTable tbody tr');
        $.each(salesgridtableRows, function (rowInd, row) {
            debugger
            var inputs = $('input, select', row);
            $.each(inputs, function (inputInd, input) {
                debugger
                var OldId = $(input).attr('id');
                var oldName = $(input).attr('name');
                oldName = oldName.replace(/\[\d\]/, '[' + rowInd + ']');
                OldId = OldId.replace(/\_\d\_/, '_' + rowInd + '_');
                $(input).attr('name', oldName);
                $(input).attr('id', OldId);
            });
        });
    },

    GoDownOnChange: function (e) {
        debugger       
        var nextCell = $(this).closest('td').next();
        $('select', nextCell).addClass('hidden');
        $('span', nextCell).remove();
        nextCell.append("<span style='font-size:2em' class='col-md-push-5 glyphicon glyphicon-refresh spin'></span>");
        WebERP.SalesGrid.GetFromVMList(
            $(this).val(),'G', 0 , 0,
            function (data, textStatus, xhr) {
                if (xhr.status == 200) {
                    debugger
                    var itemList = $('select', nextCell);
                    itemList.empty();
                    itemList.append('<option value="">Select</option>')
                    $.each(data, function (index, val) {
                        var $option;
                        $option = $('<option value="' + val.Value + '">' + val.Text + '</option>');
                        if (val.Selected) {
                            $option.attr('selected', 'selected');
                        }
                        //itemList.append('<option value="' + val.Value + '">' + val.Text + '</option>');
                        itemList.append($option);                       
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

    ItemOnChange: function (e) {     
        var currentRow = $(this).closest('tr');   
        var nextCell = $(this).closest('td').next();
        $('select', nextCell).addClass('hidden');
        $('span', nextCell).remove();
        nextCell.append("<span style='font-size:2em' class='col-md-push-5 glyphicon glyphicon-refresh spin'></span>");
        WebERP.SalesGrid.GetFromVMList(
            currentRow.find('select[name*="GODOWN_CODE"]').find('option:selected').val(), 'I', $(this).val(), 0,
            function (data, textStatus, xhr) {
                if (xhr.status == 200) {
                    debugger
                    var itemList = $('select', nextCell);
                    itemList.empty();
                    itemList.append('<option value="">Select</option>')
                    $.each(data, function (index, val) {
                        var $option;
                        $option = $('<option value="' + val.Value + '">' + val.Text + '</option>');
                        if (val.Selected) {
                            $option.attr('selected', 'selected');
                        }
                        //itemList.append('<option value="' + val.Value + '">' + val.Text + '</option>');
                        itemList.append($option);
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

    ArticalOnChange: function (e) {
        var currentRow = $(this).closest('tr');             
        WebERP.SalesGrid.GetFromVMList(
            currentRow.find('select[name*="GODOWN_CODE"]').find('option:selected').val(), 'A',
            currentRow.find('select[name*="ITEM_CODE"]').find('option:selected').val(), $(this).val(),
            function (data, textStatus, xhr) {
                if (xhr.status == 200) {
                    debugger
                    currentRow.find('[name*="SIZE_CODE"]').val(data.sizec);
                    currentRow.find('[name*="SIZE_NAME"]').val(data.size);
                    currentRow.find('[name*="SALE_QTY"]').val(data.quant);
                    WebERP.SalesGrid.CalculateQTotal();
                    WebERP.SalesGrid.CalculateGrandTotal();
                }             
            },
            function (data, textStatus, errorThrown) {
                if (textStatus != "abort") {
                    alert(errorThrown);                   
                }
                else {
                    alert('Unable to Connect while getting List')
                }
            }
        )
    },

    GetFromVMList: function (gcode, mode, icode, acode, successCallBack, errorCallBack) {
        if (WebERP.SalesGrid.currentAjaxReq !== null) {
            WebERP.SalesGrid.currentAjaxReq.abort();
            WebERP.SalesGrid.currentAjaxReq = null;
        }
        {
            if (mode == 'G')
                var ajaxUrl = "/Sales/GetVMFilter?mode=" + mode + "&gc=" + gcode; 
            if (mode == 'I')
                var ajaxUrl = "/Sales/GetVMFilter?mode=" + mode + "&gc=" + gcode + "&ic=" + icode; 
            if (mode == 'A')
                var ajaxUrl = "/Sales/GetVMFilter?mode=" + mode + "&gc=" + gcode + "&ic=" + icode + "&ac=" + acode; 

            WebERP.SalesGrid.currentAjaxReq = $.ajax({
                url: ajaxUrl,
                contentType: 'application/json; charset=utf-8',
                async: true,
                dataType: 'json',
                type: 'GET',
                success: successCallBack,
                error: errorCallBack
            });
        }
    },

    DeleteSalesRowOnClick: function (e) {
        e.preventDefault();
        $(this).closest('tr').remove();
        WebERP.SalesGrid.CalculateGrandTotal();
        WebERP.SalesGrid.CalculateQTotal();
    },
    OtherAmount: function (e) {        
        var grandTotal = 0;
        $.each($('#salesDtlTable tfoot').find('input[name*="OTH_AMT"]'), function () {
            if ($(this).val() != '' && !isNaN($(this).val())) {
                grandTotal += parseFloat($(this).val());
            }
        });      
        var TotalNet = grandTotal + parseFloat($("#TotalGrossItemAmt").val())
                                  + parseFloat($("#RoundoffAmount").val())
                                  + parseFloat($("#TotalTaxAmt").val());
        $("#TotalNetAmt").val(parseFloat(TotalNet));
    },

    TaxOnChange: function (e) {        
        WebERP.SalesGrid.TaxDataFilled(
            $(this).val(),
            function (data, textStatus, xhr) {
                if (xhr.status == 200) {                    
                    $("#igstper").val(data.igst);
                    $("#cgstper").val(data.cgst);
                    $("#sgstper").val(data.sgst); 
                    var grossAmount = $("#TotalGrossItemAmt").val();
                    $("#igstamt").val(WebERP.SalesGrid.CalculateTaxRate($("#igstper").val(), grossAmount));
                    $("#cgstamt").val(WebERP.SalesGrid.CalculateTaxRate($("#cgstper").val(), grossAmount));
                    $("#sgstamt").val(WebERP.SalesGrid.CalculateTaxRate($("#sgstper").val(), grossAmount));
                    $("#TotalTaxAmt").val(parseFloat($("#igstamt").val()) + parseFloat($("#cgstamt").val()) + parseFloat($("#sgstamt").val()));
                    $("#TotalNetAmt").val(parseFloat($("#TotalGrossItemAmt").val())
                        + parseFloat($("#RoundoffAmount").val())
                        + parseFloat($("#TotalTaxAmt").val())
                        + parseFloat($("#otherAmt1").val())
                        + parseFloat($("#otherAmt2").val()));
                }               
            },
            function (data, textStatus, errorThrown) {
                if (textStatus != "abort") {
                    alert(errorThrown);                  
                }
                else {
                    alert('Unable to Connect while getting Tax List')
                }
            }
        )       
    },

    CalculateTaxRate: function (gstPer, grossAmount) {
        debugger
        grossamt = parseFloat(grossAmount);
        gst = parseFloat(gstPer);
        return (grossamt * gst / 100).toFixed(2); 
      
    },

    TaxDataFilled: function (Code, successCallBack, errorCallBack) {
        if (WebERP.SalesGrid.currentAjaxReq !== null) {
            WebERP.SalesGrid.currentAjaxReq.abort();
            WebERP.SalesGrid.currentAjaxReq = null;
        }        
        WebERP.SalesGrid.currentAjaxReq = $.ajax({
            url: "/Sales/GetTaxList?code=" + Code,
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
        $("#salesFinancialYear").val(financial_year);
    },

    DisTagOnChange: function (e) {
        debugger
        var currentRow = $(this).closest("tr");
        var currentRate = currentRow.find("td:eq(7) input[type='number']").val();
        var disc_per;
        var discTag = $(this).val();
        disc_per = currentRow.find("td:eq(9) input[type='number']").val();
        if (discTag == "P") {
            if (disc_per.length) {
                netRate = WebERP.SalesGrid.CalculateNetRate(currentRate, disc_per, true);
            }
        }
        else
            netRate = WebERP.SalesGrid.CalculateNetRate(currentRate, disc_per, false);

        var netprice = parseFloat(netRate);
        currentRow.find("td:eq(10) input[type='text']").val((currentRate - netprice).toFixed(2));
        currentRow.find("td:eq(11) input[type='text']").val(netprice.toFixed(2));
        var currentQ = currentRow.find("td:eq(5) input[type='number']").val();
        if (currentQ.length) {
            rowAmount = WebERP.SalesGrid.CalculateFinalAmount(currentQ, netRate);
        }
        currentRow.find("td:eq(12) input[type='text']").val(rowAmount);
        WebERP.SalesGrid.CalculateGrandTotal();
        WebERP.SalesGrid.CalculateQTotal();

    },

    QuantityOnChange: function (e) {
        debugger
        var currentRow = $(this).closest('tr');
        var currentQ = $(this).val();
        if (currentQ == '') currentQ = 0;
        var netrate = currentRow.find("td:eq(11) input[type='text']").val();
        if (netrate == '') netrate = 0;
        if (netrate.length) {
            rowAmount = WebERP.SalesGrid.CalculateFinalAmount(currentQ, netrate);
        }
        currentRow.find("td:eq(12) input[type='text']").val(rowAmount);
        WebERP.SalesGrid.CalculateGrandTotal();
        WebERP.SalesGrid.CalculateQTotal();
    },

    RateOnChange: function (e) {
        var currentRow = $(this).closest('tr');
        var currentRate = $(this).val();
        if (currentRate == '')
            currentRate = 0;
        var disc_per;
        var discTag = currentRow.find('select[name*="DISCPER_TAG"]').find('option:selected').val();
        disc_per = currentRow.find("td:eq(9) input[type='number']").val();
        if (discTag == "P") {            
            if (disc_per.length) {
                netRate = WebERP.SalesGrid.CalculateNetRate(currentRate, disc_per, true);
            }
        }
        else {
            if (disc_per.length) {
                netRate = WebERP.SalesGrid.CalculateNetRate(currentRate, disc_per, false);
            }
        }

        var netprice = parseFloat(netRate);
        currentRow.find("td:eq(10) input[type='text']").val((currentRate - netprice).toFixed(2));
        currentRow.find("td:eq(11) input[type='text']").val(netprice.toFixed(2));
        var currentQ = currentRow.find("td:eq(5) input[type='number']").val();
        if (currentQ.length) {
            rowAmount = WebERP.SalesGrid.CalculateFinalAmount(currentQ, netRate);
        }
        currentRow.find("td:eq(12) input[type='text']").val(rowAmount);
        WebERP.SalesGrid.CalculateGrandTotal();
        WebERP.SalesGrid.CalculateQTotal();
    },

    DiscRateOnChange: function (e) {
        var currentRow = $(this).closest('tr');
        var disc_per = $(this).val(); 
        var currentRate = currentRow.find("td:eq(7) input[type='number']").val();
        if (currentRate == '')
            currentRate = 0;
        var discTag = currentRow.find('select[name*="DISCPER_TAG"]').find('option:selected').val();
        if (discTag == "P") {
            if (disc_per.length) {
                netRate = WebERP.SalesGrid.CalculateNetRate(currentRate, disc_per, true);
            }
        }
        else {
            if (disc_per.length) {
                netRate = WebERP.SalesGrid.CalculateNetRate(currentRate, disc_per, false);
            }
        }

        var netprice = parseFloat(netRate);
        currentRow.find("td:eq(10) input[type='text']").val((currentRate - netprice).toFixed(2));
        currentRow.find("td:eq(11) input[type='text']").val(netprice.toFixed(2));
        var currentQ = currentRow.find("td:eq(5) input[type='number']").val();
        if (currentQ.length) {
            rowAmount = WebERP.SalesGrid.CalculateFinalAmount(currentQ, netRate);
        }
        currentRow.find("td:eq(12) input[type='text']").val(rowAmount);
        WebERP.SalesGrid.CalculateGrandTotal();
        WebERP.SalesGrid.CalculateQTotal();
    },

    CalculateNetRate: function (currentRate, discount, disper) {
        ratePer = parseFloat(currentRate);
        discount = parseFloat(discount);
        if (disper) {
            return (ratePer - (ratePer * discount / 100)).toFixed(2); // Net Rate
        }
        else
            return (ratePer - discount).toFixed(2);
    },

    CalculateFinalAmount: function (quantity, NetRate) {
        quant = parseFloat(quantity);
        netrate = parseFloat(NetRate);
        return (netrate * quant).toFixed(2);
    },

    CalculateGrandTotal: function (e) {
        var grandTotal = 0;
        $.each($('#salesDtlTable').find('input[name*="ITEM_AMOUNT"]'), function () {
            if ($(this).val() != '' && !isNaN($(this).val())) {
                grandTotal += parseFloat($(this).val());
            }
        });
        $("#TotalGrossItemAmt").val(grandTotal.toFixed(2));
        var GrossRoundOff = Math.round($("#TotalGrossItemAmt").val());
        $("#RoundoffAmount").val(GrossRoundOff - ($("#TotalGrossItemAmt").val()));

        var grossAmount = $("#TotalGrossItemAmt").val();
        $("#igstamt").val(WebERP.SalesGrid.CalculateTaxRate($("#igstper").val(), grossAmount));
        $("#cgstamt").val(WebERP.SalesGrid.CalculateTaxRate($("#cgstper").val(), grossAmount));
        $("#sgstamt").val(WebERP.SalesGrid.CalculateTaxRate($("#sgstper").val(), grossAmount));
        $("#TotalTaxAmt").val(parseFloat($("#igstamt").val()) + parseFloat($("#cgstamt").val()) + parseFloat($("#sgstamt").val()));
        $("#TotalNetAmt").val(parseFloat($("#TotalGrossItemAmt").val())
            + parseFloat($("#RoundoffAmount").val())
            + parseFloat($("#TotalTaxAmt").val())
            + parseFloat($("#otherAmt1").val())
            + parseFloat($("#otherAmt2").val()));
    },

    CalculateQTotal: function (e) {
        var grandQTotal = 0;
        $.each($('#salesDtlTable').find('.salesRowQty'), function () {  //input[name*="QTY"]
            if ($(this).val() != '' && !isNaN($(this).val())) {
                grandQTotal += parseFloat($(this).val());
            }
        });
        $("#totalSaleQuant").val(grandQTotal);
    }

}