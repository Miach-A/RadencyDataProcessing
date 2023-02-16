namespace RadencyDataProcessing.Common
{
    public static class TaskExceptionHandler
    {
        public static void HandleExeption(Task task)
        {
            if (task.Exception != null)
            {
                Console.WriteLine(task.Exception?.Message);
                if (task.Exception?.StackTrace != null)
                {
                    Console.WriteLine(task.Exception?.Message);
                }

                if (task.Exception?.InnerExceptions.Count() == 0)
                {
                    return;
                }

                foreach (var innerTask in task.Exception!.InnerExceptions)
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

