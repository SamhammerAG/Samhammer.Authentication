namespace Samhammer.Authentication.Abstractions
{
    public class ApiAuthOptions
    {
        public string Issuer { get; set; }

        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public string AccessTokenUrl { get; set; }

        public string NameClaim { get; set; } = "preferred_username";

        public const string DefaultClientName = "default";
    }
}
