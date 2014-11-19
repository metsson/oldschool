# SpoonFeeder [![Build Status](https://travis-ci.org/lnucrowding/spoonfeeder.png?branch=master)](https://travis-ci.org/lnucrowding/spoonfeeder)

A simple blog application in rails using public_activity

Maybe even some form of automatic mail sender for notifications.

## Requirements

- Rails 3.2.16
- Ruby 2.0.0
- public_activity


## Installation

Clone this repo

    # install deps
    bundle install

    # start the server
    rails s

If it complains about `config/initializers/secret_token.rb`, run this from the project root and restart the server:

    echo "Spoonfeeder::Application.config.secret_token = '$(rake secret)'" > ./config/initializers/secret_token.rb

## Testing

Dont forget to run `rake db:test:clone` to sync the testing database.

    bundle exec rspec

    # watch files and run tests on save
    guard


## Login / Authentication

To add default user with username `admin` and passoword `seed` run:

    'rake db:seed'

Add `skip_before_filter :authorize` to whitelist controllers (i.e. to skip authentication)

## Other

The annotate_models gem is used to increase readability. To annotate go to root and run:

    annotate

Full instructions here: https://github.com/ctran/annotate_models
