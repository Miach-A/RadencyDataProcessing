using RadencyDataProcessing.PaymentTransactions.Interfaces;
using RadencyDataProcessing.PaymentTransactions.Models;

namespace RadencyDataProcessing
{
    public class PaymentTransactionsHandler : IPaymentTransactionsHandler
    {
        public async Task Handle(PaymentTransactionReadResult readResult)
        {
            var n = 1;
            return;
        }
    }
}
