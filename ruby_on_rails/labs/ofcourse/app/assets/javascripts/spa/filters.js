// Custom filter for converting a text-based url into an link element
ofcourse.filter('linkify', function () {
        
    return function (content) {
        if (!content) {
            return content;
        } else {
	    // Regex patterns by http://goo.gl/Z1UKF5
	    var replacePattern1 = /(\b(https?|ftp):\/\/[-A-Z0-9+&@#\/%?=~_|!:,.;]*[-A-Z0-9+&@#\/%=~_|])/gim,
		    //URLs starting with "www." (without // before it, or it'd re-link the ones done above).
		    replacePattern2 = /(^|[^\/])(www\.[\S]+(\b|$))/gim,
		    //Change email addresses to mailto:: links.
		    replacePattern3 = /(\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,6})/gim;
	 
        angular.forEach(content.match(replacePattern1), function(url) {
        	content = content.replace(replacePattern1, "<a href=\"$1\" target=\"_blank\">$1</a>");
        });
        angular.forEach(content.match(replacePattern2), function(url) {
        	content = content.replace(replacePattern2, "$1<a href=\"http://$2\" target=\"_blank\">$2</a>");
        });
        angular.forEach(content.match(replacePattern3), function(url) {
        	content = content.replace(replacePattern3, "<a href=\"mailto:$1\">$1</a>");
        });

        return content;        
        }
    }	
})