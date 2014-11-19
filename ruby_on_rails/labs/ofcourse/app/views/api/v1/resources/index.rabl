object false

child @resources, :root => 'data', :object_root => 'resource' do
	extends 'v1/resources/show'
end

child @pagination => :pagination do
    attributes :current_page, :current_offset, :more
end