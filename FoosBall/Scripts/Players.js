jQuery(window).load(function() {

    /* ******************************************************************
     * Player View
     */

    // Validation
    $('#create-player-button').on('click', function (e) {
        // Trim and get name and email for validation
        var $email = $('#Email').val($('#Email').val().trim());
        var $name = $('#Name').val($('#Name').val().trim());

        var errm = "";
        if (!!$email.val() === false || !!$name.val() === false) {
            errm = 'Please fill out both name and email';
        } else {
            if (/^.+[@]trustpilot\.com$/i.test($email.val()) === false) {
                errm = 'Please provide a valid trustpilot-email for ' + $name.val();
            } 
        }
        if (!!errm === true) {
            e.preventDefault();
            displayErrorMessage(errm);
        }
    });

});
