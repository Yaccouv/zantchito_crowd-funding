using Microsoft.AspNetCore.Mvc;

namespace Unit_Trust_Backend.Investor.DTOs
{
    public class ProjectDTO
    {
        public int CampaignId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
