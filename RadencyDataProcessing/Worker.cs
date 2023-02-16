using RadencyDataProcessing.Interfaces;

namespace RadencyDataProcessing
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IHostApplicationLifetime _hostApplicationLifetime;
        //private readonly IConfiguration _configuration;
        private readonly IProcessing _paymentTransactionsProcessing;

        public Worker(
            ILogger<Worker> logger,
            IHostApplicationLifetime hostApplicationLifetime,
            //IConfiguration configuration,
            IProcessing paymentTransactionsProcessing)
        {
            _logger = logger;
            _hostApplicationLifetime = hostApplicationLifetime;
            //_configuration = configuration;
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

        private int SecondsTillMidnight()
        {
            var now = DateTime.Now;
            var hours = 23 - now.Hour;
            var minutes = 59 - now.Minute;
            var seconds = 59 - now.Second;
            return hours * 3600 + minutes * 60 + seconds;
        }
    }
}