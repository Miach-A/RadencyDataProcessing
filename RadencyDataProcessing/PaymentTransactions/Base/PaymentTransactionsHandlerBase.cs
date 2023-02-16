namespace RadencyDataProcessing.PaymentTransactions.Base
{
    public abstract class PaymentTransactionsHandlerBase
    {
        public PaymentTransactionsHandlerBase(string source)
        {
            Source = source;
        }
        public string Source { get; set; } = string.Empty;
        public abstract Task SaveAsync();
    }
}
