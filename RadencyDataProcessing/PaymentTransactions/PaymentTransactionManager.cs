using Microsoft.Extensions.Options;
using RadencyDataProcessing.PaymentTransactions.Interfaces;

namespace RadencyDataProcessing.PaymentTransactions
{
    public class PaymentTransactionManager : IPaymentTransactionManager
    {
        private readonly string _innerDataDirectory;
        private readonly string _outgoingDataDirectory;
        private readonly IPaymentTransactionFactory _paymentTransactionFactory;
        private readonly IPaymentTransactionReader<IEnumerable<string>> _paymentTransactionsReader;
        private readonly IPaymentTransactionHandler _paymentTransactionsHandler;

        public PaymentTransactionManager(
            IOptions<PaymentTransactionsConfiguration> PaymentTransactionsConfiguration,
            IPaymentTransactionFactory paymentTransactionFactory)
        {
            _innerDataDirectory = PaymentTransactionsConfiguration.Value.InnerDataDirectory;
            _outgoingDataDirectory = PaymentTransactionsConfiguration.Value.OutgoingDataDirectory;
            _paymentTransactionFactory = paymentTransactionFactory;
            _paymentTransactionsReader = _paymentTransactionFactory.CreatePaymentTransactionsReader();
            _paymentTransactionsHandler = _paymentTransactionFactory.CreatePaymentTransactionsHandler();
        }
        public string InnerDataDirectory => _innerDataDirectory;

        public string OutgoingDataDirectory => _outgoingDataDirectory;

        public IPaymentTransactionReader Reader => _paymentTransactionsReader;

        public IPaymentTransactionHandler Handler => _paymentTransactionsHandler;

        public IPaymentTransactionEntry NewEntry()
        {
            return _paymentTransactionFactory.CreatePaymentTransactionEntry();
        }
    }
}
