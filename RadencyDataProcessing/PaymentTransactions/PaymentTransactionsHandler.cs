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
            await Task.Run(() => Save());
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
            var fileOutputfile = _fileHandling.NextAvailableFilename(Path.Combine(_paymentTransactionsConfiguration.OutgoingDataDirectory, currentDate.ToString(), "output.json"));
            Console.WriteLine(fileOutputfile);
            File.WriteAllText(fileOutputfile, JsonConvert.SerializeObject(res, Formatting.Indented));

            //Console.WriteLine(JsonConvert.SerializeObject(res, Formatting.Indented));
            var b = 2;
        }
    }
}
