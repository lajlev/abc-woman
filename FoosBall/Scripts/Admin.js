jQuery(window).load(function() {

    /* ******************************************************************
     * Admin View
     */
    $("#select-player").on('change', function() {
        $.ajax({
            type: "get",
            url: "/Players/Edit/",
            data: { id: $(this).children(":selected").attr("id") },
            success: function (data) {
                $("#player-data").html(data);
            }
        });
    });

    $("#copy-prod-to-staging").on("click", function (e) {
        e.preventDefault();
        toggleOverlay();
        
        $.ajax({
            type: 'get',
            url: '/Admin/CopyProdData/',
            success: function () {
                toggleOverlay();
                alert("Data has been copied.");
            }
        });
    });
});

function toggleOverlay() {
    var overlay = $("#overlay");

    if (overlay.size() === 0) {

        var htmlHeight = $('html').innerHeight();
        $("body").append('<div id="overlay"></div>');

        overlay = $("#overlay");

        overlay.css({
            "height": htmlHeight + "px",
            "background-color": "rgba(0, 0, 0, 0.6)",
            "left": "0",
            "position": "absolute",
            "top": "0",
            "width": "100%",
            "z-index": "1000",
        });

        overlay.append('<img src="/Content/images/ajax-loader.gif"/>');

        overlay.on('click', function () {
            toggleOverlay();
        });

    } else {
        overlay.remove();
    }

}