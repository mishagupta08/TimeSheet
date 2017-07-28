$(document).ready(function () {
    $(".preloader").hide();
});

function SaveEmployeeDetail() {
    $("#error").html("");
    var loginDetail = $('#employeeform').serialize();
    $(".preloader").show();
    $.ajax({
        url: '/Dashboard/SaveEmployeeDetail',
        type: 'Post',
        datatype: 'Json',
        data: loginDetail
    }).done(function (result) {
        $("#error").html(result);
        $(".preloader").hide();
    }).fail(function (error) {
        $("#error").html(error.statusText);
        $(".preloader").hide();
    });

    return false;
}

function SaveProjectDetail() {
    $("#error").html("");
    var loginDetail = $('#projectform').serialize();
    $(".preloader").show();
    $.ajax({
        url: '/Dashboard/SaveProjectDetail',
        type: 'Post',
        datatype: 'Json',
        data: loginDetail
    }).done(function (result) {
        $("#error").html(result);
        $(".preloader").hide();
    }).fail(function (error) {
        $("#error").html(error.statusText);
        $(".preloader").hide();
    });

    return false;
}

function SaveChatForum() {
    $("#error").html("");
    var chatDetail = $('#chatForumform').serialize();
    $(".preloader").show();
    $.ajax({
        url: '/ChatForum/SaveChatForum',
        type: 'Post',
        datatype: 'Json',
        data: chatDetail
    }).done(function (result) {
        $("#error").html(result);
        $(".preloader").hide();
    }).fail(function (error) {
        $("#error").html(error.statusText);
        $(".preloader").hide();
    });

    return false;
}

function FilterWorkReport() {
    $("#error").html("");
    var fiterDetail = $('#workDetailFilterform').serialize();
    $(".preloader").show();
    $.ajax({
        url: '/Dashboard/GetFilterWorkList',
        type: 'Post',
        datatype: 'Json',
        data: fiterDetail
    }).done(function (result) {
        $("#workListcontainer").html(result);
        $(".preloader").hide();
    }).fail(function (error) {
        $("#error").html(error.statusText);
        $(".preloader").hide();
    });

    return false;
}

function FilterProject() {
    $("#error").html("");
    var fiterDetail = $('#projectFilterform').serialize();
    $(".preloader").show();
    $.ajax({
        url: '/Dashboard/GetFilterProjectList',
        type: 'Post',
        datatype: 'Json',
        data: fiterDetail
    }).done(function (result) {
        $("#projectListcontainer").html(result);
        $(".preloader").hide();
    }).fail(function (error) {
        $("#error").html(error.statusText);
        $(".preloader").hide();
    });

    return false;
}

