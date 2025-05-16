using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Unit_Trust_Backend.Staff.Interfaces;

namespace Unit_Trust_Backend.Staff.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicantListController : ControllerBase
    {
        private readonly IApplicantListService _applicantListService;

        public ApplicantListController(IApplicantListService applicantListService)
        {
            _applicantListService = applicantListService;
        }

        [HttpGet("ApplicantList")]
        public async Task<IActionResult> GetApplicantList()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var results = await _applicantListService.GetApplicantListAsync();
            if (results == null || !results.Any())
            {
                return NotFound(new { message = "Not Found" });
            }
            return Ok(results);
        }
    }
}
