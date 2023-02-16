namespace RadencyDataProcessing.PaymentTransactions.Interfaces
{
    public interface IPaymentTransactionHandler
    {
        public Task<bool> HandleAsync(IPaymentTransactionParseResult paymentEntries);
    }
}
