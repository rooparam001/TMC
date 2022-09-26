$(document).ready(function () {
    _allPlaysMaster.fnloadData();
    // Checking whether FormData is available in browser
    $('#contactUsSendMsg').click(function () {
        var validate = true
        if ($('#name').val() == "") {
            $("#form-message-warning").html("Kindly enter your name")
            $("#form-message-warning").show()
            validate = false
        }
        else if ($('#email').val() == "") {
            $("#form-message-warning").html("Kindly enter your email adress")
            $("#form-message-warning").show()
            validate = false
        }
        else if ($('#subject').val() == "") {
            $("#form-message-warning").html("Kindly enter the subject")
            $("#form-message-warning").show()
            validate = false
        }
        else if ($('#message').val() == "") {
            $("#form-message-warning").html("Kindly enter the message")
            $("#form-message-warning").show()
            validate = false
        }
        if (validate) {
            $("#form-message-warning").html("")
            $("#form-message-warning").hide()
            if (window.FormData !== undefined) {
                // Create FormData object
                var fileData = new FormData();
                // Adding one more key to FormData object
                fileData.append('UserName', $('#name').val());
                fileData.append('EmailAdd', $('#email').val());
                fileData.append('UserSubject', $('#subject').val());
                fileData.append('UserMessage', $('#message').val());

                $.ajax({
                    url: '/Account/SaveEnquiry',
                    type: "post",
                    contentType: false, // Not to set any content header
                    processData: false, // Not to process data
                    dataType: 'json',
                    data: fileData,
                    success: function (result) {
                        $('#name').val("")
                        $('#email').val("")
                        $('#subject').val("")
                        $('#message').val("")
                        $("#form-message-success").show()
                    },
                    error: function (err) {
                        alert(err.statusText);
                    }
                });
            } else {
                alert("FormData is not supported.");
            }
        }
    });
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

                    //innerhtml += ('<div class="col-md-4 mb-3"><div class="card"><img class="img-fluid" ' +
                    //    'src="/Blogs/Plays/' + relationModelObj.thumbnailUrl + '"><div class="card-body"><a href="/home/plays?objToken=' + relationModelObj.tokenID + '"><h4 class="card-title">' + relationModelObj.title + '</h4>' +
                    //    '</a><p class="card-text">' + relationModelObj.about + '</p></div></div></div>');

                    innerhtml += ('<div class="col-md-4 mb-3"><div class="card">' +
                        '<div class="background-image-contain" style="background-image: url(\'../../Blogs/Plays/' + relationModelObj.thumbnailUrl + '\')"></div><div class="card-body"><a href="/home/plays?objToken=' + relationModelObj.tokenID + '"><h4 class="card-title">' + relationModelObj.title + '</h4>' +
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