namespace RadencyDataProcessing.PaymentTransactions.Models
{
    public class PaymentTransactionTempData
    {
        public int ParsedLines { get; set; }
        public int FoundErrors { get; set; }
        public string FileName { get; set; } = string.Empty;
    }
}
