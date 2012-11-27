function settingsCtrl($scope, $http) {
    var dataStore = {},
        inputs,
        checkboxes;

    $http.get("Admin/GetConfig").
        success(function (data) {
            dataStore.Settings = JSON.parse(data.Settings);
            dataStore.Users = JSON.parse(data.Users);

            inputs = [
                { label: "Application name", value: dataStore.Settings.Name },
                { label: "Domain name", value: dataStore.Settings.Domain },
                { label: "Admin user email", value: dataStore.Settings.AdminAccount },
            ];

            checkboxes = [
                { label: "Enable Departments", value: dataStore.Settings.RequireDepartment },
                { label: "Enable Domain Validation", value: dataStore.Settings.RequireDomainValidation },
                { label: "Allow one-on-one Matches", value: dataStore.Settings.AllowOneOnOneMatches },
                { label: "Enable Gender Specific Matches", value: dataStore.Settings.GenderSpecificMatches },
            ];

            $scope.inputs = inputs;
            $scope.checkboxes = checkboxes;
        });
}
