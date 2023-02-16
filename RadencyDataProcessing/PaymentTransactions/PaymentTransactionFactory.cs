using RadencyDataProcessing.PaymentTransactions.Base;
using RadencyDataProcessing.PaymentTransactions.Models;

namespace RadencyDataProcessing.PaymentTransactions
{
    public class PaymentTransactionFactory : PaymentTransactionFactoryBase
    {
        private IServiceProvider _serviceProvider;
        public PaymentTransactionFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public override PaymentTransactionEntry CreatePaymentTransactionEntry()
        {
            return new PaymentTransactionEntry();
        }

        public override PaymentTransactionParseResult CreatePaymentTransactionReadResult()
        {
            return new PaymentTransactionParseResult();
        }

        public override PaymentTransactionsHandler CreatePaymentTransactionsHandler(string source)
        {
            PaymentTransactionsHandler handler = (PaymentTransactionsHandler)_serviceProvider.GetService(typeof(PaymentTransactionsHandler))!;
            handler.Source = source;
            return handler;
        }
    }
}
