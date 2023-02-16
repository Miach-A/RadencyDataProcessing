using RadencyDataProcessing.PaymentTransactions.Base;

namespace RadencyDataProcessing.PaymentTransactions
{
    //public abstract class PaymentTransactionParserBase<TReadResult, TParseResult>
    //{
    //    public virtual async Task<TParseResult> ParseAsync(TReadResult transaction)
    //    {
    //        throw new NotImplementedException();
    //    }

    //}

    public abstract class PaymentTransactionParserBase<TIn, TOut>
        where TOut : PaymentTransactionParseResultBase, new()
    {
        public abstract Task<TOut> ParseAsync(TIn transaction);//object transaction{

        public abstract PaymentTransactionParseResultBase test();
    }
}
