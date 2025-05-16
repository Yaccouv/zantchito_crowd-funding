using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Unit_Trust_Backend.Data;
using Unit_Trust_Backend.Investor.DTOs;
using Unit_Trust_Backend.Investor.Interfaces;
using Unit_Trust_Backend.Investor.Models;

namespace Unit_Trust_Backend.Investor.Services
{
    public class ReportService : IReportService
    {
        private readonly MyDatabase _context;

        public ReportService(MyDatabase context)
        {
            _context = context;
        }

        public async Task<List<CampaignFundingReportDTO>> GetCampaignFundingReportAsync()
        {
            var report = await _context.Campaign
                .Select(c => new CampaignFundingReportDTO
                {
                    ProjectTitle = c.ProjectTitle,
                    RaisedAmount = _context.Payments
                        .Where(p => p.CampaignId == c.CampaignId && p.Status == "succeeded")
                        .Sum(p => (decimal?)p.Amount) ?? 0
                })
                .ToListAsync();

            return report;
        }



        public async Task<List<CampaignDTO>> GetPaymentStatusSummaryAsync()
        {
            var campaigns = await _context.Campaign
                .Where(c => c.Status == "3") // Only active campaigns
                .ToListAsync();

            List<CampaignDTO> campaignWithDetails = new List<CampaignDTO>();

            foreach (var campaign in campaigns)
            {
                // Calculate RaisedAmount
                var raisedAmount = _context.Payments
                    .Where(p => p.CampaignId == campaign.CampaignId && p.Status == "succeeded")
                    .Sum(p => (decimal?)p.Amount) ?? 0;

                // Get associated project (matching CampaignId from both tables)
                var project = await _context.Project
                    .FirstOrDefaultAsync(p => p.ProjectId == campaign.CampaignId); // Match ProjectId to CampaignId

                // If no project is found, set default values for StartDate and EndDate
                DateTime startDate = project?.StartDate ?? DateTime.MinValue;  // Default to DateTime.MinValue if null
                DateTime endDate = project?.EndDate ?? DateTime.MinValue;      // Default to DateTime.MinValue if null

                // Create a CampaignDTO object combining campaign data and project details
                var campaignDataWithDetails = new CampaignDTO
                {
                    CampaignId = campaign.CampaignId,
                    ProjectTitle = campaign.ProjectTitle,
                    FundingGoal = campaign.FundingGoal,
                    StartDate = startDate, // Non-nullable DateTime assignment
                    EndDate = endDate,     // Non-nullable DateTime assignment
                    RaisedAmount = raisedAmount
                };

                // Add the campaign data with project details to the list
                campaignWithDetails.Add(campaignDataWithDetails);
            }

            return campaignWithDetails;
        }




    }
}
