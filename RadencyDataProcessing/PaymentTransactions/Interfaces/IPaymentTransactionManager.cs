namespace RadencyDataProcessing.PaymentTransactions.Interfaces
{
    public interface IPaymentTransactionManager<T>
    {
        string InnerDataDirectory { get; }
        string OutgoingDataDirectory { get; }
        IPaymentTransactionReader<T> Reader { get; }
        IPaymentTransactionHandler Handler { get; }
        IPaymentTransactionEntry NewEntry();

    }
}
