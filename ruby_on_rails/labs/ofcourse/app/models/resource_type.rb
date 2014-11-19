class ResourceType < ActiveRecord::Base
  # Relations
  has_many :resources

  # Mass assignment fields
  attr_accessible :type_name
end
