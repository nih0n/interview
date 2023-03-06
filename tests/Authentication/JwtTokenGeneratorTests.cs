using FluentAssertions;
using Microsoft.Extensions.Options;
using Solution.Infrastructure.Authentication;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Xunit;

namespace Solution.Tests.Authentication
{
    public class JwtTokenGeneratorTests
    {
        private readonly JwtOptions _options;
        private readonly JwtTokenGenerator _generator;

        public JwtTokenGeneratorTests()
        {
            _options = new JwtOptions
            {
                Secret = "5v8x/A?D(G+KbPeS\r\n",
                Issuer = "Test",
                Audience = "Test"
            };

            _generator = new JwtTokenGenerator(Options.Create(_options));
        }

        [Fact]
        public void Generate_a_valid_token()
        {
            var token = _generator.GenerateToken();

            token.Should().NotBeEmpty();

            var handler = new JwtSecurityTokenHandler();
            var decodedToken = handler.ReadJwtToken(token);

            decodedToken.Issuer.Should().Be(_options.Issuer);
            decodedToken.Audiences.First().Should().Be(_options.Audience);
        }
    }
}
