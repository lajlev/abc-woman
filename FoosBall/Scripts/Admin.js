jQuery(window).load(function() {

    /* ******************************************************************
     * Admin View
     */
    $("#select-player").on('change', function() {
        $.ajax({
            type: "get",
            url: "/Players/Edit/",
            data: { id: $(this).children(":selected").attr("id") },
            success: function (data) {
                $("#player-data").html(data);
            }
        });
    });

    $("#copy-prod-to-staging").on("click", function (e) {
        e.preventDefault();
        toggleOverlay();
        
        $.ajax({
            url: '/Admin/CopyProdData/',
            success: function () {
                toggleOverlay();
                alert("Data has been copied.");
            }
        });
    });
});

function settingsCtrl($scope, $http) {
    var dataStore = {},
        inputs,
        checkboxes;

    $http.get("Admin/GetConfig").
        success(function(data) {
            log(data.Settings)
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