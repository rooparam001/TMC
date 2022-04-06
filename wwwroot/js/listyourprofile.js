$(document).ready(function () {
    _listyourprofileMaster.togglepages();
    $('#btnNextPrevForm').click(function () { _listyourprofileMaster.togglepages(); });
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
        _listyourprofileMaster.password = $('[name="txtPassword"]').val();
        _listyourprofileMaster.confirmpassword = $('[name="txtConfirmPassword"]').val();
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
            if (_listyourprofileMaster.validatePassword(_listyourprofileMaster.password)) {
                if (_listyourprofileMaster.validatePassword(_listyourprofileMaster.confirmpassword)) {
                    if (_listyourprofileMaster.password == _listyourprofileMaster.confirmpassword) {
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

                            fileData.append('PASSWORD', _listyourprofileMaster.password);
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
                                        if (result.respstatus == 1) {
                                            alert(result.respmessage);
                                        }
                                        else {
                                            alert(result.respmessage);
                                            window.location.href = '/account/verify/';
                                        }
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
                    else
                        alert('Passwords did not Match');
                }
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
    confirmpassword: '',
    contactNumber: '',
    pageno_selected: -1,
    validatePassword: function (cPassword) {
        var resp = true;
        if (cPassword.length == '') {
            alert("Password can't be empty.");
            resp = false;
        }
        else if ((cPassword.length < 3) || (cPassword.length > 10)) {
            alert("length of password must be between 3 and 10");
            resp = false;
        }
        return resp;
    },
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
    togglepages: function () {
        
        if (_listyourprofileMaster.pageno_selected == 0) {
            _listyourprofileMaster.pageno_selected = 1;
            $('.step2').show();
            $('.step1').hide();
            $('.step3').hide();
            $('#btnUpload').show();
            $('#btnNextPrevForm').val('Previous');
            return;
        }
        if (_listyourprofileMaster.pageno_selected == 1) {
            _listyourprofileMaster.pageno_selected = 0;
            $('.step2').hide();
            $('.step1').show();
            $('.step3').show();
            $('#btnUpload').hide();
            $('#btnNextPrevForm').val('Next');
            return;
        }
        if (_listyourprofileMaster.pageno_selected == -1) {
            _listyourprofileMaster.pageno_selected = 0;
            $('.step2').hide();
            $('.step1').show();
            $('.step3').show();
            $('#btnUpload').hide();
            $('#btnNextPrevForm').val('Next');
        }
    }
};