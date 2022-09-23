$(document).ready(function () {
    var id = fngetUrlVars()['editobj'];
    if (id) {
        _giveawayMaster.fnViewGiveAway_ID(id);
    }
    $("#btnUpload").click(function () {
        _giveawayMaster.title = $('#txtTitle').val();
        _giveawayMaster.availability = $('#txtAvailability').val();
        _giveawayMaster.city = $('#ddlplaycity option:selected').val();
        _giveawayMaster.contactdetails = $('#txtContactDet').val();
        _giveawayMaster.credits = $('#txtCredit').val();
        _giveawayMaster.creditslink = $('#txtCreditLink').val();

        // Checking whether FormData is available in browser
        if (window.FormData !== undefined) {

            var fileUpload = $("#fuUploadPictures").get(0);
            var files = fileUpload.files;

            // Create FormData object
            var fileData = new FormData();

            // Looping over all files and add it to FormData object
            for (var i = 0; i < files.length; i++) {
                fileData.append('thumbnailfiles', files[i]);
            }

            // Adding one more key to FormData object
            fileData.append('TITLE', _giveawayMaster.title);
            fileData.append('CITY', _giveawayMaster.city);
            fileData.append('AVAILABILITY', _giveawayMaster.availability);
            fileData.append('CONTACTDETAILS', _giveawayMaster.contactdetails);
            fileData.append('CREDITSTITLE', _giveawayMaster.credits);
            fileData.append('CREDITSLINK', _giveawayMaster.creditslink);

            $.ajax({
                url: '/Account/SaveGiveaway',
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
var _giveawayMaster = {
    fnViewGiveAway_ID: function (ID) {

        $.ajax({
            url: '/Account/GetSingleGiveaway',
            dataType: "json",
            method: 'GET',
            data: { objID: ID },
            success: function (data) {
                $('#txtTitle').val(data.data.objtitle);
                $('#ddlplaycity').val(data.data.city);
                $('#txtAvailability').val(data.data.enteredby);
                $('#txtCreditLink').val(data.data.creditslink);
                $('txtCredit').val(data.data.credits);
                $('txtContactDet').val(data.data.objcontactdetails);


               
            },
            error: function (err) {
                alert(err.statusText);
            }
        });
    }
};