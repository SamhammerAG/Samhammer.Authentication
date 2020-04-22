using System;
using System.Security.Claims;

namespace Samhammer.Authentication.Api
{
    internal static class ClaimsIdentityExtensions
    {
        public static bool TryGetClaim(this ClaimsIdentity claimsIdentity, Predicate<Claim> match, out Claim claim)
        {
            if (claimsIdentity.HasClaim(match))
            {
                claim = claimsIdentity.FindFirst(match);
                return true;
            }

            claim = null;
            return false;
        }
    }
}
