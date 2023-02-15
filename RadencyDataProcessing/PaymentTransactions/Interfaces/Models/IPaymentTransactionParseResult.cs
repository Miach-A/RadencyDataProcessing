namespace RadencyDataProcessing.PaymentTransactions.Interfaces
{
    public interface IPaymentTransactionParseResult
    {
        public IEnumerable<IPaymentTransactionEntry> Entries { get; set; }
        public IEnumerable<string> ErrorLines { get; set; }
    }
}
