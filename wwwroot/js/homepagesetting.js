$(document).ready(function () {

    $('ul.tabs li').click(function () {
        var tab_id = $(this).attr('data-tab');

        $('ul.tabs li').removeClass('current');
        $('.tab-content').removeClass('current');

        $(this).addClass('current');
        $("#" + tab_id).addClass('current');
    });

    _homepageMaster.fnloadData();

    $('.btn-addnewSlider').click(function () {
        $("#modalAddNew").modal();
    });

    $('#btnUpload').click(function () {

        // Checking whether FormData is available in browser
        if (window.FormData !== undefined) {

            var fileUpload = $("#formFile").get(0);
            var files = fileUpload.files;

            // Create FormData object
            var fileData = new FormData();

            // Looping over all files and add it to FormData object
            for (var i = 0; i < files.length; i++) {
                fileData.append('thumbnailfiles', files[i]);
            }


            // Adding one more key to FormData object
            fileData.append('TITLE', $('#txtslidertitle').val());
            fileData.append('DESCRIPTION', $('#txtsliderdescription').val());

            $.ajax({
                url: '/Account/SaveHomePageSlider',
                type: "post",
                contentType: false, // Not to set any content header
                processData: false, // Not to process data
                dataType: 'json',
                data: fileData,
                success: function (result) {
                    alert(result.respmessage);
                    $("#modalAddNew").modal();
                    _homepageMaster.fnloadData();
                },
                error: function (err) {
                    alert(err.statusText);
                }
            });
        } else {
            alert("FormData is not supported.");
        }
    });

})
var _homepageMaster = {
    fnloadData: function () {
        $.ajax({
            url: '/Account/GetAllHomePageSliders',
            dataType: "json",
            method: 'GET',
            success: function (data) {

                var sliderdataTable = $('.slider-data');
                sliderdataTable.empty();
                $(data.data.lst).each(function (index, relationModelObj) {

                    sliderdataTable.append('<div class="col-md-4"><div class="card">' +
                        '<img class="card-img-top" src="/Blogs/Sliders/' + relationModelObj.sliderImgURL + '" alt="Card image" style="width:100%"><div class="card-body">' +
                        '<h4 class="card-title">' + (relationModelObj.description ? relationModelObj.description : 'title not available') + '</h4>' +
                        '<i class="bi bi-trash del-slider float-right" style="cursor:pointer;" data-bs-toggle="tooltip" onClick="_homepageMaster.fnDelData_ID(' + relationModelObj.id + ')" title="Delete">' +
                        '</i></div></div></div>');
                });

            },
            error: function (err) {
                alert(err.statusText);
            }
        });
    },
    fnDelData_ID: function (ID) {
        if (confirm('Do you wish to delete?')) {
            $.ajax({
                url: '/Account/DeleteSlider',
                dataType: "json",
                method: 'get',
                contentType: "application/json; charset=utf-8",
                data: { objID: ID },
                success: function (result) {
                    alert(result.respmessage);
                    _homepageMaster.fnloadData();
                },
                error: function (err) {
                    alert(err.statusText);
                }
            });
        }
    },
};