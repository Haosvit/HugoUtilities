$(document).ready(function () {
    var path = window.location.pathname;
    path = path.replace(/\/$/, "");
    path = decodeURIComponent(path);

    $('.active').removeClass('active');
    var liTag = $(".navbar a[href='" + path + "']").closest('li');
    liTag.addClass('active');
    $(liTag).closest('li.dropdown').addClass('active');
});