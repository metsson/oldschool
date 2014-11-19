class Resource < ActiveRecord::Base
  # Relations
  belongs_to :user
  belongs_to :resource_type  
  belongs_to :licence
  has_and_belongs_to_many :tags

  # Mass assignment fields
  attr_accessible :content, :title


  # Utilized by API v2 for rendering as JSON/XML
  def api_friendly
  	[
  		id: self.id,
  		title: self.title,
  		content: self.content,
  		created: self.created_at,
  		updated: self.updated_at,
  		author: self.user.first_name,  		
  		resource_type: self.resource_type.type_name,
  		license_type: self.licence.licence_type,
  		tags: get_tags
  	]
  end

  # Returns all tags for the resource into an array
  def get_tags
	tags = self.tags.map do |tag|
		tag.tag.to_s
	end
  end 
end
