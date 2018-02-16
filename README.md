[![Build status](https://ci.appveyor.com/api/projects/status/xkosic4gm7salll3?svg=true)](https://ci.appveyor.com/project/Xabaril/jwtsimpleserver)[![MyGet CI](https://img.shields.io/myget/xabaril/v/JWTSimpleServer.svg)](http://myget.org/gallery/xabaril) [![NuGet](https://img.shields.io/nuget/v/JWTSimpleServer.svg)](https://www.nuget.org/packages/xabaril/)

# JWT Simple Server
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

	A **_password_** grant type request will require username and password parameters and will allow you to obtain an **_access token_**.

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
	
   A **_refresh_token_** grant type will allow you to generate a new access token with a new expiry time and obtain a new **_refresh token_**. (The previous refresh token will be invalidated once used).

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

## Available stores

JWT Simple Server has four different store implementations:

 * In-memory store

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

 * Entity framework store
 
 ```csharp
     public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddScoped<IAuthenticationProvider, CustomAuthenticationProvider>()
                .AddJwtSimpleServer(options => options.IssuerSigningKey = SigningKey)
                .AddJwtEntityFrameworkCoreRefreshTokenStore(options =>
                {
                    options.ConfigureDbContext = builder =>
                    {
                        builder.UseSqlServer(
                            Configuration["ConnectionStrings:DefaultConnection"],
                            sqlServerOptions => sqlServerOptions.MigrationsAssembly(typeof(Startup).Assembly.FullName));
                    };
                });                
        }
  ```
 * Redis store
   
  ```csharp
  public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IAuthenticationProvider, CustomAuthenticationProvider>()
            .AddJwtSimpleServer(setup =>
            {
                setup.IssuerSigningKey = SigningKey;
            })
            .AddDistributedRedisRefreshStokenStore( setup =>
            {
              setup.Configuration = "localhost"; //Provide your redis server configuration
              setup.InstanceName = "JwtSimpleServerInstance";
            });          
        }
  ```

 * Message pack binary store

  ```csharp
  public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IAuthenticationProvider, CustomAuthenticationProvider>()
            .AddJwtSimpleServer(setup =>
            {
                setup.IssuerSigningKey = SigningKey;
            })
            .AddJwtMessagePackRefreshTokenStore(setup =>
            {
                setup.Path = "MyBinaryStore.bin";
            });           
        }
   ```

 You can create your own store service by implementing __IRefreshTokenStore__ interface and registering it in the inversion of control container.

## Samples
 
 We have some samples with different store configurations available [here](https://github.com/Xabaril/JWTSimpleServer/tree/master/samples).

 If you launch the projects you can try a simple playground  to get access tokens and try the refresh token renewal service.

 ![JWTSimpleServer playground](https://preview.ibb.co/mkhSAn/playground.png)


 ## Typescript library

 The typescript library will allow you to easily interact will the token endpoint.

 Follow this steps to create your client if you are using the **browser** bundled library:

 **NPM - Installing the library**
 ```
 npm install jwt-simpleserver-client --save
```

 **1. Create the client options**

  ```javascript
  var defaultServerOptions = new JwtSimpleServer.ClientOptions();
  ```

  Client options parameters have default values listed in this table:

| Parameter  |  default value  |
|--:|---|
| tokenEndpoint  | "/token"  |
|  host | window.location.origin   |
| httpClient  | XMLHttpRequestClient   |
    
NOTE: You can implement your own **HttpClient** by implementing our HttpClient abstract class


**2. Creat the client providing the options object:**

 ```javascript
 var simpleServerClient =  new JwtSimpleServer.ServerClient(defaultServerOptions);
 ```

 3. Request an access token by executing _requestAccessToken_ method:

 ```javascript
 simpleServerClient.requestAccessToken({ userName: "demo", password: "demo" })
 	.then(token => {
	  // your token object will have the access token and expiral, and if configured: the refresh token
    }):
 ```

 *_Client events_

 JWT client have several observables you can subscribe to:

 | Observable  |  return value  | description |
|--:|--: |---|
| onBeforeRequestAccessToken  | void  | Will notify observers before starting the token request to the server |
|  onRequestAccessTokenSuccess | Token   | Will notify observers passing the retrieved token as parameter |
| onBeforeRequestRefreshToken  | void   | Will notify observers before starting the refresh token request to the server |
| onRequestRefreshTokenSuccess  | Token   | Will notify observers passing the retrieved refresh token as parameter  |



 
 **4. Optional: If you want the library to request new access tokens given an interval you can configure the __RefreshTokenService__**

 ```javascript
 var refreshService = new JwtSimpleServer.RefreshTokenService(simpleServerClient);

 let onTokenRefreshedFunction = (token) => {
    console.log("Refresh token service:", token);
 }

 //Start the renewal service
 refreshService.start({
    intervalSeconds: 10,
    refreshToken,
    onRefreshTokenSuccessCallback: onTokenRefreshedFunction
 });

 //Stop the renewal service
 refreshService.stop();
  ```

