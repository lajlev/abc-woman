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
     * Player View
     */
    
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