using RadencyDataProcessing.PaymentTransactions;

namespace RadencyDataProcessing
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IHostApplicationLifetime _hostApplicationLifetime;
        private readonly IConfiguration _configuration;
        private readonly PaymentTransactionsProcessing _paymentTransactionsProcessing;

        public Worker(
            ILogger<Worker> logger,
            IHostApplicationLifetime hostApplicationLifetime,
            IConfiguration configuration,
            PaymentTransactionsProcessing paymentTransactionsProcessing)
        {
            _logger = logger;
            _hostApplicationLifetime = hostApplicationLifetime;
            _configuration = configuration;
            _paymentTransactionsProcessing = paymentTransactionsProcessing;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                await Task.WhenAll(
                    _paymentTransactionsProcessing.ReadData(stoppingToken)
                    );
            }
            catch (Exception ex)
            {
                _logger.LogCritical("{exeption}{info}", "Unhandling exception. ", ex.Message);
            }
            finally
            {
                _hostApplicationLifetime.StopApplication();
            }
        }
    }
}