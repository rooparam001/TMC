$(document).ready(function () {

    $(".email-signup").hide();

    $("#signup-box-link").click(function () {
        $(".email-login").fadeOut(100);
        $(".email-signup").delay(100).fadeIn(100);
        $("#login-box-link").removeClass("active");
        $("#signup-box-link").addClass("active");
    });

    $("#login-box-link").click(function () {
        $(".email-login").delay(100).fadeIn(100);;
        $(".email-signup").fadeOut(100);
        $("#login-box-link").addClass("active");
        $("#signup-box-link").removeClass("active");
    });

    $('input[name="UserName"]').keyup(function () {
        accountMaster.validateUserName($(this).val());
    });

    $('input[name="Email"]').keyup(function () {
        accountMaster.validateUserEmail($(this).val());
    });

    $('input[name="ContactNumber"]').keyup(function () {
        accountMaster.validateUserNumber($(this).val());
    });

    $('input[name="UPassword"]').keyup(function () {
        accountMaster.validateUserPassword($(this).val());
    });

    $('input[name="ConPassword"]').keyup(function () {
        accountMaster.validateUserConfirmPassword($('input[name="UPassword"]').val(), $(this).val());
    });

    $(document).on('click', '#formRegister button[type=submit]', function (e) {

        if (accountMaster.nameError || accountMaster.emailError || accountMaster.contactNumberError || accountMaster.passwordError) {
            alert('Please complete the form');
            e.preventDefault();
        }

    });
});

var accountMaster = {
    emailError: true,
    contactNumberError: true,
    passwordError: true,
    nameError: true,
    validateUserName: function (name) {

        if (name.length == '') {
            $('#errName').show();
            $('#errName').html("field can't be empty.");
            this.nameError = true;
        }
        else if ((name.length < 3) || (name.length > 10)) {
            $('#errName').show();
            $('#errName').html("length of username must be between 3 and 10");
            this.nameError = true;
        }
        else {
            $('#errName').hide();
            this.nameError = false;
        }

    },
    validateUserEmail: function (email) {

        if (email.length == '') {
            $('#errEmail').show();
            $('#errEmail').html("field can't be empty.");
            this.emailError = true;
        }
        else {
            $('#errEmail').hide();
            this.emailError = false;
        }

        if (!this.emailError) {
            let regex = /^([_\-\.0-9a-zA-Z]+)@([_\-\.0-9a-zA-Z]+)\.([a-zA-Z]){2,7}$/;
            let s = email;
            if (regex.test(s)) {
                $('#errEmail').hide();
                accountMaster.emailError = false;
            }
            else {
                $('#errEmail').show();
                $('#errEmail').html("enter correct emailID");
                accountMaster.emailError = true;
            }
        }

        if (!this.emailError) {
            $.get(url = '/account/validateUserEmail', data = email, success = function (data) {
                if (data) {
                    alert('yes');
                } else {
                    alert('false');
                }
            });
        }

    },
    validateUserNumber: function (cNumber) {

        if (cNumber.length == '') {
            $('#errContactNo').show();
            $('#errContactNo').html("field can't be empty.");
            this.contactNumberError = true;
        }
        else {
            $('#errContactNo').hide();
            this.contactNumberError = false;
        }

        if (!this.contactNumberError) {
            let regex = /^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$/;
            let s = cNumber;
            if (regex.test(s)) {
                $('#errContactNo').hide();
                accountMaster.contactNumberError = false;
            }
            else {
                $('#errContactNo').show();
                $('#errContactNo').html("enter correct Mobile Number");
                accountMaster.contactNumberError = true;
            }
        }
    },
    validateUserPassword: function (cPassword) {

        if (cPassword.length == '') {
            $('#errPassword').show();
            $('#errPassword').html("field can't be empty.");
            this.passwordError = true;
        }
        else if ((cPassword.length < 3) || (cPassword.length > 10)) {
            $('#errPassword').show();
            $('#errPassword').html("length of password must be between 3 and 10");
            this.passwordError = true;
        }
        else {
            $('#errPassword').hide();
            this.passwordError = false;
        }

    },
    validateUserConfirmPassword: function (uPassword, cPassword) {

        if (uPassword.length == '') {
            $('#errPassword').show();
            $('#errPassword').html("field can't be empty.");
            this.passwordError = true;
        }
        else if ((uPassword.length < 3) || (uPassword.length > 10)) {
            $('#errPassword').show();
            $('#errPassword').html("length of password must be between 3 and 10");
            this.passwordError = true;
        }
        else {
            $('#errPassword').hide();
            this.passwordError = false;
        }

        if (cPassword.length == '') {
            $('#errConfirmPassword').show();
            $('#errConfirmPassword').html("field can't be empty.");
            this.passwordError = true;
        }
        else if ((cPassword.length < 3) || (cPassword.length > 10)) {
            $('#errConfirmPassword').show();
            $('#errConfirmPassword').html("length of password must be between 3 and 10");
            this.passwordError = true;
        }
        else {
            $('#errConfirmPassword').hide();
            this.passwordError = false;
        }

        if (!this.passwordError) {
            if (uPassword == cPassword) {
                $('#errPassword').hide();
                $('#errConfirmPassword').hide();
                this.passwordError = false;
            } else {
                $('#errConfirmPassword').show();
                $('#errConfirmPassword').html("passwords didn't Match");
                this.passwordError = true;
            }
        }
    }
};