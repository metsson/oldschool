class ApplicationController < ActionController::Base
  protect_from_forgery
  before_filter :authorize  #just add 'skip_before_filter :authorize' to whitelist other controllers

  def current_user
    @current_user ||= User.find(session[:user_id]) if session[:user_id]
  end
  helper_method :current_user
  hide_action :current_user

  protected

  def authorize
    unless User.find_by_id(session[:user_id])
      redirect_to login_url, notice: "Please log in"
    end
  end

end
