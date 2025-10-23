using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FOOD.DATA.Entites;
using FOOD.DATA.Infrastructure;
using FOOD.MODEL.Model;

namespace FOOD.SERVICES.MenuServices
{
    public class MenuService : IMenuService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MenuService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Menu>> GetAllMenusAsync()
        {
            var menus = await _unitOfWork.MenuRepository.GetAll();
            return menus;
        }

        public async Task<Menu> GetMenuByIdAsync(int id)
        {
            var menu = await _unitOfWork.MenuRepository.GetById(id);
            return menu;
        }

        public async Task<bool> CreateMenuAsync(MenuModel menuModel)
        {
            try
            {
                menuModel.CreatedDate = DateTime.UtcNow;
                menuModel.CreatedBy = "System"; 

                var menuEntity = _mapper.Map<Menu>(menuModel);
                await _unitOfWork.MenuRepository.Add(menuEntity);

                var rowsAffected = await _unitOfWork.Commit();
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating menu", ex);
            }
        }

        public async Task<bool> UpdateMenuAsync(int id, MenuModel menuModel)
        {
            try
            {
                var existingMenu = await _unitOfWork.MenuRepository.GetById(id);
                if (existingMenu == null)
                    throw new KeyNotFoundException($"Menu with ID {id} not found");

                _mapper.Map(menuModel, existingMenu);

                existingMenu.ModifiedDate = DateTime.UtcNow;
                existingMenu.ModifiedBy = "System"; 

                var rowsAffected = await _unitOfWork.Commit();
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating menu with ID {id}", ex);
            }
        }

        public async Task<bool> DeleteMenuAsync(int id)
        {
            try
            {
                var menu = await _unitOfWork.MenuRepository.GetById(id);
                if (menu == null)
                    throw new KeyNotFoundException($"Menu with ID {id} not found");

                _unitOfWork.MenuRepository.Delete(menu);
                var rowsAffected = await _unitOfWork.Commit();
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting menu with ID {id}", ex);
            }
        }

    }
}