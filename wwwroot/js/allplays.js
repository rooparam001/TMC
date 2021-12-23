$(document).ready(function () {
    _allPlaysMaster.fnloadData();
    _allPlaysMaster.fnshowGenre();
    _allPlaysMaster.fnshowLanguage();
    _allPlaysMaster.fnshowCities();
});
$('.filter-item').on("click", function () {
    _allPlaysMaster.fnSelectFilter($(this));
});

_allPlaysMaster = {
    selectedGenre: [],
    selectedLanguage: [],
    selectedCity: [],
    fnloadData: function () {
        $.ajax({
            url: '/Home/GetAllPlays',
            dataType: "json",
            data: { 'genres': _allPlaysMaster.selectedGenre.join(","), languages: _allPlaysMaster.selectedLanguage.join(","), cities: _allPlaysMaster.selectedCity.join(",") },
            method: 'GET',
            success: function (data) {

                var playdataTable = $('.dataview');
                playdataTable.empty();

                if (data.data.length == 0) {
                    playdataTable.append('<div class="col-md-4"><div class="card mb-4 box-shadow">No results found.</div></div>');
                }

                $(data.data).each(function (index, relationModelObj) {
                    //playdataTable.append('<div class="col-md-4"><div class="card mb-4 box-shadow"><img class="card-img-top" src="/Blogs/Plays/' + relationModelObj.thumbnailUrl + '" alt="Play Image">' +
                    //    '<div class="card-body"><p class="card-text"><h5>' + relationModelObj.title + '</h5>' + relationModelObj.about + '</p><div class="d-flex justify-content-between align-items-center"><div class="btn-group">' +
                    //    '<a href="/home/plays?objToken=' + relationModelObj.tokenID + '" class="btn btn-sm btn-outline-secondary">View</a><i href="' + relationModelObj.bookUrl + '" class="btn btn-sm btn-outline-secondary">Book</i></div>' +
                    //    '<small class="text-muted">' + relationModelObj.dateCreated + '</small></div></div></div></div>');
                    playdataTable.append('<div class="col-md-4"><div class="card mb-4 box-shadow"><img class="card-img-top" src="/Blogs/Plays/' + relationModelObj.thumbnailUrl + '" alt="Play Image">' +
                        '<div class="card-body"><p class="card-text"><h5>' + relationModelObj.title + '</h5>' + relationModelObj.about + '</p><div class="d-flex justify-content-between align-items-center"><div class="btn-group">' +
                        '<a href="/home/plays?objToken=' + relationModelObj.tokenID + '" class="btn btn-sm btn-outline-secondary">View</a></div>' +
                        '<small class="text-muted">' + relationModelObj.dateCreated + '</small></div></div></div></div>');
                });

            },
            error: function (err) {
                alert(err.statusText);
            }
        });
    },
    fnSelectFilter: function (item) {
        var ifAdd = false;
        if (item.hasClass('filter-item-selected')) {
            item.removeClass('filter-item-selected');
        }
        else {
            item.addClass('filter-item-selected');
            ifAdd = true;
        }

        var itemID = item.attr('filter-key');
        var itemVal = item.attr('filter-val');
        var filterType = item.attr('filter-attr');

        switch (filterType) {
            case 'genre':
                if (ifAdd) {
                    _allPlaysMaster.selectedGenre.push(itemID);
                }
                else {
                    _allPlaysMaster.selectedGenre = _allPlaysMaster.selectedGenre.filter(function (elem) {
                        return elem != itemID;
                    });
                }
                break;
            case 'language':
                if (ifAdd) {
                    _allPlaysMaster.selectedLanguage.push(itemID);
                }
                else {
                    _allPlaysMaster.selectedLanguage = _allPlaysMaster.selectedLanguage.filter(function (elem) {
                        return elem != itemID;
                    });
                }
                break;
            case 'city':
                if (ifAdd) {
                    _allPlaysMaster.selectedCity.push(itemID);
                }
                else {
                    _allPlaysMaster.selectedCity = _allPlaysMaster.selectedCity.filter(function (elem) {
                        return elem != itemID;
                    });
                }
                break;
        };

        _allPlaysMaster.fnloadData();

    },
    fnshowGenre: function () {
        $.ajax({
            url: '/Home/GetAllGenres',
            dataType: "json",
            method: 'GET',
            success: function (data) {

                var genredataTable = $('.tbl-genre');
                genredataTable.empty();

                $(data.data).each(function (index, relationModelObj) {
                    genredataTable.append('<tr><td class="filter-item-td"><span class="filter-item" onclick="_allPlaysMaster.fnSelectFilter($(this));" filter-attr="genre" filter-key="' + relationModelObj.id + '" filter-val="' + relationModelObj.genre + '">' + relationModelObj.genre + '</span></td></tr>');
                });

            },
            error: function (err) {
                alert(err.statusText);
            }
        });
    },
    fnshowLanguage: function () {
        $.ajax({
            url: '/Home/GetAllLanguages',
            dataType: "json",
            method: 'GET',
            success: function (data) {

                var langdataTable = $('.tbl-language');
                langdataTable.empty();

                $(data.data).each(function (index, relationModelObj) {
                    langdataTable.append('<tr><td class="filter-item-td"><span class="filter-item" onclick="_allPlaysMaster.fnSelectFilter($(this));" filter-attr="language" filter-key="' + relationModelObj.id + '" filter-val="' + relationModelObj.languageval + '">' + relationModelObj.languageval + '</span></td></tr>');
                });

            },
            error: function (err) {
                alert(err.statusText);
            }
        });
    },
    fnshowCities: function () {
        $.ajax({
            url: '/Home/GetAllCities',
            dataType: "json",
            method: 'GET',
            success: function (data) {

                var citydataTable = $('.tbl-city');
                citydataTable.empty();

                $(data.data).each(function (index, relationModelObj) {
                    citydataTable.append('<tr><td class="filter-item-td"><span class="filter-item" onclick="_allPlaysMaster.fnSelectFilter($(this));" filter-attr="city" filter-key="' + relationModelObj.id + '" filter-val="' + relationModelObj.city + '">' + relationModelObj.city + '</span></td></tr>');
                });

            },
            error: function (err) {
                alert(err.statusText);
            }
        });
    }
};