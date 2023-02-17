namespace RadencyDataProcessing.Interfaces
{
    public interface IProcessing
    {
        public Task Processing(CancellationToken stoppingToken);
    }
}
