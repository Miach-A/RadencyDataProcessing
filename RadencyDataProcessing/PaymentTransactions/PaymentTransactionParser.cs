using RadencyDataProcessing.PaymentTransactions.Interfaces;

namespace RadencyDataProcessing.PaymentTransactions
{
    public class PaymentTransactionParser : IPaymentTransactionParser<IEnumerable<string>>
    {
        public IAsyncEnumerable<IPaymentTransactionParseResult> ParseAsync(IAsyncEnumerable<IEnumerable<string>> transaction)
        {
            throw new NotImplementedException();
        }
    }
}
