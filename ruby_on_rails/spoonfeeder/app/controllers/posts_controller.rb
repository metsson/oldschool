class PostsController < ApplicationController
  # GET /posts
  # GET /posts.json
  def index
    @posts = Post.find(:all, :order => 'created_at DESC')

    respond_to do |format|
      format.html # index.html.erb
      format.json { render json: @posts }
    end
  end

  # GET /posts/1
  # GET /posts/1.json
  def show
    @post = Post.find(params[:id])

    if current_user && current_user.likes_post?(@post)
      @like = "Yes!"
    end
    respond_to do |format|
      format.html # show.html.erb
      format.json { render json: @post }
    end
  end

  # GET /posts/new
  # GET /posts/new.json
  def new
    @post = Post.new
    @users = User.all

    respond_to do |format|
      format.html # new.html.erb
      format.json { render json: @post }
    end
  end

  # GET /posts/1/edit
  def edit
    @post = Post.find(params[:id])
    @users = User.all
  end

  # POST /posts
  # POST /posts.json
  def create
    @post = Post.new(params[:post])
    @users = User.all

    respond_to do |format|
      if @post.save
        format.html { redirect_to @post, notice: 'Post was successfully created.' }
        format.json { render json: @post, status: :created, location: @post }
      else
        format.html { render action: "new" }
        format.json { render json: @post.errors, status: :unprocessable_entity }
      end
    end
  end

  # PUT /posts/1
  # PUT /posts/1.json
  def update
    @post = Post.find(params[:id])
    @users = User.all

    respond_to do |format|
      if @post.update_attributes(params[:post])
        format.html { redirect_to @post, notice: 'Post was successfully updated.' }
        format.json { head :no_content }
      else
        format.html { render action: "edit" }
        format.json { render json: @post.errors, status: :unprocessable_entity }
      end
    end
  end

  # DELETE /posts/1
  # DELETE /posts/1.json
  def destroy
    @post = Post.find(params[:id])
    @post.destroy

    respond_to do |format|
      format.html { redirect_to posts_url }
      format.json { head :no_content }
    end
  end

  # Counts up or down a a post like by the user
  def like
    post = Post.find_by_id(params[:id])

    if post

      like = current_user.likes.find_by_post_id(params[:id])

      if like
        # This post is already liked
        like.destroy
      else
        # Like post
        new_like = Like.create(:user_id => current_user.id, :post_id => post.id)
        post.likes << new_like
        current_user.likes << new_like
        post.save
        current_user.save
      end
    end

    redirect_to post
  end
end
