$(document).ready(function () {

    _playsMaster.fnloadData();
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

            fileUpload = $("#sliderFile").get(0);
            files = fileUpload.files;

            // Looping over all files and add it to FormData object
            for (var i = 0; i < files.length; i++) {
                fileData.append('sliderfiles', files[i]);
            }

            // Adding one more key to FormData object
            fileData.append('TITLE', $('#txtplaytitle').val());
            fileData.append('ACTOR', $('#txtplayactor').val());
            fileData.append('DIRECTOR', $('#txtplaydirector').val());
            fileData.append('NUMBER_OF_SHOWS', $('#txtnumberofshow').val());
            fileData.append('PREMIERDATE', $('#txtplaydate').val());
            fileData.append('SYNOPSIS', $('#txtareasynopsis').val());
            fileData.append('TRAILERLINK', $('#txtplaytrailerlink').val());
            fileData.append('WRITER', $('#txtplaywriter').val());

            $.ajax({
                url: '/Account/SavePlay',
                type: "post",
                contentType: false, // Not to set any content header
                processData: false, // Not to process data
                dataType: 'json',
                data: fileData,
                success: function (result) {
                    alert(result.respmessage);
                    $("#modalAddNew").modal();
                    _playsMaster.fnloadData();
                },
                error: function (err) {
                    alert(err.statusText);
                }
            });
        } else {
            alert("FormData is not supported.");
        }
    });

    $('#btnDeleteAll').click(function () {
        _playsMaster.fnDelAllData();
    });
});

_playsMaster = {
    fnloadData: function () {
        $.ajax({
            url: '/Account/GetAllPlays',
            dataType: "json",
            method: 'GET',
            success: function (data) {

                var playdataTable = $('table tbody');
                playdataTable.empty();
                var rowCount = 0;
                $(data.data).each(function (index, relationModelObj) {

                    rowCount++;
                    playdataTable.append('<tr><td>' + rowCount + '</td><td>'
                        + relationModelObj.title + '</td><td>' + relationModelObj.writer + '</td><td>'
                        + relationModelObj.director + '</td><td>' + relationModelObj.actor + '</td><td>'
                        + relationModelObj.dateCreated + '</td><td>' +
                        '<span onclick="_playsMaster.fnDelData_ID(' + relationModelObj.id + ')"><i class="bi bi-trash" name="btnDelete" style="cursor:pointer;" data-bs-toggle="tooltip" title="Delete"></i></span></td></tr>');
                });

            },
            error: function (err) {
                alert(err.statusText);
            }
        });
    },
    fnDelData_ID: function (ID) {
        $.ajax({
            url: '/Account/DeletePlay',
            dataType: "json",
            method: 'get',
            contentType: "application/json; charset=utf-8",
            data: { objID: ID },
            success: function (result) {
                alert(result.respmessage);
                _playsMaster.fnloadData();
            },
            error: function (err) {
                alert(err.statusText);
            }
        });
    },
    fnDelAllData: function () {

        $.ajax({
            url: '/Account/DeleteAllExistingPlays',
            type: "get",
            contentType: false, // Not to set any content header
            processData: false, // Not to process data
            dataType: 'json',
            success: function (result) {
                alert(result.respmessage);
                _playsMaster.fnloadData();
            },
            error: function (err) {
                alert(err.statusText);
            }
        });
    }
};