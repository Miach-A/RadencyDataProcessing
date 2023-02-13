using RadencyDataProcessing.PaymentTransactions;
using System.ComponentModel.DataAnnotations;

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
                if (!ValidateConfiguration())
                {
                    throw new ValidationException("Validation exception. ");
                }
                //while (!stoppingToken.IsCancellationRequested)
                //{
                await Task.WhenAll(
                    _paymentTransactionsProcessing.ReadData(stoppingToken)
                    );
                //}
            }
            catch (ValidationException ex)
            {
                _logger.LogCritical("{exeption}", ex.Message);
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

        private bool ValidateConfiguration()
        {
            bool ValidationPassed = true;
            if (_configuration.GetValue<string>("InnerDataPath") == null)
            {
                _logger.LogError("Set InnerDataPath in config file. ");
                ValidationPassed = false;
            }

            if (_configuration.GetValue<string>("OutgoingDataPath") == null)
            {
                _logger.LogError("Set OutgoingDataPath in config file. ");
                ValidationPassed = false;
            }

            return ValidationPassed;
        }
    }
}