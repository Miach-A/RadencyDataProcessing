namespace RadencyDataProcessing.PaymentTransactions.Interfaces
{
    public interface IPaymentTransactionParser<T>
    {
        Task<IPaymentTransactionParseResult> ParseAsync(T transaction);
    }
}
