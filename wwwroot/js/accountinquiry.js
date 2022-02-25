$(document).ready(function () {
    //load data on the page load
    _inquiryMaster.fnloadData();
});
var _inquiryMaster = {
    fnloadData: function () {
        $.ajax({
            url: '/Account/GetAllInquiries',
            dataType: "json",
            method: 'GET',
            success: function (data) {
                
                var playdataTable = $('table tbody');
                playdataTable.empty();
                var rowCount = 0;
                $(data.data).each(function (index, relationModelObj) {

                    rowCount++;
                    playdataTable.append('<tr><td>' + rowCount + '</td><td>'
                        + relationModelObj.username + '</td><td>' + relationModelObj.emailadd + '</td><td>'
                        + relationModelObj.usersubject + '</td><td>' + relationModelObj.usermessage + '</td><td>'
                        + relationModelObj.datecreated + '</td>' +
                        '</tr>');
                });

            },
            error: function (err) {
                alert(err.statusText);
            }
        });
    }
};