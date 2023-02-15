using RadencyDataProcessing.PaymentTransactions.Interfaces;
using RadencyDataProcessing.PaymentTransactions.Models;

namespace RadencyDataProcessing.PaymentTransactions
{
    public class PaymentTransactionFactory : IPaymentTransactionFactory<IEnumerable<string>>
    {
        public IPaymentTransactionEntry CreatePaymentTransactionEntry()
        {
            return new PaymentTransactionEntry();
        }

        public IPaymentTransactionParser<IEnumerable<string>> CreatePaymentTransactionParser()
        {
            return new PaymentTransactionParser();
        }

        public IPaymentTransactionParseResult CreatePaymentTransactionReadResult()
        {
            return new PaymentTransactionParseResult();
        }

        public IPaymentTransactionHandler CreatePaymentTransactionsHandler()
        {
            return new PaymentTransactionsHandler();
        }

        public IPaymentTransactionReader<IEnumerable<string>> CreatePaymentTransactionsReader()
        {
            return new PaymentTransactionsReader();
        }
    }
}
