using RadencyDataProcessing;
using RadencyDataProcessing.Common;
using RadencyDataProcessing.Extensions;
using RadencyDataProcessing.Interfaces;
using RadencyDataProcessing.PaymentTransactions;
using Serilog;

try
{
    var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

    Log.Logger = new LoggerConfiguration()
        .ReadFrom.Configuration(configuration)
        .CreateLogger();

    IHost host = Host.CreateDefaultBuilder(args)
        .UseWindowsService()
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

            services.AddTransient<PaymentTransactionsHandler>();

            services.AddSingleton<TaskExceptionHandler>();
            services.AddSingleton<FileHandler>();
            services.AddSingleton<PaymentTransactionsReader>();
            services.AddSingleton<PaymentTransactionParser>();
            services.AddSingleton<PaymentTransactionFactory>();
            services.AddSingleton<PaymentTransactionManager>();
            services.AddSingleton(typeof(IProcessing), typeof(PaymentTransactionsProcessing));
            services.AddHostedService<Worker>();
        })
        .UseSerilog()
        .Build();

    host.Run();
}
catch (ApplicationException ex)
{
    Log.Fatal(ex.Message);
}
finally
{
    Log.CloseAndFlush();
}
