using RadencyDataProcessing.PaymentTransactions.Interfaces;
using RadencyDataProcessing.PaymentTransactions.Models;

namespace RadencyDataProcessing
{
    public class PaymentTransactionsHandler : IPaymentTransactionsHandler
    {
        public Task Handle(PaymentTransactionReadResult readResult)
        {
            throw new NotImplementedException();
        }
    }
}
