$(document).ready(function () {

    $("#btnUpload").click(function () {
        _scriptMaster.title = $('#txtTitle').val();       
        _scriptMaster.city = $('#ddlplaycity option:selected').val();      
        _scriptMaster.credits = $('#txtCredit').val();
        _scriptMaster.creditslink = $('#txtCreditLink').val();

        // Checking whether FormData is available in browser
        if (window.FormData !== undefined) {

            var fileUpload = $("#fuUploadPdf").get(0);
            var files = fileUpload.files;

            // Create FormData object
            var fileData = new FormData();

            // Looping over all files and add it to FormData object
            for (var i = 0; i < files.length; i++) {
                fileData.append('thumbnailfiles', files[i]);
            }

            // Adding one more key to FormData object
            fileData.append('TITLE', _scriptMaster.title);
            fileData.append('CITY', _scriptMaster.city);           
            fileData.append('CREDITSTITLE', _scriptMaster.credits);
            fileData.append('CREDITSLINK', _scriptMaster.creditslink);

            $.ajax({
                url: '/Account/SaveScript',
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
var _scriptMaster = {
    title: '',
    city: '',   
    credits: '',
    creditslink: ''
};