using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FOOD.DATA.Repository.InventoryRepository;
using FOOD.DATA.Repository.MenuRepository;
using FOOD.DATA.Repository.OderRepository;
using FOOD.DATA.Repository.RecipeRepository;
using FOOD.DATA.Repository.UserRepository;

namespace FOOD.DATA.Infrastructure
{
    public interface IUnitOfWork
    {
        public IUserRepository UserRepository { get; }
        public IInventoryRepository InventoryRepository { get; }  
        public IMenuRepository MenuRepository { get; }
        public IOrderRepository OrderRepository { get; }    
        public IRecipeRepository RecipeRepository { get; }
        public Task<int> Commit();
    }
}
