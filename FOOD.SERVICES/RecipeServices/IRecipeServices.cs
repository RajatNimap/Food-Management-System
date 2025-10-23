using System.Collections.Generic;
using System.Threading.Tasks;
using FOOD.DATA.Entites;
using FOOD.MODEL.Model;

namespace FOOD.SERVICES.RecipeServices
{
    public interface IRecipeService
    {
        Task<IEnumerable<Recipe>> GetAllRecipesAsync();
        Task<Recipe> GetRecipeByIdAsync(int id);
        Task<bool> CreateRecipeAsync(RecipeModel recipeModel);
        Task<bool> UpdateRecipeAsync(int id, RecipeModel recipeModel);
        Task<bool> DeleteRecipeAsync(int id);
    }
}