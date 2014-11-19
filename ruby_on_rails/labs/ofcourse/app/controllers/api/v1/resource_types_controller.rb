module Api
	module V1
		class ResourceTypesController < ApiMaster
			before_filter :check_access_token

			# GET api/v1/resourcetypes
			def index
				@resourceTypes = ResourceType.all
			end
		end
	end
end