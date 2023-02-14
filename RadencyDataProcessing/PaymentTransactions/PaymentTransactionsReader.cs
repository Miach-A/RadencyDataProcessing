using RadencyDataProcessing.PaymentTransactions.Interfaces;
using RadencyDataProcessing.PaymentTransactions.Models;

namespace RadencyDataProcessing
{
    public class PaymentTransactionsReader : IPaymentTransactionsReader
    {
        public async Task<PaymentTransactionReadResult> Read(string path)
        {
            var fileExtension = Path.GetExtension(path);
            if (fileExtension != ".txt"
                && fileExtension != ".csv")
            {
                return new PaymentTransactionReadResult() { Skip = true };
            }

            if (fileExtension == ".txt")
            {
                ReadTxt(path);
            }

            return new PaymentTransactionReadResult();
        }

        private void ReadTxt(string path)
        {
            List<PaymentTransactionEntry> resList = new List<PaymentTransactionEntry>();

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
            }

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
