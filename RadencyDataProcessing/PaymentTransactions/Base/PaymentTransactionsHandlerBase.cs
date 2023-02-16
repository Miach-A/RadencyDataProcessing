namespace RadencyDataProcessing.PaymentTransactions.Base
{
    public abstract class PaymentTransactionsHandlerBase
    {
        public string Source { get; set; } = string.Empty;
        public abstract Task SaveAsync();
    }
}
