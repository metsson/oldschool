object @user

node do |user|
	node(:profile_link) { |user| api_user_url(user)
end