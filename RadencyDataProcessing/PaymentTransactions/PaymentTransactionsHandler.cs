using RadencyDataProcessing.PaymentTransactions.Interfaces;

namespace RadencyDataProcessing
{
    public class PaymentTransactionsHandler : IPaymentTransactionHandler
    {
        public async Task HandleAsync(IAsyncEnumerable<IPaymentTransactionParseResult> readResult)
        {
            var n = 1;
            return;
        }
    }
}
