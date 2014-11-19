class CreateComments < ActiveRecord::Migration
  def change
    create_table :comments do |t|
      t.references :post
      t.references :user
            
      t.text "comment", :limit => 300
      t.timestamps
    end
  end
end
