using System.ComponentModel.DataAnnotations;

namespace RadencyDataProcessing.PaymentTransactions
{
    public class PaymentTransactionsConfiguration
    {
        [Required]
        public string InnerDataDirectory { get; set; } = string.Empty;
        [Required]
        public string OutgoingDataDirectory { get; set; } = string.Empty;
    }
}
