using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Samhammer.Authentication.Api.Keycloak
{
    public static class AuthenticationBuilderExtensions
    {
        public static AuthenticationBuilder AddKeycloak(this AuthenticationBuilder builder, Action<ApiAuthOptions> configureOptions)
        {
            builder.Services.Configure(configureOptions);
            builder.AddKeycloak();
            return builder;
        }

        public static AuthenticationBuilder AddKeycloak(this AuthenticationBuilder builder, IConfiguration configuration)
        {
            builder.Services.Configure<ApiAuthOptions>(configuration.GetSection(nameof(ApiAuthOptions)));
            builder.AddKeycloak();
            return builder;
        }

        private static void AddKeycloak(this AuthenticationBuilder builder)
        {
            builder.Services.AddSingleton<IClaimsTransformation, KeycloakClaimsTransformation>();

            builder.Services.AddSingleton<IConfigureOptions<JwtBearerOptions>, ConfigureKeycloakOptions>();
            builder.AddJwtBearer();
        }
    }
}
