using RadencyDataProcessing.PaymentTransactions.Interfaces;

namespace RadencyDataProcessing
{
    public class PaymentTransactionsHandler : IPaymentTransactionHandler
    {
        public async Task HandleAsync(IPaymentTransactionParseResult parseResult)
        {
            var n = 1;
            return;
        }
    }
}
