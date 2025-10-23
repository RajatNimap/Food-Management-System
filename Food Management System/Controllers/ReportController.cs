using FOOD.SERVICES.Reports;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Food_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IDailyOrderReport _reportService;
        public ReportController(IDailyOrderReport reportService)
        {
            _reportService = reportService;   
        }

        [HttpGet]
        public async Task<IActionResult> Report(DateOnly date)
        {
           var data= await _reportService.DailyReport(date);
           return Ok(data);
        }

    }
}
