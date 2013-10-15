FoosBall.controller('SignupController', ['$scope', '$resource', 'session', function ($scope, $resource, session) {
    $scope.signupMessage;
    $scope.showSignupMessage = false;

    $scope.submitSignup = function() {
        var requestParameters = {
            email: $scope.email,
            name: $scope.name,
            password: $scope.password
        };

        var User = $resource('Account/Register');
        var newUser = new User(requestParameters);
        var newUserPromise = newUser.$save();

        newUserPromise.then(function(responseData) {
            if (!!responseData && responseData.Success === true) {
                $scope.getPlayer();
                var sessionPromise = session.getSession(true);

                sessionPromise.then(function(sessionInfo) {
                    angular.forEach(sessionInfo, function(value, key) {
                        $scope.$parent.session[key] = value;
                    });
                });

            }

            $scope.signupMessage = responseData.Message;
            $scope.showSignupMessage = true;
        });
    };
}]);
