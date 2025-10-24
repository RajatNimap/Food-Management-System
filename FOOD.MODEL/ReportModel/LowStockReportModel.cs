using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOOD.MODEL.ReportModel
{
    public class LowStockReportModel
    {
        public DateTime GeneratedAt { get; set; }
        public int TotalLowStockItems { get; set; }
        public int CiticalItems { get; set; }
        public LowStockSummaryModel? lowStockSummaryModel { get; set; }
        public ICollection<InverntoryReportModel>? LowStockItems { get; set; }

    }
}
