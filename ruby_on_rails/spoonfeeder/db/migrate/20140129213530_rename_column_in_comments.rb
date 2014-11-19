class RenameColumnInComments < ActiveRecord::Migration
  def change
    rename_column :comments, :comment, :comment_entry
  end
end
