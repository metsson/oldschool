class CreateTableResourcesTags < ActiveRecord::Migration
  def up
  	create_table :resources_tags, :id => false do |t|
  		t.integer "resource_id"
  		t.integer "tag_id"
  	end

  	# Optimizing
  	add_index :resources_tags, ["resource_id", "tag_id"]

  end

  def down
  	drop_table :resources_tags
  end
end
