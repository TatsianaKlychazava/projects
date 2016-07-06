(function () {
    var apotekaApp = angular.module('apotekaApp');

    apotekaApp.controller('checkoutController', function ($scope, $http) {
        $scope.init = function (checkoutModel) {
            $scope.checkoutModel = checkoutModel;
        }

        $scope.navigate = function (stepNumber) {
            if (!$scope.isDisabled(stepNumber)) {
                $scope.checkoutModel.CurrentStep = stepNumber;
            }
        }

        $scope.next = function () {
            $scope.checkoutModel.CurrentStep++;
            $scope.save();
        }

        $scope.back = function () {
            $scope.checkoutModel.CurrentStep--;
        }

        $scope.isActive = function(stepNumber) {
            return $scope.checkoutModel.CurrentStep === stepNumber;
        }

        $scope.isDisabled = function (stepNumber) {
            return stepNumber > $scope.checkoutModel.CurrentStep;
        }

        $scope.totalPrice = function() {
            var total = 0;
            for (var i = 0; i < $scope.checkoutModel.Items.length; i++) {
                var item = $scope.checkoutModel.Items[i];
                total += (item.PricePerUnit * item.Count);
           }
           return total;
        }

        $scope.save = function () {
            $http.post('/order/updateorder',  $scope.checkoutModel);
            return total;
        }
    });
})();