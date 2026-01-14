using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOOD.DATA.Entites
{
    public class Inventory:BaseEntity
    {
        public string ItemName { get; set; }
        public string Unit { get; set; }
        public decimal QuantityAvailable { get; set; }
        public decimal ReorderLevel { get; set; }  
        public ICollection<Recipe> Recipes { get; set; } = new List<Recipe>();

    }
}
