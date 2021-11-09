$(document).ready(function () {
    $.ajax({
        url: '/Account/GetAllUpcomingPlay',
        dataType: "json",
        method: 'GET',
        success: function (data) {

            var playdataTable = $('.dataview');
            playdataTable.empty();
            $(data.data).each(function (index, relationModelObj) {
                playdataTable.append('<div class="col-sm-12 col-md-6 col-lg-4"><div class="card"><img src="/Blogs/UpcomingPlays/' + relationModelObj.imageurl + '" class="card-img-top" alt="' + relationModelObj.title + '">' +
                    '<div class="card-body"><h5 class="card-title">' + relationModelObj.title + '</h5><p class="card-text">' + relationModelObj.playdate + ' | ' + relationModelObj.playtime + '</p></div><ul class="list-group list-group-flush">' +
                    '<li class="list-group-item"><a href="' + relationModelObj.vieweventlink + '" target="_blank" class="card-link">View the event</a></li>' +
                    '<li class="list-group-item"><a href="' + relationModelObj.ticketsbuylink + '" target="_blank" class="card-link">Get the tickets</a></li></ul></div></div>');
            });

        },
        error: function (err) {
            alert(err.statusText);
        }
    });
});