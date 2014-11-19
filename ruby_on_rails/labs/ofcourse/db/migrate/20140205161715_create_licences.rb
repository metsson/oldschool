class CreateLicences < ActiveRecord::Migration
  def change
    create_table :licences do |t|

      t.references :resource
      t.text :licence_text, :null => false
      t.timestamps
    end
  end
end
