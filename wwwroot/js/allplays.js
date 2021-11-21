$(document).ready(function () {
    _allPlaysMaster.fnloadData();
});

_allPlaysMaster = {
    fnloadData: function () {
        $.ajax({
            url: '/Home/GetAllPlays',
            dataType: "json",
            method: 'GET',
            success: function (data) {
                
                var playdataTable = $('.dataview');
                playdataTable.empty();

                $(data.data).each(function (index, relationModelObj) {
                    playdataTable.append('<div class="col-md-4"><div class="card mb-4 box-shadow"><img class="card-img-top" src="/Blogs/Plays/' + relationModelObj.thumbnailUrl + '" alt="Play Image">' +
                        '<div class="card-body"><p class="card-text"><h5>' + relationModelObj.title + '</h5>' + relationModelObj.about + '</p><div class="d-flex justify-content-between align-items-center"><div class="btn-group">' +
                        '<a href="/home/plays?objToken=' + relationModelObj.tokenID + '" class="btn btn-sm btn-outline-secondary">View</a><i href="' + relationModelObj.bookUrl + '" class="btn btn-sm btn-outline-secondary">Book</i></div>' +
                        '<small class="text-muted">' + relationModelObj.dateCreated + '</small></div></div></div></div>');
                });

            },
            error: function (err) {
                alert(err.statusText);
            }
        });
    },
};