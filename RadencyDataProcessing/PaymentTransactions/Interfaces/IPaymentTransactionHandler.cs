namespace RadencyDataProcessing.PaymentTransactions.Interfaces
{
    public interface IPaymentTransactionHandler
    {
        public Task Handle(IPaymentTransactionParseResult paymentEntries);
    }
}
