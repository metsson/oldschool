object @resource 

node do |r|	
	node(:resource_link) { |r| api_v1_resource_url(r) }		
end