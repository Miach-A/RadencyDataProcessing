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
            //Console.WriteLine(JsonSerializer.Serialize(parseResult,JsonSerializerOptions.));
            Console.WriteLine(JsonConvert.SerializeObject(ParseResult, Formatting.Indented));
            var b = 2;
        }
    }
}
