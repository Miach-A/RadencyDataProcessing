namespace RadencyDataProcessing.PaymentTransactions.Interfaces
{
    public interface IPaymentTransactionFactory<T>
    {
        public IPaymentTransactionEntry CreatePaymentTransactionEntry();

        public IPaymentTransactionParseResult CreatePaymentTransactionReadResult();

        //public IPaymentTransactionReader<T> CreatePaymentTransactionsReader();

        //public IPaymentTransactionHandler CreatePaymentTransactionsHandler();

        //public IPaymentTransactionParser<T> CreatePaymentTransactionParser();
    }
}
