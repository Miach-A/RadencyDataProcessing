using RadencyDataProcessing.PaymentTransactions.Interfaces;
using RadencyDataProcessing.PaymentTransactions.Models;

namespace RadencyDataProcessing.PaymentTransactions
{
    public class PaymentTransactionFactory : IPaymentTransactionFactory
    {
        public IPaymentTransactionEntry CreatePaymentTransactionEntry()
        {
            return new PaymentTransactionEntry();
        }

        public IPaymentTransactionReadResult CreatePaymentTransactionReadResult()
        {
            return new PaymentTransactionReadResult();
        }

        public IPaymentTransactionsHandler CreatePaymentTransactionsHandler()
        {
            return new PaymentTransactionsHandler();
        }

        public IPaymentTransactionsReader CreatePaymentTransactionsReader()
        {
            return new PaymentTransactionsReader(this);
        }
    }
}
