FoosBall.service('session', ['$resource', function ($resource) {
    var self = this;
    this.getSession = function (refresh) {
        var url = '/Base/GetSession';
        url += (refresh) ? '?refresh=true' : '';
        var Session = $resource(url);
        var promise = Session.get().$promise;
        return promise;
    };

    this.login = function (requestParameters) {
        var Login = $resource('Account/Logon');
        var login = new Login(requestParameters);
        var loginPromise = login.$save();
        return loginPromise;
    };
    this.autoLogin = function (scope) {
        var AccountLogon = $resource('/Account/Logon'),
        logonPromise = AccountLogon.get().$promise;
        logonPromise.then(function () {
            var sessionPromise = self.getSession();
            sessionPromise.then(function (sessionInfo) {
                window.angular.forEach(sessionInfo, function (value, key) {
                    scope.session[key] = value;
                });
            });
        });
    };
    this.logout = function (scope, callback) {
        var AccountLogoff = $resource('/Account/LogOff'),
            logoffPromise = AccountLogoff.get().$promise;
        logoffPromise.then(function () {
            var sessionPromise = self.getSession();
            sessionPromise.then(function (sessionInfo) {
                window.angular.forEach(sessionInfo, function (value, key) {
                    scope.session[key] = value;
                });
            });
            if (callback) {
                callback();
            }
        });
    };
}]);
