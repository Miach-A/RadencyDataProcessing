using RadencyDataProcessing;
using RadencyDataProcessing.PaymentTransactions;
using RadencyDataProcessing.PaymentTransactions.Interfaces;



IHost host = Host.CreateDefaultBuilder(args)

    .ConfigureServices((context, services) =>
    {
        services.Configure<PaymentTransactionsConfiguration>(context.Configuration.GetSection("PaymentTransactions"));
        services.AddSingleton(typeof(IPaymentTransactionsReader), typeof(PaymentTransactionsReader));
        services.AddSingleton<PaymentTransactionsProcessing>();
        services.AddHostedService<Worker>();
    })
    .Build();

host.Run();
