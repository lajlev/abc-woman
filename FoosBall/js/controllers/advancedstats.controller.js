FoosBall.controller('AdvancedStatsController', ['$scope', 'api', 'advancedStats', function ($scope, api, advancedStats) {
    $scope.matchesAreReady = false;
    $scope.isStatsCalculated = false;
    
        
    // Get the promise of all players for the select drop downs
    var promiseOfPlayers = api.getExperiencedPlayers(),
        $thisSelect,
        valueBeforeChange;
    
    // Upon request completion
    promiseOfPlayers.then(function (players) {
        $scope.players = players;
    });
    
    // Get the promise of all matches from the api
    var promiseOfMatches = api.getAllMatches();

    // When matches are delivered assign the matches to the scope 
    promiseOfMatches.then(function (arrayOfAllMatches) {
        $scope.matches = arrayOfAllMatches;
        $scope.matchesAreReady = true;
    });
    
    $scope.submitPlayers = function () {
        $scope.stats = advancedStats.get($scope.matches, $scope.participants, $scope.players);
        $scope.isStatsCalculated = true;
    };

    // Save a value for later use
    $scope.onPlayerSelectFocus = function (event) {
        $thisSelect = $(event.target);
        valueBeforeChange = $thisSelect.find(':selected').val();
    };

    // Make sure that a player can only be selected in one select box at a time
    $scope.onPlayerSelectChange = function () {
        var $thisOption = $thisSelect.find(':selected');
        // reset options 
        $.each($('option[value="' + valueBeforeChange + '"]').not($thisOption), function (idx, element) {
            $(element).removeAttr('disabled');
        });
        // if the chosen option is default (empty)
        if (!$thisOption.val() === false) {
            $.each($('option[value="' + $thisOption.val() + '"]').not($thisOption), function (idx, element) {
                $(element).attr('disabled', 'disabled');
            });
        }
        valueBeforeChange = $thisSelect.find(':selected').val();
    };

    $scope.addRotation = function() {
        var target = angular.element('.stat-data-container');
        target.addClass("active");
    };

}]);
