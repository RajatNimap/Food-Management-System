using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOOD.DATA.Entites
{
    public class OrderItems : BaseEntity  
    {
       // public int OrderDetailId { get; set; }
        public int OrderId { get; set; }
        public Orders Order { get; set; }  
        public int MenuId { get; set; }
        public Menu Menu { get; set; }
        public int QuantityOrdered { get; set; }  
        public decimal UnitPrice { get; set; }
    }
}
