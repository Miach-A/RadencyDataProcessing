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
        private readonly IPaymentTransactionHandler _paymentTransactionsHandler;

        public PaymentTransactionManager(
            IOptions<PaymentTransactionsConfiguration> PaymentTransactionsConfiguration,
            IPaymentTransactionFactory<IEnumerable<string>> paymentTransactionFactory)
        {
            _innerDataDirectory = PaymentTransactionsConfiguration.Value.InnerDataDirectory;
            _outgoingDataDirectory = PaymentTransactionsConfiguration.Value.OutgoingDataDirectory;
            _paymentTransactionFactory = paymentTransactionFactory;
            _paymentTransactionsReader = _paymentTransactionFactory.CreatePaymentTransactionsReader();
            _paymentTransactionsHandler = _paymentTransactionFactory.CreatePaymentTransactionsHandler();
        }
        public string InnerDataDirectory => _innerDataDirectory;

        public string OutgoingDataDirectory => _outgoingDataDirectory;

        public IPaymentTransactionHandler Handler => _paymentTransactionsHandler;

        public IPaymentTransactionReader<IEnumerable<string>> Reader => _paymentTransactionsReader;

        public IPaymentTransactionEntry NewEntry()
        {
            return _paymentTransactionFactory.CreatePaymentTransactionEntry();
        }
    }
}
