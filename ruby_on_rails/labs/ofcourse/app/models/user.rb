class User < ActiveRecord::Base
	# Relations
	has_many :resources
	
	# Mass assignment fields
  	attr_accessible :first_name, :last_name, :email, :password, :password_confirmation 

  	# Validations
	validates :email, presence: true, uniqueness: true
	validates :password, presence: { on: :create }, length: { minimum: 3 }
	validates :password_confirmation, presence: { on: :create }, length: { minimum: 3 }

	has_secure_password
end