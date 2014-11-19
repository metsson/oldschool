class Application < ActiveRecord::Base

	# Mass assignment fields
	attr_accessible :application_name

	# Relations
	belongs_to :developer

	# Validations
	validates :application_name,
			  :presence => { :message => 'can\'t be blank'},
			  :uniqueness => { :message => 'is already taken, try another one'}	
end
