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
            data: { city: _profileMaster.selectedCity, role: _profileMaster.selectedRole, gender: _profileMaster.selectedGender, language: _profileMaster.languageval },
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
                    //crewdataTable.append('<div class="col-md-3 data-item"><div class="card mb-4 box-shadow">' +
                    //    '<img class="card-img-top" src="' + (relationModelObj.imageURL ? '/Blogs/ProfileData/' + relationModelObj.imageURL : 'https://mdbcdn.b-cdn.net/img/Photos/new-templates/bootstrap-chat/ava3.webp') + '" alt="Director Image" alt-id=' + relationModelObj.id + '>' +
                    //    '<div class="card-body"><p class="card-text">' + relationModelObj.usertitle + '</p><p style="display:none;" alt-id=' + relationModelObj.id + '>' + relationModelObj.userrole + '</p>' +
                    //    '<div class="d-flex justify-content-between align-items-center">' +
                    //    '<button type="button" class="btn btn-sm btn-outline-primary" onclick="_profileMaster.fnViewProfile_ID(' + relationModelObj.id + ')">'
                    //    + '<i class="fas fa-eye"></i> View</button><button type="button" onclick="chatService.fnloadnewgroup(' + relationModelObj.accountID + ')" class="btn btn-sm btn-outline-warning">'
                    //    + '<i class="far fa-comments"></i> Contact</button></div></div></div></div>');
                    var currProfilePic = (relationModelObj.imageURL ? '../../Blogs/ProfileData/' + relationModelObj.imageURL : 'https://mdbcdn.b-cdn.net/img/Photos/new-templates/bootstrap-chat/ava3.webp');
                    crewdataTable.append('<div class="col-md-3 data-item"><div class="card mb-4 box-shadow">' +
                        '<div class="card-img-top background-image-contain" style="background-image: url(\'' + currProfilePic + '\')" alt="backstage crew Image" alt-id=' + relationModelObj.id + '></div>' +
                        '<div class="card-body"><p class="card-text">' + relationModelObj.usertitle + '</p><p style="display:none;" alt-id=' + relationModelObj.id + '>' + relationModelObj.userrole + '</p>' +
                        '<div class="d-flex justify-content-between align-items-center">' +
                        '<button type="button" class="btn btn-sm btn-outline-primary" onclick="_profileMaster.fnViewProfile_ID(' + relationModelObj.id + ')">'
                        + '<i class="fas fa-eye"></i> View</button><button type="button" onclick="chatService.fnloadnewgroup(' + relationModelObj.accountID + ')" class="btn btn-sm btn-outline-warning">'
                        + '<i class="far fa-comments"></i> Contact</button></div></div></div></div>');
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
        window.location.href = "/Home/BackStageProfile?ProfileId=" + ID;
    }
};