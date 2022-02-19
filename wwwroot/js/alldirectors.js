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
                    //directordataTable.append('<div class="col-md-3 data-item"><div class="card mb-4 box-shadow">' +
                    //    '<img class="card-img-top" src="/Blogs/Directors/' + relationModelObj.imageURL + '" alt="Director Image" alt-id=' + relationModelObj.id + '>' +
                    //    '<div class="card-body"><p class="card-text">' + relationModelObj.title + '</p><p style="display:none;" alt-id=' + relationModelObj.id + '>' + relationModelObj.description + '</p>' +
                    //    '<div class="d-flex justify-content-between align-items-center">' +
                    //    '<button type="button" class="btn btn-sm btn-outline-primary" onclick="_directorsMaster.fnViewDirector(\'' + relationModelObj.title + '\',' + relationModelObj.id + ')">'
                    //    + '<i class="fas fa-eye"></i> View</button><button type="button" class="btn btn-sm btn-outline-warning">'
                    //    + '<i class="far fa-comments"></i> Contact</button></div></div></div></div>');
                    //commented contact us button
                    directordataTable.append('<div class="col-md-3 data-item"><div class="card mb-4 box-shadow">' +
                        '<img class="card-img-top" src="/Blogs/Directors/' + relationModelObj.imageURL + '" alt="Director Image" alt-id=' + relationModelObj.id + '>' +
                        '<div class="card-body"><p class="card-text">' + relationModelObj.title + '</p><p style="display:none;" alt-id=' + relationModelObj.id + '>' + relationModelObj.description + '</p>' +
                        '<div class="d-flex justify-content-between align-items-center">' +
                        '<button type="button" class="btn btn-sm btn-outline-primary" onclick="_directorsMaster.fnViewDirector(\'' + relationModelObj.title + '\',' + relationModelObj.id + ')">'
                        + '<i class="fas fa-eye"></i> View</button></div></div></div></div>');
                });

            },
            error: function (err) {
                alert(err.statusText);
            }
        });
    },
    fnViewDirector: function (Name, Id) {
        $("#modalViewDirector").modal();
        $('#modalheading').html(Name);
        $('#modalimg').attr('src', $('img[alt-id=' + Id + ']').attr('src'));
        $('#modalp').html($('p[alt-id=' + Id + ']').html());
    }
};