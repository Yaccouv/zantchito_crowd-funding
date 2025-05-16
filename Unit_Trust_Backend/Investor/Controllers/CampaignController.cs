using Microsoft.AspNetCore.Mvc;
using Unit_Trust_Backend.Investor.DTOs;
using Unit_Trust_Backend.Investor.Interfaces;
using Unit_Trust_Backend.Investor.Models;
using Unit_Trust_Backend.Investor.Services;
using Unit_Trust_Backend.Services;

namespace Unit_Trust_Backend.Investor.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CampaignController : ControllerBase
    {
        private readonly ICampaignService _campaignService;

        public CampaignController(ICampaignService campaignService)
        {
            _campaignService = campaignService;
        }

        [HttpPost("campaign")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreateCampaign([FromForm] CampaignDTO campaignDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _campaignService.CreateCampaignAsync(campaignDTO);

            if (result)
                return Ok(new { message = "Campaign registered successfully!" });
            else
                return Conflict(new { message = "Failed to register campaign." });
        }


        [HttpGet("getCampaigns")]
        public async Task<IActionResult> GetCampaigns()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var campaign = await _campaignService.GetCampaigns();
            if (campaign == null || !campaign.Any())
            {
                return NotFound(new { message = "No investments found for the given userId." });
            }

            return Ok(campaign);
        }

        [HttpGet("getUserById/{userId}")]
        public async Task<IActionResult> GetUserById(int userId)
        {
            var user = await _campaignService.GetUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPut("updateStatus/{id}")]
        public async Task<IActionResult> UpdateCampaignStatus([FromRoute] int id, [FromBody] string status)
        {
            var success = await _campaignService.UpdateCampaignStatusAsync(id, status);
            if (!success)
            {
                return NotFound();
            }

            return Ok();
        }


        [HttpGet("getActiveCampaigns")]
        public async Task<IActionResult> GetActiveCampaigns()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var campaign = await _campaignService.GetActiveCampaigns();
            if (campaign == null || !campaign.Any())
            {
                return NotFound(new { message = "No investments found for the given userId." });
            }

            return Ok(campaign);
        }


    }
}
