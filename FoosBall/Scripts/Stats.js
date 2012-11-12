jQuery(window).load(function () {

    /* ******************************************************************
     * Stats / Player View
     */
    var userId = $("#current-user-id").val();
    if (!!userId) {
        $("." + userId).addClass("current-user");
    }
});
