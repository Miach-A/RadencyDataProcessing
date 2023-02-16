using RadencyDataProcessing.PaymentTransactions.Base;

namespace RadencyDataProcessing.PaymentTransactions.Models
{
    public class PaymentTransactionParseResult : PaymentTransactionParseResultBase
    {
        public IEnumerable<string> ErrorLines { get; set; } = new List<string>();
    }
}
