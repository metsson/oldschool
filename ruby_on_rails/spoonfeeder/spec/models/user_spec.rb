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

require 'spec_helper'

describe User do

  it "is valid with a username and confirmed password" do
    user = User.new(
      name: 'Username',
      password: 'Password',
      password_confirmation: 'Password')
    expect(user).to be_valid
  end

  it "is invalid without a username" do
    expect(User.new(name: nil)).to have(1).errors_on(:name)
  end

  it "is invalid without a password" do
    expect(User.new(password: nil)).to have(1).errors_on(:password)
  end

  it "is invalid without a password confirmation" do
    expect(User.new(password_confirmation: nil)).to have(1).errors_on(:password_confirmation)
  end

  it "is invalid if it is a duplicate" do
    user = User.new(
      name: 'Username',
      password: 'Password',
      password_confirmation: 'Password')

    user.save

    duplicate_user = User.new(
      name: 'Username',
      password: 'Password',
      password_confirmation: 'Password')

    expect(duplicate_user).to be_invalid

  end

  it "always has at least one admin user remaining" do
    user1 = User.create(
      # create first user
      name: 'Username',
      password: 'Password',
      password_confirmation: 'Password')

      # create second user
    user2 = User.create(
      name: 'Username2',
      password: 'Password2',
      password_confirmation: 'Password2')
   
    user1.save
    user2.save
    user1.destroy
 
    expect{ user2.destroy }.to raise_error
  end
end
