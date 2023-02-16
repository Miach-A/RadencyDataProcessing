namespace RadencyDataProcessing.PaymentTransactions.Interfaces
{
    public interface IProcessing
    {
        public Task Processing(CancellationToken stoppingToken);
    }
}
