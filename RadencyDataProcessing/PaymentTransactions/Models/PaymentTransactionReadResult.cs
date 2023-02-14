using RadencyDataProcessing.PaymentTransactions.Interfaces;

namespace RadencyDataProcessing.PaymentTransactions.Models
{
    public class PaymentTransactionReadResult : IPaymentTransactionReadResult
    {
        public IEnumerable<IPaymentTransactionEntry> Entries { get; set; } = new List<IPaymentTransactionEntry>();
        public string ReadFilePath { get; set; } = string.Empty;
        public IEnumerable<string> ErrorLines { get; set; } = new List<string>();
        public bool Skip { get; set; } = false;

    }
}
