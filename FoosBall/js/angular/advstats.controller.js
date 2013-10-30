function advStatsController($scope, api) {
    // Define an array for the matches in the scope
    $scope.matches = [];

    var promiseOfPlayers = api.getAllPlayers();
    
    promiseOfPlayers.then(function(players) {
        $scope.players = players;
    });

    $scope.submitAdvStats = function() {
        console.log($scope.matchParticipants.player1Id);

        // Get the promise of all matches from the api
        var promiseOfMatches = api.getAllMatches();

        // When matches are delivered assign the matches to the scope 
        promiseOfMatches.then(function (arrayOfAllMatches) {
            $scope.stats = calculateStats(arrayOfAllMatches, $scope.matchParticipants.player1Id, $scope.matchParticipants.player2Id, $scope.matchParticipants.player3Id, $scope.matchParticipants.player4Id);
        });

    }

    function calculateStats(matches, id1, id2, id3, id4) {
        
        angular.forEach(matches, function (match, index) {
            if (match.RedPlayer2.Id == id2) {
                return matches;
            }
        });
    }
    
    $scope.addRotation = function () {
        var target = angular.element('.stat-data-container');
            target.addClass("active");
    };

}



advStatsController.$inject = ['$scope', 'api'];
