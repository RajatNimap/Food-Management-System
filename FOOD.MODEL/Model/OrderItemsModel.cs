using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FOOD.MODEL.Model;

namespace FFOOD.MODEL.Model
{
    public class OrderItemsModel : BaseEntityModel
    {
      //  public int OrderDetailId { get; set; }
       // public int OrderId { get; set; }
      //  public OrdersModel Order { get; set; }  
        public int MenuId { get; set; }
      //  public MenuModel Menu { get; set; }
        public int QuantityOrdered { get; set; }  
        public decimal UnitPrice { get; set; }
    }
}
