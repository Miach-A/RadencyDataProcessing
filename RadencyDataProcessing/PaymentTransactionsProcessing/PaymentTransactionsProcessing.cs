namespace RadencyDataProcessing
{
    public class PaymentTransactionsProcessing
    {
        private readonly ILogger<Worker> _logger;
        public PaymentTransactionsProcessing(
            ILogger<Worker> logger)
        {
            _logger = logger;
        }
        public async Task ReadData(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
