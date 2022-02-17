namespace Knight.MysqlTest2.Logging
{
    /// <summary>
    /// Represents a logging object
    /// </summary>
    public static class Log
    {
        /// <summary>
        /// Logs an error with a default message
        /// </summary>
        public static void LogError()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("An error has occured!");
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        /// <summary>
        /// Logs an error with a specified message.
        /// </summary>
        /// <param name="message">Message to display</param>
        public static void LogError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        /// <summary>
        /// Logs an error with a specified exception's message.
        /// </summary>
        /// <param name="exception">The exception to display</param>
        public static void LogError(Exception exception)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(exception.Message);
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        /// <summary>
        /// Logs an error with a specified message and an exception's message.
        /// </summary>
        /// <param name="message">Message to display</param>
        /// <param name="exception">The exception to display</param>
        public static void LogError(string message, Exception exception)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{message}: {exception.Message}");
            Console.ForegroundColor = ConsoleColor.Gray;
        }



        /// <summary>
        /// Logs a successful operation with a default message
        /// </summary>
        public static void LogSuccess()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Operation succeeded");
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        /// <summary>
        /// Logs a successful operation with a specified message
        /// </summary>
        /// <param name="message">Message to display</param>
        public static void LogSuccess(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{message}");
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
}