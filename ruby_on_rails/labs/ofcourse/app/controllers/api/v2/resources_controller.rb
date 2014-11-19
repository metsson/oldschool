module Api
	module V2
		class ResourcesController < ApiMaster

			# GET /api/v2/resources
			# Optional paraments: /api/v2/resources?page=:int&?take=:int
			def index
				# Pagination; set defaults or get queries
				page = 0 || params[:page]
				take = 10 || params[:take]
				
				api_friendly_format(Resource.limit(take).offset(page), page, take)			
			end

			# GET /api/v2/resources/:id
			def show
				begin
					resource = Resource.find(params[:id])
					respond_with resource.api_friendly
				rescue 
					entry_not_found
				end				
			end

			# POST /api/v2/resources/:resource
			# @todo Fix authentication
			def create
				resource = Resource.new
				resource.content = params[:content]
				resource.title = params[:title]
				resource.user_id = @user.id
				resource.licence_id = params[:license_id]
				resource.resource_type_id = params[:resource_type_id]
				tags = params[:tags]

				if tags
					set_resource_tags(tags, resource)
				end

				# Respond with success message or error
				if resource.save 										
					resource_updated_or_created(resource, :created)
				else
					message = 
					[
						:developer_message => 'Something went wrong while creating the resource.',
						:user_message => 'The resource was not saved. Please try again.',
						:api_startpage => api_v2_resources_url						
					]
					response.status = :conflict
					respond_with message
				end

			end

			# PUT /api/v2/resources/:id
			# @todo Fix authentication
			def update
				begin
					resource = Resource.find_by_id(params[:id])						
					if resource.user_id == @user.id

						# The user in context may update the resource retrieved
						resource.content = params[:content]
						resource.title = params[:title]
						resource.user_id = @user.id
						resource.licence_id = params[:license_id]
						resource.resource_type_id = params[:resource_type_id]
						tags = params[:tags]

						if tags
							set_resource_tags(tags, resource)
						end

						if resource.save
							resource_updated_or_created(resource, :ok)
						end									
					else
						resource_action_unauthorized
					end					
				rescue 
					entry_not_found
				end				
			end

			# DELETE /api/v2/resources/:id
			# @todo Fix authentication
			def destroy
				respond_with Resource.destroy(params[:id])
			end	

			# Helper action
			# For each given tag by user, create if not exists
			private
			def add_or_create_tag(tag)
				Tag.where(tag: tag).first_or_create!
			end	

			# Helper action
			# Splits and strips tag values 
			private
			def set_resource_tags(tags, resource)
				split_tags = tags.split(",").map(&:to_s)
				split_tags.each do |split_tag|
					resource.tags << add_or_create_tag(split_tag.strip)				
				end
				resource.save!
			end		

			# Helper action
			# Common response if a resource's author is not corrensponding to the user in context
			private
			def resource_action_unauthorized
		  		responseMessage = 
		  		[
		  			status: 401, 
		  			user_message: 'Something along the way went wrong. Please try again or contact us.',
		  			developer_message: 'Something along the way went wrong. Please try again or contact us.',
		  			api_startpage: api_v2_resources_url
		  		]

		  		response.status = :unauthorized
		  		respond_with error_message
			end	

			# Helper action
			# Common response if a resource is updated or created without problems
			private 
			def resource_updated_or_created(resource, status)				
				message = 
				[
					user_message: 'The resource was updated/created without problems',
					developer_message: 'The resource was updated/created without problems',
					resource_url: api_v2_resources_url(resource)
				]
				response.status = status
				respond_with message
			end
		end
	end
end