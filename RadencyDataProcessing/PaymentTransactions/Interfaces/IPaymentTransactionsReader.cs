namespace RadencyDataProcessing.PaymentTransactions.Interfaces
{
    public interface IPaymentTransactionsReader
    {
        public Task<IPaymentTransactionReadResult> Read(string path);
    }
}
