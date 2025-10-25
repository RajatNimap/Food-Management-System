using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FOOD.DATA.Entites;
using FOOD.DATA.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace FOOD.DATA.Repository.OderRepository
{
    public class OrderRepository : Repository<Orders>, IOrderRepository
    {
        private readonly DataContext dbcontext;
        public OrderRepository(DataContext dbcontext): base(dbcontext)
        {
            this.dbcontext = dbcontext;
        }

        public async Task<Menu?> GetMenuWithRecipesAsync(int menuId)
        {
            return await dbcontext.menus
                .Include(m => m.Recipes).ThenInclude(r => r.Inventory)  
                .FirstOrDefaultAsync(m => m.Id == menuId);  
        }

        public async Task<List<Orders>> GetMenuWithRecipesDateTime(DateOnly date)
        {
            var start = date.ToDateTime(TimeOnly.MinValue);
            var nextDay = start.AddDays(1);

            var utcStart = TimeZoneInfo.ConvertTimeToUtc(start);
            var utcNextDay = TimeZoneInfo.ConvertTimeToUtc(nextDay);
            return await dbcontext.orders.Include(x => x.OrderItems).ThenInclude(y => y.Menu)
                .Where(o => o.CreatedDate >= utcStart && o.CreatedDate < utcNextDay).ToListAsync();
        }

       
    }
}
