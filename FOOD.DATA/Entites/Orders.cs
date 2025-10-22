using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOOD.DATA.Entites
{
    public class Orders : BaseEntity
    {
        public int UserId { get; set; }  
        public User User { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string Address { get; set; }
        public decimal TotalAmount { get; set; }
        public ICollection<OrderItems> OrderItems { get; set; } = new List<OrderItems>();
    }
}
