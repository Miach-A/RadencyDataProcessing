namespace RadencyDataProcessing.PaymentTransactions.Interfaces
{
    public interface IPaymentTransactionsReader
    {
        public List<IPaymentEntry> Read();
    }
}
