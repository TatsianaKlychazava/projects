(function () {
    var apotekaApp = angular.module('apotekaApp');

    apotekaApp.controller('checkoutController', function ($scope, $http) {
        $scope.init = function (checkoutModel) {
            $scope.checkoutModel = checkoutModel;
        }

        $scope.next = function() {
            $scope.checkoutModel.CurrentStep++;
        }
        $scope.back = function () {
            $scope.checkoutModel.CurrentStep--;
        }
    });
})();