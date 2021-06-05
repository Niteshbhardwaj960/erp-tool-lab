$(document).ready(function () {
    $('.Select2DropDown').select2();
});

$('form').on('keyup keypress', function (e) {    
    var keyCode = e.keyCode || e.which;
    if (keyCode === 13) {
        e.preventDefault();
        return false;
    }
});
