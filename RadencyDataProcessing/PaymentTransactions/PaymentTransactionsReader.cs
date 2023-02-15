using RadencyDataProcessing.PaymentTransactions.Interfaces;

namespace RadencyDataProcessing
{
    public class PaymentTransactionsReader : IPaymentTransactionReader<IEnumerable<string>>
    {
        //private readonly NumberFormatInfo _numberFormatInfo;
        //private readonly DateTimeFormatInfo _dateTimeFormatInfo;
        //private readonly IPaymentTransactionFactory _paymentTransactionFactory;
        public PaymentTransactionsReader(
//            IPaymentTransactionFactory transactionFactory
)
        {
            //            _paymentTransactionFactory = transactionFactory;
        }
        public async IAsyncEnumerable<IEnumerable<string>> ReadAsync(string path)
        {
            //return await Task.Run(() => Read(path));
            int chunkSize = 1000;
            int countInChunk = 0;
            StreamReader reader = new(path);
            string? data;
            List<string> chunk = new List<string>(chunkSize);

            while (true)
            {
                data = await reader.ReadLineAsync();
                if (data == null) break;
                countInChunk++;
                chunk.Add(data);

                if (countInChunk == chunkSize)
                {
                    countInChunk = 0;
                    chunk.Clear();
                    yield return chunk;
                }
            }

            if (chunk.Count > 0)
            {
                yield return chunk;
            }

        }

        //private IPaymentTransactionParseResult Read(string path)
        //{
        //    var result = _paymentTransactionFactory.CreatePaymentTransactionReadResult();
        //    result.ReadFilePath = path;

        //    ReadTxt(path, result);

        //    return result;
        //}

        //private void ReadTxt(string path, IPaymentTransactionParseResult result)
        //{
        //    List<IPaymentTransactionEntry> resList = new();
        //    List<string> ErrorList = new();

        //    StreamReader reader = new(path);
        //    string? data;

        //    data = reader.ReadLine();
        //    while (data != null)
        //    {
        //        var valuesArray = SplitIgnoreQuotes(data, ",").ToArray();
        //        var entry = _paymentTransactionFactory.CreatePaymentTransactionEntry();
        //        if (entry.SetData<string[]>(valuesArray))
        //        {
        //            resList.Add(entry);
        //        }
        //        else
        //        {
        //            ErrorList.Add(data);
        //        }
        //        data = reader.ReadLine();
        //    }

        //    result.Entries = resList;
        //    result.ErrorLines = ErrorList;
        //}

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
