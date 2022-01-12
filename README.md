[![Build Status](https://travis-ci.com/SamhammerAG/Samhammer.Authentication.svg?branch=master)](https://travis-ci.com/SamhammerAG/Samhammer.Authentication)

## Samhammer.Authentication.Api

This provides a way to secure your api with keycloak jwt bearer authentication.

#### How to add this to your project:
- reference this nuget package: https://www.nuget.org/packages/Samhammer.Authentication.Api/

#### How to use:

### Keycloak JWT Authentication

Add it to your api.
```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddJwtAuthentication()
        .AddKeycloak(Configuration);
}

public void Configure(IApplicationBuilder app)
{
    app.UseAuthentication();
    app.UseAuthorization();
}
```

Api calls requires auhorization header with an JWT token from keycloak.
```curl
POST https://myapi/action HTTP/1.1
Auhorization: Bearer JwtTokenContent
```

If you pass "IConfiguration" instead of "Action\<ApiAuthOptions\>" to "AddKeycloak" add the following to appsettings.json:
```js
  "ApiAuthOptions": {
    "Issuer": "<<KeycloakTokenIssuerUrl>>",
    "ClientId": "<<ClientIdRepresentingYourApp>>"
  }
```

### Guest Authentication

Add it to your api.
```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddAuthentication(GuestAuthenticationDefaults.AuthenticationScheme)
        .AddGuest(Configuration);
}
```

Api calls requires header guestid with an "Version 4 UUID".
```curl
POST https://myapi/action HTTP/1.1
guestid: 1c11792b-538f-4908-992d-6570bb268e60
```

If you pass "IConfiguration" instead of "Action\<GuestAuthOptions\>" to "AddGuest" you can can override the default settings in appsettings.json:
```js
  "GuestAuthOptions": {
    "Enabled": true,
    "Name": "guest-[GuestID]",    
    "Role": "SomeGuestRole",
    "Validator": "[0-9a-fA-F]{8}\\-[0-9a-fA-F]{4}\\-[0-9a-fA-F]{4}\\-[0-9a-fA-F]{4}\\-[0-9a-fA-F]{12}"
  }
```

### Mixed Authentication
You can also setup both authentication types. In the example below jwt keycloak will be the default.

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddJwtAuthentication()
        .AddKeycloak(Configuration)
        .AddGuest(Configuration);
}
```

You can setup your supported authentication types on each controller action per attribute.

```csharp
[HttpPost]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme + ", " + GuestAuthenticationDefaults.AuthenticationScheme)]
public async Task<IActionResult> ActionForBoth()
{}

[HttpPost]
[Authorize(GuestAuthenticationDefaults.AuthenticationScheme)]
public async Task<IActionResult> ActionForGuests()
{}
```


## Contribute

#### How to publish a nuget package
- Create a tag and lat the github action do the publishing for you
