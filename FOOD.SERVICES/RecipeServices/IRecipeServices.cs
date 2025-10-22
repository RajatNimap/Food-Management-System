using System.Collections.Generic;
using System.Threading.Tasks;
using FOOD.MODEL.Model;

namespace FOOD.SERVICES.RecipeServices
{
    public interface IRecipeService
    {
        Task<IEnumerable<RecipeModel>> GetAllRecipesAsync();
        Task<RecipeModel> GetRecipeByIdAsync(int id);
        Task<bool> CreateRecipeAsync(RecipeModel recipeModel);
        Task<bool> UpdateRecipeAsync(int id, RecipeModel recipeModel);
        Task<bool> DeleteRecipeAsync(int id);
    }
}