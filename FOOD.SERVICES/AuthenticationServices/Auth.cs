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
using System.Security.Cryptography;
using Azure.Core;

namespace FOOD.SERVICES.AuthenticationServices
{
    public class Auth : IAuth
    {

        private readonly IUnitOfWork unitOfWork;
        private readonly IJwtService jwtService;
        public Auth(IUnitOfWork unitOfWork,IConfiguration config, IJwtService jwtService    )
        {
            this.unitOfWork = unitOfWork;
            this.jwtService = jwtService;
        }

        public async Task<(string,string)> IsAuthenticated(LoginModel login)
        {
            var user = await unitOfWork.UserRepository.verifyMail(login.Email);
            if (user == null)
            {
                return  Task.FromResult((string.Empty, string.Empty)).Result;   
            }
            var verifyCredential = BCrypt.Net.BCrypt.Verify(login.Password, user.Password);
            if(verifyCredential == false)
            {
                return Task.FromResult((string.Empty, string.Empty)).Result;
            }
         
            var AccessToken = jwtService.JwtAccessToken(user);   
            var RefreshToken = jwtService.JwtRefreshToken();
            if (RefreshToken != null)
            {
               var refreshTokenData = new RefreshToken
               {
                   Token = RefreshToken,
                   UserId = user.Id,    
                   ExpiresDate = DateTime.UtcNow.AddDays(7),
                   Email = user.Email,
                   IsRevoked = false    

               };
                await unitOfWork.RefreshTokenRepository.Add(refreshTokenData);
                await unitOfWork.Commit();  
            }   
            return (AccessToken, RefreshToken); 

        }

        public async Task<(string, string)> RefreshTokenIssue(string refreshToken)
        {
            
            var isValidToken = await unitOfWork.RefreshTokenRepository.IsValidToken(refreshToken);
            if (isValidToken == null)
            {
                return (string.Empty, string.Empty);
            }
            isValidToken.IsRevoked = true;  
            var user = await unitOfWork.UserRepository.GetById(isValidToken.UserId);  

            var accessToken = jwtService.JwtAccessToken(user);
            var refreshNewToken = jwtService.JwtRefreshToken();
            var refreshTokenData = new RefreshToken
            {
                Token = refreshNewToken,
                UserId = user.Id,
                ExpiresDate = DateTime.UtcNow.AddDays(7),
                Email = user.Email,
                IsRevoked = false
            };
            await unitOfWork.RefreshTokenRepository.Add(refreshTokenData);
            await unitOfWork.Commit();
            return (accessToken, refreshNewToken);
        }

        public async Task<bool> RevokedToken(string refreshToken) {

            var RevokedToken = await unitOfWork.RefreshTokenRepository.IsValidToken(refreshToken);
            if (RevokedToken != null) { 

                    RevokedToken.IsRevoked = true;
                    await unitOfWork.Commit();
            }
            return true;

        }
    }
}
