using System.Collections.Generic;
using System.Threading.Tasks;
using FOOD.DATA.Entites;
using FOOD.MODEL.Model;
using FOOD.MODEL.Pagination;

namespace FOOD.SERVICES.OrderServices
{
    public interface IOrderService
    {
        Task<IEnumerable<OrdersModel>> GetAllOrdersAsync();
        Task<OrdersModel> GetOrderByIdAsync(int id);
        Task<bool> UpdateOrderAsync(int id, OrdersModel orderModel);
        Task<bool> DeleteOrderAsync(int id);
        Task<OrderCreatedResult> PlacingOrder(OrdersModel order);   
        Task<PaginationModel<Orders>> GetAllQuerable(int pnum,int psize);
    }
}