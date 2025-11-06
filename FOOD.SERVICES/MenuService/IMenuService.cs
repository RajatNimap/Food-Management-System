using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FOOD.DATA.Entites;
using FOOD.MODEL.Model;
using FOOD.MODEL.Pagination;

namespace FOOD.SERVICES.MenuServices
{
    public interface IMenuService
    {
        Task<IEnumerable<Menu>> GetAllMenusAsync();
        Task<Menu> GetMenuByIdAsync(int id);
        Task<bool> CreateMenuAsync(MenuModel menuModel);
        Task<bool> UpdateMenuAsync(int id, MenuModel menuModel);
        Task<bool> DeleteMenuAsync(int id);
        Task<PaginationModel<Menu>> GetAllQuerable(int pnum, int psize);

    }
}