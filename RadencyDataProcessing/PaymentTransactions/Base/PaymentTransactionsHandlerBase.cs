namespace RadencyDataProcessing.PaymentTransactions.Base
{
    public abstract class PaymentTransactionsHandlerBase<TIn>
        where TIn : PaymentTransactionParseResultBase, new()
    {
        public abstract Task HandleAsync(TIn parseResult);
    }
}
