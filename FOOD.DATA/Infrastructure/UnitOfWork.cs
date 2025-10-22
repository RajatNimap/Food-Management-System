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
using Microsoft.EntityFrameworkCore;

namespace FOOD.DATA.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
       //public IUserRepository UserRepository { get; }
        private DataContext dbcontext; 
        private IUserRepository userRepository;
        private IInventoryRepository inventoryRepository;
        private IMenuRepository menuRepository;
        private IOrderRepository orderRepository;
        private IRecipeRepository recipeRepository;
       public UnitOfWork(DataContext _dbcontext)
       {
           // UserRepository = userepo;
            dbcontext=_dbcontext;
       }
                
        public IUserRepository UserRepository { 

            get{ 
                
                return userRepository = userRepository ?? new UserRepository(dbcontext);
            } 
        }    
        public IInventoryRepository InventoryRepository
        {
            get
            {
                    return inventoryRepository ?? new InventoryRepository(dbcontext);   
            }
        }

        public IMenuRepository MenuRepository
        {
            get
            {
                 return menuRepository ?? new MenuRepository(dbcontext);        
            }
        }

        public IOrderRepository OrderRepository
        {

            get
            {
                return orderRepository ?? new OrderRepository(dbcontext);   
            }
        }

        public IRecipeRepository RecipeRepository
        {
            get {

                return recipeRepository ?? new RecipeRepository(dbcontext);
            }
        }
        public async Task<int> Commit()
        {
            return await dbcontext.SaveChangesAsync();
        }
    }
}
