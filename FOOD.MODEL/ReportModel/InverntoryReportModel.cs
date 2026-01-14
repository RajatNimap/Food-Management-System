using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOOD.MODEL.ReportModel
{
    public class InverntoryReportModel
    {
        public int Id { get; set; }
        public string ItemName { get; set; }
        public string unit { get; set; }
        public decimal QuantityAvailable { get; set; }
        public decimal ReorderLevel { get; set; }
        public decimal StockDeficit => ReorderLevel - QuantityAvailable;
        public string Status
        {
            get
            {

                if (QuantityAvailable <= 0) return "Out of Stock";
                else if (QuantityAvailable < ReorderLevel) return "Low Stock";
                else return "In Stock";
            }
        }

    }
}
