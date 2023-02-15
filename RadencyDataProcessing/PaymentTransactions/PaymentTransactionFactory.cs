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

        public IPaymentTransactionParseResult CreatePaymentTransactionReadResult()
        {
            return new PaymentTransactionParseResult();
        }

        public IPaymentTransactionHandler CreatePaymentTransactionsHandler()
        {
            return new PaymentTransactionsHandler();
        }

        public IPaymentTransactionReader CreatePaymentTransactionsReader()
        {
            return new PaymentTransactionsReader(this);
        }
    }
}
