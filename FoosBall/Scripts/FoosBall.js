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
    var $playerForm = $('#create-player-form');
    $('.toggle-create-player').on('click', function (e) {
        e.preventDefault();
        $this = $(this);
        $playerForm.slideToggle();
        if (!$this.hasClass('close')) {
            $this.toggle();
        } else {
            $('.toggle-create-player').not($this).toggle();
        }
    });
    
    /* ******************************************************************
     * Match & MatchResult View
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