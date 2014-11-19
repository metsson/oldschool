module Api
	module V2
		# ApiMaster contains actions for authorization and authentication.
		# But also actions that serve as helpers for its child-controllers.
		class ApiMaster < ApplicationController
			respond_to :json
			respond_to :xml		
			skip_before_filter :verify_authenticity_token	

			# Check if the given token is valid
			def check_access_token				
				token = request.headers["X-Access-Token"] || nil

				if token
					find_token = ApiKey.find_by_access_token(token)

					if find_token.nil?
						invalid_access_token
					end
				else
					invalid_access_token
				end
			end	

			# Renders error message for api client if access token is invalid 
			private
			def invalid_access_token
		  		message = 
		  		[
					status: 401, 
					user_message: 'Something went wrong during the request, please try again later.',
					developer_message: 'The given access token seems invalid. Please go to your account and make sure the token used is valid.'
		  		]

		  		response.status = :unauthorized
		  		respond_with message
			end

			# Utilized as a rescue action for GET in resources that return no entry
			# Responds with message and status 404
			def entry_not_found
				message = 
				[
					status: 404,
					user_message: 'The resource you were looking for cannot be found.',
					developer_message: 'The resource could not be found, please check your parameters and try again.',
					root_url:  api_v2_resources_url
				]
				response.status = :not_found
				respond_with message
			end

			# Utilized for rendering resources the restafarian way
			# Mostly used for the Resource model
			def api_friendly_format(data, page, take)

				if data.count > 0								
					
					# Format each resource using its own
					# formatting method
					items = data.map do |item|
						item.api_friendly
					end

					data =
					[
						status: 200,
						data: items,

						pagination: 
						[
							page: page,
							count: data.count,	
							next_page: get_next_page(data.count, page, take)		
						]
					]
					response.status = :ok
					respond_with data
				else
					# There is no data to show
					data =
					[
						status: 404,
						user_message: 'There are no resources to show.',
						developer_message: 'There are no resources to show. Make sure you have set the right parameters for pagination.'
					]
					response.status = :not_found
					respond_with data
				end
			end

			# Renders url to next page or back to main page if there is no more to show
			def get_next_page(count, page, take)
				if count >= take
					"#{api_v2_resources_url}?page=#{page.to_i + 1}&take=#{take}"
				else
					api_v2_resources_url
				end														
			end
		end
	end
end