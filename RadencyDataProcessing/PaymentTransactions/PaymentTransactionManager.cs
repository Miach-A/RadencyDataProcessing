using Microsoft.Extensions.Options;
using RadencyDataProcessing.PaymentTransactions.Interfaces;

namespace RadencyDataProcessing.PaymentTransactions
{
    public class PaymentTransactionManager : IPaymentTransactionManager<IEnumerable<string>>
    {
        private readonly string _innerDataDirectory;
        private readonly string _outgoingDataDirectory;
        private readonly IPaymentTransactionFactory<IEnumerable<string>> _paymentTransactionFactory;
        private readonly IPaymentTransactionReader<IEnumerable<string>> _paymentTransactionsReader;
        private readonly IPaymentTransactionParser<IEnumerable<string>> _paymentTransactionsParser;
        private readonly IPaymentTransactionHandler _paymentTransactionsHandler;

        public PaymentTransactionManager(
            IOptions<PaymentTransactionsConfiguration> PaymentTransactionsConfiguration,
            IPaymentTransactionFactory<IEnumerable<string>> paymentTransactionFactory,
            IPaymentTransactionReader<IEnumerable<string>> paymentTransactionReader,
             IPaymentTransactionParser<IEnumerable<string>> paymentTransactionParser,
              IPaymentTransactionHandler paymentTransactionHandler)
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

        public IPaymentTransactionHandler Handler => _paymentTransactionsHandler;

        public IPaymentTransactionReader<IEnumerable<string>> Reader => _paymentTransactionsReader;

        public IPaymentTransactionParser<IEnumerable<string>> Parser => _paymentTransactionsParser;

        public IPaymentTransactionFactory<IEnumerable<string>> Factory => _paymentTransactionFactory;
    }
}
