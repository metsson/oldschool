class ApiKey < ActiveRecord::Base
  # Mass assignment fields
  attr_accessible :access_token, :request_counter

  # Relations
  belongs_to :application, dependent: :destroy
end
