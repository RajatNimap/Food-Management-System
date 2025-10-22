using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FOOD.MODEL.Model;

namespace FOOD.SERVICES.MenuServices
{
    public interface IMenuService
    {
        Task<IEnumerable<MenuModel>> GetAllMenusAsync();
        Task<MenuModel> GetMenuByIdAsync(int id);
        Task<bool> CreateMenuAsync(MenuModel menuModel);
        Task<bool> UpdateMenuAsync(int id, MenuModel menuModel);
        Task<bool> DeleteMenuAsync(int id);
    }
}