function BaseController($scope, $resource, $location) {
    $scope.session = {};
    $scope.hideMainMenu = true;
    $scope.hideLogonMenu = true;
    $location.path('/').replace();

    var logonPromise = autoLogin();
    logonPromise.then(function () {
        getSession();
    });

    function getSession() {
        var Session = $resource('/Base/GetSession');
        var promise = Session.get().$promise;

        promise.then(function(sessionInfo) {
            angular.forEach(sessionInfo, function(value, key) {
                $scope.session[key] = value;
            });
        });
    };

    function autoLogin() {
        var Logon = $resource('/Account/Logon'),
            promise = Logon.get().$promise;

        return promise;
    };

    $scope.showLogonMenu = function () {
        $scope.hideLogonMenu = !$scope.hideLogonMenu;
        $scope.hideMainMenu = true;
    };

    $scope.showMainMenu = function () {
        $scope.hideMainMenu = !$scope.hideMainMenu;
        $scope.hideLogonMenu = true;
    };

    $scope.logout = function () {
        var Logon = $resource('/Account/LogOff'),
            promise = Logon.get().$promise;

        promise.then(function() {
            getSession();
        });
    };
}

BaseController.$inject = ['$scope', '$resource', '$location'];