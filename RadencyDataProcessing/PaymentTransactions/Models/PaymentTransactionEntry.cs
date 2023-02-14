using RadencyDataProcessing.PaymentTransactions.Interfaces;
using System.Globalization;

namespace RadencyDataProcessing.PaymentTransactions.Models
{
    public class PaymentTransactionEntry : IPaymentTransactionEntry
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
            if (data is string[] strings)
            {
                //put in the parser class? 
                var _numberFormatInfo = new NumberFormatInfo();
                _numberFormatInfo.NumberDecimalSeparator = ".";
                var _dateTimeFormatInfo = new DateTimeFormatInfo();

                if (strings.Count() != 7) return false;
                foreach (string s in strings)
                {
                    if (s.Length == 0) return false;
                }

                if (Decimal.TryParse(strings[3], _numberFormatInfo, out decimal payment) == false) return false;
                if (DateTime.TryParseExact(strings[4], "yyyy-dd-MM", _dateTimeFormatInfo, DateTimeStyles.None, out DateTime date) == false) return false;
                if (long.TryParse(strings[5], out long accountNumber) == false) return false;

                FirstName = strings[0];
                LastName = strings[1];
                Address = strings[2];
                Payment = payment;
                Date = date;
                AccountNumber = accountNumber;
                Service = strings[6];

                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
