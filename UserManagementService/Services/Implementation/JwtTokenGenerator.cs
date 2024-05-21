using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserManagementService.Models;
using UserManagementService.Services.Contract;

namespace UserManagementService.Services.Implementation
{
    public class JwtTokenGenerator : ITokenGenerator
    {
        private readonly JwtOptions _jwtOptions;

        public JwtTokenGenerator(IOptions<JwtOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions.Value;
        }
        public string GenerateToken(ApplicationUser User, IEnumerable<string> roles)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var keys = Encoding.UTF8.GetBytes(_jwtOptions.Secret);
            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Email,User.Email),
                new Claim(JwtRegisteredClaimNames.Sub,User.Id),
                new Claim(JwtRegisteredClaimNames.Name,User.UserName),
                new Claim("role",roles.FirstOrDefault()?.ToUpper()),
                //new claim("Platform","IOS"),
            };
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role.ToUpper())));
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Issuer = _jwtOptions.Issuer,
                Audience = _jwtOptions.Audience,
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keys), SecurityAlgorithms.HmacSha256)
            };
            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            return jwtTokenHandler.WriteToken(token);

        }
    }
}
