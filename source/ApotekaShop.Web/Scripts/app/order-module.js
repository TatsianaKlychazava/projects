(function() {
    var apotekaApp = angular.module('apotekaApp');

    apotekaApp.controller('orderController', function ($scope, $http) {
        $scope.init = function(model, currencyName, productDetailsUrl) {
            $scope.orderItems = model.OrderItems;
            $scope.currencyName = currencyName;
            $scope.productDetailsUrl = productDetailsUrl;
            $scope.getTotal();
        }

        $scope.changeCount = function (order, value) {
            if (value >= 0) {
                order.Count = value;
                $http.post('/order/UpdateItemCount', { 'id': order.Id, 'count': value });
                $scope.getTotal();
            }
        }
        
        $scope.remove = function(index) {
            var order = $scope.orderItems[index];

            $scope.orderItems.splice(index, 1);

            $http.post('/order/DeleteItem', { 'id': order.Id });
            $scope.$parent.itemsCount = $scope.orderItems.length;
            $scope.getTotal();
        }

        $scope.getTotal = function () {
            var total = 0;
            for (var i = 0; i < $scope.orderItems.length; i++) {
                var item = $scope.orderItems[i];
                total += (item.PricePerUnit * item.Count);
            }
            $scope.total = total;
        }
    });
})();
