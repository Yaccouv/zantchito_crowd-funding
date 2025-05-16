namespace Unit_Trust_Backend.DTOs
{
    public class DepositDTO
    {
        public int TransactionId { get; set; }
        public int AccountsId { get; set; }
        public double Amount { get; set; }
        public string TransactionType { get; set; }
        public string Status { get; set; }
        public DateTime Transactiondate { get; set; }
    }
}
