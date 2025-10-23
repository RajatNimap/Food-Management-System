using FOOD.SERVICES.OrderServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FOOD.DATA.Entites;
using FOOD.MODEL.Model;
namespace Food_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService _orderService)
        {
            this._orderService = _orderService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return Ok(orders);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            return Ok(order);
        }
        [HttpPost]
        public async Task<IActionResult> CreateOrder(OrdersModel model)
        {
            var result = await _orderService.UpdateOrder(model);
            if (result)
                return Ok();
            else
                return BadRequest("Failed to create order");
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, OrdersModel model)
        {
            var result = await _orderService.UpdateOrder(model);
            if (result)
                return Ok();
            else
                return BadRequest("Failed to update order");
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var result = await _orderService.DeleteOrderAsync(id);
            if (result)
                return Ok();
            else
                return BadRequest("Failed to delete order");
        }

        [HttpPost("PlacingOrder")]
        public async Task<IActionResult> PlacingOrder(OrdersModel model)
        {
            var result = await _orderService.PlacingOrder(model);
            return Ok(result);
        }
    }
}
