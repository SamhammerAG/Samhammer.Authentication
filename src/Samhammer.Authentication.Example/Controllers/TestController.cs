using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Samhammer.Authentication.Api.Guest;

namespace Samhammer.Authentication.Example.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        [Authorize(AuthenticationSchemes = GuestAuthenticationDefaults.AuthenticationScheme + ", " + JwtBearerDefaults.AuthenticationScheme, Roles = "default")]
        [HttpGet("both")]
        public string Get()
        {
            return $"Welcome {User.Identity.Name}!";
        }

        [Authorize(Roles = "default")]
        [HttpGet("unspecific")]
        public string GetUnspecified()
        {
            return $"Welcome {User.Identity.Name}!";
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "default")]
        [HttpGet("keyCloakOnly")]
        public string GetKeyCloak()
        {
            return $"Welcome {User.Identity.Name}!";
        }

        [Authorize(AuthenticationSchemes = GuestAuthenticationDefaults.AuthenticationScheme, Roles = "default")]
        [HttpGet("userSessionOnly")]
        public string GetUserSession()
        {
            return $"Welcome {User.Identity.Name}!";
        }


        [Authorize(AuthenticationSchemes = GuestAuthenticationDefaults.AuthenticationScheme + ", " + JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("userInfo")]
        public ActionResult<object> GetUserInfo()
        {
            return new
            {
                loginName = User.Identity.Name,
                isGuest = User.IsGuest(),
                guestID = User.GetGuestID(),
                firstName = User.FindFirst(ClaimTypes.GivenName)?.Value,
                lastName = User.FindFirst(ClaimTypes.Surname)?.Value,
                roles = string.Join(", ", User.FindAll(ClaimTypes.Role).Select(c => c.Value))
            };
        }
    }
}
