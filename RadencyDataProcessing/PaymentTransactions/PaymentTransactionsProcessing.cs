namespace RadencyDataProcessing.PaymentTransactions
{
    public class PaymentTransactionsProcessing
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _configuration;
        public PaymentTransactionsProcessing(
            ILogger<Worker> logger,
            IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }
        public async Task ReadData(CancellationToken stoppingToken)
        {
            var directory = _configuration.GetValue<string>("InnerDataPath11");
            if (directory == null)
            {
                throw new ArgumentNullException("appsettings.json -> InnerDataPath");
            }

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
                //var files = Directory.GetFiles(directory);
            }
        }
    }
}
