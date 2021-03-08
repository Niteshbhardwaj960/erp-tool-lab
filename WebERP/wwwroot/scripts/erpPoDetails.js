if (typeof (WebERP) == "undefined") {
    WebERP = {};
}

WebERP.PurchasingOrders = {
    templateRow: {},

    Init: function () {

        $('#addRow').click(WebERP.PurchasingOrders.AddRowOnClick);
        $('#poDtlTable tbody').on('click', '#deleteRow', WebERP.PurchasingOrders.DeleteRowOnClick);
        $('#poDtlTable tbody').on('click', '#duplicateRow', WebERP.PurchasingOrders.DuplicateRowOnClick);

        WebERP.PurchasingOrders.templateRow = $('tr.template').clone();
    },

    AddRowOnClick: function (e) {        
        e.preventDefault();
        WebERP.PurchasingOrders._AddRow(WebERP.PurchasingOrders.templateRow, true);
    },

    DuplicateRowOnClick: function (e) {        
        e.preventDefault();
        var currentRow = $(this).closest('tr');
        WebERP.PurchasingOrders._AddRow(currentRow, false);
    },
    DeleteRowOnClick: function (e) {        
        e.preventDefault();
        $(this).closest('tr').remove();        
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
        //WebERP.PurchasingOrders._SetProperInputNames();
    },

    _SetProperInputNames() {
        var pogridtableRows = $('#poDtlTable tbody tr');
        $.each(pogridtableRows, function (rowInd, row) {
            var inputs = $('input, select', row);
            $.each(inputs, function (inputInd, input) {
                var oldName = $(input).attr('name');
                oldName = oldName.replace(/\[\d\]/, '[' + rowInd + ']');
                $(input).attr('name', oldName);
            });


        })
    }
}