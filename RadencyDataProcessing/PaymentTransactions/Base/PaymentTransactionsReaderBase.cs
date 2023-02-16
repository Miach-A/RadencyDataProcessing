namespace RadencyDataProcessing.PaymentTransactions.Base
{
    //public abstract class PaymentTransactionsReaderBase<TReadIn, TReadResult>
    //{
    //    public virtual IAsyncEnumerable<TReadResult> ReadAsync(TReadIn path)
    //    {
    //        throw new NotImplementedException();
    //    }

    //}
    public abstract class PaymentTransactionsReaderBase<TIn, TOut>
    {
        public abstract TOut ReadAsync(TIn path);
    }
}
