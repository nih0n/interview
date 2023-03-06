using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Solution.Application.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Solution.Infrastructure.Authentication
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly JwtOptions _options;

        public JwtTokenGenerator(IOptions<JwtOptions> options) => _options = options.Value;

        public string GenerateToken()
        {
            var secret = Encoding.ASCII.GetBytes(_options.Secret);

            var credentials = new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256);

            var descriptor = new SecurityTokenDescriptor
            {
                Issuer = _options.Issuer,
                Audience = _options.Audience,
                SigningCredentials = credentials
            };

            var handler = new JwtSecurityTokenHandler();

            var token = handler.CreateToken(descriptor);

            return handler.WriteToken(token);
        }
    }
}
