FoosBall.controller('AdminController', ['$scope', 'api', function ($scope, api) {

    $scope.replayAllMatches = function () {
        var promiseOfReplay = api.replayAllMatches();

        promiseOfReplay.then(function () {
            alert("All matches have been replayed");
        });
    };

    var getAllPlayers = function () {
        var promiseOfPlayers = api.getAllPlayers();

        promiseOfPlayers.then(function (players) {
            $scope.players = players;
        });
    };

    getAllPlayers();
}]);

FoosBall.controller('AppConfigController', ['$scope', 'api', function ($scope, api) {
    $scope.config;

    var getConfig = function () {
        var promiseOfConfig = api.getConfig();

        promiseOfConfig.then(function (config) {
            $scope.config = config;
            $scope.$parent.config = config;
            $scope.config.AdminAccounts = $scope.config.AdminAccounts.toString().replace(/,/g, ", ");
        });
    };

    $scope.setConfig = function () {
        var config = window.angular.copy($scope.config);
        config.AdminAccounts = config.AdminAccounts.replace(/ /g, "").split(",");
        var promiseOfConfig = api.setConfig(config);
        
        promiseOfConfig.then(function (response) {
            if (!response) {
                return;
            }
            
            if (!response.Success) {
                alert(response.Message);
            } else {
                getConfig();
                alert(response.Message);
            }
        });
    };

    getConfig();

}]);
