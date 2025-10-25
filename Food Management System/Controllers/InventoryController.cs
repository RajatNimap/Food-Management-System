using FOOD.DATA.Entites;
using FOOD.MODEL.Model;
using FOOD.SERVICES.Inventery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Food_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;

        public InventoryController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        [HttpGet]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> GetAllInventory()
        {
            var inventoryItems = await _inventoryService.GetAllInventory();
            return Ok(inventoryItems);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetSingleInventory(int id)
        {
            var inventory = await _inventoryService.GetSinglInventory(id);
            return Ok(inventory);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddInventory(InventoryModel model)
        {
            var result = await _inventoryService.AddInventory(model);
            if (result)
                return Ok();
            else
                return BadRequest("Failed to add inventory item");
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateInventory(int id, InventoryModel model)
        {
            var result = await _inventoryService.UpdateInventory(id, model);
            if (result)
                return Ok();
            else
                return BadRequest("Failed to update inventory item");
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteInventory(int id)
        {
            var result = await _inventoryService.DeleteInventory(id);
            if (result)
                return Ok();
            else
                return BadRequest("Failed to delete inventory item");
        }
    }
}