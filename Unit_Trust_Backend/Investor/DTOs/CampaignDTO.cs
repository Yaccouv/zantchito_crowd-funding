using System;
using Microsoft.AspNetCore.Http;

namespace Unit_Trust_Backend.Investor.DTOs
{
    public class CampaignDTO
    {
        public int CampaignId { get; set; }
        public int UserId { get; set; }
        public string BusinessName { get; set; }
        public string BusinessCategory { get; set; }
        public string ProjectTitle { get; set; }
        public double FundingGoal { get; set; }
        public string BusinessDescription { get; set; }
        public string Status { get; set; }
        public decimal RaisedAmount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime StartDate { get; set; }


        public IFormFile BusinessImage { get; set; }
        public IFormFile IDImage { get; set; }
    }
}
