FoosBall.controller('LoginController', ['$scope', 'session', function($scope, session) {
    $scope.loginMessage;
    $scope.showLoginMessage = false;

    $scope.submitLogin = function() {
        var loginPromise = session.login({
            email: $scope.email,
            password: $scope.password,
            rememberMe: $scope.rememberMe || false
        });

        loginPromise.then(function(responseData) {
            if (!responseData) {
                return;
            }
            
            if (responseData.Success === true) {
                angular.forEach(responseData.Data, function (value, key) {
                    $scope.session[key] = value;
                });
                $scope.uiSettings.hideMainMenu = true;
                $scope.uiSettings.hideSignupMenu = true;
                $scope.uiSettings.hideLogonMenu = true;

                clearLogonForm($scope);
            } else {
                $scope.loginMessage = responseData.Message;
                $scope.showLoginMessage = true;
            }
        });
    };
    
    function clearLogonForm(scope) {
        scope.loginMessage;
        scope.showLoginMessage = false;
        scope.email = "";
        scope.name = "";
        scope.password = "";
    }
}]);