class CreateApiKeys < ActiveRecord::Migration
  def change
    create_table :api_keys do |t|
      t.references :application
      t.string :access_token, :null => false
      t.integer :request_counter, :default => 0
    end
  end
end
