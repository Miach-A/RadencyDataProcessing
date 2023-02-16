using RadencyDataProcessing.PaymentTransactions.Base;

namespace RadencyDataProcessing.PaymentTransactions.Models
{
    public class PaymentTransactionParseResult : PaymentTransactionParseResultBase
    {
        public List<string> ErrorLines { get; set; } = new List<string>();
    }
}
