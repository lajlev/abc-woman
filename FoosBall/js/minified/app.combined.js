function BaseController($scope, $resource) {
    $scope.session = {};

    var logonPromise = autoLogin();
    logonPromise.then(function () {
        getSession();
    });
    
    function getSession() {
        var Session = $resource('/Base/GetSession');
        var promise = Session.get().$promise;

        promise.then(function(sessionInfo) {
            angular.forEach(sessionInfo, function(value, key) {
                $scope.session[key] = value;
            });
        });
    };

    function autoLogin() {
        var Logon = $resource('/Account/Logon'),
            promise = Logon.get().$promise;

        return promise;
    };

    $scope.logout = function () {
        var Logon = $resource('/Account/LogOff'),
            promise = Logon.get().$promise;

        promise.then(function() {
            getSession();
        });
    };
}

BaseController.$inject = ['$scope', '$resource'];
function AccountController($scope, $resource) {
    $scope.loginForm = {};

    $scope.submitLogin = function () {
        var Login = $resource('Account/LogOn', {
            email: $scope.loginForm.email,
            password: $scope.loginForm.password,
            rememberMe: $scope.submitLogin
        });
        var promise = Login.save().$promise;

        promise.then(function() {
            
        });
    };
}

AccountController.$inject = ['$scope', '$resource'];
function HomeController() {
    
}

function MatchesController($scope, $resource) {
    var $thisSelect,
        valueBeforeChange,
        currentUserTag = document.getElementById('current-user-id');

    $scope.pageSize = 10;
    $scope.hideForm = true;
    $scope.matches = [];
    $scope.hex_md5 = hex_md5;
    $scope.currentUserId = currentUserTag ? currentUserTag.value : '';

    $scope.cancelMatch = function(index) {
        var match = $scope.matches[index],
            CancelMatch = $resource('Matches/Delete?id=' + match.Id),
            promise = CancelMatch.save().$promise;

        promise.then(function() {
            $scope.matches.splice(index, 1);
            var ReplayMatches = $resource('Admin/ReplayMatches'),
            replayPromise = ReplayMatches.save().$promise;

            replayPromise.then(function () {
                
            });
        });
    };

    $scope.onPlayerSelectFocus = function(event) {
        $thisSelect = $(event.target);
        valueBeforeChange = $thisSelect.find(':selected').val();
    };

    $scope.onPlayerSelectChange = function() {
        var $thisOption = $thisSelect.find(':selected');
        // reset options 
        $.each($('option[value="' + valueBeforeChange + '"]').not($thisOption), function(idx, element) {
            $(element).removeAttr('disabled');
        });
        // if the chosen option is default (empty)
        if (!$thisOption.val() === false) {
            $.each($('option[value="' + $thisOption.val() + '"]').not($thisOption), function(idx, element) {
                $(element).attr('disabled', 'disabled');
            });
        }
        valueBeforeChange = $thisSelect.find(':selected').val();
        writeMatchPredictions();
    };

    // Start fetching players, return a promise
    $scope.getPlayers = function () {
        var Players = $resource('Matches/GetPlayers');
        var promise = Players.query().$promise;

        promise.then(function(players) {
            $scope.players = players;
        });
    };

    // Start fetching matches, return a promise
    $scope.getMatches = function (numberOfMatches, startFromMatch) {
        var num = numberOfMatches || $scope.pageSize;
        var start = startFromMatch || 0;
        var Matches = $resource('Matches/GetMatches?numberOfMatches=' + num + '&startFromMatch=' + start);
        var promise = Matches.query().$promise;

        promise.then(function(matches) {
            angular.forEach(matches, function (value, key) {
                var preparedMatch = prepareMatch(matches[key]);
                $scope.matches.push(preparedMatch);
            });
        });
    };

    $scope.getMatches($scope.pageSize, $scope.matches.length);
    $scope.getPlayers();

    function writeMatchPredictions() {

        var $redTeam = $('#team-red-players'),
            $blueTeam = $('#team-blue-players'),
            $redTeamPrediction = $('.score-prediction', $redTeam),
            $blueTeamPrediction = $('.score-prediction', $blueTeam),
            redPlayer1Rating = $('[name="redPlayer1"]', $redTeam).find(':selected').attr('data-player-rating'),
            redPlayer2Rating = $('[name="redPlayer2"]', $redTeam).find(':selected').attr('data-player-rating'),
            bluePlayer1Rating = $('[name="bluePlayer1"]', $blueTeam).find(':selected').attr('data-player-rating'),
            bluePlayer2Rating = $('[name="bluePlayer2"]', $blueTeam).find(':selected').attr('data-player-rating'),
            redPlayerRatings = parseInt(nvl(redPlayer1Rating, 0)) + parseInt(nvl(redPlayer2Rating, 0)),
            bluePlayerRatings = parseInt(nvl(bluePlayer1Rating, 0)) + parseInt(nvl(bluePlayer2Rating, 0)),
            winnerRating = Math.max(redPlayerRatings, bluePlayerRatings),
            loserRating = Math.min(redPlayerRatings, bluePlayerRatings);

        if (redPlayerRatings !== 0 && bluePlayerRatings !== 0) {
            $.ajax({
                url: 'Matches/GetRating',
                data: {
                    winnerRating: winnerRating,
                    loserRating: loserRating,
                },
                type: 'get',
                success: function (rating) {
                    var roundedRedRating = Math.round(redPlayerRatings),
                        roundedBlueRating = Math.round(bluePlayerRatings),
                        roundedWinnerChance = Math.round(100 * rating.ExpectedScore),
                        roundedLoserChance = Math.round(100 - (100 * rating.ExpectedScore)),
                        roundedWinnerGain = Math.round(rating.RatingModifier),
                        roundedLoserGain = Math.round(rating.KModifier - rating.RatingModifier);

                    if (redPlayerRatings === winnerRating) {
                        $redTeamPrediction.find('.rating').text(roundedRedRating);
                        $redTeamPrediction.find('.chance').text(roundedWinnerChance);
                        $redTeamPrediction.find('.gain').text(roundedWinnerGain);
                        $blueTeamPrediction.find('.rating').text(roundedBlueRating);
                        $blueTeamPrediction.find('.chance').text(roundedLoserChance);
                        $blueTeamPrediction.find('.gain').text(roundedLoserGain);
                    } else {
                        $blueTeamPrediction.find('.rating').text(roundedBlueRating);
                        $blueTeamPrediction.find('.chance').text(roundedWinnerChance);
                        $blueTeamPrediction.find('.gain').text(roundedWinnerGain);
                        $redTeamPrediction.find('.rating').text(roundedRedRating);
                        $redTeamPrediction.find('.chance').text(roundedLoserChance);
                        $redTeamPrediction.find('.gain').text(roundedLoserGain);
                    }

                    $redTeamPrediction.removeClass('hide');
                    $blueTeamPrediction.removeClass('hide');
                }
            });
        } else {
            $redTeamPrediction.addClass('hide');
            $blueTeamPrediction.addClass('hide');
        }
    }

}

MatchesController.$inject = ['$scope', '$resource'];

function SubmitMatchController($scope, $resource) {
    $scope.submitMatch = function () {
        var Match = $resource('Matches/SubmitMatch');
        var promise = Match.save($scope.match).$promise;

        promise.then(function (response) {
            // Reset the match form
            angular.forEach($scope.match, function (value, key) {
                $scope.match[key] = null;
            });
            $('option:disabled').removeAttr('disabled');
            $scope.$parent.hideForm = true;

            if (response.success) {
                var preparedMatch = prepareMatch(response.returnedMatch);
                $scope.matches.unshift(preparedMatch);
            }
        });
    };
}

SubmitMatchController.$inject = ['$scope', '$resource'];

function prepareMatch(match) {
    var time = parseInt(match.GameOverTime.replace(/\D/g, ""));
    var date = new Date(time);

    match.UnixTime = time;
    match.DistributedRating = match.DistributedRating.toString().replace(/(\d{1,2})(\.)(\d{2})(.*)/g, "$1,$3");
    match.GameOverDate = date.toDateString().replace(/(\w{3}) (\w{3}) (\d{2}) (\d{2})(\d{2})/g, date.getDate() + " $2 " + " $5");
    match.GameOverTime = date.toLocaleTimeString().replace(/(\d{2})(\.)(\d{2})(\.)(\d{2})/g, ", $1:$3");
    return match;
}
function PlayersController($scope, $resource) {
    $scope.players = [];
    
    // Start fetching players, return a promise
    $scope.getPlayers = function () {
        var Players = $resource('Players/GetPlayers');
        var promise = Players.query().$promise;

        promise.then(function (players) {
            var preparedPlayer;
            
            angular.forEach(players, function (player, index) {
                preparedPlayer = preparePlayer(player, index);
                $scope.players.push(preparedPlayer);
            });
        });
    };

    $scope.getPlayers();

    function preparePlayer(player, index) {
        player.GravatarUrl = 'http://www.gravatar.com/avatar/' + hex_md5(player.Email) + '?d=mm';
        player.Ranking = index + 1;
        player.Rating1 = parseFloat(player.Rating).toFixed(0);
        player.Rating2 = parseFloat(player.Rating).toFixed(2);
        player.Ratio1 = parseFloat(player.Ratio).toFixed(2);
        player.Ratio2 = parseFloat(player.Ratio).toFixed(4);

        return player;
    }
}

PlayersController.$inject = ['$scope', '$resource'];

function PlayerStatsController($scope, $resource) {
    $scope.playerStats = [];
    $scope.hex_md5 = hex_md5;

    // Start fetching player statistics, return a promise
    $scope.getPlayerStats = function() {
        var search = "?playerId=" + $location.search()["playerId"],
            PlayerStats = $resource('/Stats/GetPlayerStatistics' + search),
            promise = PlayerStats.get().$promise;
        

        promise.then(function(playerStats) {
            $scope.playerStats = preparePlayerStats(playerStats);
        });
    };

    $scope.getPlayerRatingData = function() {
        var search = "?playerId=" + $location.search()["playerId"],
            ChartData = $resource('/Stats/GetPlayerRatingData' + search),
            promise = ChartData.get().$promise,
            preparedChartData;
        
        promise.then(function (chartData) {
            preparedChartData = prepareChartData(chartData);
            renderChart(preparedChartData);
        });
    };

    $scope.getPlayerStatsUrl = function (playerId) {
        return '/#/playerstats?playerId=' + playerId;
    };

    $scope.getPlayerStats();
    $scope.getPlayerRatingData();

    function preparePlayerStats(playerStats) {
        var statsUrl = '/#/playerstats?playerId=';
        playerStats.Player.GravatarUrl = 'http://www.gravatar.com/avatar/' + hex_md5(playerStats.Player.Email) + '?d=mm';
        playerStats.Player.Url = statsUrl + playerStats.Player.Id;
        playerStats.Bff.Player.Url = statsUrl + playerStats.Bff.Player.Id;
        playerStats.Rbff.Player.Url = statsUrl + playerStats.Rbff.Player.Id;
        playerStats.Eae.Player.Url = statsUrl + playerStats.Eae.Player.Id;

        return playerStats;
    }

    function prepareChartData(chartData) {
        var preparedChartData = { DataPoints: [] };

        angular.forEach(chartData.DataPoints, function (value, key) {
            preparedChartData.DataPoints.push([
                new Date(Date.UTC(
                    value.TimeSet[0], // year
                    parseInt(value.TimeSet[1]) - 1, // month (0-based)
                    value.TimeSet[2], // day
                    value.TimeSet[3], // hour
                    value.TimeSet[4], // minute
                    value.TimeSet[5]  // second
                )).toDateString(),
                parseFloat(value.Rating.toPrecision(6))
            ]);
        });

        preparedChartData.MinimumValue = chartData.MinimumValue - 50;
        preparedChartData.MaximumValue = chartData.MaximumValue + 50;
        preparedChartData.PlayerName = chartData.Player.Name;
        preparedChartData.DataPoints[0][0] = 'The "birth" of ' + preparedChartData.PlayerName;

        return preparedChartData;
    }

    function renderChart(chartData) {
        new Highcharts.Chart({
            chart: {
                renderTo: 'player-rating-chart',
                zoomType: 'x',
                spacingRight: 10
            },
            title: {
                text: chartData.PlayerName + 's rating over time'
            },
            subtitle: {
                text: document.ontouchstart === undefined
                        ? 'Click and drag in the plot area to zoom in'
                        : 'Drag your finger over the plot to zoom in'
            },
            xAxis: {
                title: {
                    text: null
                }
            },
            yAxis: {
                title: {
                    text: null
                },
                min: chartData.MinimumValue,
                max: chartData.MaximumValue,
                startOnTick: false,
                showFirstLabel: false
            },
            tooltip: {
                shared: true
            },
            legend: {
                enabled: false
            },
            plotOptions: {
                area: {
                    fillColor: {
                        linearGradient: { x1: 0, y1: 0, x2: 0, y2: 1 },
                        stops: [
                            [0, Highcharts.getOptions().colors[0]],
                            [1, 'rgba(2,0,0,0)']
                        ]
                    },
                    lineWidth: 1,
                    marker: {
                        enabled: false,
                        states: {
                            hover: {
                                enabled: true,
                                radius: 5
                            }
                        }
                    },
                    shadow: false,
                    states: {
                        hover: {
                            lineWidth: 1
                        }
                    }
                }
            },
            series: [{
                type: 'area',
                name: 'Rating',
                data: chartData.DataPoints
            }]
        });
    }
}

PlayerStatsController.$inject = ['$scope', '$resource'];

function StatsController($scope, $resource) {
    $scope.stats = [];
    $scope.hex_md5 = hex_md5;
    
    // Start fetching statistics, return a promise
    $scope.getStats = function () {
        var Stats = $resource('Stats/GetStatistics');
        var promise = Stats.get().$promise;

        promise.then(function (stats) {
            $scope.stats = prepareStats(stats);
        });
    };

    $scope.getPlayerStatsUrl = function (playerId) {
        return '/#/playerstats?playerId=' + playerId;
    };

    $scope.getStats();

    function prepareStats(stats) {
        stats.MostFights.GravatarUrl = 'http://www.gravatar.com/avatar/' + hex_md5(stats.MostFights.Email) + '?d=mm';
        stats.MostWins.GravatarUrl = 'http://www.gravatar.com/avatar/' + hex_md5(stats.MostWins.Email) + '?d=mm';
        stats.MostLosses.GravatarUrl = 'http://www.gravatar.com/avatar/' + hex_md5(stats.MostLosses.Email) + '?d=mm';
        stats.TopRanked.GravatarUrl = 'http://www.gravatar.com/avatar/' + hex_md5(stats.TopRanked.Email) + '?d=mm';
        stats.BottomRanked.GravatarUrl = 'http://www.gravatar.com/avatar/' + hex_md5(stats.BottomRanked.Email) + '?d=mm';
        stats.HighestRatingEver.GravatarUrl = 'http://www.gravatar.com/avatar/' + hex_md5(stats.HighestRatingEver.Email) + '?d=mm';
        stats.LowestRatingEver.GravatarUrl = 'http://www.gravatar.com/avatar/' + hex_md5(stats.LowestRatingEver.Email) + '?d=mm';
        stats.LongestWinningStreak.GravatarUrl = 'http://www.gravatar.com/avatar/' + hex_md5(stats.LongestWinningStreak.Player.Email) + '?d=mm';
        stats.LongestLosingStreak.GravatarUrl = 'http://www.gravatar.com/avatar/' + hex_md5(stats.LongestLosingStreak.Player.Email) + '?d=mm';

        return stats;
    }
}

StatsController.$inject = ['$scope', '$resource'];

angular.
    module('FoosBall', ['ngRoute', 'ngResource']).
    config(['$routeProvider', function($routeProvider) {
        $routeProvider
            .when('/', { templateUrl: '/partials/home.html' })
            .when('/stats', { templateUrl: '/partials/stats.html' })
            .when('/playerstats', { templateUrl: '/partials/playerstats.html' })
            .when('/features', { templateUrl: '/partials/features.html' })
            .when('/matches', { templateUrl: '/partials/matches.html', controller: MatchesController })
            .when('/players', { templateUrl: '/partials/players.html', controller: PlayersController })
            .when('/logon', { templateUrl: '/partials/logon.html', controller: PlayersController })
            .when('/edit-user', { templateUrl: '/partials/edit-user.html', controller: PlayersController })
            //.when('/admin', { templateUrl: '/admin.html', controller: AdminController })
            .otherwise({ redirectTo: '/' });
    }]).
    directive('foosballScore', function () {
        return {
            require: 'ngModel',
            link: function (scope, elm, attrs, ctrl) {
                ctrl.$parsers.unshift(function (viewValue) {
                    var otherScore = document.querySelector('.team-score:not(#' + elm.attr('id') + ')');

                    if (!viewValue || !otherScore.value || viewValue === otherScore.value) {
                        scope.resolveMatchForm.redScore.$setValidity('foosballScore', false);
                        scope.resolveMatchForm.blueScore.$setValidity('foosballScore', false);
                    } else {
                        scope.resolveMatchForm.redScore.$setValidity('foosballScore', true);
                        scope.resolveMatchForm.blueScore.$setValidity('foosballScore', true);
                    }
                    // update scope with the validated value
                    return viewValue;
                });
            }
        };
    }).
    directive('timeLeftToEdit', function ($timeout) {
        return function (scope, element, attributes) {
            if (scope.currentUserId) {
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
                if (scope.match.PlayersHash.indexOf(scope.currentUserId) > -1)
                {
                    updateLater();
                }
            }
        };
    });
