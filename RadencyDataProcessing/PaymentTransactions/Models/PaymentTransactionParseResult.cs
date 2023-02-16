using RadencyDataProcessing.PaymentTransactions.Base;

namespace RadencyDataProcessing.PaymentTransactions.Models
{
    public class PaymentTransactionParseResult : PaymentTransactionParseResultBase
    {
        public List<PaymentTransactionEntry> Entries { get; set; } = new List<PaymentTransactionEntry>();
        public List<string> ErrorLines { get; set; } = new List<string>();
    }
}
