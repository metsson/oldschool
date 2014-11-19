# This migration comes from choo_choo (originally 20140310144724)
class CreateChooChooActivities < ActiveRecord::Migration
  def change
    drop_table :choo_choo_activities

    create_table :choo_choo_activities do |t|

      t.string :last_action

      t.belongs_to :master_event, polymorphic: true
      t.belongs_to :last_event, polymorphic: true

      t.timestamps
    end
  end
end
