using FOOD.MODEL.Model;
using FOOD.SERVICES.MenuServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Food_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly IMenuService _menuService;

        public MenuController(IMenuService menuService)
        {
            _menuService = menuService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Cashier")]
        public async Task<IActionResult> GetAllMenus()
        {
            var menus = await _menuService.GetAllMenusAsync();
            return Ok(menus);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Cashier")]
        public async Task<IActionResult> GetMenuById(int id)
        {
            var menu = await _menuService.GetMenuByIdAsync(id);
            return Ok(menu);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateMenu(MenuModel model)
        {
            var result = await _menuService.CreateMenuAsync(model);
            if (result)
                return Ok();
            else
                return BadRequest("Failed to create menu");
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateMenu(int id, MenuModel model)
        {
            var result = await _menuService.UpdateMenuAsync(id, model);
            if (result)
                return Ok();
            else
                return BadRequest("Failed to update menu");
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteMenu(int id)
        {
            var result = await _menuService.DeleteMenuAsync(id);
            if (result)
                return Ok();
            else
                return BadRequest("Failed to delete menu");
        }

       
    }
}