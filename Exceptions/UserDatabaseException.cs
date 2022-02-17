namespace Knight.MysqlTest2.Exceptions
{
    /// <summary>
    /// Base class for all user database exceptions
    /// </summary>
    public class UserDatabaseException: Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserDatabaseException"></see> class with a default error message.
        /// </summary>
        public UserDatabaseException() 
        : base("An error has occured with the userdatabase")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserDatabaseException"></see> class with a specified error message.
        /// </summary>
        /// <param name="message"></param>
        public UserDatabaseException(string message) 
        : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserDatabaseException"></see> class with specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public UserDatabaseException(string message, Exception innerException) 
        : base(message, innerException)
        {
        }

    }
}