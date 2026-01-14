using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FOOD.DATA.Entites;
using FOOD.DATA.Infrastructure;

namespace FOOD.DATA.Repository.RecipeRepository
{
    public interface IRecipeRepository : IRepository<Recipe>
    {
    }
}
