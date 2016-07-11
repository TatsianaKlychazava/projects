﻿(function () {
    var apotekaApp = angular.module('apotekaApp');

    apotekaApp.controller('checkoutController', function ($scope, $http, $timeout) {
        var aggregatePaymentDataUrl = document.location.origin + "/Order/AggregatePaymentData";

        $scope.init = function (checkoutModel) {
            $scope.checkoutModel = checkoutModel;
            $scope.voucherCodeApplyed = true;
        }

        $scope.navigate = function (stepNumber) {
            if (!$scope.isDisabled(stepNumber)) {
                $scope.checkoutModel.CurrentStep = stepNumber;
            }
        }

        $scope.next = function() {
            $scope.checkoutModel.CurrentStep++;
            $scope.save();
        }

        $scope.applyCode = function() {
            $scope.voucherCodeApplyed = !$scope.voucherCodeApplyed;
        }

        $scope.voucherCodeIsValid = function () {
            return $scope.voucherCodeApplyed;
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
        }

        $scope.submitPayment = function ($event, form) {
            $event.preventDefault();

            $http.post('/order/updateorder', $scope.checkoutModel);

            $http.get(aggregatePaymentDataUrl).then(successCallback);

            function successCallback(response) {
                $scope.paymentData = response.data;
                $timeout(function () {
                    $event.target.submit();
                }, 10);
            }
        }
    });
})();