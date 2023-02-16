using Newtonsoft.Json;
using RadencyDataProcessing.PaymentTransactions.Base;
using RadencyDataProcessing.PaymentTransactions.Models;

namespace RadencyDataProcessing
{
    public class PaymentTransactionsHandler : PaymentTransactionsHandlerBase
    {
        public PaymentTransactionsHandler(string source) : base(source)
        {
        }

        public PaymentTransactionParseResult ParseResult { get; set; } = new PaymentTransactionParseResult();
        public override async Task SaveAsync()
        {
            await Task.Run(() => Save());
        }

        private void Save()
        {
            var res = ParseResult.Entries
                .GroupBy(x => new { x.Service, x.City })
                .GroupBy(x => x.Key.City)
                    .Select(x => new
                    {
                        city = x.Key,
                        services = x.Select(y => new
                        {
                            name = y.Key.Service,
                            payers = y.Select(z => new
                            {
                                name = z.FirstName + z.LastName,
                                payment = z.Payment,
                                date = z.Date,
                                account_number = z.AccountNumber
                            }),
                            total = y.Sum(z => z.Payment)
                        }),
                        total = x.Sum(y => y.Sum(z => z.Payment))
                    });

            Console.WriteLine(JsonConvert.SerializeObject(res, Formatting.Indented));
            var b = 2;
        }
    }
}
