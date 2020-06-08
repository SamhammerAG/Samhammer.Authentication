using System;
using System.Collections.Generic;
using System.Security.Claims;
using Newtonsoft.Json;
using Samhammer.Authentication.Abstractions;

namespace Samhammer.Authentication.Api.Keycloak
{
    internal class TokenRoleResolver
    {
        private static readonly Predicate<Claim> ResourceAccessMatcher = claim => claim.Type == "resource_access";
        private static readonly string RolesKeyName = "roles";

        /// <summary>
        /// Gets all roles of the running client (audience).
        /// </summary>
        /// <remarks>
        /// The resource_access inside a token looks like this:
        /// {
        ///      "clientId1": {
        ///          "roles": ["role1", "role2"]
        ///      },
        ///      "clientId2": {
        ///          "roles": ["role3", "role4"]
        ///      }
        /// }.
        /// </remarks>
        /// <param name="identity">The current identity.</param>
        /// <param name="clientId">The id if a client.</param>
        /// <returns>A array of roles.</returns>
        public static string[] GetRoles(ClaimsIdentity identity, string clientId)
        {
            var retVal = new string[] { };

            if (identity.TryGetClaim(ResourceAccessMatcher, out var resourceAccessClaim))
            {
                var resourceAccess = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string[]>>>(resourceAccessClaim.Value);

                if (resourceAccess.TryGetValue(clientId, out var clientResourceAccess)
                    && clientResourceAccess.TryGetValue(RolesKeyName, out var roles))
                {
                    retVal = roles;
                }
            }

            return retVal;
        }
    }
}
