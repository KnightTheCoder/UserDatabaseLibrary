namespace Knight.UserDatabase.Exceptions
{
    /// <summary>
    /// Occurs when a query fails
    /// </summary>
    public class QueryFailedException: UserDatabaseException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QueryFailedException"></see> class with a default error message.
        /// </summary>
        public QueryFailedException()
        : base("Query failed")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryFailedException"></see> class with a specified error message.
        /// </summary>
        /// <param name="message"></param>
        public QueryFailedException(string message)
        : base (message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryFailedException"></see> class with specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public QueryFailedException(string message, Exception innerException) 
        : base(message, innerException)
        {
        }

    }
}