using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Skaters.Models.CustomClass;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Skaters.Repositories.AuthRepositories
{
    public class TokenRepository:ITokenRepository
    {
        private readonly IConfiguration configuration;

        public TokenRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public string CreateToken(Applicationuser user, List<string> roles,string userId)
        {
           
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.UserData,userId));
            claims.Add(new Claim(ClaimTypes.Email, user.Email));
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                configuration["Jwt:Issuer"],
                configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);

        }
    }
}
