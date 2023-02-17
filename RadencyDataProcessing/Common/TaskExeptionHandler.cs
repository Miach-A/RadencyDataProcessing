namespace RadencyDataProcessing.Common
{
    public class TaskExceptionHandler
    {
        private readonly ILogger<TaskExceptionHandler> _loger;
        public TaskExceptionHandler(
            ILogger<TaskExceptionHandler> logger)
        {
            _loger = logger;
        }
        public void HandleExeption(Task task)
        {
            if (task.Exception != null)
            {
                LogException(task.Exception);
            }
        }

        public void LogException(Exception exception)
        {
            _loger.LogError(exception.Message);
            if (exception.StackTrace != null)
            {
                _loger.LogError(exception.StackTrace);
            }

            if (exception is AggregateException aggregateException)
            {
                if (aggregateException.InnerExceptions.Count() == 0)
                {
                    return;
                }

                foreach (var innerException in aggregateException.InnerExceptions)
                {
                    _loger.LogError(innerException.Message);
                    if (innerException.StackTrace != null)
                    {
                        _loger.LogError(innerException.StackTrace);
                    }
                }
            }
        }
    }

}

