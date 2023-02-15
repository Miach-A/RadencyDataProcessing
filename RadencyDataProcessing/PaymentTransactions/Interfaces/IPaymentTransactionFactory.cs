namespace RadencyDataProcessing.PaymentTransactions.Interfaces
{
    public interface IPaymentTransactionFactory
    {
        public IPaymentTransactionEntry CreatePaymentTransactionEntry();

        public IPaymentTransactionParseResult CreatePaymentTransactionReadResult();

        public IPaymentTransactionReader CreatePaymentTransactionsReader();

        public IPaymentTransactionHandler CreatePaymentTransactionsHandler();
    }
}
