using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOOD.DATA.Entites
{
    public class Menu : BaseEntity  
    {
        public int MenuId { get; set; }
        public string MenuName { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public bool IsAvailable { get; set; } 
        public ICollection<Recipe> Recipes { get; set; } = new List<Recipe>();
        public ICollection<OrderItems> OrderItems { get; set; } = new List<OrderItems>();
    }
}

