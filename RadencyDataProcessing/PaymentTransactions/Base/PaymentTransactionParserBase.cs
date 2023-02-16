using RadencyDataProcessing.PaymentTransactions.Base;

namespace RadencyDataProcessing.PaymentTransactions
{
    public abstract class PaymentTransactionParserBase<TIn, TOut>
        where TOut : PaymentTransactionParseResultBase, new()
    {
        public abstract Task<TOut> ParseAsync(TIn transaction);
    }
}
