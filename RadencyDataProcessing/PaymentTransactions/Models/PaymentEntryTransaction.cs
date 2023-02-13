using RadencyDataProcessing.PaymentTransactions.Interfaces;

namespace RadencyDataProcessing.PaymentTransactions.Models
{
    public class PaymentEntryTransaction : IPaymentEntry
    {
        public PaymentEntryTransaction() { }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public decimal Payment { get; set; }
        public DateTime Date { get; set; }
        public long AccountNumber { get; set; }
        public string Service { get; set; }
    }
}
