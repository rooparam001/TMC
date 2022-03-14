$(document).ready(function () {

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

            $.ajax({
                url: '/Account/SaveProfile',
                type: "post",
                contentType: false, // Not to set any content header
                processData: false, // Not to process data
                dataType: 'json',
                data: fileData,
                success: function (result) {
                    alert(result.respmessage);
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
    gender: ''
};