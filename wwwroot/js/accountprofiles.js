$(document).ready(function () {
    //load data on the page load
    _profileMaster.fnloadData();
});
var _profileMaster = {
    fnloadData: function () {
        $.ajax({
            url: '/Account/GetAllProfiles',
            dataType: "json",
            method: 'GET',
            success: function (data) {

                var playdataTable = $('table tbody');
                playdataTable.empty();
                var rowCount = 0;
                $(data.data).each(function (index, relationModelObj) {

                    rowCount++;
                    playdataTable.append('<tr><td>' + rowCount + '</td><td>'
                        + relationModelObj.usertitle + '</td><td>' + relationModelObj.userrole + '</td><td>'
                        + relationModelObj.profiletypeof + '</td><td>' + relationModelObj.useremail + '</td><td>'
                        + relationModelObj.datecreated + '</td><td>' +
                        '<span onclick="_profileMaster.fnDelData_ID(' + relationModelObj.id + ')">' +
                        '<i class="bi bi-person-x-fill" name="btnDelete" style="cursor:pointer;" data-bs-toggle="tooltip" title="Deactivate"></i>' +
                        '</span><span onclick="_profileMaster.fnViewProfile_ID(' + relationModelObj.id + ',' + relationModelObj.usertitle +')">' +
                        '&nbsp <i class="bi bi-eye-fill" name="btnEdit" style="cursor:pointer;" data-bs-toggle="tooltip" title="View"></i>' +
                        '</span></td></tr>');
                });

            },
            error: function (err) {
                alert(err.statusText);
            }
        });
    },
    fnDelData_ID: function (ID) {
        if (confirm('Do you wish to deactivate?')) {
            $.ajax({
                url: '/Account/DeleteProfile',
                dataType: "json",
                method: 'get',
                contentType: "application/json; charset=utf-8",
                data: { objID: ID },
                success: function (result) {
                    alert(result.respmessage);
                    _profileMaster.fnloadData();
                },
                error: function (err) {
                    alert(err.statusText);
                }
            });
        }
    },
    fnViewProfile_ID: function (ID, name) {
        window.location.href = "/Home/BackStageProfile?ProfileId=" + ID + "&ProfileName=" + name;
    }
    
};