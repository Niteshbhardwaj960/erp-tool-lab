// Call the dataTables jQuery plugin
$(document).ready(function () {    
    $('#dataTable').DataTable();    
});

(function ($) {
    "use strict";

    $('#dtstate').DataTable();

    $('#dtcity').DataTable();
})(jQuery); 

//$(function () {

//    $('#addCountry').on('click', function (e) {
//        debugger
//        e.preventDefault();        
//        $.ajax({
//            type: "POST",
//            url: "Location/AddCountry",
//            dataType: "json",
//            contentType: 'application/json; charset=utf-8',
//            data: { country_code: "US", country_name:"United States of America" },//$('#menu-form').serialize(),
//            success: function (response) {
//                alert(response['response']);
//            },
//            error: function (e) {
//                alert('error' + e.responseText);
//            }
//        });
//        return false;
//    });
//});

