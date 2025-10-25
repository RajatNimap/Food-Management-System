using FOOD.SERVICES.ReportExport;
using FOOD.SERVICES.Reports;
using Microsoft.AspNetCore.Authorization;
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
        private readonly ILowStockExcel _lowStockExcelExporter;
        public ReportController(IDailyOrderReport reportService, ILowStockReports lowStockReports, IDailyReportExcel dailyReportExcel, ILowStockExcel lowStockExcelExporter)
        {
            _reportService = reportService;
            _lowStockReports = lowStockReports;
            _dailyReportExcel = dailyReportExcel;
            _lowStockExcelExporter = lowStockExcelExporter;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Report(DateOnly date)
        {
           var data= await _reportService.DailyReport(date);
           return Ok(data);
        }

        [HttpGet("LowStockReport")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> LowStockReport()
        {
            var data = await _lowStockReports.GetLowStockReport();
            return Ok(data);
        }

        [HttpGet("DailyReport")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DownloadReport(DateOnly reportDate)
        {

           
            var fileBytes = await _dailyReportExcel.DailyExport(reportDate);

         
            string fileName = $"DailyReport{Guid.NewGuid()}.xlsx";
            return File(fileBytes,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        fileName);
        }
        [HttpGet("LowStockExcel")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> LowStockExcel()
        {
            var fileBytes = await _lowStockExcelExporter.LowStockExport();
            string fileName = $"LowStockReport{Guid.NewGuid()}.xlsx";
            return File(fileBytes,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        fileName);
        }

    }
}
