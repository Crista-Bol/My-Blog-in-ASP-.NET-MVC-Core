var pageCount;
var totalPages = $("#totalPages");

$(document).ready(
    function () {
        pageCount = 1;

        if (pageCount >= totalPages) {
            $("#moreArticles").html("No more articles >>");
            $("#moreArticles").addClass("disabled");
        }

        $("#artDiv").empty();
        load(pageCount);
    });

function load(pageCount) {
    
    $.ajax({
        type: "GET",
        url: "/Articles/moreArticles?pageCount=" + pageCount + "&searchVal=" + $("#search").val(),
        dataType: "json",
        success: function (data) {

            $.each(data.data, function () {
                var words = this.body.trim().split(/\s+/).length;
                var readingTime = Math.ceil(words / 225);

                body = (this.body.length) > 150 ? this.body.substring(0, 150) : this.body;
                pubDate = new Date(this.published_Date);

                $("#artDiv").append(
                    drawArticle(this.image, this.id, this.header, pubDate.toLocaleDateString("en-US"), body, this.comCount, readingTime));
            });
        }
    });
}

$("#moreArticles").on("click", function () {
    pageCount++;
    if (pageCount >= totalPages) {
        $("#moreArticles").html("No more articles >>");
        $("#moreArticles").addClass("disabled");
    }
    load(pageCount);
});

function drawArticle(img, id, header, publishedDate, body, comCount, readingTime) {

    return `<div class="item mb-5">
                            <div class="media">
                                <img class="mr-3 img-fluid post-thumb d-md-flex" src="/img/${img}" alt="image" />
                                <div class="media-body">
                                    <h3 class="title mb-1"><a href="Articles/Detail?Id=${id}">${header}</a></h3>
                                    <div class="meta mb-1"><span class="date">Published ${publishedDate}</span><span class="time">${readingTime} min read</span><span class="comment">${comCount} comments</span ></div >
                                    <div class="intro">
                                        ${body}
                                    </div>
                                    <a class="more-link" href="Articles/Detail?Id=${id}">Read more &rarr;</a>

                                </div>
                            </div>
                        </div>`;
}