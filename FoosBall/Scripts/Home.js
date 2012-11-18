jQuery(window).load(function () {

    /* ******************************************************************
     * Home View
     */
    $(function () {
        var connection = $.connection('/Events');

        // Declare a function on the chat hub so the server can invoke it          
        connection.received(function (data) {
            $('#event-feed').append('<li>received: ' + data + '</li>');
        });

        connection.stateChanged(function (change) {
            if (change.newState === $.signalR.connectionState.reconnecting) {
                $('#event-feed').append('<li>stateChanged: re-connecting</li>');
            }
            else if (change.newState === $.signalR.connectionState.connected) {
                $('#event-feed').append('<li>stateChanged: The server is online</li>');
            }
        });

        connection.reconnected(function () {
            $('#event-feed').append('<li>reconnected: reconnected</li>');
        });

        connection.start(function () {
            $('#event-feed').append('<li>start: Connection started</li>');
        });
        


        $("#broadcast").click(function () {
            connection.send($('#msg').val());
        });
    });
});