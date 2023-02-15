namespace RadencyDataProcessing.PaymentTransactions.Interfaces
{
    public interface IPaymentTransactionManager
    {
        string InnerDataDirectory { get; }
        string OutgoingDataDirectory { get; }
        IPaymentTransactionReader Reader { get; }
        IPaymentTransactionHandler Handler { get; }
        IPaymentTransactionEntry NewEntry();

    }
}
