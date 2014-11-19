module Api
	module V1
		class LicencesController < ApiMaster
			before_filter :check_access_token

			# GET api/v1/licences
			def index
				@licences = Licence.all
			end
		end
	end
end
