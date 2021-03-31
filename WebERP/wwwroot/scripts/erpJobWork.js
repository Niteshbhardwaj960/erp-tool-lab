$("#addProcess").click(function (e) {
    debugger
    e.preventDefault();
    var tempProc = $('.jwDropDown').find('option:selected').text();
    var gridProc = tempProc.substr(6, 1);   
    $('#jwDtlTable').find('tr').each(function () {
        debugger
        var row = $(this);
        if (row.find('input[type="checkbox"]').is(':checked')) {
            row.find('[name*="PROC_CODE"]').val(gridProc);
            row.find('input[type="checkbox"]').prop("checked", false);
        }
    });
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