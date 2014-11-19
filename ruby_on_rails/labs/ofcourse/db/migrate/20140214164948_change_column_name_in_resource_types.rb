class ChangeColumnNameInResourceTypes < ActiveRecord::Migration
  def up
  	rename_column :resource_types , :type, :type_name
  end

  def down
  end
end
