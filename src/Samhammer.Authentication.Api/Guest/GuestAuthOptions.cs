using Microsoft.AspNetCore.Authentication;

namespace Samhammer.Authentication.Api.Guest
{
    public class GuestAuthOptions : AuthenticationSchemeOptions
    {
        public bool Enabled { get; set; }

        public string Name { get; set; }

        public string Role { get; set; }

        public GuestAuthOptions()
        {
            // set default values
            Name = $"guest-{GuestAuthenticationDefaults.Placeholder}";
            Enabled = true;
        }
    }
}
