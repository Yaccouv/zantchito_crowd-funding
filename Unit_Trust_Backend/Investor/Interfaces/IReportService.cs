using System.Collections.Generic;
using System.Threading.Tasks;
using Unit_Trust_Backend.Investor.DTOs;
using Unit_Trust_Backend.Investor.Models;

namespace Unit_Trust_Backend.Investor.Interfaces
{
    public interface IReportService
    {
        Task<List<CampaignFundingReportDTO>> GetCampaignFundingReportAsync();
        Task<List<CampaignDTO>> GetPaymentStatusSummaryAsync();
    }
}
