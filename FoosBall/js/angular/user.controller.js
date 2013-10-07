function UserController($scope, $resource) {
    $scope.user = {};

    $scope.getPlayer = function () {
        var Player = $resource('Account/GetPlayer');
        var promise = Player.get().$promise;

        promise.then(function (player) {
            $scope.user = preparePlayer(player);
        });
    };

    function preparePlayer(player) {
        player.GravatarUrl = 'http://www.gravatar.com/avatar/' + md5.hex_md5(player.Email) + '?d=mm';
        return player;
    }

    $scope.getPlayer();
}

UserController.$inject = ['$scope', '$resource'];
