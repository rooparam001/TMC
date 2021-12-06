$(document).ready(function () {
    _directorsMaster.fnloadData();
});
_directorsMaster = {
    fnloadData: function () {
        $.ajax({
            url: '/Home/GetAllDirectors',
            dataType: "json",
            method: 'GET',
            success: function (data) {
                var directordataTable = $('.data-view');
                directordataTable.empty();
                $(data.data).each(function (index, relationModelObj) {
                    directordataTable.append('<div class="col-md-3 data-item"><div class="card mb-4 box-shadow">' +
                        '<img class="card-img-top" src="/Blogs/Directors/' + relationModelObj.imageURL + '" alt="Director Image">' +
                        '<div class="card-body"><p class="card-text">' + relationModelObj.title + '</p><div class="d-flex justify-content-between align-items-center">' +
                        '</div></div></div></div>');
                });

            },
            error: function (err) {
                alert(err.statusText);
            }
        });
    }
};