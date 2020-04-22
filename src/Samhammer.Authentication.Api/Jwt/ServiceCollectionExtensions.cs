using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Samhammer.Authentication.Api.Jwt
{
    public static class ServiceCollectionExtensions
    {
        public static AuthenticationBuilder AddJwtAuthentication(this IServiceCollection services)
        {
            services.AddTransient<IConfigureOptions<AuthenticationOptions>, ConfigureAuthenticationOptions>();
            return services.AddAuthentication();
        }
    }
}
