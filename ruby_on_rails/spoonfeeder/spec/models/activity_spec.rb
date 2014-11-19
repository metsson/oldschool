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

require 'spec_helper'

describe Activity do
  pending "add some examples to (or delete) #{__FILE__}"
end
