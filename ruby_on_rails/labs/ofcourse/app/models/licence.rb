class Licence < ActiveRecord::Base
  # Relations
  has_many :resources
  
  # Mass assignment fields
  attr_accessible :licence_type, :licence_text
end
