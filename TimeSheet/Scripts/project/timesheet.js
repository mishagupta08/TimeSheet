$(document).ready(function () {
    $(".preloader").hide();
});

function SaveTimesheetDetail() {
    $("#error").html("");
    var loginDetail = $('#timesheetform').serialize();
      $(".preloader").show();
    $.ajax({
        url: '/Timesheet/SaveTimesheetEntry',
        type: 'Post',
        datatype: 'Json',
        data: loginDetail
    }).done(function (result) {
        $("#error").html(result);
        $(".preloader").hide();
    }).fail(function (error) {
        $("#loginError").html(error.statusText);
        $(".preloader").hide();
        $("#closeError").show();
        $(".preloader").hide();
    });

    return false;
}