// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
//$(function () {
//    var PlaceHolderElement = $('#PlaceHolderHere');
//    $('button[data-toggle="ajax-modal"]').click(function (event) {        
//        debugger
//        var url = $(this).data('url');      
//        $.get(url).done(function (data) {
//            debugger
//            //$('#PlaceHolderHere').html(data);
//            //$('#PlaceHolderHere > .modal', data).modal('show');
//            PlaceHolderElement.html(data);
//            PlaceHolderElement.find('.modal').modal('show');
//        });
//    })

//    PlaceHolderElement.on('click', '[data-save="modal"]', function (event) {
//        var form = $(this).parents('.modal').find('form');
//        var actionUrl = form.attr('action');
//        var sendData = form.serialize();
//        $.post(actionUrl, sendData).done(function (data) {
//            PlaceHolderElement.find('.modal').modal('hide')
//        })
//    });
//});

//(function ($) {
//    function Index() {
//        var $this = this;
//        function initialize() {

//            $(".popup").on('click', function (e) {
//                modelPopup(this);
//            });

//            function modelPopup(reff) {
//                var url = $(reff).data('url');

//                $.get(url).done(function (data) {
//                    debugger;
//                    $('#modal-create-edit-user').find(".modal-dialog").html(data);
//                    $('#modal-create-edit-user > .modal', data).modal("show");
//                });

//            }
//        }

//        $this.init = function () {
//            initialize();
//        };
//    }
//    $(function () {
//        var self = new Index();
//        self.init();
//    });
//}(jQuery)); 