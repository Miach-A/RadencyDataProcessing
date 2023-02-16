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
                                    Path.GetFileName(file).Substring(FileHandling.NewPrefix().Length)));
                Directory.Move(file, filename);
            }

            var ProcessedDirectory = Path.Combine(_paymentTransactionManager.InnerDataDirectory, "Processed");
            _fileHandling.CreateDirectoryIfNotExist(ProcessedDirectory);

            var midnightWorker = Task.Run(() => MidnightWorker(stoppingToken))
                .ContinueWith(task => TaskExceptionHandler.HandleExeption(task), TaskContinuationOptions.OnlyOnFaulted);

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
                        var newFilePath = Path.Combine(InProgressDirectory, FileHandling.NewPrefix() + Path.GetFileName(file));
                        Directory.Move(file, newFilePath);
                        movedFiles.Add(newFilePath);
                    }

                    var filesWorker = ProcessFilesAsync(movedFiles);
                }

                await Task.Delay(1000, stoppingToken);
            }
        }

        private async Task ProcessFilesAsync(IEnumerable<string> files)
        {
            await Task.CompletedTask;

            foreach (var file in files)
            {
                var fileWorker = Task.Run(() => ProcessFileAsync(file))
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

        private async void MidnightWorker(CancellationToken stoppingToken)
        {
            await Task.Delay(TimeSpan.FromSeconds(SecondsTillMidnight()));
            await Task.CompletedTask;

            var handler = _paymentTransactionManager.Factory.CreatePaymentTransactionsMidnightHandler();
            var workerFist = Task.Run(() => handler.MidnightWork())
                .ContinueWith(task => TaskExceptionHandler.HandleExeption(task), TaskContinuationOptions.OnlyOnFaulted);

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromHours(24));
                var midnightWorker = Task.Run(() => handler.MidnightWork())
                    .ContinueWith(task => TaskExceptionHandler.HandleExeption(task), TaskContinuationOptions.OnlyOnFaulted);
            }
        }

        private int SecondsTillMidnight()
        {
            var now = DateTime.Now;
            var hours = 23 - now.Hour;
            var minutes = 59 - now.Minute;
            var seconds = 59 - now.Second;
            return hours * 3600 + minutes * 60 + seconds;
        }
    }
}
