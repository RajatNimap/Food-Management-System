using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using FOOD.DATA.Entites;
using FOOD.DATA.Infrastructure;
using FOOD.MODEL.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;


namespace FOOD.SERVICES.AuthenticationServices
{
    public class Auth : IAuth
    {

        private readonly IUnitOfWork unitOfWork;
        private readonly IConfiguration config;
        public Auth(IUnitOfWork unitOfWork,IConfiguration config)
        {
            this.unitOfWork = unitOfWork;
            this.config = config;
        }
        public async Task<string> IsAuthenticated(LoginModel login)
        {
            var user = await unitOfWork.UserRepository.verifyMail(login.Email);
            if (user == null)
            {
                return string.Empty;
            }
            var verifyCredential = BCrypt.Net.BCrypt.Verify(login.Password, user.Password);
          
              if(verifyCredential == false)
               {
                return string.Empty;
               }
         

            return GenerateToken(user);
           
        }

        private string GenerateToken(User user) {

            var SecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:Key"]));
            var Credential = new SigningCredentials(SecurityKey,SecurityAlgorithms.HmacSha256);
            var Claims = new[]
            {

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


    }
}
