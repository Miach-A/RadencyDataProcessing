namespace RadencyDataProcessing.PaymentTransactions.Base
{
    //public abstract class PaymentTransactionFactoryBase<TEntry, TParseResult>
    //    where TEntry : PaymentTransactionEntryBase, new()
    //    where TParseResult : PaymentTransactionParseResultBase<TEntry>, new()
    //{
    //    public virtual TEntry CreatePaymentTransactionEntry()
    //    {
    //        return new TEntry();
    //    }

    //    public virtual TParseResult CreatePaymentTransactionReadResult()
    //    {
    //        return new TParseResult();
    //    }
    //}
    public abstract class PaymentTransactionFactoryBase
    {
        public abstract PaymentTransactionEntryBase CreatePaymentTransactionEntry();


        public abstract PaymentTransactionParseResultBase CreatePaymentTransactionReadResult();

    }
}
