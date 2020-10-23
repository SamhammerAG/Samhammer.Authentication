namespace Samhammer.Authentication.Api.Guest
{
    public static class GuestAuthenticationDefaults
    {
        public const string AuthenticationScheme = "Guest";
        public const string HeaderKey = "X-GuestSession";
        public const string ClaimKey = "GuestSession";
    }
}
