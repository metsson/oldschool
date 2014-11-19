class ApplicationsController < ApplicationController

	# Redirects to log in or dashboard
	def index		
	end

	# Used for the registration form
	def new
		@application = Application.new		
	end

	# Register new application or re-render the form
	def create
		@application = Application.new(params[:application])

		if @application.save
			flash[:developer] = "Yay! Your application has been registered!"
			current_developer.applications << @application
			# Randomizer as in http://goo.gl/qpI5Rv
			access_token = Array.new(32){rand(36).to_s(36)}.join
			key = ApiKey.create(:access_token => access_token)
			key.application = @application
			key.save
			redirect_to developer_home_path
		else
			render :action => 'register'
		end
	end

	# Get the app and its key data
	def show		
		@app = Application.find_by_id(params[:id])
		@key = ApiKey.find_by_application_id(@app)
		is_owner_of_application(@app.developer)
	end

	# Delete the app given its id (actually goes through ApiKey using :dependent)
	def destroy
		app_api_key = ApiKey.find_by_application_id(params[:id])

		if app_api_key
			app_api_key.destroy
			flash[:developer] = "The app was successfully deleted."
		else
			flash[:developer] = "Something went wrong and the app is not deleted"
		end	
		redirect_to developer_home_path			
	end
end