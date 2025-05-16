using System;
using System.ComponentModel.DataAnnotations;

namespace Unit_Trust_Backend.Investor.Models
{
    public class Campaign
    {
        public int CampaignId { get; set; }
        public int UserId { get; set; }

        [Required]
        public string BusinessName { get; set; }

        [Required]
        public string BusinessCategory { get; set; }

        [Required]
        public string ProjectTitle { get; set; }

        [Required]
        public double FundingGoal { get; set; }

        [Required]
        public string BusinessDescription { get; set; }
        public string Status { get; set; }

        public string BusinessImage { get; set; }
        public string IDImage { get; set; }
        public DateTime CreatedAt { get; set; }
        

        public User User { get; set; }
    }
}
