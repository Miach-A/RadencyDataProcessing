using RadencyDataProcessing;
using RadencyDataProcessing.Extensions;
using RadencyDataProcessing.PaymentTransactions;
using RadencyDataProcessing.PaymentTransactions.Interfaces;

try
{
    IHost host = Host.CreateDefaultBuilder(args)

        .ConfigureServices((context, services) =>
        {
            services.Configure<PaymentTransactionsConfiguration>(context.Configuration.GetSection("PaymentTransactions"));

            services.PostConfigure<PaymentTransactionsConfiguration>(settings =>
            {
                var configErrors = settings.ValidationErrors().ToArray();
                if (configErrors.Any())
                {
                    var aggrErrors = string.Join(",", configErrors);
                    var count = configErrors.Length;
                    var configType = settings.GetType().Name;
                    throw new ApplicationException(
                        $"Found {count} configuration error(s) in {configType}: {aggrErrors}");
                }
            });

            services.AddSingleton(typeof(IPaymentTransactionFactory), typeof(PaymentTransactionFactory));
            services.AddSingleton(typeof(IPaymentTransactionManager), typeof(PaymentTransactionManager));
            services.AddSingleton(typeof(IPaymentTransactionProcessing), typeof(PaymentTransactionsProcessing));
            services.AddHostedService<Worker>();
        })
        .Build();

    host.Run();
}
catch (ApplicationException ex)
{
    Console.WriteLine(ex.Message);
}
