using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Unit_Trust_Backend.Data;
using Unit_Trust_Backend.DTOs;
using Unit_Trust_Backend.Investor.Interfaces;

namespace Unit_Trust_Backend.Investor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepositController : ControllerBase
    {
        private readonly IDepositService _depositService;

        public DepositController(IDepositService depositService)
        {
            _depositService = depositService;
        }

        [HttpPost("deposit")]
        public async Task<IActionResult> deposit(DepositDTO depositDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _depositService.DepositAsync(depositDTO);
            if (result)
                return Ok(new { message = "success" });
            else
                return Conflict(new { message = "Not Sucessful" });

        }
    }
}
