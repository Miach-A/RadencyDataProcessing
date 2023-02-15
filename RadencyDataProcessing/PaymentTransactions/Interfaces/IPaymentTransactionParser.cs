namespace RadencyDataProcessing.PaymentTransactions.Interfaces
{
    public interface IPaymentTransactionParser<T>
    {
        IAsyncEnumerable<IPaymentTransactionParseResult> ParseAsync(IAsyncEnumerable<T> transaction);
    }
}
