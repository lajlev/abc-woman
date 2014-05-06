FoosBall.controller('BaseController', ['$scope', 'session', 'appSettings', 'staticResources', function ($scope, session, appSettings, staticResources) {
    $scope.session = {};
    $scope.appSettings = {};
    $scope.staticResources = {};
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
        $scope.appSettings.ready = true;
        $scope.appSettings.AppNameWithEnvironment = getAppNameWithEnvironment($scope.appSettings.AppName, $scope.appSettings.Environment);
    });

    $scope.showLogonMenu = function() {
        $scope.uiSettings.hideLogonMenu = !$scope.uiSettings.hideLogonMenu;
        $scope.uiSettings.hideSignupMenu = true;
        $scope.uiSettings.hideMainMenu = true;
        setTimeout(function () {
            $('input[name="email"]', 'form[name="loginForm"]').focus();
        }, 5);
    };

    $scope.showSignupMenu = function() {
        $scope.uiSettings.hideSignupMenu = !$scope.uiSettings.hideSignupMenu;
        $scope.uiSettings.hideLogonMenu = true;
        $scope.uiSettings.hideMainMenu = true;
        setTimeout(function () {
            $('input[name="email"]', 'form[name="signupForm"]').focus();
        }, 5);
    };

    $scope.showMainMenu = function() {
        $scope.uiSettings.hideMainMenu = !$scope.uiSettings.hideMainMenu;
        $scope.uiSettings.hideLogonMenu = true;
        $scope.uiSettings.hideSignupMenu = true;
    };

    $scope.staticResources.backgroundImageUrl = staticResources.getBackgroundImageUrl($scope);
    $scope.staticResources.footballIconUrl = staticResources.getFootballIconUrl($scope);
    $scope.staticResources.cssUrl = staticResources.getCssUrl($scope);

    function getAppNameWithEnvironment(appName, environment) {
        var env = environment.toLowerCase() === 'production' ? "" : (environment + " ");
        return (env + appName);
    }
}]);
