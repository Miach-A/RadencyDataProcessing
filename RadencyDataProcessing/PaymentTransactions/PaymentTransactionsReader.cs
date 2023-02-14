using RadencyDataProcessing.PaymentTransactions.Interfaces;
using RadencyDataProcessing.PaymentTransactions.Models;

namespace RadencyDataProcessing
{
    public class PaymentTransactionsReader : IPaymentTransactionsReader
    {
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
                var valuesArray = data.Split(",");
                if (CreateEntry(valuesArray, out PaymentTransactionEntry entry))
                {
                    resList.Add(entry);
                }
                else
                {
                    ErrorList.Add(data);
                }
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

            if (Decimal.TryParse(strings[3], out decimal payment) == false) return false;
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
    }
}
