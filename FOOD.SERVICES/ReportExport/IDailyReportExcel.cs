using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOOD.SERVICES.ReportExport
{
    public interface IDailyReportExcel
    {
        public Task<byte[]> DailyExport(DateOnly date);   
    }
}
