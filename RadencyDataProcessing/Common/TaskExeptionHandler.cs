namespace RadencyDataProcessing.Common
{
    public static class TaskExceptionHandler
    {
        public static void HandleExeption(Task task)
        {
            if (task.Exception != null)
            {
                LogException(task.Exception);
            }
        }

        public static void LogException(Exception exception)
        {
            Console.WriteLine(exception.Message);
            if (exception.StackTrace != null)
            {
                Console.WriteLine(exception.StackTrace);
            }

            if (exception is AggregateException aggregateException)
            {
                if (aggregateException.InnerExceptions.Count() == 0)
                {
                    return;
                }

                foreach (var innerTask in aggregateException.InnerExceptions)
                {
                    if (innerTask.StackTrace != null)
                    {
                        Console.WriteLine(innerTask.StackTrace);
                    }
                }
            }
        }
    }

}

