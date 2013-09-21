function PlayerStatsController($scope, $resource) {
    $scope.playerStats = [];
    $scope.hex_md5 = hex_md5;

    // Start fetching player statistics, return a promise
    $scope.getPlayerStats = function() {
        var PlayerStats = $resource('/Stats/GetPlayerStatistics' + window.location.search);
        var promise = PlayerStats.get().$promise;

        promise.then(function(playerStats) {
            $scope.playerStats = prepareStats(playerStats);
        });
    };

    $scope.getPlayerRatingData = function() {
        var ChartData = $resource('/Stats/GetPlayerRatingData' + window.location.search);
        var promise = ChartData.get().$promise;

        promise.then(function (chartData) {
            
        });
    };

    $scope.getPlayerStatsUrl = function (playerId) {
        return '/Stats/Player?playerId=' + playerId;
    };

    $scope.getPlayerStats();
    //$scope.getPlayerRatingData();
}

function prepareStats(playerStats) {
    var statsUrl = '/Stats/Player?playerId=';
    playerStats.Player.GravatarUrl = 'http://www.gravatar.com/avatar/' + hex_md5(playerStats.Player.Email) + '?d=mm';
    playerStats.Player.Url = statsUrl + playerStats.Player.Id;
    playerStats.Bff.Player.Url = statsUrl + playerStats.Bff.Player.Id;
    playerStats.Rbff.Player.Url = statsUrl + playerStats.Rbff.Player.Id;
    playerStats.Eae.Player.Url = statsUrl + playerStats.Eae.Player.Id;

    return playerStats;
}