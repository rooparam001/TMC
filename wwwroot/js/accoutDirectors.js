$(document).ready(function () {
    _directorsMaster.fnloadData();
    $('button[name="btnAdd"]').click(function () {
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
            fileData.append('TITLE', $('#txtdirectortitle').val());

            $.ajax({
                url: '/Account/SaveDirector',
                type: "post",
                contentType: false, // Not to set any content header
                processData: false, // Not to process data
                dataType: 'json',
                data: fileData,
                success: function (result) {
                    alert(result.respmessage);
                    $("#modalAddNew").modal('hide');
                    _directorsMaster.fnloadData();
                },
                error: function (err) {
                    alert(err.statusText);
                }
            });
        } else {
            alert("FormData is not supported.");
        }
    });
});
_directorsMaster = {
    fnloadData: function () {
        $.ajax({
            url: '/Account/GetAllDirectors',
            dataType: "json",
            method: 'GET',
            success: function (data) {
                var directordataTable = $('.data-view');
                directordataTable.empty();
                $(data.data).each(function (index, relationModelObj) {
                    directordataTable.append('<div class="col-md-4 data-item"><div class="card mb-4 box-shadow">' +
                        '<img class="card-img-top" src="/Blogs/Directors/' + relationModelObj.imageURL + '" alt="Director Image">' +
                        '<div class="card-body"><p class="card-text">' + relationModelObj.title + '</p><div class="d-flex justify-content-between align-items-center">' +
                        '<button type="button" class="btn btn-sm btn-outline-danger" onclick="_directorsMaster.fnDelData_ID(' + relationModelObj.id + ')">' +
                        '<i class="fas fa-trash"></i></button><small class="text-muted">' + relationModelObj.dateCreated + '</small>' +
                        '</div></div></div></div>');
                });

            },
            error: function (err) {
                alert(err.statusText);
            }
        });
    },
    fnDelData_ID: function (ID) {
        $.ajax({
            url: '/Account/DeleteDirector',
            dataType: "json",
            method: 'get',
            contentType: "application/json; charset=utf-8",
            data: { objID: ID },
            success: function (result) {
                alert(result.respmessage);
                _directorsMaster.fnloadData();
            },
            error: function (err) {
                alert(err.statusText);
            }
        });
    }
};