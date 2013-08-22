angular.module('FoosBall', ['ngResource'])
    .directive('resolvedScore', function () {
        return {
            require: 'ngModel',
            link: function (scope, elm, attrs, ctrl) {
                ctrl.$parsers.unshift(function (viewValue) {
                    ctrl.$setValidity('resolvedScore', false);
                    alert('arrgghhhh!');
                });
            }
        };
    });;
