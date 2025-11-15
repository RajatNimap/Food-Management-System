using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FOOD.DATA.Entites;
using FOOD.DATA.Infrastructure;
using FOOD.MODEL.HelperModel;
using FOOD.MODEL.Model;
using FOOD.MODEL.Pagination;
using FOOD.Utility.Extension;
using Microsoft.Extensions.Caching.Memory;

namespace FOOD.SERVICES.MenuServices
{
    public class MenuService : IMenuService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserContext _userContext;
        private readonly IMemoryCache _cache;

        public MenuService(IUnitOfWork unitOfWork, IMapper mapper,IUserContext userContext, IMemoryCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userContext = userContext;
            _cache = cache;
        }

        public async Task<IEnumerable<Menu>> GetAllMenusAsync()
        {
            var menus = await _unitOfWork.MenuRepository.GetAll();
            return menus;
        }

        public async Task<MenuModel> GetMenuByIdAsync(int id)
        {
            var Menukey = $"Menu{id}";
            if(_cache.TryGetValue(Menukey,out MenuModel? cachedata))
            {
                if(cachedata != null)
                {
                    return cachedata;
                }
            }
            var menu = await _unitOfWork.MenuRepository.GetById(id);
            var model=_mapper.Map<MenuModel>(menu);

            _cache.Set(Menukey, model, TimeSpan.FromMinutes(5));
            return model;
        }

        public async Task<bool> CreateMenuAsync(MenuModel menuModel)
        {
            try
            {
                menuModel.CreatedDate = DateTime.UtcNow;
                menuModel.CreatedBy = _userContext.GetCurrentUserId(); 

                var menuEntity = _mapper.Map<Menu>(menuModel);
                await _unitOfWork.MenuRepository.Add(menuEntity);

                var rowsAffected = await _unitOfWork.Commit();
                if(rowsAffected > 0)
                {
                   
                }
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
                existingMenu.ModifiedBy = _userContext.GetCurrentUserId(); 

                var rowsAffected = await _unitOfWork.Commit();
                if(rowsAffected > 0)
                {
                    _cache.Remove($"Menu{id}"); 
                }
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
                if (rowsAffected > 0)
                {
                    _cache.Remove($"Menu{id}");
                }
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting menu with ID {id}", ex);
            }
        }

        public async Task<PaginationModel<MenuModel>> GetAllQuerable(int pnum, int psize)
        {
            try
            {
                var MenuCacheKey = $"MenuList_Page{pnum}_Size{psize}";
                if(_cache.TryGetValue(MenuCacheKey,out PaginationModel<MenuModel>? cachedData))
                {
                    if (cachedData != null)
                    {
                        Console.WriteLine("Fetching data from cache.");
                        return cachedData;
                    }
                }
                
                var data = await _unitOfWork.MenuRepository.GetAllQuerable().PagedResult(pnum, psize, x => x.Id);
                var model = _mapper.Map<List<MenuModel>>(data.Items);
                var result = new PaginationModel<MenuModel>
                {
                        Items = model,
                        PageNumer =data.PageNumer,
                        PageSize =data.PageSize,
                        TotalRecord=data.TotalRecord
                };
                _cache.Set(MenuCacheKey,result,TimeSpan.FromMinutes(5));
                Console.WriteLine("Fetching from data.");

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while getting  order", ex);

            }
        }
    }
}