$(document).ready(function () {
    $('input[name="btnSubmit"]').click(function () {
        _jointherevolution.varamt = $('input[name="txtdonateamt"]').val();
        _jointherevolution.varcityid = $('#ddlCity option:selected').val();
        _jointherevolution.varfullname = $('input[name="txtfullname"]').val();
        _jointherevolution.varemailid = $('input[name="txtemailid"]').val();
        _jointherevolution.varcontactnumber = $('input[name="txtcontactnumber"]').val();

        if (_jointherevolution.validateUserEmail(_jointherevolution.varemailid)) {
            if (_jointherevolution.validateUserName(_jointherevolution.varfullname)) {
                if (_jointherevolution.validateUserNumber(_jointherevolution.varcontactnumber)) {

                    // Checking whether FormData is available in browser
                    if (window.FormData !== undefined) {

                        // Create FormData object
                        var fileData = new FormData();

                        // Adding one more key to FormData object
                        fileData.append('CITY', _jointherevolution.varcityid);
                        fileData.append('CONTACTNUMBER', _jointherevolution.varcontactnumber);
                        fileData.append('TOTAMOUNT', _jointherevolution.varamt);
                        fileData.append('FULLNAME', _jointherevolution.varfullname);
                        fileData.append('EMAILID', _jointherevolution.varemailid);

                        $.ajax({
                            url: '/Home/SaveDonation',
                            type: "post",
                            contentType: false, // Not to set any content header
                            processData: false, // Not to process data
                            dataType: 'json',
                            data: fileData,
                            success: function (result) {
                                if (result.respstatus == 0) {
                                    if (result.data.id > 0) {
                                        _jointherevolution.pKey = result.data.pKey;
                                        _jointherevolution.pID = result.data.pid;

                                        var options = {
                                            "key": _jointherevolution.pID, // Enter the Key ID generated from the Dashboard
                                            "amount": _jointherevolution.fnappendAmt, // Amount is in currency subunits. Default currency is INR. Hence, 50000 refers to 50000 paise
                                            "currency": "INR",
                                            "name": _jointherevolution.varfullname,
                                            "description": "Test Transaction",
                                            "image": "https://www.theatremanagementcompany.com/TMC/Images/White%20logo.png",
                                            "order_id": _jointherevolution.pKey, //This is a sample Order ID. Pass the `id` obtained in the response of Step 1
                                            "handler": function (response) {
                                                _jointherevolution.fnSaveSuccess_Razorpay(response);
                                            },
                                            "prefill": {
                                                "name": _jointherevolution.varfullname,
                                                "email": _jointherevolution.varemailid,
                                                "contact": _jointherevolution.varcontactnumber
                                            },
                                            "notes": {
                                                "address": "TMC"
                                            },
                                            "theme": {
                                                "color": "#3399cc"
                                            }
                                        };
                                        var rzp1 = new Razorpay(options);
                                        rzp1.on('payment.failed', function (response) {
                                            _jointherevolution.fnSaveError_Razorpay(response.error);
                                        });
                                        rzp1.open();
                                    }
                                }
                                else
                                    alert(result.respmessage);
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
                    alert('Please input valid Contact Number');
            }
            else
                alert('Please input valid Name');
        }
        else
            alert('Please input valid email-ID');
    });
});


var _jointherevolution = {
    varamt: 0,
    varcityid: '',
    varfullname: '',
    varemailid: '',
    varcontactnumber: '',
    pID: '',
    pKey: '',
    fnappendAmt: function (amt) {
        if (!isNaN(amt)) {
            $('input[name="txtdonateamt"]').val(amt);
        }
    },
    validateUserName: function (name) {

        if (name.length == '') {
            return false;
        }

        return true;
    },
    validateUserEmail: function (email) {
        if (email.length == '') {
            return false;
        }

        if (!this.emailError) {
            let regex = /^([_\-\.0-9a-zA-Z]+)@([_\-\.0-9a-zA-Z]+)\.([a-zA-Z]){2,7}$/;
            let s = email;
            if (regex.test(s)) {
                return true;
            }
            else {
                return false;
            }
        }
    },
    validateUserNumber: function (cNumber) {

        if (cNumber.length == '') {
            return false;
        }

        if (!this.contactNumberError) {
            let regex = /^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$/;
            let s = cNumber;
            if (regex.test(s)) {
                return true;
            }
            else {
                return false;
            }
        }
    },
    fnSaveSuccess_Razorpay: function (response) {
        
        // Create FormData object
        var fileData = new FormData();

        // Adding one more key to FormData object
        fileData.append('razorpay_payment_id', response.razorpay_payment_id);
        fileData.append('razorpay_signature', response.razorpay_signature);
        fileData.append('razorpay_order_id', response.razorpay_order_id);

        $.ajax({
            url: '/Home/SavePayment',
            type: "post",
            contentType: false, // Not to set any content header
            processData: false, // Not to process data
            dataType: 'json',
            data: fileData,
            success: function (result) {

                $('.pay-area').hide();
                var dispresult = false;
                if (result) {
                    if (result.respstatus == 0) {
                        dispresult = true;
                    }
                }

                if (dispresult)
                    $('.pay-success').show();
                else
                    $('.pay-failure').show();

            },
            error: function (err) {
                returnObj = '';
            }
        });
    },
    fnSaveError_Razorpay: function (error) {

        // Create FormData object
        var fileData = new FormData();

        // Adding one more key to FormData object
        fileData.append('code', error.code);
        fileData.append('description', error.description);
        fileData.append('source', error.source);
        fileData.append('step', error.step);
        fileData.append('reason', error.reason);
        fileData.append('razorpay_order_id', error.metadata.order_id);
        fileData.append('razorpay_payment_id', error.metadata.payment_id);

        $.ajax({
            url: '/Home/SavePayment',
            type: "post",
            contentType: false, // Not to set any content header
            processData: false, // Not to process data
            dataType: 'json',
            data: fileData,
            success: function (result) {

                $('.pay-area').hide();
                var dispresult = false;
                if (result) {
                    if (result.respstatus == 0) {
                        dispresult = true;
                    }
                }

                if (dispresult)
                    $('.pay-success').show();
                else
                    $('.pay-failure').show();

            },
            error: function (err) {
                returnObj = '';
            }
        });
    }
};