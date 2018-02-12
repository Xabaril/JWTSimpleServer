# JWTSimpleServer
A light-weight, dynamic jwt server for ASP.NET Core 2.0

## What is the motivation behind it?

JWT Simple server arises from the need of having an ease-to-use JWT server in ASP.NET, avoiding the user all the ceremony configuration and providing additional features.

## What JWT Simple Server offers?

* Easy to use JWT Server, configured with a few lines of code.
* Flexible and customizable. You can provide your own authentication and store mechanisms.
* Implements middleware that exposes the token endpoint so you don't have to create and mantain your own.
* Provides refresh tokens feature with several store implementations (InMemory, Entity Framework, Redis, Message Pack).
* Provides a typescript library that will allow you to interact with JWT Server easily. This library offers a JWT Client to request and refresh access tokens and a refresh token automatic renewal service.

## Getting Started

1. Install the standard Nuget package into your ASP.NET Core application.

	```
    Install-Package Comming soon!
	```
2. Create your own IAuthenticationProvider for user authentication. You should execute context.success and provide the user claims that will be encoded in the token or context.Reject if the authentication was not successful.

  	```csharp
    public class CustomAuthenticationProvider : IAuthenticationProvider
    {
        public Task ValidateClientAuthentication(JwtSimpleServerContext context)
        {
            if(context.UserName == "demo" && context.Password == "demo")
            {
                var claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.Name, "demo"));

                context.Success(claims);
            }
            else
            {
                context.Reject("Invalid user authentication");
            }
            
            return Task.CompletedTask;            
        }
    }
  	```

3. In the _ConfigureServices_ method of _Startup.cs_, register JWTSimpleServer services, defining one refresh token store (Optional: By default we register [NoRefreshTokenStore](https://github.com/Xabaril/JWTSimpleServer/blob/c5aeca936105942b96a56419b56c42159896881d/src/JWTSimpleServer/JwtSimpleServerServiceCollectionExtensions.cs#L24) implementation).

	```csharp
    public void ConfigureServices(IServiceCollection services)
    {
        services
            .AddSingleton<IAuthenticationProvider, CustomAuthenticationProvider>()
            .AddJwtSimpleServer(setup =>
            {
                setup.IssuerSigningKey = SigningKey;
            })
            .AddJwtInMemoryRefreshTokenStore();
    }
	```
  
4. In the _Configure_ method, add the middleware to the server exposing the token endpoint and handling it's requests.

	```csharp
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
          app.UseJwtSimpleServer(setup =>
          {
              setup.IssuerSigningKey = SigningKey;
          });
    }
	```
5. Two grant types are supported right now by the server: **_password_** and **_refresh_token_**

	A **_password_** grant type request will require username and password parameters and will allow you to obtain a **_request token_**.

	Sample request:
	```html
	POST https://localhost:44305/Token HTTP/1.1
	Host: localhost:44305
	User-Agent: Mozilla/5.0 (Windows NT 6.3; WOW64; rv:32.0) Gecko/20100101 Firefox/32.0
	Accept: */*
	Content-Type: application/x-www-form-urlencoded; charset=UTF-8
	X-Requested-With: XMLHttpRequest
	Referer: https://localhost:44305/
	Content-Length: 68

	grant_type=password&username=demo&password=demo
	```
	HTTP Response
	
	```json
	{
		"access_token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9....",
		"expires_in": 900,
		"refresh_token": "77e248a4a3814308931d63b10fb1e7f7"
	}
	```
	
   A **_refresh_token_** grant type will allow you to create a new access token with a new expiry time and obtain a new **_refresh token_**. (The previous refresh token will be invalidated once used).

   The required parameter for this grant type is the refresh token you were previously provided.

	Sample request:
	```html
	POST https://localhost:44305/Token HTTP/1.1
	Host: localhost:44305
	User-Agent: Mozilla/5.0 (Windows NT 6.3; WOW64; rv:32.0) Gecko/20100101 Firefox/32.0
	Accept: */*
	Content-Type: application/x-www-form-urlencoded; charset=UTF-8
	X-Requested-With: XMLHttpRequest
	Referer: https://localhost:44305/
	Content-Length: 68

	grant_type:refresh_token&refresh_token:77e248a4a3814308931d63b10fb1e7f7
	```
	
	HTTP Response
	
	```json
	{
		"access_token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9....",
		"expires_in": 900,
		"refresh_token": "3521442655fc4ec5b41a1b2d9ce846aa"
	}
	```
