using Microsoft.AspNetCore.Authentication;

namespace Samhammer.Authentication.Api.Guest
{
    public class GuestAuthOptions : AuthenticationSchemeOptions
    {
        public bool Enabled { get; set; } = true;

        public string Name { get; set; }

        public string Role { get; set; }
    }
}
