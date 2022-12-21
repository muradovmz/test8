## Steps for getting started with Auth0
  * [Login or Signup](#login-or-signup)
  * [Creating an application](#creating-an-application)
  * [Creating an api](#creating-an-api)
  * [Creating a user](#creating-a-user)
  * [Getting access token](#getting-access-token)
  * [Configuring service-starter with auth0](#configuring-service-starter-dotnetcore-with-auth0)
  * [Running application](#running-service-starter-dotnetcore)
  * [Calling the resource server(service starter dotnetcore) using access token](#Configuring service starter dotnetcore with auth0)

Each step is explained below

### Login or Signup
  * Go to auth0 at https://auth0.com/ 
  * If you are already an existing user login with username and password
  * Or Signup as new user with email and password  
    * Then signup will redirect you to create a default tenant
      * Give valid tenant domain name ("service-starter-dotnetcore"), try with different name if tenant name already exists 
      * Choose region which  host all of your data in that region (e.g., US)
    * The next button will redirect you to give extra information about new user
      * Choose an account type (e.g., Company), specify a company name and employees (e.g., 1000+)
      * Then click on create account and it redirects you to auth0 dashboard.
    
### Creating an application
  * From dashboard go to Applications -> Create application
  * Give the name of the application as "service-starter-dotnetcore-application"
  * Choose the application type you are creating to access the source(e.g., single page web application) then click on create it will redirect to next page.
  * Select the technology are you using for your single page web web application ( e.g., react )
  * Then it will give a GitHub sample reference to start with the login page. You can start working with this when you want to build a front end. Here to complete these initial steps, we are not intrested in front end part so we don’t want to worry about that sample.

### Creating an api
  * From dashboard go to Applications -> Api -> Create Api.
  * Specify name ("service-starter-dotnetcore-api"), identifier (http://localhost:5000), Signing Algorithm (RS256 default algorithm).
  * Then click on create.
  
### Creating a user
  * From dashboard go to Applications -> User management -> Create user.
  * Specify email(user1@gmail.com),password and choose connection (e.g., User-password-authentication default connection)
  * Click on create.

### Getting access token
  * For getting an access token here **we are using Resource Owner Password Grant flow**
  * By default, the application we created("service-starter-dotnetcore-application") dont have access to implement resource owner password grant flow.
  * From dashboard go to Applications -> "service-starter-dotnetcore-application" -> settings -> show advanced settings -> grant types -> enable password flow(e.g., Password) -> save changes
  * By using the below curl command you can get the access token.
  ```
//substitute <password> with the user password before running the command 
    curl --request POST \
      --url 'https://service-starter-dotnetcore/oauth/token' \
      --header 'content-type: application/x-www-form-urlencoded' \
      --data grant_type=http://auth0.com/oauth/grant-type/password-realm \
      --data username=user1@gmail.com \
      --data password=<password> \
      --data audience=http://localhost:8080 \
      --data 'client_id=YOUR_CLIENT_ID' \
      --data client_secret=YOUR_CLIENT_SECRET \
      --data realm=Username-Password-Authentication

//in case you used different names to configure than mentioned in the steps, then customise and run your own version of this command as specified below
//in the below command, in the url argument substitute YOUR_TENANT_DOMAIN with the tenant domain name if you gave a different one,
//substitute <username> and <password> with the user name and password you gave in user creation step above,
// specify audience as your API identifier if different, 
// substitute YOUR_CLIENT_ID and YOUR_CLIENT_SECRET with values as per your auth0 configuration if different

      curl --request POST \
      --url 'https://YOUR_TENANT_DOMAIN/oauth/token' \
      --header 'content-type: application/x-www-form-urlencoded' \
      --data grant_type=http://auth0.com/oauth/grant-type/password-realm \
      --data username=user1@gmail.com \
      --data password=<password> \
      --data audience=YOUR_API_IDENTIFIER \
      --data 'client_id=YOUR_CLIENT_ID' \
      --data client_secret=YOUR_CLIENT_SECRET \
      --data realm=Username-Password-Authentication
  ```
  * You would get a Response that looks like this:
  ```
    {
        "access_token": "eyNjE0NDAwMTNiZjgwNTciLCJhdWQiOiJodHRwczovL2FwaSIsImlhdCI6MTU5MjEzNjQwMSw5ralN2Yzk2TkhQWUZ0TnNwUlFQd09zMzVZRlZmciIsInNjb3BlIjoicmVhZDptZs9y_-qMl3fr7X3sYHSOmf_cp_SkeiNOtc97MAV7PeZ",
        "expires_in": 86400,
        "token_type": "Bearer"
    }
  ```

  * YOUR_DOMAIN
  ```
     From dashboard go to  Applications -> service-starter-dotnetcore-application -> settings -> Domain
  ```
  *  YOUR_API_IDENTIFIER
  ```
     From dashboard go to  Api -> service-starter-dotnetcore-api -> settings -> identifier
  ```
  * YOUR_CLIENT_ID
  ```
     From dashboard go to  Applications -> service-starter-dotnetcore-application -> settings -> Client ID
  ```
  * YOUR_CLIENT_SECRET
  ```
     From dashboard go to  Applications -> service-starter-dotnetcore-application -> settings -> Client Secret
  ```

### Configuring service starter dotnetcore with auth0
  * To enable authentication, go to `appsettings.Development.json` and set `Auth Section`.
  
  ```
    "Auth": {
    "Enabled": "true",
    "Authority": "https://service-starter-dotnetcore.us.auth0.com/",
    "Audience": "http://localhost:5000"
  }
  ```

### Calling the resource server(service starter) using access token
  * By getting the access token{ACCESS_TOKEN} we can call the resource server(service starter dotnetcore) to access the apis.
  * We can access the service starter dotnetcore apis using below url
  ```
curl --request GET \
  --url http://localhost:5000/api/check \
  --header 'authorization: Bearer {ACCESS_TOKEN}' \
  
  ```
