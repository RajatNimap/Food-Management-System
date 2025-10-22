using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FOOD.DATA.Entites;
using FOOD.MODEL.Model;

namespace FOOD.SERVICES.Inventery
{
    public interface IInventoryService
    {
        public Task<IEnumerable<Inventory>> GetAllInventory();
        public Task<Inventory> GetSinglInventory(int id);
        public Task<bool> AddInventory(InventoryModel model);
        public Task<bool> UpdateInventory(int id, InventoryModel user);
        public Task<bool> DeleteInventory(int id);
    }
}
