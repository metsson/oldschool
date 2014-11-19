// Autocomplete for movie title searches
$(function () {
    $("#Keyword").autocomplete({
        source: function (request, response) {            
            $.ajax({
                url: '/Home/InstantSearch',
                data: { keyword: request.term },
                dataType: "json",
                type: "GET",
                success: function (data) {
                    response($.map(data, function (item) {
                        return { value: item };
                    }))
                }
            })
        },
        //Require at least one character for the keyword
        minLength: 1,
        // Submit the selected keyword
        select: function (event, ui) {
            $("#Keyword").val(ui.item.value);
            $("#Keyword").submit();
        }
    });
});