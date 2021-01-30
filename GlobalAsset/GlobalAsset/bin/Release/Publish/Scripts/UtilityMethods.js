var ShowMessage = function (type, message) {
    $('#mdlMessageViewer').removeClass('modal-warning');
    $('#mdlMessageViewer').removeClass('modal-danger');
    $('#mdlMessageViewer').removeClass('modal-success');
    $('#mdlMessageViewer').removeClass('modal-info');
    $('#mdlMessageViewer').removeClass('modal-primary');

    $('#mdlMessageViewer .modal-title').hide();
    $('#mdlMessageViewer #mdlTitle' + type).show();

    $('#mdlMessageViewer').addClass('modal-' + type);
    $('#mdlMessageViewer .lblMessage').text(message);
    $('#mdlMessageViewer').modal();
}


$(document).ready(function () {
    Pace.stop();
});

var makeMenuActive = function (itemName) {
    $('.subMenuItem').removeClass('active');
    $('.' + itemName).addClass('active');
}