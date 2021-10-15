$(document).ready(function () {
    var viewCount = 0;
    if (localStorage.getItem("SiteViewCount") === null) {
        localStorage.setItem('SiteViewCount', 1);
    }
    else {
        viewCount = localStorage.getItem("SiteViewCount");
        localStorage.removeItem("SiteViewCount");
        localStorage.setItem('SiteViewCount', ++viewCount);

        if (viewCount <= 2) {
            localStorage.colorSetting = '#a4509b';
            $('.main-body-content').hide();
            $('.loadingcurtain-main').show();
        }
        else {
            $('.loadingcurtain-main').hide();
            $('.main-body-content').show();
        }
    }


    $('.loadingcurtain-main').click(function () {
        $(this).slideUp(1000, 'linear', function () {
            $('.loadingcurtain-main').hide();
            $('.main-body-content').show();
        });
    });
});