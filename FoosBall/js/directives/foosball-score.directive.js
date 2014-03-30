FoosBall.directive('foosballScore', function () {
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
});
