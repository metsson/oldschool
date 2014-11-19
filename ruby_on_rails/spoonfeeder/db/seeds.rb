# This file should contain all the record creation needed to seed the database with its default values.
# The data can then be loaded with the rake db:seed (or created alongside the db with db:setup).
#
# Examples:
#
#   cities = City.create([{ name: 'Chicago' }, { name: 'Copenhagen' }])
#   Mayor.create(name: 'Emanuel', city: cities.first)

admin = User.create(name: 'admin', password: 'seed', password_confirmation: 'seed')
u2 = User.create(name: 'Mr.Choo', password: 'test', password_confirmation: 'test')

post1 = Post.create(title: 'Hello Rails!', entry: 'This is a blog entry', user: u2)
comment = Comment.create(comment_entry: 'I am Choo, who are you?', post: post1, user: u2)

Post.create(title: 'My Brand New Post!', entry: 'With some text in it', user: u2)

post2 = Post.create(title: 'admins have more fun', entry: 'Some content goes here', user: admin)
