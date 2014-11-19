class CreateResources < ActiveRecord::Migration
  def change
    create_table :resources do |t|
    	t.references :user
    	t.references :tag
    	t.references :licence
    	t.references :resource_type
    	t.string :content, :null => false
    	t.timestamps
    end
  end
end
