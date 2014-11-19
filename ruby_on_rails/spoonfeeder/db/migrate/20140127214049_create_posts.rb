class CreatePosts < ActiveRecord::Migration
  def change
    create_table :posts do |t|
      t.references :user
      
      t.string "title", :limit => 100, :null => false
      t.text "entry", :limit => 600, :null => false
      t.timestamps
    end
  end
end
