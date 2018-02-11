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

3. In the _ConfigureServices_ method of _Startup.cs_, register Xabaril, defining one or more features.

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
  
4. In the _Configure_ method, insert middleware to expose the generated Xabaril as JSON endpoint.

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

	```
	```
