using FOOD.MODEL.Model;
using FOOD.SERVICES.AuthenticationServices;
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
        public async Task<IActionResult> Login(LoginModel login)
        {
            var token = await auth.IsAuthenticated(login);
            if (string.IsNullOrWhiteSpace(token)) {

                return BadRequest("Invalid Authentication");
                            
            }
            return Ok(token);
        }
    }
}
