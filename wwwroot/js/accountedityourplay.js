$(document).ready(function () {

    var id = fngetUrlVars()['editobj'];
    if (id) {
        _accountplay.fnEditData_ID(id);
    }
    $('#btnUpload').click(function () {

        // Checking whether FormData is available in browser
        if (window.FormData !== undefined) {

            var fileUpload = $("#fucensorcertificate").get(0);
            var files = fileUpload.files;

            // Create FormData object
            var fileData = new FormData();

            // Looping over all files and add it to FormData object
            for (var i = 0; i < files.length; i++) {
                fileData.append('censorcertificate', files[i]);
            }

            fileUpload = $("#fubannerimage").get(0);
            files = fileUpload.files;

            // Looping over all files and add it to FormData object
            for (var i = 0; i < files.length; i++) {
                fileData.append('thumbnailfiles', files[i]);
            }

            fileUpload = $("#fuTechRider").get(0);
            files = fileUpload.files;

            // Looping over all files and add it to FormData object
            for (var i = 0; i < files.length; i++) {
                fileData.append('techrider', files[i]);
            }

            fileUpload = $("#fusliderFile").get(0);
            files = fileUpload.files;

            // Looping over all files and add it to FormData object
            for (var i = 0; i < files.length; i++) {
                fileData.append('sliderfiles', files[i]);
            }

            // Adding one more key to FormData object
            fileData.append('TITLE', $('#txtplaytitle').val());
            fileData.append('GROUPTITLE', $('#txtgrouptitle').val());
            fileData.append('ACTOR', $('#txtplayactor').val());
            fileData.append('WRITER', $('#txtplaywriter').val());
            fileData.append('DIRECTOR', $('#txtplaydirector').val());
            fileData.append('NUMBER_OF_SHOWS', $('#txtnumberofshow').val());
            fileData.append('PREMIERDATE', $('#txtplaydate').val());
            fileData.append('TRAILERLINK', $('#txtplaytrailerlink').val());
            fileData.append('GENRE', $('#txtplaygenre').val());
            fileData.append('LANGUAGE', $('#txtplaylanguage').val());
            fileData.append('CASTNCREDIT', $('#txtplaycastncredit').val());
            fileData.append('SUITABLEFORAGE', $('#txtplayage').val());
            fileData.append('DURATION', $('#txtplayduration').val());
            fileData.append('CITY', $('#ddlplaycity option:selected').val());
            fileData.append('FACEBOOKHANDLEURL', $('#txtfacebook').val());
            fileData.append('INSTAGRAMHANDLEURL', $('#txttwitter').val());
            fileData.append('TWITTERHANDLEURL', $('#txtinstagram').val());
            fileData.append('SYNOPSISFORSOCIALHANDLES', $('#txtareasynopsisSocialHandles').val());
            fileData.append('GROUPINFO', $('#txtareagroupinfo').val());
            fileData.append('PLAYLINK', $('#txtplaylink').val());
            fileData.append('SYNOPSIS', $('#txtareasynopsis').val());
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
var _accountplay = {
    fnEditData_ID: function (ID) {
        $.ajax({
            url: '/Account/GetSinglePlay',
            dataType: "json",
            method: 'get',
            contentType: "application/json; charset=utf-8",
            data: { objID: ID },
            success: function (result) {
                
                if (result.data) {
                    $('#txtplaytitle').val(result.data.title);
                    $('#txtgrouptitle').val(result.data.grouptitle);
                    $('#txtplayactor').val(result.data.actor);
                    $('#txtplaywriter').val(result.data.writer);
                    $('#txtplaydirector').val(result.data.director);
                    $('#txtnumberofshow').val(result.data.numbeR_OF_SHOWS);
                    $('#txtplaydate').val(result.data.premierdate);
                    $('#txtplaytrailerlink').val(result.data.trailerlink);
                    $('#txtplaygenre').val(result.data.genre);
                    $('#txtplaylanguage').val(result.data.langauage);
                    $('#txtplaycastncredit').val(result.data.langauage);
                    $('#txtplayage').val(result.data.agesuitablefor);
                    $('#txtplayduration').val(result.data.duration);
                    $('#ddlplaycity').val(result.data.city);
                    $('#txtfacebook').val(result.data.groupfacebooK_HANDLEURL);
                    $('#txttwitter').val(result.data.grouptwitteR_HANDLEURL);
                    $('#txtinstagram').val(result.data.groupinstagaraM_HANDLEURL);
                    $('#txtareasynopsisSocialHandles').val(result.data.synopsiS_SOCIALHANDLES);
                    $('#GROUPINFO').val(result.data.groupinfo);
                    $('#PLAYLINK').val(result.data.playlink);
                    $('#txtareasynopsis').val(result.data.synopsis);
                    $('#HFID').val(result.data.id);
                }
            },
            error: function (err) {
                returnObj = '';
            }
        });
    }
};