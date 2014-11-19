object @resource
attributes :title, :content

node(:id) { |r| r.id }

node :more do |resource|
  { :about_resource => partial('v1/shared/_resource_information', :object => resource) }
end

node :links do |resource|
  { :resource_links => partial('v1/shared/_resource_links', :object => resource) }
end

node :related do |resource|
  { :find_by => partial('v1/shared/_resource_related_links', :object => resource) }
end