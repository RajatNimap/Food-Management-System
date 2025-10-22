using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOOD.MODEL.Model
{
    public class RecipeModel
    {
        public int RecipeId { get; set; }
        public int MenuId { get; set; }  
        public MenuModel Menu { get; set; }  
        public int ItemId { get; set; }
        public InventoryModel InventoryModel { get; set; }  
        public decimal QuantityRequired { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
