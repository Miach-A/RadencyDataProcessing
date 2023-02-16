namespace RadencyDataProcessing.PaymentTransactions.Base
{
    //public abstract class PaymentTransactionParseResultBase<TParseResult> where TParseResult : PaymentTransactionEntryBase, new()
    //{
    //    public IEnumerable<TParseResult> Entries { get; set; } = new List<TParseResult>();
    //}
    public abstract class PaymentTransactionParseResultBase
    {
        public IEnumerable<PaymentTransactionEntryBase> Entries { get; set; } = new List<PaymentTransactionEntryBase>();
    }
}
