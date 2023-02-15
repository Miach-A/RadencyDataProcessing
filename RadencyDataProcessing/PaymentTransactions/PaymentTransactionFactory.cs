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

        public IPaymentTransactionParseResult CreatePaymentTransactionReadResult()
        {
            return new PaymentTransactionParseResult();
        }

        //public IPaymentTransactionParser<IEnumerable<string>> CreatePaymentTransactionParser()
        //{
        //    return new PaymentTransactionParser(this);
        //}

        //public IPaymentTransactionHandler CreatePaymentTransactionsHandler()
        //{
        //    return new PaymentTransactionsHandler();
        //}

        //public IPaymentTransactionReader<IEnumerable<string>> CreatePaymentTransactionsReader()
        //{
        //    return new PaymentTransactionsReader();
        //}
    }
}
