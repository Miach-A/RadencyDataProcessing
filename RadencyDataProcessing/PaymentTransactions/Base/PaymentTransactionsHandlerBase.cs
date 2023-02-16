namespace RadencyDataProcessing.PaymentTransactions.Base
{
    //public abstract class PaymentTransactionsHandlerBase<TParseResult>
    //{
    //    public virtual async Task HandleAsync(TParseResult parseResult)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
    public abstract class PaymentTransactionsHandlerBase<TIn>
        where TIn : PaymentTransactionParseResultBase, new()
    {
        public abstract Task HandleAsync(TIn parseResult);
    }
}
