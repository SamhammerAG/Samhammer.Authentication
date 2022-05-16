using System.Linq;
using System.Security.Authentication;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Samhammer.Authentication.Abstractions;

namespace Samhammer.Authentication.Api.Keycloak
{
    /// <summary>
    /// Microsoft Identity model does not support keycloaks resource_access.
    /// Add all roles to a flat list in claimsIdentity.
    /// </summary>
    public class KeycloakClaimsTransformation : IClaimsTransformation
    {
        private IOptions<ApiAuthOptions> AuthOptions { get; }

        public KeycloakClaimsTransformation(IOptions<ApiAuthOptions> authOptions)
        {
            AuthOptions = authOptions;
        }

        public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            var claimsIdentity = (ClaimsIdentity)principal.Identity;

            if (ShouldTransform(claimsIdentity))
            {
                AddTransformClaim(claimsIdentity);
                MapRoleClaim(claimsIdentity);
                MapNameClaim(claimsIdentity);
            }

            return Task.FromResult(principal);
        }

        private bool ShouldTransform(ClaimsIdentity claimsIdentity)
        {
            // skip transform for identities that were already transformed
            if (claimsIdentity.HasClaim(c => c.Type == nameof(KeycloakClaimsTransformation)))
            {
                return false;
            }

            // only transform for identities issued by keyCloak
            if (claimsIdentity.TryGetClaim(c => c.Type == "iss", out var issClaim))
            {
                return string.Equals(issClaim.Value, AuthOptions.Value.Issuer);
            }

            return false;
        }

        private void AddTransformClaim(ClaimsIdentity claimsIdentity)
        {
            claimsIdentity.AddClaim(new Claim(nameof(KeycloakClaimsTransformation), "true"));
        }

        private void MapRoleClaim(ClaimsIdentity claimsIdentity)
        {
            var roles = TokenRoleResolver.GetRoles(claimsIdentity, AuthOptions.Value.ClientId);

            foreach (var role in roles)
            {
                claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, role));
            }
        }

        public void MapNameClaim(ClaimsIdentity claimsIdentity)
        {
            if (claimsIdentity.TryGetClaim(c => c.Type == AuthOptions.Value.NameClaim, out var claimToSet))
            {
                var nameClaim = claimsIdentity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
                claimsIdentity.TryRemoveClaim(nameClaim);
                claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, claimToSet.Value));
                return;
            }

            throw new AuthenticationException($"Claim {AuthOptions.Value.NameClaim} is missing.");
        }
    }
}
