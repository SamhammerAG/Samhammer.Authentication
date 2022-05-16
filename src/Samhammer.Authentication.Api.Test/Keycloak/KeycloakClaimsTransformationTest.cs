using System.Security.Authentication;
using System.Security.Claims;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Samhammer.Authentication.Abstractions;
using Samhammer.Authentication.Api.Keycloak;
using Xunit;

namespace Samhammer.Authentication.Api.Test.Keycloak
{
    public class KeycloakClaimsTransformationTest
    {
        public KeycloakClaimsTransformation Transformation { get; }

        public IOptions<ApiAuthOptions> ApiAuthOptions { get; }

        public ClaimsIdentity ClaimsIdentity { get; }

        public KeycloakClaimsTransformationTest()
        {
            ApiAuthOptions = Options.Create(new ApiAuthOptions());
            Transformation = new KeycloakClaimsTransformation(ApiAuthOptions);

            ClaimsIdentity = new ClaimsIdentity();
            ClaimsIdentity.AddClaim(new Claim(ClaimTypes.Name, "Max Mustermann"));
            ClaimsIdentity.AddClaim(new Claim(ClaimTypes.Email, "mmustermann@sag.de"));
            ClaimsIdentity.AddClaim(new Claim("preferred_username", "mmustermann"));
        }

        [Fact]
        public void MapNameClaimDefault()
        {
            Transformation.MapNameClaim(ClaimsIdentity);

            ClaimsIdentity.TryGetClaim(c => c.Type == ClaimTypes.Name, out var claim).Should().BeTrue();
            claim.Value.Should().Be("mmustermann");
        }

        [Fact]
        public void MapNameClaimEmail()
        {
            ApiAuthOptions.Value.NameClaim = ClaimTypes.Email;
            
            Transformation.MapNameClaim(ClaimsIdentity);

            ClaimsIdentity.TryGetClaim(c => c.Type == ClaimTypes.Name, out var claim).Should().BeTrue();
            claim.Value.Should().Be("mmustermann@sag.de");
        }

        [Fact]
        public void MapNameClaimNotExists()
        {
            ApiAuthOptions.Value.NameClaim = "asfgdfhgdfkghlfkdgh";

            Transformation
                .Invoking(s => s.MapNameClaim(ClaimsIdentity))
                .Should().Throw<AuthenticationException>();
        }
    }
}
