(function searchFocus() {
    
    // Search box
    if ($("#Keyword")) {
        $("#Keyword").focus();
    }
})();

function success(ajaxContext) {
    // Enable new search
    resetSearchField();
    // Trigger broadcaster (signalR websocket proxy)
    broadcastNewSearch();
}

function failure(ajaxContext) {
    // Empty the ajax target element.
    $('#search-result').empty();
    setToastrOptions();
    console.log(ajaxContext);
    // Convert the response text, a JSON string, into a dictionary.
    // If no JSON is given (e g 'Bad Request') show generic user feedback
    try {
        var errors = JSON.parse(ajaxContext.responseText)["Errors"];
        
        if (errors.Keyword.length >= 1) {
            
            errors.Keyword.forEach(function (error) {
                toastr.error(error, "Ooppps!");
            });
        }
    } catch (e) {
        toastr.error("Something went wrong - please try again!", "Ooppps!");
    }
    // Enable new search
    resetSearchField();
}

function loading () {
    // Disable search and show feedback
    $("#search-form :input").attr("disabled", true);
    $('#search-result').empty();
    $('#search-result').append("<p class='ajax-feedback loading'>Hold on as we're calculating real ratings in here.</p>");
}

function queryCompleted () {
    // Enable new search
    resetSearchField();
}

function resetSearchField() {
    $("#search-form :input").attr("disabled", false);
    $("#Keyword").focus();
    $("#Keyword").select();
}

// Set toastr display options
function setToastrOptions() {
    toastr.options = {
        "closeButton": false,
        "debug": false,
        "positionClass": "toast-bottom-full-width",
        "onclick": null,
        "showDuration": "300",
        "hideDuration": "1000",
        "timeOut": "5000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    }
}

// Checks if client has a search result, gets the movieID and sends to the server.
// If a movie is found by that ID, the hub appends it to the fron page, creating a live search stream.
function broadcastNewSearch () {

    // Declare a proxy to reference the hub. 
    var search = $.connection.searches;

    // Function called by the hub if a valid a tag has been built
    search.client.broadcastMessage = function (link) {

        if ($(".list-group").length === 0) {
            $("#broadcaster").append("<div class='page-header'><h2>Live search stream</h2></div><div class='list-group'></div>");
        }

        $(link).hide().prependTo(".list-group").fadeIn("slow");
    };

    // Start the connection.
    $.connection.hub.start().done(function () {
        // If there is a search result (client has made a search)
        // parse the movie ID and send it to the server
        if ($(".search-result").length !== 0) {
            var movieId = parseInt($(".search-result").attr("id"));
            // ...returns a HTML anchor tag if a movie was found
            search.server.send(movieId);
        }
    });
};
