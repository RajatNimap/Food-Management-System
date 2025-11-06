using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using FOOD.DATA.Entites;
using FOOD.DATA.Infrastructure;
using FOOD.MODEL.HelperModel;
using FOOD.MODEL.Model;
using FOOD.MODEL.Pagination;
using FOOD.SERVICES.HttpContext;
using FOOD.SERVICES.Pagination;

namespace FOOD.SERVICES.RecipeServices
{
    public class RecipeService : IRecipeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserContext _userContext;

        public RecipeService(IUnitOfWork unitOfWork, IMapper mapper, IUserContext userContext)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userContext = userContext;
        }

        public async Task<IEnumerable<Recipe>> GetAllRecipesAsync()
        {
            try
            {
                var recipes = await _unitOfWork.RecipeRepository.GetAll();
                return recipes;
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while retrieving all recipes", ex);
            }
        }

        public async Task<Recipe> GetRecipeByIdAsync(int id)
        {
            try
            {
                var recipe = await _unitOfWork.RecipeRepository.GetById(id);
                if (recipe == null)
                    throw new KeyNotFoundException($"Recipe with ID {id} not found");

                return recipe;
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error occurred while retrieving recipe with ID {id}", ex);
            }
        }

        public async Task<bool> CreateRecipeAsync(RecipeModel recipeModel)
        {
            try
            {
                recipeModel.CreatedDate = DateTime.UtcNow;
                var recipeEntity = _mapper.Map<Recipe>(recipeModel);
                await _unitOfWork.RecipeRepository.Add(recipeEntity);

                var rowsAffected = await _unitOfWork.Commit();
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while creating recipe", ex);
            }
        }

        public async Task<bool> UpdateRecipeAsync(int id, RecipeModel recipeModel)
        {
            try
            {
                var existingRecipe = await _unitOfWork.RecipeRepository.GetById(id);
                if (existingRecipe == null)
                    throw new KeyNotFoundException($"Recipe with ID {id} not found");

                _mapper.Map(recipeModel, existingRecipe);

                var rowsAffected = await _unitOfWork.Commit();
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error occurred while updating recipe with ID {id}", ex);
            }
        }

        public async Task<bool> DeleteRecipeAsync(int id)
        {
            try
            {
                var recipe = await _unitOfWork.RecipeRepository.GetById(id);
                if (recipe == null)
                    throw new KeyNotFoundException($"Recipe with ID {id} not found");

                _unitOfWork.RecipeRepository.Delete(recipe);
                var rowsAffected = await _unitOfWork.Commit();
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error occurred while deleting recipe with ID {id}", ex);
            }
        }

        public async Task<PaginationModel<Recipe>> GetPaginationAsync(int pnum,int psize)
        {
            try
            {
                return await _unitOfWork.RecipeRepository.GetAllQuerable()
                    .PagedResult(pnum, psize, r => r.RecipeId);   

            }
            catch(Exception ex)
            {
                throw new Exception("error occur while fetching data", ex);
            }
        }
    }
}