jQuery(window).load(function () {

    /* ******************************************************************
     * Home View
     */
    window.setInterval("getEventFeed()", 1000);

});

function getEventFeed() {
    $.ajax({
        type: "get",
        cache: true,
        url: "/Home/GetEventFeed",
        success: function(data) {
            $.each(data, function(index, value) {
                log(value);
            });
        }
    });
    
    $("#event-feed").append(".");
}