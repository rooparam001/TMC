$(document).ready(function () {

    $('ul.tabs li').click(function () {
        var tab_id = $(this).attr('data-tab');

        $('ul.tabs li').removeClass('current');
        $('.tab-content').removeClass('current');

        $(this).addClass('current');
        $("#" + tab_id).addClass('current');
    });

    _playsMaster.fnloadData();
    $('button[class="btn-addnewSlider"]').click(function () {
        $("#modalAddNew").modal();
    });

    $('#btnUpload').click(function () {

        // Checking whether FormData is available in browser
        if (window.FormData !== undefined) {

            var fileUpload = $("#formFile").get(0);
            var files = fileUpload.files;

            // Create FormData object
            var fileData = new FormData();

            // Looping over all files and add it to FormData object
            for (var i = 0; i < files.length; i++) {
                fileData.append('sliderfiles', files[i]);
            }


            // Adding one more key to FormData object
            fileData.append('TITLE', $('#txtplaytitle').val());
            fileData.append('DESCRIPTION', $('#txtplaytrailerlink').val());
            fileData.append('ID', $('#HFID').val());

            $.ajax({
                url: '/Account/SavePlay',
                type: "post",
                contentType: false, // Not to set any content header
                processData: false, // Not to process data
                dataType: 'json',
                data: fileData,
                success: function (result) {
                    alert(result.respmessage);
                    $("#modalAddNew").modal();
                    _playsMaster.fnloadData();
                },
                error: function (err) {
                    alert(err.statusText);
                }
            });
        } else {
            alert("FormData is not supported.");
        }
    });

})
var _homepageMaster = {

};