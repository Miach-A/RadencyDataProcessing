using RadencyDataProcessing.PaymentTransactions.Base;
using RadencyDataProcessing.PaymentTransactions.Models;

namespace RadencyDataProcessing
{
    public class PaymentTransactionsHandler : PaymentTransactionsHandlerBase<PaymentTransactionParseResult>
    {
        public override async Task<bool> HandleAsync(PaymentTransactionParseResult parseResult)
        {
            return await Task.Run(() => Handle());
        }

        public bool Handle()
        {
            return true;
        }
    }
}
