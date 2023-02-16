namespace RadencyDataProcessing.PaymentTransactions.Interfaces
{
    public interface IPaymentTransactionManager1<T>
    {
        string InnerDataDirectory { get; }
        string OutgoingDataDirectory { get; }
        IPaymentTransactionReader<T> Reader { get; }
        IPaymentTransactionHandler Handler { get; }
        IPaymentTransactionParser<T> Parser { get; }
    }
}
