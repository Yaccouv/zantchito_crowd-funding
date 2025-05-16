using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Unit_Trust_Backend.Investor.Models
{
    public class Project
    {
        public int CampaignId { get; set; }
        public int ProjectId { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
