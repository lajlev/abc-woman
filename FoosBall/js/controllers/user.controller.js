FoosBall.controller('UserController', ['$scope', '$resource', 'session', function($scope, $resource, session) {
    $scope.user = {};
    $scope.updateMessage;
    $scope.showValidationMessage = false;

    getPlayer();
    
    function getPlayer() {
        var User = $resource('Account/GetUser');
        var promise = User.get().$promise;

        promise.then(function(user) {
            $scope.user = user;
        });
    }

    $scope.submitUserDetails = function() {
        var User = $resource('Account/Edit', {
            email: $scope.user.Email,
            name: $scope.user.Name,
            oldPassword: $scope.user.OldPassword,
            newPassword: $scope.user.NewPassword,
        });

        var promise = User.save().$promise;

        promise.then(function(responseData) {
            if (!!responseData && responseData.Success === true) {
                $scope.getPlayer();
                var sessionPromise = session.getSession(true);

                sessionPromise.then(function(sessionInfo) {
                    angular.forEach(sessionInfo, function(value, key) {
                        $scope.$parent.session[key] = value;
                    });
                });

            }

            $scope.updateMessage = responseData.Message;
            $scope.showValidationMessage = true;
        });
    };
}]);
