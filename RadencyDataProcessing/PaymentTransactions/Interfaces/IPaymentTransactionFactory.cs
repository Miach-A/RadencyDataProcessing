using RadencyDataProcessing.PaymentTransactions.Base;

namespace RadencyDataProcessing.PaymentTransactions.Interfaces
{
    public interface IPaymentTransactionFactory<TEntry, TParseResult>
        where TEntry : PaymentTransactionEntryBase, new()
        where TParseResult : PaymentTransactionParseResultBase, new()
    {
        public TEntry CreatePaymentTransactionEntry();

        public TParseResult CreatePaymentTransactionReadResult();
    }
}
