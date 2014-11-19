# This migration comes from choo_choo (originally 20140310170148)
class RemoveLocomotivesAndCarriages < ActiveRecord::Migration
  def change
    drop_table :choo_choo_locomotives
    drop_table :choo_choo_carriages
  end
end
