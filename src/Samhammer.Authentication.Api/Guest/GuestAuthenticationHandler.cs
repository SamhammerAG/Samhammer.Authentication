using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
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

                if (!IsAuthorized())
                {
                    return Task.FromResult(AuthenticateResult.Fail("UserSession authentication failed"));
                }

                var claims = CreateClaims();
                var identity = new ClaimsIdentity(claims, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);

                return Task.FromResult(AuthenticateResult.Success(ticket));
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "UserSession authentication error");
                return Task.FromResult(AuthenticateResult.Fail("UserSession authentication error"));
            }
        }

        private bool IsAuthorized()
        {
            var sessionId = GetSessionId();
            return !string.IsNullOrEmpty(sessionId);
        }

        private List<Claim> CreateClaims()
        {
            var sessionId = GetSessionId();
            var claims = new List<Claim>();

            claims.Add(new Claim(GuestAuthenticationDefaults.ClaimKey, sessionId));

            if (!string.IsNullOrEmpty(Options.Name))
            {
                claims.Add(new Claim(ClaimTypes.Name, Options.Name));
            }

            if (!string.IsNullOrEmpty(Options.Role))
            {
                claims.Add(new Claim(ClaimTypes.Role, Options.Role));
            }

            return claims;
        }

        private string GetSessionId()
        {
            return Context.Request.Headers.SingleOrDefault(h => h.Key.Equals(GuestAuthenticationDefaults.HeaderKey)).Value;
        }
    }
}
