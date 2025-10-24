using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FOOD.DATA.Entites;
using FOOD.DATA.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace FOOD.DATA.Repository.InventoryRepository
{
    public class InventoryRepository:Repository<Inventory>, IInventoryRepository    
    {
        private readonly DataContext dbcontext;
        public InventoryRepository(DataContext dbcontext):base(dbcontext) 
        {
            this.dbcontext = dbcontext;
        }

        public async Task<IEnumerable<Inventory>> GetItemsBelowQuantity(decimal quantity)
        {
            return await dbcontext.inventories.Where(x => x.QuantityAvailable < quantity).ToListAsync();  
        }

        public async Task<IEnumerable<Inventory>> GetLowStockItems()
        {
           return await dbcontext.inventories.Where(x => x.QuantityAvailable < x.ReorderLevel)
          .OrderBy(x=>x.QuantityAvailable).ToListAsync();
        }

        public async Task<bool> IstheItemLowStock(int itemId)
        {
            var data= await dbcontext.inventories.FirstOrDefaultAsync(x => x.Id == itemId);
            return data?.QuantityAvailable < data?.ReorderLevel ? true : false;
        }
    }
}
