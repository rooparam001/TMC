$(document).ready(function () {

    $("#btnUpload").click(function () {
        _listyourprofileMaster.title = $('#txtTitle').val();
        _listyourprofileMaster.availability = $('#txtAvailability').val();
        _listyourprofileMaster.city = $('#ddlplaycity option:selected').val();
        _listyourprofileMaster.contactdetails = $('#txtContactDet').val();

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
            fileData.append('TITLE', _listyourprofileMaster.roleID);
            fileData.append('CITY', _listyourprofileMaster.city);
            fileData.append('AVAILABILITY', _listyourprofileMaster.fieldExcellence);
            fileData.append('CONTACTDETAILS', _listyourprofileMaster.prevWorkDet);

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
var _giveawayMaster = {
    title: '',
    city: '',
    availability: '',
    contactdetails: ''
};