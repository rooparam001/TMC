$(document).ready(function () {
    _giveawayMaster.fnloadData();
});
var _giveawayMaster = {
    fnloadData: function () {
        $.ajax({
            url: '/Account/GetAllGiveaways',
            dataType: "json",
            method: 'GET',
            success: function (data) {

                var playdataTable = $('table tbody');
                playdataTable.empty();
                var rowCount = 0;
                $(data.data).each(function (index, relationModelObj) {
                    
                    rowCount++;
                    playdataTable.append('<tr><td>' + rowCount + '</td><td>' +
                        relationModelObj.objtitle + '</td><td>' + relationModelObj.city + '</td><td>' +
                        relationModelObj.datecreated + '</td><td>' + (relationModelObj.isaccepted ? '<span class="badge badge-success">Accepted</span>' : '<span class="badge badge-warning">Pending</span>') + '</td><td>' +
                        (relationModelObj.isaccepted ? '' : '<span onclick="_giveawayMaster.fnAcceptData_ID(' + relationModelObj.id + ')"><i class="fas fa-check-circle" name="btnAccept" style="cursor:pointer;" data-bs-toggle="tooltip" title="Accept"></i></span>') +
                        '&nbsp<span onclick="_giveawayMaster.fnDelData_ID(' + relationModelObj.id + ')">' +
                        '<i class="fas fa-trash-alt" name="btnDelete" style="cursor:pointer;" data-bs-toggle="tooltip" title="Deactivate"></i>' +
                        '</span>&nbsp<span onclick="_giveawayMaster.fnEditGiveAway_ID(' + relationModelObj.id + ')">' +
                        '&nbsp<i class="bi bi-pencil-square" name="btnEdit" style="cursor:pointer;" data-bs-toggle="tooltip" title="Edit"></i>' +
                        '</span></td></tr>');
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
                url: '/Account/DeleteGiveaway',
                dataType: "json",
                method: 'get',
                contentType: "application/json; charset=utf-8",
                data: { objID: ID },
                success: function (result) {
                    alert(result.respmessage);
                    _giveawayMaster.fnloadData();
                },
                error: function (err) {
                    alert(err.statusText);
                }
            });
        }
    },
    fnAcceptData_ID: function (ID) {
        if (confirm('Do you wish to accept?')) {
            $.ajax({
                url: '/Account/AcceptGiveaway',
                dataType: "json",
                method: 'get',
                contentType: "application/json; charset=utf-8",
                data: { objID: ID },
                success: function (result) {
                    alert(result.respmessage);
                    _giveawayMaster.fnloadData();
                },
                error: function (err) {
                    alert(err.statusText);
                }
            });
        }
    },
    fnViewGiveAway_ID: function (ID) {

        $.ajax({
            url: '/Account/GetSingleGiveaway',
            dataType: "json",
            method: 'GET',
            data: { objID: ID },
            success: function (data) {

                $('#modalheading').html(data.data.objtitle);

                $('[name="giveaway_title"]').html(data.data.objtitle);
                $('[name="giveaway_city"]').html(data.data.city);
                $('[name="giveaway_enteredby"]').html(data.data.enteredby);
                $('[name="giveaway_acceptstatus"]').html((data.data.isaccepted ? '<span class="badge badge-success">Accepted</span>' : '<span class="badge badge-warning">Pending</span>'));
                $('[name="giveaway_availability"]').html(data.data.objavailability);
                $('[name="giveaway_contactdetails"]').html(data.data.objcontactdetails);


                var htmlStr = '';
                var arrayOfFiles = [];
                if (data.data.objpictures) {
                    arrayOfFiles = data.data.objpictures.split(",");
                    for (i = 0; i < arrayOfFiles.length; i++) {
                        if (arrayOfFiles[i]) {
                            if (arrayOfFiles[i].indexOf('.pdf'))
                                htmlStr += '<a href="/Blogs/Sliders/' + arrayOfFiles[i] + '" target="_blank""><img class="img-fluid" src="/TMC/Images/blue-circle-document-file.png""/></a>';
                            else
                                htmlStr += '<img class="img-fluid" src="/Blogs/Sliders/' + arrayOfFiles[i] + '"" />';
                        }
                    }
                    $('[name="giveaway_pictures"]').html(htmlStr);
                }

                $("#modalViewGiveAaway").modal();
            },
            error: function (err) {
                alert(err.statusText);
            }
        });
    },
    fnEditGiveAway_ID: function (ID) {
        window.location.href = ('/account/editYourGiveAway/?editobj=' + ID);
    }    
};