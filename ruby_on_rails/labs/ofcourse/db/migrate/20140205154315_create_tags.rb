class CreateTags < ActiveRecord::Migration
  def change
    create_table :tags do |t|
    	t.references :resource
      	t.string :tag, :null => false, :limit => 200
      	t.timestamps
    end
  end
end
