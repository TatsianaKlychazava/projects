(function () {
    var apotekaApp = angular.module('apotekaApp');

    apotekaApp.controller('checkoutController', function ($scope, $http) {
        $scope.init = function (checkoutModel) {
            $scope.checkoutModel = checkoutModel;
        }
    });
})();