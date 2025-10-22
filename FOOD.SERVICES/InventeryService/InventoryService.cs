using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using FOOD.DATA.Entites;
using FOOD.DATA.Infrastructure;
using FOOD.MODEL.Model;

namespace FOOD.SERVICES.Inventery
{
    public class InventoryService : IInventoryService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper _mapper;

        public InventoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Inventory>> GetAllInventory()
        {
            return await unitOfWork.InventoryRepository.GetAll();
            
        }

        public async Task<Inventory> GetSinglInventory(int id)
        {
            return await unitOfWork.InventoryRepository.GetById(id);
        }

        public async Task<bool> AddInventory(InventoryModel model)
        {
            model.CreatedDate = DateTime.UtcNow;
            model.CreatedBy = "System"; 
            
            var inventoryEntity = _mapper.Map<Inventory>(model);
            await unitOfWork.InventoryRepository.Add(inventoryEntity);
            
            var rowsAffected = await unitOfWork.Commit();
            return rowsAffected > 0;
        }

        public async Task<bool> UpdateInventory(int id, InventoryModel model)
        {
            var existingInventory = await unitOfWork.InventoryRepository.GetById(id);
            if (existingInventory == null)
                throw new KeyNotFoundException("Inventory not found");

            _mapper.Map(model, existingInventory);
            
            existingInventory.ModifiedDate = DateTime.UtcNow;
            existingInventory.ModifiedBy = "System"; 
            
            var rowsAffected = await unitOfWork.Commit();
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteInventory(int id)
        {
            var inventory = await unitOfWork.InventoryRepository.GetById(id);
            unitOfWork.InventoryRepository.Delete(inventory);
           var rowaffected= await unitOfWork.Commit();
            return rowaffected > 0;

        }
    }
}