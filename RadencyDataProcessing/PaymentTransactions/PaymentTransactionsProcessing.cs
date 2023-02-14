using Microsoft.Extensions.Options;
using RadencyDataProcessing.PaymentTransactions.Interfaces;

namespace RadencyDataProcessing.PaymentTransactions
{
    public class PaymentTransactionsProcessing : IPaymentTransactionsProcessing
    {
        private readonly ILogger<Worker> _logger;
        private readonly string _innerDataDirectory;
        private readonly string _outgoingDataDirectory;
        private readonly IPaymentTransactionsReader _paymentTransactionsReader;
        private readonly IPaymentTransactionsHandler _paymentTransactionsHandler;

        public PaymentTransactionsProcessing(
            ILogger<Worker> logger,
            IOptions<PaymentTransactionsConfiguration> PaymentTransactionsConfiguration,
            IPaymentTransactionsReader paymentTransactionsReader,
            IPaymentTransactionsHandler paymentTransactionsHandler)
        {
            _logger = logger;
            _innerDataDirectory = PaymentTransactionsConfiguration.Value.InnerDataDirectory;
            _outgoingDataDirectory = PaymentTransactionsConfiguration.Value.OutgoingDataDirectory;
            _paymentTransactionsReader = paymentTransactionsReader;
            _paymentTransactionsHandler = paymentTransactionsHandler;
        }

        public async Task TransactionProcessing(CancellationToken stoppingToken)
        {
            var InProgressDirectory = Path.Combine(_innerDataDirectory, "InProgress");
            CreateDirectoryIfNotExist(InProgressDirectory);

            var filesNotProcessedLastTime = Directory.GetFiles(InProgressDirectory);
            foreach (var file in filesNotProcessedLastTime)
            {
                Directory.Move(file, Path.Combine(_innerDataDirectory, Path.GetFileName(file).Substring(NewInProgressPrefix().Length))); // or magic numbers antipattern 37 = Guid.NewGuid() + "_"
            }

            var ProcessedDirectory = Path.Combine(_innerDataDirectory, "Processed");
            CreateDirectoryIfNotExist(ProcessedDirectory);
            while (!stoppingToken.IsCancellationRequested)
            {
                //Read files. Move to folder "in progress". While reading is in progress, can get a new list after N seconds and process it too
                //after processing, move to the "processed" folder  
                _logger.LogInformation("Process files running at: {time}", DateTimeOffset.Now);
                var files = Directory.GetFiles(_innerDataDirectory);

                if (files.Length > 0)
                {
                    for (int i = 0; i < files.Length; i++)
                    {
                        var newFilePath = Path.Combine(InProgressDirectory, NewInProgressPrefix() + Path.GetFileName(files[i]));
                        Directory.Move(files[i], newFilePath);
                        files[i] = newFilePath;
                    }

                    Task ParallelProcessing = ProcessFiles(files);
                }

                await Task.Delay(1000, stoppingToken);
            }
        }

        private string NewInProgressPrefix()
        {
            return Guid.NewGuid().ToString() + "_";
        }

        private async Task ProcessFiles(string[] files)
        {
            for (int i = 0; i < files.Length; i++)
            {
                var ParallelProcessing = _paymentTransactionsReader.Read(files[i])
                    .ContinueWith(result => _paymentTransactionsHandler.Handle(result.Result));
            }

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
