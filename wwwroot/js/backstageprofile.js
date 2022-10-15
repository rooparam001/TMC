var getUrlParameter = function getUrlParameter(sParam) {
    var sPageURL = window.location.search.substring(1),
        sURLVariables = sPageURL.split('&'),
        sParameterName,
        i;

    for (i = 0; i < sURLVariables.length; i++) {
        sParameterName = sURLVariables[i].split('=');

        if (sParameterName[0] === sParam) {
            return sParameterName[1] === undefined ? true : decodeURIComponent(sParameterName[1]);
        }
    }
    return false;
};

$(document).ready(function () {
    //load data on the page load
    var Id = getUrlParameter("ProfileId");
    var Name = getUrlParameter("ProfileName");
    _backstageProfile.fnViewProfile_ID(Id,Name);
});
var _backstageProfile = {
    fnViewProfile_ID: function (ID,Name) {

        $.ajax({
            url: '/Account/GetProfile_ByID',
            dataType: "json",
            method: 'GET',
            data: { objID: ID, objName: Name },
            success: function (data) {

                $('#heading').html(data.data.userrole);

                if (data.data.imageURL)
                    $('[name="profile_picture"]').attr('src', '/Blogs/ProfileData/' + data.data.imageURL);
                else
                    $('[name="profile_picture"]').attr('src', 'https://mdbcdn.b-cdn.net/img/Photos/new-templates/bootstrap-chat/ava3.webp');

                $('[name="profile_title"]').html(data.data.usertitle);
                $('[name="profile_role"]').html(data.data.userrole);
                $('[name="profile_city"]').html(data.data.usercity);
                $('[name="profile_typeof"]').html(data.data.profiletypeof);
                $('[name="profile_email"]').html(data.data.useremail);
                $('[name="profile_fldexcellence"]').html(data.data.userfldofexcellence);
                $('[name="profile_previousworkdetails"]').html(data.data.userprvworkexp);
                $('[name="profile_languages"]').html(data.data.userlanguages);
                $('[name="profile_totexp"]').html(data.data.usertotalexpinyears);
                


                var htmlStr = '';
                var arrayOfFiles = [];
                if (data.data.userdegreeurl) {
                    arrayOfFiles = data.data.userdegreeurl.split(",");
                    for (i = 0; i < arrayOfFiles.length; i++) {
                        if (arrayOfFiles[i])
                            htmlStr += '<a href="/Blogs/ProfileData/' + arrayOfFiles[i] + '" target="_blank">File</a>';
                    }
                    $('[name="profile_degree"]').html(htmlStr);
                }

                arrayOfFiles = [];
                htmlStr = '';
                if (data.data.userletterofref) {
                    arrayOfFiles = data.data.userletterofref.split(",");
                    for (i = 0; i < arrayOfFiles.length; i++) {
                        if (arrayOfFiles[i])
                            htmlStr += '<a href="/Blogs/ProfileData/' + arrayOfFiles[i] + '" target="_blank">File</a>';
                    }
                
                    $('[name="profile_letterofref"]').html(htmlStr);
                }

                arrayOfFiles = [];
                htmlStr = '';
                if (data.data.usercertificates) {
                    arrayOfFiles = data.data.usercertificates.split(",");
                    for (i = 0; i < arrayOfFiles.length; i++) {
                        if (arrayOfFiles[i])
                            htmlStr += '<a href="/Blogs/ProfileData/' + arrayOfFiles[i] + '" target="_blank">File</a>';
                    }
                    $('[name="profile_certification"]').html(htmlStr);
                }

                arrayOfFiles = [];
                htmlStr = '';
                if (data.data.userawards) {
                    arrayOfFiles = data.data.userawards.split(",");
                    for (i = 0; i < arrayOfFiles.length; i++) {
                        if (arrayOfFiles[i])
                            htmlStr += '<a href="/Blogs/ProfileData/' + arrayOfFiles[i] + '" target="_blank">File</a>';

                    }
                    $('[name="profile_awards"]').html(htmlStr);
                }

                arrayOfFiles = [];
                htmlStr = '';
                if (data.data.useruploadedwork) {
                    arrayOfFiles = data.data.useruploadedwork.split(",");
                    for (i = 0; i < arrayOfFiles.length; i++) {
                        if (arrayOfFiles[i])
                            htmlStr += '<a href="/Blogs/ProfileData/' + arrayOfFiles[i] + '" target="_blank">File</a>';
                    }
                    $('[name="profile_work"]').html(htmlStr);
                }

                if (data.data.useruploadedwork) {
                    arrayOfFiles = data.data.useruploadedwork.split(",");
                    for (i = 0; i < arrayOfFiles.length; i++) {
                        if (arrayOfFiles[i])
                            htmlStr += '<a href="/Blogs/ProfileData/' + arrayOfFiles[i] + '" target="_blank">File</a>';
                    }
                    $('[name="profile_work"]').html(htmlStr);
                }

                if (data.data.workprofile) {
                    arrayOfFiles = data.data.workprofile.split("\n");
                    for (i = 0; i < arrayOfFiles.length; i++) {
                        if (arrayOfFiles[i].trim() != "") {
                            arrayOfFileSubArray = arrayOfFiles[i].split(",")
                            for (j = 0; j < arrayOfFileSubArray.length; j++) {
                                if (arrayOfFileSubArray[j].trim() != "")
                                    htmlStr += '<a class="workLinks" href="' + arrayOfFileSubArray[j] + '" target="_blank">' + arrayOfFileSubArray[j] + '</a>';
                            }
                        }
                    }
                    $('[name="links_work"]').html(htmlStr);
                }

               // $("#modalViewProfile").modal();
            },
            error: function (err) {
                alert(err.statusText);
            }
        });

    }
}