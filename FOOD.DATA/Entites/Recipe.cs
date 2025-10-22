using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOOD.DATA.Entites
{
    public class Recipe
    {
        public int RecipeId { get; set; }
        public int MenuId { get; set; }  
        public Menu Menu { get; set; }   
        public int ItemId { get; set; }
        public Inventory Inventory { get; set; } 
        public decimal QuantityRequired { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
