using Microsoft.Extensions.Options;

namespace RadencyDataProcessing.PaymentTransactions
{
    public class PaymentTransactionsProcessing
    {
        private readonly ILogger<Worker> _logger;
        private readonly string _innerDataDirectory;
        private readonly string _outgoingDataDirectory;

        public PaymentTransactionsProcessing(
            ILogger<Worker> logger,
            IOptions<PaymentTransactionsConfiguration> PaymentTransactionsConfiguration)
        {
            _logger = logger;
            _innerDataDirectory = PaymentTransactionsConfiguration.Value.InnerDataDirectory;
            _outgoingDataDirectory = PaymentTransactionsConfiguration.Value.OutgoingDataDirectory;
            Console.WriteLine(_innerDataDirectory);
            Console.WriteLine(_outgoingDataDirectory);
        }

        private void ValidateConfiguration()
        {
            if (_innerDataDirectory == string.Empty
                || _outgoingDataDirectory == string.Empty)
            {
                throw new ArgumentException("appsettings.json -> InnerDataDirectory / OutgoingDataDirectory");
            }
        }

        public async Task ReadData(CancellationToken stoppingToken)
        {
            var InProgressDirectory = Path.Combine(_innerDataDirectory, "InProgress");
            CreateDirectoryIfNotExist(InProgressDirectory);

            var filesNotProcessedLastTime = Directory.GetFiles(InProgressDirectory);
            foreach (var file in filesNotProcessedLastTime)
            {
                Directory.Move(file, Path.Combine(_innerDataDirectory, Path.GetFileName(file)));
            }

            var ProcessedDirectory = Path.Combine(_innerDataDirectory, "Processed");
            CreateDirectoryIfNotExist(ProcessedDirectory);

            while (!stoppingToken.IsCancellationRequested)
            {
                //Read files. Move to folder "in progress". While reading is in progress, can get a new list after N seconds and process it too
                //after processing, move to the "processed" folder  
                _logger.LogInformation("Process files running at: {time}", DateTimeOffset.Now);
                //ProcessFiles();
                await Task.Delay(1000, stoppingToken);
            }
        }

        private async Task ProcessFiles()
        {
            var files = Directory.GetFiles(_innerDataDirectory);
            await Task.Delay(1000);
        }

        private void CreateDirectoryIfNotExist(string directory)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }
    }
}
