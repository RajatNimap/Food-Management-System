using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FOOD.DATA.Entites;
using FOOD.DATA.Infrastructure;

namespace FOOD.DATA.Repository.OderRepository
{
    public interface IOrderRepository : IRepository<Orders>
    {
      //  public Task<OrderCreatedResult> CreateOrders(Orders model);

        public Task<Menu?> GetMenuWithRecipesAsync(int menuId);  
        
        public Task UpdateInventory(Inventory inventory);  
    }

}
