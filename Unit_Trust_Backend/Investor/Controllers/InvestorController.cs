using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Unit_Trust_Backend.DTOs;
using Unit_Trust_Backend.Investor.Interfaces;

namespace Unit_Trust_Backend.Investor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvestorController : ControllerBase
    {
        private readonly IInvestorService _investorService;

        public InvestorController(IInvestorService investorService)
        {
            _investorService = investorService;
        }

        [HttpPost("investor")]
        public async Task<IActionResult> createAccount(InvestorDTO investorDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _investorService.CreateInvestorAsync(investorDTO);
            if (!result)
            {
                return Conflict(new { message = "not Sucessfull" });
            }
            else
            {
                return Ok(new { message = "successful" });
            }
        }

        [HttpGet("getInvestments")]
        public async Task<IActionResult> GetInvestments()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var investments = await _investorService.GetInvestmentsAsync();
            if (investments == null || !investments.Any())
            {
                return NotFound(new { message = "No investments found for the given userId." });
            }

            return Ok(investments);
        }
    }
}
