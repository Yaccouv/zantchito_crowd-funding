namespace Unit_Trust_Backend.Investor.DTOs
{
    public class CampaignFundingReportDTO
    {
        public string ProjectTitle { get; set; }
        public decimal RaisedAmount { get; set; }
    }

    public class PaymentStatusSummaryDTO
    {
        public string Status { get; set; }
        public int Count { get; set; }
    }
}
