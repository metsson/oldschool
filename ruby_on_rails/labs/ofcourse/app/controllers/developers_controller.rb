class DevelopersController < ApplicationController
	
	# Only render once own dashboard or login
	def index
		can_see_dashboard
	end

	# Simple developer panel
	def home
		if current_developer
			@developer = current_developer			
		else
			require_login
		end
	end

	## Registration and authentication actions

	# Used for the registration form
	def register
		@developer = Developer.new
	end

	# Register new developer or re-render the form
	def create
		@developer = Developer.new(params[:developer])

		if @developer.save
			session[:developer_id] = @developer.id
			flash[:developer] = "Yay! You've now signed up as a developer!"
			redirect_to developer_home_path
		else
			render :action => 'register'
		end
	end

	# Authenticate user or re-render login form
	def login
		if current_developer
			redirect_to developer_home_path
		end
	end

	# Destroy session and go to root path
	def logout
		session[:developer_id] = nil
		redirect_to root_path
	end

	def authenticate
		dev = Developer.find_by_email(params[:email])

		if dev && dev.authenticate(params[:password])	
			session[:developer_id] = dev.id
			flash[:developer] = "Yay! Nice to see you again!"
			redirect_to developer_home_path
		else
			flash[:developer] = "Your e-mail/password was not valid. Please try again!"
			render :action => 'login'
		end
	end	
end
