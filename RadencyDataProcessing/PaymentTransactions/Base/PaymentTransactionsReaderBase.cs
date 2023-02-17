namespace RadencyDataProcessing.PaymentTransactions.Base
{
    public abstract class PaymentTransactionsReaderBase<TIn, TOut>
    {
        public abstract TOut ReadAsync(TIn path);
    }
}
