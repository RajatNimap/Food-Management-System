using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOOD.MODEL.ReportModel
{
    public class DailyReportModel
    {

        public DateOnly ReportDate { get; set; }    
        public int TotalOrders { get; set; }    
        public int TotalItemsSold { get; set; }
        public decimal TotalRevenue { get; set; }   
        public ICollection<ReportOrderItemModel>? Orders { get; set; } = new List<ReportOrderItemModel>();  

    }
}
