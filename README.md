# JWTSimpleServer
A light-weight, dynamic jwt server for ASP.NET Core 2.0

## What is it?
Prompted by the need for an easy-to-use JWT Server in ASP.NET Core 2.0 without a lot ceremony configuration and easily extendable and also a way to renewed access token throught refresh tokens.

## Getting Started

1. Install the standard Nuget package into your ASP.NET Core application.

	```
    Install-Package Comming soon!
	```
2. Create your own IAuthenticationProvider for login purpose.

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

3. In the _ConfigureServices_ method of _Startup.cs_, register JWTSimpleServer, defining one refresh token store (Optional: By default we register [NoRefreshTokenStore](https://github.com/Xabaril/JWTSimpleServer/blob/c5aeca936105942b96a56419b56c42159896881d/src/JWTSimpleServer/JwtSimpleServerServiceCollectionExtensions.cs#L24) implementation).

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
  
4. In the _Configure_ method, insert middleware to expose the generated token.

	```csharp
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
          app.UseJwtSimpleServer(setup =>
          {
              setup.IssuerSigningKey = SigningKey;
          });
    }
	```
5. Now we can make calls to the token endpoint to retrieve a JWT token:

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
	
6. If we have previous registered a refresh token store, we can make calls to refresh tokens

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
