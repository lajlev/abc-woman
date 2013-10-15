FoosBall.controller('LoginController', ['$scope', '$resource', function($scope, $resource) {
    $scope.loginMessage;
    $scope.showLoginMessage = false;

    $scope.submitLogin = function() {
        var requestParameters = {
            email: $scope.email,
            password: $scope.password,
            rememberMe: $scope.rememberMe || false
        };

        var Login = $resource('Account/Logon');
        var login = new Login(requestParameters);
        var loginPromise = login.$save();

        loginPromise.then(function(responseData) {
            if (!!responseData) {
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