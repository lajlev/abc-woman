FoosBall.controller('StatsController', ['$scope', '$resource', function($scope, $resource) {
    $scope.statsDataReady = false;
    $scope.stats = [];

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
