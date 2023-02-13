using RadencyDataProcessing;
using RadencyDataProcessing.PaymentTransactions;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddSingleton<PaymentTransactionsProcessing>();
        services.AddHostedService<Worker>();
    })
    .Build();

host.Run();
