namespace RadencyDataProcessing.PaymentTransactions.Interfaces
{
    public interface IPaymentTransactionFactory
    {
        public IPaymentTransactionEntry CreatePaymentTransactionEntry();

        public IPaymentTransactionReadResult CreatePaymentTransactionReadResult();

        public IPaymentTransactionsReader CreatePaymentTransactionsReader();

        public IPaymentTransactionsHandler CreatePaymentTransactionsHandler();
    }
}
