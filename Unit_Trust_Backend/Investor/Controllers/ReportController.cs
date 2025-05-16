using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Unit_Trust_Backend.Investor.Interfaces;

namespace Unit_Trust_Backend.Investor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("campaign-funding")]
        public async Task<IActionResult> GetCampaignFundingReport()
        {
            var data = await _reportService.GetCampaignFundingReportAsync();
            return Ok(data);
        }

        [HttpGet("payment-status-summary")]
        public async Task<IActionResult> GetPaymentStatusSummary()
        {
            var data = await _reportService.GetPaymentStatusSummaryAsync();
            return Ok(data);
        }
    }
}
