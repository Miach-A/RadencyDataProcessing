using RadencyDataProcessing;
using RadencyDataProcessing.Extensions;
using RadencyDataProcessing.Interfaces;
using RadencyDataProcessing.PaymentTransactions;

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

            services.AddSingleton<PaymentTransactionsReader>();
            services.AddSingleton<PaymentTransactionParser>();
            services.AddSingleton<PaymentTransactionFactory>();
            services.AddSingleton<PaymentTransactionManager>();
            services.AddSingleton(typeof(IProcessing), typeof(PaymentTransactionsProcessing));
            services.AddHostedService<Worker>();
        })
        .Build();

    host.Run();
}
catch (ApplicationException ex)
{
    Console.WriteLine(ex.Message);
}
