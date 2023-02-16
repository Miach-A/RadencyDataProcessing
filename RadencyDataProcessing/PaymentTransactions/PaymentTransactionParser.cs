using RadencyDataProcessing.PaymentTransactions.Models;
using System.Globalization;

namespace RadencyDataProcessing.PaymentTransactions
{
    public class PaymentTransactionParser : PaymentTransactionParserBase<IEnumerable<string>, PaymentTransactionParseResult>
    {
        private readonly NumberFormatInfo _numberFormatInfo;
        private readonly DateTimeFormatInfo _dateTimeFormatInfo;
        private readonly PaymentTransactionFactory _paymentTransactionFactory;
        public PaymentTransactionParser(PaymentTransactionFactory transactionFactory)
        {
            _paymentTransactionFactory = transactionFactory;
            _numberFormatInfo = new NumberFormatInfo();
            _numberFormatInfo.NumberDecimalSeparator = ".";
            _dateTimeFormatInfo = new DateTimeFormatInfo();
        }
        public override async Task<PaymentTransactionParseResult> ParseAsync(IEnumerable<string> transaction)
        {
            return await Task.Run(() => Parse(transaction));
        }

        public PaymentTransactionParseResult Parse(IEnumerable<string> transaction)
        {
            var result = _paymentTransactionFactory.CreatePaymentTransactionReadResult();
            List<PaymentTransactionEntry> entries = new List<PaymentTransactionEntry>();
            List<string> errors = new List<string>();

            foreach (var data in transaction)
            {
                var strings = SplitIgnoreQuotes(data, ",").ToArray();
                var entry = Handle(strings);

                if (entry != null)
                {
                    entries.Add(entry);
                }
                else
                {
                    errors.Add(data);
                }
            }
            result.Entries = entries;
            result.ErrorLines = errors;
            return result;
        }

        private PaymentTransactionEntry? Handle(string[] strings)
        {
            if (strings.Count() != 7) return null;
            foreach (string s in strings)
            {
                if (s.Length == 0) return null;
            }

            if (Decimal.TryParse(strings[3], _numberFormatInfo, out decimal payment) == false) return null;
            if (DateTime.TryParseExact(strings[4], "yyyy-dd-MM", _dateTimeFormatInfo, DateTimeStyles.None, out DateTime date) == false) return null;
            if (long.TryParse(strings[5], out long accountNumber) == false) return null;
            var citySearch = SplitIgnoreQuotes(strings[2], ",");
            if (citySearch.Count() == 0) return null;

            var entry = _paymentTransactionFactory.CreatePaymentTransactionEntry();
            entry.FirstName = strings[0];
            entry.LastName = strings[1];
            entry.City = citySearch[0];
            entry.Payment = payment;
            entry.Date = date;
            entry.AccountNumber = accountNumber;
            entry.Service = strings[6];

            return entry;
        }

        private List<string> SplitIgnoreQuotes(string input, string separator)
        {
            List<string> tokens = new();
            int startPosition = 0;
            bool isInQuotes = false;
            for (int currentPosition = 0; currentPosition < input.Length; currentPosition++)
            {
                if (input[currentPosition] == (char)34 || input[currentPosition] == (char)8220 || input[currentPosition] == (char)8221)
                {
                    isInQuotes = !isInQuotes;
                }
                else if (input[currentPosition].ToString() == separator && !isInQuotes)
                {
                    var resStr = input.Substring(startPosition, currentPosition - startPosition).Trim((char)32, (char)34, (char)8220, (char)8221);
                    tokens.Add(resStr);
                    startPosition = currentPosition + 1;
                }
            }

            string lastToken = input.Substring(startPosition).Trim((char)32, (char)34, (char)8220, (char)8221);
            if (lastToken.Equals(separator))
            {
                tokens.Add("");
            }
            else
            {
                tokens.Add(lastToken);
            }

            return tokens;
        }
    }
}
