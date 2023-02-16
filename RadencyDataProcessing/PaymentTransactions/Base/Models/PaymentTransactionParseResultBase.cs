namespace RadencyDataProcessing.PaymentTransactions.Base
{
    public abstract class PaymentTransactionParseResultBase
    {
        public List<PaymentTransactionEntryBase> Entries { get; set; } = new List<PaymentTransactionEntryBase>();
    }
}
