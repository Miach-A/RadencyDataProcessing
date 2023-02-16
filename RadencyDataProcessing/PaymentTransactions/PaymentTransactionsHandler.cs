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
        private readonly FileHandling _fileHandling;
        //private string _currentDate;
        private string _outputDirectoryPath;
        private string _outputTempDirectoryPath;
        private string _inputProcessedDirectoryPath;
        private string _inputProcessedFilePath;
        private string _outputFile;
        private string _outputTempFile;

        public PaymentTransactionsHandler(
            IOptions<PaymentTransactionsConfiguration> PaymentTransactionsConfiguration,
            FileHandling fileHandling)
        {
            _paymentTransactionsConfiguration = PaymentTransactionsConfiguration.Value;
            _fileHandling = fileHandling;
        }

        public PaymentTransactionParseResult ParseResult { get; set; } = new PaymentTransactionParseResult();

        public override async Task SaveAsync()
        {
            await Task.Run(() => Save())
                .ContinueWith(task => TaskExceptionHandler.HandleExeption(task), TaskContinuationOptions.OnlyOnFaulted);
        }

        private void Save()
        {
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


            SetPaths(DateTime.Now.ToString("MM-dd-yyyy"));

            var tempData = new PaymentTransactionTempData()
            {
                ParsedLines = ParseResult.Entries.Count(),
                FoundErrors = ParseResult.ErrorLines.Count(),
                FileName = _inputProcessedFilePath
            };

            File.WriteAllText(_outputFile, JsonConvert.SerializeObject(res, Formatting.Indented));
            File.WriteAllText(_outputTempFile, JsonConvert.SerializeObject(tempData, Formatting.Indented));
            File.Move(Source, _inputProcessedFilePath);
        }

        private void SetPaths(string date)
        {
            _outputDirectoryPath = Path.Combine(_paymentTransactionsConfiguration.OutgoingDataDirectory, date);

            _inputProcessedDirectoryPath = Path.Combine(_paymentTransactionsConfiguration.InnerDataDirectory, "Processed");
            _inputProcessedFilePath = Path.Combine(_inputProcessedDirectoryPath, Path.GetFileName(Source).Substring(FileHandling.NewPrefix().Length));
            _fileHandling.CreateDirectoryIfNotExist(_outputDirectoryPath);
            _outputFile = Path.Combine(_outputDirectoryPath, string.Concat(Guid.NewGuid().ToString(), "-output.json"));
            SetTempDataPaths(date);
        }

        private void SetTempDataPaths(string date)
        {
            _outputDirectoryPath = Path.Combine(_paymentTransactionsConfiguration.OutgoingDataDirectory, date);
            _outputTempDirectoryPath = Path.Combine(_outputDirectoryPath, "temp");
            _fileHandling.CreateDirectoryIfNotExist(_outputTempDirectoryPath);
            _outputTempFile = Path.Combine(_outputTempDirectoryPath, string.Concat(Guid.NewGuid().ToString(), "-temp.json"));
        }

        public void MidnightWork()
        {
            if (Directory.Exists(_outputTempDirectoryPath))
            {
                return;
            }
            SetTempDataPaths(DateTime.Now.AddDays(-1).ToString("MM-dd-yyyy"));


            int totalLines = 0;
            int errors = 0;
            List<string> invalidFiles = new List<string>();
            var files = Directory.GetFiles(_outputTempDirectoryPath);
            foreach (var file in files)
            {
                var res = JsonConvert.DeserializeObject<PaymentTransactionTempData>(File.ReadAllText(file));
                totalLines += res.ParsedLines;
                errors += res.FoundErrors;
                if (res.FoundErrors > 0)
                {
                    invalidFiles.Add(res.FileName);
                }
            }

            int parsedFiles = files.Count();

            StreamWriter stream = new StreamWriter(Path.Combine(_outputDirectoryPath, "meta.log"));
            stream.WriteLine(string.Concat("parsed_files: ", parsedFiles));
            stream.WriteLine(string.Concat("parsed_lines: ", totalLines));
            stream.WriteLine(string.Concat("found_errors: ", errors));
            stream.WriteLine(string.Concat("invalid_files: [", String.Join(", ", invalidFiles), "]"));
            stream.Close();
        }
    }
}
