using RadencyDataProcessing.PaymentTransactions.Base;
using RadencyDataProcessing.PaymentTransactions.Models;

namespace RadencyDataProcessing.PaymentTransactions
{
    public class PaymentTransactionFactory : PaymentTransactionFactoryBase
    {
        public override PaymentTransactionEntry CreatePaymentTransactionEntry()
        {
            return new PaymentTransactionEntry();
        }

        public override PaymentTransactionParseResult CreatePaymentTransactionReadResult()
        {
            return new PaymentTransactionParseResult();
        }
    }
}
