# == Schema Information
#
# Table name: comments
#
#  id            :integer          not null, primary key
#  post_id       :integer
#  username      :string(255)      default("Anonymous")
#  comment_entry :text(300)
#  created_at    :datetime         not null
#  updated_at    :datetime         not null
#

require 'spec_helper'

describe Comment do
  pending "add some examples to (or delete) #{__FILE__}"
end
