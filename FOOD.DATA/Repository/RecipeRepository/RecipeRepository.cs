using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FOOD.DATA.Entites;
using FOOD.DATA.Infrastructure;

namespace FOOD.DATA.Repository.RecipeRepository
{
    public class RecipeRepository:Repository<Recipe>,IRecipeRepository
    {
        private readonly DataContext dbcontext;
        public RecipeRepository(DataContext dbcontext):base(dbcontext)
        {
            this.dbcontext = dbcontext;
        }

    }
}
