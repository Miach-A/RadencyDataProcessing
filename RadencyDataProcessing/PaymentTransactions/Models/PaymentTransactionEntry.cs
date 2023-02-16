using RadencyDataProcessing.PaymentTransactions.Base;

namespace RadencyDataProcessing.PaymentTransactions.Models
{
    public class PaymentTransactionEntry : PaymentTransactionEntryBase
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public decimal Payment { get; set; }
        public DateTime Date { get; set; }
        public long AccountNumber { get; set; }
        public string Service { get; set; } = string.Empty;
        public bool SetData<T>(T data)
        {
            if ((data is object[] strings) && strings.Count() == 7)
            {
                FirstName = (string)strings[0];
                LastName = (string)strings[1];
                Address = (string)strings[2];
                Payment = (decimal)strings[3];
                Date = (DateTime)strings[4];
                AccountNumber = (long)strings[5];
                Service = (string)strings[6];
                return true;
            }

            return false;
        }
    }
}
