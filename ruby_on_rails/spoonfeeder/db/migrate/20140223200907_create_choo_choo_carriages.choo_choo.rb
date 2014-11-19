# This migration comes from choo_choo (originally 20140221100201)
class CreateChooChooCarriages < ActiveRecord::Migration
  def change
    add_column :choo_choo_carriages, :recipient_id, :integer
  end
end
