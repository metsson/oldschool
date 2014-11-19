class AdminController < ApplicationController
  def index
    @total_users = User.count
    @time = Time.now
  end
end
