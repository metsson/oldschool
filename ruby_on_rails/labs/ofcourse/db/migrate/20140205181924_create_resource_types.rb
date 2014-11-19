class CreateResourceTypes < ActiveRecord::Migration
  def change
    create_table :resource_types do |t|
      t.references :resource
      t.string :type, :null => false, :limit => 100
      t.timestamps
    end
  end
end
