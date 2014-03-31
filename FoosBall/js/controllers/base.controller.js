FoosBall.controller('BaseController', ['$scope', 'session', 'appSettings', function ($scope, session, appSettings) {
    $scope.session = {};
    $scope.appSettings = {};
    $scope.uiSettings = {};
    $scope.uiSettings.hideMainMenu = true;
    $scope.uiSettings.hideLogonMenu = true;
    $scope.uiSettings.hideSignupMenu = true;

    session.autoLogin($scope);
    $scope.logout = function() {
        session.logout($scope);
    };

    var promiseOfAppSettings = appSettings.getAppSettings();
    promiseOfAppSettings.then(function(response) {
        $scope.appSettings = response;
        $scope.appSettings.AppNameWithEnvironment = getAppNameWithEnvironment($scope.appSettings.AppName, $scope.appSettings.Environment);
    });

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

    $scope.getBackgroundImage = function () {
        var remoteUrl = 'https://s3-eu-west-1.amazonaws.com/images.trustpilot.com/static/foosball/background.jpg';
        var localUrl = '/css/foosball-background.jpg';

        if (!$scope.appSettings.Environment || $scope.appSettings.Environment === 'Local') {
            return localUrl;
        } else {
            return remoteUrl;
        }
    };

    $scope.getFootballIcon = function () {
        var remoteUrl = 'https://s3-eu-west-1.amazonaws.com/images.trustpilot.com/static/foosball/icon_football.png';
        var localUrl = '/css/icon_football.png';

        if (!$scope.appSettings.Environment || $scope.appSettings.Environment === 'Local') {
            return localUrl;
        } else {
            return remoteUrl;
        }
    };

    function getAppNameWithEnvironment(appName, environment) {
        var env = environment.toLowerCase() === 'production' ? "" : (environment + " ");
        return (env + appName);
    }
}]);
