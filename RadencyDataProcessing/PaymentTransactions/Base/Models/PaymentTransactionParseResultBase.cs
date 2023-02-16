namespace RadencyDataProcessing.PaymentTransactions.Base
{
    public abstract class PaymentTransactionParseResultBase
    {
        public IEnumerable<PaymentTransactionEntryBase> Entries { get; set; } = new List<PaymentTransactionEntryBase>();
    }
}
