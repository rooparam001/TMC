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
                        '</span><span onclick="_profileMaster.fnViewProfile_ID(' + relationModelObj.id + ')">' +
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
    fnViewProfile_ID: function (ID) {

        $.ajax({
            url: '/Account/GetProfile_ByID',
            dataType: "json",
            method: 'GET',
            data: { objID: ID },
            success: function (data) {

                $('#modalheading').html(data.data.userrole);

                $('[name="profile_title"]').html(data.data.usertitle);
                $('[name="profile_role"]').html(data.data.userrole);
                $('[name="profile_city"]').html(data.data.usercity);
                $('[name="profile_typeof"]').html(data.data.profiletypeof);
                $('[name="profile_email"]').html(data.data.useremail);
                $('[name="profile_fldexcellence"]').html(data.data.userfldofexcellence);
                $('[name="profile_previousworkdetails"]').html(data.data.userprvworkexp);
                $('[name="profile_languages"]').html(data.data.userlanguages);
                $('[name="profile_totexp"]').html(data.data.usertotalexpinyears);


                var htmlStr = '';
                var arrayOfFiles = [];
                if (data.data.userdegreeurl) {
                    arrayOfFiles = data.data.userdegreeurl.split(",");
                    for (i = 0; i < arrayOfFiles.length; i++) {
                        if (arrayOfFiles[i])
                            htmlStr += '<a href="/Blogs/ProfileData/' + arrayOfFiles[i] + '" target="_blank">File</a>';
                    }
                    $('[name="profile_degree"]').html(htmlStr);
                }

                arrayOfFiles = [];
                htmlStr = '';
                if (data.data.userletterofref) {
                    arrayOfFiles = data.data.userletterofref.split(",");
                    for (i = 0; i < arrayOfFiles.length; i++) {
                        if (arrayOfFiles[i])
                            htmlStr += '<a href="/Blogs/ProfileData/' + arrayOfFiles[i] + '" target="_blank">File</a>';
                    }
                    $('[name="profile_letterofref"]').html(htmlStr);
                }

                arrayOfFiles = [];
                htmlStr = '';
                if (data.data.usercertificates) {
                    arrayOfFiles = data.data.usercertificates.split(",");
                    for (i = 0; i < arrayOfFiles.length; i++) {
                        if (arrayOfFiles[i])
                            htmlStr += '<a href="/Blogs/ProfileData/' + arrayOfFiles[i] + '" target="_blank">File</a>';
                    }
                    $('[name="profile_certification"]').html(htmlStr);
                }

                arrayOfFiles = [];
                htmlStr = '';
                if (data.data.userawards) {
                    arrayOfFiles = data.data.userawards.split(",");
                    for (i = 0; i < arrayOfFiles.length; i++) {
                        if (arrayOfFiles[i])
                            htmlStr += '<a href="/Blogs/ProfileData/' + arrayOfFiles[i] + '" target="_blank">File</a>';
                    }
                    $('[name="profile_awards"]').html(htmlStr);
                }

                arrayOfFiles = [];
                htmlStr = '';
                if (data.data.useruploadedwork) {
                    arrayOfFiles = data.data.useruploadedwork.split(",");
                    for (i = 0; i < arrayOfFiles.length; i++) {
                        if (arrayOfFiles[i])
                            htmlStr += '<a href="/Blogs/ProfileData/' + arrayOfFiles[i] + '" target="_blank">File</a>';
                    }
                    $('[name="profile_work"]').html(htmlStr);
                }

                $("#modalViewProfile").modal();
            },
            error: function (err) {
                alert(err.statusText);
            }
        });

    }
};