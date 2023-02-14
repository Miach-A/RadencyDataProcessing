namespace RadencyDataProcessing.PaymentTransactions.Interfaces
{
    public interface IPaymentTransactionsHandler
    {
        public Task Handle(IPaymentTransactionReadResult paymentEntries);
    }
}
