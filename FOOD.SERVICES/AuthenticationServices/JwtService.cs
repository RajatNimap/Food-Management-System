using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using FOOD.DATA.Entites;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace FOOD.SERVICES.AuthenticationServices
{
    public class JwtService : IJwtService   
    {
        private readonly IConfiguration config;
        public JwtService(IConfiguration _config)
        {
            config = _config;
        }
        public string JwtAccessToken(User user)
        {
            var SecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:Key"]));
            var Credential = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256);
            var Claims = new[]
            {

                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim (JwtRegisteredClaimNames.Email,user.Email),
                new Claim (JwtRegisteredClaimNames.Name,user.Name),
                new Claim (ClaimTypes.Role,user.Role.ToString())

            };
            var Token = new JwtSecurityToken(

                issuer: config["JWT:Issuer"],
                audience: config["JWT:Audience"],
                claims: Claims,
                expires: DateTime.UtcNow.AddHours(3),
                signingCredentials: Credential

            );

            return new JwtSecurityTokenHandler().WriteToken(Token);
        }

        public string JwtRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
    }
}
