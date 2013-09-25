function AccountController($scope, $http) {
    $scope.submitLogin = function () {
        $.ajax({
            type: 'post',
            url: '/Account/LogOn',
            data: {},
            success: function() {
                console.log('we did it!');
            }
        });

    };
}

AccountController.$inject = ['$scope', '$resource'];