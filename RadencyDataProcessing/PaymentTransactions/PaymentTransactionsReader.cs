using RadencyDataProcessing.PaymentTransactions.Interfaces;
using RadencyDataProcessing.PaymentTransactions.Models;
using System.Globalization;

namespace RadencyDataProcessing
{
    public class PaymentTransactionsReader : IPaymentTransactionsReader
    {
        private readonly NumberFormatInfo _numberFormatInfo;
        public PaymentTransactionsReader()
        {
            _numberFormatInfo = new NumberFormatInfo();
            _numberFormatInfo.NumberDecimalSeparator = ".";
        }
        public async Task<PaymentTransactionReadResult> Read(string path)
        {
            var result = new PaymentTransactionReadResult();
            result.ReadFilePath = path;

            var fileExtension = Path.GetExtension(path);
            if (fileExtension != ".txt"
                && fileExtension != ".csv")
            {
                result.Skip = true;
                return result;
            }

            if (fileExtension == ".txt")
            {
                ReadTxt(path, result);
            }


            return result;
        }

        private void ReadTxt(string path, PaymentTransactionReadResult result)
        {
            List<PaymentTransactionEntry> resList = new List<PaymentTransactionEntry>();
            List<string> ErrorList = new List<string>();

            StreamReader reader = new StreamReader(path);
            string? data;

            data = reader.ReadLine();
            while (data != null)
            {
                //var valuesArray = data.Split(",");
                var valuesArray = SplitIgnoreQuotes(data, ",").ToArray();
                if (CreateEntry(valuesArray, out PaymentTransactionEntry entry))
                {
                    resList.Add(entry);
                }
                else
                {
                    ErrorList.Add(data);
                }
                data = reader.ReadLine();
            }

            result.Entry = resList;
            result.ErrorLines = ErrorList;
        }

        private bool CreateEntry(string[] strings, out PaymentTransactionEntry entry)
        {
            entry = new PaymentTransactionEntry();
            if (strings.Count() != 7) return false;
            foreach (string s in strings)
            {
                if (s.Length == 0) return false;
            }

            if (Decimal.TryParse(strings[3], _numberFormatInfo, out decimal payment) == false) return false;
            if (DateTime.TryParse(strings[4], out DateTime date) == false) return false;
            if (long.TryParse(strings[5], out long accountNumber) == false) return false;

            entry.FirstName = strings[0];
            entry.LastName = strings[1];
            entry.Address = strings[2];
            entry.Payment = payment;
            entry.Date = date;
            entry.AccountNumber = accountNumber;
            entry.Service = strings[6];

            return true;

        }
        private List<string> SplitIgnoreQuotes(string input, string separator)
        {
            List<string> tokens = new List<string>();
            int startPosition = 0;
            bool isInQuotes = false;
            for (int currentPosition = 0; currentPosition < input.Length; currentPosition++)
            {
                if (input[currentPosition] == '\"' || input[currentPosition] == (char)8220 || input[currentPosition] == (char)8221)
                {
                    isInQuotes = !isInQuotes;
                }
                else if (input[currentPosition].ToString() == separator && !isInQuotes) //','
                {
                    tokens.Add(input.Substring(startPosition, currentPosition - startPosition).Trim());
                    startPosition = currentPosition + 1;
                }
            }

            string lastToken = input.Substring(startPosition);
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
