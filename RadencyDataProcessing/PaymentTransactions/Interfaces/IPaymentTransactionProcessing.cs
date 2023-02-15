namespace RadencyDataProcessing.PaymentTransactions.Interfaces
{
    public interface IPaymentTransactionProcessing
    {
        public Task TransactionProcessing(CancellationToken stoppingToken);
    }
}
