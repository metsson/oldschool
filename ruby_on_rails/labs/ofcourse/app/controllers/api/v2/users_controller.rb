module Api
	module V2
		class UsersController < ApiMaster

			def index
				@user = User.first
			end

			def show
				respond_with User.find(params[:id])
			end
		end
	end
end