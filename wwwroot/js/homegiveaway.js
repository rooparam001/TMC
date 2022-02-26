$(document).ready(function () {
    _allGiveawayMaster.fnPopulateDDL();
    _allGiveawayMaster.fnloadData();
    $("input[name='txtSearch']").change(function () {
        _allGiveawayMaster.searchText = $(this).val();
        _allGiveawayMaster.fnloadData();
    });
    $(".ddlCity").change(function () {
        _allGiveawayMaster.city = $(this).val();
        _allGiveawayMaster.fnloadData();
    });
});

_allGiveawayMaster = {
    searchText: '',
    selectedCity: '',
    fnloadData: function () {
        $.ajax({
            url: '/Home/GetAllGiveaways',
            dataType: "json",
            method: 'GET',
            data: { city: _allGiveawayMaster.city, searchTxt: _allGiveawayMaster.searchText },
            success: function (data) {

                var sliderdataTable = $('.dataview');
                sliderdataTable.empty();
                var cnt = 0;
                $(data.data).each(function (index, relationModelObj) {
                    var htmlStr = '';

                    htmlStr = '<div class="col-md-4"><div class="card mb-4 box-shadow"><div id="divCarousel' + cnt + '" class="carousel slide" data-ride="carousel"><div class="carousel-inner">';
                    arrayOfFiles = relationModelObj.objpictures.split(",");

                    for (i = 0; i < arrayOfFiles.length; i++) {

                        if (arrayOfFiles[i])
                            htmlStr += '<div class="carousel-item ' + (i == 0 ? 'active' : '') + '"><img class="d-block w-100" src="/Blogs/Sliders/' + arrayOfFiles[i] + '" alt="First slide"></div>';
                    }

                    htmlStr += '</div><a class="carousel-control-prev" href="#divCarousel' + cnt + '" role="button" data-slide="prev"><i class="fas fa-angle-left"></i></a><a class="carousel-control-next" href="#divCarousel' + cnt + '" role="button" data-slide="next"><i class="fas fa-angle-right"></i></a></div>';
                    htmlStr += '<div class="card-body"><p class="card-text">' + (relationModelObj.objtitle ? relationModelObj.objtitle : '') + '</p><div class="d-flex justify-content-between align-items-center">' +
                        '<small class="text-muted">' + (relationModelObj.city ? relationModelObj.city : '') + '</small></div></div>';

                    sliderdataTable.append(htmlStr);
                    cnt++;
                });

            },
            error: function (err) {
                alert(err.statusText);
            }
        });
    },
    fnPopulateDDL: function () {
        $.ajax({
            url: '/Home/GetAllCities',
            dataType: "json",
            method: 'GET',
            success: function (data) {

                var citydataTable = $('.ddlCity');
                citydataTable.empty();
                citydataTable.append('<option value="0">All Cities</option>');
                $(data.data).each(function (index, relationModelObj) {
                    citydataTable.append('<option value="' + relationModelObj.id + '">' + relationModelObj.city + '</option>');
                });

            },
            error: function (err) {
                alert(err.statusText);
            }
        });
    }
};