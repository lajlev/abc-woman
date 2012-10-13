jQuery(window).load(function() {

    /* ******************************************************************
     * Player View
     */

    var $playerForm = $('#create-player-form');
    var $wrapper = $('#create-player-form .wrapper');
    $('.open-create-player').on('click', function (e) {
        e.preventDefault();
        $playerForm.toggle();
        $wrapper.slideToggle(500);
    });
    $('.close-create-player').on('click', function (e) {
        e.preventDefault();
        $wrapper.slideToggle(500, function () {
            $playerForm.toggle();
        });
    });

    // Validation
    var $name = $('#Name');
    var $email = $('#Email');
    $('#create-player-button').on('click', function (e) {
        var errm = "";
        if (!!$email.val() === false || !!$name.val() === false) {
            errm = 'Please fill out both name and email';
        }
        if (!!errm === true) {
            e.preventDefault();
            displayErrorMessage(errm);
        }
    });

});
