using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FFOOD.MODEL.Model;

namespace FOOD.MODEL.Model
{
    public class MenuModel : BaseEntityModel
    {
        public int MenuId { get; set; }
        public string MenuName { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public bool IsAvailable { get; set; } 
        //public ICollection<RecipeModel> Recipes { get; set; } = new List<RecipeModel>();
        //public ICollection<OrderItemsModel> OrderItems { get; set; } = new List<OrderItemsModel>();
    }
}

