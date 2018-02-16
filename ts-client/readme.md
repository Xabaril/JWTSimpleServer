## Javascript JWT Simple Server client library

Home Repository:

https://github.com/Xabaril/JWTSimpleServer


 **NPM - Installing the library**
 ```
 npm install jwt-simpleserver-client --save
```



 The typescript library will allow you to easily interact will the token endpoint.

 Follow this steps to create your client if you are using the **browser** bundled library:

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