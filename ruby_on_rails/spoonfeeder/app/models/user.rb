# == Schema Information
#
# Table name: users
#
#  id              :integer          not null, primary key
#  name            :string(255)
#  password_digest :string(255)
#  created_at      :datetime         not null
#  updated_at      :datetime         not null
#

class User < ActiveRecord::Base
  attr_accessible :name, :password, :password_confirmation, :password_digest
  validates :name, presence: true, uniqueness: true
  validates :password, presence: { on: :create }, length: { minimum: 3, allow_blank: true }
  validates :password_confirmation, presence: { on: :create }, length: { minimum: 3, allow_blank: true }

  has_secure_password

  after_destroy :ensure_one_admin_remains

  has_many :posts
  has_many :activities
  has_many :likes

  def likes_post?(post)
    self.likes.find_by_post_id(post.id)
  end

  private
  def ensure_one_admin_remains
    # TODO: are all users admins?
    if User.count.zero?
      raise "Can't delete last user"
    end
  end

end
