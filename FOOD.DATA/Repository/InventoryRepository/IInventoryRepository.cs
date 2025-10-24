using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FOOD.DATA.Entites;
using FOOD.DATA.Infrastructure;

namespace FOOD.DATA.Repository.InventoryRepository
{
    public interface IInventoryRepository:IRepository<Inventory>
    {
        Task<IEnumerable<Inventory>> GetLowStockItems();
        Task<IEnumerable<Inventory>> GetItemsBelowQuantity(decimal quantity);   
        Task<bool> IstheItemLowStock(int itemId);

    }
}
