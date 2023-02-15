using RadencyDataProcessing.PaymentTransactions.Interfaces;
using System.Globalization;

namespace RadencyDataProcessing.PaymentTransactions
{
    public class PaymentTransactionParser : IPaymentTransactionParser<IEnumerable<string>>
    {
        private readonly NumberFormatInfo _numberFormatInfo;
        private readonly DateTimeFormatInfo _dateTimeFormatInfo;
        private readonly IPaymentTransactionFactory<IEnumerable<string>> _paymentTransactionFactory;
        public PaymentTransactionParser(
            IPaymentTransactionFactory<IEnumerable<string>> transactionFactory)
        {
            _paymentTransactionFactory = transactionFactory;
            _numberFormatInfo = new NumberFormatInfo();
            _numberFormatInfo.NumberDecimalSeparator = ".";
            _dateTimeFormatInfo = new DateTimeFormatInfo();
        }
        public async Task<IPaymentTransactionParseResult> ParseAsync(IEnumerable<string> transaction)
        {
            return await Task.Run(() => Parse(transaction));
        }

        public IPaymentTransactionParseResult Parse(IEnumerable<string> transaction)
        {
            var result = _paymentTransactionFactory.CreatePaymentTransactionReadResult();
            List<IPaymentTransactionEntry> entries = new List<IPaymentTransactionEntry>();
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

        private IPaymentTransactionEntry? Handle(string[] strings)
        {
            if (strings.Count() != 7) return null;
            foreach (string s in strings)
            {
                if (s.Length == 0) return null;
            }

            if (Decimal.TryParse(strings[3], _numberFormatInfo, out decimal payment) == false) return null;
            if (DateTime.TryParseExact(strings[4], "yyyy-dd-MM", _dateTimeFormatInfo, DateTimeStyles.None, out DateTime date) == false) return null;
            if (long.TryParse(strings[5], out long accountNumber) == false) return null;

            var entry = _paymentTransactionFactory.CreatePaymentTransactionEntry();
            object[] propherty = new object[] { strings[0], strings[1], strings[2], payment, date, accountNumber, strings[6] };
            if (entry.SetData<object[]>(propherty))
            {
                return entry;
            }

            return null;
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
