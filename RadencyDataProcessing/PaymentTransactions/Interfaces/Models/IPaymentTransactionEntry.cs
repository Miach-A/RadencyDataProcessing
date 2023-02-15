namespace RadencyDataProcessing.PaymentTransactions.Interfaces
{
    public interface IPaymentTransactionEntry
    {
        public bool SetData<T>(T data);
    }
}
