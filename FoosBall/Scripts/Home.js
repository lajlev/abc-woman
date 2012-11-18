jQuery(window).load(function () {

    /* ******************************************************************
     * Home View
     */
    $(function () {
        var connection = $.connection('/Events');

        connection.received(function (data) {
            $('#event-feed').prepend('<li>' + data + '</li>');
        });

        connection.start();
    });
});