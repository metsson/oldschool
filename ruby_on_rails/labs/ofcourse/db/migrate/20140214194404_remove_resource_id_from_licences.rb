class RemoveResourceIdFromLicences < ActiveRecord::Migration
  def up
  	remove_column :licences, :resource_id
  end

  def down
  	add_column :licences, :resource_id, :integer
  end
end
