using Microsoft.Extensions.Options;

namespace RadencyDataProcessing.PaymentTransactions
{
    public class PaymentTransactionManager
    {
        private readonly string _innerDataDirectory;
        private readonly string _outgoingDataDirectory;
        private readonly PaymentTransactionFactory _paymentTransactionFactory;
        private readonly PaymentTransactionsReader _paymentTransactionsReader;
        private readonly PaymentTransactionParser _paymentTransactionsParser;
        private readonly PaymentTransactionsHandler _paymentTransactionsHandler;

        public PaymentTransactionManager(
            IOptions<PaymentTransactionsConfiguration> PaymentTransactionsConfiguration,
            PaymentTransactionFactory paymentTransactionFactory,
            PaymentTransactionsReader paymentTransactionReader,
            PaymentTransactionParser paymentTransactionParser,
            PaymentTransactionsHandler paymentTransactionHandler)
        {
            _innerDataDirectory = PaymentTransactionsConfiguration.Value.InnerDataDirectory;
            _outgoingDataDirectory = PaymentTransactionsConfiguration.Value.OutgoingDataDirectory;
            _paymentTransactionFactory = paymentTransactionFactory;
            _paymentTransactionsReader = paymentTransactionReader;
            _paymentTransactionsHandler = paymentTransactionHandler;
            _paymentTransactionsParser = paymentTransactionParser;
        }
        public string InnerDataDirectory => _innerDataDirectory;

        public string OutgoingDataDirectory => _outgoingDataDirectory;

        public PaymentTransactionsHandler Handler => _paymentTransactionsHandler;

        public PaymentTransactionsReader Reader => _paymentTransactionsReader;

        public PaymentTransactionParser Parser => _paymentTransactionsParser;
    }
}
