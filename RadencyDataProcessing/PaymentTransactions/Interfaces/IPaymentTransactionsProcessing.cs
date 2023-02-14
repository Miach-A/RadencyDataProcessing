namespace RadencyDataProcessing.PaymentTransactions.Interfaces
{
    public interface IPaymentTransactionsProcessing
    {
        public Task TransactionProcessing(CancellationToken stoppingToken);
    }
}
