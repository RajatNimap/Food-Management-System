using FOOD.MODEL.Model;
using FOOD.SERVICES.RecipeServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Food_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipeController : ControllerBase
    {
        private readonly IRecipeService _recipeService;

        public RecipeController(IRecipeService recipeService)
        {
            _recipeService = recipeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRecipes()
        {
            var recipes = await _recipeService.GetAllRecipesAsync();
            return Ok(recipes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRecipeById(int id)
        {
            var recipe = await _recipeService.GetRecipeByIdAsync(id);
            return Ok(recipe);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRecipe(RecipeModel model)
        {
            var result = await _recipeService.CreateRecipeAsync(model);
            if (result)
                return Ok();
            else
                return BadRequest("Failed to create recipe");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRecipe(int id, RecipeModel model)
        {
            var result = await _recipeService.UpdateRecipeAsync(id, model);
            if (result)
                return Ok();
            else
                return BadRequest("Failed to update recipe");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecipe(int id)
        {
            var result = await _recipeService.DeleteRecipeAsync(id);
            if (result)
                return Ok();
            else
                return BadRequest("Failed to delete recipe");
        }
    }
}