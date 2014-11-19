class ResourcesTags < ActiveRecord::Base
  belongs_to :resources
  belongs_to :tags
end
