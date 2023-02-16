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
        private string _currentDate;
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


            SetPaths();

            File.WriteAllText(_outputFile, JsonConvert.SerializeObject(res, Formatting.Indented));

            var tempData = new List<string>();
            tempData.Add(ParseResult.Entries.Count().ToString());
            tempData.Add(ParseResult.ErrorLines.Count().ToString());
            tempData.Add(_inputProcessedFilePath);

            StreamWriter writer = new StreamWriter(_outputTempFile);
            foreach (var row in tempData)
            {
                writer.WriteLine(row);
            }
            writer.Close();

            // --------------  temporarily ---------------------
            File.Move(Source, _inputProcessedFilePath);
        }

        private void SetPaths()
        {
            _currentDate = DateTime.Now.ToString("MM-dd-yyyy");
            _outputDirectoryPath = Path.Combine(_paymentTransactionsConfiguration.OutgoingDataDirectory, _currentDate.ToString());
            _outputTempDirectoryPath = Path.Combine(_outputDirectoryPath, "temp");
            _inputProcessedDirectoryPath = Path.Combine(_paymentTransactionsConfiguration.InnerDataDirectory, "Processed");

            _inputProcessedFilePath = Path.Combine(_inputProcessedDirectoryPath, Path.GetFileName(Source));

            _fileHandling.CreateDirectoryIfNotExist(_outputDirectoryPath);
            _fileHandling.CreateDirectoryIfNotExist(_outputTempDirectoryPath);

            _outputFile = Path.Combine(_outputDirectoryPath, string.Concat(Guid.NewGuid().ToString(), "-output.json"));
            _outputTempFile = Path.Combine(_outputTempDirectoryPath, string.Concat(Guid.NewGuid().ToString(), "-temp.txt"));
        }

        public void MidnightWork()
        {
            //await Task.CompletedTask;

            //work
        }
    }
}
