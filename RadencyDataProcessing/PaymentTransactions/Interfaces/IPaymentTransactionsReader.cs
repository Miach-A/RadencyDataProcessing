namespace RadencyDataProcessing.PaymentTransactions.Interfaces
{
    public interface IPaymentTransactionsReader
    {
        public Task<IEnumerable<IPaymentEntry>> Read(string path);
    }
}
