using System.Collections.Generic;
using System.Threading.Tasks;
using Unit_Trust_Backend.Investor.Models;
using Unit_Trust_Backend.Investor.DTOs;

namespace Unit_Trust_Backend.Investor.Interfaces
{
    public interface ICampaignService
    {
        Task<bool> CreateCampaignAsync(CampaignDTO campaignDTO);
        Task<List<Campaign>> GetCampaigns();
        Task<List<object>> GetActiveCampaigns();
        Task GetUserId(int userId);
        Task<User> GetUserByIdAsync(int userId);
        Task<bool> UpdateCampaignStatusAsync(int campaignId, string status);
    }
}
