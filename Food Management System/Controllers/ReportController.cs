using FOOD.SERVICES.ReportExport;
using FOOD.SERVICES.Reports;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Food_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IDailyOrderReport _reportService;
        private readonly ILowStockReports _lowStockReports;
        private readonly IDailyReportExcel _dailyReportExcel;
        public ReportController(IDailyOrderReport reportService, ILowStockReports lowStockReports, IDailyReportExcel dailyReportExcel)
        {
            _reportService = reportService;
            _lowStockReports = lowStockReports;
            _dailyReportExcel = dailyReportExcel;
        }

        [HttpGet]
        public async Task<IActionResult> Report(DateOnly date)
        {
           var data= await _reportService.DailyReport(date);
           return Ok(data);
        }

        [HttpGet("LowStockReport")]
        public async Task<IActionResult> LowStockReport()
        {
            var data = await _lowStockReports.GetLowStockReport();
            return Ok(data);
        }

        [HttpGet("report")]
        public async Task<IActionResult> DownloadReport(DateOnly reportDate)
        {

            // Generate Excel file
            var fileBytes = await _dailyReportExcel.DailyExport(reportDate);

            // Return as downloadable file
            string fileName = $"DailyReport{Guid.NewGuid()}.xlsx";
            return File(fileBytes,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        fileName);
        }
    }
}
