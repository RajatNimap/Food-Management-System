using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FOOD.DATA.Entites;
using FOOD.MODEL.Model;
using FOOD.MODEL.Pagination;

namespace FOOD.SERVICES.Inventery
{
    public interface IInventoryService
    {
         Task<IEnumerable<Inventory>> GetAllInventory();
         Task<Inventory> GetSinglInventory(int id);
         Task<bool> AddInventory(InventoryModel model);
         Task<bool> UpdateInventory(int id, InventoryModel user);
         Task<bool> DeleteInventory(int id);
         Task<PaginationModel<Inventory>> GetAllQuerable(int pnum, int psize);

    }
}
