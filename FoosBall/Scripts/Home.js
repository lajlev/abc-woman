jQuery(window).load(function () {

    /* ******************************************************************
     * Home View
     */
    $(function () {
        var connection = $.connection('/Events');

        // Declare a function on the chat hub so the server can invoke it          
        connection.received(function(data) {
            $('#event-feed').append('<li>' + data + '</li>');
        });

        connection.start();

        $("#broadcast").click(function () {
            connection.send($('#msg').val());
        });
    });
});