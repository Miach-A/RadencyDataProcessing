using Microsoft.Extensions.Options;
using RadencyDataProcessing.PaymentTransactions.Interfaces;

namespace RadencyDataProcessing.PaymentTransactions
{
    public class PaymentTransactionsProcessing : IPaymentTransactionProcessing
    {
        private readonly ILogger<Worker> _logger;
        private readonly IPaymentTransactionManager<IEnumerable<string>> _paymentTransactionManager;

        public PaymentTransactionsProcessing(
            ILogger<Worker> logger,
            IOptions<PaymentTransactionsConfiguration> PaymentTransactionsConfiguration,
            IPaymentTransactionManager<IEnumerable<string>> paymentTransactionManager
            )
        {
            _logger = logger;
            _paymentTransactionManager = paymentTransactionManager;
        }

        public async Task TransactionProcessing(CancellationToken stoppingToken)
        {
            var InProgressDirectory = Path.Combine(_paymentTransactionManager.InnerDataDirectory, "InProgress");
            CreateDirectoryIfNotExist(InProgressDirectory);

            var filesNotProcessedLastTime = Directory.GetFiles(InProgressDirectory);
            foreach (var file in filesNotProcessedLastTime)
            {
                Directory.Move(file, Path.Combine(_paymentTransactionManager.InnerDataDirectory, Path.GetFileName(file).Substring(NewInProgressPrefix().Length))); // or magic numbers antipattern 37 = Guid.NewGuid() + "_"
            }

            var ProcessedDirectory = Path.Combine(_paymentTransactionManager.InnerDataDirectory, "Processed");
            CreateDirectoryIfNotExist(ProcessedDirectory);
            while (!stoppingToken.IsCancellationRequested)
            {
                //Read files. Move to folder "in progress". While reading is in progress, can get a new list after N seconds and process it too
                //after processing, move to the "processed" folder  
                _logger.LogInformation("Process files running at: {time}", DateTimeOffset.Now);
                var files = Directory.GetFiles(_paymentTransactionManager.InnerDataDirectory).Where(file => file.EndsWith(".txt") || file.EndsWith(".csv"));

                if (files.Count() > 0)
                {
                    List<string> movedFiles = new List<string>(files.Count());
                    foreach (var file in files)
                    {
                        var newFilePath = Path.Combine(InProgressDirectory, NewInProgressPrefix() + Path.GetFileName(file));
                        Directory.Move(file, newFilePath);
                        movedFiles.Add(newFilePath);
                    }

                    var ParallelProcessing = ProcessFilesAsync(movedFiles);
                }

                await Task.Delay(1000, stoppingToken);
            }
        }

        private string NewInProgressPrefix()
        {
            return Guid.NewGuid().ToString() + "_";
        }

        private async Task ProcessFilesAsync(IEnumerable<string> files)
        {
            await Task.CompletedTask;

            foreach (var file in files)
            {
                var ParallelProcessing = Task.Run(() => ProcessFileAsync(file));
            }
        }

        private async Task ProcessFileAsync(string file)
        {
            var readChunks = _paymentTransactionManager.Reader.ReadAsync(file);
            await foreach (var readChunk in readChunks)
            {
                var parsingChank = await _paymentTransactionManager.Parser.ParseAsync(readChunk);
                await _paymentTransactionManager.Handler.HandleAsync(parsingChank);
            }

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
