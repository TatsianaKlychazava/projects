$(document).ready(function () {
    var url = document.location.origin + "/ProductDetails/Autocomplete";
    $('input[name=query]').each(function() {
        $(this).autocomplete({
            source: function(request, response) {
                $.ajax({
                    url: url,
                    type: "Get",
                    dataType: "json",
                    data: { query: request.term },
                    success: function(data) {
                        response($.map(data, function(item) {
                            return { label: item.Name, value: item.Value };
                        }));

                    }
                });
            },
            minLength: 3
        }).data("ui-autocomplete")._renderItem = function(ul, item) {
            return $("<li></li>")
                .data("item.autocomplete", item)
                .append($.parseHTML(item.label))
                .appendTo(ul);
        };
    });
});