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
            var directory = _configuration.GetValue<string>("InnerDataPath");
            if (directory == null)
            {
                throw new ArgumentNullException("appsettings.json -> InnerDataPath");
            }

            var InProgressDirectoryPath = Path.Combine(directory, "InProgress");
            if (!Directory.Exists(InProgressDirectoryPath))
            {
                Directory.CreateDirectory(InProgressDirectoryPath);
            }

            //var filesNotProcessedLastTime = Directory.GetFiles(InProgressDirectoryPath); 
            //foreach (var file in filesNotProcessedLastTime)
            //{
            //    Directory.Move()
            //}

            var ProcessedDirectoryPath = Path.Combine(directory, "Processed");
            if (!Directory.Exists(ProcessedDirectoryPath))
            {
                Directory.CreateDirectory(ProcessedDirectoryPath);
            }

            while (!stoppingToken.IsCancellationRequested)
            {
                //Read files. Move to folder "in progress". While reading is in progress, can get a new list after N seconds and process it too
                //after processing, move to the "processed" folder

                var files = Directory.GetFiles(directory);


                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);

            }
        }
    }
}
