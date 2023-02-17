using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RadencyDataProcessing.Common;
using RadencyDataProcessing.PaymentTransactions;
using RadencyDataProcessing.PaymentTransactions.Base;
using RadencyDataProcessing.PaymentTransactions.Models;

namespace RadencyDataProcessing
{
    public class PaymentTransactionsHandler : PaymentTransactionsHandlerBase
    {
        private readonly PaymentTransactionsConfiguration _paymentTransactionsConfiguration;
        private readonly FileHandler _fileHandler;
        private readonly TaskExceptionHandler _taskExceptionHandler;
        private string _outputDirectoryPath = string.Empty;
        private string _outputTempDirectoryPath = string.Empty;
        private string _inputProcessedDirectoryPath = string.Empty;
        private string _inputProcessedFilePath = string.Empty;
        private string _outputFile = string.Empty;
        private string _outputTempFile = string.Empty;

        public PaymentTransactionsHandler(
            IOptions<PaymentTransactionsConfiguration> PaymentTransactionsConfiguration,
            FileHandler fileHandler,
            TaskExceptionHandler taskExceptionHandler)
        {
            _paymentTransactionsConfiguration = PaymentTransactionsConfiguration.Value;
            _fileHandler = fileHandler;
            _taskExceptionHandler = taskExceptionHandler;
        }

        public PaymentTransactionParseResult ParseResult { get; set; } = new PaymentTransactionParseResult();

        public override async Task SaveAsync()
        {
            await Task.Run(() => Save())
                .ContinueWith(task => _taskExceptionHandler.HandleExeption(task), TaskContinuationOptions.OnlyOnFaulted);
        }

        private void Save()
        {
            SetPaths(DateTime.Now.ToString("MM-dd-yyyy"));

            var res = ParseResult.Entries
                .GroupBy(entry => new { entry.Service, entry.City })
                .GroupBy(groupService => groupService.Key.City)
                    .Select(groupCity => new
                    {
                        city = groupCity.Key,
                        services = groupCity.Select(groupService => new
                        {
                            name = groupService.Key.Service,
                            payers = groupService.Select(entry => new
                            {
                                name = string.Concat(entry.FirstName, " ", entry.LastName),
                                payment = entry.Payment,
                                date = entry.Date,
                                account_number = entry.AccountNumber
                            }),
                            total = groupService.Sum(entry => entry.Payment)
                        }),
                        total = groupCity.Sum(groupService => groupService.Sum(entry => entry.Payment))
                    });

            var tempData = new PaymentTransactionTempData()
            {
                ParsedLines = ParseResult.Entries.Count(),
                FoundErrors = ParseResult.ErrorLines.Count(),
                FileName = _inputProcessedFilePath
            };

            var rollback = false;
            try
            {
                File.WriteAllText(_outputFile, JsonConvert.SerializeObject(res, Formatting.Indented));
                File.WriteAllText(_outputTempFile, JsonConvert.SerializeObject(tempData, Formatting.Indented));
                File.Move(Source, _inputProcessedFilePath);
            }
            catch (Exception ex)
            {
                rollback = true;
                throw new AggregateException(ex);
            }
            finally
            {
                if (rollback)
                {
                    if (File.Exists(_outputFile)) File.Delete(_outputFile);
                    if (File.Exists(_outputTempFile)) File.Delete(_outputTempFile);
                    if (File.Exists(_inputProcessedFilePath)) File.Move(_inputProcessedFilePath, Source);
                    if (File.Exists(Source))
                    {
                        var fileName = _fileHandler.NextAvailableFilename(Path.Combine(_paymentTransactionsConfiguration.InnerDataDirectory,
                                                                                        Path.GetFileName(Source).Substring(_fileHandler.NewPrefix().Length)));
                        File.Move(Source, fileName);
                    }
                }
            }
        }

        private void SetPaths(string date)
        {
            _outputDirectoryPath = Path.Combine(_paymentTransactionsConfiguration.OutgoingDataDirectory, date);
            _inputProcessedDirectoryPath = Path.Combine(_paymentTransactionsConfiguration.InnerDataDirectory, "Processed");
            _inputProcessedFilePath = Path.Combine(_inputProcessedDirectoryPath, Path.GetFileName(Source));
            _fileHandler.CreateDirectoryIfNotExist(_outputDirectoryPath);
            _outputFile = Path.Combine(_outputDirectoryPath, string.Concat(Guid.NewGuid().ToString(), "-output.json"));
            SetTempDataPaths(date);
        }

        private void SetTempDataPaths(string date)
        {
            _outputDirectoryPath = Path.Combine(_paymentTransactionsConfiguration.OutgoingDataDirectory, date);
            _outputTempDirectoryPath = Path.Combine(_outputDirectoryPath, "temp");
            _fileHandler.CreateDirectoryIfNotExist(_outputTempDirectoryPath);
            _outputTempFile = Path.Combine(_outputTempDirectoryPath, string.Concat(Guid.NewGuid().ToString(), "-temp.json"));
        }

        public void MidnightWork()
        {
            SetTempDataPaths(DateTime.Now.AddDays(-1).ToString("MM-dd-yyyy"));

            var metaFile = Path.Combine(_outputDirectoryPath, "meta.log");
            if (File.Exists(metaFile))
            {
                return;
            }

            int totalLines = 0;
            int errors = 0;
            List<string> invalidFiles = new List<string>();
            var files = Directory.GetFiles(_outputTempDirectoryPath);
            foreach (var file in files)
            {
                var res = JsonConvert.DeserializeObject<PaymentTransactionTempData>(File.ReadAllText(file));
                if (res == null) continue;
                totalLines += res.ParsedLines;
                errors += res.FoundErrors;
                if (res.FoundErrors > 0)
                {
                    invalidFiles.Add(res.FileName);
                }
            }

            int parsedFiles = files.Count();

            var rollback = false;
            try
            {
                StreamWriter stream = new StreamWriter(metaFile);
                stream.WriteLine(string.Concat("parsed_files: ", parsedFiles));
                stream.WriteLine(string.Concat("parsed_lines: ", totalLines));
                stream.WriteLine(string.Concat("found_errors: ", errors));
                stream.WriteLine(string.Concat("invalid_files: [", String.Join(", ", invalidFiles), "]"));
                stream.Close();
            }
            catch (Exception ex)
            {
                rollback = true;
                throw new AggregateException(ex);
            }
            finally
            {
                if (rollback)
                {
                    if (File.Exists(metaFile)) File.Delete(metaFile);
                }
                else
                {
                    Directory.Delete(_outputTempDirectoryPath, true);
                }
            }
        }
    }
}
