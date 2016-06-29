(function () {
    var apotekaApp = angular.module('apotekaApp', []);

    apotekaApp.controller('commonController', function ($scope, $http) {
        var addToOrderUrl = document.location.origin + "/Order/AddItem?id=";

        $scope.init = function(count) {
            $scope.itemsCount = count;
        }

        $scope.addOrder = function (id, $event) {
            $event.preventDefault();
            $http.get(addToOrderUrl+id).then(successCallback);
        };

        function successCallback(response) {
            if (response.data.status === "Done") {
                $scope.itemsCount = response.data.count;
            } else {
                alert(response.data.message);
            };
        }
    });
})();