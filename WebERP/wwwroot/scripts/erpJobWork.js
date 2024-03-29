﻿$(document).ready(function () {    
    $('.jwDt').DataTable();        
});

$('#jwComDropDown').change(function (e) {
    e.preventDefault();
    var tempProc = $('#jwComDropDown').find('option:selected').val();
    var gridProc = tempProc; 
    $('#jwDtlTable').find('tr').each(function () {        
        var row = $(this);
        var qtyFilled = row.find("td:eq(7) input[type='number']").val();
        if (qtyFilled == null || typeof qtyFilled === "undefined" || qtyFilled=="") {
            row.find('[name*="PROC_CODE"]').val("0");
        }
        else {
            row.find('[name*="PROC_CODE"]').val(gridProc);
        }        
    });
});

$('.jwUserFilledQty').change(function (e) {    
    e.preventDefault();
    var currentRow = $(this).closest('tr');   
    var qtyFilled = currentRow.find("td:eq(7) input[type='number']").val();
    if (qtyFilled == null || typeof qtyFilled === "undefined" || qtyFilled == "") {        
        currentRow.find('[name*="PROC_CODE"]').val("0");
    }
    else {
        var gridProc = $('#jwComDropDown').find('option:selected').val();
        currentRow.find('[name*="PROC_CODE"]').val(gridProc);
    }  
});

$(function () {
    $('[data-toggle="tooltip"]').tooltip();
})
    

$("#jwEditDocDate").change(function () {
    var date = $(this).val();
    var financial_year = "";
    var today = new Date(date);
    if ((today.getMonth()) == 1 || (today.getMonth()) == 2 || (today.getMonth()) == 3) {
        financial_year = (today.getFullYear() - 1) + "" + today.getFullYear()
    } else {
        financial_year = today.getFullYear() + "" + (today.getFullYear() + 1)
    }
    $("#jwEditDocFinYear").val(financial_year);
});

$("#jwDocDate").change(function () {
    var date = $(this).val();
    var financial_year = "";
    var today = new Date(date);
    if ((today.getMonth()) == 1 || (today.getMonth()) == 2 || (today.getMonth()) == 3) {
        financial_year = (today.getFullYear() - 1) + "" + today.getFullYear()
    } else {
        financial_year = today.getFullYear() + "" + (today.getFullYear() + 1)
    }
    $("#jwDocFinYear").val(financial_year);
});

//$('.tickJobWork').click(function (e) {
//    var currentRow = $(this).closest('tr');    
//    var tempProc = $('.jwDropDown').find('option:selected').text();
//    var gridProc = tempProc.substr(6, 1);
//    $('#jwDtlTable').find('tr').each(function () {
//        var row = $(this);
//        if (row.find('input[type="checkbox"]').is(':checked')) {
//            row.find('[name*="PROC_CODE"]').val(gridProc);
//        }
//        else
//            row.find('[name*="PROC_CODE"]').val("0");
//    });  
//});

//var jwtableRows = $('#jwDtlTable tbody tr');
//$.each(jwtableRows, function (rowInd, row) {
//    var row = $(this);
//    if (row.find('input[type="checkbox"]').is(':checked') &&
//        row.find('textarea').val().length <= 0) {
//        alert('You must fill the text area!');
//    }
//    var inputs = $('[name*="PROC_CODE"], checkbox', row);
//    $.each(inputs, function (inputInd, input) {            
//        $(input).val(gridProc); 
//    });
//});