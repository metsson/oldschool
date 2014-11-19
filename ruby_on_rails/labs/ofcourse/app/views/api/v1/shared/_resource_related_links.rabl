object @resource 

node do |r|
	node(:license) { |r| api_v1_resources_by_license_url(r.licence.licence_type.to_s) }		
	node(:type) { |r| api_v1_resources_by_type_url(r.resource_type.type_name.to_s) }
end