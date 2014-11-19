# == Schema Information
#
# Table name: activities
#
#  id             :integer          not null, primary key
#  trackable_id   :integer
#  trackable_type :string(255)
#  owner_id       :integer
#  owner_type     :string(255)
#  key            :string(255)
#  parameters     :text
#  recipient_id   :integer
#  recipient_type :string(255)
#  created_at     :datetime         not null
#  updated_at     :datetime         not null
#

class Activity < ActiveRecord::Base
  belongs_to :user
  belongs_to :trackable, polymorphic: true
  attr_accessible :action, :trackable
end
