using RadencyDataProcessing.Interfaces;

namespace RadencyDataProcessing
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IHostApplicationLifetime _hostApplicationLifetime;
        private readonly IProcessing _paymentTransactionsProcessing;

        public Worker(
            ILogger<Worker> logger,
            IHostApplicationLifetime hostApplicationLifetime,
            IProcessing paymentTransactionsProcessing)
        {
            _logger = logger;
            _hostApplicationLifetime = hostApplicationLifetime;
            _paymentTransactionsProcessing = paymentTransactionsProcessing;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                await Task.WhenAll(
                    _paymentTransactionsProcessing.Processing(stoppingToken)
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