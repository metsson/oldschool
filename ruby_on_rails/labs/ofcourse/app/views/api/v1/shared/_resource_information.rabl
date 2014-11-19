object @resource 

node do |r|	
	node(:created_at) { |r| r.created_at }
	node(:updated_at) { |r| r.updated_at }
	node(:author_name) { |r| r.user.first_name }
	node(:resource_type) { |r| r.resource_type.type_name }
	node(:resource_license) { |r| r.licence.licence_type }
	node(:tags) { |r| r.get_tags }	
end