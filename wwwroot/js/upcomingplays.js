$(document).ready(function () {

    _upcomingplaysMaster.fnloadData();
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
                fileData.append('files', files[i]);
            }

            // Adding one more key to FormData object
            fileData.append('PLAYDATE', $('#txtplaydate').val());
            fileData.append('PLAYTIME', $('#txtplaytime').val());
            fileData.append('TICKETSBUYLINK', $('#txtplayeventbuylink').val());
            fileData.append('TITLE', $('#txtplaytitle').val());
            fileData.append('VIEWEVENTLINK', $('#txtplayeventlink').val());

            $.ajax({
                url: '/Account/SaveUpcomingPlay',
                type: "post",
                contentType: false, // Not to set any content header
                processData: false, // Not to process data
                dataType: 'json',
                data: fileData,
                success: function (result) {
                    alert(result.respmessage);
                    $("#modalAddNew").modal();
                    _upcomingplaysMaster.fnloadData();
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
        _upcomingplaysMaster.fnDelAllData();
    });
});

_upcomingplaysMaster = {
    fnloadData: function () {
        $.ajax({
            url: '/Account/GetAllUpcomingPlay',
            dataType: "json",
            method: 'GET',
            success: function (data) {

                var playdataTable = $('table tbody');
                playdataTable.empty();
                var rowCount = 0;
                $(data.data).each(function (index, relationModelObj) {
                    rowCount++;
                    playdataTable.append('<tr><td>' + rowCount + '</td><td>'
                        + relationModelObj.title + '</td><td>' + relationModelObj.playdate + '</td><td>' +
                        + relationModelObj.playtime + '</td><td><a href="' + relationModelObj.vieweventlink + '">Link</a></td><td>' +
                        '<a href="' + relationModelObj.ticketsbuylink + '">Link</a></td><td>' +
                        '<span onclick="_upcomingplaysMaster.fnDelData_ID(' + relationModelObj.id + ')"><i class="bi bi-trash" name="btnDelete" style="cursor:pointer;" data-bs-toggle="tooltip" title="Delete"></i></span></td></tr>');
                });

            },
            error: function (err) {
                alert(err.statusText);
            }
        });
    },
    fnDelData_ID: function (ID) {
        $.ajax({
            url: '/Account/DeleteUpcomingPlay',
            dataType: "json",
            method: 'get',
            contentType: "application/json; charset=utf-8",
            data: { objID: ID },
            success: function (result) {
                alert(result.respmessage);
                _upcomingplaysMaster.fnloadData();
            },
            error: function (err) {
                alert(err.statusText);
            }
        });
    },
    fnDelAllData: function () {

        $.ajax({
            url: '/Account/DeleteAllUpcomingPlay',
            type: "get",
            contentType: false, // Not to set any content header
            processData: false, // Not to process data
            dataType: 'json',
            success: function (result) {
                alert(result.respmessage);
                _upcomingplaysMaster.fnloadData();
            },
            error: function (err) {
                alert(err.statusText);
            }
        });
    }
};