namespace RadencyDataProcessing.PaymentTransactions.Interfaces
{
    public interface IPaymentTransactionHandler
    {
        public Task HandleAsync(IAsyncEnumerable<IPaymentTransactionParseResult> paymentEntries);
    }
}
