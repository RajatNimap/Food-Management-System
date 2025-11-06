using FOOD.MODEL.Model;
using FOOD.SERVICES.AuthenticationServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Food_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuth auth;
        public AuthController(IAuth _auth)
        {
            auth = _auth;   
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginModel login)
        {
            var token = await auth.IsAuthenticated(login);
            if (string.IsNullOrWhiteSpace(token.Item1) || string.IsNullOrWhiteSpace(token.Item2)) {

                return BadRequest("Invalid Authentication");
                            
            }
            return Ok(new
            {
                AccessToken = token.Item1,
                RefreshToken = token.Item2  
            });
        }
        [HttpPost("RefreshToken")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken(string token)
        {

            var GeneratingToken = await auth.RefreshTokenIssue(token);
            if (string.IsNullOrWhiteSpace(GeneratingToken.Item1) || string.IsNullOrWhiteSpace(GeneratingToken.Item2))
            {

                return BadRequest("Invalid Authentication");

            }
            return Ok(new
            {
                AccessToken = GeneratingToken.Item1,
                RefreshToken = GeneratingToken.Item2
            });
        }
    }
}
