class AddLicenceTypeToLicences < ActiveRecord::Migration
  def change
  	add_column :licences, :licence_type, :string
  end
end