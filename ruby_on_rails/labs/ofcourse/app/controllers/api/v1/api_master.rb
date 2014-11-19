module Api
	module V1
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
		  		responseMessage = [status: 401, 
		  						   user_message: 'Something went wrong during the request, please try again later.',
		  						   developer_message: 'The given access token seems invalid. Please go to your account and make sure the token used is valid.']

		  		response.status = :unauthorized
		  		respond_with responseMessage
			end

			# Authorize user using http basic
			def authorize_user!				
				begin
					authenticate_or_request_with_http_basic do |username, password|
						fethced_user = User.find_by_first_name(username)

						if fethced_user.nil? && fethced_user.authenticate(password) == false
							@user = nil
							unauthorized_user
						else
							@user = fethced_user
						end
					end
				rescue
					unauthorized_user
				end
			end	

			# Shared for all GET calls where the resource isn't found
			rescue_from ActiveRecord::RecordNotFound do
				resource_not_found
			end

      		private
      		def unauthorized_user
      			render :text => "Invalid login credentials!", :status => :unauthorized
			end					

			# Common response if a resource isn't found
			private
			def resource_not_found
		  		responseMessage = 
		  		[
		  			status: 404, 
					user_message: 'The requested resource could\'t be found.',
					developer_message: 'The requested resource was not found. Please check your API call again.',
					api_startpage: api_v1_resources_url
				]

		  		response.status = :not_found
		  		respond_with responseMessage
			end							      			
		end
	end
end