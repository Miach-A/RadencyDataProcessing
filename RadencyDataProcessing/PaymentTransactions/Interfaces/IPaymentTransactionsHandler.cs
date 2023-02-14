using RadencyDataProcessing.PaymentTransactions.Models;

namespace RadencyDataProcessing.PaymentTransactions.Interfaces
{
    public interface IPaymentTransactionsHandler
    {
        public Task Handle(PaymentTransactionReadResult paymentEntries);
    }
}
