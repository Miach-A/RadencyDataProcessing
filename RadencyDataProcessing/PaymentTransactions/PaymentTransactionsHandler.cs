using RadencyDataProcessing.PaymentTransactions.Interfaces;

namespace RadencyDataProcessing
{
    public class PaymentTransactionsHandler : IPaymentTransactionsHandler
    {
        public async Task Handle(IPaymentTransactionReadResult readResult)
        {
            var n = 1;
            return;
        }
    }
}
