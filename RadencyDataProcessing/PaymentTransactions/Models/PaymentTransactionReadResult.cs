using RadencyDataProcessing.PaymentTransactions.Interfaces;

namespace RadencyDataProcessing.PaymentTransactions.Models
{
    public class PaymentTransactionParseResult : IPaymentTransactionParseResult
    {
        public IEnumerable<IPaymentTransactionEntry> Entries { get; set; } = new List<IPaymentTransactionEntry>();
        public string ReadFilePath { get; set; } = string.Empty;
        public IEnumerable<string> ErrorLines { get; set; } = new List<string>();

    }
}
