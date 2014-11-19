ofcourse.controller('ListController', function ($scope, Restangular, $location, $filter, toaster) {	
	var resources = Restangular.all('resources');
	// Initial page 
	var page = 1;
	
	// Check header if more resources available
	var moreResources = 0;

	resources.getList({"page": page}).then(function(resources) {
		$scope.resources = resources;			
		// Set initial value for pagination, usually this will be true
		
		if (resources.pagination) {
			moreResources = resources.pagination.more || 0;
		}		
	});

	// Fired by custom directive once user is reaching bottom of
	// the app canvas, loading more resources to the $scope (aka infinite scrolling)
	$scope.loadResources = function () {		
		// Call next page and attach to $scope if not empty
		if (moreResources > 0) {			
			page += 1;

			resources.getList({"page": page}).then(function(newResources) {										
				if (newResources && newResources.length > 0) {
					// Adding each new resource one by one otherwise $watch
					// throws dupes error
					var total = newResources.length;				
					for (var i = 0; i < total; i++) {
						$scope.resources.push(newResources[i]);
					};
					toaster.pop('success', "Loading resources...", total + " resources added.");
				}
				// Update value for pagination
				if (newResources.pagination) {
					moreResources = newResources.pagination.more || 0;
				}		
			})
		} else {
			toaster.pop('info', "Loading resources...", "No more resources found!");
		}
	}  	
});

ofcourse.controller('UpdateController', function ($scope, Restangular, resource, $location) {
	// Send the unedited resource to the $scope
	$scope.editResource = Restangular.copy(resource);
	
	// Delete resource in context upon confirmation
	$scope.delete = function () {		
		if (confirm("Do you really want to delete the resource?")) {
			resource.remove().then(function () {
				alert("The resource was removed!");
				// Update the $scope var once the repsonse is OK http://goo.gl/NklhNj
				$scope.resources = _.without($scope.resources, resource);
				$location.path('/');
			}, function errorCallback(response) {
	          // Getting status for search query
	          switch (response.status) {            
	            case 500:
	              alert("A server error occurred during deletion. Please try again!");
	              break;
	            case 401:
	              alert("You need to login in order to be able to delete a resource.");
	              break;	              
	            case 403:
	              alert("You cannot delete resources that don't belong to you.");
	              break;
	            default:
	              alert("There was an unknown issue. Please try again later!");
	          }
			});	
		}
	}

	$scope.update = function () {		
		// Anything to put?
		if (angular.equals($scope.editResource, resource) === true) {
			alert("Nothing to update!");
		} else {			
			$scope.editResource.put().then(function () {
			  alert("The resource was updated! Going back to all resources.")
			  $location.path('/');
			}, function errorCallback(response) {
	          // Getting status for search query
	          switch (response.status) {            
	            case 500:
	              alert("A server error occurred while updating your resource. Please try again!");
	              break;
	            case 401:
	              alert("You need to login in order to be able to update a resource.");
	              break;	              
	            case 403:
	              alert("You cannot update resources that don't belong to you.");
	              break;
	            default:
	              alert("There was an unknown issue. Please try again later!");
	          }
			});	
		}
	};
});


ofcourse.controller('AddController', function ($scope, Restangular, $location) {
	
	// Set up new resource 
	$scope.resource = {};

	// Populate select list with licenses
	var licenses = Restangular.all('licences');
	var about_license = $scope.about_license || "";

	licenses.getList().then(function(data) {
		$scope.licenses = data.data;	
	});
	
	// Set the id of the license with the one selected by the user	
	$scope.licenseChosen = function () {
		$scope.about_license = $scope.chosen_license.license.licence_text;
	}

	// Populate select list with resource types
	var resourceTypes = Restangular.all('resource_types');

	resourceTypes.getList().then(function(data) {
		$scope.resourceTypes = data.data;	
	});

	$scope.save = function () {	
		// One type of validation is better than no validation at all!
		if ($scope.resource.title && $scope.resource.content) {
			if ($scope.resource.title.length < 10 ||
				$scope.resource.content.length < 10) {
				alert("Please add title and content that contain more than 10 characters.");
			} else {
				// Set default values for license and resource types or use the one
				// given by the user
				$scope.resource.license_id = $scope.chosen_license.license.id || 1;
				$scope.resource.resource_type_id = $scope.chosen_resource_type.resource_type.id || 1;
			    
			    Restangular.all('resources').post($scope.resource).then(function (resource) {
			      alert("The resource was successfully uploaded! Going back to all resources.");
			      $location.path('/');
			    }, function errorCallback(response) {
		          // Getting status for resource post
		          switch (response.status) {            
		            case 500:
		              alert("A server error occurred while saving your resource. Please try again!");
		              break;
		            case 401:
		              alert("You need to log in to be able to post a new resource");
		              break;
		            default:
		              alert("There was an unknown issue. Please try again later!");
		          }
				});			
			}	
		} else {
			alert("You must enter a value for the title and content of the resource.");
		}
	};  	
});

ofcourse.controller('SearchController', function ($scope, Restangular, $location) {
	$scope.resources = $scope.resources || [];
	$scope.searchResult = "Search";

	$scope.searchByUser = function () {		
		search('user', $scope.keyword);		
	}	

	$scope.searchByTag = function () {
		search('tag', $scope.keyword);		
	}	

	$scope.searchByLicense = function () {
		search('license', $scope.keyword);		
	}				

	// Refactoring function for the different search types
	// @returns Array of resources, if any
	var search = function (searchType, keyword) {
		if (keyword === undefined) {
			alert("No search term entered.");
			return;
		}						
		var baseResources = Restangular.all('resources');
		// Used to bind dynamic queries
		var params = {};
		params[searchType] = keyword;

		// Fetch resources for the given search type and keyword
		baseResources.getList(params).then(function (data) {
			// Inject search results if any
			$scope.resources = data;
			$scope.searchResult = data.length + " hit(s) for \'" + keyword + "\' in " + searchType + "s";
		}, function errorCallback(response) {
			$scope.resources = [];
			$scope.searchResult = "No matches";
          // Getting status for search query
          switch (response.status) {            
            case 500:
              alert("A server error occurred while searching. Please try again!");
              break;
            case 404:
              alert("No search results found. Please try again!");
              break;
            default:
              alert("There was an unknown issue. Please try again later!");
          }
		});			
	}	
});
