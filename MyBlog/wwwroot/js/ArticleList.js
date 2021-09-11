
document.getElementById("sendButton").onclick = function () {
    myFunction()
};

function IncHeartNum(Id) {
    
    $.ajax({
        "url": "/Articles/GiveHeart?Id=" + Id,
        "type": "PUT",
        "datatype": "json",
        "success": function (data) {

            $('#heartNumber').html(data.heartNumber);
        }
    });
}


function loadCommentList() {
    
        $.ajax({
            "url": "/Articles/commentList",
            "type": "GET",
            "datatype": "json",
            "success": function (data) {

                $.each(data.data, function () {
                    $("#commentList").append(
                        `<div class="comment-box">
                            <div class="media-heading">${this.commenter} ${(this.ip!=null)?this.ip:' '}</div>
                            <div class="media-body"><p>${this.detail}</p></div>
                            <div class="media-footer">
                                <div class="mr-2"><i class="fas fa-clock"></i>${getFormattedDate(this.date)}</div>
                                <div class="mr-2"><a id="giveHeart" onclick=IncHeartNum(${this.id})><i class="fas fa-heart" style="color:blue"></i><span id="heartNumber" class="pl-1">${this.heartVoteNumber}</span></a></div>
                                <div class="mr-2"><a id="giveBrokenHeart"><i class="fas fa-heart-broken" style="color: red"></i><span class="pl-1">${this.brokenHeartVoteNumber}</span></a></div>
                               
                            </div>
                        </div>

                         `);
                        
                    
                });       
            }
            
        });
}
function myFunction() {
    var commenter = document.getElementById("Commenter").value;
    var detail = document.getElementById("Detail").value;
    var articleId = document.getElementById("articleId").value;
    $.ajax({
        type: "POST",
        url: "/Articles/SendComment?commenter="+commenter+"&detail=" + detail + "&articleId=" + articleId,
        success: function (data) {
            loadCommentList();
            
        }
    });
}

var dataTable;

$(document).ready(
    function () {
        loadDataTable();
        loadCommentList();
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
            { "data": "body", "width": "35%" },
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