# This migration comes from choo_choo (originally 20140311120314)
class RenameEventsToNodes < ActiveRecord::Migration
  def change
    rename_column :choo_choo_activities, :last_event_id, :last_updated_node_id
    rename_column :choo_choo_activities, :last_event_type, :last_updated_node_type

    rename_column :choo_choo_activities, :master_event_id, :parent_node_id
    rename_column :choo_choo_activities, :master_event_type, :parent_node_type
  end
end
