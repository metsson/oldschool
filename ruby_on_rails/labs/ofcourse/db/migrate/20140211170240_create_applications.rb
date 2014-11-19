class CreateApplications < ActiveRecord::Migration
  def change
    create_table :applications do |t|
    	t.references :developer
    	t.string :application_name, :null => false
    	t.timestamps
    end
  end
end
