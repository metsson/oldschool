class ApplicationController < ActionController::Base
  protect_from_forgery
  
  # Returns the session-stored developer
  def current_developer
  	@current_developer ||= Developer.find(session[:developer_id]) if session[:developer_id]
  end

  # Use for authorizing action access
  def require_login
  	if current_developer.nil? then
  		flash[:developer] = "You need to log in to access the requested page."
  		redirect_to log_in_path  		
  	end
  end

  # Used to authorize access to specific developer areas
  def can_see_dashboard
  	if current_developer
  		redirect_to developer_home_path
  	else
  		require_login
  	end
  end

  # Restricting access to apps not authored by the developer in context 
  # unless the one in context is an administrator
  def is_owner_of_application(developer_in_context)
    unless developer_in_context.id == current_developer.id
        if developer_in_context.is_admin == false
          flash[:developer] = "You don't have access to the required app."
          redirect_to developer_home_path
        end
    end
  end
end
