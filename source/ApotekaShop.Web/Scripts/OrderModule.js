var OrderModule = (function () {
    this.initializeAddToOrderButtons = function initializeAddToCartButtons() {
        var addToOrderUrl = document.location.origin + "/Order/AddItem";

        function addToCart(id) {
            $.ajax({
                type: "GET",
                url: addToOrderUrl,
                data: { 'id': id },
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: successFunc
            });
        }

        function successFunc(data) {
            if (data.status === "Done") {
                $("#orderItemsCount").text(data.count + " items");
            } else {
                alert(data.message);
            };
        }

        $(".addToOrder").click(function (event) {
            event.preventDefault();
            var $self = $(this);
            var id = $self.data("id");
            addToCart(id);
        });
    };

    this.initializeOrderDetailsPage = function initializeOrderDetailsPage() {
       /* function removeFromCard(id) {
            $.ajax({
                type: "GET",
                url: addToCardUrl,
                data: { 'id': id },
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: successFunc
            });
        }*/
    };


    // public API
    return {
        initializeAddToOrderButtons: initializeAddToOrderButtons,
        initializeOrderDetailsPage: initializeOrderDetailsPage
    };
})();