$(document).ready(function () {
    //load data on the page load
    _profileMaster.fnloadData();
    _profileMaster.fnPopulateDDL();

    $(".ddlRoleType").change(function () {
        _profileMaster.selectedRole = $(this).val();
        _profileMaster.fnloadData();
    });
    $(".ddlCity").change(function () {
        _profileMaster.selectedCity = $(this).val();
        _profileMaster.fnloadData();
    });
    $(".ddlLanguage").change(function () {
        _profileMaster.languageval = $(this).val();
        _profileMaster.fnloadData();
    });
    $(".ddlGender").change(function () {
        _profileMaster.selectedGender = $(this).val();
        _profileMaster.fnloadData();
    });
});
var _profileMaster = {
    selectedCity: '',
    selectedRole: '',
    selectedGender: '',
    fnloadData: function () {
        $.ajax({
            url: '/Home/GetAllProfiles',
            dataType: "json",
            method: 'GET',
            data: { city: _profileMaster.selectedCity, role: _profileMaster.selectedRole, gender: _profileMaster.selectedGender, language: _profileMaster.userlanguages },
            success: function (data) {

                var crewdataTable = $('.data-view');
                crewdataTable.empty();
                $(data.data).each(function (index, relationModelObj) {
                    //crewdataTable.append('<div class="col-md-3 data-item"><div class="card mb-4 box-shadow">' +
                    //    '<img class="card-img-top" src="/Blogs/Directors/' + relationModelObj.imageURL + '" alt="Director Image" alt-id=' + relationModelObj.id + '>' +
                    //    '<div class="card-body"><p class="card-text">' + relationModelObj.title + '</p><p style="display:none;" alt-id=' + relationModelObj.id + '>' + relationModelObj.description + '</p>' +
                    //    '<div class="d-flex justify-content-between align-items-center">' +
                    //    '<button type="button" class="btn btn-sm btn-outline-primary" onclick="_profileMaster.fnViewDirector(\'' + relationModelObj.title + '\',' + relationModelObj.id + ')">'
                    //    + '<i class="fas fa-eye"></i> View</button><button type="button" class="btn btn-sm btn-outline-warning">'
                    //    + '<i class="far fa-comments"></i> Contact</button></div></div></div></div>');
                    //commented contact us button
                    crewdataTable.append('<div class="col-md-3 data-item"><div class="card mb-4 box-shadow">' +
                        '<img class="card-img-top" src="' + (relationModelObj.imageURL ? '/Blogs/ProfileData/' + relationModelObj.imageURL : 'https://mdbcdn.b-cdn.net/img/Photos/new-templates/bootstrap-chat/ava3.webp') + '" alt="Director Image" alt-id=' + relationModelObj.id + '>' +
                        '<div class="card-body"><p class="card-text">' + relationModelObj.usertitle + '</p><p style="display:none;" alt-id=' + relationModelObj.id + '>' + relationModelObj.userrole + '</p>' +
                        '<div class="d-flex justify-content-between align-items-center">' +
                        '<button type="button" class="btn btn-sm btn-outline-primary" onclick="_profileMaster.fnViewProfile_ID(' + relationModelObj.id + ')">'
                        + '<i class="fas fa-eye"></i> View</button></div></div></div></div>');
                });

            },
            error: function (err) {
                alert(err.statusText);
            }
        });
    },
    fnPopulateDDL: function () {
        var ddlObject = '';
        $.ajax({
            url: '/Home/GetAllCities',
            dataType: "json",
            method: 'GET',
            success: function (data) {

                ddlObject = $('.ddlCity');
                ddlObject.empty();
                ddlObject.append('<option value="0">All Cities</option>');
                $(data.data).each(function (index, relationModelObj) {
                    ddlObject.append('<option value="' + relationModelObj.id + '">' + relationModelObj.city + '</option>');
                });

            },
            error: function (err) {
                alert(err.statusText);
            }
        });

        $.ajax({
            url: '/Home/GetAllRoles',
            dataType: "json",
            method: 'GET',
            success: function (data) {
                debugger;
                ddlObject = $('.ddlRoleType');
                ddlObject.empty();
                ddlObject.append('<option value="0">All Roles</option>');
                $(data.data).each(function (index, relationModelObj) {
                    ddlObject.append('<option value="' + relationModelObj.id + '">' + relationModelObj.roleName + '</option>');
                });

            },
            error: function (err) {
                alert(err.statusText);
            }
        });

        $.ajax({
            url: '/Home/GetAllLanguages',
            dataType: "json",
            method: 'GET',
            success: function (data) {

                ddlObject = $('.ddlLanguage');
                ddlObject.empty();
                ddlObject.append('<option value="0">All Languages</option>');
                $(data.data).each(function (index, relationModelObj) {
                    ddlObject.append('<option value="' + relationModelObj.id + '">' + relationModelObj.languageval + '</option>');
                });

            },
            error: function (err) {
                alert(err.statusText);
            }
        });
    },
    fnViewProfile_ID: function (ID) {

        $.ajax({
            url: '/Account/GetProfile_ByID',
            dataType: "json",
            method: 'GET',
            data: { objID: ID },
            success: function (data) {

                $('#modalheading').html(data.data.userrole);

                if (data.data.imageURL)
                    $('[name="profile_picture"]').attr('src', '/Blogs/ProfileData/' + data.data.imageURL);
                else
                    $('[name="profile_picture"]').attr('src', 'https://mdbcdn.b-cdn.net/img/Photos/new-templates/bootstrap-chat/ava3.webp');

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