Ofcourse::Application.routes.draw do
	root to: 'spa#index'

	# API Routing
	namespace :api, defaults: {format: 'json'} do
		namespace :v1 do 
			resources :resources
			resources :users
			resources :licences
			resources :resource_types
			get 'license/:id' => 'resources#license', as: 'license'
			get 'resources/:tag' => 'resources#search_by_tag', as: 'resources_by_tag'
			get 'resources/:license' => 'resources#search_by_license_type', as: 'resources_by_license'
			get 'resources/:user' => 'resources#search_by_user', as: 'resources_by_user'			
			get 'resources:resource_type' => 'resources#search_by_resource_type', as: 'resources_by_type'
		end

		# Rome wasn't build in a day. But this version was.
		namespace :v2 do 
			resources :resources						
		end		
	end	

	# Application management routes
	get 'applications' => 'applications#index', as: 'applications'
	
	# Developer routes
	get 'login' => 'developers#login', as: 'log_in'
	post 'login' => 'developers#authenticate', as: 'log_in'
	get 'logout' => 'developers#logout', as: 'log_out'
	get 'register' => 'developers#register', as: 'sign_up'
	get 'developer' => 'developers#home', as: 'developer_home'

	# Restful routes
	resources :applications
	resources :developers
end