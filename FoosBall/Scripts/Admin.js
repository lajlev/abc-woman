jQuery(window).load(function() {

    /* ******************************************************************
     * Admin View
     */

    $("#select-player").on('change', function() {
        $.ajax({
            type: "get",
            cache: true,
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
            cache: true,
            url: '/Admin/CopyProdData/',
            success: function () {
                toggleOverlay();
                alert("Data has been copied.");
            }
        });
    });
    
    $("#copy-prod-to-local").on("click", function (e) {
        e.preventDefault();
        toggleOverlay();
        
        $.ajax({
            cache: true,
            url: '/Admin/CopyProdData/',
            data: { environment: "Local" },
            success: function () {
                toggleOverlay();
                alert("Data has been copied.");
            }
        });
    });
});