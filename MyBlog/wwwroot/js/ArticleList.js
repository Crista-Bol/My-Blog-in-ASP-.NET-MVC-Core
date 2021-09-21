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
            {
                "data": "header",
                "width": "20%"
            },
            {
                "data": "body",
                "render": function (data) {
                    return (data.length > 100 ? data.substring(0, 100) : data);
                },
                "width": "35%"
            },
            {
                "data": "created_Date",
                "render": function (data) {
                    return `${getFormattedDate(data)}`;
                },
                "width": "10%"
            },
            {
                "data": "published_Date",
                "render": function (data) {
                    return `${getFormattedDate(data)}`;  
                },
                "width": "10%"
            },
            {
                "data": "image",
                "render": function (data) {
                    return (data != null && data!='') ? `<span style="color:green;"><i class="fas fa-check"></i></span>` : ``;
                },
                "width": "5%"
            },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="text-center">
                            <a class='btn btn-success text-white style='cursor:pointer; width:70px' href="/Articles/Upsert?id=${data}">Edit</a>
                            &nbsp;
                            <a class='btn btn-danger text-white' style='cursor:pointer; width:70px;' onclick=Delete('/Articles/Delete?id='+${data})>Delete</a>
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

function getFormattedDate(data) {
    if (data != null) {
        var date = new Date(data);
        return formattedDate = date.getDate() + '-' + (date.getMonth() + 1) + '-' + date.getFullYear();
    }
    return '';
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