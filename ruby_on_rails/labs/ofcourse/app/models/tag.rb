class Tag < ActiveRecord::Base
  # Relations
  has_and_belongs_to_many :resources

  # Mass assignment fields
  attr_accessible :tag
end
