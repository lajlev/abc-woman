jQuery(window).load(function() {
    $.globals = {
        errorState: {}
    };
    
    /* ******************************************************************
     * jQuery Ajax default configuration
     */
    $.ajaxSetup({
        type: 'post',
        cache: false,
        error: function (jqHxr, statusText, errorThrown) {
            displayErrorMessage(statusText + ': ' + errorThrown);
        }
    });
    
    /* Menu highlighting */
    var activeTab = $('#page').attr('class');
    if (!!activeTab !== false) {
        var $menu = $('#main-menu');
        $menu.find('li').removeClass('selected');
        $('.' + activeTab, $menu).addClass('selected');
    }
});

/* ******************************************************************
 * Custom js functions
 */
function displayErrorMessage(errorMessage, selector) {
    $.globals.errorState[selector] = true;
    var $container = (!!selector === true) ? $(".validation-message." + selector) : $(".validation-message.All");
    if ($container.size() !== false) {
        $container.html(errorMessage).show();
    } else {
        alert(selector + " error: " + errorMessage);
    }
}

function clearErrorMessage(selector) {
    $.globals.errorState[selector] = false;
    var $container = (!!selector === true) ? $(".validation-message." + selector) : $(".validation-message");
    $container.html("").hide();
}

function log(str) {
    console.log(str);
}

function errorState() {
    var state = false;

    $.each($.globals.errorState, function (key, value) {
        if (value) {
            return state = true;
        }
    });
    
    return state;
}

function toggleOverlay() {
    var overlay = $("#overlay");
    
    if (overlay.size() === 0) {

        var htmlHeight = $('html').innerHeight();
        $("body").append('<div id="overlay"></div>');

        overlay = $("#overlay");

        overlay.css({
            "height": htmlHeight + "px",
            "background-color": "rgba(0, 0, 0, 0.6)",
            "left": "0",
            "position": "absolute",
            "top": "0",
            "width": "100%",
            "z-index": "1000",
        });

        overlay.append('<img src="/Content/images/ajax-loader.gif"/>');

        overlay.on('click', function () {
            toggleOverlay();
        });
        
    } else {
        overlay.remove();
    }

}