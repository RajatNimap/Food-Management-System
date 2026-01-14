using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOOD.MODEL.ReportModel
{
    public class ReportSummaryModel
    {
        public string? MenuName { get; set; }    
        public int Quantity { get; set; } = 0;
        public Decimal unitprice { get; set; } = 0; 

    }
}
