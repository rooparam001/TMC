$(document).ready(function () {
    _allPlaysMaster.fnloadData();
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

                var playdataTable = $('#carouselExampleIndicators2 .carousel-inner');
                playdataTable.empty();

                if (data.data.length == 0) {
                    playdataTable.append('<div class="col-md-4">No results found.</div>');
                }
                var colcount = 3; var rowcount = 0; var innerhtml = '';
                $(data.data).each(function (index, relationModelObj) {
                    
                    if (colcount == 3) {
                        if (rowcount == 0)
                            innerhtml += '<div class="carousel-item active"><div class="row">';
                        else
                            innerhtml += '<div class="carousel-item"><div class="row">';
                    }

                    innerhtml += ('<div class="col-md-4 mb-3"><div class="card"><img class="img-fluid" ' +
                        'src="/Blogs/Plays/' + relationModelObj.thumbnailUrl + '"><div class="card-body"><a href="/home/plays?objToken=' + relationModelObj.tokenID + '"><h4 class="card-title">' + relationModelObj.title + '</h4>' +
                        '</a><p class="card-text">' + relationModelObj.about + '</p></div></div></div>');

                    colcount--;
                    rowcount++;

                    if (colcount == 0) {
                        colcount = 3;
                        innerhtml += ('</div></div>');
                    }

                });
                playdataTable.append(innerhtml);
            },
            error: function (err) {
                alert(err.statusText);
            }
        });

        $.ajax({
            url: '/Home/GetAllUpcomingPlay',
            dataType: "json",
            method: 'GET',
            success: function (data) {

                var playdataTable = $('#carouselExampleIndicators3 .carousel-inner');
                playdataTable.empty();

                if (data.data.length == 0) {
                    playdataTable.append('<div class="col-md-4">No results found.</div>');
                }
                var colcount = 3; var rowcount = 0; var innerhtml = '';
                $(data.data).each(function (index, relationModelObj) {
                    if (colcount == 3) {
                        if (rowcount == 0)
                            innerhtml += '<div class="carousel-item active"><div class="row">';
                        else
                            innerhtml += '<div class="carousel-item"><div class="row">';
                    }

                    innerhtml += ('<div class="col-md-4 mb-3"><div class="card"><img class="img-fluid" ' +
                        'src="/Blogs/UpcomingPlays/' + relationModelObj.imageurl + '"><div class="card-body"><h4 class="card-title">' + relationModelObj.title + '</h4>' +
                        '</div></div></div>');

                    colcount--;
                    rowcount++;

                    if (colcount == 0) {
                        colcount = 3;
                        innerhtml += ('</div></div>');
                    }

                });
                playdataTable.append(innerhtml);
            },
            error: function (err) {
                alert(err.statusText);
            }
        });

        $.ajax({
            url: '/Account/GetAllHomePageSliders',
            dataType: "json",
            method: 'GET',
            success: function (data) {

                var sliderdataTable = $('.carousel-inner');
                sliderdataTable.empty();
                $(data.data.lst).each(function (index, relationModelObj) {

                    sliderdataTable.append('<div class="carousel-item ' + (index == 0 ? 'active' : '') + '"><img class="d-block w-100" src="/Blogs/Sliders/' + relationModelObj.sliderImgURL + '" alt="First slide">' +
                        '<div class="carousel-caption d-none d-md-block"><h5>' + (relationModelObj.title ? relationModelObj.title : '') + '</h5>' +
                        '<p>' + (relationModelObj.description ? relationModelObj.description : '') + '</p></div></div>');
                });

            },
            error: function (err) {
                alert(err.statusText);
            }
        });
    }
};