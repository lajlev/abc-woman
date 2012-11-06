jQuery(window).load(function() {

    /* ******************************************************************
     * Admin View
     */

    $("#select-player").on('change', function() {
        log("fetching player: " + $(this).children(":selected").attr("id"));
        $.ajax({
            type: "get",
            cache: true,
            url: "/Players/Edit/",
            data: { id: $(this).children(":selected").attr("id") },
            success: function (data) {
                log(data);
                $("#player-data").html(data);
            }
        });
    });
});
