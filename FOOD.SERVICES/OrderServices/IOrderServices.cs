using System.Collections.Generic;
using System.Threading.Tasks;
using FOOD.DATA.Entites;
using FOOD.MODEL.Model;

namespace FOOD.SERVICES.OrderServices
{
    public interface IOrderService
    {
        Task<IEnumerable<OrdersModel>> GetAllOrdersAsync();
        Task<OrdersModel> GetOrderByIdAsync(int id);
        Task<bool> UpdateOrder(OrdersModel orderModel);
        Task<bool> UpdateOrderAsync(int id, OrdersModel orderModel);
        Task<bool> DeleteOrderAsync(int id);
        Task<OrderCreatedResult> PlacingOrder(OrdersModel order);   
    }
}