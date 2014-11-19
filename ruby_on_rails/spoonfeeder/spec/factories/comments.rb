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

# Read about factories at https://github.com/thoughtbot/factory_girl

FactoryGirl.define do
  factory :comment do
  end
end
