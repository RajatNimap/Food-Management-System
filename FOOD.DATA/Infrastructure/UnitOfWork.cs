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
        private readonly DataContext _dbContext;
        public IUserRepository UserRepository { get; }
        public IInventoryRepository InventoryRepository { get; }
        public IMenuRepository MenuRepository { get; }
        public IOrderRepository OrderRepository { get; }
        public IRecipeRepository RecipeRepository { get; }

        public UnitOfWork(
            DataContext dbContext,
            IUserRepository userRepository,
            IInventoryRepository inventoryRepository,
            IMenuRepository menuRepository,
            IOrderRepository orderRepository,
            IRecipeRepository recipeRepository)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            UserRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            InventoryRepository = inventoryRepository ?? throw new ArgumentNullException(nameof(inventoryRepository));
            MenuRepository = menuRepository ?? throw new ArgumentNullException(nameof(menuRepository));
            OrderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            RecipeRepository = recipeRepository ?? throw new ArgumentNullException(nameof(recipeRepository));
        }

        public async Task<int> Commit()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _dbContext?.Dispose();
        }
    }
}