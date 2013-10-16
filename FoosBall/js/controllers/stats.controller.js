FoosBall.controller('StatsController', ['$scope', '$resource', function($scope, $resource) {
    $scope.statsDataReady = false;
    $scope.stats = [];
    $scope.hex_md5 = md5.hex_md5;

    // Start fetching statistics, return a promise
    $scope.getStats = function() {
        var Stats = $resource('Stats/GetStatistics');
        var promise = Stats.get().$promise;

        promise.then(function(stats) {
            $scope.stats = stats;
            $scope.statsDataReady = true;
        });
    };

    $scope.getStats();
}]);
