jQuery(window).load(function () {
    /* ******************************************************************
     * Match View
     */
    var $r1 = $('#red-player-1');
    var $b1 = $('#blue-player-1');
    var $playerSelects = $('.select-player');
    
    $('#create-match-button').on('click', function (e) {
        var errm = "";
        if (!!$r1.val() === false || !!$b1.val() === false) {
            errm = 'Choose at least a Player 1 on each team';
        }
        if (!!errm === true) {
            e.preventDefault();
            displayErrorMessage(errm);
        }
    });

    var valueBeforeChange;
    var $thisSelect;
    
    $playerSelects.on('focus', function (e) {
        $thisSelect = $(e.target);
        valueBeforeChange = $thisSelect.find(':selected').val();
    }).on('change', function () {
        var $thisOption = $thisSelect.find(':selected');

        // reset options 
        $.each($('option[value="' + valueBeforeChange + '"]').not($thisOption), function (idx, element) {
            $(element).removeAttr('disabled');
        });
        // if the chosen option is default (empty) 
        if (!$thisOption.val() === false) {
            $.each($('option[value="' + $thisOption.val() + '"]').not($thisOption), function (idx, element) {
                $(element).attr('disabled','disabled');
            });
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
