using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FFOOD.MODEL.Model;

namespace FOOD.MODEL.Model
{
    public class OrdersModel : BaseEntityModel
    {
        public int UserId { get; set; }  
        public UserModel Usermodel { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string Address { get; set; }
        public decimal TotalAmount { get; set; }
        public ICollection<OrderItemsModel> OrderItems { get; set; } = new List<OrderItemsModel>();
    }
}
