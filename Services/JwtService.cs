using api.Models;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace api.Services
{
    public class JwtService
    {
        private readonly IConfiguration _config;
        private readonly SymmetricSecurityKey _key;

        public JwtService(IConfiguration config)
        {
            _config = config;
            _key = new SymmetricSecurityKey(System.Text.
                    Encoding.UTF8.GetBytes(_config["JWT:SigningKey"]!));
        }
        public string GenerateJwtToken(AppUser user, List<string> roles)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.GivenName, user.UserName!),
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = creds,
                Issuer = _config["JWT:Issuer"],
                Audience = _config["JWT:Audience"],
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
