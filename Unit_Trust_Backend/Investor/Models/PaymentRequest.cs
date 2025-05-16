namespace Unit_Trust_Backend.Investor.Models
{
    public class PaymentRequest
    {
        public int Amount { get; set; } // in cents
        public int CampaignId { get; set; }
    }
}
