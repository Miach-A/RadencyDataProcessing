namespace RadencyDataProcessing.PaymentTransactions.Interfaces
{
    public interface IPaymentTransactionReader<T>
    {
        //public Task<IPaymentTransactionReadResult> ReadAsync(string path);
        public IAsyncEnumerable<T> ReadAsync(string path);
    }
}
