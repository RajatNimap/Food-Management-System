using FOOD.MODEL.Model;
using FOOD.SERVICES.UserServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BCrypt;
using Microsoft.AspNetCore.Authorization;

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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUser() {

            var UserDetail = await userServices.GetAllUser();
            return Ok(UserDetail);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetSingleUser(int id)
        {
            var User = await userServices.GetSingleUser(id);
            return Ok(User);    
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddUser(UserModel model)
        {
                await userServices.AddUser(model);  
                return Ok($"User Added Successfully");
                
        }
        [HttpPut("{Id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Updateuser(int Id, UserModel model)
        {
            await userServices.UpdateUser(Id, model);
            return Ok("User Updated Successfully");    
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await userServices.DeleteUser(id);
            return Ok("User Deleted");
        }
    }
}
