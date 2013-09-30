function AccountController($scope) {
    $scope.submitLogin = function () {
        var requestParameters = {
            email: $scope.email,
            password: $scope.password,
            rememberMe: $scope.rememberMe || false
        };

        var requestConfig = {
            url: 'Account/LogOn',
            method: 'post',
            data: requestParameters,
            success: function (loginInfo) {
                angular.forEach(loginInfo.Session, function(value, key) {
                    $scope.session[key] = value;
                });
                $scope.hideMainMenu = true;
                $scope.hideLogonMenu = true;
                $scope.$apply();
            }
        };

        $.ajax(requestConfig);
    };
}

AccountController.$inject = ['$scope'];