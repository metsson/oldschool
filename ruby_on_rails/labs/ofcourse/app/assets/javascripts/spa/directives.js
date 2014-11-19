// Custom directive for loading more resources while scrolling
// Inspired and based upon Pablo Recio @ https://coderwall.com/p/cl7zyg
ofcourse.directive('infiniteScrolling', function () {
  return {
  	// Using as an attribute
  	restrict: 'A',

    link: function (scope, element, attrs) {
		// Using neater bottom detector by http://jsfiddle.net/8PkQN/1/
		window.onscroll = function(ev) {
			if ((window.innerHeight + window.scrollY) >= document.body.offsetHeight) {
				scope.$apply(attrs.infiniteScrolling);
			}
		}   
	}    
  };
});