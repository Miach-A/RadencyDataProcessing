namespace RadencyDataProcessing.PaymentTransactions.Base
{
    public abstract class PaymentTransactionFactoryBase
    {
        public abstract PaymentTransactionEntryBase CreatePaymentTransactionEntry();


        public abstract PaymentTransactionParseResultBase CreatePaymentTransactionReadResult();

    }
}
