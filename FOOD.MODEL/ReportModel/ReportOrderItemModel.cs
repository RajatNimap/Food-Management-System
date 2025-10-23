using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace FOOD.MODEL.ReportModel
{
    public class ReportOrderItemModel
    {
        public int OrderId { get; set; }    
        public string? CustomerName { get; set; }    
        public decimal TotalAmount { get; set; }
        public ICollection<ReportSummaryModel>? OrderItems { get; set; } =new List<ReportSummaryModel>();
    }
}
