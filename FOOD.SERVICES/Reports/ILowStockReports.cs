using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FOOD.MODEL.ReportModel;

namespace FOOD.SERVICES.Reports
{
    public interface ILowStockReports
    {

        public Task<LowStockReportModel> GetLowStockReport();
        public Task<IEnumerable<InverntoryReportModel>> GetLowStockItmes(); 
    }
}
