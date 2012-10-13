jQuery(window).load(function() {
    /* ******************************************************************
     * jQuery Ajax default configuration
     */
    $.ajaxSetup({
        type: 'post',
        error: function(jqHxr, statusText, errorThrown) {
            displayErrorMessage(statusText + ': ' + errorThrown);
        }
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