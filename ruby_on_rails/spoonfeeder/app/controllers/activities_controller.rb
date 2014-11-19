class ActivitiesController < ApplicationController
  def index
    @activities = ChooChoo::Activity.order('updated_at DESC').all
  end
end
