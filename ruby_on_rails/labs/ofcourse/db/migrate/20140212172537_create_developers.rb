class CreateDevelopers < ActiveRecord::Migration
  def change
    create_table :developers do |t|
    	t.string :email, :null => false
    	t.string :password_digest, :null => false
    	t.boolean :admin, :default => 0
      	t.timestamps
    end
  end
end
