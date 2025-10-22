using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FOOD.DATA.Entites;

namespace FOOD.MODEL.Model
{
    public class OrderCreatedResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public int? OrderId { get; set; }
        public Orders order { get; set; }
        public List<ShortageItem> ShortageItems { get; set; } = new List<ShortageItem>();
    }
}
