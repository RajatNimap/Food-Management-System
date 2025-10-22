using FOOD.MODEL.Model;
using FOOD.SERVICES.UserServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BCrypt;

namespace Food_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserServices userServices;
        public UserController(IUserServices userServices)
        {
            this.userServices = userServices;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUser() {

            var UserDetail = await userServices.GetAllUser();
            return Ok(UserDetail);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSingleUser(int id)
        {
            var User = await userServices.GetSingleUser(id);
            return Ok(User);    
        }
        [HttpPost]
        public async Task<IActionResult> AddUser(UserModel model)
        {
                await userServices.AddUser(model);  
                return Ok();
                
        }
        [HttpPut("{Id}")]
        public async Task<IActionResult> Updateuser(int Id, UserModel model)
        {
            await userServices.UpdateUser(Id, model);
            return Ok();    
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await userServices.DeleteUser(id);
            return Ok();
        }
    }
}
