require 'ostruct'
module Api
	module V1
		class ResourcesController < ApiMaster
			before_filter :check_access_token
			before_filter :authorize_user!, :only =>  ['create', 'destroy', 'update']

			# api/v1/resources
			# OPTIONAL api/v1/resources?page=1&take=5
			def index
				if params[:user]
					@resources = search_by_user
				elsif params[:tag]
					@resources = search_by_tag
				elsif params[:license]
					@resources = search_by_license_type
				else
					@resources = Resource.page(page).per(take)
				end
				
				# Respond with empty object if this is the end
				if @resources.count == 0
					respond_with []
				end

				# Using open struct to get along with Rabl
				# If there is more resources for the value of 'more' indicates that by being > 0
				@pagination = OpenStruct.new
				@pagination.current_page = page
				@pagination.current_offset = take
				@pagination.more = Resource.page(page.to_i + 1).per(take).size
			end

			# api/v1/resources/:id
			def show
				@resource = Resource.find(params[:id]) 
			end

			# POST api/v1/resources/:resource
			def create
				resource = Resource.new
				resource.content = params[:content]
				resource.title = params[:title]
				resource.user_id = @user.id
				resource.licence_id = params[:license_id]
				resource.resource_type_id = params[:resource_type_id]
				
				# Apply tags (if new create them)
				apply_tags(resource, params[:tags])

				if resource.save 										
					render :json => { :developer_message => 'The resource was successfully created.',
									  :user_message => 'The resource was successfully created.',
									  :resource_url => api_v1_resource_url(resource)
									}, :status => :created
				else
					render :json => { :developer_message => 'Something went wrong while creating the resource.',
									  :user_message => 'The resource was not saved. Please try again.',
									  :api_startpage => api_v1_resources_url
									}, :status => :conflict
				end
			end

			# api/v1/resources/:id _method DELETE
			def destroy
				resource = Resource.find_by_id(params[:id])

				# Delete resource if found or respond with no content
				if resource		
					if resource.user_id == @user.id
														
					resource.destroy
					render :json => { :developer_message => 'The resource was successfully deleted.',
									  :user_message => 'The resource was successfully deleted.',
									  :api_startpage => api_v1_resources_url
									}, :status => :ok	
					else
						resource_action_unauthorized
					end				
				else
					resource_not_found
				end
			end

			# api/v1/resources/:íd
			def update
				resource = Resource.find_by_id(params[:id])
				if resource					
					if resource.user_id == @user.id

						# The user in context may update the resource retrieved
						Resource.update(params[:id], updatable_params)
						
						render :json => { :developer_message => 'The resource was successfully updated.',
										  :user_message => 'The resource was successfully updated.',
										  :resource_url => api_v1_resource_url(params[:id])
										}, :status => :ok											
					else
						resource_action_unauthorized
					end
				else
					resource_not_found
				end
			end

			# api/v1/resources/license/:id
			def license
				@license = Licence.find_by_id(params[:id])
			end

			# api/v1/resources/:tag
			def search_by_tag
				tag = Tag.find_by_tag(params[:tag])

				if tag
					@resources = tag.resources
				else
					resource_not_found
				end				
			end

			# api/v1/resources/:type
			def search_by_resource_type
				type = ResourceType.find_by_type_name(params[:resource_type])
				
				if type
					@resources = Resource.find_all_by_resource_type_id(type.id)
				else
					resource_not_found
				end				
			end	

			# api/v1/resources/:license_type e g MIT
			def search_by_license_type
				license = Licence.find_by_licence_type(params[:license])		

				if license
					Resource.find_all_by_licence_id(license.id)
				else
					resource_not_found
				end 
			end

			# api/v1/resources/:user
			def search_by_user
				user = User.find_by_first_name(params[:user])

				if user
					@resources = Resource.find_all_by_user_id(user.id)
				else
					resource_not_found
				end
			end

			# Common response if a resource's author is not corrensponding to the user in context
			private
			def resource_action_unauthorized
		  		responseMessage = [status: 401, 
		  						   user_message: 'You are not authorized to edit/delete this resource as it does not belong to you.',
		  						   developer_message: 'Something along the way went wrong. Please try again or contact us.',
		  						   api_startpage: api_v1_resources_url]

		  		response.status = :unauthorized
		  		
		  		respond_to do |format|
		  			format.xml { render xml: responseMessage }
		  			format.json {render json: responseMessage }
		  		end
			end				


			# Used for mass-assignment when updating a resource
			def updatable_params
				{
					content: params[:content],				 				  
					title: params[:title]
				}
			end

			# Sets the query ?page= to params[:page] or 1
			def page
				params[:page].to_i || 1
			end

			# Sets the query ?take= to params[:take] or 10
			def take
				params[:take].to_i || 10
			end			

			# Applies tags given by user, if any, using existing one or by creating new
			def apply_tags(resource, tags)
				if tags
					allTags = tags.split(/[\s,]+/)

					allTags.each do |tag|
						oldTag = Tag.find_by_tag(tag)
						if oldTag
							oldTag.resources << resource
							oldTag.save
						else
							newTag = Tag.create(:tag => tag)
							newTag.save
							resource.tags << newTag
						end
					end
				end
			end
		end
	end
end