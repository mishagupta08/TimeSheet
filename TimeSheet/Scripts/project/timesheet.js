$(document).ready(function () {
    $(".preloader").hide();
});

function SaveTimesheetDetail() {
    $("#error").html("");

    var datestring = $('#dateinput').val();
    datearray = datestring.split("-");

    var lengthOfMonth = (datearray[1]).length;
    var twoDigitMonth = (lengthOfMonth > 1) ? (datearray[1]) : '0' + (datearray[1]);
    var lengthOfDate  =  ((datearray[0])).length;
    var twoDigitdate  =  (lengthOfDate > 1) ? (datearray[0]) : '0' + (datearray[0]);
    var newFromDate   = twoDigitdate + "-" + twoDigitMonth + "-" + datearray[2];
        
         $('#dateinput').val(newFromDate);
         $(".preloader").show();
         var loginDetail = $('#timesheetform').serialize();
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
