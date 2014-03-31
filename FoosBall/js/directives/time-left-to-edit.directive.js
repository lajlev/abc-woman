FoosBall.directive('timeLeftToEdit', function ($timeout) {
    return function (scope, element, attributes) {
        if (scope.session.User) {
            var timeoutId,
                matchTime = attributes.timeLeftToEdit;

            function getTimeDifference() {
                var currentTime = new Date().getTime(),
                    timeLimit = 1000 * 60 * 5, // equals 5 minutes
                    timeDifference = matchTime - currentTime + timeLimit;

                if (timeDifference <= 0) {
                    element.remove();
                } else {
                    element.removeClass('hide');
                }

                return prettyTime(timeDifference);
            }

            function prettyTime(milliseconds) {
                var seconds = parseInt((milliseconds / 1000) % 60),
                    minutes = parseInt((milliseconds / (1000 * 60)) % 60);

                if (seconds < 10) {
                    seconds = "0" + seconds;
                }

                return minutes + ':' + seconds;
            }

            function updateTime() {
                element.val('Cancel match (' + getTimeDifference() + ')');
            }

            function updateLater() {
                timeoutId = $timeout(function () {
                    updateTime();
                    updateLater();
                }, 1000);
            }

            // listen on DOM destroy (removal) event, and cancel the next UI update
            // to prevent updating time after the DOM element was removed.
            element.on('$destroy', function () {
                $timeout.cancel(timeoutId);
            });

            // Only update if the user is logged in and participated in the match
            if (scope.match.PlayersHash.indexOf(scope.session.User.Id) > -1) {
                updateLater();
            }
        }
    };
});

