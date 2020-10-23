using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;

namespace Samhammer.Authentication.Api.Guest
{
    public static class AuthenticationBuilderExtensions
    {
        public static AuthenticationBuilder AddGuest(this AuthenticationBuilder builder)
        {
            builder.AddGuest(_ => { });
            return builder;
        }

        public static AuthenticationBuilder AddGuest(this AuthenticationBuilder builder, Action<GuestAuthOptions> configureOptions)
        {
            builder.AddGuest(GuestAuthenticationDefaults.AuthenticationScheme, configureOptions);
            return builder;
        }

        public static AuthenticationBuilder AddGuest(this AuthenticationBuilder builder, string scheme, Action<GuestAuthOptions> configureOptions)
        {
            builder.AddScheme<GuestAuthOptions, GuestAuthenticationHandler>(scheme, configureOptions);
            return builder;
        }

        public static AuthenticationBuilder AddGuest(this AuthenticationBuilder builder, IConfiguration configuration)
        {
            builder.AddGuest(GuestAuthenticationDefaults.AuthenticationScheme, configuration);
            return builder;
        }

        public static AuthenticationBuilder AddGuest(this AuthenticationBuilder builder, string scheme, IConfiguration configuration)
        {
            builder.AddGuest(scheme, options =>
            {
                var section = configuration.GetSection(nameof(GuestAuthOptions));
                section.Bind(options);
            });
            return builder;
        }
    }
}
