$(document).ready(function () {
    _listyourprofileMaster.fnViewProfile_ID();

    $("#btnUpload").click(function () {
        _listyourprofileMaster.roleID = $('[name="selectUserRole"] option:selected').val();
        _listyourprofileMaster.fullname = $('[name="txtFullName"]').val();
        _listyourprofileMaster.emailadd = $('[name="txtEmail"]').val();
        _listyourprofileMaster.city = $('[name="ddlplaycity"] option:selected').val();
        _listyourprofileMaster.fieldExcellence = $('[name="txtFieldExcellence"]').val();
        _listyourprofileMaster.prevWorkDet = $('[name="txtPrevWorkDet"]').val();
        _listyourprofileMaster.languages = $('[name="txtLanguages"]').val();
        _listyourprofileMaster.expYrs = $('[name="txtExpYrs"]').val();
        _listyourprofileMaster.typeOf = $('[name="txtTypeOf"]').val();
        _listyourprofileMaster.age = $('[name="txtAge"]').val();
        _listyourprofileMaster.gender = $('[name="selectGender"]').val();
        _listyourprofileMaster.contactNumber = $('[name="txtContactNumber"]').val();

        if (_listyourprofileMaster.typeOf == null || _listyourprofileMaster.typeOf == '') {
            alert("Type of can't be empty.");
        }
        if (_listyourprofileMaster.fullname == null || _listyourprofileMaster.fullname == '') {
            alert("Title can't be empty.");
        }
        if (_listyourprofileMaster.emailadd == null || _listyourprofileMaster.emailadd == '') {
            alert("Email Address can't be empty.");
        }
        if (_listyourprofileMaster.age == null || _listyourprofileMaster.age == '') {
            alert("age can't be empty.");
        }
        if (_listyourprofileMaster.validateUserNumber(_listyourprofileMaster.contactNumber)) {
            // Checking whether FormData is available in browser
            if (window.FormData !== undefined) {

                var fileUpload = $("[name='fuDegree']").get(0);
                var files = fileUpload.files;

                // Create FormData object
                var fileData = new FormData();

                // Looping over all files and add it to FormData object
                for (var i = 0; i < files.length; i++) {
                    fileData.append('fudegree', files[i]);
                }

                fileUpload = $("[name='fuLetterofRef']").get(0);
                files = fileUpload.files;

                // Looping over all files and add it to FormData object
                for (var i = 0; i < files.length; i++) {
                    fileData.append('fuLetterofRef', files[i]);
                }

                fileUpload = $("[name='fuCertificates']").get(0);
                files = fileUpload.files;

                // Looping over all files and add it to FormData object
                for (var i = 0; i < files.length; i++) {
                    fileData.append('fuCertificates', files[i]);
                }

                fileUpload = $("[name='fuAwardsAchiev']").get(0);
                files = fileUpload.files;

                // Looping over all files and add it to FormData object
                for (var i = 0; i < files.length; i++) {
                    fileData.append('fuAwardsAchiev', files[i]);
                }

                fileUpload = $("[name='fuUploadWork']").get(0);
                files = fileUpload.files;

                // Looping over all files and add it to FormData object
                for (var i = 0; i < files.length; i++) {
                    fileData.append('fuUploadWork', files[i]);
                }

                fileUpload = $("[name='formFile']").get(0);
                files = fileUpload.files;

                // Looping over all files and add it to FormData object
                fileData.append('fuProfilePicture', files[0]);

                // Adding one more key to FormData object
                fileData.append('ROLEID', _listyourprofileMaster.roleID);
                fileData.append('FULLNAME', _listyourprofileMaster.fullname);
                fileData.append('EMAILID', _listyourprofileMaster.emailadd);
                fileData.append('CITY', _listyourprofileMaster.city);
                fileData.append('FLDOFEXCELLENCE', _listyourprofileMaster.fieldExcellence);
                fileData.append('PREVWORKDET', _listyourprofileMaster.prevWorkDet);
                fileData.append('LANGUAGES', _listyourprofileMaster.languages);
                fileData.append('EXPYRS', _listyourprofileMaster.expYrs);
                fileData.append('PROFILETYPEOF', _listyourprofileMaster.typeOf);
                fileData.append('USERAGE', _listyourprofileMaster.age);
                fileData.append('USERGENDER', _listyourprofileMaster.gender);

                fileData.append('CONTACTNUMBER', _listyourprofileMaster.contactNumber);

                $.ajax({
                    url: '/Account/SaveProfile',
                    type: "post",
                    contentType: false, // Not to set any content header
                    processData: false, // Not to process data
                    dataType: 'json',
                    data: fileData,
                    success: function (result) {

                        if (result) {
                            alert(result.respmessage);
                        }
                        else
                            alert('Something went wrong, please try again.');
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
var _listyourprofileMaster = {
    roleID: '',
    fullname: '',
    emailadd: '',
    city: '',
    fieldExcellence: '',
    prevWorkDet: '',
    languages: '',
    expYrs: '',
    typeOf: '',
    age: 0,
    gender: '',
    password: '',
    contactNumber: '',
    validateUserNumber: function (cNumber) {
        var resp = true;
        if (cNumber.length == '') {
            alert("Contact number can't be empty.");
            resp = false;
        }

        if (resp) {
            let regex = /^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$/;
            let s = cNumber;
            if (!regex.test(s)) {
                alert("Enter correct Mobile Number");
                resp = false;
            }
        }
        return resp;
    },
    fnViewProfile_ID: function () {

        $.ajax({
            url: '/Account/GetProfileEdit_ByID',
            dataType: "json",
            method: 'GET',
            data: { objID: 0 },
            success: function (data) {

                $("select[name='selectUserRole']").val(data.data.userrole.toLowerCase());
                $("select[name='ddlplaycity']").val(data.data.usercity);
                $("select[name='selectGender']").val(data.data.usergender);
                //if (data.data.imageURL)
                //    $('[name="profile_picture"]').attr('src', '/Blogs/ProfileData/' + data.data.imageURL);
                //else
                //    $('[name="profile_picture"]').attr('src', 'https://mdbcdn.b-cdn.net/img/Photos/new-templates/bootstrap-chat/ava3.webp');

                $('[name="txtFullName"]').val(data.data.usertitle);
                $('[name="txtTypeOf"]').val(data.data.profiletypeof);
                $('[name="txtEmail"]').val(data.data.useremail);
                $('[name="txtAge"]').val(data.data.userage);
                $('[name="txtAge"]').val(data.data.userage);
                $('[name="txtPrevWorkDet"]').html(data.data.userprvworkexp);
                $('[name="txtFieldExcellence"]').html(data.data.userfldofexcellence);
                $('[name="txtLanguages"]').html(data.data.userlanguages);
                $('[name="txtExpYrs"]').html(data.data.usertotalexpinyears);
                $('[name="txtContactNumber"]').html(data.data.contactNumber);
                $('[name="txtLanguages"]').html(data.data.userlanguages);

                var htmlStr = '';
                var arrayOfFiles = [];
                if (data.data.userdegreeurl) {
                    arrayOfFiles = data.data.userdegreeurl.split(",");
                    for (i = 0; i < arrayOfFiles.length; i++) {
                        if (arrayOfFiles[i])
                            htmlStr += '<a href="/Blogs/ProfileData/' + arrayOfFiles[i] + '" target="_blank">File</a>';
                    }
                    $('p[name="profile_degree"]').html(htmlStr);
                }

                arrayOfFiles = [];
                htmlStr = '';
                if (data.data.userletterofref) {
                    arrayOfFiles = data.data.userletterofref.split(",");
                    for (i = 0; i < arrayOfFiles.length; i++) {
                        if (arrayOfFiles[i])
                            htmlStr += '<a href="/Blogs/ProfileData/' + arrayOfFiles[i] + '" target="_blank">File</a>';
                    }
                    $('p[name="profile_letterofref"]').html(htmlStr);
                }

                arrayOfFiles = [];
                htmlStr = '';
                if (data.data.usercertificates) {
                    arrayOfFiles = data.data.usercertificates.split(",");
                    for (i = 0; i < arrayOfFiles.length; i++) {
                        if (arrayOfFiles[i])
                            htmlStr += '<a href="/Blogs/ProfileData/' + arrayOfFiles[i] + '" target="_blank">File</a>';
                    }
                    $('p[name="profile_certification"]').html(htmlStr);
                }

                arrayOfFiles = [];
                htmlStr = '';
                if (data.data.userawards) {
                    arrayOfFiles = data.data.userawards.split(",");
                    for (i = 0; i < arrayOfFiles.length; i++) {
                        if (arrayOfFiles[i])
                            htmlStr += '<a href="/Blogs/ProfileData/' + arrayOfFiles[i] + '" target="_blank">File</a>';
                    }
                    $('p[name="profile_awards"]').html(htmlStr);
                }

                arrayOfFiles = [];
                htmlStr = '';
                if (data.data.useruploadedwork) {
                    arrayOfFiles = data.data.useruploadedwork.split(",");
                    for (i = 0; i < arrayOfFiles.length; i++) {
                        if (arrayOfFiles[i])
                            htmlStr += '<a href="/Blogs/ProfileData/' + arrayOfFiles[i] + '" target="_blank">File</a>';
                    }
                    $('p[name="profile_work"]').html(htmlStr);
                }

            },
            error: function (err) {
                alert(err.statusText);
            }
        });

    }
};