var dataTable;

$(document).ready(
    function () {
        loadDataTable();
    });


function loadDataTable() {
    
    dataTable = $('#articleList').DataTable({
        "ajax": {
            "url": "/articles/getAll/",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "header", "width": "20%" },
            { "data": "body", "width": "20%" },
            { "data": "created_Date", "width": "20%" },
            { "data": "published", "width": "20%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="text-center">
                            <a class="btn btn-success text-white" href="/Articles/Upsert?id=${data}">Edit</a>
                        </div>
                        <div class="text-center">
                            <a class="btn btn-danger text-white" onclick=Delete('/Articles/Delete?id='+${data})>Delete</a>
                        </div>
                    `
                }, "width": "20%"
            }
        ],
        "language": {
            "emptyTable": "no data found"
        },
        "width": "100%"
    });
    
}

function Delete(url) {
    swal({
        title: "Are you sure?",
        text: "Once delete it, you will not be able to recover it.",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((willdelete) => {
        $.ajax({
            type: "Delete",
            url: url,
            success: function (data) {
                if (data.success) {
                    toastr.success(data.message);
                    dataTable.ajax.reload();
                } else {
                    toastr.error(data.message);
                }
            }
        });

    });
}