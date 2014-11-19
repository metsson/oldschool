# Welcome to OfCourse API
OfCourse provides a RESTful API that allows registered professionals to manage lists of resources for their course in context. The resource may be a link, video, image, text or code snippet. Whatever the type, each resource is categorized by given resource type, tags and licence (if any) and information for its creation and user. 

# How to install the API

 * You will need to download the content of /ofcourse into your local Ruby on Rails root folder
 * Run bundle install
 * Run rake db:migrate
 * run db:seed to seed the database with local data NOTE! This step must be done!
 
 As of this version of the API, the root page is set to the SPA. Therefore you will need to follow these steps to register for an access token!
 * Go to localhost:3000/applications and click on 'Sign up'
 * Register with valid credentials
 * Next click on 'Set up new app' and enter a name when required
 * You should now be redirected to 'My apps'
 * Click on the newly created app and notice the field 'Access Token' as you will need this for authenticating your SPA

# How to install the SPA
Before continuing, make sure that you have ran rake db:seed so that you have some demo data in your local sqlite database. The foremost important reason for 
seeding the database is that you will need one user credentials in order to authenticate into the SPA. 

* First thing to do is to edit the file found in ofcourse/app/assets/javascripts/spa/app-init.js.erb:31

```js
RestangularProvider.setDefaultHeaders({ 'X-Access-Token': '[access token here]' });   
```
Replace access_token with the access token retrieved from the API and you are all set!

# Credentials into the SPA for CRUD operations
When adding, updating or deleting a resource the API utilizes http basic authentication to make sure you are authorized to 
call unsafe methods. 

As noted before, the API has not yet implementet oAuth so you will need to have seeded the API database before using fully CRUD operations. If you have done this, then supply the following credentials if prompted by the API:

* username: TestUser
* password: Password



# Getting started
Although the API applies [HATEOAS](http://en.wikipedia.org/wiki/HATEOAS), it might be a good idea to know what the response content might be for each specific API call you will make. Below, you will find a complete documentation that you will guide you through the structure of the API and how it responds to your calls.

**Quick facts**
 
 * Rate limit: No limit at the moment
 * Cost: Free
 * Access token: Required
 * Respond type: JSON (default)/XML

 **Main API URL**

 You can access the root endpoint of the API by going to

		localhost:3000/api/v1/resources

 From here you will be able to navigate through links given from the API in JSON. Please **notice** that the API root will not be included during the rest of the documentation.

 **JSON seems to be the default response type but I want to use XML**

 All you have to do in this case is simply adding .xml at the end of your API call. For instance:

		localhost:3000/api/v1/resources/1.xml

 The response will be in XML as following

		<?xml version="1.0" encoding="UTF-8"?>
		<resource>
		  <title>My youtube link</title>
		  <content>Link</content>
		  <more>
		    <about-resource>
		      <created-at type="datetime">2014-02-13T18:40:28Z</created-at>
		      <updated-at type="datetime">2014-02-13T18:40:28Z</updated-at>
		      <author-name>Merkur</author-name>
		      <resource-type>Link</resource-type>
		      <resource-license>MIT License</resource-license>
		    </about-resource>
		  </more>
		  <links>
		    <resource-links>
		      <author-link>http://localhost:3000/api/v1/users/1</author-link>
		      <resource-link>http://localhost:3000/api/v1/resources/1</resource-link>
		      <license-link>http://localhost:3000/api/v1/license/1</license-link>
		    </resource-links>
		  </links>
		  <related>
		    <find-by>
		      <license>http://localhost:3000/api/v1/resources/license/MIT%20License</license>
		      <user>http://localhost:3000/api/v1/resources/user/Merkur</user>
		      <type>http://localhost:3000/api/v1/resources/type/Link</type>
		      <tags>http://localhost:3000/api/v1/resources/tags/link%20tag</tags>
		    </find-by>
		  </related>
		</resource>

## Authorizing by using your access token

You will need an access token for being able to use the API. Go to the site and sign up for a developer account. By doing that you will be able to create limitless amount of apps with their respective access tokens. Once you have an app set up you can start making API calls by always including your access token into the header.

	X-Access-Token	[your token here]

You are now good to go and can start using the ofcourse API.


## Authenticating a user

For editing, deleting and creating new resources you will need an authenticated user in place. For instance, a PUT POST to api/v1/resources/1 will prompt you to enter the credentials of a user. Enter the email and password of the user for authenticating. Please notice that if the authenticated user is other than the user of the resource in context, you will not be able to proceed. 

## All resources 
GET

    /resources

RESPONSE

    {
    "data": [
        {
            "resource": {
                "title": "My youtube link",
                "content": "Link",
                "more": {
                    "about_resource": {
                        "created_at": "2014-02-13T18:40:28Z",
                        "updated_at": "2014-02-13T18:40:28Z",
                        "author_name": "Merkur",
                        "resource_type": "Link",
                        "resource_license": "MIT License"
                    }
                },
                "links": {
                    "resource_links": {
                        "author_link": "http://localhost:3000/api/v1/users/1",
                        "resource_link": "http://localhost:3000/api/v1/resources/1",
                        "license_link": "http://localhost:3000/api/v1/license/1"
                    }
                },
                "related": {
                    "find_by": {
                        "license": "http://localhost:3000/api/v1/resources/license/MIT%20License",
                        "user": "http://localhost:3000/api/v1/resources/user/Merkur",
                        "type": "http://localhost:3000/api/v1/resources/type/Link",
                        "tags": "http://localhost:3000/api/v1/resources/tags/link%20tag"
                    }
                }
            }
        }
        ...        
    	]
	}

## Pagination for resources

Calls to /resources are limited to 10 records per page. Unless you set the page query to a value you will hence only be shown the first 5 records retrived from the databse. If your page value is too big, a 404 with its proper message is shown. Meaning that there is no content found on that page. 

GET

    /resources?page=135323

RESPONSE

    {
        "developer_message": "There are no more resources to see.",
        "user_message": "Something went wrong while fetching the resources.",
        "api_startpage": "http://localhost:3000/api/v1/resources"
    }
    
## Resource by id
GET

    /resources/1

RESPONSE

    {
    "title": "My youtube link",
    "content": "Link",
    "more": {
        "about_resource": {
            "created_at": "2014-02-13T18:40:28Z",
            "updated_at": "2014-02-13T18:40:28Z",
            "author_name": "Merkur",
            "resource_type": "Link",
            "resource_license": "MIT License"
        }
    },
    "links": {
        "resource_links": {
            "author_link": "http://localhost:3000/api/v1/users/1",
            "resource_link": "http://localhost:3000/api/v1/resources/1",
            "license_link": "http://localhost:3000/api/v1/license/1"
        }
    },
    "related": {
        "find_by": {
            "license": "http://localhost:3000/api/v1/resources/license/MIT%20License",
            "user": "http://localhost:3000/api/v1/resources/user/Merkur",
            "type": "http://localhost:3000/api/v1/resources/type/Link",
            "tags": "http://localhost:3000/api/v1/resources/tags/link%20tag"
        }
    }
	}

## Resource by tag name
GET

    /resources/tags/link tag

RESPONSE

    {
    "data": [
        {
            "resource": {
                "title": "My youtube link",
                "content": "Link",
                "more": {
                    "about_resource": {
                        "created_at": "2014-02-13T18:40:28Z",
                        "updated_at": "2014-02-13T18:40:28Z",
                        "author_name": "Merkur",
                        "resource_type": "Link",
                        "resource_license": "MIT License"
                    }
                },
                "links": {
                    "resource_links": {
                        "author_link": "http://localhost:3000/api/v1/users/1",
                        "resource_link": "http://localhost:3000/api/v1/resources/1",
                        "license_link": "http://localhost:3000/api/v1/license/1"
                    }
                },
                "related": {
                    "find_by": {
                        "license": "http://localhost:3000/api/v1/resources/license/MIT%20License",
                        "user": "http://localhost:3000/api/v1/resources/user/Merkur",
                        "type": "http://localhost:3000/api/v1/resources/type/Link",
                        "tags": "http://localhost:3000/api/v1/resources/tags/link%20tag"
                    }
                }
            }
        }
        ...
    	]
	}

## Resource by license type
GET

    /resources/license/MIT lincese

RESPONSE 

        {
    "data": [
        {
            "resource": {
                "title": "My youtube link",
                "content": "Link",
                "more": {
                    "about_resource": {
                        "created_at": "2014-02-13T18:40:28Z",
                        "updated_at": "2014-02-13T18:40:28Z",
                        "author_name": "Merkur",
                        "resource_type": "Link",
                        "resource_license": "MIT License"
                    }
                },
                "links": {
                    "resource_links": {
                        "author_link": "http://localhost:3000/api/v1/users/1",
                        "resource_link": "http://localhost:3000/api/v1/resources/1",
                        "license_link": "http://localhost:3000/api/v1/license/1"
                    }
                },
                "related": {
                    "find_by": {
                        "license": "http://localhost:3000/api/v1/resources/license/MIT%20License",
                        "user": "http://localhost:3000/api/v1/resources/user/Merkur",
                        "type": "http://localhost:3000/api/v1/resources/type/Link",
                        "tags": "http://localhost:3000/api/v1/resources/tags/link%20tag"
                    }
                }
            }
        }
        ...
    	]
	} 


## Resource by user(name)
GET

    /resources/user/Merkur

RESPONSE  

        {
    "data": [
        {
            "resource": {
                "title": "My youtube link",
                "content": "Link",
                "more": {
                    "about_resource": {
                        "created_at": "2014-02-13T18:40:28Z",
                        "updated_at": "2014-02-13T18:40:28Z",
                        "author_name": "Merkur",
                        "resource_type": "Link",
                        "resource_license": "MIT License"
                    }
                },
                "links": {
                    "resource_links": {
                        "author_link": "http://localhost:3000/api/v1/users/1",
                        "resource_link": "http://localhost:3000/api/v1/resources/1",
                        "license_link": "http://localhost:3000/api/v1/license/1"
                    }
                },
                "related": {
                    "find_by": {
                        "license": "http://localhost:3000/api/v1/resources/license/MIT%20License",
                        "user": "http://localhost:3000/api/v1/resources/user/Merkur",
                        "type": "http://localhost:3000/api/v1/resources/type/Link",
                        "tags": "http://localhost:3000/api/v1/resources/tags/link%20tag"
                    }
                }
            }
        }
        ...
    	]
	}

## Post a new resource

POST 

    /resources/

REQUIRED FORM FIELDS
**Notice that you need an authenticated user for posting a resource**

    content
    title 
    tags 
    license_id 
    resource_type_id

RESPONSE

        {
            "developer_message": "The resource was successfully created",
            "user_message": "The resource was successfully created",
            "resource_url": "http://localhost:3000/api/v1/resources/23"
        }


## Delete a resorce

POST 

    /resources/1

REQUIRED FORM FIELDS
**Notice that you need an authenticated user, that also is author of the resource, for deletion.**    

    _method set to DELETE

RESPONSE

        {
            "developer_message": "The resource was not find. Make sure the API call is correct",
            "user_message": "The resource was not found",
            "api_startpage": "http://localhost:3000/api/v1/resources"
        }

The response above is given if the resource id set to delete already is deleted. Otherwise you will get a status 200 and a proper feedback message telling you that the resource has been deleted.

## Update a resource

POST 

    /resources/1

REQUIRED FORM FIELDS

**Notice that you need an authenticated user, that also is the author of the resource, for posting a resource**

    _method set to PUT
    content
    title 

RESPONSE

        {
            "developer_message": "The resource was successfully updated.",
            "user_message": "The resource was successfully updated.",
            "resource_url": "http://localhost:3000/api/v1/resources/1"
        }  