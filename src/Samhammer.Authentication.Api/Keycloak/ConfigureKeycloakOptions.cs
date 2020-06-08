using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Samhammer.Authentication.Abstractions;

namespace Samhammer.Authentication.Api.Keycloak
{
    public class ConfigureKeycloakOptions : IConfigureNamedOptions<JwtBearerOptions>
    {
        private IOptions<ApiAuthOptions> ApiAuthOptions { get; }

        public ConfigureKeycloakOptions(IOptions<ApiAuthOptions> apiAuthOptions)
        {
            ApiAuthOptions = apiAuthOptions;
        }

        public void Configure(string name, JwtBearerOptions options)
        {
            if (string.Equals(name, JwtBearerDefaults.AuthenticationScheme))
            {
                Configure(options);
            }
        }

        public void Configure(JwtBearerOptions options)
        {
            options.Authority = ApiAuthOptions.Value.Issuer;
            options.Audience = ApiAuthOptions.Value.ClientId;
        }
    }
}
