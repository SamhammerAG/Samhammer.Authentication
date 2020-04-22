﻿[![Build Status](https://travis-ci.com/SamhammerAG/Samhammer.Authentication.svg?branch=master)](https://travis-ci.com/SamhammerAG/Samhammer.Authentication)

## Samhammer.Authentication.Api

This provides a way to secure your api with keycloak jwt bearer authentication.

#### How to add this to your project:
- reference this nuget package: https://www.nuget.org/packages/Samhammer.Authentication.Api/

#### How to use:

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

## Contribute

#### How to publish a nuget package
- set package version in Samhammer.Auth.Api.csproj
- create git tag
- dotnet pack -c Release
- nuget push .Samhammer.Swagger.Default\bin\Release\Samhammer.Auth.Api.*.nupkg NUGET_API_KEY -src https://api.nuget.org/v3/index.json
- (optional) nuget setapikey NUGET_API_KEY -source https://api.nuget.org/v3/index.json
