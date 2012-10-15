jQuery(window).load(function () {
    /* ******************************************************************
     * Match View
     */
    var $r1 = $('#red-player-1');
    var $b1 = $('#blue-player-1');
    var $playerSelects = $('.select-player');
    $r1.focus();
    
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

    $playerSelects.on('change', function(e) {
        var $thisSelect = $(e.target);
        var $thisOption = $thisSelect.find(':selected');
        var $allOptions = $('.select-player > option').not(':first-child');
        console.log('SelectBox: ' + $thisSelect.attr('id')+ ' Option: '+$thisOption.val());
        $.each($allOptions, function (idx,htmlElement) {
            console.log(htmlElement.value);
        });
        
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
