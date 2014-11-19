require 'faker'

# Known user credentials required for testing CRUD in the spa
user = User.create(:first_name => "TestUser", :last_name => "TestUser1", :email => "tester@live.se", :password => 'Password', :password_confirmation => 'Password')

# Preset values for the faked resources
linkTag = Tag.create(:tag => "link")
youtubeTag = Tag.create(:tag => "youtube")

linkType = ResourceType.create(:type_name => "Link")
videoType = ResourceType.create(:type_name => "Video")
pictureType = ResourceType.create(:type_name => "Picture")
textType = ResourceType.create(:type_name => "Text")

tag = Tag.create(:tag => "spa")

license = Licence.create(:licence_text => "Read more about the license at http://opensource.org/licenses/MIT", :licence_type => "MIT")
license2 = Licence.create(:licence_text => "Read more about the license at http://www.gnu.org/licenses/gpl-2.0.html", :licence_type => "GPL v2")

# Using Faker gem for resources
100.times do
	resource = Resource.create(
		:content => Faker::Lorem.paragraph, 
		:title => Faker::Lorem.sentence)

	tag.resources << resource
	#resource.tags << tag
	resource.licence = license
	resource.resource_type = textType
	user.resources << resource		
end