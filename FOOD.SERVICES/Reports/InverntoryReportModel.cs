using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOOD.SERVICES.Reports
{
    public class InverntoryReportModel
    {
        public int InventoryId { get; set; }
        public string ItemName { get; set; }
        public string unit{ get; set; }
        public decimal QuantityAvalible { get; set; }
        public decimal RecordLevel { get; set; }
        public decimal StockDeficit => RecordLevel - QuantityAvalible;
        public string Status { 
            get
            {

                if(QuantityAvalible <= 0) return "Out of Stock";
                else if (QuantityAvalible < RecordLevel) return "Low Stock";
                else return "In Stock";
        }   }

    }
}
