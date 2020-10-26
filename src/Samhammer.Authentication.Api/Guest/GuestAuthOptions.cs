using Microsoft.AspNetCore.Authentication;

namespace Samhammer.Authentication.Api.Guest
{
    public class GuestAuthOptions : AuthenticationSchemeOptions
    {
        public bool Enabled { get; set; }

        public string Name { get; set; }

        public string Role { get; set; }

        public string Validator { get; set; }

        public GuestAuthOptions()
        {
            Name = $"guest-{GuestAuthenticationDefaults.Placeholder}";
            Validator = "[0-9a-fA-F]{8}\\-[0-9a-fA-F]{4}\\-[0-9a-fA-F]{4}\\-[0-9a-fA-F]{4}\\-[0-9a-fA-F]{12}"; // uuid v4
            Enabled = true;
        }
    }
}
