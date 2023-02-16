namespace RadencyDataProcessing.PaymentTransactions.Interfaces
{
    public interface IPaymentTransactionParseResult
    {
        public IEnumerable<IPaymentTransactionEntryBase> Entries { get; set; }
        public IEnumerable<string> ErrorLines { get; set; }
    }
}
