# This migration comes from choo_choo (originally 20140223212120)
class AddRecipienttypeToCarriage < ActiveRecord::Migration
  def change
    add_column :choo_choo_carriages, :recipient_type, :string
  end
end
