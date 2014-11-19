class Developer < ActiveRecord::Base
	# Mass assignment fields
	attr_accessible :email, :password, :password_confirmation

	# Relations
	has_many :applications

	# Validations
	validates :email, 
			  :presence => { :message => 'can\'t be blank'}, 
			  :uniqueness => { :message => 'is already registered'}
	
	validates :password, 
			  presence: { on: :create }, length: { minimum: 3 }
	
	validates :password_confirmation, 
			  presence: { on: :create }, length: { minimum: 3 }

	has_secure_password 

	# Used to authorize administrative links/tools
	def is_admin
		self.admin == true
	end
end
