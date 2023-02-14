namespace RadencyDataProcessing.PaymentTransactions.Interfaces
{
    public interface IPaymentTransactionReadResult
    {
        public IEnumerable<IPaymentTransactionEntry> Entries { get; set; }
        public string ReadFilePath { get; set; }
        public IEnumerable<string> ErrorLines { get; set; }
        public bool Skip { get; set; }
    }
}
