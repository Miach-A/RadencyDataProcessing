using RadencyDataProcessing;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddSingleton<PaymentTransactionsProcessing>();
        services.AddHostedService<Worker>();
    })
    .Build();

host.Run();
