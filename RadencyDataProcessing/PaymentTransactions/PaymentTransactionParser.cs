using RadencyDataProcessing.PaymentTransactions.Interfaces;

namespace RadencyDataProcessing.PaymentTransactions
{
    public class PaymentTransactionParser : IPaymentTransactionParser<IEnumerable<string>>
    {
        public async Task<IPaymentTransactionParseResult> ParseAsync(IEnumerable<string> transaction)
        {
            throw new NotImplementedException();
        }
    }
}
