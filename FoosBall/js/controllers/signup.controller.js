FoosBall.controller('SignupController', ['$scope', '$resource', 'session', function ($scope, $resource, session) {
    $scope.signupMessage = "";
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
            if (!responseData) {
                return;
            }

            if (responseData.Success === true) {
                var sessionPromise = session.getSession(true);

                sessionPromise.then(function(sessionInfo) {
                    angular.forEach(sessionInfo, function(value, key) {
                        $scope.$parent.session[key] = value;
                    });
                });

                clearSignupForm($scope);
            }

            $scope.signupMessage = responseData.Message;
            $scope.showSignupMessage = true;
        });
    };
    
    function clearSignupForm(scope) {
        scope.signupMessage = "";
        scope.showSignupMessage = false;
        scope.email = "";
        scope.name = "";
        scope.password = "";
    }
}]);
