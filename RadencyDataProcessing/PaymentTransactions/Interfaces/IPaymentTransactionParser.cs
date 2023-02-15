namespace RadencyDataProcessing.PaymentTransactions.Interfaces
{
    public interface IPaymentTransactionParser<T>
    {
        IPaymentTransactionParseResult Parse(IAsyncEnumerable<T> transaction);
    }
}
