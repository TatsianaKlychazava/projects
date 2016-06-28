$(document).ready(function () {
    var addToCardUrl = document.location.origin + "/Order/AddItem";
    
    function addToCard(id) {
        $.ajax({
            type: "GET",
            url: addToCardUrl,
            data: { 'id': id },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: successFunc,
            error: errorFunc
        });
    }

    function successFunc(data) {
        if (data.status === "Done") {
            $("#orderItemsCount").text(data.count + " items");
        } else {
            alert(data.message);
        };
    }

    function errorFunc() {
        alert('error');
    }

    $(".addToCard").click(function(event) {
        event.preventDefault();
        var $self = $(this);
        var id = $self.data("id");
        addToCard(id);
    });
});