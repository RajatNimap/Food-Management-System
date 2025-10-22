using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FOOD.DATA.Entites;
using FOOD.DATA.Infrastructure;

namespace FOOD.DATA.Repository.MenuRepository
{
    public class MenuRepository:Repository<Menu>,IMenuRepository
    {
        private readonly DataContext dbcontext;

        public MenuRepository(DataContext dbcontext ):base(dbcontext)
        {
            this.dbcontext = dbcontext;
        }
    }
}
