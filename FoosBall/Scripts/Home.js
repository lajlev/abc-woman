jQuery(window).load(function () {

    /* ******************************************************************
     * Home View
     */
    $(function () {
        // Proxy created on the fly          
        var chat = $.connection.chat;

        // Declare a function on the chat hub so the server can invoke it          
        chat.client.addMessage = function (message) {
            $('#messages').append('<li>' + message + '</li>');
        };

        $("#broadcast").click(function () {
            // Call the chat method on the server
            chat.server.send($('#msg').val());
        });

        // Start the connection
        $.connection.hub.start();
    });
});