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
                .GroupBy(group => group.Key.City)
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

            var currentDate = DateTime.Now.ToString("MM-dd-yyyy");
            var outputDirectoryPath = Path.Combine(_paymentTransactionsConfiguration.OutgoingDataDirectory, currentDate.ToString());
            var outputTempDirectoryPath = Path.Combine(outputDirectoryPath, "temp");
            var inputProcessedDirectoryPath = Path.Combine(_paymentTransactionsConfiguration.InnerDataDirectory, "Processed");

            var inputProcessedFilePath = Path.Combine(inputProcessedDirectoryPath, Path.GetFileName(Source));

            _fileHandling.CreateDirectoryIfNotExist(outputDirectoryPath);
            _fileHandling.CreateDirectoryIfNotExist(outputTempDirectoryPath);

            var outputFile = Path.Combine(outputDirectoryPath, string.Concat(Guid.NewGuid().ToString(), "-output.json"));
            var outputTempFile = Path.Combine(outputTempDirectoryPath, string.Concat(Guid.NewGuid().ToString(), "-temp.txt"));

            File.WriteAllText(outputFile, JsonConvert.SerializeObject(res, Formatting.Indented));

            var tempData = new List<string>();
            tempData.Add(ParseResult.Entries.Count().ToString());
            tempData.Add(ParseResult.ErrorLines.Count().ToString());
            tempData.Add(inputProcessedFilePath);

            StreamWriter writer = new StreamWriter(outputTempFile);
            foreach (var row in tempData)
            {
                writer.WriteLine(row);
            }
            writer.Close();

            // --------------  temporarily ---------------------
            //File.Move(Source, inputProcessedFilePath);
        }
    }
}
