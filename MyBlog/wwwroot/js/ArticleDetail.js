﻿$(document).ready(
    function () {
        load();
    });

function load() {
    $.ajax({
        type: "GET",
        url: "/Articles/commentList?articleId=" + document.getElementById("articleId").value,
        dataType: "json",
        success: function (data) {

            $("#commentList").empty();
            $.each(data.data, function () {
                $("#commentList").prepend(
                    drawEachComment(this.id, this.commenter, this.ip, this.detail,this.date, this.heartVoteNumber, this.brokenHeartVoteNumber)
                );
            });

            //For comment count
            var rowCount = $('#commentList').children('div').length;
            if (rowCount != 0) {

                var comSpanCreated = document.getElementById("commentsNum");
                if (!comSpanCreated) {
                    var comSpan = document.createElement("span");
                    comSpan.setAttribute("id", "commentsNum");
                    comSpan.setAttribute("class", "comments");
                    comSpan.innerHTML = (rowCount == 1) ? rowCount + " comment" : rowCount + " comments";

                    var prevSpan = document.getElementById("readingTime");
                    prevSpan.parentNode.insertBefore(comSpan, prevSpan.nextSibling);
                } else {
                    comSpanCreated.innerHTML = (rowCount == 1) ? rowCount + " comment" : rowCount + " comments";
                }
                
            } else {
                $("#commentsNum").remove();
            }

            //For Reading time
            const text = document.getElementById("article").innerText;
            const wpm = 225;
            const words = text.trim().split(/\s+/).length;
            const time = Math.ceil(words / wpm);
            document.getElementById("readingTime").innerText = time +" min read";
            
        }
    });
}

function deleteComment(id) {
    
    $.ajax({
        type: "Delete",
        url: "/Articles/DeleteComment?id=" + id,
        dataType: "json",
        success: function (data) {
            if (data.success)
                load();
        }
    });
}

function addComment(articleId) {

    var commenter = document.getElementById("cmr").value;
    var detail = document.getElementById("dt").value;
    var articleId = articleId;

    $.ajax({
        type: "POST",
        url: "/Articles/SendComment?commenter=" + commenter + "&detail=" + detail + "&articleId=" + articleId,
        dataType: "json",
        success: function (data) {
            if (data.success)
                load();
        }
    });
    document.getElementById("cmr").value = "";
    document.getElementById("dt").value="";
}

function drawEachComment(id, commenter, ip, detail,date, heartVoteNumber, brokenHeartVoteNumber) {
    return `<div class="comment-box" >
                    <div class="media-heading">${commenter} </div>
                    <div class="media-body"><p>${detail}</p></div>
                    <div class="media-footer">
                        <div class="mr-2"><i class="fas fa-clock mr-1"></i>${getFormattedDate(date)}</div>
                        <div class="mr-2"><a id="giveHeart" onclick=IncHeartNum(${id})> <i class="fas fa-heart" style="color:blue"></i><span id="heartNumber" class="pl-1">${heartVoteNumber}</span></a></div>
                        <div class="mr-2"><a id="giveBrokenHeart" onclick=IncBrokenHeartNum(${id})><i class="fas fa-heart-broken" style="color: red"></i><span class="pl-1">${brokenHeartVoteNumber}</span></a></div>
                        ${(document.getElementById("IsAdmin").value == 1 ? `<div class="mr-2"><a onclick=deleteComment(${id})><i class="fas fa-trash-alt" style="color:red"></i></a></div>`:``)}
                 </div >`;
}
function getFormattedDate(data) {
    if (data != null) {
        var date = new Date(data);
        return formattedDate = date.getHours() + ":" + date.getMinutes() + " " + date.getDate() + '-' + (date.getMonth() + 1) + '-' + date.getFullYear();
    }
    return '';
}
function deleteComment(Id) {

    $.ajax({
        url: "/Articles/DeleteComment?Id=" + Id,
        type: "Delete",
        datatype: "json",
        success: function (data) {
            if (data.success)
                load();
        }
    });
}
function IncHeartNum(id) {
    $.ajax({
        url: "/Articles/GiveHeart?Id=" + id,
        type: "Put",
        datatype: "json",
        success: function (data) {
            if (data.success) {
                load();
            }            
        }
    });
    
}
function IncBrokenHeartNum(id) {
    $.ajax({
        url: "/Articles/GiveBrokenHeart?Id=" + id,
        type: "Put",
        datatype: "json",
        success: function (data) {
            if (data.success) {
                load();
            }
        }
    });

}
