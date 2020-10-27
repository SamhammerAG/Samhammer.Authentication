using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Samhammer.Authentication.Api.Guest
{
    public class GuestAuthenticationHandler : AuthenticationHandler<GuestAuthOptions>
    {
        public GuestAuthenticationHandler(IOptionsMonitor<GuestAuthOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            try
            {
                if (!Options.Enabled)
                {
                    return Task.FromResult(AuthenticateResult.NoResult());
                }

                if (!Context.Request.Headers.ContainsKey(GuestAuthenticationDefaults.HeaderKey))
                {
                    return Task.FromResult(AuthenticateResult.NoResult());
                }

                var guestId = Context.Request.Headers[GuestAuthenticationDefaults.HeaderKey];

                if (!IsAuthorized(guestId))
                {
                    Logger.LogInformation("Failed to validate the GuestID");
                    return Task.FromResult(AuthenticateResult.Fail("Guest authentication failed"));
                }

                Logger.LogInformation("Successfully validated the GuestID.");

                var claims = CreateClaims(guestId);
                var identity = new ClaimsIdentity(claims, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);

                return Task.FromResult(AuthenticateResult.Success(ticket));
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Exception occurred on authenticate with GuestID.");
                return Task.FromResult(AuthenticateResult.Fail("Guest authentication error"));
            }
        }

        private bool IsAuthorized(string guestId)
        {
            if (string.IsNullOrEmpty(Options.Validator))
            {
                return !string.IsNullOrEmpty(guestId);
            }

            return Regex.IsMatch(guestId, Options.Validator);
        }

        private List<Claim> CreateClaims(string guestId)
        {
            var name = Options.Name.Replace(GuestAuthenticationDefaults.Placeholder, guestId, StringComparison.OrdinalIgnoreCase);

            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, name));
            claims.Add(new Claim(GuestAuthenticationDefaults.ClaimKey, guestId));

            if (!string.IsNullOrEmpty(Options.Role))
            {
                claims.Add(new Claim(ClaimTypes.Role, Options.Role));
            }

            return claims;
        }
    }
}
