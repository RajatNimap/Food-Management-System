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
    [Authorize(Roles = "Admin")]
    public class UserController : ControllerBase
    {
        private readonly IUserServices userServices;
        public UserController(IUserServices userServices)
        {
            this.userServices = userServices;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUser(int pageNumber,int pageSize) {

            var UserDetail = await userServices.GetAllUserPagination(pageNumber, pageSize);
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
                return Ok($"User Added Successfully");
                
        }
        [HttpPut("{Id}")]
        public async Task<IActionResult> Updateuser(int Id, UserModel model)
        {
            await userServices.UpdateUser(Id, model);
            return Ok("User Updated Successfully");    
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await userServices.DeleteUser(id);
            return Ok("User Deleted");
        }
    }
}
