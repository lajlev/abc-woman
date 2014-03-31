FoosBall.service('appSettings', ['$resource', function ($resource) {
    this.getAppSettings = function (refresh) {
        var url = '/Base/GetAppSettings';
        var Session = $resource(url);
        var promise = Session.get().$promise;
        return promise;
    };
}]);
