[![Build Status](https://travis-ci.com/SamhammerAG/Samhammer.Authentication.svg?branch=master)](https://travis-ci.com/SamhammerAG/Samhammer.Authentication)

## Samhammer.Authentication.Api

This provides a way to secure your api with keycloak jwt bearer authentication.

#### How to add this to your project:
- reference this nuget package: https://www.nuget.org/packages/Samhammer.Authentication.Api/

#### How to use:

### Keycloak JWT Authentication

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

If you pass "IConfiguration" instead of "Action\<ApiAuthOptions\>" to "AddKeycloak" add the following to appsettings.json:
```js
  "ApiAuthOptions": {
    "Issuer": "<<KeycloakTokenIssuerUrl>>",
    "ClientId": "<<ClientIdRepresentingYourApp>>"
  }
```

### Guest Authentication

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddAuthentication(GuestAuthenticationDefaults.AuthenticationScheme)
        .AddGuest(Configuration);
}
```

If you pass "IConfiguration" instead of "Action\<GuestAuthOptions\>" to "AddGuest" you can can setup configuration by appsettings.json:
```js
  "GuestAuthOptions": {
    "Enabled": true,
    "Role": "SomeGuestRole"
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
...

[HttpPost]
[Authorize(GuestAuthenticationDefaults.AuthenticationScheme)]
public async Task<IActionResult> ActionForGuests()
```


## Contribute

#### How to publish a nuget package
- set package version in Samhammer.Authentication.Api.csproj
- set package version in Samhammer.Authentication.Abstractions.csproj
- create git tag
- dotnet pack -c Release
- nuget push .Samhammer.Authentication.Api\bin\Release\Samhammer.Authentication.Api.*.nupkg NUGET_API_KEY -src https://api.nuget.org/v3/index.json
- nuget push .Samhammer.Authentication.Abstractions\bin\Release\Samhammer.Authentication.Abstractions.*.nupkg NUGET_API_KEY -src https://api.nuget.org/v3/index.json
- (optional) nuget setapikey NUGET_API_KEY -source https://api.nuget.org/v3/index.json
