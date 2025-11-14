using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using FOOD.DATA.Entites;
using FOOD.DATA.Infrastructure;
using FOOD.MODEL.Model;
using FOOD.MODEL.Pagination;
using FOOD.Utility;
using FOOD.Utility.Extension;

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
        public async Task<InventoryModel> GetSinglInventory(int id)
        {
            var data = await unitOfWork.InventoryRepository.GetById(id);
            return _mapper.Map<InventoryModel>(data);
            
        }
        public async Task<bool> AddInventory(InventoryModel model)
        {
            model.CreatedDate = DateTime.UtcNow;
            model.CreatedBy = null; 
            
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
            existingInventory.ModifiedBy = null; 
            
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

        public async Task<PaginationModel<InventoryModel>> GetAllQuerable(int pnum, int psize)
        {
            try
            {
                var data= await unitOfWork.InventoryRepository.GetAllQuerable().PagedResult(pnum, psize, x => x.Id);
                var model=_mapper.Map<List<InventoryModel>>(data.Items);
                return new PaginationModel<InventoryModel>
                {
                    Items = model,
                    TotalRecord = data.TotalRecord,
                    PageNumer = data.PageNumer,
                    PageSize = data.PageSize
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while getting  order", ex);

            }
        }
    }
}