using RadencyDataProcessing.PaymentTransactions.Models;

namespace RadencyDataProcessing.PaymentTransactions.Interfaces
{
    public interface IPaymentTransactionsReader
    {
        public Task<PaymentTransactionReadResult> Read(string path);
    }
}
