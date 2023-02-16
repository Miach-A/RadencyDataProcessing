using RadencyDataProcessing.Common;
using RadencyDataProcessing.Interfaces;

namespace RadencyDataProcessing.PaymentTransactions
{
    public class PaymentTransactionsProcessing : IProcessing
    {
        private readonly ILogger<Worker> _logger;
        private readonly PaymentTransactionManager _paymentTransactionManager;
        private readonly FileHandling _fileHandling;

        public PaymentTransactionsProcessing(
            ILogger<Worker> logger,
            PaymentTransactionManager paymentTransactionManager,
            FileHandling fileHandling
            )
        {
            _logger = logger;
            _paymentTransactionManager = paymentTransactionManager;
            _fileHandling = fileHandling;

        }

        public async Task Processing(CancellationToken stoppingToken)
        {
            var InProgressDirectory = Path.Combine(_paymentTransactionManager.InnerDataDirectory, "InProgress");
            _fileHandling.CreateDirectoryIfNotExist(InProgressDirectory);

            var filesNotProcessedLastTime = Directory.GetFiles(InProgressDirectory);
            foreach (var file in filesNotProcessedLastTime)
            {
                var filename = _fileHandling.NextAvailableFilename(
                    Path.Combine(_paymentTransactionManager.InnerDataDirectory,
                                    Path.GetFileName(file).Substring(NewInProgressPrefix().Length)));
                Directory.Move(file, filename);
            }

            var ProcessedDirectory = Path.Combine(_paymentTransactionManager.InnerDataDirectory, "Processed");
            _fileHandling.CreateDirectoryIfNotExist(ProcessedDirectory);

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Process files running at: {time}", DateTimeOffset.Now);
                var files = Directory.GetFiles(_paymentTransactionManager.InnerDataDirectory)
                    .Where(file => Path.GetExtension(file).ToLower() == ".txt"
                        || Path.GetExtension(file).ToLower() == ".csv");

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
                var ParallelProcessing = Task.Run(() => ProcessFileAsync(file))
                    .ContinueWith(task => TaskExceptionHandler.HandleExeption(task), TaskContinuationOptions.OnlyOnFaulted);
            }
        }

        private async Task ProcessFileAsync(string file)
        {
            var hanler = _paymentTransactionManager.Factory.CreatePaymentTransactionsHandler(file);
            var readChunks = _paymentTransactionManager.Reader.ReadAsync(file);
            await foreach (var readChunk in readChunks)
            {
                var parsingChank = await _paymentTransactionManager.Parser.ParseAsync(readChunk);
                hanler.ParseResult.Entries.AddRange(parsingChank.Entries);
                hanler.ParseResult.ErrorLines.AddRange(parsingChank.ErrorLines);
            }
            await hanler.SaveAsync();
        }

    }
}
