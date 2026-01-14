using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FOOD.MODEL.ReportModel;

namespace FOOD.SERVICES.Reports
{
    public interface IDailyOrderReport
    {
        public Task<DailyReportModel> DailyReport(DateOnly reportDate); 
      
    }
}
