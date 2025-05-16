using Microsoft.AspNetCore.Mvc;

namespace Unit_Trust_Backend.Investor.Models
{
    public class Payment
    {
        public int PaymentId { get; set; }
        public int CampaignId { get; set; }
        public double Amount { get; set; }
        public string Status { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}
