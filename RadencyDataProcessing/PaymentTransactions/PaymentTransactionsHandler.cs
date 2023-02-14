using RadencyDataProcessing.PaymentTransactions.Interfaces;

namespace RadencyDataProcessing
{
    public class PaymentTransactionsHandler : IPaymentTransactionsHandler
    {
        public Task Handle(IEnumerable<IPaymentEntry> paymentEntries)
        {
            throw new NotImplementedException();
        }
    }
}
