using RadencyDataProcessing.PaymentTransactions.Interfaces;

namespace RadencyDataProcessing.PaymentTransactions.Models
{
    public class PaymentTransactionReadResult
    {
        public IEnumerable<IPaymentEntry> Entry { get; set; } = new List<IPaymentEntry>();
        public string ReadFilePath { get; set; } = string.Empty;
        //public string ErrorFilePath { get; set; } = string.Empty;
        public IEnumerable<string> ErrorLines { get; set; }
        public bool Skip { get; set; } = false;

    }
}
