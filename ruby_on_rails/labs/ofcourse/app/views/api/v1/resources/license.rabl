object @license
attributes :licence_type => 'license', :licence_text => 'about'

node :links do
  { :all_resources => api_v1_resources_url() }
end