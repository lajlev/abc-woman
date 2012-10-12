jQuery(window).load(function () {
    /* ******************************************************************
     * jQuery Ajax default configuration
     */
    $.ajaxSetup({
        type: 'post',
        error: function (jqHxr, statusText, errorThrown){
            displayErrorMessage(statusText+': '+errorThrown);
        }
    });
    
    /* ******************************************************************
     * Player View
     */
    //var $playerForm = $('#create-player-form');
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
    $('#create-player-button').on('click',function (e) {
        var errm = "";
        if (!!$email.val() === false || !!$name.val() === false) {
            errm = 'Please fill out both name and email';
        }
        if (!!errm === true) {
            e.preventDefault();
            displayErrorMessage(errm);
        }
    });

    /* ******************************************************************
     * Match View
     */
    var $r1 = $('#red-player-1');
    var $b1 = $('#blue-player-1');
    $('#create-match-button').on('click', function (e) {
        var errm = "";
        if (!!$r1.val() === false || !!$b1.val() === false) {
            errm = 'Please choose at least 2 Players';
        }
        if (!!errm === true) {
            e.preventDefault();
            displayErrorMessage(errm);
        }
    });
    /* ******************************************************************
     * SaveMatchResult View
     */
    var $teamRedScore = $('#team-red-score');
    $('#team-red-score-slider').on('change', function (e) {
        $teamRedScore.val(e.target.value);
    });
    var $teamBlueScore = $('#team-blue-score');
    $('#team-blue-score-slider').on('change', function (e) {
        $teamBlueScore.val(e.target.value);
    });

});

/* ******************************************************************
 * Custom js functions
 */
function displayErrorMessage(errorMessage) {
    if ($("#validation-message").size() !== false) {
        $("#validation-message").html(errorMessage + "<br/>");
    } else {
        alert(errorMessage);
    }
    return false; // stop 
}