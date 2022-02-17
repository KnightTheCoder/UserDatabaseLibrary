namespace Knight.MysqlTest2.Exceptions
{
    /// <summary>
    /// Occurs when a connection isn't made
    /// </summary>
    public class DatabaseConnectionFailedException: UserDatabaseException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseConnectionFailedException"></see> class with a default error message.
        /// </summary>
        public DatabaseConnectionFailedException()
        : base("Login failed")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseConnectionFailedException"></see> class with a specified error message.
        /// </summary>
        /// <param name="message"></param>
        public DatabaseConnectionFailedException(string message)
        : base (message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseConnectionFailedException"></see> class with specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public DatabaseConnectionFailedException(string message, Exception innerException) 
        : base(message, innerException)
        {
        }
    }
}