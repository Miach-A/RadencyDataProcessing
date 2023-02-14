using RadencyDataProcessing.PaymentTransactions.Interfaces;

namespace RadencyDataProcessing
{
    public class PaymentTransactionsReader : IPaymentTransactionsReader
    {
        //private readonly NumberFormatInfo _numberFormatInfo;
        //private readonly DateTimeFormatInfo _dateTimeFormatInfo;
        private readonly IPaymentTransactionFactory _paymentTransactionFactory;
        public PaymentTransactionsReader(
            IPaymentTransactionFactory transactionFactory)
        {
            _paymentTransactionFactory = transactionFactory;
        }
        public async Task<IPaymentTransactionReadResult> Read(string path)
        {
            var result = _paymentTransactionFactory.CreatePaymentTransactionReadResult();
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

        private void ReadTxt(string path, IPaymentTransactionReadResult result)
        {
            List<IPaymentTransactionEntry> resList = new List<IPaymentTransactionEntry>();
            List<string> ErrorList = new List<string>();

            StreamReader reader = new StreamReader(path);
            string? data;

            data = reader.ReadLine();
            while (data != null)
            {
                var valuesArray = SplitIgnoreQuotes(data, ",").ToArray();
                var entry = _paymentTransactionFactory.CreatePaymentTransactionEntry();
                if (entry.SetData<string[]>(valuesArray))
                {
                    resList.Add(entry);
                }
                else
                {
                    ErrorList.Add(data);
                }
                data = reader.ReadLine();
            }

            result.Entries = resList;
            result.ErrorLines = ErrorList;
        }

        private List<string> SplitIgnoreQuotes(string input, string separator)
        {
            List<string> tokens = new List<string>();
            int startPosition = 0;
            bool isInQuotes = false;
            for (int currentPosition = 0; currentPosition < input.Length; currentPosition++)
            {
                if (input[currentPosition] == (char)34 || input[currentPosition] == (char)8220 || input[currentPosition] == (char)8221)
                {
                    isInQuotes = !isInQuotes;
                }
                else if (input[currentPosition].ToString() == separator && !isInQuotes) //','
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
