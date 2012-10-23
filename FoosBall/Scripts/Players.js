jQuery(window).load(function() {

    /* ******************************************************************
     * Player View
     */

    var $playerForm = $('#create-player-form');
    var $wrapper = $('#create-player-form .wrapper');

    $('.open-create-player').on('click', function (e) {
        e.preventDefault();
        $playerForm.toggle();
        $wrapper.slideToggle(500, function () {
            $name[0].focus();
        });
    });
    
    $('.close-create-player').on('click', function (e) {
        e.preventDefault();
        $wrapper.slideToggle(500, function () {
            $playerForm.toggle();
        });
    });

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
