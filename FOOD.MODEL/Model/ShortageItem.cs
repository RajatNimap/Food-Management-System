using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOOD.MODEL.Model
{
    public class ShortageItem
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public decimal RequiredQuantity { get; set; }
        public decimal AvailableQuantity { get; set; }
        public decimal ShortageQuantity { get; set; }
        public int MenuId { get; set; }
        public string MenuName { get; set; }
    }
}
