using RadencyDataProcessing.PaymentTransactions.Interfaces;

namespace RadencyDataProcessing
{
    public class PaymentTransactionsHandler : IPaymentTransactionHandler
    {
        public async Task Handle(IPaymentTransactionParseResult readResult)
        {
            var n = 1;
            return;
        }
    }
}
