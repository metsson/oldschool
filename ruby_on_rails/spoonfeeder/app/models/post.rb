# == Schema Information
#
# Table name: posts
#
#  id         :integer          not null, primary key
#  title      :string(255)
#  entry      :text
#  created_at :datetime         not null
#  updated_at :datetime         not null
#

class Post < ActiveRecord::Base
  include ChooChoo::ParentNode

  attr_accessible :title, :entry, :user, :user_id
  validates :title, presence: true
  validates :entry, presence: true

  belongs_to :user
  has_many :comments
  has_many :likes

  def excerpt(max_length = 60)
    self.entry[0..60]
  end
end
