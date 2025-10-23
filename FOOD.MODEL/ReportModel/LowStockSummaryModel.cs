using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOOD.MODEL.ReportModel
{
    public class LowStockSummaryModel
    {
        public int TotalItmes { get; set; }
        public int CriticalCount { get; set; }
        public int WarningCount { get; set; }
    }
}
