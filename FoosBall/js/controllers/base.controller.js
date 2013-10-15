FoosBall.controller('BaseController', ['$scope', 'session', function ($scope, session) {
    $scope.session = {};
    $scope.uiSettings = {};
    $scope.uiSettings.hideMainMenu = true;
    $scope.uiSettings.hideLogonMenu = true;
    $scope.uiSettings.hideSignupMenu = true;

    session.autoLogin($scope);
    $scope.logout = function() {
        session.logout($scope);
    };

    $scope.showLogonMenu = function() {
        $scope.uiSettings.hideLogonMenu = !$scope.uiSettings.hideLogonMenu;
        $scope.uiSettings.hideSignupMenu = true;
        $scope.uiSettings.hideMainMenu = true;
    };

    $scope.showSignupMenu = function() {
        $scope.uiSettings.hideSignupMenu = !$scope.uiSettings.hideSignupMenu;
        $scope.uiSettings.hideLogonMenu = true;
        $scope.uiSettings.hideMainMenu = true;
    };

    $scope.showMainMenu = function() {
        $scope.uiSettings.hideMainMenu = !$scope.uiSettings.hideMainMenu;
        $scope.uiSettings.hideLogonMenu = true;
        $scope.uiSettings.hideSignupMenu = true;
    };
}]);
