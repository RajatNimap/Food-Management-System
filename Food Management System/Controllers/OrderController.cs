using FOOD.SERVICES.OrderServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FOOD.DATA.Entites;
using FOOD.MODEL.Model;
using Microsoft.AspNetCore.Authorization;
namespace Food_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        
        [HttpGet]
        [Authorize(Roles = "Cashier,Admin")]
        public async Task<IActionResult> GetAllOrders(int pnum, int psize)
        {
            var orders = await _orderService.GetAllQuerable(pnum,psize);
            return Ok(orders);
        }
        [HttpGet("{id}")]
        [Authorize(Roles = "Cashier,Admin")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            return Ok(order);
        }
       
        [HttpPut("{id}")]
        [Authorize(Roles = "Cashier,Admin")]
        public async Task<IActionResult> UpdateOrder(int id, OrdersModel model)
        {
            var result = await _orderService.UpdateOrderAsync(id,model);
            if (result)
                return Ok();
            else
                return BadRequest("Failed to update order");
        }
        [Authorize(Roles = "Cashier")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var result = await _orderService.DeleteOrderAsync(id);
            if (result)
                return Ok();
            else
                return BadRequest("Failed to delete order");
        }
        [Authorize(Roles = "Cashier,Admin")]
        [HttpPost("PlacingOrder")]
        public async Task<IActionResult> PlacingOrder(OrdersModel model)
        {
            var result = await _orderService.PlacingOrder(model);
            return Ok(result);
        }
    }
}
