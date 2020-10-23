using System.Security.Claims;

namespace Samhammer.Authentication.Api.Guest
{
    public static class ClaimsPrincipalExtensions
    {
        public static bool IsGuest(this ClaimsPrincipal user)
        {
            return string.Equals(user.Identity.AuthenticationType, GuestAuthenticationDefaults.AuthenticationScheme);
        }

        public static string GetGuestKey(this ClaimsPrincipal user)
        {
            return user.HasClaim(c => c.Type == GuestAuthenticationDefaults.ClaimKey)
                ? user.FindFirst(GuestAuthenticationDefaults.ClaimKey).Value
                : null;
        }
    }
}
