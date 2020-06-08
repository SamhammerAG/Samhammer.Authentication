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

            MapRoleClaim(claimsIdentity);
            MapNameClaim(claimsIdentity);

            return Task.FromResult(principal);
        }

        private void MapRoleClaim(ClaimsIdentity claimsIdentity)
        {
            var roles = TokenRoleResolver.GetRoles(claimsIdentity, AuthOptions.Value.ClientId);

            foreach (var role in roles)
            {
                claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, role));
            }
        }

        private void MapNameClaim(ClaimsIdentity claimsIdentity)
        {
            if (claimsIdentity.HasClaim(c => c.Type == ClaimTypes.Name))
            {
                return;
            }

            if (claimsIdentity.TryGetClaim(c => c.Type == "preferred_username", out var claim))
            {
                claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, claim.Value));
                return;
            }

            throw new AuthenticationException($"Claim {ClaimTypes.Name} is missing.");
        }
    }
}
